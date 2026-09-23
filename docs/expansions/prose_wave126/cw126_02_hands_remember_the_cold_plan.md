# EXPANSION CW126-02 — Hands Remember the Cold

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_pianist` — The Pianist.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters.json`.
**Thesis:** A quiet social encounter that makes room for a blind pianist’s practice without explaining it away or turning attention into a reward.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A quiet social encounter that makes room for a blind pianist’s practice without explaining it away or turning attention into a reward. The plan builds an optional scene bank around the exact local description “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

How can the player share a moment with the pianist while keeping his own purpose, history, and desired response his to disclose?

## 3. Verified source record

The source record contains these exact fields: id: "enc_pianist"; title: "The Pianist"; description: "An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold."; category: "Social"; baseWeight: 1.5; stealthWeightMultiplier: 0.5; speedWeightMultiplier: 1.5; minDangerLevel: 0.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "listen", "text": "Sit on the rubble and listen until he stops.", "moraleDelta": 4, "guiltDelta": 0}, {"choiceId": "share_food", "text": "Leave a portion of your rations on the piano bench.", "moraleDelta": 5, "guiltDelta": 0}, {"choiceId": "tell_about_bunker", "text": "Tell him the coordinates to your shelter.", "moraleDelta": 2, "guiltDelta": 1}, {"choiceId": "destroy_piano", "text": "Cut the piano strings with wire snips. Force him to move.", "moraleDelta": 0, "guiltDelta": 5}]. Source: `Assets/StreamingAssets/Data/narrative_encounters.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters.json` | `enc_pianist` | title=The Pianist; category=Social; description=An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `listen` — “Sit on the rubble and listen until he stops.” (moraleDelta 4, guiltDelta 0)
- `share_food` — “Leave a portion of your rations on the piano bench.” (moraleDelta 5, guiltDelta 0)
- `tell_about_bunker` — “Tell him the coordinates to your shelter.” (moraleDelta 2, guiltDelta 1)
- `destroy_piano` — “Cut the piano strings with wire snips. Force him to move.” (moraleDelta 0, guiltDelta 5)

## 4. Fixed canon and open space

Use only the canonical description from narrative_encounters.json as the live anchor. A separate expansion record reuses enc_pianist with a different concert-hall / three-note description; treat it as an unresolved duplicate and do not blend its “fourth note” into this plan. Do not invent the man’s name, repertoire, cause of blindness, home, feelings, or reason to keep playing. Do not frame blindness as a puzzle, inspiration device, or lack of agency. No medical treatment or instrument-repair instructions.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This is the loaded base-catalog record. The expansion catalog contains a different description under the same ID; this proposal does not merge or select that competing version.

## 5. Human center

The canonical encounter gives a roofless conservatory, an out-of-tune piano, blindness, muscle memory, and fingers stiff from cold. Those facts support presence and attention, not a diagnosis, song title, or hidden biography.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A listener who wants to help:** Offers presence, food, or information without assuming the pianist owes a response.
- **A companion who measures silence differently:** Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.
- **The pianist, only in optional authored dialogue:** May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.

All voices and dialogue are editorial unless the source explicitly quotes them. The encounter description is not a transcript. Use simple diction and differentiated attention: one person may describe a physical fact, another may qualify an inference, and a third may stop the conversation. Never attribute a proposed sentence to the source character without an authored decision and a clear speaker label.

## 7. Placement and current reachability

The canonical encounter is in narrative_encounters.json, the base file named by NarrativeEncounterCatalogLoader.FileName. NarrativeHostSession registers definitions from that loader. This supports a loaded-catalog claim, not a claim that any new passage here is already presented. The same ID also appears as a small demo definition in NarrativeHeadlessDemo.cs. A separate record in narrative_encounters_expansion.json reuses enc_pianist with incompatible descriptive details; before any content integration, the data owner must resolve provenance and avoid combining them. This prose plan uses only the base description and base choices.

No generated draft is present in production data. If a future content owner considers a line, verify the present schema and existing consumer first; do not create a parallel catalog, generic narrative panel, new route, registry, or save path as part of this prose plan. A source-file entry and a static utilization mapping are not runtime proof.

## 8. Player agency

The draft bank makes room for reading, asking, listening, declining, leaving, and silence. These are authoring postures, not promised controls or branches. The plan does not attach trust, morality, reputation, inventory, medical state, faction standing, relationship values, rewards, unlocks, or saved outcomes to a player’s interpretation. Existing source choices remain the complete choice list unless a separately authorized content decision changes them.

## 9. Continuity, dignity, and safety

**Boundary review.** Separate direct observation from inference in every line. The plan’s specific exclusions are listed in Section 4; carry those limits into voice, staging, and callback text without restating them as new setting facts.

Keep the distinction visible between a catalog fact, a character’s allegation, a traveler’s inference, and an editor’s optional flourish. Material detail should be quiet and specific. No real places, people, wars, hazardous procedures, medical recommendations, weapon techniques, or copied game text are introduced.

## 10. Existing owner and implementation boundary

