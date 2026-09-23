# EXPANSION CW126-06 — The Key Left in Place

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_locked_room` — The Locked Room.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`.
**Thesis:** A compact threshold story that treats the key, drip, and clock as evidence of presence without deciding who or what is behind the door.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A compact threshold story that treats the key, drip, and clock as evidence of presence without deciding who or what is behind the door. The plan builds an optional scene bank around the exact local description “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

What does care require at a locked threshold when the key is available but the person on the other side is unknown?

## 3. Verified source record

The source record contains these exact fields: id: "enc_locked_room"; title: "The Locked Room"; description: "An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person."; category: "Mystery"; baseWeight: 1.5; stealthWeightMultiplier: 1.0; speedWeightMultiplier: 1.0; minDangerLevel: 0.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "open_it", "text": "Turn the key and open the door.", "moraleDelta": 2, "guiltDelta": 2}, {"choiceId": "knock_first", "text": "Pound on the steel door. Listen for breathing.", "moraleDelta": 3, "guiltDelta": 0}, {"choiceId": "leave_key", "text": "Leave the door and the key untouched.", "moraleDelta": 0, "guiltDelta": 0}, {"choiceId": "take_key", "text": "Pull the key from the lock and take it with you.", "moraleDelta": 1, "guiltDelta": 2}]. Source: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | `enc_locked_room` | title=The Locked Room; category=Mystery; description=An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `open_it` — “Turn the key and open the door.” (moraleDelta 2, guiltDelta 2)
- `knock_first` — “Pound on the steel door. Listen for breathing.” (moraleDelta 3, guiltDelta 0)
- `leave_key` — “Leave the door and the key untouched.” (moraleDelta 0, guiltDelta 0)
- `take_key` — “Pull the key from the lock and take it with you.” (moraleDelta 1, guiltDelta 2)

## 4. Fixed canon and open space

Do not reveal the room’s contents, identity, breathing, clock function, or water source. Do not provide lock manipulation, bypass, forced-entry, or rescue procedures. The source says “someone” left the key for the next person; preserve this as its narrative claim, not independently verified intent. Do not make the drip or ticking into a puzzle with a correct solution.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This record is present in the expansion JSON, but current NarrativeEncounterCatalogLoader loads narrative_encounters.json, narrative_encounters_npc_arcs.json, and micro_locations.json. ContentUtilizationScanner references are static mapping declarations, not proof that this expansion file is loaded. Treat every passage below as editorial and currently unverified for runtime reachability.

## 5. Human center

The source gives a security door locked from outside, a key in the cylinder, water and clock sounds, an oiled lock, and an interpretation that someone left the key for the next person. It does not identify the room or its occupant.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A traveler who speaks before acting:** Uses a knock as a social offer, not an audio test.
- **A companion who is wary of inherited instructions:** Asks who the key is for without deciding the answer.
- **A quiet witness who chooses to leave:** Treats non-entry as a complete decision rather than a failure state.

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

The content anchor is `enc_locked_room` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: The Door in the Interior

An interior security door gives a boundary without naming the building. The movement asks: Can a threshold feel particular without adding an address? Its source handle is “interior security door”: A barrier in an unknown interior, not a full layout. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: The Key Still There

The brass key remains in the cylinder. The movement asks: Does availability equal invitation? Its source handle is “locked from the outside”: A fact with troubling implications but no named captor. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: Water and Clock

Two sounds arrive through the door, neither of which identifies a person. The movement asks: How can sound create presence without becoming a clue system? Its source handle is “brass key in the cylinder”: An object present, not proof that entry is welcome. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: Oil on the Lock

The lock is described as heavily oiled. The movement asks: What should a physical detail imply—and what must it not prove? Its source handle is “slow drip of water”: A sound without identified source or quantity. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: A Note Without a Note

No written instruction is present; the source interprets the key’s placement. The movement asks: Can the author preserve that inference while leaving motive open? Its source handle is “rhythmic ticking of a mechanical clock”: A sound without a deadline or mechanism to solve. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: Turn, Knock, Leave

The listed choices permit action and restraint. The movement asks: How can each option retain dignity without forecasting an unseen outcome? Its source handle is “heavily oiled”: A condition of the lock, not instruction for operating it. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: The Door in the Interior × interior security door

**Beat question:** What can the writer say about “interior security door” during “The Door in the Interior” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 001 — interior security door — The Door in the Interior

For “The Door in the Interior” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “interior security door.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “The Door in the Interior” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “interior security door” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “interior security door” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — interior security door — The Door in the Interior

This proposed field-note fragment, beat 001 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “interior security door” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “The Door in the Interior” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “interior security door” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “interior security door” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — interior security door — The Door in the Interior

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “interior security door” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “interior security door” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “The Door in the Interior” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “interior security door” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “interior security door” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — interior security door — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “interior security door” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “The Door in the Interior” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “interior security door” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “interior security door” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: The Door in the Interior × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “The Door in the Interior” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 002 — locked from the outside — The Door in the Interior

For “The Door in the Interior” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “locked from the outside” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “The Door in the Interior” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “locked from the outside” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “locked from the outside” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — locked from the outside — The Door in the Interior

This proposed field-note fragment, beat 002 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “locked from the outside” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “The Door in the Interior” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “locked from the outside” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “locked from the outside” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — locked from the outside — The Door in the Interior

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “locked from the outside” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “locked from the outside” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “The Door in the Interior” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “locked from the outside” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “locked from the outside” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — locked from the outside — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “locked from the outside.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “The Door in the Interior” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “locked from the outside” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “locked from the outside” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: The Door in the Interior × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “The Door in the Interior” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 003 — brass key in the cylinder — The Door in the Interior

For “The Door in the Interior” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “brass key in the cylinder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “The Door in the Interior” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “brass key in the cylinder” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “brass key in the cylinder” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — brass key in the cylinder — The Door in the Interior

