# EXPANSION CW126-07 — What the Ledger Cannot Guarantee

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_library_cache` — The Library Cache.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`.
**Thesis:** A shared-cache encounter about reciprocal practice, anonymity, and the difference between a written precedent and a promise that supplies will remain.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A shared-cache encounter about reciprocal practice, anonymity, and the difference between a written precedent and a promise that supplies will remain. The plan builds an optional scene bank around the exact local description “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

How can a ledger preserve a practice of taking and leaving without turning unknown travelers into a guaranteed supply chain?

## 3. Verified source record

The source record contains these exact fields: id: "enc_library_cache"; title: "The Library Cache"; description: "A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old."; category: "Discovery"; baseWeight: 2.0; stealthWeightMultiplier: 1.0; speedWeightMultiplier: 1.0; minDangerLevel: 0.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "use_and_replenish", "text": "Take the iodine, leave surplus ammunition, and sign the ledger.", "moraleDelta": 4, "guiltDelta": 0}, {"choiceId": "take_water", "text": "Take only the iodine. Sign the ledger.", "moraleDelta": 3, "guiltDelta": 1}, {"choiceId": "add_to_cache", "text": "Leave medical supplies in the cache. Take nothing.", "moraleDelta": 5, "guiltDelta": 0}, {"choiceId": "take_all", "text": "Strip the cache entirely. Leave the ledger blank.", "moraleDelta": 0, "guiltDelta": 5}]. Source: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | `enc_library_cache` | title=The Library Cache; category=Discovery; description=A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `use_and_replenish` — “Take the iodine, leave surplus ammunition, and sign the ledger.” (moraleDelta 4, guiltDelta 0)
- `take_water` — “Take only the iodine. Sign the ledger.” (moraleDelta 3, guiltDelta 1)
- `add_to_cache` — “Leave medical supplies in the cache. Take nothing.” (moraleDelta 5, guiltDelta 0)
- `take_all` — “Strip the cache entirely. Leave the ledger blank.” (moraleDelta 0, guiltDelta 5)

## 4. Fixed canon and open space

Do not invent ledger names, quantities, handwriting identities, a cause for the week-long gap, clinical uses, a functioning supply system, or proof that the last writer is alive. Do not give medical instructions about iodine or suggest that the blanket solves exposure. Preserve the practice as a description, not a new inventory or replenishment mechanic.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This record is present in the expansion JSON, but current NarrativeEncounterCatalogLoader loads narrative_encounters.json, narrative_encounters_npc_arcs.json, and micro_locations.json. ContentUtilizationScanner references are static mapping declarations, not proof that this expansion file is loaded. Treat every passage below as editorial and currently unverified for runtime reachability.

## 5. Human center

The library is stripped to studs; behind ruined encyclopedias is a cache of iodine, a thermal blanket, and a ledger. Travelers take what they need and leave what they can. Its last entry is a week old.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A traveler who has used shared stores:** Sees generosity and uncertainty in the same shelf.
- **A companion who distrusts blank records:** Wants to know whether a cache can be relied on.
- **An anonymous proposed ledger writer:** May leave a short line without claiming to be the last entry’s author.

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

The content anchor is `enc_library_cache` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: Stripped to the Studs

The library’s loss is visible before the cache is found. The movement asks: How can absence set the scale without a catalogue of destruction? Its source handle is “public library”: A civic space with no surviving service guarantee. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: Books as Cover

The ruined encyclopedias conceal a small remaining store. The movement asks: What does their placement permit us to see without explaining who hid it? Its source handle is “stripped to the studs”: A description of damage, not an invitation to enumerate losses. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: Three Things in the Cache

Iodine, a thermal blanket, and a ledger are named. The movement asks: How can prose keep the items specific without inventing quantities or uses? Its source handle is “ruined encyclopedias”: The stated concealment place, without a named volume or text. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: Taking What Is Needed

The ledger records an imperfect reciprocal practice. The movement asks: Can need be written without asking every traveler to prove deserving? Its source handle is “survivor cache”: A description of a cache, not proof of its permanence. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: Leaving What One Can

The practice allows different capacity at different moments. The movement asks: How do we avoid turning generosity into a quota? Its source handle is “iodine”: A named item only; no dose, use, or treatment is supplied. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: A Week Since the Last Entry

The gap is a gap, not a disappearance report. The movement asks: What does the reader feel when a record stops short? Its source handle is “thermal blanket”: A named item only; no performance guarantee is added. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: Stripped to the Studs × public library

**Beat question:** What can the writer say about “public library” during “Stripped to the Studs” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 001 — public library — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “public library.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives A companion who distrusts blank records a distinct perspective on “public library” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “Stripped to the Studs” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “public library” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “public library” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — public library — Stripped to the Studs

This proposed field-note fragment, beat 001 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “public library” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “Stripped to the Studs” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “public library” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “public library” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — public library — Stripped to the Studs

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “public library” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “public library” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A traveler who has used shared stores a distinct perspective on “public library” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “Stripped to the Studs” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “public library” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “public library” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — public library — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “public library” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives A companion who distrusts blank records a distinct perspective on “public library” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “Stripped to the Studs” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “public library” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “public library” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: Stripped to the Studs × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “Stripped to the Studs” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 002 — stripped to the studs — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “stripped to the studs” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “Stripped to the Studs” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “stripped to the studs” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “stripped to the studs” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — stripped to the studs — Stripped to the Studs

This proposed field-note fragment, beat 002 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “stripped to the studs” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “Stripped to the Studs” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “stripped to the studs” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “stripped to the studs” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — stripped to the studs — Stripped to the Studs

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “stripped to the studs” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “stripped to the studs” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “Stripped to the Studs” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “stripped to the studs” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “stripped to the studs” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — stripped to the studs — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “stripped to the studs.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “Stripped to the Studs” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “stripped to the studs” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “stripped to the studs” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: Stripped to the Studs × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “Stripped to the Studs” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 003 — ruined encyclopedias — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “ruined encyclopedias” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “Stripped to the Studs” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “ruined encyclopedias” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “ruined encyclopedias” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — ruined encyclopedias — Stripped to the Studs