The content anchor is `enc_pianist` in `Assets/StreamingAssets/Data/narrative_encounters.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: A Room Without a Roof

The conservatory is open to weather, while the encounter remains socially quiet. The movement asks: How can the scene locate us without decorating the ruin? Its source handle is “roofless conservatory”: The canonical setting, open to weather and acoustically unpromised. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: A Piano Out of Tune

The damaged sound is part of the present, not a clue to solve. The movement asks: What does careful listening notice without correcting the instrument? Its source handle is “warped upright piano”: A specific instrument with no repair history supplied. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: Hands Remember

The source names muscle memory and stiff fingers, leaving the history of practice unknown. The movement asks: How can skill appear without a flashback or explanation? Its source handle is “horribly out of tune”: A sound quality, not a code or an invitation to teach tuning. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: Attention Without a Look

He does not need to look toward the entrant for the scene to be mutual. The movement asks: What forms of acknowledgment are possible without demanding eye contact? Its source handle is “completely blind”: A fact that does not erase expertise, preference, or agency. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: An Offer, Not a Bargain

Existing choices include listening, food, shelter coordinates, or destroying the piano. The movement asks: How can optional prose preserve refusal and consequence without praising coercion? Its source handle is “muscle memory”: A practiced action, not proof of a particular past. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: When Playing Stops

The canonical choice says to listen until he stops; it does not say why or what follows. The movement asks: How can an ending honor a pause without making it a revelation? Its source handle is “fingers stiff from the cold”: A present bodily detail, not a diagnosis or prescribed treatment. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: A Room Without a Roof × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “A Room Without a Roof” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 001 — roofless conservatory — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “roofless conservatory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “A Room Without a Roof” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “roofless conservatory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “roofless conservatory” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — roofless conservatory — A Room Without a Roof

This proposed field-note fragment, beat 001 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “roofless conservatory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “A Room Without a Roof” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “roofless conservatory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “roofless conservatory” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — roofless conservatory — A Room Without a Roof

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “roofless conservatory” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “roofless conservatory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “A Room Without a Roof” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “roofless conservatory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “roofless conservatory” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — roofless conservatory — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “roofless conservatory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “A Room Without a Roof” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “roofless conservatory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “roofless conservatory” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: A Room Without a Roof × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “A Room Without a Roof” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 002 — warped upright piano — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “warped upright piano” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A listener who wants to help a distinct perspective on “warped upright piano” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “A Room Without a Roof” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “warped upright piano” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “warped upright piano” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — warped upright piano — A Room Without a Roof

This proposed field-note fragment, beat 002 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “warped upright piano” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “A Room Without a Roof” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “warped upright piano” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “warped upright piano” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — warped upright piano — A Room Without a Roof

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “warped upright piano” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “warped upright piano” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “A Room Without a Roof” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “warped upright piano” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “warped upright piano” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — warped upright piano — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “warped upright piano.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A listener who wants to help a distinct perspective on “warped upright piano” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “A Room Without a Roof” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “warped upright piano” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “warped upright piano” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: A Room Without a Roof × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “A Room Without a Roof” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 003 — horribly out of tune — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “horribly out of tune” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “A Room Without a Roof” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “horribly out of tune” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “horribly out of tune” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — horribly out of tune — A Room Without a Roof

This proposed field-note fragment, beat 003 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “horribly out of tune.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “A Room Without a Roof” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “horribly out of tune” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “horribly out of tune” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — horribly out of tune — A Room Without a Roof

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “horribly out of tune” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “horribly out of tune” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “A Room Without a Roof” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “horribly out of tune” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “horribly out of tune” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — horribly out of tune — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “horribly out of tune” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “A Room Without a Roof” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “horribly out of tune” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “horribly out of tune” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: A Room Without a Roof × completely blind

**Beat question:** What can the writer say about “completely blind” during “A Room Without a Roof” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 004 — completely blind — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “completely blind” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives A companion who measures silence differently a distinct perspective on “completely blind” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “A Room Without a Roof” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “completely blind” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “completely blind” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — completely blind — A Room Without a Roof

This proposed field-note fragment, beat 004 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “completely blind” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “A Room Without a Roof” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “completely blind” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “completely blind” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — completely blind — A Room Without a Roof

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “completely blind” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “completely blind” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A listener who wants to help a distinct perspective on “completely blind” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “A Room Without a Roof” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “completely blind” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “completely blind” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — completely blind — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “completely blind” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives A companion who measures silence differently a distinct perspective on “completely blind” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “A Room Without a Roof” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “completely blind” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “completely blind” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: A Room Without a Roof × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “A Room Without a Roof” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 005 — muscle memory — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “muscle memory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A listener who wants to help a distinct perspective on “muscle memory” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “A Room Without a Roof” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “muscle memory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “muscle memory” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — muscle memory — A Room Without a Roof

This proposed field-note fragment, beat 005 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “muscle memory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “A Room Without a Roof” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “muscle memory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “muscle memory” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — muscle memory — A Room Without a Roof

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “muscle memory” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “muscle memory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “A Room Without a Roof” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “muscle memory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “muscle memory” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — muscle memory — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “muscle memory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A listener who wants to help a distinct perspective on “muscle memory” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “A Room Without a Roof” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “muscle memory” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “muscle memory” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: A Room Without a Roof × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “A Room Without a Roof” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 006 — fingers stiff from the cold — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “fingers stiff from the cold.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “A Room Without a Roof” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “fingers stiff from the cold” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “fingers stiff from the cold” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — fingers stiff from the cold — A Room Without a Roof

This proposed field-note fragment, beat 006 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “fingers stiff from the cold” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “A Room Without a Roof” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “fingers stiff from the cold” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “fingers stiff from the cold” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — fingers stiff from the cold — A Room Without a Roof

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “fingers stiff from the cold” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “fingers stiff from the cold” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “A Room Without a Roof” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “fingers stiff from the cold” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “fingers stiff from the cold” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — fingers stiff from the cold — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “fingers stiff from the cold” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “A Room Without a Roof” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “fingers stiff from the cold” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “fingers stiff from the cold” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: A Room Without a Roof × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “A Room Without a Roof” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 007 — listen until he stops — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “listen until he stops” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “A Room Without a Roof” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “listen until he stops” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “listen until he stops” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — listen until he stops — A Room Without a Roof

This proposed field-note fragment, beat 007 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “listen until he stops” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “A Room Without a Roof” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “listen until he stops” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “listen until he stops” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — listen until he stops — A Room Without a Roof

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “listen until he stops” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “listen until he stops” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A listener who wants to help a distinct perspective on “listen until he stops” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “A Room Without a Roof” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “listen until he stops” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “listen until he stops” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — listen until he stops — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “listen until he stops.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “A Room Without a Roof” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “listen until he stops” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “listen until he stops” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: A Room Without a Roof × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “A Room Without a Roof” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: How can the scene locate us without decorating the ruin?

#### Scene draft 008 — coordinates to your shelter — A Room Without a Roof

For “A Room Without a Roof” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “coordinates to your shelter” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “A Room Without a Roof” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “A Room Without a Roof” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “coordinates to your shelter” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “coordinates to your shelter” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — coordinates to your shelter — A Room Without a Roof

This proposed field-note fragment, beat 008 in “A Room Without a Roof,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “coordinates to your shelter.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “A Room Without a Roof” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “A Room Without a Roof.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “A Room Without a Roof” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “coordinates to your shelter” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “coordinates to your shelter” during “A Room Without a Roof” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — coordinates to your shelter — A Room Without a Roof

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “coordinates to your shelter” during “A Room Without a Roof,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “coordinates to your shelter” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “A Room Without a Roof” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “A Room Without a Roof.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “A Room Without a Roof” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “coordinates to your shelter” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “coordinates to your shelter” during “A Room Without a Roof” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — coordinates to your shelter — A Room Without a Roof

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “A Room Without a Roof” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “coordinates to your shelter” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “A Room Without a Roof” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “A Room Without a Roof.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “A Room Without a Roof” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“How can the scene locate us without decorating the ruin?”—as a revision test tied to “coordinates to your shelter” during “A Room Without a Roof.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “coordinates to your shelter” during “A Room Without a Roof” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Room Without a Roof” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: A Piano Out of Tune × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “A Piano Out of Tune” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 009 — roofless conservatory — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “roofless conservatory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “A Piano Out of Tune” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “roofless conservatory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “roofless conservatory” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — roofless conservatory — A Piano Out of Tune

This proposed field-note fragment, beat 009 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “roofless conservatory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “A Piano Out of Tune” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “roofless conservatory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “roofless conservatory” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — roofless conservatory — A Piano Out of Tune

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “roofless conservatory” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “roofless conservatory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “A Piano Out of Tune” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “roofless conservatory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “roofless conservatory” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — roofless conservatory — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “roofless conservatory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “A Piano Out of Tune” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “roofless conservatory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “roofless conservatory” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: A Piano Out of Tune × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “A Piano Out of Tune” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 010 — warped upright piano — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “warped upright piano.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “A Piano Out of Tune” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “warped upright piano” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “warped upright piano” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — warped upright piano — A Piano Out of Tune

This proposed field-note fragment, beat 010 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “warped upright piano” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A listener who wants to help a distinct perspective on “warped upright piano” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “A Piano Out of Tune” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “warped upright piano” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “warped upright piano” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — warped upright piano — A Piano Out of Tune

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “warped upright piano” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “warped upright piano” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “A Piano Out of Tune” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “warped upright piano” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “warped upright piano” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — warped upright piano — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “warped upright piano” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “A Piano Out of Tune” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “warped upright piano” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “warped upright piano” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: A Piano Out of Tune × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “A Piano Out of Tune” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 011 — horribly out of tune — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “horribly out of tune” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “A Piano Out of Tune” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “horribly out of tune” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “horribly out of tune” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — horribly out of tune — A Piano Out of Tune

This proposed field-note fragment, beat 011 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “horribly out of tune” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “A Piano Out of Tune” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “horribly out of tune” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “horribly out of tune” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — horribly out of tune — A Piano Out of Tune

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “horribly out of tune” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “horribly out of tune” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “A Piano Out of Tune” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “horribly out of tune” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “horribly out of tune” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — horribly out of tune — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “horribly out of tune.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “A Piano Out of Tune” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “horribly out of tune” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “horribly out of tune” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: A Piano Out of Tune × completely blind

**Beat question:** What can the writer say about “completely blind” during “A Piano Out of Tune” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 012 — completely blind — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “completely blind” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A listener who wants to help a distinct perspective on “completely blind” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “A Piano Out of Tune” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “completely blind” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “completely blind” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — completely blind — A Piano Out of Tune

This proposed field-note fragment, beat 012 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “completely blind.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives A companion who measures silence differently a distinct perspective on “completely blind” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “A Piano Out of Tune” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “completely blind” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “completely blind” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — completely blind — A Piano Out of Tune

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “completely blind” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “completely blind” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “A Piano Out of Tune” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “completely blind” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “completely blind” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — completely blind — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “completely blind” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A listener who wants to help a distinct perspective on “completely blind” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “A Piano Out of Tune” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “completely blind” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “completely blind” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: A Piano Out of Tune × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “A Piano Out of Tune” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 013 — muscle memory — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “muscle memory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “A Piano Out of Tune” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “muscle memory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “muscle memory” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — muscle memory — A Piano Out of Tune

This proposed field-note fragment, beat 013 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “muscle memory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A listener who wants to help a distinct perspective on “muscle memory” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “A Piano Out of Tune” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “muscle memory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “muscle memory” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — muscle memory — A Piano Out of Tune

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “muscle memory” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “muscle memory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “A Piano Out of Tune” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “muscle memory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “muscle memory” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — muscle memory — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “muscle memory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “A Piano Out of Tune” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “muscle memory” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “muscle memory” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: A Piano Out of Tune × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “A Piano Out of Tune” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 014 — fingers stiff from the cold — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “fingers stiff from the cold” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “A Piano Out of Tune” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “fingers stiff from the cold” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “fingers stiff from the cold” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — fingers stiff from the cold — A Piano Out of Tune

This proposed field-note fragment, beat 014 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “fingers stiff from the cold” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “A Piano Out of Tune” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “fingers stiff from the cold” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “fingers stiff from the cold” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — fingers stiff from the cold — A Piano Out of Tune

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “fingers stiff from the cold” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “fingers stiff from the cold.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “A Piano Out of Tune” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “fingers stiff from the cold” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “fingers stiff from the cold” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — fingers stiff from the cold — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “fingers stiff from the cold” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “A Piano Out of Tune” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “fingers stiff from the cold” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “fingers stiff from the cold” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: A Piano Out of Tune × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “A Piano Out of Tune” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 015 — listen until he stops — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “listen until he stops.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A listener who wants to help a distinct perspective on “listen until he stops” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “A Piano Out of Tune” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “listen until he stops” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “listen until he stops” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — listen until he stops — A Piano Out of Tune

This proposed field-note fragment, beat 015 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “listen until he stops” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “A Piano Out of Tune” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “listen until he stops” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “listen until he stops” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — listen until he stops — A Piano Out of Tune

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “listen until he stops” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “listen until he stops” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “A Piano Out of Tune” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “listen until he stops” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “listen until he stops” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — listen until he stops — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “listen until he stops” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A listener who wants to help a distinct perspective on “listen until he stops” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “A Piano Out of Tune” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “listen until he stops” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “listen until he stops” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: A Piano Out of Tune × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “A Piano Out of Tune” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: What does careful listening notice without correcting the instrument?

#### Scene draft 016 — coordinates to your shelter — A Piano Out of Tune

For “A Piano Out of Tune” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “coordinates to your shelter” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “A Piano Out of Tune” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “A Piano Out of Tune” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “coordinates to your shelter” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “coordinates to your shelter” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — coordinates to your shelter — A Piano Out of Tune

This proposed field-note fragment, beat 016 in “A Piano Out of Tune,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “coordinates to your shelter” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “A Piano Out of Tune” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “A Piano Out of Tune.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “A Piano Out of Tune” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “coordinates to your shelter” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “coordinates to your shelter” during “A Piano Out of Tune” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — coordinates to your shelter — A Piano Out of Tune

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “coordinates to your shelter” during “A Piano Out of Tune,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “coordinates to your shelter” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “A Piano Out of Tune” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “A Piano Out of Tune.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “A Piano Out of Tune” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “coordinates to your shelter” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “coordinates to your shelter” during “A Piano Out of Tune” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — coordinates to your shelter — A Piano Out of Tune

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “A Piano Out of Tune” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “coordinates to your shelter.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “A Piano Out of Tune” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “A Piano Out of Tune.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “A Piano Out of Tune” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“What does careful listening notice without correcting the instrument?”—as a revision test tied to “coordinates to your shelter” during “A Piano Out of Tune.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “coordinates to your shelter” during “A Piano Out of Tune” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Piano Out of Tune” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: Hands Remember × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “Hands Remember” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 017 — roofless conservatory — Hands Remember

For “Hands Remember” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “roofless conservatory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “Hands Remember” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “roofless conservatory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “roofless conservatory” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — roofless conservatory — Hands Remember

This proposed field-note fragment, beat 017 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “roofless conservatory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “Hands Remember” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “roofless conservatory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “roofless conservatory” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — roofless conservatory — Hands Remember

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “roofless conservatory” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “roofless conservatory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “Hands Remember” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “roofless conservatory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “roofless conservatory” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — roofless conservatory — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “roofless conservatory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “Hands Remember” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “roofless conservatory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “roofless conservatory” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: Hands Remember × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “Hands Remember” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 018 — warped upright piano — Hands Remember

For “Hands Remember” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “warped upright piano” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “Hands Remember” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “warped upright piano” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “warped upright piano” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — warped upright piano — Hands Remember

This proposed field-note fragment, beat 018 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “warped upright piano” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “Hands Remember” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “warped upright piano” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “warped upright piano” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — warped upright piano — Hands Remember

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “warped upright piano” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “warped upright piano.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A listener who wants to help a distinct perspective on “warped upright piano” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “Hands Remember” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “warped upright piano” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “warped upright piano” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — warped upright piano — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “warped upright piano” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “Hands Remember” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “warped upright piano” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “warped upright piano” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: Hands Remember × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “Hands Remember” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 019 — horribly out of tune — Hands Remember

For “Hands Remember” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “horribly out of tune.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “Hands Remember” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “horribly out of tune” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “horribly out of tune” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — horribly out of tune — Hands Remember

This proposed field-note fragment, beat 019 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “horribly out of tune” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “Hands Remember” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “horribly out of tune” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “horribly out of tune” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — horribly out of tune — Hands Remember

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “horribly out of tune” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “horribly out of tune” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “Hands Remember” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “horribly out of tune” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “horribly out of tune” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — horribly out of tune — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “horribly out of tune” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “Hands Remember” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “horribly out of tune” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “horribly out of tune” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: Hands Remember × completely blind

**Beat question:** What can the writer say about “completely blind” during “Hands Remember” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 020 — completely blind — Hands Remember

For “Hands Remember” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “completely blind” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “Hands Remember” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “completely blind” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “completely blind” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — completely blind — Hands Remember

This proposed field-note fragment, beat 020 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “completely blind” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A listener who wants to help a distinct perspective on “completely blind” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “Hands Remember” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “completely blind” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “completely blind” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — completely blind — Hands Remember

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “completely blind” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “completely blind” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives A companion who measures silence differently a distinct perspective on “completely blind” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “Hands Remember” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “completely blind” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “completely blind” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — completely blind — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “completely blind.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “Hands Remember” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “completely blind” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “completely blind” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: Hands Remember × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “Hands Remember” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 021 — muscle memory — Hands Remember

For “Hands Remember” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “muscle memory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “Hands Remember” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “muscle memory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “muscle memory” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — muscle memory — Hands Remember

This proposed field-note fragment, beat 021 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “muscle memory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “Hands Remember” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “muscle memory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “muscle memory” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — muscle memory — Hands Remember

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “muscle memory” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “muscle memory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A listener who wants to help a distinct perspective on “muscle memory” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “Hands Remember” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “muscle memory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “muscle memory” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — muscle memory — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “muscle memory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “Hands Remember” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “muscle memory” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “muscle memory” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: Hands Remember × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “Hands Remember” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 022 — fingers stiff from the cold — Hands Remember

For “Hands Remember” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “fingers stiff from the cold” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “Hands Remember” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “fingers stiff from the cold” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “fingers stiff from the cold” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — fingers stiff from the cold — Hands Remember

This proposed field-note fragment, beat 022 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “fingers stiff from the cold” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “Hands Remember” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “fingers stiff from the cold” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “fingers stiff from the cold” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — fingers stiff from the cold — Hands Remember

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “fingers stiff from the cold” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “fingers stiff from the cold” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “Hands Remember” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “fingers stiff from the cold” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “fingers stiff from the cold” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — fingers stiff from the cold — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “fingers stiff from the cold” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “Hands Remember” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “fingers stiff from the cold” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “fingers stiff from the cold” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: Hands Remember × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “Hands Remember” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 023 — listen until he stops — Hands Remember

For “Hands Remember” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “listen until he stops” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “Hands Remember” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “listen until he stops” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “listen until he stops” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — listen until he stops — Hands Remember

This proposed field-note fragment, beat 023 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “listen until he stops” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A listener who wants to help a distinct perspective on “listen until he stops” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “Hands Remember” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “listen until he stops” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “listen until he stops” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — listen until he stops — Hands Remember

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “listen until he stops” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “listen until he stops.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “Hands Remember” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “listen until he stops” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “listen until he stops” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — listen until he stops — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “listen until he stops” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “Hands Remember” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “listen until he stops” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “listen until he stops” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: Hands Remember × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “Hands Remember” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: How can skill appear without a flashback or explanation?

#### Scene draft 024 — coordinates to your shelter — Hands Remember

For “Hands Remember” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “coordinates to your shelter.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “Hands Remember” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “Hands Remember” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “coordinates to your shelter” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “coordinates to your shelter” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — coordinates to your shelter — Hands Remember

This proposed field-note fragment, beat 024 in “Hands Remember,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “coordinates to your shelter” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “Hands Remember” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “Hands Remember.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “Hands Remember” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “coordinates to your shelter” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “coordinates to your shelter” during “Hands Remember” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — coordinates to your shelter — Hands Remember

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “coordinates to your shelter” during “Hands Remember,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “coordinates to your shelter” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “Hands Remember” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “Hands Remember.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “Hands Remember” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “coordinates to your shelter” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “coordinates to your shelter” during “Hands Remember” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — coordinates to your shelter — Hands Remember

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “Hands Remember” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “coordinates to your shelter” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “Hands Remember” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “Hands Remember.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “Hands Remember” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“How can skill appear without a flashback or explanation?”—as a revision test tied to “coordinates to your shelter” during “Hands Remember.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “coordinates to your shelter” during “Hands Remember” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Hands Remember” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: Attention Without a Look × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “Attention Without a Look” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 025 — roofless conservatory — Attention Without a Look

For “Attention Without a Look” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “roofless conservatory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “Attention Without a Look” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “roofless conservatory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “roofless conservatory” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — roofless conservatory — Attention Without a Look

This proposed field-note fragment, beat 025 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “roofless conservatory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “Attention Without a Look” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “roofless conservatory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “roofless conservatory” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — roofless conservatory — Attention Without a Look

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “roofless conservatory” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “roofless conservatory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “Attention Without a Look” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “roofless conservatory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “roofless conservatory” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — roofless conservatory — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “roofless conservatory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “Attention Without a Look” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “roofless conservatory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “roofless conservatory” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: Attention Without a Look × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “Attention Without a Look” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 026 — warped upright piano — Attention Without a Look

For “Attention Without a Look” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “warped upright piano” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A listener who wants to help a distinct perspective on “warped upright piano” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “Attention Without a Look” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “warped upright piano” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “warped upright piano” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — warped upright piano — Attention Without a Look

This proposed field-note fragment, beat 026 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “warped upright piano” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “Attention Without a Look” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “warped upright piano” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “warped upright piano” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — warped upright piano — Attention Without a Look

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “warped upright piano” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “warped upright piano” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “Attention Without a Look” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “warped upright piano” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “warped upright piano” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — warped upright piano — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “warped upright piano” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A listener who wants to help a distinct perspective on “warped upright piano” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “Attention Without a Look” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “warped upright piano” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “warped upright piano” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: Attention Without a Look × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “Attention Without a Look” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 027 — horribly out of tune — Attention Without a Look

For “Attention Without a Look” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “horribly out of tune” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “Attention Without a Look” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “horribly out of tune” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “horribly out of tune” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — horribly out of tune — Attention Without a Look

This proposed field-note fragment, beat 027 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “horribly out of tune” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “Attention Without a Look” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “horribly out of tune” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “horribly out of tune” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — horribly out of tune — Attention Without a Look

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “horribly out of tune” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “horribly out of tune.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “Attention Without a Look” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “horribly out of tune” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “horribly out of tune” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — horribly out of tune — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “horribly out of tune” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “Attention Without a Look” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “horribly out of tune” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “horribly out of tune” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: Attention Without a Look × completely blind

**Beat question:** What can the writer say about “completely blind” during “Attention Without a Look” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 028 — completely blind — Attention Without a Look

For “Attention Without a Look” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “completely blind.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives A companion who measures silence differently a distinct perspective on “completely blind” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “Attention Without a Look” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “completely blind” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “completely blind” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — completely blind — Attention Without a Look

This proposed field-note fragment, beat 028 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “completely blind” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “Attention Without a Look” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “completely blind” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “completely blind” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — completely blind — Attention Without a Look

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “completely blind” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “completely blind” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A listener who wants to help a distinct perspective on “completely blind” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “Attention Without a Look” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “completely blind” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “completely blind” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — completely blind — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “completely blind” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives A companion who measures silence differently a distinct perspective on “completely blind” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “Attention Without a Look” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “completely blind” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “completely blind” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: Attention Without a Look × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “Attention Without a Look” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 029 — muscle memory — Attention Without a Look

For “Attention Without a Look” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “muscle memory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A listener who wants to help a distinct perspective on “muscle memory” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “Attention Without a Look” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “muscle memory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “muscle memory” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — muscle memory — Attention Without a Look

This proposed field-note fragment, beat 029 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “muscle memory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “Attention Without a Look” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “muscle memory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “muscle memory” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — muscle memory — Attention Without a Look

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “muscle memory” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “muscle memory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “Attention Without a Look” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “muscle memory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “muscle memory” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — muscle memory — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “muscle memory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A listener who wants to help a distinct perspective on “muscle memory” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “Attention Without a Look” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “muscle memory” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “muscle memory” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: Attention Without a Look × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “Attention Without a Look” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 030 — fingers stiff from the cold — Attention Without a Look

For “Attention Without a Look” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “fingers stiff from the cold” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “Attention Without a Look” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “fingers stiff from the cold” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “fingers stiff from the cold” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — fingers stiff from the cold — Attention Without a Look

This proposed field-note fragment, beat 030 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “fingers stiff from the cold.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “Attention Without a Look” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “fingers stiff from the cold” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “fingers stiff from the cold” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — fingers stiff from the cold — Attention Without a Look

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “fingers stiff from the cold” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “fingers stiff from the cold” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “Attention Without a Look” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “fingers stiff from the cold” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “fingers stiff from the cold” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — fingers stiff from the cold — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “fingers stiff from the cold” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “Attention Without a Look” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “fingers stiff from the cold” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “fingers stiff from the cold” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: Attention Without a Look × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “Attention Without a Look” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 031 — listen until he stops — Attention Without a Look

For “Attention Without a Look” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “listen until he stops” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “Attention Without a Look” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “listen until he stops” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “listen until he stops” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — listen until he stops — Attention Without a Look

This proposed field-note fragment, beat 031 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “listen until he stops” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “Attention Without a Look” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “listen until he stops” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “listen until he stops” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — listen until he stops — Attention Without a Look

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “listen until he stops” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “listen until he stops” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A listener who wants to help a distinct perspective on “listen until he stops” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “Attention Without a Look” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “listen until he stops” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “listen until he stops” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — listen until he stops — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “listen until he stops” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “Attention Without a Look” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “listen until he stops” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “listen until he stops” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: Attention Without a Look × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “Attention Without a Look” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: What forms of acknowledgment are possible without demanding eye contact?

#### Scene draft 032 — coordinates to your shelter — Attention Without a Look

For “Attention Without a Look” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “coordinates to your shelter” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “Attention Without a Look” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “Attention Without a Look” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “coordinates to your shelter” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “coordinates to your shelter” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — coordinates to your shelter — Attention Without a Look

This proposed field-note fragment, beat 032 in “Attention Without a Look,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “coordinates to your shelter” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “Attention Without a Look” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “Attention Without a Look.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “Attention Without a Look” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “coordinates to your shelter” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “coordinates to your shelter” during “Attention Without a Look” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — coordinates to your shelter — Attention Without a Look

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “coordinates to your shelter” during “Attention Without a Look,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “coordinates to your shelter.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “Attention Without a Look” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “Attention Without a Look.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “Attention Without a Look” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “coordinates to your shelter” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “coordinates to your shelter” during “Attention Without a Look” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — coordinates to your shelter — Attention Without a Look

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “Attention Without a Look” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “coordinates to your shelter” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “Attention Without a Look” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “Attention Without a Look.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “Attention Without a Look” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“What forms of acknowledgment are possible without demanding eye contact?”—as a revision test tied to “coordinates to your shelter” during “Attention Without a Look.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “coordinates to your shelter” during “Attention Without a Look” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Attention Without a Look” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: An Offer, Not a Bargain × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “An Offer, Not a Bargain” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 033 — roofless conservatory — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “roofless conservatory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “An Offer, Not a Bargain” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “roofless conservatory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “roofless conservatory” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — roofless conservatory — An Offer, Not a Bargain

This proposed field-note fragment, beat 033 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “roofless conservatory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “An Offer, Not a Bargain” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “roofless conservatory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “roofless conservatory” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — roofless conservatory — An Offer, Not a Bargain

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “roofless conservatory” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “roofless conservatory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “An Offer, Not a Bargain” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “roofless conservatory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “roofless conservatory” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — roofless conservatory — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “roofless conservatory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “An Offer, Not a Bargain” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “roofless conservatory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “roofless conservatory” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: An Offer, Not a Bargain × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “An Offer, Not a Bargain” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 034 — warped upright piano — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “warped upright piano” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “An Offer, Not a Bargain” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “warped upright piano” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “warped upright piano” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — warped upright piano — An Offer, Not a Bargain

This proposed field-note fragment, beat 034 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “warped upright piano.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A listener who wants to help a distinct perspective on “warped upright piano” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “An Offer, Not a Bargain” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “warped upright piano” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “warped upright piano” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — warped upright piano — An Offer, Not a Bargain

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “warped upright piano” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “warped upright piano” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “An Offer, Not a Bargain” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “warped upright piano” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “warped upright piano” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — warped upright piano — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “warped upright piano” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “An Offer, Not a Bargain” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “warped upright piano” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “warped upright piano” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: An Offer, Not a Bargain × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “An Offer, Not a Bargain” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 035 — horribly out of tune — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “horribly out of tune” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “An Offer, Not a Bargain” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “horribly out of tune” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “horribly out of tune” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — horribly out of tune — An Offer, Not a Bargain

This proposed field-note fragment, beat 035 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “horribly out of tune” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “An Offer, Not a Bargain” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “horribly out of tune” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “horribly out of tune” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — horribly out of tune — An Offer, Not a Bargain

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “horribly out of tune” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “horribly out of tune” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “An Offer, Not a Bargain” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “horribly out of tune” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “horribly out of tune” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — horribly out of tune — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “horribly out of tune” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “An Offer, Not a Bargain” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “horribly out of tune” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “horribly out of tune” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: An Offer, Not a Bargain × completely blind

**Beat question:** What can the writer say about “completely blind” during “An Offer, Not a Bargain” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 036 — completely blind — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “completely blind” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A listener who wants to help a distinct perspective on “completely blind” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “An Offer, Not a Bargain” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “completely blind” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “completely blind” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — completely blind — An Offer, Not a Bargain

This proposed field-note fragment, beat 036 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “completely blind” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives A companion who measures silence differently a distinct perspective on “completely blind” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “An Offer, Not a Bargain” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “completely blind” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “completely blind” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — completely blind — An Offer, Not a Bargain

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “completely blind” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “completely blind.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “An Offer, Not a Bargain” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “completely blind” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “completely blind” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — completely blind — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “completely blind” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A listener who wants to help a distinct perspective on “completely blind” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “An Offer, Not a Bargain” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “completely blind” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “completely blind” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: An Offer, Not a Bargain × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “An Offer, Not a Bargain” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 037 — muscle memory — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “muscle memory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “An Offer, Not a Bargain” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “muscle memory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “muscle memory” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — muscle memory — An Offer, Not a Bargain

This proposed field-note fragment, beat 037 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “muscle memory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A listener who wants to help a distinct perspective on “muscle memory” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “An Offer, Not a Bargain” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “muscle memory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “muscle memory” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — muscle memory — An Offer, Not a Bargain

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “muscle memory” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “muscle memory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “An Offer, Not a Bargain” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “muscle memory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “muscle memory” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — muscle memory — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “muscle memory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “An Offer, Not a Bargain” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “muscle memory” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “muscle memory” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: An Offer, Not a Bargain × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “An Offer, Not a Bargain” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 038 — fingers stiff from the cold — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “fingers stiff from the cold” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “An Offer, Not a Bargain” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “fingers stiff from the cold” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “fingers stiff from the cold” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — fingers stiff from the cold — An Offer, Not a Bargain

This proposed field-note fragment, beat 038 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “fingers stiff from the cold” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “An Offer, Not a Bargain” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “fingers stiff from the cold” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “fingers stiff from the cold” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — fingers stiff from the cold — An Offer, Not a Bargain

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “fingers stiff from the cold” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “fingers stiff from the cold” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “An Offer, Not a Bargain” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “fingers stiff from the cold” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “fingers stiff from the cold” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — fingers stiff from the cold — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “fingers stiff from the cold.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “An Offer, Not a Bargain” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “fingers stiff from the cold” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “fingers stiff from the cold” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: An Offer, Not a Bargain × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “An Offer, Not a Bargain” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 039 — listen until he stops — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “listen until he stops” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A listener who wants to help a distinct perspective on “listen until he stops” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “An Offer, Not a Bargain” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “listen until he stops” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “listen until he stops” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — listen until he stops — An Offer, Not a Bargain

This proposed field-note fragment, beat 039 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “listen until he stops.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “An Offer, Not a Bargain” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “listen until he stops” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “listen until he stops” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — listen until he stops — An Offer, Not a Bargain

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “listen until he stops” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “listen until he stops” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “An Offer, Not a Bargain” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “listen until he stops” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “listen until he stops” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — listen until he stops — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “listen until he stops” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A listener who wants to help a distinct perspective on “listen until he stops” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “An Offer, Not a Bargain” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “listen until he stops” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “listen until he stops” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: An Offer, Not a Bargain × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “An Offer, Not a Bargain” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: How can optional prose preserve refusal and consequence without praising coercion?

#### Scene draft 040 — coordinates to your shelter — An Offer, Not a Bargain

For “An Offer, Not a Bargain” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “coordinates to your shelter” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “An Offer, Not a Bargain” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “An Offer, Not a Bargain” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “coordinates to your shelter” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “coordinates to your shelter” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — coordinates to your shelter — An Offer, Not a Bargain

This proposed field-note fragment, beat 040 in “An Offer, Not a Bargain,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “coordinates to your shelter” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “An Offer, Not a Bargain” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “An Offer, Not a Bargain.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “An Offer, Not a Bargain” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “coordinates to your shelter” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “coordinates to your shelter” during “An Offer, Not a Bargain” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — coordinates to your shelter — An Offer, Not a Bargain

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “coordinates to your shelter” during “An Offer, Not a Bargain,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “coordinates to your shelter” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “An Offer, Not a Bargain” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “An Offer, Not a Bargain.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “An Offer, Not a Bargain” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “coordinates to your shelter” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “coordinates to your shelter” during “An Offer, Not a Bargain” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — coordinates to your shelter — An Offer, Not a Bargain

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “An Offer, Not a Bargain” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “coordinates to your shelter” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “An Offer, Not a Bargain” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “An Offer, Not a Bargain.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “An Offer, Not a Bargain” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“How can optional prose preserve refusal and consequence without praising coercion?”—as a revision test tied to “coordinates to your shelter” during “An Offer, Not a Bargain.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “coordinates to your shelter” during “An Offer, Not a Bargain” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “An Offer, Not a Bargain” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: When Playing Stops × roofless conservatory

**Beat question:** What can the writer say about “roofless conservatory” during “When Playing Stops” while preserving this limit: the canonical setting, open to weather and acoustically unpromised. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 041 — roofless conservatory — When Playing Stops

For “When Playing Stops” and the source phrase “roofless conservatory,” the candidate passage attends to The canonical setting, open to weather and acoustically unpromised. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “roofless conservatory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “When Playing Stops” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “roofless conservatory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “roofless conservatory” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — roofless conservatory — When Playing Stops

This proposed field-note fragment, beat 041 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “roofless conservatory” is the point of return. The canonical setting, open to weather and acoustically unpromised. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “roofless conservatory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A listener who wants to help a distinct perspective on “roofless conservatory” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “When Playing Stops” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “roofless conservatory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “roofless conservatory” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — roofless conservatory — When Playing Stops

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “roofless conservatory” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “roofless conservatory” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives A companion who measures silence differently a distinct perspective on “roofless conservatory” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “When Playing Stops” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “roofless conservatory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “roofless conservatory” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — roofless conservatory — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “roofless conservatory” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “roofless conservatory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “roofless conservatory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives The pianist, only in optional authored dialogue a distinct perspective on “roofless conservatory” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “When Playing Stops” × “roofless conservatory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “roofless conservatory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “roofless conservatory” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “roofless conservatory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: When Playing Stops × warped upright piano

**Beat question:** What can the writer say about “warped upright piano” during “When Playing Stops” while preserving this limit: a specific instrument with no repair history supplied. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 042 — warped upright piano — When Playing Stops

For “When Playing Stops” and the source phrase “warped upright piano,” the candidate passage attends to A specific instrument with no repair history supplied. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “warped upright piano” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “When Playing Stops” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “warped upright piano” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “warped upright piano” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — warped upright piano — When Playing Stops

This proposed field-note fragment, beat 042 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warped upright piano” is the point of return. A specific instrument with no repair history supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “warped upright piano” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives The pianist, only in optional authored dialogue a distinct perspective on “warped upright piano” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “When Playing Stops” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “warped upright piano” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “warped upright piano” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — warped upright piano — When Playing Stops

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “warped upright piano” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “warped upright piano” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A listener who wants to help a distinct perspective on “warped upright piano” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “When Playing Stops” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “warped upright piano” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “warped upright piano” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — warped upright piano — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warped upright piano” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “warped upright piano.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warped upright piano” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives A companion who measures silence differently a distinct perspective on “warped upright piano” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “When Playing Stops” × “warped upright piano,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “warped upright piano” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “warped upright piano” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warped upright piano.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: When Playing Stops × horribly out of tune

**Beat question:** What can the writer say about “horribly out of tune” during “When Playing Stops” while preserving this limit: a sound quality, not a code or an invitation to teach tuning. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 043 — horribly out of tune — When Playing Stops

For “When Playing Stops” and the source phrase “horribly out of tune,” the candidate passage attends to A sound quality, not a code or an invitation to teach tuning. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “horribly out of tune” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “When Playing Stops” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “horribly out of tune” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “horribly out of tune” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — horribly out of tune — When Playing Stops

This proposed field-note fragment, beat 043 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “horribly out of tune” is the point of return. A sound quality, not a code or an invitation to teach tuning. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “horribly out of tune.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives A companion who measures silence differently a distinct perspective on “horribly out of tune” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “When Playing Stops” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “horribly out of tune” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “horribly out of tune” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — horribly out of tune — When Playing Stops

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “horribly out of tune” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “horribly out of tune” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives The pianist, only in optional authored dialogue a distinct perspective on “horribly out of tune” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “When Playing Stops” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “horribly out of tune” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “horribly out of tune” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — horribly out of tune — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “horribly out of tune” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “horribly out of tune” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “horribly out of tune” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A listener who wants to help a distinct perspective on “horribly out of tune” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “When Playing Stops” × “horribly out of tune,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “horribly out of tune” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “horribly out of tune” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “horribly out of tune.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: When Playing Stops × completely blind

**Beat question:** What can the writer say about “completely blind” during “When Playing Stops” while preserving this limit: a fact that does not erase expertise, preference, or agency. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 044 — completely blind — When Playing Stops

For “When Playing Stops” and the source phrase “completely blind,” the candidate passage attends to A fact that does not erase expertise, preference, or agency. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “completely blind” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “When Playing Stops” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “completely blind” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “completely blind” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — completely blind — When Playing Stops

This proposed field-note fragment, beat 044 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “completely blind” is the point of return. A fact that does not erase expertise, preference, or agency. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “completely blind” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A listener who wants to help a distinct perspective on “completely blind” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “When Playing Stops” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “completely blind” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “completely blind” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — completely blind — When Playing Stops

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “completely blind” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “completely blind” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives A companion who measures silence differently a distinct perspective on “completely blind” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “When Playing Stops” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “completely blind” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “completely blind” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — completely blind — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “completely blind” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “completely blind” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “completely blind” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives The pianist, only in optional authored dialogue a distinct perspective on “completely blind” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “When Playing Stops” × “completely blind,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “completely blind” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “completely blind” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “completely blind.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: When Playing Stops × muscle memory

**Beat question:** What can the writer say about “muscle memory” during “When Playing Stops” while preserving this limit: a practiced action, not proof of a particular past. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 045 — muscle memory — When Playing Stops

For “When Playing Stops” and the source phrase “muscle memory,” the candidate passage attends to A practiced action, not proof of a particular past. The present action begins small: a companion waiting through a phrase without counting it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “muscle memory” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “When Playing Stops” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “muscle memory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “muscle memory” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — muscle memory — When Playing Stops

This proposed field-note fragment, beat 045 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “muscle memory” is the point of return. A practiced action, not proof of a particular past. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “muscle memory” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives The pianist, only in optional authored dialogue a distinct perspective on “muscle memory” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “When Playing Stops” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “muscle memory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “muscle memory” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — muscle memory — When Playing Stops

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “muscle memory” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “muscle memory.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A listener who wants to help a distinct perspective on “muscle memory” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “When Playing Stops” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “muscle memory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “muscle memory” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — muscle memory — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “muscle memory” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “muscle memory” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “muscle memory” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives A companion who measures silence differently a distinct perspective on “muscle memory” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “When Playing Stops” × “muscle memory,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “muscle memory” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “muscle memory” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “muscle memory.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: When Playing Stops × fingers stiff from the cold

**Beat question:** What can the writer say about “fingers stiff from the cold” during “When Playing Stops” while preserving this limit: a present bodily detail, not a diagnosis or prescribed treatment. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 046 — fingers stiff from the cold — When Playing Stops

For “When Playing Stops” and the source phrase “fingers stiff from the cold,” the candidate passage attends to A present bodily detail, not a diagnosis or prescribed treatment. The present action begins small: an offered ration kept separate from the instrument. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “fingers stiff from the cold.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “When Playing Stops” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “fingers stiff from the cold” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “fingers stiff from the cold” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — fingers stiff from the cold — When Playing Stops

This proposed field-note fragment, beat 046 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “fingers stiff from the cold” is the point of return. A present bodily detail, not a diagnosis or prescribed treatment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “fingers stiff from the cold” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives A companion who measures silence differently a distinct perspective on “fingers stiff from the cold” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “When Playing Stops” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “fingers stiff from the cold” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “fingers stiff from the cold” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — fingers stiff from the cold — When Playing Stops

The proposed exchange gives the pianist, only in optional authored dialogue a distinct reason to speak. Its authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” The talk concerns “fingers stiff from the cold” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “fingers stiff from the cold” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives The pianist, only in optional authored dialogue a distinct perspective on “fingers stiff from the cold” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “When Playing Stops” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard the room change when I entered. That is enough for now.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “fingers stiff from the cold” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “fingers stiff from the cold” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — fingers stiff from the cold — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “fingers stiff from the cold” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “fingers stiff from the cold” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “fingers stiff from the cold” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A listener who wants to help a distinct perspective on “fingers stiff from the cold” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “When Playing Stops” × “fingers stiff from the cold,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “fingers stiff from the cold” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “fingers stiff from the cold” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “fingers stiff from the cold.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: When Playing Stops × listen until he stops

**Beat question:** What can the writer say about “listen until he stops” during “When Playing Stops” while preserving this limit: an existing choice whose wording can hold patience without guaranteeing a response. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 047 — listen until he stops — When Playing Stops

For “When Playing Stops” and the source phrase “listen until he stops,” the candidate passage attends to An existing choice whose wording can hold patience without guaranteeing a response. The present action begins small: the keys under fingers that do not need to be described as perfect. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “listen until he stops” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “When Playing Stops” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “listen until he stops” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “listen until he stops” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — listen until he stops — When Playing Stops

This proposed field-note fragment, beat 047 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “listen until he stops” is the point of return. An existing choice whose wording can hold patience without guaranteeing a response. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “listen until he stops” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A listener who wants to help a distinct perspective on “listen until he stops” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “When Playing Stops” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “listen until he stops” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “listen until he stops” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — listen until he stops — When Playing Stops

The proposed exchange gives a companion who measures silence differently a distinct reason to speak. Its authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” The talk concerns “listen until he stops” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “listen until he stops” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives A companion who measures silence differently a distinct perspective on “listen until he stops” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “When Playing Stops” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “You can keep the doorway. I do not need you to come closer.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “listen until he stops” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “listen until he stops” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — listen until he stops — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “listen until he stops” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “listen until he stops.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “listen until he stops” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives The pianist, only in optional authored dialogue a distinct perspective on “listen until he stops” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “When Playing Stops” × “listen until he stops,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “listen until he stops” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “listen until he stops” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “listen until he stops.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: When Playing Stops × coordinates to your shelter

**Beat question:** What can the writer say about “coordinates to your shelter” during “When Playing Stops” while preserving this limit: an offer with real privacy implications; no later arrival is established. The larger movement question is: How can an ending honor a pause without making it a revelation?

#### Scene draft 048 — coordinates to your shelter — When Playing Stops

For “When Playing Stops” and the source phrase “coordinates to your shelter,” the candidate passage attends to An offer with real privacy implications; no later arrival is established. The present action begins small: the doorway left open as the listener decides whether to stay. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “coordinates to your shelter” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “When Playing Stops” scene draft, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “When Playing Stops” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “coordinates to your shelter” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “coordinates to your shelter” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — coordinates to your shelter — When Playing Stops

This proposed field-note fragment, beat 048 in “When Playing Stops,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “coordinates to your shelter” is the point of return. An offer with real privacy implications; no later arrival is established. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “coordinates to your shelter.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “When Playing Stops” field-note fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives The pianist, only in optional authored dialogue a distinct perspective on “coordinates to your shelter” during “When Playing Stops.” The optional authoring note is: “May choose a brief boundary or acknowledgment; any spoken line is proposed text, not established canon.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “When Playing Stops” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “If you leave food, leave it where I can choose to take it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “coordinates to your shelter” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “coordinates to your shelter” during “When Playing Stops” in a changed register: “You can keep the doorway. I do not need you to come closer.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — coordinates to your shelter — When Playing Stops

The proposed exchange gives a listener who wants to help a distinct reason to speak. Its authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” The talk concerns “coordinates to your shelter” during “When Playing Stops,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “coordinates to your shelter” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “When Playing Stops” conversation fragment, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A listener who wants to help a distinct perspective on “coordinates to your shelter” during “When Playing Stops.” The optional authoring note is: “Offers presence, food, or information without assuming the pianist owes a response.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “When Playing Stops” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “You can keep the doorway. I do not need you to come closer.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If you leave food, leave it where I can choose to take it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “coordinates to your shelter” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “coordinates to your shelter” during “When Playing Stops” in a changed register: “I heard the room change when I entered. That is enough for now.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — coordinates to your shelter — When Playing Stops

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “coordinates to your shelter” through “When Playing Stops” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “coordinates to your shelter” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “When Playing Stops” conditional return vignette, is narrow. The local description says: “An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “coordinates to your shelter” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives A companion who measures silence differently a distinct perspective on “coordinates to your shelter” during “When Playing Stops.” The optional authoring note is: “Finds the lack of eye contact unfamiliar, then recognizes that attention need not look back.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “When Playing Stops” × “coordinates to your shelter,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard the room change when I entered. That is enough for now.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“How can an ending honor a pause without making it a revelation?”—as a revision test tied to “coordinates to your shelter” during “When Playing Stops.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “coordinates to your shelter” during “When Playing Stops” in a changed register: “If you leave food, leave it where I can choose to take it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “When Playing Stops” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “coordinates to your shelter.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_pianist`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

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

The proposal is local to `enc_pianist` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_pianist` in `Assets/StreamingAssets/Data/narrative_encounters.json` | Catalog presence does not by itself show where prose is presented. |
| Description | An old man plays a warped upright piano in the ruins of a roofless conservatory. The piano is horribly out of tune. The man is completely blind. He plays from muscle memory, his fingers stiff from the cold. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_pianist` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.