This proposed field-note fragment, beat 003 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “brass key in the cylinder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “The Door in the Interior” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “brass key in the cylinder” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “brass key in the cylinder” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — brass key in the cylinder — The Door in the Interior

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “brass key in the cylinder” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “brass key in the cylinder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “The Door in the Interior” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “brass key in the cylinder” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “brass key in the cylinder” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — brass key in the cylinder — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “brass key in the cylinder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “The Door in the Interior” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “brass key in the cylinder” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “brass key in the cylinder” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: The Door in the Interior × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “The Door in the Interior” while preserving this limit: a sound without identified source or quantity. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 004 — slow drip of water — The Door in the Interior

For “The Door in the Interior” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “slow drip of water” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “The Door in the Interior” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “slow drip of water” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “slow drip of water” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — slow drip of water — The Door in the Interior

This proposed field-note fragment, beat 004 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “slow drip of water” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “The Door in the Interior” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “slow drip of water” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “slow drip of water” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — slow drip of water — The Door in the Interior

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “slow drip of water” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “slow drip of water” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “The Door in the Interior” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “slow drip of water” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “slow drip of water” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — slow drip of water — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “slow drip of water” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “The Door in the Interior” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “slow drip of water” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “slow drip of water” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: The Door in the Interior × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “The Door in the Interior” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 005 — rhythmic ticking of a mechanical clock — The Door in the Interior

For “The Door in the Interior” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “rhythmic ticking of a mechanical clock” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “The Door in the Interior” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “rhythmic ticking of a mechanical clock” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — rhythmic ticking of a mechanical clock — The Door in the Interior

This proposed field-note fragment, beat 005 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “rhythmic ticking of a mechanical clock” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “The Door in the Interior” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — rhythmic ticking of a mechanical clock — The Door in the Interior

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “rhythmic ticking of a mechanical clock” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “rhythmic ticking of a mechanical clock.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “The Door in the Interior” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — rhythmic ticking of a mechanical clock — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “rhythmic ticking of a mechanical clock” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “The Door in the Interior” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: The Door in the Interior × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “The Door in the Interior” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 006 — heavily oiled — The Door in the Interior

For “The Door in the Interior” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “heavily oiled.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “The Door in the Interior” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “heavily oiled” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “heavily oiled” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — heavily oiled — The Door in the Interior

This proposed field-note fragment, beat 006 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “heavily oiled” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “The Door in the Interior” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “heavily oiled” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “heavily oiled” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — heavily oiled — The Door in the Interior

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “heavily oiled” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “heavily oiled” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “The Door in the Interior” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “heavily oiled” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “heavily oiled” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — heavily oiled — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “heavily oiled” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “The Door in the Interior” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “heavily oiled” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “heavily oiled” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: The Door in the Interior × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “The Door in the Interior” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 007 — someone locked this room — The Door in the Interior

For “The Door in the Interior” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “someone locked this room” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “The Door in the Interior” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “someone locked this room” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “someone locked this room” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — someone locked this room — The Door in the Interior

This proposed field-note fragment, beat 007 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “someone locked this room” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “The Door in the Interior” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “someone locked this room” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “someone locked this room” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — someone locked this room — The Door in the Interior

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “someone locked this room” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “someone locked this room” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “The Door in the Interior” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “someone locked this room” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “someone locked this room” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — someone locked this room — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “someone locked this room.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “The Door in the Interior” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “someone locked this room” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “someone locked this room” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: The Door in the Interior × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “The Door in the Interior” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: Can a threshold feel particular without adding an address?

#### Scene draft 008 — left the key for the next person — The Door in the Interior

For “The Door in the Interior” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “left the key for the next person” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “The Door in the Interior” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “The Door in the Interior” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “left the key for the next person” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “left the key for the next person” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — left the key for the next person — The Door in the Interior

This proposed field-note fragment, beat 008 in “The Door in the Interior,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “left the key for the next person.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “The Door in the Interior” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “The Door in the Interior.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “The Door in the Interior” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “left the key for the next person” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “left the key for the next person” during “The Door in the Interior” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — left the key for the next person — The Door in the Interior

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “left the key for the next person” during “The Door in the Interior,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “left the key for the next person” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “The Door in the Interior” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “The Door in the Interior.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “The Door in the Interior” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “left the key for the next person” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “left the key for the next person” during “The Door in the Interior” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — left the key for the next person — The Door in the Interior

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “The Door in the Interior” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “left the key for the next person” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “The Door in the Interior” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “The Door in the Interior.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “The Door in the Interior” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“Can a threshold feel particular without adding an address?”—as a revision test tied to “left the key for the next person” during “The Door in the Interior.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “left the key for the next person” during “The Door in the Interior” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Door in the Interior” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: The Key Still There × interior security door

**Beat question:** What can the writer say about “interior security door” during “The Key Still There” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: Does availability equal invitation?

#### Scene draft 009 — interior security door — The Key Still There

For “The Key Still There” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “interior security door” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “The Key Still There” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “interior security door” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “interior security door” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — interior security door — The Key Still There

This proposed field-note fragment, beat 009 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “interior security door” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “The Key Still There” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “interior security door” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “interior security door” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — interior security door — The Key Still There

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “interior security door” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “interior security door.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “The Key Still There” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “interior security door” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “interior security door” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — interior security door — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “interior security door” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “The Key Still There” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “interior security door” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “interior security door” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: The Key Still There × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “The Key Still There” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: Does availability equal invitation?

#### Scene draft 010 — locked from the outside — The Key Still There

For “The Key Still There” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “locked from the outside.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “The Key Still There” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “locked from the outside” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “locked from the outside” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — locked from the outside — The Key Still There

This proposed field-note fragment, beat 010 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “locked from the outside” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “The Key Still There” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “locked from the outside” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “locked from the outside” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — locked from the outside — The Key Still There

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “locked from the outside” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “locked from the outside” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “The Key Still There” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “locked from the outside” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “locked from the outside” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — locked from the outside — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “locked from the outside” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “The Key Still There” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “locked from the outside” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “locked from the outside” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: The Key Still There × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “The Key Still There” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: Does availability equal invitation?

#### Scene draft 011 — brass key in the cylinder — The Key Still There

For “The Key Still There” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “brass key in the cylinder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “The Key Still There” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “brass key in the cylinder” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “brass key in the cylinder” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — brass key in the cylinder — The Key Still There