This proposed field-note fragment, beat 003 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “ruined encyclopedias.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “Stripped to the Studs” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “ruined encyclopedias” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “ruined encyclopedias” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — ruined encyclopedias — Stripped to the Studs

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “ruined encyclopedias” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “ruined encyclopedias” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “Stripped to the Studs” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “ruined encyclopedias” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “ruined encyclopedias” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — ruined encyclopedias — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “ruined encyclopedias” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “Stripped to the Studs” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “ruined encyclopedias” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “ruined encyclopedias” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: Stripped to the Studs × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “Stripped to the Studs” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 004 — survivor cache — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “survivor cache” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “Stripped to the Studs” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “survivor cache” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “survivor cache” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — survivor cache — Stripped to the Studs

This proposed field-note fragment, beat 004 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “survivor cache” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “Stripped to the Studs” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “survivor cache” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “survivor cache” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — survivor cache — Stripped to the Studs

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “survivor cache” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “survivor cache” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “Stripped to the Studs” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “survivor cache” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “survivor cache” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — survivor cache — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “survivor cache” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “Stripped to the Studs” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “survivor cache” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “survivor cache” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: Stripped to the Studs × iodine

**Beat question:** What can the writer say about “iodine” during “Stripped to the Studs” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 005 — iodine — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “iodine” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “Stripped to the Studs” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “iodine” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “iodine” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — iodine — Stripped to the Studs

This proposed field-note fragment, beat 005 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “iodine” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “Stripped to the Studs” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “iodine” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “iodine” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — iodine — Stripped to the Studs

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “iodine” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “iodine.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “Stripped to the Studs” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “iodine” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “iodine” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — iodine — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “iodine” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “Stripped to the Studs” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “iodine” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “iodine” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: Stripped to the Studs × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “Stripped to the Studs” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 006 — thermal blanket — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “thermal blanket.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “Stripped to the Studs” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “thermal blanket” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “thermal blanket” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — thermal blanket — Stripped to the Studs

This proposed field-note fragment, beat 006 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “thermal blanket” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “Stripped to the Studs” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “thermal blanket” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “thermal blanket” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — thermal blanket — Stripped to the Studs

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “thermal blanket” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “thermal blanket” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “Stripped to the Studs” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “thermal blanket” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “thermal blanket” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — thermal blanket — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “thermal blanket” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “Stripped to the Studs” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “thermal blanket” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “thermal blanket” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: Stripped to the Studs × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “Stripped to the Studs” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 007 — taking what they need — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “taking what they need” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “Stripped to the Studs” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “taking what they need” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “taking what they need” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — taking what they need — Stripped to the Studs

This proposed field-note fragment, beat 007 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “taking what they need” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “Stripped to the Studs” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “taking what they need” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “taking what they need” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — taking what they need — Stripped to the Studs

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “taking what they need” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “taking what they need” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “Stripped to the Studs” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “taking what they need” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “taking what they need” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — taking what they need — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “taking what they need.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “Stripped to the Studs” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “taking what they need” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “taking what they need” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: Stripped to the Studs × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “Stripped to the Studs” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: How can absence set the scale without a catalogue of destruction?

#### Scene draft 008 — last entry is a week old — Stripped to the Studs

For “Stripped to the Studs” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “last entry is a week old” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “Stripped to the Studs” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “Stripped to the Studs” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “last entry is a week old” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “last entry is a week old” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — last entry is a week old — Stripped to the Studs

This proposed field-note fragment, beat 008 in “Stripped to the Studs,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “last entry is a week old.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “Stripped to the Studs” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Stripped to the Studs.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “Stripped to the Studs” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “last entry is a week old” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “last entry is a week old” during “Stripped to the Studs” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — last entry is a week old — Stripped to the Studs

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “last entry is a week old” during “Stripped to the Studs,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “last entry is a week old” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “Stripped to the Studs” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Stripped to the Studs.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “Stripped to the Studs” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “last entry is a week old” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “last entry is a week old” during “Stripped to the Studs” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — last entry is a week old — Stripped to the Studs

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “Stripped to the Studs” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “last entry is a week old” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “Stripped to the Studs” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Stripped to the Studs.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “Stripped to the Studs” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“How can absence set the scale without a catalogue of destruction?”—as a revision test tied to “last entry is a week old” during “Stripped to the Studs.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “last entry is a week old” during “Stripped to the Studs” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Stripped to the Studs” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: Books as Cover × public library

**Beat question:** What can the writer say about “public library” during “Books as Cover” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 009 — public library — Books as Cover

For “Books as Cover” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “public library” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A traveler who has used shared stores a distinct perspective on “public library” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “Books as Cover” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “public library” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “public library” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — public library — Books as Cover

This proposed field-note fragment, beat 009 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “public library” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives A companion who distrusts blank records a distinct perspective on “public library” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “Books as Cover” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “public library” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “public library” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — public library — Books as Cover

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “public library” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “public library.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “Books as Cover” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “public library” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “public library” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — public library — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “public library” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A traveler who has used shared stores a distinct perspective on “public library” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “Books as Cover” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “public library” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “public library” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: Books as Cover × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “Books as Cover” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 010 — stripped to the studs — Books as Cover

For “Books as Cover” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “stripped to the studs.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “Books as Cover” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “stripped to the studs” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “stripped to the studs” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — stripped to the studs — Books as Cover

This proposed field-note fragment, beat 010 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “stripped to the studs” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “Books as Cover” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “stripped to the studs” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “stripped to the studs” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — stripped to the studs — Books as Cover

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “stripped to the studs” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “stripped to the studs” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “Books as Cover” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “stripped to the studs” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “stripped to the studs” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — stripped to the studs — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “stripped to the studs” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “Books as Cover” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “stripped to the studs” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “stripped to the studs” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: Books as Cover × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “Books as Cover” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 011 — ruined encyclopedias — Books as Cover

