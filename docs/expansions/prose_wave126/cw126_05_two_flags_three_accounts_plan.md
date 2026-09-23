# EXPANSION CW126-05 — Two Flags, Three Accounts

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_two_camps` — The Two Camps.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`.
**Thesis:** A valley encounter in which visible differences and incompatible testimony stay unresolved long enough for caution to have a human cost.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A valley encounter in which visible differences and incompatible testimony stay unresolved long enough for caution to have a human cost. The plan builds an optional scene bank around the exact local description “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

How can prose sustain uncertainty about both camps without making either allegation true by repetition?

## 3. Verified source record

The source record contains these exact fields: id: "enc_two_camps"; title: "The Two Camps"; description: "Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying."; category: "Misinformation"; baseWeight: 2.0; stealthWeightMultiplier: 1.0; speedWeightMultiplier: 1.0; minDangerLevel: 1.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "ridge_figure", "text": "Trust the ridge scout. Approach the cold camp.", "moraleDelta": 0, "guiltDelta": 3}, {"choiceId": "smoke_camp", "text": "Trust the cooking smoke. Approach the valley camp.", "moraleDelta": 0, "guiltDelta": 3}, {"choiceId": "avoid_both", "text": "Avoid both. Detour three miles around the valley.", "moraleDelta": 2, "guiltDelta": 0}, {"choiceId": "watch_longer", "text": "Hold position. Watch them for a full day before deciding.", "moraleDelta": 3, "guiltDelta": 0}]. Source: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | `enc_two_camps` | title=The Two Camps; category=Misinformation; description=Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `ridge_figure` — “Trust the ridge scout. Approach the cold camp.” (moraleDelta 0, guiltDelta 3)
- `smoke_camp` — “Trust the cooking smoke. Approach the valley camp.” (moraleDelta 0, guiltDelta 3)
- `avoid_both` — “Avoid both. Detour three miles around the valley.” (moraleDelta 2, guiltDelta 0)
- `watch_longer` — “Hold position. Watch them for a full day before deciding.” (moraleDelta 3, guiltDelta 0)

## 4. Fixed canon and open space

Do not decide which camp is truthful, stage an attack, explain the absence of smoke, create a third witness, or make either accusation fact. Keep the allegation “cannibal den” attributed to the scout who used it. No real survival or surveillance procedures, combat tactics, faction system, reputation outcome, or route length beyond the existing choice text.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This record is present in the expansion JSON, but current NarrativeEncounterCatalogLoader loads narrative_encounters.json, narrative_encounters_npc_arcs.json, and micro_locations.json. ContentUtilizationScanner references are static mapping declarations, not proof that this expansion file is loaded. Treat every passage below as editorial and currently unverified for runtime reachability.

## 5. Human center

Two camps share a bleached white flag; one has smoke; scouts offer opposing accusations. The catalog itself says they could both be lying. No faction names or confirmed history are supplied.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A traveler who records claims separately:** Does not use a shared flag as proof of shared intent.
- **A companion who wants a verdict:** Feels the pressure of choosing and can say so without becoming foolish.
- **A late-arriving listener in a proposed return:** Questions whether the record preserves who said what.

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

The content anchor is `enc_two_camps` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: Across the Valley

The camps are visible from a distance but their daily life is not. The movement asks: How much can distance make a reader think they know? Its source handle is “two camps”: A count that does not make either group a unified character. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: One Smoke, One Cold Camp

A difference in visible smoke invites interpretation. The movement asks: Can an image remain an observation instead of a verdict? Its source handle is “across a valley”: A viewing distance, not an exact map. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: The Ridge Scout Speaks

The first accusation has a source and a point of view. The movement asks: How should prose preserve that attribution? Its source handle is “same bleached white flag”: An identical visible marker with no established shared organization. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: Yesterday’s Counterclaim

The smoking camp’s scout gave a contrary allegation yesterday. The movement asks: What changes when both stories are allowed to be strategic? Its source handle is “cooking smoke”: A sign of activity, not proof of benevolence. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: The Shared Flag

A symbol appears at both camps, but its meaning is not explained. The movement asks: How can a repeated symbol remain ambiguous? Its source handle is “cold camp”: A description in one choice, not a verified lack of people or supplies. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: Detour, Wait, or Approach

Existing choice text offers several stances; the scene need not declare one morally pure. The movement asks: What does a decision cost when knowledge cannot be finished? Its source handle is “ridge scout says”: An attributed claim rather than narrator fact. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: Across the Valley × two camps

**Beat question:** What can the writer say about “two camps” during “Across the Valley” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 001 — two camps — Across the Valley

For “Across the Valley” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “two camps.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives A companion who wants a verdict a distinct perspective on “two camps” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “Across the Valley” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “two camps” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “two camps” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — two camps — Across the Valley

This proposed field-note fragment, beat 001 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “two camps” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “Across the Valley” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “two camps” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “two camps” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — two camps — Across the Valley

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “two camps” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “two camps” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A traveler who records claims separately a distinct perspective on “two camps” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “Across the Valley” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “two camps” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “two camps” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — two camps — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “two camps” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives A companion who wants a verdict a distinct perspective on “two camps” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “Across the Valley” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “two camps” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “two camps” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: Across the Valley × across a valley

**Beat question:** What can the writer say about “across a valley” during “Across the Valley” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 002 — across a valley — Across the Valley

For “Across the Valley” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “across a valley” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A traveler who records claims separately a distinct perspective on “across a valley” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “Across the Valley” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “across a valley” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “across a valley” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — across a valley — Across the Valley

This proposed field-note fragment, beat 002 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “across a valley” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives A companion who wants a verdict a distinct perspective on “across a valley” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “Across the Valley” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “across a valley” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “across a valley” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — across a valley — Across the Valley

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “across a valley” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “across a valley” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “Across the Valley” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “across a valley” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “across a valley” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — across a valley — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “across a valley.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A traveler who records claims separately a distinct perspective on “across a valley” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “Across the Valley” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “across a valley” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “across a valley” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: Across the Valley × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “Across the Valley” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 003 — same bleached white flag — Across the Valley

For “Across the Valley” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “same bleached white flag” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “Across the Valley” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “same bleached white flag” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “same bleached white flag” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — same bleached white flag — Across the Valley