This proposed field-note fragment, beat 011 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “brass key in the cylinder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “The Key Still There” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “brass key in the cylinder” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “brass key in the cylinder” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — brass key in the cylinder — The Key Still There

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “brass key in the cylinder” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “brass key in the cylinder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “The Key Still There” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “brass key in the cylinder” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “brass key in the cylinder” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — brass key in the cylinder — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “brass key in the cylinder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “The Key Still There” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “brass key in the cylinder” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “brass key in the cylinder” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: The Key Still There × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “The Key Still There” while preserving this limit: a sound without identified source or quantity. The larger movement question is: Does availability equal invitation?

#### Scene draft 012 — slow drip of water — The Key Still There

For “The Key Still There” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “slow drip of water” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “The Key Still There” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “slow drip of water” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “slow drip of water” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — slow drip of water — The Key Still There

This proposed field-note fragment, beat 012 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “slow drip of water.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “The Key Still There” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “slow drip of water” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “slow drip of water” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — slow drip of water — The Key Still There

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “slow drip of water” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “slow drip of water” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “The Key Still There” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “slow drip of water” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “slow drip of water” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — slow drip of water — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “slow drip of water” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “The Key Still There” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “slow drip of water” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “slow drip of water” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: The Key Still There × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “The Key Still There” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: Does availability equal invitation?

#### Scene draft 013 — rhythmic ticking of a mechanical clock — The Key Still There

For “The Key Still There” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “rhythmic ticking of a mechanical clock” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “The Key Still There” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “rhythmic ticking of a mechanical clock” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — rhythmic ticking of a mechanical clock — The Key Still There

This proposed field-note fragment, beat 013 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “rhythmic ticking of a mechanical clock” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “The Key Still There” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — rhythmic ticking of a mechanical clock — The Key Still There

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “rhythmic ticking of a mechanical clock” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “rhythmic ticking of a mechanical clock” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “The Key Still There” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — rhythmic ticking of a mechanical clock — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “rhythmic ticking of a mechanical clock” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “The Key Still There” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: The Key Still There × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “The Key Still There” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: Does availability equal invitation?

#### Scene draft 014 — heavily oiled — The Key Still There

For “The Key Still There” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “heavily oiled” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “The Key Still There” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “heavily oiled” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “heavily oiled” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — heavily oiled — The Key Still There

This proposed field-note fragment, beat 014 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “heavily oiled” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “The Key Still There” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “heavily oiled” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “heavily oiled” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — heavily oiled — The Key Still There

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “heavily oiled” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “heavily oiled.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “The Key Still There” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “heavily oiled” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “heavily oiled” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — heavily oiled — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “heavily oiled” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “The Key Still There” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “heavily oiled” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “heavily oiled” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: The Key Still There × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “The Key Still There” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: Does availability equal invitation?

#### Scene draft 015 — someone locked this room — The Key Still There

For “The Key Still There” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “someone locked this room.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “The Key Still There” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “someone locked this room” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “someone locked this room” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — someone locked this room — The Key Still There

This proposed field-note fragment, beat 015 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “someone locked this room” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “The Key Still There” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “someone locked this room” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “someone locked this room” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — someone locked this room — The Key Still There

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “someone locked this room” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “someone locked this room” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “The Key Still There” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “someone locked this room” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “someone locked this room” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — someone locked this room — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “someone locked this room” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “The Key Still There” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “someone locked this room” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “someone locked this room” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: The Key Still There × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “The Key Still There” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: Does availability equal invitation?

#### Scene draft 016 — left the key for the next person — The Key Still There

For “The Key Still There” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “left the key for the next person” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “The Key Still There” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “The Key Still There” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“Does availability equal invitation?”—as a revision test tied to “left the key for the next person” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “left the key for the next person” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — left the key for the next person — The Key Still There

This proposed field-note fragment, beat 016 in “The Key Still There,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “left the key for the next person” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “The Key Still There” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “The Key Still There.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “The Key Still There” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “left the key for the next person” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “left the key for the next person” during “The Key Still There” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — left the key for the next person — The Key Still There

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “left the key for the next person” during “The Key Still There,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “left the key for the next person” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “The Key Still There” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “The Key Still There.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “The Key Still There” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“Does availability equal invitation?”—as a revision test tied to “left the key for the next person” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “left the key for the next person” during “The Key Still There” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — left the key for the next person — The Key Still There

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “The Key Still There” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “left the key for the next person.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “The Key Still There” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “The Key Still There.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “The Key Still There” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“Does availability equal invitation?”—as a revision test tied to “left the key for the next person” during “The Key Still There.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “left the key for the next person” during “The Key Still There” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Key Still There” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: Water and Clock × interior security door

**Beat question:** What can the writer say about “interior security door” during “Water and Clock” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 017 — interior security door — Water and Clock

For “Water and Clock” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “interior security door” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “Water and Clock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “interior security door” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “interior security door” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — interior security door — Water and Clock

This proposed field-note fragment, beat 017 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “interior security door” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “Water and Clock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “interior security door” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “interior security door” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — interior security door — Water and Clock

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “interior security door” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “interior security door” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “Water and Clock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “interior security door” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “interior security door” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — interior security door — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “interior security door” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “Water and Clock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “interior security door” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “interior security door” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: Water and Clock × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “Water and Clock” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 018 — locked from the outside — Water and Clock

For “Water and Clock” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “locked from the outside” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “Water and Clock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “locked from the outside” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “locked from the outside” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — locked from the outside — Water and Clock

This proposed field-note fragment, beat 018 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “locked from the outside” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “Water and Clock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “locked from the outside” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “locked from the outside” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — locked from the outside — Water and Clock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “locked from the outside” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “locked from the outside.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “Water and Clock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “locked from the outside” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “locked from the outside” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — locked from the outside — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “locked from the outside” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “Water and Clock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “locked from the outside” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “locked from the outside” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: Water and Clock × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “Water and Clock” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 019 — brass key in the cylinder — Water and Clock

For “Water and Clock” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “brass key in the cylinder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “Water and Clock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “brass key in the cylinder” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “brass key in the cylinder” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — brass key in the cylinder — Water and Clock