For “Books as Cover” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “ruined encyclopedias” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “Books as Cover” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “ruined encyclopedias” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “ruined encyclopedias” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — ruined encyclopedias — Books as Cover

This proposed field-note fragment, beat 011 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “ruined encyclopedias” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “Books as Cover” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “ruined encyclopedias” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “ruined encyclopedias” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — ruined encyclopedias — Books as Cover

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “ruined encyclopedias” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “ruined encyclopedias” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “Books as Cover” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “ruined encyclopedias” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “ruined encyclopedias” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — ruined encyclopedias — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “ruined encyclopedias.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “Books as Cover” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “ruined encyclopedias” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “ruined encyclopedias” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: Books as Cover × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “Books as Cover” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 012 — survivor cache — Books as Cover

For “Books as Cover” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “survivor cache” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “Books as Cover” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “survivor cache” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “survivor cache” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — survivor cache — Books as Cover

This proposed field-note fragment, beat 012 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “survivor cache.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “Books as Cover” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “survivor cache” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “survivor cache” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — survivor cache — Books as Cover

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “survivor cache” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “survivor cache” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “Books as Cover” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “survivor cache” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “survivor cache” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — survivor cache — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “survivor cache” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “Books as Cover” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “survivor cache” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “survivor cache” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: Books as Cover × iodine

**Beat question:** What can the writer say about “iodine” during “Books as Cover” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 013 — iodine — Books as Cover

For “Books as Cover” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “iodine” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “Books as Cover” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “iodine” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “iodine” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — iodine — Books as Cover

This proposed field-note fragment, beat 013 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “iodine” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “Books as Cover” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “iodine” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “iodine” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — iodine — Books as Cover

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “iodine” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “iodine” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “Books as Cover” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “iodine” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “iodine” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — iodine — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “iodine” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “Books as Cover” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “iodine” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “iodine” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: Books as Cover × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “Books as Cover” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 014 — thermal blanket — Books as Cover

For “Books as Cover” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “thermal blanket” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “Books as Cover” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “thermal blanket” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “thermal blanket” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — thermal blanket — Books as Cover

This proposed field-note fragment, beat 014 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “thermal blanket” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “Books as Cover” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “thermal blanket” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “thermal blanket” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — thermal blanket — Books as Cover

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “thermal blanket” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “thermal blanket.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “Books as Cover” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “thermal blanket” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “thermal blanket” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — thermal blanket — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “thermal blanket” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “Books as Cover” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “thermal blanket” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “thermal blanket” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: Books as Cover × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “Books as Cover” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 015 — taking what they need — Books as Cover

For “Books as Cover” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “taking what they need.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “Books as Cover” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “taking what they need” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “taking what they need” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — taking what they need — Books as Cover

This proposed field-note fragment, beat 015 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “taking what they need” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “Books as Cover” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “taking what they need” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “taking what they need” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — taking what they need — Books as Cover

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “taking what they need” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “taking what they need” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “Books as Cover” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “taking what they need” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “taking what they need” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — taking what they need — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “taking what they need” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “Books as Cover” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “taking what they need” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “taking what they need” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: Books as Cover × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “Books as Cover” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: What does their placement permit us to see without explaining who hid it?

#### Scene draft 016 — last entry is a week old — Books as Cover

For “Books as Cover” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “last entry is a week old” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “Books as Cover” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “Books as Cover” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “last entry is a week old” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “last entry is a week old” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — last entry is a week old — Books as Cover

This proposed field-note fragment, beat 016 in “Books as Cover,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “last entry is a week old” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “Books as Cover” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Books as Cover.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “Books as Cover” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “last entry is a week old” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “last entry is a week old” during “Books as Cover” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — last entry is a week old — Books as Cover

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “last entry is a week old” during “Books as Cover,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “last entry is a week old” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “Books as Cover” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Books as Cover.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “Books as Cover” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “last entry is a week old” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “last entry is a week old” during “Books as Cover” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — last entry is a week old — Books as Cover

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “Books as Cover” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “last entry is a week old.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “Books as Cover” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Books as Cover.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “Books as Cover” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“What does their placement permit us to see without explaining who hid it?”—as a revision test tied to “last entry is a week old” during “Books as Cover.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “last entry is a week old” during “Books as Cover” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Books as Cover” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: Three Things in the Cache × public library

**Beat question:** What can the writer say about “public library” during “Three Things in the Cache” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 017 — public library — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “public library” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “Three Things in the Cache” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “public library” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “public library” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — public library — Three Things in the Cache

This proposed field-note fragment, beat 017 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “public library” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A traveler who has used shared stores a distinct perspective on “public library” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “Three Things in the Cache” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “public library” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “public library” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — public library — Three Things in the Cache

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “public library” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “public library” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives A companion who distrusts blank records a distinct perspective on “public library” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “Three Things in the Cache” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “public library” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “public library” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — public library — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “public library” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “Three Things in the Cache” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “public library” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “public library” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: Three Things in the Cache × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “Three Things in the Cache” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 018 — stripped to the studs — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “stripped to the studs” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “Three Things in the Cache” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “stripped to the studs” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “stripped to the studs” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — stripped to the studs — Three Things in the Cache

This proposed field-note fragment, beat 018 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “stripped to the studs” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “Three Things in the Cache” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “stripped to the studs” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “stripped to the studs” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — stripped to the studs — Three Things in the Cache

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “stripped to the studs” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “stripped to the studs.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “Three Things in the Cache” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “stripped to the studs” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “stripped to the studs” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — stripped to the studs — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “stripped to the studs” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “Three Things in the Cache” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “stripped to the studs” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “stripped to the studs” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: Three Things in the Cache × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “Three Things in the Cache” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 019 — ruined encyclopedias — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “ruined encyclopedias.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “Three Things in the Cache” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “ruined encyclopedias” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “ruined encyclopedias” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — ruined encyclopedias — Three Things in the Cache