This proposed field-note fragment, beat 003 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “same bleached white flag.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “Across the Valley” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “same bleached white flag” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “same bleached white flag” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — same bleached white flag — Across the Valley

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “same bleached white flag” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “same bleached white flag” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “Across the Valley” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “same bleached white flag” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “same bleached white flag” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — same bleached white flag — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “same bleached white flag” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “Across the Valley” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “same bleached white flag” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “same bleached white flag” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: Across the Valley × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “Across the Valley” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 004 — cooking smoke — Across the Valley

For “Across the Valley” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “cooking smoke” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “Across the Valley” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cooking smoke” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “cooking smoke” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — cooking smoke — Across the Valley

This proposed field-note fragment, beat 004 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “cooking smoke” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “Across the Valley” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cooking smoke” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “cooking smoke” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — cooking smoke — Across the Valley

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “cooking smoke” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “cooking smoke” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “Across the Valley” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cooking smoke” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “cooking smoke” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — cooking smoke — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “cooking smoke” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “Across the Valley” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cooking smoke” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “cooking smoke” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: Across the Valley × cold camp

**Beat question:** What can the writer say about “cold camp” during “Across the Valley” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 005 — cold camp — Across the Valley

For “Across the Valley” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “cold camp” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A traveler who records claims separately a distinct perspective on “cold camp” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “Across the Valley” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cold camp” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “cold camp” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — cold camp — Across the Valley

This proposed field-note fragment, beat 005 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “cold camp” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives A companion who wants a verdict a distinct perspective on “cold camp” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “Across the Valley” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cold camp” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “cold camp” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — cold camp — Across the Valley

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “cold camp” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “cold camp.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “Across the Valley” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cold camp” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “cold camp” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — cold camp — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “cold camp” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A traveler who records claims separately a distinct perspective on “cold camp” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “Across the Valley” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “cold camp” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “cold camp” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: Across the Valley × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “Across the Valley” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 006 — ridge scout says — Across the Valley

For “Across the Valley” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “ridge scout says.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “Across the Valley” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “ridge scout says” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “ridge scout says” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — ridge scout says — Across the Valley

This proposed field-note fragment, beat 006 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “ridge scout says” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “Across the Valley” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “ridge scout says” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “ridge scout says” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — ridge scout says — Across the Valley

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “ridge scout says” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “ridge scout says” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “Across the Valley” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “ridge scout says” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “ridge scout says” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — ridge scout says — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “ridge scout says” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “Across the Valley” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “ridge scout says” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “ridge scout says” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: Across the Valley × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “Across the Valley” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 007 — told you yesterday — Across the Valley

For “Across the Valley” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “told you yesterday” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “Across the Valley” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “told you yesterday” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “told you yesterday” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — told you yesterday — Across the Valley

This proposed field-note fragment, beat 007 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “told you yesterday” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “Across the Valley” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “told you yesterday” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “told you yesterday” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — told you yesterday — Across the Valley

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “told you yesterday” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “told you yesterday” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “Across the Valley” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “told you yesterday” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “told you yesterday” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — told you yesterday — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “told you yesterday.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “Across the Valley” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “told you yesterday” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “told you yesterday” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: Across the Valley × both be lying

**Beat question:** What can the writer say about “both be lying” during “Across the Valley” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: How much can distance make a reader think they know?

#### Scene draft 008 — both be lying — Across the Valley

For “Across the Valley” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “both be lying” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “Across the Valley” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A traveler who records claims separately a distinct perspective on “both be lying” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “Across the Valley” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “both be lying” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “both be lying” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — both be lying — Across the Valley

This proposed field-note fragment, beat 008 in “Across the Valley,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “both be lying.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “Across the Valley” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives A companion who wants a verdict a distinct perspective on “both be lying” during “Across the Valley.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “Across the Valley” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “both be lying” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “both be lying” during “Across the Valley” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — both be lying — Across the Valley

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “both be lying” during “Across the Valley,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “both be lying” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “Across the Valley” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “Across the Valley.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “Across the Valley” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “both be lying” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “both be lying” during “Across the Valley” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — both be lying — Across the Valley

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “Across the Valley” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “both be lying” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “Across the Valley” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A traveler who records claims separately a distinct perspective on “both be lying” during “Across the Valley.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “Across the Valley” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“How much can distance make a reader think they know?”—as a revision test tied to “both be lying” during “Across the Valley.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “both be lying” during “Across the Valley” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Across the Valley” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: One Smoke, One Cold Camp × two camps

**Beat question:** What can the writer say about “two camps” during “One Smoke, One Cold Camp” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 009 — two camps — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “two camps” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A traveler who records claims separately a distinct perspective on “two camps” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “One Smoke, One Cold Camp” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “two camps” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “two camps” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — two camps — One Smoke, One Cold Camp

This proposed field-note fragment, beat 009 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “two camps” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives A companion who wants a verdict a distinct perspective on “two camps” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “One Smoke, One Cold Camp” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “two camps” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “two camps” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — two camps — One Smoke, One Cold Camp

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “two camps” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “two camps.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “One Smoke, One Cold Camp” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “two camps” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “two camps” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — two camps — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “two camps” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A traveler who records claims separately a distinct perspective on “two camps” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “One Smoke, One Cold Camp” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “two camps” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “two camps” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: One Smoke, One Cold Camp × across a valley

**Beat question:** What can the writer say about “across a valley” during “One Smoke, One Cold Camp” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 010 — across a valley — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “across a valley.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “One Smoke, One Cold Camp” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “across a valley” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “across a valley” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — across a valley — One Smoke, One Cold Camp

This proposed field-note fragment, beat 010 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “across a valley” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A traveler who records claims separately a distinct perspective on “across a valley” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “One Smoke, One Cold Camp” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “across a valley” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “across a valley” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — across a valley — One Smoke, One Cold Camp

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “across a valley” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “across a valley” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives A companion who wants a verdict a distinct perspective on “across a valley” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “One Smoke, One Cold Camp” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “across a valley” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “across a valley” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — across a valley — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “across a valley” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “One Smoke, One Cold Camp” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “across a valley” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “across a valley” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: One Smoke, One Cold Camp × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “One Smoke, One Cold Camp” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 011 — same bleached white flag — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “same bleached white flag” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “One Smoke, One Cold Camp” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “same bleached white flag” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “same bleached white flag” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — same bleached white flag — One Smoke, One Cold Camp