This proposed field-note fragment, beat 019 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “brass key in the cylinder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “Water and Clock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “brass key in the cylinder” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “brass key in the cylinder” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — brass key in the cylinder — Water and Clock

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “brass key in the cylinder” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “brass key in the cylinder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “Water and Clock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “brass key in the cylinder” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “brass key in the cylinder” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — brass key in the cylinder — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “brass key in the cylinder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “Water and Clock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “brass key in the cylinder” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “brass key in the cylinder” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: Water and Clock × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “Water and Clock” while preserving this limit: a sound without identified source or quantity. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 020 — slow drip of water — Water and Clock

For “Water and Clock” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “slow drip of water” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “Water and Clock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “slow drip of water” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “slow drip of water” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — slow drip of water — Water and Clock

This proposed field-note fragment, beat 020 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “slow drip of water” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “Water and Clock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “slow drip of water” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “slow drip of water” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — slow drip of water — Water and Clock

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “slow drip of water” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “slow drip of water” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “Water and Clock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “slow drip of water” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “slow drip of water” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — slow drip of water — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “slow drip of water.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “Water and Clock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “slow drip of water” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “slow drip of water” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: Water and Clock × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “Water and Clock” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 021 — rhythmic ticking of a mechanical clock — Water and Clock

For “Water and Clock” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “rhythmic ticking of a mechanical clock” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “Water and Clock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “rhythmic ticking of a mechanical clock” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — rhythmic ticking of a mechanical clock — Water and Clock

This proposed field-note fragment, beat 021 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “rhythmic ticking of a mechanical clock.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “Water and Clock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — rhythmic ticking of a mechanical clock — Water and Clock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “rhythmic ticking of a mechanical clock” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “rhythmic ticking of a mechanical clock” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “Water and Clock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — rhythmic ticking of a mechanical clock — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “rhythmic ticking of a mechanical clock” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “Water and Clock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: Water and Clock × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “Water and Clock” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 022 — heavily oiled — Water and Clock

For “Water and Clock” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “heavily oiled” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “Water and Clock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “heavily oiled” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “heavily oiled” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — heavily oiled — Water and Clock

This proposed field-note fragment, beat 022 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “heavily oiled” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “Water and Clock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “heavily oiled” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “heavily oiled” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — heavily oiled — Water and Clock

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “heavily oiled” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “heavily oiled” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “Water and Clock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “heavily oiled” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “heavily oiled” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — heavily oiled — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “heavily oiled” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “Water and Clock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “heavily oiled” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “heavily oiled” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: Water and Clock × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “Water and Clock” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 023 — someone locked this room — Water and Clock

For “Water and Clock” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “someone locked this room” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “Water and Clock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “someone locked this room” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “someone locked this room” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — someone locked this room — Water and Clock

This proposed field-note fragment, beat 023 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “someone locked this room” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “Water and Clock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “someone locked this room” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “someone locked this room” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — someone locked this room — Water and Clock

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “someone locked this room” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “someone locked this room.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “Water and Clock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “someone locked this room” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “someone locked this room” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — someone locked this room — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “someone locked this room” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “Water and Clock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “someone locked this room” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “someone locked this room” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: Water and Clock × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “Water and Clock” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: How can sound create presence without becoming a clue system?

#### Scene draft 024 — left the key for the next person — Water and Clock

For “Water and Clock” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “left the key for the next person.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “Water and Clock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “Water and Clock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “left the key for the next person” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “left the key for the next person” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — left the key for the next person — Water and Clock

This proposed field-note fragment, beat 024 in “Water and Clock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “left the key for the next person” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “Water and Clock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “Water and Clock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “Water and Clock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “left the key for the next person” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “left the key for the next person” during “Water and Clock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — left the key for the next person — Water and Clock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “left the key for the next person” during “Water and Clock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “left the key for the next person” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “Water and Clock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “Water and Clock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “Water and Clock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “left the key for the next person” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “left the key for the next person” during “Water and Clock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — left the key for the next person — Water and Clock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “Water and Clock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “left the key for the next person” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “Water and Clock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “Water and Clock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “Water and Clock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“How can sound create presence without becoming a clue system?”—as a revision test tied to “left the key for the next person” during “Water and Clock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “left the key for the next person” during “Water and Clock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Water and Clock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: Oil on the Lock × interior security door

**Beat question:** What can the writer say about “interior security door” during “Oil on the Lock” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 025 — interior security door — Oil on the Lock

For “Oil on the Lock” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “interior security door” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “Oil on the Lock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “interior security door” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “interior security door” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — interior security door — Oil on the Lock

This proposed field-note fragment, beat 025 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “interior security door.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “Oil on the Lock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “interior security door” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “interior security door” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — interior security door — Oil on the Lock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “interior security door” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “interior security door” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “Oil on the Lock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “interior security door” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “interior security door” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — interior security door — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “interior security door” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “Oil on the Lock” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “interior security door” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “interior security door” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: Oil on the Lock × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “Oil on the Lock” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 026 — locked from the outside — Oil on the Lock

For “Oil on the Lock” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “locked from the outside” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “Oil on the Lock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “locked from the outside” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “locked from the outside” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — locked from the outside — Oil on the Lock

This proposed field-note fragment, beat 026 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “locked from the outside” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “Oil on the Lock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “locked from the outside” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “locked from the outside” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — locked from the outside — Oil on the Lock

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “locked from the outside” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “locked from the outside” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “Oil on the Lock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “locked from the outside” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “locked from the outside” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — locked from the outside — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “locked from the outside” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “Oil on the Lock” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “locked from the outside” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “locked from the outside” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: Oil on the Lock × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “Oil on the Lock” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 027 — brass key in the cylinder — Oil on the Lock

For “Oil on the Lock” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “brass key in the cylinder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “Oil on the Lock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “brass key in the cylinder” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “brass key in the cylinder” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — brass key in the cylinder — Oil on the Lock