This proposed field-note fragment, beat 019 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “ruined encyclopedias” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “Three Things in the Cache” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “ruined encyclopedias” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “ruined encyclopedias” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — ruined encyclopedias — Three Things in the Cache

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “ruined encyclopedias” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “ruined encyclopedias” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “Three Things in the Cache” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “ruined encyclopedias” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “ruined encyclopedias” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — ruined encyclopedias — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “ruined encyclopedias” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “Three Things in the Cache” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “ruined encyclopedias” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “ruined encyclopedias” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: Three Things in the Cache × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “Three Things in the Cache” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 020 — survivor cache — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “survivor cache” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “Three Things in the Cache” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “survivor cache” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “survivor cache” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — survivor cache — Three Things in the Cache

This proposed field-note fragment, beat 020 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “survivor cache” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “Three Things in the Cache” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “survivor cache” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “survivor cache” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — survivor cache — Three Things in the Cache

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “survivor cache” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “survivor cache” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “Three Things in the Cache” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “survivor cache” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “survivor cache” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — survivor cache — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “survivor cache.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “Three Things in the Cache” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “survivor cache” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “survivor cache” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: Three Things in the Cache × iodine

**Beat question:** What can the writer say about “iodine” during “Three Things in the Cache” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 021 — iodine — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “iodine” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “Three Things in the Cache” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “iodine” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “iodine” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — iodine — Three Things in the Cache

This proposed field-note fragment, beat 021 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “iodine.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “Three Things in the Cache” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “iodine” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “iodine” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — iodine — Three Things in the Cache

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “iodine” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “iodine” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “Three Things in the Cache” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “iodine” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “iodine” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — iodine — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “iodine” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “Three Things in the Cache” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “iodine” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “iodine” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: Three Things in the Cache × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “Three Things in the Cache” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 022 — thermal blanket — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “thermal blanket” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “Three Things in the Cache” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “thermal blanket” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “thermal blanket” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — thermal blanket — Three Things in the Cache

This proposed field-note fragment, beat 022 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “thermal blanket” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “Three Things in the Cache” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “thermal blanket” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “thermal blanket” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — thermal blanket — Three Things in the Cache

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “thermal blanket” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “thermal blanket” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “Three Things in the Cache” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “thermal blanket” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “thermal blanket” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — thermal blanket — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “thermal blanket” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “Three Things in the Cache” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “thermal blanket” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “thermal blanket” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: Three Things in the Cache × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “Three Things in the Cache” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 023 — taking what they need — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “taking what they need” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “Three Things in the Cache” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “taking what they need” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “taking what they need” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — taking what they need — Three Things in the Cache

This proposed field-note fragment, beat 023 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “taking what they need” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “Three Things in the Cache” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “taking what they need” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “taking what they need” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — taking what they need — Three Things in the Cache

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “taking what they need” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “taking what they need.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “Three Things in the Cache” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “taking what they need” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “taking what they need” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — taking what they need — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “taking what they need” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “Three Things in the Cache” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “taking what they need” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “taking what they need” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: Three Things in the Cache × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “Three Things in the Cache” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: How can prose keep the items specific without inventing quantities or uses?

#### Scene draft 024 — last entry is a week old — Three Things in the Cache

For “Three Things in the Cache” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “last entry is a week old.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “Three Things in the Cache” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “Three Things in the Cache” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “last entry is a week old” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “last entry is a week old” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — last entry is a week old — Three Things in the Cache

This proposed field-note fragment, beat 024 in “Three Things in the Cache,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “last entry is a week old” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “Three Things in the Cache” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Three Things in the Cache.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “Three Things in the Cache” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “last entry is a week old” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “last entry is a week old” during “Three Things in the Cache” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — last entry is a week old — Three Things in the Cache

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “last entry is a week old” during “Three Things in the Cache,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “last entry is a week old” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “Three Things in the Cache” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Three Things in the Cache.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “Three Things in the Cache” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “last entry is a week old” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “last entry is a week old” during “Three Things in the Cache” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — last entry is a week old — Three Things in the Cache

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “Three Things in the Cache” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “last entry is a week old” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “Three Things in the Cache” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Three Things in the Cache.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “Three Things in the Cache” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“How can prose keep the items specific without inventing quantities or uses?”—as a revision test tied to “last entry is a week old” during “Three Things in the Cache.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “last entry is a week old” during “Three Things in the Cache” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Three Things in the Cache” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: Taking What Is Needed × public library

**Beat question:** What can the writer say about “public library” during “Taking What Is Needed” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 025 — public library — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “public library” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives A companion who distrusts blank records a distinct perspective on “public library” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “Taking What Is Needed” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “public library” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “public library” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — public library — Taking What Is Needed

This proposed field-note fragment, beat 025 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “public library.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “Taking What Is Needed” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “public library” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “public library” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — public library — Taking What Is Needed

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “public library” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “public library” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A traveler who has used shared stores a distinct perspective on “public library” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “Taking What Is Needed” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “public library” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “public library” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — public library — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “public library” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives A companion who distrusts blank records a distinct perspective on “public library” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “Taking What Is Needed” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “public library” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “public library” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: Taking What Is Needed × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “Taking What Is Needed” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 026 — stripped to the studs — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “stripped to the studs” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “Taking What Is Needed” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “stripped to the studs” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “stripped to the studs” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — stripped to the studs — Taking What Is Needed

This proposed field-note fragment, beat 026 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “stripped to the studs” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “Taking What Is Needed” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “stripped to the studs” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “stripped to the studs” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — stripped to the studs — Taking What Is Needed

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “stripped to the studs” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “stripped to the studs” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “Taking What Is Needed” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “stripped to the studs” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “stripped to the studs” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — stripped to the studs — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “stripped to the studs” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “Taking What Is Needed” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “stripped to the studs” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “stripped to the studs” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: Taking What Is Needed × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “Taking What Is Needed” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 027 — ruined encyclopedias — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “ruined encyclopedias” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “Taking What Is Needed” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “ruined encyclopedias” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “ruined encyclopedias” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — ruined encyclopedias — Taking What Is Needed