This proposed field-note fragment, beat 011 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “same bleached white flag” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “One Smoke, One Cold Camp” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “same bleached white flag” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “same bleached white flag” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — same bleached white flag — One Smoke, One Cold Camp

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “same bleached white flag” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “same bleached white flag” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “One Smoke, One Cold Camp” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “same bleached white flag” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “same bleached white flag” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — same bleached white flag — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “same bleached white flag.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “One Smoke, One Cold Camp” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “same bleached white flag” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “same bleached white flag” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: One Smoke, One Cold Camp × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “One Smoke, One Cold Camp” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 012 — cooking smoke — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “cooking smoke” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “One Smoke, One Cold Camp” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cooking smoke” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “cooking smoke” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — cooking smoke — One Smoke, One Cold Camp

This proposed field-note fragment, beat 012 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “cooking smoke.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “One Smoke, One Cold Camp” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cooking smoke” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “cooking smoke” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — cooking smoke — One Smoke, One Cold Camp

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “cooking smoke” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “cooking smoke” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “One Smoke, One Cold Camp” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cooking smoke” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “cooking smoke” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — cooking smoke — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “cooking smoke” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “One Smoke, One Cold Camp” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cooking smoke” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “cooking smoke” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: One Smoke, One Cold Camp × cold camp

**Beat question:** What can the writer say about “cold camp” during “One Smoke, One Cold Camp” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 013 — cold camp — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “cold camp” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “One Smoke, One Cold Camp” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cold camp” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “cold camp” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — cold camp — One Smoke, One Cold Camp

This proposed field-note fragment, beat 013 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “cold camp” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A traveler who records claims separately a distinct perspective on “cold camp” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “One Smoke, One Cold Camp” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cold camp” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “cold camp” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — cold camp — One Smoke, One Cold Camp

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “cold camp” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “cold camp” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives A companion who wants a verdict a distinct perspective on “cold camp” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “One Smoke, One Cold Camp” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cold camp” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “cold camp” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — cold camp — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “cold camp” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “One Smoke, One Cold Camp” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “cold camp” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “cold camp” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: One Smoke, One Cold Camp × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “One Smoke, One Cold Camp” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 014 — ridge scout says — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “ridge scout says” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “One Smoke, One Cold Camp” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “ridge scout says” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “ridge scout says” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — ridge scout says — One Smoke, One Cold Camp

This proposed field-note fragment, beat 014 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “ridge scout says” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “One Smoke, One Cold Camp” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “ridge scout says” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “ridge scout says” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — ridge scout says — One Smoke, One Cold Camp

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “ridge scout says” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “ridge scout says.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “One Smoke, One Cold Camp” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “ridge scout says” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “ridge scout says” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — ridge scout says — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “ridge scout says” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “One Smoke, One Cold Camp” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “ridge scout says” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “ridge scout says” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: One Smoke, One Cold Camp × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “One Smoke, One Cold Camp” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 015 — told you yesterday — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “told you yesterday.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “One Smoke, One Cold Camp” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “told you yesterday” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “told you yesterday” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — told you yesterday — One Smoke, One Cold Camp

This proposed field-note fragment, beat 015 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “told you yesterday” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “One Smoke, One Cold Camp” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “told you yesterday” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “told you yesterday” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — told you yesterday — One Smoke, One Cold Camp

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “told you yesterday” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “told you yesterday” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “One Smoke, One Cold Camp” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “told you yesterday” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “told you yesterday” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — told you yesterday — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “told you yesterday” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “One Smoke, One Cold Camp” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “told you yesterday” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “told you yesterday” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: One Smoke, One Cold Camp × both be lying

**Beat question:** What can the writer say about “both be lying” during “One Smoke, One Cold Camp” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: Can an image remain an observation instead of a verdict?

#### Scene draft 016 — both be lying — One Smoke, One Cold Camp

For “One Smoke, One Cold Camp” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “both be lying” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “One Smoke, One Cold Camp” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “One Smoke, One Cold Camp” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “both be lying” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “both be lying” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — both be lying — One Smoke, One Cold Camp

This proposed field-note fragment, beat 016 in “One Smoke, One Cold Camp,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “both be lying” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “One Smoke, One Cold Camp” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A traveler who records claims separately a distinct perspective on “both be lying” during “One Smoke, One Cold Camp.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “One Smoke, One Cold Camp” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “both be lying” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “both be lying” during “One Smoke, One Cold Camp” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — both be lying — One Smoke, One Cold Camp

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “both be lying” during “One Smoke, One Cold Camp,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “both be lying” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “One Smoke, One Cold Camp” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives A companion who wants a verdict a distinct perspective on “both be lying” during “One Smoke, One Cold Camp.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “One Smoke, One Cold Camp” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “both be lying” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “both be lying” during “One Smoke, One Cold Camp” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — both be lying — One Smoke, One Cold Camp

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “One Smoke, One Cold Camp” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “both be lying.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “One Smoke, One Cold Camp” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “One Smoke, One Cold Camp.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “One Smoke, One Cold Camp” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“Can an image remain an observation instead of a verdict?”—as a revision test tied to “both be lying” during “One Smoke, One Cold Camp.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “both be lying” during “One Smoke, One Cold Camp” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Smoke, One Cold Camp” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: The Ridge Scout Speaks × two camps

**Beat question:** What can the writer say about “two camps” during “The Ridge Scout Speaks” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 017 — two camps — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “two camps” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “The Ridge Scout Speaks” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “two camps” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “two camps” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — two camps — The Ridge Scout Speaks

This proposed field-note fragment, beat 017 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “two camps” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A traveler who records claims separately a distinct perspective on “two camps” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “The Ridge Scout Speaks” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “two camps” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “two camps” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — two camps — The Ridge Scout Speaks

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “two camps” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “two camps” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives A companion who wants a verdict a distinct perspective on “two camps” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “The Ridge Scout Speaks” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “two camps” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “two camps” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — two camps — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “two camps” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “The Ridge Scout Speaks” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “two camps” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “two camps” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: The Ridge Scout Speaks × across a valley

**Beat question:** What can the writer say about “across a valley” during “The Ridge Scout Speaks” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 018 — across a valley — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “across a valley” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives A companion who wants a verdict a distinct perspective on “across a valley” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “The Ridge Scout Speaks” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “across a valley” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “across a valley” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — across a valley — The Ridge Scout Speaks

This proposed field-note fragment, beat 018 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “across a valley” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “The Ridge Scout Speaks” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “across a valley” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “across a valley” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — across a valley — The Ridge Scout Speaks

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “across a valley” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “across a valley.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A traveler who records claims separately a distinct perspective on “across a valley” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “The Ridge Scout Speaks” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “across a valley” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “across a valley” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — across a valley — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “across a valley” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives A companion who wants a verdict a distinct perspective on “across a valley” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “The Ridge Scout Speaks” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “across a valley” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “across a valley” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: The Ridge Scout Speaks × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “The Ridge Scout Speaks” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 019 — same bleached white flag — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “same bleached white flag.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “The Ridge Scout Speaks” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “same bleached white flag” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “same bleached white flag” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — same bleached white flag — The Ridge Scout Speaks