This proposed field-note fragment, beat 027 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “brass key in the cylinder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “Oil on the Lock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “brass key in the cylinder” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “brass key in the cylinder” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — brass key in the cylinder — Oil on the Lock

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “brass key in the cylinder” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “brass key in the cylinder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “Oil on the Lock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “brass key in the cylinder” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “brass key in the cylinder” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — brass key in the cylinder — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “brass key in the cylinder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “Oil on the Lock” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “brass key in the cylinder” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “brass key in the cylinder” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: Oil on the Lock × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “Oil on the Lock” while preserving this limit: a sound without identified source or quantity. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 028 — slow drip of water — Oil on the Lock

For “Oil on the Lock” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “slow drip of water.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “Oil on the Lock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “slow drip of water” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “slow drip of water” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — slow drip of water — Oil on the Lock

This proposed field-note fragment, beat 028 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “slow drip of water” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “Oil on the Lock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “slow drip of water” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “slow drip of water” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — slow drip of water — Oil on the Lock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “slow drip of water” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “slow drip of water” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “Oil on the Lock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “slow drip of water” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “slow drip of water” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — slow drip of water — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “slow drip of water” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “Oil on the Lock” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “slow drip of water” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “slow drip of water” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: Oil on the Lock × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “Oil on the Lock” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 029 — rhythmic ticking of a mechanical clock — Oil on the Lock

For “Oil on the Lock” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “rhythmic ticking of a mechanical clock” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “Oil on the Lock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “rhythmic ticking of a mechanical clock” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — rhythmic ticking of a mechanical clock — Oil on the Lock

This proposed field-note fragment, beat 029 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “rhythmic ticking of a mechanical clock” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “Oil on the Lock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — rhythmic ticking of a mechanical clock — Oil on the Lock

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “rhythmic ticking of a mechanical clock” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “rhythmic ticking of a mechanical clock” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “Oil on the Lock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — rhythmic ticking of a mechanical clock — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “rhythmic ticking of a mechanical clock.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “Oil on the Lock” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: Oil on the Lock × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “Oil on the Lock” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 030 — heavily oiled — Oil on the Lock

For “Oil on the Lock” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “heavily oiled” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “Oil on the Lock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “heavily oiled” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “heavily oiled” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — heavily oiled — Oil on the Lock

This proposed field-note fragment, beat 030 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “heavily oiled.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “Oil on the Lock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “heavily oiled” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “heavily oiled” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — heavily oiled — Oil on the Lock

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “heavily oiled” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “heavily oiled” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “Oil on the Lock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “heavily oiled” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “heavily oiled” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — heavily oiled — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “heavily oiled” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “Oil on the Lock” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “heavily oiled” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “heavily oiled” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: Oil on the Lock × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “Oil on the Lock” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 031 — someone locked this room — Oil on the Lock

For “Oil on the Lock” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “someone locked this room” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “Oil on the Lock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “someone locked this room” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “someone locked this room” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — someone locked this room — Oil on the Lock

This proposed field-note fragment, beat 031 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “someone locked this room” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “Oil on the Lock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “someone locked this room” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “someone locked this room” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — someone locked this room — Oil on the Lock

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “someone locked this room” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “someone locked this room” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “Oil on the Lock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “someone locked this room” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “someone locked this room” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — someone locked this room — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “someone locked this room” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “Oil on the Lock” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “someone locked this room” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “someone locked this room” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: Oil on the Lock × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “Oil on the Lock” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: What should a physical detail imply—and what must it not prove?

#### Scene draft 032 — left the key for the next person — Oil on the Lock

For “Oil on the Lock” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “left the key for the next person” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “Oil on the Lock” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “Oil on the Lock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “left the key for the next person” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “left the key for the next person” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — left the key for the next person — Oil on the Lock

This proposed field-note fragment, beat 032 in “Oil on the Lock,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “left the key for the next person” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “Oil on the Lock” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “Oil on the Lock.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “Oil on the Lock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “left the key for the next person” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “left the key for the next person” during “Oil on the Lock” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — left the key for the next person — Oil on the Lock

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “left the key for the next person” during “Oil on the Lock,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “left the key for the next person.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “Oil on the Lock” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “Oil on the Lock.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “Oil on the Lock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “left the key for the next person” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “left the key for the next person” during “Oil on the Lock” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — left the key for the next person — Oil on the Lock

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “Oil on the Lock” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “left the key for the next person” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “Oil on the Lock” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “Oil on the Lock.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “Oil on the Lock” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“What should a physical detail imply—and what must it not prove?”—as a revision test tied to “left the key for the next person” during “Oil on the Lock.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “left the key for the next person” during “Oil on the Lock” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Oil on the Lock” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: A Note Without a Note × interior security door

**Beat question:** What can the writer say about “interior security door” during “A Note Without a Note” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 033 — interior security door — A Note Without a Note

For “A Note Without a Note” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “interior security door” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “A Note Without a Note” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “interior security door” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “interior security door” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — interior security door — A Note Without a Note

This proposed field-note fragment, beat 033 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “interior security door” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “A Note Without a Note” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “interior security door” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “interior security door” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — interior security door — A Note Without a Note

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “interior security door” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “interior security door” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “A Note Without a Note” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “interior security door” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “interior security door” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — interior security door — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “interior security door.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “A Note Without a Note” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “interior security door” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “interior security door” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: A Note Without a Note × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “A Note Without a Note” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 034 — locked from the outside — A Note Without a Note

For “A Note Without a Note” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “locked from the outside” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “A Note Without a Note” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “locked from the outside” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “locked from the outside” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — locked from the outside — A Note Without a Note

This proposed field-note fragment, beat 034 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “locked from the outside.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “A Note Without a Note” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “locked from the outside” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “locked from the outside” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — locked from the outside — A Note Without a Note

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “locked from the outside” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “locked from the outside” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “A Note Without a Note” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “locked from the outside” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “locked from the outside” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — locked from the outside — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “locked from the outside” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “A Note Without a Note” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “locked from the outside” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “locked from the outside” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: A Note Without a Note × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “A Note Without a Note” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 035 — brass key in the cylinder — A Note Without a Note

For “A Note Without a Note” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “brass key in the cylinder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “A Note Without a Note” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “brass key in the cylinder” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “brass key in the cylinder” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — brass key in the cylinder — A Note Without a Note