This proposed field-note fragment, beat 027 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “ruined encyclopedias” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “Taking What Is Needed” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “ruined encyclopedias” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “ruined encyclopedias” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — ruined encyclopedias — Taking What Is Needed

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “ruined encyclopedias” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “ruined encyclopedias.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “Taking What Is Needed” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “ruined encyclopedias” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “ruined encyclopedias” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — ruined encyclopedias — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “ruined encyclopedias” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “Taking What Is Needed” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “ruined encyclopedias” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “ruined encyclopedias” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: Taking What Is Needed × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “Taking What Is Needed” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 028 — survivor cache — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “survivor cache.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “Taking What Is Needed” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “survivor cache” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “survivor cache” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — survivor cache — Taking What Is Needed

This proposed field-note fragment, beat 028 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “survivor cache” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “Taking What Is Needed” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “survivor cache” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “survivor cache” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — survivor cache — Taking What Is Needed

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “survivor cache” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “survivor cache” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “Taking What Is Needed” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “survivor cache” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “survivor cache” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — survivor cache — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “survivor cache” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “Taking What Is Needed” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “survivor cache” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “survivor cache” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: Taking What Is Needed × iodine

**Beat question:** What can the writer say about “iodine” during “Taking What Is Needed” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 029 — iodine — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “iodine” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “Taking What Is Needed” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “iodine” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “iodine” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — iodine — Taking What Is Needed

This proposed field-note fragment, beat 029 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “iodine” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “Taking What Is Needed” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “iodine” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “iodine” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — iodine — Taking What Is Needed

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “iodine” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “iodine” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “Taking What Is Needed” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “iodine” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “iodine” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — iodine — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “iodine.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “Taking What Is Needed” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “iodine” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “iodine” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: Taking What Is Needed × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “Taking What Is Needed” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 030 — thermal blanket — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “thermal blanket” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “Taking What Is Needed” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “thermal blanket” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “thermal blanket” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — thermal blanket — Taking What Is Needed

This proposed field-note fragment, beat 030 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “thermal blanket.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “Taking What Is Needed” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “thermal blanket” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “thermal blanket” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — thermal blanket — Taking What Is Needed

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “thermal blanket” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “thermal blanket” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “Taking What Is Needed” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “thermal blanket” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “thermal blanket” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — thermal blanket — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “thermal blanket” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “Taking What Is Needed” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “thermal blanket” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “thermal blanket” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: Taking What Is Needed × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “Taking What Is Needed” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 031 — taking what they need — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “taking what they need” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “Taking What Is Needed” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “taking what they need” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “taking what they need” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — taking what they need — Taking What Is Needed

This proposed field-note fragment, beat 031 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “taking what they need” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “Taking What Is Needed” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “taking what they need” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “taking what they need” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — taking what they need — Taking What Is Needed

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “taking what they need” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “taking what they need” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “Taking What Is Needed” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “taking what they need” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “taking what they need” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — taking what they need — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “taking what they need” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “Taking What Is Needed” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “taking what they need” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “taking what they need” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: Taking What Is Needed × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “Taking What Is Needed” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: Can need be written without asking every traveler to prove deserving?

#### Scene draft 032 — last entry is a week old — Taking What Is Needed

For “Taking What Is Needed” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “last entry is a week old” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “Taking What Is Needed” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “Taking What Is Needed” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “last entry is a week old” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “last entry is a week old” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — last entry is a week old — Taking What Is Needed

This proposed field-note fragment, beat 032 in “Taking What Is Needed,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “last entry is a week old” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “Taking What Is Needed” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Taking What Is Needed.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “Taking What Is Needed” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “last entry is a week old” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “last entry is a week old” during “Taking What Is Needed” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — last entry is a week old — Taking What Is Needed

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “last entry is a week old” during “Taking What Is Needed,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “last entry is a week old.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “Taking What Is Needed” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Taking What Is Needed.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “Taking What Is Needed” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “last entry is a week old” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “last entry is a week old” during “Taking What Is Needed” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — last entry is a week old — Taking What Is Needed

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “Taking What Is Needed” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “last entry is a week old” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “Taking What Is Needed” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Taking What Is Needed.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “Taking What Is Needed” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“Can need be written without asking every traveler to prove deserving?”—as a revision test tied to “last entry is a week old” during “Taking What Is Needed.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “last entry is a week old” during “Taking What Is Needed” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Taking What Is Needed” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: Leaving What One Can × public library

**Beat question:** What can the writer say about “public library” during “Leaving What One Can” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 033 — public library — Leaving What One Can

For “Leaving What One Can” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “public library” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A traveler who has used shared stores a distinct perspective on “public library” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “Leaving What One Can” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “public library” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “public library” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — public library — Leaving What One Can

This proposed field-note fragment, beat 033 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “public library” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives A companion who distrusts blank records a distinct perspective on “public library” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “Leaving What One Can” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “public library” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “public library” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — public library — Leaving What One Can

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “public library” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “public library” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “Leaving What One Can” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “public library” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “public library” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — public library — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “public library.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A traveler who has used shared stores a distinct perspective on “public library” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “Leaving What One Can” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “public library” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “public library” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: Leaving What One Can × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “Leaving What One Can” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 034 — stripped to the studs — Leaving What One Can

For “Leaving What One Can” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “stripped to the studs” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “Leaving What One Can” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “stripped to the studs” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “stripped to the studs” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — stripped to the studs — Leaving What One Can

This proposed field-note fragment, beat 034 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “stripped to the studs.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “Leaving What One Can” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “stripped to the studs” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “stripped to the studs” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — stripped to the studs — Leaving What One Can

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “stripped to the studs” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “stripped to the studs” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “Leaving What One Can” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “stripped to the studs” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “stripped to the studs” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — stripped to the studs — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “stripped to the studs” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “Leaving What One Can” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “stripped to the studs” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “stripped to the studs” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: Leaving What One Can × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “Leaving What One Can” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 035 — ruined encyclopedias — Leaving What One Can