This proposed field-note fragment, beat 019 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “same bleached white flag” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “The Ridge Scout Speaks” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “same bleached white flag” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “same bleached white flag” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — same bleached white flag — The Ridge Scout Speaks

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “same bleached white flag” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “same bleached white flag” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “The Ridge Scout Speaks” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “same bleached white flag” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “same bleached white flag” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — same bleached white flag — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “same bleached white flag” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “The Ridge Scout Speaks” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “same bleached white flag” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “same bleached white flag” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: The Ridge Scout Speaks × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “The Ridge Scout Speaks” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 020 — cooking smoke — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “cooking smoke” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “The Ridge Scout Speaks” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cooking smoke” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “cooking smoke” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — cooking smoke — The Ridge Scout Speaks

This proposed field-note fragment, beat 020 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “cooking smoke” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “The Ridge Scout Speaks” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cooking smoke” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “cooking smoke” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — cooking smoke — The Ridge Scout Speaks

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “cooking smoke” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “cooking smoke” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “The Ridge Scout Speaks” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cooking smoke” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “cooking smoke” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — cooking smoke — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “cooking smoke.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “The Ridge Scout Speaks” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cooking smoke” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “cooking smoke” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: The Ridge Scout Speaks × cold camp

**Beat question:** What can the writer say about “cold camp” during “The Ridge Scout Speaks” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 021 — cold camp — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “cold camp” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives A companion who wants a verdict a distinct perspective on “cold camp” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “The Ridge Scout Speaks” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cold camp” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “cold camp” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — cold camp — The Ridge Scout Speaks

This proposed field-note fragment, beat 021 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “cold camp.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “The Ridge Scout Speaks” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cold camp” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “cold camp” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — cold camp — The Ridge Scout Speaks

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “cold camp” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “cold camp” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A traveler who records claims separately a distinct perspective on “cold camp” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “The Ridge Scout Speaks” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cold camp” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “cold camp” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — cold camp — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “cold camp” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives A companion who wants a verdict a distinct perspective on “cold camp” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “The Ridge Scout Speaks” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “cold camp” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “cold camp” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: The Ridge Scout Speaks × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “The Ridge Scout Speaks” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 022 — ridge scout says — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “ridge scout says” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “The Ridge Scout Speaks” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “ridge scout says” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “ridge scout says” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — ridge scout says — The Ridge Scout Speaks

This proposed field-note fragment, beat 022 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “ridge scout says” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “The Ridge Scout Speaks” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “ridge scout says” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “ridge scout says” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — ridge scout says — The Ridge Scout Speaks

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “ridge scout says” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “ridge scout says” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “The Ridge Scout Speaks” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “ridge scout says” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “ridge scout says” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — ridge scout says — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “ridge scout says” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “The Ridge Scout Speaks” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “ridge scout says” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “ridge scout says” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: The Ridge Scout Speaks × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “The Ridge Scout Speaks” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 023 — told you yesterday — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “told you yesterday” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “The Ridge Scout Speaks” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “told you yesterday” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “told you yesterday” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — told you yesterday — The Ridge Scout Speaks

This proposed field-note fragment, beat 023 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “told you yesterday” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “The Ridge Scout Speaks” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “told you yesterday” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “told you yesterday” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — told you yesterday — The Ridge Scout Speaks

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “told you yesterday” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “told you yesterday.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “The Ridge Scout Speaks” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “told you yesterday” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “told you yesterday” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — told you yesterday — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “told you yesterday” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “The Ridge Scout Speaks” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “told you yesterday” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “told you yesterday” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: The Ridge Scout Speaks × both be lying

**Beat question:** What can the writer say about “both be lying” during “The Ridge Scout Speaks” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: How should prose preserve that attribution?

#### Scene draft 024 — both be lying — The Ridge Scout Speaks

For “The Ridge Scout Speaks” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “both be lying.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “The Ridge Scout Speaks” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives A companion who wants a verdict a distinct perspective on “both be lying” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “The Ridge Scout Speaks” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“How should prose preserve that attribution?”—as a revision test tied to “both be lying” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “both be lying” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — both be lying — The Ridge Scout Speaks

This proposed field-note fragment, beat 024 in “The Ridge Scout Speaks,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “both be lying” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “The Ridge Scout Speaks” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “The Ridge Scout Speaks.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “The Ridge Scout Speaks” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “both be lying” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “both be lying” during “The Ridge Scout Speaks” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — both be lying — The Ridge Scout Speaks

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “both be lying” during “The Ridge Scout Speaks,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “both be lying” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “The Ridge Scout Speaks” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A traveler who records claims separately a distinct perspective on “both be lying” during “The Ridge Scout Speaks.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “The Ridge Scout Speaks” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“How should prose preserve that attribution?”—as a revision test tied to “both be lying” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “both be lying” during “The Ridge Scout Speaks” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — both be lying — The Ridge Scout Speaks

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “The Ridge Scout Speaks” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “both be lying” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “The Ridge Scout Speaks” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives A companion who wants a verdict a distinct perspective on “both be lying” during “The Ridge Scout Speaks.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “The Ridge Scout Speaks” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“How should prose preserve that attribution?”—as a revision test tied to “both be lying” during “The Ridge Scout Speaks.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “both be lying” during “The Ridge Scout Speaks” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Ridge Scout Speaks” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: Yesterday’s Counterclaim × two camps

**Beat question:** What can the writer say about “two camps” during “Yesterday’s Counterclaim” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 025 — two camps — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “two camps” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives A companion who wants a verdict a distinct perspective on “two camps” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “Yesterday’s Counterclaim” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “two camps” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “two camps” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — two camps — Yesterday’s Counterclaim

This proposed field-note fragment, beat 025 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “two camps.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “Yesterday’s Counterclaim” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “two camps” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “two camps” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — two camps — Yesterday’s Counterclaim

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “two camps” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “two camps” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A traveler who records claims separately a distinct perspective on “two camps” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “Yesterday’s Counterclaim” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “two camps” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “two camps” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — two camps — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “two camps” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives A companion who wants a verdict a distinct perspective on “two camps” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “Yesterday’s Counterclaim” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “two camps” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “two camps” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: Yesterday’s Counterclaim × across a valley