This proposed field-note fragment, beat 035 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “brass key in the cylinder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “A Note Without a Note” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “brass key in the cylinder” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “brass key in the cylinder” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — brass key in the cylinder — A Note Without a Note

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “brass key in the cylinder” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “brass key in the cylinder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “A Note Without a Note” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “brass key in the cylinder” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “brass key in the cylinder” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — brass key in the cylinder — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “brass key in the cylinder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “A Note Without a Note” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “brass key in the cylinder” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “brass key in the cylinder” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: A Note Without a Note × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “A Note Without a Note” while preserving this limit: a sound without identified source or quantity. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 036 — slow drip of water — A Note Without a Note

For “A Note Without a Note” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “slow drip of water” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “A Note Without a Note” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “slow drip of water” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “slow drip of water” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — slow drip of water — A Note Without a Note

This proposed field-note fragment, beat 036 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “slow drip of water” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “A Note Without a Note” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “slow drip of water” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “slow drip of water” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — slow drip of water — A Note Without a Note

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “slow drip of water” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “slow drip of water.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “A Note Without a Note” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “slow drip of water” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “slow drip of water” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — slow drip of water — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “slow drip of water” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “A Note Without a Note” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “slow drip of water” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “slow drip of water” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: A Note Without a Note × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “A Note Without a Note” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 037 — rhythmic ticking of a mechanical clock — A Note Without a Note

For “A Note Without a Note” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “rhythmic ticking of a mechanical clock.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “A Note Without a Note” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “rhythmic ticking of a mechanical clock” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — rhythmic ticking of a mechanical clock — A Note Without a Note

This proposed field-note fragment, beat 037 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “rhythmic ticking of a mechanical clock” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “A Note Without a Note” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — rhythmic ticking of a mechanical clock — A Note Without a Note

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “rhythmic ticking of a mechanical clock” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “rhythmic ticking of a mechanical clock” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “A Note Without a Note” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — rhythmic ticking of a mechanical clock — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “rhythmic ticking of a mechanical clock” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “A Note Without a Note” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: A Note Without a Note × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “A Note Without a Note” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 038 — heavily oiled — A Note Without a Note

For “A Note Without a Note” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “heavily oiled” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “A Note Without a Note” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “heavily oiled” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “heavily oiled” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — heavily oiled — A Note Without a Note

This proposed field-note fragment, beat 038 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “heavily oiled” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “A Note Without a Note” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “heavily oiled” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “heavily oiled” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — heavily oiled — A Note Without a Note

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “heavily oiled” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “heavily oiled” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “A Note Without a Note” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “heavily oiled” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “heavily oiled” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — heavily oiled — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “heavily oiled.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “A Note Without a Note” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “heavily oiled” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “heavily oiled” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: A Note Without a Note × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “A Note Without a Note” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 039 — someone locked this room — A Note Without a Note

For “A Note Without a Note” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “someone locked this room” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “A Note Without a Note” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “someone locked this room” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “someone locked this room” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — someone locked this room — A Note Without a Note

This proposed field-note fragment, beat 039 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “someone locked this room.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “A Note Without a Note” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “someone locked this room” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “someone locked this room” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — someone locked this room — A Note Without a Note

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “someone locked this room” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “someone locked this room” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “A Note Without a Note” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “someone locked this room” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “someone locked this room” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — someone locked this room — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “someone locked this room” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “A Note Without a Note” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “someone locked this room” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “someone locked this room” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: A Note Without a Note × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “A Note Without a Note” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: Can the author preserve that inference while leaving motive open?

#### Scene draft 040 — left the key for the next person — A Note Without a Note

For “A Note Without a Note” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “left the key for the next person” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “A Note Without a Note” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “A Note Without a Note” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “left the key for the next person” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “left the key for the next person” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — left the key for the next person — A Note Without a Note

This proposed field-note fragment, beat 040 in “A Note Without a Note,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “left the key for the next person” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “A Note Without a Note” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “A Note Without a Note.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “A Note Without a Note” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “left the key for the next person” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “left the key for the next person” during “A Note Without a Note” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — left the key for the next person — A Note Without a Note

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “left the key for the next person” during “A Note Without a Note,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “left the key for the next person” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “A Note Without a Note” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “A Note Without a Note.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “A Note Without a Note” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “left the key for the next person” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “left the key for the next person” during “A Note Without a Note” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — left the key for the next person — A Note Without a Note

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “A Note Without a Note” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “left the key for the next person” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “A Note Without a Note” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “A Note Without a Note.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “A Note Without a Note” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“Can the author preserve that inference while leaving motive open?”—as a revision test tied to “left the key for the next person” during “A Note Without a Note.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “left the key for the next person” during “A Note Without a Note” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Note Without a Note” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: Turn, Knock, Leave × interior security door

**Beat question:** What can the writer say about “interior security door” during “Turn, Knock, Leave” while preserving this limit: a barrier in an unknown interior, not a full layout. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 041 — interior security door — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “interior security door,” the candidate passage attends to A barrier in an unknown interior, not a full layout. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “interior security door.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “Turn, Knock, Leave” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “interior security door” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “interior security door” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — interior security door — Turn, Knock, Leave

This proposed field-note fragment, beat 041 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “interior security door” is the point of return. A barrier in an unknown interior, not a full layout. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “interior security door” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A traveler who speaks before acting a distinct perspective on “interior security door” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “Turn, Knock, Leave” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “interior security door” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “interior security door” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — interior security door — Turn, Knock, Leave

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “interior security door” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “interior security door” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives A companion who is wary of inherited instructions a distinct perspective on “interior security door” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “Turn, Knock, Leave” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “interior security door” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “interior security door” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — interior security door — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “interior security door” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “interior security door” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “interior security door” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives A quiet witness who chooses to leave a distinct perspective on “interior security door” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “Turn, Knock, Leave” × “interior security door,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “interior security door” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “interior security door” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “interior security door.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: Turn, Knock, Leave × locked from the outside