For “Leaving What One Can” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “ruined encyclopedias” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “Leaving What One Can” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “ruined encyclopedias” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “ruined encyclopedias” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — ruined encyclopedias — Leaving What One Can

This proposed field-note fragment, beat 035 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “ruined encyclopedias” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “Leaving What One Can” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “ruined encyclopedias” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “ruined encyclopedias” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — ruined encyclopedias — Leaving What One Can

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “ruined encyclopedias” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “ruined encyclopedias” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “Leaving What One Can” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “ruined encyclopedias” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “ruined encyclopedias” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — ruined encyclopedias — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “ruined encyclopedias” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “Leaving What One Can” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “ruined encyclopedias” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “ruined encyclopedias” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: Leaving What One Can × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “Leaving What One Can” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 036 — survivor cache — Leaving What One Can

For “Leaving What One Can” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “survivor cache” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “Leaving What One Can” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “survivor cache” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “survivor cache” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — survivor cache — Leaving What One Can

This proposed field-note fragment, beat 036 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “survivor cache” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “Leaving What One Can” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “survivor cache” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “survivor cache” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — survivor cache — Leaving What One Can

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “survivor cache” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “survivor cache.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “Leaving What One Can” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “survivor cache” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “survivor cache” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — survivor cache — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “survivor cache” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “Leaving What One Can” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “survivor cache” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “survivor cache” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: Leaving What One Can × iodine

**Beat question:** What can the writer say about “iodine” during “Leaving What One Can” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 037 — iodine — Leaving What One Can

For “Leaving What One Can” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “iodine.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “Leaving What One Can” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “iodine” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “iodine” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — iodine — Leaving What One Can

This proposed field-note fragment, beat 037 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “iodine” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A traveler who has used shared stores a distinct perspective on “iodine” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “Leaving What One Can” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “iodine” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “iodine” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — iodine — Leaving What One Can

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “iodine” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “iodine” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives A companion who distrusts blank records a distinct perspective on “iodine” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “Leaving What One Can” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “iodine” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “iodine” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — iodine — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “iodine” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “Leaving What One Can” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “iodine” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “iodine” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: Leaving What One Can × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “Leaving What One Can” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 038 — thermal blanket — Leaving What One Can

For “Leaving What One Can” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “thermal blanket” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “Leaving What One Can” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “thermal blanket” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “thermal blanket” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — thermal blanket — Leaving What One Can

This proposed field-note fragment, beat 038 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “thermal blanket” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “Leaving What One Can” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “thermal blanket” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “thermal blanket” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — thermal blanket — Leaving What One Can

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “thermal blanket” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “thermal blanket” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “Leaving What One Can” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “thermal blanket” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “thermal blanket” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — thermal blanket — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “thermal blanket.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “Leaving What One Can” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “thermal blanket” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “thermal blanket” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: Leaving What One Can × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “Leaving What One Can” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 039 — taking what they need — Leaving What One Can

For “Leaving What One Can” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “taking what they need” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “Leaving What One Can” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “taking what they need” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “taking what they need” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — taking what they need — Leaving What One Can

This proposed field-note fragment, beat 039 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “taking what they need.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “Leaving What One Can” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “taking what they need” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “taking what they need” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — taking what they need — Leaving What One Can

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “taking what they need” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “taking what they need” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “Leaving What One Can” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “taking what they need” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “taking what they need” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — taking what they need — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “taking what they need” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “Leaving What One Can” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “taking what they need” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “taking what they need” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: Leaving What One Can × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “Leaving What One Can” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: How do we avoid turning generosity into a quota?

#### Scene draft 040 — last entry is a week old — Leaving What One Can

For “Leaving What One Can” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “last entry is a week old” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “Leaving What One Can” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “Leaving What One Can” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “last entry is a week old” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “last entry is a week old” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — last entry is a week old — Leaving What One Can

This proposed field-note fragment, beat 040 in “Leaving What One Can,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “last entry is a week old” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “Leaving What One Can” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “Leaving What One Can.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “Leaving What One Can” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “last entry is a week old” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “last entry is a week old” during “Leaving What One Can” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — last entry is a week old — Leaving What One Can

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “last entry is a week old” during “Leaving What One Can,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “last entry is a week old” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “Leaving What One Can” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “Leaving What One Can.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “Leaving What One Can” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “last entry is a week old” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “last entry is a week old” during “Leaving What One Can” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — last entry is a week old — Leaving What One Can

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “Leaving What One Can” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “last entry is a week old” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “Leaving What One Can” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “Leaving What One Can.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “Leaving What One Can” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“How do we avoid turning generosity into a quota?”—as a revision test tied to “last entry is a week old” during “Leaving What One Can.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “last entry is a week old” during “Leaving What One Can” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving What One Can” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: A Week Since the Last Entry × public library

**Beat question:** What can the writer say about “public library” during “A Week Since the Last Entry” while preserving this limit: a civic space with no surviving service guarantee. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 041 — public library — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “public library,” the candidate passage attends to A civic space with no surviving service guarantee. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “public library.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “A Week Since the Last Entry” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “public library” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “public library” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — public library — A Week Since the Last Entry

This proposed field-note fragment, beat 041 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “public library” is the point of return. A civic space with no surviving service guarantee. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “public library” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A traveler who has used shared stores a distinct perspective on “public library” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “A Week Since the Last Entry” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “public library” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “public library” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — public library — A Week Since the Last Entry

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “public library” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “public library” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives A companion who distrusts blank records a distinct perspective on “public library” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “A Week Since the Last Entry” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “public library” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “public library” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — public library — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “public library” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “public library” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “public library” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives An anonymous proposed ledger writer a distinct perspective on “public library” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “A Week Since the Last Entry” × “public library,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “public library” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “public library” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “public library.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: A Week Since the Last Entry × stripped to the studs