**Beat question:** What can the writer say about “across a valley” during “Yesterday’s Counterclaim” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 026 — across a valley — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “across a valley” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A traveler who records claims separately a distinct perspective on “across a valley” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “Yesterday’s Counterclaim” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “across a valley” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “across a valley” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — across a valley — Yesterday’s Counterclaim

This proposed field-note fragment, beat 026 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “across a valley” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives A companion who wants a verdict a distinct perspective on “across a valley” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “Yesterday’s Counterclaim” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “across a valley” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “across a valley” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — across a valley — Yesterday’s Counterclaim

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “across a valley” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “across a valley” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “Yesterday’s Counterclaim” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “across a valley” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “across a valley” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — across a valley — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “across a valley” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A traveler who records claims separately a distinct perspective on “across a valley” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “Yesterday’s Counterclaim” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “across a valley” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “across a valley” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: Yesterday’s Counterclaim × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “Yesterday’s Counterclaim” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 027 — same bleached white flag — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “same bleached white flag” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “Yesterday’s Counterclaim” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “same bleached white flag” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “same bleached white flag” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — same bleached white flag — Yesterday’s Counterclaim

This proposed field-note fragment, beat 027 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “same bleached white flag” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “Yesterday’s Counterclaim” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “same bleached white flag” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “same bleached white flag” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — same bleached white flag — Yesterday’s Counterclaim

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “same bleached white flag” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “same bleached white flag.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “Yesterday’s Counterclaim” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “same bleached white flag” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “same bleached white flag” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — same bleached white flag — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “same bleached white flag” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “Yesterday’s Counterclaim” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “same bleached white flag” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “same bleached white flag” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: Yesterday’s Counterclaim × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “Yesterday’s Counterclaim” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 028 — cooking smoke — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “cooking smoke.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “Yesterday’s Counterclaim” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cooking smoke” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “cooking smoke” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — cooking smoke — Yesterday’s Counterclaim

This proposed field-note fragment, beat 028 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “cooking smoke” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “Yesterday’s Counterclaim” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cooking smoke” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “cooking smoke” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — cooking smoke — Yesterday’s Counterclaim

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “cooking smoke” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “cooking smoke” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “Yesterday’s Counterclaim” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cooking smoke” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “cooking smoke” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — cooking smoke — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “cooking smoke” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “Yesterday’s Counterclaim” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cooking smoke” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “cooking smoke” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: Yesterday’s Counterclaim × cold camp

**Beat question:** What can the writer say about “cold camp” during “Yesterday’s Counterclaim” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 029 — cold camp — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “cold camp” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A traveler who records claims separately a distinct perspective on “cold camp” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “Yesterday’s Counterclaim” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cold camp” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “cold camp” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — cold camp — Yesterday’s Counterclaim

This proposed field-note fragment, beat 029 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “cold camp” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives A companion who wants a verdict a distinct perspective on “cold camp” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “Yesterday’s Counterclaim” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cold camp” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “cold camp” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — cold camp — Yesterday’s Counterclaim

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “cold camp” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “cold camp” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “Yesterday’s Counterclaim” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cold camp” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “cold camp” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — cold camp — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “cold camp.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A traveler who records claims separately a distinct perspective on “cold camp” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “Yesterday’s Counterclaim” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “cold camp” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “cold camp” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: Yesterday’s Counterclaim × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “Yesterday’s Counterclaim” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 030 — ridge scout says — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “ridge scout says” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “Yesterday’s Counterclaim” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “ridge scout says” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “ridge scout says” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — ridge scout says — Yesterday’s Counterclaim

This proposed field-note fragment, beat 030 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “ridge scout says.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “Yesterday’s Counterclaim” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “ridge scout says” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “ridge scout says” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — ridge scout says — Yesterday’s Counterclaim

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “ridge scout says” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “ridge scout says” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “Yesterday’s Counterclaim” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “ridge scout says” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “ridge scout says” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — ridge scout says — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “ridge scout says” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “Yesterday’s Counterclaim” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “ridge scout says” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “ridge scout says” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: Yesterday’s Counterclaim × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “Yesterday’s Counterclaim” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 031 — told you yesterday — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “told you yesterday” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “Yesterday’s Counterclaim” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “told you yesterday” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “told you yesterday” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — told you yesterday — Yesterday’s Counterclaim

This proposed field-note fragment, beat 031 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “told you yesterday” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “Yesterday’s Counterclaim” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “told you yesterday” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “told you yesterday” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — told you yesterday — Yesterday’s Counterclaim

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “told you yesterday” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “told you yesterday” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “Yesterday’s Counterclaim” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “told you yesterday” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “told you yesterday” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — told you yesterday — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “told you yesterday” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “Yesterday’s Counterclaim” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “told you yesterday” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “told you yesterday” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: Yesterday’s Counterclaim × both be lying

**Beat question:** What can the writer say about “both be lying” during “Yesterday’s Counterclaim” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: What changes when both stories are allowed to be strategic?

#### Scene draft 032 — both be lying — Yesterday’s Counterclaim

For “Yesterday’s Counterclaim” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “both be lying” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “Yesterday’s Counterclaim” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A traveler who records claims separately a distinct perspective on “both be lying” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “Yesterday’s Counterclaim” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “both be lying” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “both be lying” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — both be lying — Yesterday’s Counterclaim

This proposed field-note fragment, beat 032 in “Yesterday’s Counterclaim,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “both be lying” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “Yesterday’s Counterclaim” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives A companion who wants a verdict a distinct perspective on “both be lying” during “Yesterday’s Counterclaim.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “Yesterday’s Counterclaim” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “both be lying” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “both be lying” during “Yesterday’s Counterclaim” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — both be lying — Yesterday’s Counterclaim

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “both be lying” during “Yesterday’s Counterclaim,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “both be lying.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “Yesterday’s Counterclaim” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “Yesterday’s Counterclaim.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “Yesterday’s Counterclaim” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “both be lying” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “both be lying” during “Yesterday’s Counterclaim” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — both be lying — Yesterday’s Counterclaim

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “Yesterday’s Counterclaim” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “both be lying” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “Yesterday’s Counterclaim” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A traveler who records claims separately a distinct perspective on “both be lying” during “Yesterday’s Counterclaim.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “Yesterday’s Counterclaim” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“What changes when both stories are allowed to be strategic?”—as a revision test tied to “both be lying” during “Yesterday’s Counterclaim.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “both be lying” during “Yesterday’s Counterclaim” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Yesterday’s Counterclaim” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: The Shared Flag × two camps