**Beat question:** What can the writer say about “locked from the outside” during “Turn, Knock, Leave” while preserving this limit: a fact with troubling implications but no named captor. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 042 — locked from the outside — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “locked from the outside,” the candidate passage attends to A fact with troubling implications but no named captor. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “locked from the outside” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “Turn, Knock, Leave” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “locked from the outside” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “locked from the outside” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — locked from the outside — Turn, Knock, Leave

This proposed field-note fragment, beat 042 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “locked from the outside” is the point of return. A fact with troubling implications but no named captor. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “locked from the outside” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives A quiet witness who chooses to leave a distinct perspective on “locked from the outside” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “Turn, Knock, Leave” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “locked from the outside” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “locked from the outside” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — locked from the outside — Turn, Knock, Leave

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “locked from the outside” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “locked from the outside” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A traveler who speaks before acting a distinct perspective on “locked from the outside” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “Turn, Knock, Leave” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “locked from the outside” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “locked from the outside” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — locked from the outside — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “locked from the outside” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “locked from the outside.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “locked from the outside” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives A companion who is wary of inherited instructions a distinct perspective on “locked from the outside” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “Turn, Knock, Leave” × “locked from the outside,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “locked from the outside” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “locked from the outside” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “locked from the outside.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: Turn, Knock, Leave × brass key in the cylinder

**Beat question:** What can the writer say about “brass key in the cylinder” during “Turn, Knock, Leave” while preserving this limit: an object present, not proof that entry is welcome. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 043 — brass key in the cylinder — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “brass key in the cylinder,” the candidate passage attends to An object present, not proof that entry is welcome. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “brass key in the cylinder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “Turn, Knock, Leave” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “brass key in the cylinder” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “brass key in the cylinder” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — brass key in the cylinder — Turn, Knock, Leave

This proposed field-note fragment, beat 043 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “brass key in the cylinder” is the point of return. An object present, not proof that entry is welcome. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “brass key in the cylinder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives A companion who is wary of inherited instructions a distinct perspective on “brass key in the cylinder” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “Turn, Knock, Leave” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “brass key in the cylinder” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “brass key in the cylinder” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — brass key in the cylinder — Turn, Knock, Leave

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “brass key in the cylinder” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “brass key in the cylinder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives A quiet witness who chooses to leave a distinct perspective on “brass key in the cylinder” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “Turn, Knock, Leave” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “brass key in the cylinder” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “brass key in the cylinder” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — brass key in the cylinder — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “brass key in the cylinder” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “brass key in the cylinder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “brass key in the cylinder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A traveler who speaks before acting a distinct perspective on “brass key in the cylinder” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “Turn, Knock, Leave” × “brass key in the cylinder,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “brass key in the cylinder” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “brass key in the cylinder” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “brass key in the cylinder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: Turn, Knock, Leave × slow drip of water

**Beat question:** What can the writer say about “slow drip of water” during “Turn, Knock, Leave” while preserving this limit: a sound without identified source or quantity. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 044 — slow drip of water — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “slow drip of water,” the candidate passage attends to A sound without identified source or quantity. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “slow drip of water” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “Turn, Knock, Leave” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “slow drip of water” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “slow drip of water” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — slow drip of water — Turn, Knock, Leave

This proposed field-note fragment, beat 044 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “slow drip of water” is the point of return. A sound without identified source or quantity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “slow drip of water” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A traveler who speaks before acting a distinct perspective on “slow drip of water” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “Turn, Knock, Leave” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “slow drip of water” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “slow drip of water” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — slow drip of water — Turn, Knock, Leave

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “slow drip of water” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “slow drip of water” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives A companion who is wary of inherited instructions a distinct perspective on “slow drip of water” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “Turn, Knock, Leave” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “slow drip of water” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “slow drip of water” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — slow drip of water — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “slow drip of water” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “slow drip of water” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “slow drip of water” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives A quiet witness who chooses to leave a distinct perspective on “slow drip of water” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “Turn, Knock, Leave” × “slow drip of water,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “slow drip of water” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “slow drip of water” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “slow drip of water.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: Turn, Knock, Leave × rhythmic ticking of a mechanical clock

**Beat question:** What can the writer say about “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave” while preserving this limit: a sound without a deadline or mechanism to solve. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 045 — rhythmic ticking of a mechanical clock — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “rhythmic ticking of a mechanical clock,” the candidate passage attends to A sound without a deadline or mechanism to solve. The present action begins small: the traveler naming what is heard and omitting who made it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “rhythmic ticking of a mechanical clock” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “Turn, Knock, Leave” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — rhythmic ticking of a mechanical clock — Turn, Knock, Leave

This proposed field-note fragment, beat 045 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “rhythmic ticking of a mechanical clock” is the point of return. A sound without a deadline or mechanism to solve. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “rhythmic ticking of a mechanical clock” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives A quiet witness who chooses to leave a distinct perspective on “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “Turn, Knock, Leave” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — rhythmic ticking of a mechanical clock — Turn, Knock, Leave

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “rhythmic ticking of a mechanical clock.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A traveler who speaks before acting a distinct perspective on “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “Turn, Knock, Leave” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — rhythmic ticking of a mechanical clock — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “rhythmic ticking of a mechanical clock” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “rhythmic ticking of a mechanical clock” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “rhythmic ticking of a mechanical clock” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives A companion who is wary of inherited instructions a distinct perspective on “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “Turn, Knock, Leave” × “rhythmic ticking of a mechanical clock,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “rhythmic ticking of a mechanical clock” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “rhythmic ticking of a mechanical clock.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: Turn, Knock, Leave × heavily oiled

**Beat question:** What can the writer say about “heavily oiled” during “Turn, Knock, Leave” while preserving this limit: a condition of the lock, not instruction for operating it. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 046 — heavily oiled — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “heavily oiled,” the candidate passage attends to A condition of the lock, not instruction for operating it. The present action begins small: a knock followed by a pause too short to become a test. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “heavily oiled.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “Turn, Knock, Leave” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “heavily oiled” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “heavily oiled” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — heavily oiled — Turn, Knock, Leave