**Beat question:** What can the writer say about “stripped to the studs” during “A Week Since the Last Entry” while preserving this limit: a description of damage, not an invitation to enumerate losses. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 042 — stripped to the studs — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “stripped to the studs,” the candidate passage attends to A description of damage, not an invitation to enumerate losses. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “stripped to the studs” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “A Week Since the Last Entry” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “stripped to the studs” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “stripped to the studs” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — stripped to the studs — A Week Since the Last Entry

This proposed field-note fragment, beat 042 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “stripped to the studs” is the point of return. A description of damage, not an invitation to enumerate losses. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “stripped to the studs” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives An anonymous proposed ledger writer a distinct perspective on “stripped to the studs” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “A Week Since the Last Entry” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “stripped to the studs” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “stripped to the studs” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — stripped to the studs — A Week Since the Last Entry

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “stripped to the studs” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “stripped to the studs” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A traveler who has used shared stores a distinct perspective on “stripped to the studs” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “A Week Since the Last Entry” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “stripped to the studs” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “stripped to the studs” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — stripped to the studs — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “stripped to the studs” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “stripped to the studs.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “stripped to the studs” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives A companion who distrusts blank records a distinct perspective on “stripped to the studs” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “A Week Since the Last Entry” × “stripped to the studs,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “stripped to the studs” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “stripped to the studs” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “stripped to the studs.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: A Week Since the Last Entry × ruined encyclopedias

**Beat question:** What can the writer say about “ruined encyclopedias” during “A Week Since the Last Entry” while preserving this limit: the stated concealment place, without a named volume or text. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 043 — ruined encyclopedias — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “ruined encyclopedias,” the candidate passage attends to The stated concealment place, without a named volume or text. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “ruined encyclopedias” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “A Week Since the Last Entry” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “ruined encyclopedias” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “ruined encyclopedias” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — ruined encyclopedias — A Week Since the Last Entry

This proposed field-note fragment, beat 043 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ruined encyclopedias” is the point of return. The stated concealment place, without a named volume or text. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “ruined encyclopedias.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives A companion who distrusts blank records a distinct perspective on “ruined encyclopedias” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “A Week Since the Last Entry” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “ruined encyclopedias” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “ruined encyclopedias” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — ruined encyclopedias — A Week Since the Last Entry

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “ruined encyclopedias” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “ruined encyclopedias” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives An anonymous proposed ledger writer a distinct perspective on “ruined encyclopedias” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “A Week Since the Last Entry” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “ruined encyclopedias” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “ruined encyclopedias” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — ruined encyclopedias — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ruined encyclopedias” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “ruined encyclopedias” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ruined encyclopedias” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A traveler who has used shared stores a distinct perspective on “ruined encyclopedias” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “A Week Since the Last Entry” × “ruined encyclopedias,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “ruined encyclopedias” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “ruined encyclopedias” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ruined encyclopedias.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: A Week Since the Last Entry × survivor cache

**Beat question:** What can the writer say about “survivor cache” during “A Week Since the Last Entry” while preserving this limit: a description of a cache, not proof of its permanence. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 044 — survivor cache — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “survivor cache,” the candidate passage attends to A description of a cache, not proof of its permanence. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “survivor cache” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “A Week Since the Last Entry” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “survivor cache” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “survivor cache” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — survivor cache — A Week Since the Last Entry

This proposed field-note fragment, beat 044 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “survivor cache” is the point of return. A description of a cache, not proof of its permanence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “survivor cache” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A traveler who has used shared stores a distinct perspective on “survivor cache” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “A Week Since the Last Entry” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “survivor cache” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “survivor cache” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — survivor cache — A Week Since the Last Entry

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “survivor cache” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “survivor cache” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives A companion who distrusts blank records a distinct perspective on “survivor cache” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “A Week Since the Last Entry” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “survivor cache” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “survivor cache” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — survivor cache — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “survivor cache” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “survivor cache” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “survivor cache” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives An anonymous proposed ledger writer a distinct perspective on “survivor cache” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “A Week Since the Last Entry” × “survivor cache,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “survivor cache” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “survivor cache” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “survivor cache.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: A Week Since the Last Entry × iodine

**Beat question:** What can the writer say about “iodine” during “A Week Since the Last Entry” while preserving this limit: a named item only; no dose, use, or treatment is supplied. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 045 — iodine — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “iodine,” the candidate passage attends to A named item only; no dose, use, or treatment is supplied. The present action begins small: a page edge worn where many hands could have turned it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “iodine” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives A companion who distrusts blank records a distinct perspective on “iodine” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “A Week Since the Last Entry” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “iodine” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “iodine” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — iodine — A Week Since the Last Entry

This proposed field-note fragment, beat 045 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “iodine” is the point of return. A named item only; no dose, use, or treatment is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “iodine” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives An anonymous proposed ledger writer a distinct perspective on “iodine” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “A Week Since the Last Entry” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “iodine” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “iodine” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — iodine — A Week Since the Last Entry

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “iodine” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “iodine.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A traveler who has used shared stores a distinct perspective on “iodine” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “A Week Since the Last Entry” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “iodine” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “iodine” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — iodine — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “iodine” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “iodine” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “iodine” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives A companion who distrusts blank records a distinct perspective on “iodine” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “A Week Since the Last Entry” × “iodine,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “iodine” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “iodine” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “iodine.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: A Week Since the Last Entry × thermal blanket

**Beat question:** What can the writer say about “thermal blanket” during “A Week Since the Last Entry” while preserving this limit: a named item only; no performance guarantee is added. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 046 — thermal blanket — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “thermal blanket,” the candidate passage attends to A named item only; no performance guarantee is added. The present action begins small: a ledger held apart from the three supplies. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “thermal blanket.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “A Week Since the Last Entry” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “thermal blanket” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “thermal blanket” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — thermal blanket — A Week Since the Last Entry