**Beat question:** What can the writer say about “two camps” during “The Shared Flag” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 033 — two camps — The Shared Flag

For “The Shared Flag” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “two camps” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A traveler who records claims separately a distinct perspective on “two camps” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “The Shared Flag” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “two camps” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “two camps” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — two camps — The Shared Flag

This proposed field-note fragment, beat 033 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “two camps” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives A companion who wants a verdict a distinct perspective on “two camps” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “The Shared Flag” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “two camps” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “two camps” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — two camps — The Shared Flag

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “two camps” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “two camps” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “The Shared Flag” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “two camps” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “two camps” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — two camps — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “two camps.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A traveler who records claims separately a distinct perspective on “two camps” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “The Shared Flag” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “two camps” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “two camps” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: The Shared Flag × across a valley

**Beat question:** What can the writer say about “across a valley” during “The Shared Flag” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 034 — across a valley — The Shared Flag

For “The Shared Flag” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “across a valley” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “The Shared Flag” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “across a valley” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “across a valley” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — across a valley — The Shared Flag

This proposed field-note fragment, beat 034 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “across a valley.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A traveler who records claims separately a distinct perspective on “across a valley” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “The Shared Flag” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “across a valley” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “across a valley” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — across a valley — The Shared Flag

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “across a valley” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “across a valley” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives A companion who wants a verdict a distinct perspective on “across a valley” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “The Shared Flag” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “across a valley” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “across a valley” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — across a valley — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “across a valley” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “The Shared Flag” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “across a valley” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “across a valley” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: The Shared Flag × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “The Shared Flag” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 035 — same bleached white flag — The Shared Flag

For “The Shared Flag” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “same bleached white flag” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “The Shared Flag” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “same bleached white flag” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “same bleached white flag” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — same bleached white flag — The Shared Flag

This proposed field-note fragment, beat 035 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “same bleached white flag” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “The Shared Flag” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “same bleached white flag” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “same bleached white flag” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — same bleached white flag — The Shared Flag

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “same bleached white flag” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “same bleached white flag” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “The Shared Flag” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “same bleached white flag” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “same bleached white flag” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — same bleached white flag — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “same bleached white flag” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “The Shared Flag” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “same bleached white flag” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “same bleached white flag” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: The Shared Flag × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “The Shared Flag” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 036 — cooking smoke — The Shared Flag

For “The Shared Flag” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “cooking smoke” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “The Shared Flag” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cooking smoke” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “cooking smoke” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — cooking smoke — The Shared Flag

This proposed field-note fragment, beat 036 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “cooking smoke” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “The Shared Flag” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cooking smoke” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “cooking smoke” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — cooking smoke — The Shared Flag

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “cooking smoke” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “cooking smoke.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “The Shared Flag” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cooking smoke” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “cooking smoke” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — cooking smoke — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “cooking smoke” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “The Shared Flag” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cooking smoke” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “cooking smoke” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: The Shared Flag × cold camp

**Beat question:** What can the writer say about “cold camp” during “The Shared Flag” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 037 — cold camp — The Shared Flag

For “The Shared Flag” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “cold camp.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “The Shared Flag” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cold camp” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “cold camp” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — cold camp — The Shared Flag

This proposed field-note fragment, beat 037 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “cold camp” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A traveler who records claims separately a distinct perspective on “cold camp” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “The Shared Flag” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cold camp” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “cold camp” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — cold camp — The Shared Flag

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “cold camp” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “cold camp” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives A companion who wants a verdict a distinct perspective on “cold camp” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “The Shared Flag” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cold camp” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “cold camp” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — cold camp — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “cold camp” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “The Shared Flag” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “cold camp” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “cold camp” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: The Shared Flag × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “The Shared Flag” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 038 — ridge scout says — The Shared Flag

For “The Shared Flag” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “ridge scout says” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “The Shared Flag” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “ridge scout says” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “ridge scout says” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — ridge scout says — The Shared Flag

This proposed field-note fragment, beat 038 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “ridge scout says” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “The Shared Flag” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “ridge scout says” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “ridge scout says” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — ridge scout says — The Shared Flag

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “ridge scout says” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “ridge scout says” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “The Shared Flag” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “ridge scout says” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “ridge scout says” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — ridge scout says — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “ridge scout says.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “The Shared Flag” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “ridge scout says” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “ridge scout says” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: The Shared Flag × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “The Shared Flag” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 039 — told you yesterday — The Shared Flag

For “The Shared Flag” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “told you yesterday” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “The Shared Flag” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “told you yesterday” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “told you yesterday” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — told you yesterday — The Shared Flag

This proposed field-note fragment, beat 039 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “told you yesterday.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “The Shared Flag” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “told you yesterday” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “told you yesterday” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — told you yesterday — The Shared Flag

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “told you yesterday” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “told you yesterday” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “The Shared Flag” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “told you yesterday” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “told you yesterday” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — told you yesterday — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “told you yesterday” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “The Shared Flag” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “told you yesterday” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “told you yesterday” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: The Shared Flag × both be lying

**Beat question:** What can the writer say about “both be lying” during “The Shared Flag” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: How can a repeated symbol remain ambiguous?

#### Scene draft 040 — both be lying — The Shared Flag

For “The Shared Flag” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “both be lying” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “The Shared Flag” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “The Shared Flag” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “both be lying” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “both be lying” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — both be lying — The Shared Flag

This proposed field-note fragment, beat 040 in “The Shared Flag,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “both be lying” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “The Shared Flag” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A traveler who records claims separately a distinct perspective on “both be lying” during “The Shared Flag.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “The Shared Flag” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “both be lying” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “both be lying” during “The Shared Flag” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — both be lying — The Shared Flag

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “both be lying” during “The Shared Flag,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “both be lying” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “The Shared Flag” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives A companion who wants a verdict a distinct perspective on “both be lying” during “The Shared Flag.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “The Shared Flag” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “both be lying” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “both be lying” during “The Shared Flag” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — both be lying — The Shared Flag

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “The Shared Flag” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “both be lying” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “The Shared Flag” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “The Shared Flag.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “The Shared Flag” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“How can a repeated symbol remain ambiguous?”—as a revision test tied to “both be lying” during “The Shared Flag.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “both be lying” during “The Shared Flag” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “The Shared Flag” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: Detour, Wait, or Approach × two camps