This proposed field-note fragment, beat 046 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “heavily oiled” is the point of return. A condition of the lock, not instruction for operating it. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “heavily oiled” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives A companion who is wary of inherited instructions a distinct perspective on “heavily oiled” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “Turn, Knock, Leave” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “heavily oiled” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “heavily oiled” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — heavily oiled — Turn, Knock, Leave

The proposed exchange gives a quiet witness who chooses to leave a distinct reason to speak. Its authoring note is: “Treats non-entry as a complete decision rather than a failure state.” The talk concerns “heavily oiled” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “heavily oiled” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives A quiet witness who chooses to leave a distinct perspective on “heavily oiled” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “Turn, Knock, Leave” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The key is there. That still leaves us a choice.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “heavily oiled” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “heavily oiled” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — heavily oiled — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “heavily oiled” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “heavily oiled” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “heavily oiled” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A traveler who speaks before acting a distinct perspective on “heavily oiled” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “Turn, Knock, Leave” × “heavily oiled,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “heavily oiled” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “heavily oiled” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “heavily oiled.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: Turn, Knock, Leave × someone locked this room

**Beat question:** What can the writer say about “someone locked this room” during “Turn, Knock, Leave” while preserving this limit: the source’s conclusion with its unnamed subject retained. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 047 — someone locked this room — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “someone locked this room,” the candidate passage attends to The source’s conclusion with its unnamed subject retained. The present action begins small: the key held in frame while no hand reaches for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “someone locked this room” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “Turn, Knock, Leave” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “someone locked this room” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “someone locked this room” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — someone locked this room — Turn, Knock, Leave

This proposed field-note fragment, beat 047 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “someone locked this room” is the point of return. The source’s conclusion with its unnamed subject retained. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “someone locked this room” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A traveler who speaks before acting a distinct perspective on “someone locked this room” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “Turn, Knock, Leave” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “someone locked this room” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “someone locked this room” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — someone locked this room — Turn, Knock, Leave

The proposed exchange gives a companion who is wary of inherited instructions a distinct reason to speak. Its authoring note is: “Asks who the key is for without deciding the answer.” The talk concerns “someone locked this room” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “someone locked this room” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives A companion who is wary of inherited instructions a distinct perspective on “someone locked this room” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “Turn, Knock, Leave” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I hear water and a clock. I do not know who is here.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “someone locked this room” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “someone locked this room” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — someone locked this room — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “someone locked this room” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “someone locked this room.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “someone locked this room” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives A quiet witness who chooses to leave a distinct perspective on “someone locked this room” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “Turn, Knock, Leave” × “someone locked this room,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “someone locked this room” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “someone locked this room” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “someone locked this room.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: Turn, Knock, Leave × left the key for the next person

**Beat question:** What can the writer say about “left the key for the next person” during “Turn, Knock, Leave” while preserving this limit: a possible intention that remains unconfirmed by a note or witness. The larger movement question is: How can each option retain dignity without forecasting an unseen outcome?

#### Scene draft 048 — left the key for the next person — Turn, Knock, Leave

For “Turn, Knock, Leave” and the source phrase “left the key for the next person,” the candidate passage attends to A possible intention that remains unconfirmed by a note or witness. The present action begins small: the door remaining closed at the end of the selected passage. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “left the key for the next person” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “Turn, Knock, Leave” scene draft, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “Turn, Knock, Leave” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “left the key for the next person” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “left the key for the next person” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — left the key for the next person — Turn, Knock, Leave

This proposed field-note fragment, beat 048 in “Turn, Knock, Leave,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “left the key for the next person” is the point of return. A possible intention that remains unconfirmed by a note or witness. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “left the key for the next person.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “Turn, Knock, Leave” field-note fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives A quiet witness who chooses to leave a distinct perspective on “left the key for the next person” during “Turn, Knock, Leave.” The optional authoring note is: “Treats non-entry as a complete decision rather than a failure state.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “Turn, Knock, Leave” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “We can leave the door as we found it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “left the key for the next person” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “left the key for the next person” during “Turn, Knock, Leave” in a changed register: “I hear water and a clock. I do not know who is here.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — left the key for the next person — Turn, Knock, Leave

The proposed exchange gives a traveler who speaks before acting a distinct reason to speak. Its authoring note is: “Uses a knock as a social offer, not an audio test.” The talk concerns “left the key for the next person” during “Turn, Knock, Leave,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “left the key for the next person” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “Turn, Knock, Leave” conversation fragment, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A traveler who speaks before acting a distinct perspective on “left the key for the next person” during “Turn, Knock, Leave.” The optional authoring note is: “Uses a knock as a social offer, not an audio test.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “Turn, Knock, Leave” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “I hear water and a clock. I do not know who is here.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “We can leave the door as we found it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “left the key for the next person” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “left the key for the next person” during “Turn, Knock, Leave” in a changed register: “The key is there. That still leaves us a choice.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — left the key for the next person — Turn, Knock, Leave

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “left the key for the next person” through “Turn, Knock, Leave” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “left the key for the next person” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “Turn, Knock, Leave” conditional return vignette, is narrow. The local description says: “An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “left the key for the next person” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives A companion who is wary of inherited instructions a distinct perspective on “left the key for the next person” during “Turn, Knock, Leave.” The optional authoring note is: “Asks who the key is for without deciding the answer.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “Turn, Knock, Leave” × “left the key for the next person,” a possible line, offered as newly authored dialogue rather than canon, is: “The key is there. That still leaves us a choice.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“How can each option retain dignity without forecasting an unseen outcome?”—as a revision test tied to “left the key for the next person” during “Turn, Knock, Leave.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “left the key for the next person” during “Turn, Knock, Leave” in a changed register: “We can leave the door as we found it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Turn, Knock, Leave” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “left the key for the next person.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_locked_room`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

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

The proposal is local to `enc_locked_room` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_locked_room` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | Catalog presence does not by itself show where prose is presented. |
| Description | An interior security door, locked from the outside. The brass key is still in the cylinder. From inside, you hear the slow drip of water and the loud, rhythmic ticking of a mechanical clock. The lock cylinder is heavily oiled. Someone locked this room and left the key for the next person. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_locked_room` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.