This proposed field-note fragment, beat 046 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thermal blanket” is the point of return. A named item only; no performance guarantee is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “thermal blanket” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives A companion who distrusts blank records a distinct perspective on “thermal blanket” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “A Week Since the Last Entry” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “thermal blanket” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “thermal blanket” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — thermal blanket — A Week Since the Last Entry

The proposed exchange gives an anonymous proposed ledger writer a distinct reason to speak. Its authoring note is: “May leave a short line without claiming to be the last entry’s author.” The talk concerns “thermal blanket” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “thermal blanket” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives An anonymous proposed ledger writer a distinct perspective on “thermal blanket” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “A Week Since the Last Entry” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “The last entry is a week old. It is not an answer about today.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “thermal blanket” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “thermal blanket” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — thermal blanket — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thermal blanket” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “thermal blanket” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thermal blanket” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A traveler who has used shared stores a distinct perspective on “thermal blanket” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “A Week Since the Last Entry” × “thermal blanket,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “thermal blanket” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “thermal blanket” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thermal blanket.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: A Week Since the Last Entry × taking what they need

**Beat question:** What can the writer say about “taking what they need” during “A Week Since the Last Entry” while preserving this limit: an account of practice, not a measure of need. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 047 — taking what they need — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “taking what they need,” the candidate passage attends to An account of practice, not a measure of need. The present action begins small: a blank line left available without demanding a name. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “taking what they need” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “A Week Since the Last Entry” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “taking what they need” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “taking what they need” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — taking what they need — A Week Since the Last Entry

This proposed field-note fragment, beat 047 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “taking what they need” is the point of return. An account of practice, not a measure of need. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “taking what they need” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A traveler who has used shared stores a distinct perspective on “taking what they need” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “A Week Since the Last Entry” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “taking what they need” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “taking what they need” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — taking what they need — A Week Since the Last Entry

The proposed exchange gives a companion who distrusts blank records a distinct reason to speak. Its authoring note is: “Wants to know whether a cache can be relied on.” The talk concerns “taking what they need” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “taking what they need” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives A companion who distrusts blank records a distinct perspective on “taking what they need” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “A Week Since the Last Entry” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write what you took only if the ledger asks for that much.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “taking what they need” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “taking what they need” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — taking what they need — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “taking what they need” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “taking what they need.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “taking what they need” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives An anonymous proposed ledger writer a distinct perspective on “taking what they need” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “A Week Since the Last Entry” × “taking what they need,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “taking what they need” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “taking what they need” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “taking what they need.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: A Week Since the Last Entry × last entry is a week old

**Beat question:** What can the writer say about “last entry is a week old” during “A Week Since the Last Entry” while preserving this limit: a time gap that does not reveal what happened afterward. The larger movement question is: What does the reader feel when a record stops short?

#### Scene draft 048 — last entry is a week old — A Week Since the Last Entry

For “A Week Since the Last Entry” and the source phrase “last entry is a week old,” the candidate passage attends to A time gap that does not reveal what happened afterward. The present action begins small: one proposed note that admits it cannot promise who comes next. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “last entry is a week old” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “A Week Since the Last Entry” scene draft, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “A Week Since the Last Entry” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “last entry is a week old” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “last entry is a week old” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — last entry is a week old — A Week Since the Last Entry

This proposed field-note fragment, beat 048 in “A Week Since the Last Entry,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “last entry is a week old” is the point of return. A time gap that does not reveal what happened afterward. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “last entry is a week old.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “A Week Since the Last Entry” field-note fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives An anonymous proposed ledger writer a distinct perspective on “last entry is a week old” during “A Week Since the Last Entry.” The optional authoring note is: “May leave a short line without claiming to be the last entry’s author.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “A Week Since the Last Entry” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Leave what you can. The sentence already makes room for less.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “last entry is a week old” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “last entry is a week old” during “A Week Since the Last Entry” in a changed register: “Write what you took only if the ledger asks for that much.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — last entry is a week old — A Week Since the Last Entry

The proposed exchange gives a traveler who has used shared stores a distinct reason to speak. Its authoring note is: “Sees generosity and uncertainty in the same shelf.” The talk concerns “last entry is a week old” during “A Week Since the Last Entry,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “last entry is a week old” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “A Week Since the Last Entry” conversation fragment, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A traveler who has used shared stores a distinct perspective on “last entry is a week old” during “A Week Since the Last Entry.” The optional authoring note is: “Sees generosity and uncertainty in the same shelf.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “A Week Since the Last Entry” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “Write what you took only if the ledger asks for that much.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Leave what you can. The sentence already makes room for less.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “last entry is a week old” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “last entry is a week old” during “A Week Since the Last Entry” in a changed register: “The last entry is a week old. It is not an answer about today.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — last entry is a week old — A Week Since the Last Entry

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “last entry is a week old” through “A Week Since the Last Entry” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “last entry is a week old” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “A Week Since the Last Entry” conditional return vignette, is narrow. The local description says: “A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “last entry is a week old” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives A companion who distrusts blank records a distinct perspective on “last entry is a week old” during “A Week Since the Last Entry.” The optional authoring note is: “Wants to know whether a cache can be relied on.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “A Week Since the Last Entry” × “last entry is a week old,” a possible line, offered as newly authored dialogue rather than canon, is: “The last entry is a week old. It is not an answer about today.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“What does the reader feel when a record stops short?”—as a revision test tied to “last entry is a week old” during “A Week Since the Last Entry.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “last entry is a week old” during “A Week Since the Last Entry” in a changed register: “Leave what you can. The sentence already makes room for less.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Week Since the Last Entry” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “last entry is a week old.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_library_cache`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

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

The proposal is local to `enc_library_cache` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_library_cache` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | Catalog presence does not by itself show where prose is presented. |
| Description | A public library, stripped to the studs. But behind a row of ruined encyclopedias, you find a survivor cache: iodine, a thermal blanket, and a ledger. The ledger is a log of travelers who have sheltered here, taking what they need and leaving what they can. The last entry is a week old. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_library_cache` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.