**Beat question:** What can the writer say about “two camps” during “Detour, Wait, or Approach” while preserving this limit: a count that does not make either group a unified character. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 041 — two camps — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “two camps,” the candidate passage attends to A count that does not make either group a unified character. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “two camps.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “Detour, Wait, or Approach” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “two camps” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “two camps” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — two camps — Detour, Wait, or Approach

This proposed field-note fragment, beat 041 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two camps” is the point of return. A count that does not make either group a unified character. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “two camps” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A traveler who records claims separately a distinct perspective on “two camps” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “Detour, Wait, or Approach” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “two camps” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “two camps” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — two camps — Detour, Wait, or Approach

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “two camps” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “two camps” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives A companion who wants a verdict a distinct perspective on “two camps” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “Detour, Wait, or Approach” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “two camps” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “two camps” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — two camps — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two camps” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “two camps” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two camps” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives A late-arriving listener in a proposed return a distinct perspective on “two camps” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “Detour, Wait, or Approach” × “two camps,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “two camps” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “two camps” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two camps.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: Detour, Wait, or Approach × across a valley

**Beat question:** What can the writer say about “across a valley” during “Detour, Wait, or Approach” while preserving this limit: a viewing distance, not an exact map. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 042 — across a valley — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “across a valley,” the candidate passage attends to A viewing distance, not an exact map. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “across a valley” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives A companion who wants a verdict a distinct perspective on “across a valley” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “Detour, Wait, or Approach” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “across a valley” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “across a valley” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — across a valley — Detour, Wait, or Approach

This proposed field-note fragment, beat 042 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “across a valley” is the point of return. A viewing distance, not an exact map. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “across a valley” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives A late-arriving listener in a proposed return a distinct perspective on “across a valley” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “Detour, Wait, or Approach” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “across a valley” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “across a valley” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — across a valley — Detour, Wait, or Approach

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “across a valley” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “across a valley” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A traveler who records claims separately a distinct perspective on “across a valley” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “Detour, Wait, or Approach” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “across a valley” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “across a valley” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — across a valley — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “across a valley” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “across a valley.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “across a valley” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives A companion who wants a verdict a distinct perspective on “across a valley” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “Detour, Wait, or Approach” × “across a valley,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “across a valley” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “across a valley” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “across a valley.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: Detour, Wait, or Approach × same bleached white flag

**Beat question:** What can the writer say about “same bleached white flag” during “Detour, Wait, or Approach” while preserving this limit: an identical visible marker with no established shared organization. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 043 — same bleached white flag — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “same bleached white flag,” the candidate passage attends to An identical visible marker with no established shared organization. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “same bleached white flag” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “Detour, Wait, or Approach” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “same bleached white flag” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “same bleached white flag” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — same bleached white flag — Detour, Wait, or Approach

This proposed field-note fragment, beat 043 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “same bleached white flag” is the point of return. An identical visible marker with no established shared organization. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “same bleached white flag.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives A companion who wants a verdict a distinct perspective on “same bleached white flag” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “Detour, Wait, or Approach” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “same bleached white flag” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “same bleached white flag” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — same bleached white flag — Detour, Wait, or Approach

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “same bleached white flag” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “same bleached white flag” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives A late-arriving listener in a proposed return a distinct perspective on “same bleached white flag” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “Detour, Wait, or Approach” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “same bleached white flag” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “same bleached white flag” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — same bleached white flag — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “same bleached white flag” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “same bleached white flag” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “same bleached white flag” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A traveler who records claims separately a distinct perspective on “same bleached white flag” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “Detour, Wait, or Approach” × “same bleached white flag,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “same bleached white flag” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “same bleached white flag” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “same bleached white flag.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: Detour, Wait, or Approach × cooking smoke

**Beat question:** What can the writer say about “cooking smoke” during “Detour, Wait, or Approach” while preserving this limit: a sign of activity, not proof of benevolence. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 044 — cooking smoke — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “cooking smoke,” the candidate passage attends to A sign of activity, not proof of benevolence. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “cooking smoke” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “Detour, Wait, or Approach” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cooking smoke” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “cooking smoke” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — cooking smoke — Detour, Wait, or Approach

This proposed field-note fragment, beat 044 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cooking smoke” is the point of return. A sign of activity, not proof of benevolence. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “cooking smoke” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A traveler who records claims separately a distinct perspective on “cooking smoke” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “Detour, Wait, or Approach” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cooking smoke” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “cooking smoke” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — cooking smoke — Detour, Wait, or Approach

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “cooking smoke” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “cooking smoke” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives A companion who wants a verdict a distinct perspective on “cooking smoke” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “Detour, Wait, or Approach” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cooking smoke” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “cooking smoke” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — cooking smoke — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cooking smoke” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “cooking smoke” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cooking smoke” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives A late-arriving listener in a proposed return a distinct perspective on “cooking smoke” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “Detour, Wait, or Approach” × “cooking smoke,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cooking smoke” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “cooking smoke” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cooking smoke.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: Detour, Wait, or Approach × cold camp

**Beat question:** What can the writer say about “cold camp” during “Detour, Wait, or Approach” while preserving this limit: a description in one choice, not a verified lack of people or supplies. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 045 — cold camp — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “cold camp,” the candidate passage attends to A description in one choice, not a verified lack of people or supplies. The present action begins small: the white cloth changing shape in the same wind. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “cold camp” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives A companion who wants a verdict a distinct perspective on “cold camp” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “Detour, Wait, or Approach” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cold camp” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “cold camp” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — cold camp — Detour, Wait, or Approach

This proposed field-note fragment, beat 045 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “cold camp” is the point of return. A description in one choice, not a verified lack of people or supplies. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “cold camp” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives A late-arriving listener in a proposed return a distinct perspective on “cold camp” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “Detour, Wait, or Approach” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cold camp” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “cold camp” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — cold camp — Detour, Wait, or Approach

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “cold camp” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “cold camp.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A traveler who records claims separately a distinct perspective on “cold camp” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “Detour, Wait, or Approach” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cold camp” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “cold camp” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — cold camp — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “cold camp” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “cold camp” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “cold camp” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives A companion who wants a verdict a distinct perspective on “cold camp” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “Detour, Wait, or Approach” × “cold camp,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “cold camp” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “cold camp” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “cold camp.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: Detour, Wait, or Approach × ridge scout says

**Beat question:** What can the writer say about “ridge scout says” during “Detour, Wait, or Approach” while preserving this limit: an attributed claim rather than narrator fact. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 046 — ridge scout says — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “ridge scout says,” the candidate passage attends to An attributed claim rather than narrator fact. The present action begins small: two versions written on separate lines with their speakers named. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “ridge scout says.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “Detour, Wait, or Approach” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “ridge scout says” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “ridge scout says” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — ridge scout says — Detour, Wait, or Approach

This proposed field-note fragment, beat 046 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “ridge scout says” is the point of return. An attributed claim rather than narrator fact. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “ridge scout says” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives A companion who wants a verdict a distinct perspective on “ridge scout says” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “Detour, Wait, or Approach” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “ridge scout says” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “ridge scout says” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — ridge scout says — Detour, Wait, or Approach

The proposed exchange gives a late-arriving listener in a proposed return a distinct reason to speak. Its authoring note is: “Questions whether the record preserves who said what.” The talk concerns “ridge scout says” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “ridge scout says” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives A late-arriving listener in a proposed return a distinct perspective on “ridge scout says” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “Detour, Wait, or Approach” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “A flag can match and the people beneath it can still disagree.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “ridge scout says” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “ridge scout says” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — ridge scout says — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “ridge scout says” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “ridge scout says” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “ridge scout says” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A traveler who records claims separately a distinct perspective on “ridge scout says” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “Detour, Wait, or Approach” × “ridge scout says,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “ridge scout says” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “ridge scout says” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “ridge scout says.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: Detour, Wait, or Approach × told you yesterday

**Beat question:** What can the writer say about “told you yesterday” during “Detour, Wait, or Approach” while preserving this limit: a second account with its own timing and speaker. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 047 — told you yesterday — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “told you yesterday,” the candidate passage attends to A second account with its own timing and speaker. The present action begins small: a line of sight interrupted by ordinary distance. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “told you yesterday” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “Detour, Wait, or Approach” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “told you yesterday” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “told you yesterday” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — told you yesterday — Detour, Wait, or Approach

This proposed field-note fragment, beat 047 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “told you yesterday” is the point of return. A second account with its own timing and speaker. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “told you yesterday” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A traveler who records claims separately a distinct perspective on “told you yesterday” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “Detour, Wait, or Approach” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “told you yesterday” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “told you yesterday” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — told you yesterday — Detour, Wait, or Approach

The proposed exchange gives a companion who wants a verdict a distinct reason to speak. Its authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” The talk concerns “told you yesterday” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “told you yesterday” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives A companion who wants a verdict a distinct perspective on “told you yesterday” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “Detour, Wait, or Approach” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “I heard that from the ridge scout. I did not see it happen.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “told you yesterday” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “told you yesterday” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — told you yesterday — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “told you yesterday” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “told you yesterday.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “told you yesterday” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives A late-arriving listener in a proposed return a distinct perspective on “told you yesterday” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “Detour, Wait, or Approach” × “told you yesterday,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “told you yesterday” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “told you yesterday” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “told you yesterday.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: Detour, Wait, or Approach × both be lying

**Beat question:** What can the writer say about “both be lying” during “Detour, Wait, or Approach” while preserving this limit: an explicit opening for uncertainty, not a new conclusion. The larger movement question is: What does a decision cost when knowledge cannot be finished?

#### Scene draft 048 — both be lying — Detour, Wait, or Approach

For “Detour, Wait, or Approach” and the source phrase “both be lying,” the candidate passage attends to An explicit opening for uncertainty, not a new conclusion. The present action begins small: a traveler pausing before turning an allegation into a warning. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “both be lying” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “Detour, Wait, or Approach” scene draft, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives A companion who wants a verdict a distinct perspective on “both be lying” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “Detour, Wait, or Approach” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “both be lying” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “both be lying” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — both be lying — Detour, Wait, or Approach

This proposed field-note fragment, beat 048 in “Detour, Wait, or Approach,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “both be lying” is the point of return. An explicit opening for uncertainty, not a new conclusion. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “both be lying.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “Detour, Wait, or Approach” field-note fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives A late-arriving listener in a proposed return a distinct perspective on “both be lying” during “Detour, Wait, or Approach.” The optional authoring note is: “Questions whether the record preserves who said what.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “Detour, Wait, or Approach” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “Write both claims down. Do not merge them into one fact.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “both be lying” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “both be lying” during “Detour, Wait, or Approach” in a changed register: “I heard that from the ridge scout. I did not see it happen.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — both be lying — Detour, Wait, or Approach

The proposed exchange gives a traveler who records claims separately a distinct reason to speak. Its authoring note is: “Does not use a shared flag as proof of shared intent.” The talk concerns “both be lying” during “Detour, Wait, or Approach,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “both be lying” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “Detour, Wait, or Approach” conversation fragment, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A traveler who records claims separately a distinct perspective on “both be lying” during “Detour, Wait, or Approach.” The optional authoring note is: “Does not use a shared flag as proof of shared intent.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “Detour, Wait, or Approach” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “I heard that from the ridge scout. I did not see it happen.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Write both claims down. Do not merge them into one fact.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “both be lying” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “both be lying” during “Detour, Wait, or Approach” in a changed register: “A flag can match and the people beneath it can still disagree.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — both be lying — Detour, Wait, or Approach

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “both be lying” through “Detour, Wait, or Approach” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “both be lying” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “Detour, Wait, or Approach” conditional return vignette, is narrow. The local description says: “Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “both be lying” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives A companion who wants a verdict a distinct perspective on “both be lying” during “Detour, Wait, or Approach.” The optional authoring note is: “Feels the pressure of choosing and can say so without becoming foolish.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “Detour, Wait, or Approach” × “both be lying,” a possible line, offered as newly authored dialogue rather than canon, is: “A flag can match and the people beneath it can still disagree.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“What does a decision cost when knowledge cannot be finished?”—as a revision test tied to “both be lying” during “Detour, Wait, or Approach.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “both be lying” during “Detour, Wait, or Approach” in a changed register: “Write both claims down. Do not merge them into one fact.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Detour, Wait, or Approach” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “both be lying.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_two_camps`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

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

The proposal is local to `enc_two_camps` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_two_camps` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | Catalog presence does not by itself show where prose is presented. |
| Description | Two camps are pitched across a valley, both flying the same bleached white flag. One camp has cooking smoke. The other does not. A scout on the ridge says the smoking camp is a raider ambush; a scout from the smoking camp told you yesterday the ridge camp is a cannibal den. They can't both be telling the truth. But they could both be lying. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_two_camps` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.