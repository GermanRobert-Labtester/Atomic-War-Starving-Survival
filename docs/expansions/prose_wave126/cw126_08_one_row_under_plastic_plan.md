# EXPANSION CW126-08 — One Row Under Plastic

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_greenhouse_keeper` — The Greenhouse Keeper.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`.
**Thesis:** A barter encounter about a keeper’s ongoing work and the small, negotiated future represented by a single row of pale seedlings.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A barter encounter about a keeper’s ongoing work and the small, negotiated future represented by a single row of pale seedlings. The plan builds an optional scene bank around the exact local description “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

How can a scene make the seedlings matter without promising harvest, yield, or a new agriculture system?

## 3. Verified source record

The source record contains these exact fields: id: "enc_greenhouse_keeper"; title: "The Greenhouse Keeper"; description: "A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt."; category: "Observation"; baseWeight: 2.0; stealthWeightMultiplier: 1.0; speedWeightMultiplier: 1.0; minDangerLevel: 0.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "seedlings_for_salt", "text": "Trade mineral salt for the seedlings.", "moraleDelta": 4, "guiltDelta": 0}, {"choiceId": "labor_for_seedlings", "text": "Offer manual labor repairing the plastic in exchange for the seedlings.", "moraleDelta": 3, "guiltDelta": 0}, {"choiceId": "take_seedlings", "text": "Raise your weapon. Demand the seedlings.", "moraleDelta": -3, "guiltDelta": 7}, {"choiceId": "leave_and_report", "text": "Leave without trading. Note the location for the shelter log.", "moraleDelta": 2, "guiltDelta": 2}]. Source: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | `enc_greenhouse_keeper` | title=The Greenhouse Keeper; category=Observation; description=A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `seedlings_for_salt` — “Trade mineral salt for the seedlings.” (moraleDelta 4, guiltDelta 0)
- `labor_for_seedlings` — “Offer manual labor repairing the plastic in exchange for the seedlings.” (moraleDelta 3, guiltDelta 0)
- `take_seedlings` — “Raise your weapon. Demand the seedlings.” (moraleDelta -3, guiltDelta 7)
- `leave_and_report` — “Leave without trading. Note the location for the shelter log.” (moraleDelta 2, guiltDelta 2)

## 4. Fixed canon and open space

Do not name a crop, add growing instructions, promise yield, diagnose the plants, or explain why the keeper remains. Do not give weapon handling or combat tactics. Do not write coercion as a clever route; the source choice to demand seedlings with a weapon has negative morale and high guilt deltas, but those values are not a universal moral verdict. Keep the keeper’s offer and refusal agency legible.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This record is present in the expansion JSON, but current NarrativeEncounterCatalogLoader loads narrative_encounters.json, narrative_encounters_npc_arcs.json, and micro_locations.json. ContentUtilizationScanner references are static mapping declarations, not proof that this expansion file is loaded. Treat every passage below as editorial and currently unverified for runtime reachability.

## 5. Human center

A municipal greenhouse is shattered, one corner is warm under thick plastic, a woman tends one row of pale green seedlings, and she offers seedlings for mineral salt. Her stance beside a shotgun signals a boundary but not her biography.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A traveler who wants to bargain:** Makes a clear offer and accepts that the keeper may decline.
- **The keeper, in optional authored dialogue:** Can name terms and stop the conversation without disclosing a name or history.
- **A companion who sees only the future crop:** Learns not to turn a fragile row into a guaranteed promise.

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

The content anchor is `enc_greenhouse_keeper` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: Municipal Glass Broken

The greenhouse is damaged, but the scene does not need to tour every pane. The movement asks: What does one intact corner mean against the larger ruin? Its source handle is “shattered municipal greenhouse”: A damaged public structure whose full history is unknown. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: Warmth Under Plastic

A small microclimate persists beneath thick sheeting. The movement asks: How can warmth be sensed without adding measurements or technical details? Its source handle is “thick plastic sheeting”: A source detail that defines the sheltered corner, not a how-to. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: One Row of Pale Green

The seedlings remain singular and fragile in the record. The movement asks: What future can a scene imply without guaranteeing a harvest? Its source handle is “warm microclimate”: A felt contrast, not a measured growing environment. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: A Keeper at Work

The woman tends the row and leans on a shotgun. The movement asks: How do work and boundary share the frame without turning her into a threat? Its source handle is “single row”: A count that keeps the scene small. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: Salt for Seedlings

She proposes an exchange, giving the interaction its terms. The movement asks: Can dialogue honor barter without inventing a full market? Its source handle is “pale green seedlings”: A color and stage, not a crop identity. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: Leaving the Row with Its Keeper

Trading, offering labor, demanding, or leaving appear in the source choices. The movement asks: How does the closing preserve the keeper’s ownership of what remains? Its source handle is “woman is tending”: An ongoing action and a role in the present. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: Municipal Glass Broken × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “Municipal Glass Broken” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 001 — shattered municipal greenhouse — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “shattered municipal greenhouse.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “Municipal Glass Broken” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “shattered municipal greenhouse” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “shattered municipal greenhouse” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — shattered municipal greenhouse — Municipal Glass Broken

This proposed field-note fragment, beat 001 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “shattered municipal greenhouse” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “Municipal Glass Broken” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “shattered municipal greenhouse” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “shattered municipal greenhouse” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — shattered municipal greenhouse — Municipal Glass Broken

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “shattered municipal greenhouse” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “shattered municipal greenhouse” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “Municipal Glass Broken” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “shattered municipal greenhouse” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “shattered municipal greenhouse” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — shattered municipal greenhouse — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “shattered municipal greenhouse” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “Municipal Glass Broken” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “shattered municipal greenhouse” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “shattered municipal greenhouse” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: Municipal Glass Broken × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “Municipal Glass Broken” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 002 — thick plastic sheeting — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “thick plastic sheeting” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “Municipal Glass Broken” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “thick plastic sheeting” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “thick plastic sheeting” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — thick plastic sheeting — Municipal Glass Broken

This proposed field-note fragment, beat 002 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “thick plastic sheeting” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “Municipal Glass Broken” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “thick plastic sheeting” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “thick plastic sheeting” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — thick plastic sheeting — Municipal Glass Broken

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “thick plastic sheeting” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “thick plastic sheeting” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “Municipal Glass Broken” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “thick plastic sheeting” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “thick plastic sheeting” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — thick plastic sheeting — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “thick plastic sheeting.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “Municipal Glass Broken” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “thick plastic sheeting” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “thick plastic sheeting” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: Municipal Glass Broken × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “Municipal Glass Broken” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 003 — warm microclimate — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “warm microclimate” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “Municipal Glass Broken” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “warm microclimate” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “warm microclimate” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — warm microclimate — Municipal Glass Broken

This proposed field-note fragment, beat 003 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “warm microclimate.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “Municipal Glass Broken” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “warm microclimate” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “warm microclimate” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — warm microclimate — Municipal Glass Broken

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “warm microclimate” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “warm microclimate” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “Municipal Glass Broken” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “warm microclimate” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “warm microclimate” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — warm microclimate — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “warm microclimate” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “Municipal Glass Broken” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “warm microclimate” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “warm microclimate” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: Municipal Glass Broken × single row

**Beat question:** What can the writer say about “single row” during “Municipal Glass Broken” while preserving this limit: a count that keeps the scene small. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 004 — single row — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “single row” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “Municipal Glass Broken” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “single row” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “single row” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — single row — Municipal Glass Broken

This proposed field-note fragment, beat 004 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “single row” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives A companion who sees only the future crop a distinct perspective on “single row” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “Municipal Glass Broken” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “single row” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “single row” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — single row — Municipal Glass Broken

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “single row” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “single row” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A traveler who wants to bargain a distinct perspective on “single row” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “Municipal Glass Broken” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “single row” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “single row” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — single row — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “single row” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “Municipal Glass Broken” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “single row” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “single row” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: Municipal Glass Broken × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “Municipal Glass Broken” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 005 — pale green seedlings — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “pale green seedlings” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “Municipal Glass Broken” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “pale green seedlings” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “pale green seedlings” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — pale green seedlings — Municipal Glass Broken

This proposed field-note fragment, beat 005 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “pale green seedlings” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “Municipal Glass Broken” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “pale green seedlings” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “pale green seedlings” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — pale green seedlings — Municipal Glass Broken

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “pale green seedlings” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “pale green seedlings.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “Municipal Glass Broken” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “pale green seedlings” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “pale green seedlings” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — pale green seedlings — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “pale green seedlings” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “Municipal Glass Broken” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “pale green seedlings” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “pale green seedlings” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: Municipal Glass Broken × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “Municipal Glass Broken” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 006 — woman is tending — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “woman is tending.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “Municipal Glass Broken” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “woman is tending” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “woman is tending” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — woman is tending — Municipal Glass Broken

This proposed field-note fragment, beat 006 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “woman is tending” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “Municipal Glass Broken” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “woman is tending” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “woman is tending” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — woman is tending — Municipal Glass Broken

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “woman is tending” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “woman is tending” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “Municipal Glass Broken” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “woman is tending” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “woman is tending” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — woman is tending — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “woman is tending” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “Municipal Glass Broken” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “woman is tending” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “woman is tending” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: Municipal Glass Broken × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “Municipal Glass Broken” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 007 — double-barreled shotgun — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “double-barreled shotgun” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “Municipal Glass Broken” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “double-barreled shotgun” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “double-barreled shotgun” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — double-barreled shotgun — Municipal Glass Broken

This proposed field-note fragment, beat 007 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “double-barreled shotgun” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “Municipal Glass Broken” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “double-barreled shotgun” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “double-barreled shotgun” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — double-barreled shotgun — Municipal Glass Broken

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “double-barreled shotgun” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “double-barreled shotgun” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “Municipal Glass Broken” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “double-barreled shotgun” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “double-barreled shotgun” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — double-barreled shotgun — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “double-barreled shotgun.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “Municipal Glass Broken” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “double-barreled shotgun” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “double-barreled shotgun” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: Municipal Glass Broken × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “Municipal Glass Broken” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: What does one intact corner mean against the larger ruin?

#### Scene draft 008 — seedlings for mineral salt — Municipal Glass Broken

For “Municipal Glass Broken” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “seedlings for mineral salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “Municipal Glass Broken” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “Municipal Glass Broken” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “seedlings for mineral salt” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “seedlings for mineral salt” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — seedlings for mineral salt — Municipal Glass Broken

This proposed field-note fragment, beat 008 in “Municipal Glass Broken,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “seedlings for mineral salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “Municipal Glass Broken” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “Municipal Glass Broken.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “Municipal Glass Broken” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “seedlings for mineral salt” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “seedlings for mineral salt” during “Municipal Glass Broken” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — seedlings for mineral salt — Municipal Glass Broken

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “seedlings for mineral salt” during “Municipal Glass Broken,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “seedlings for mineral salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “Municipal Glass Broken” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Municipal Glass Broken.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “Municipal Glass Broken” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “seedlings for mineral salt” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “seedlings for mineral salt” during “Municipal Glass Broken” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — seedlings for mineral salt — Municipal Glass Broken

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “Municipal Glass Broken” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “seedlings for mineral salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “Municipal Glass Broken” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “Municipal Glass Broken.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “Municipal Glass Broken” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“What does one intact corner mean against the larger ruin?”—as a revision test tied to “seedlings for mineral salt” during “Municipal Glass Broken.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “seedlings for mineral salt” during “Municipal Glass Broken” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Municipal Glass Broken” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: Warmth Under Plastic × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “Warmth Under Plastic” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 009 — shattered municipal greenhouse — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “shattered municipal greenhouse” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “Warmth Under Plastic” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “shattered municipal greenhouse” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “shattered municipal greenhouse” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — shattered municipal greenhouse — Warmth Under Plastic

This proposed field-note fragment, beat 009 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “shattered municipal greenhouse” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “Warmth Under Plastic” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “shattered municipal greenhouse” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “shattered municipal greenhouse” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — shattered municipal greenhouse — Warmth Under Plastic

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “shattered municipal greenhouse” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “shattered municipal greenhouse.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “Warmth Under Plastic” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “shattered municipal greenhouse” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “shattered municipal greenhouse” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — shattered municipal greenhouse — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “shattered municipal greenhouse” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “Warmth Under Plastic” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “shattered municipal greenhouse” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “shattered municipal greenhouse” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: Warmth Under Plastic × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “Warmth Under Plastic” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 010 — thick plastic sheeting — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “thick plastic sheeting.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “Warmth Under Plastic” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “thick plastic sheeting” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “thick plastic sheeting” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — thick plastic sheeting — Warmth Under Plastic

This proposed field-note fragment, beat 010 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “thick plastic sheeting” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “Warmth Under Plastic” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “thick plastic sheeting” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “thick plastic sheeting” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — thick plastic sheeting — Warmth Under Plastic

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “thick plastic sheeting” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “thick plastic sheeting” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “Warmth Under Plastic” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “thick plastic sheeting” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “thick plastic sheeting” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — thick plastic sheeting — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “thick plastic sheeting” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “Warmth Under Plastic” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “thick plastic sheeting” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “thick plastic sheeting” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: Warmth Under Plastic × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “Warmth Under Plastic” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 011 — warm microclimate — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “warm microclimate” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “Warmth Under Plastic” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “warm microclimate” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “warm microclimate” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — warm microclimate — Warmth Under Plastic

This proposed field-note fragment, beat 011 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “warm microclimate” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “Warmth Under Plastic” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “warm microclimate” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “warm microclimate” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — warm microclimate — Warmth Under Plastic

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “warm microclimate” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “warm microclimate” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “Warmth Under Plastic” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “warm microclimate” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “warm microclimate” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — warm microclimate — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “warm microclimate.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “Warmth Under Plastic” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “warm microclimate” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “warm microclimate” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: Warmth Under Plastic × single row

**Beat question:** What can the writer say about “single row” during “Warmth Under Plastic” while preserving this limit: a count that keeps the scene small. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 012 — single row — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “single row” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A traveler who wants to bargain a distinct perspective on “single row” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “Warmth Under Plastic” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “single row” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “single row” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — single row — Warmth Under Plastic

This proposed field-note fragment, beat 012 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “single row.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “Warmth Under Plastic” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “single row” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “single row” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — single row — Warmth Under Plastic

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “single row” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “single row” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives A companion who sees only the future crop a distinct perspective on “single row” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “Warmth Under Plastic” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “single row” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “single row” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — single row — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “single row” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A traveler who wants to bargain a distinct perspective on “single row” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “Warmth Under Plastic” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “single row” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “single row” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: Warmth Under Plastic × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “Warmth Under Plastic” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 013 — pale green seedlings — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “pale green seedlings” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “Warmth Under Plastic” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “pale green seedlings” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “pale green seedlings” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — pale green seedlings — Warmth Under Plastic

This proposed field-note fragment, beat 013 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “pale green seedlings” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “Warmth Under Plastic” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “pale green seedlings” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “pale green seedlings” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — pale green seedlings — Warmth Under Plastic

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “pale green seedlings” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “pale green seedlings” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “Warmth Under Plastic” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “pale green seedlings” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “pale green seedlings” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — pale green seedlings — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “pale green seedlings” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “Warmth Under Plastic” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “pale green seedlings” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “pale green seedlings” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: Warmth Under Plastic × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “Warmth Under Plastic” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 014 — woman is tending — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “woman is tending” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “Warmth Under Plastic” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “woman is tending” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “woman is tending” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — woman is tending — Warmth Under Plastic

This proposed field-note fragment, beat 014 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “woman is tending” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “Warmth Under Plastic” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “woman is tending” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “woman is tending” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — woman is tending — Warmth Under Plastic

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “woman is tending” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “woman is tending.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “Warmth Under Plastic” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “woman is tending” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “woman is tending” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — woman is tending — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “woman is tending” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “Warmth Under Plastic” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “woman is tending” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “woman is tending” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: Warmth Under Plastic × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “Warmth Under Plastic” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 015 — double-barreled shotgun — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “double-barreled shotgun.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “Warmth Under Plastic” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “double-barreled shotgun” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “double-barreled shotgun” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — double-barreled shotgun — Warmth Under Plastic

This proposed field-note fragment, beat 015 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “double-barreled shotgun” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “Warmth Under Plastic” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “double-barreled shotgun” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “double-barreled shotgun” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — double-barreled shotgun — Warmth Under Plastic

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “double-barreled shotgun” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “double-barreled shotgun” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “Warmth Under Plastic” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “double-barreled shotgun” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “double-barreled shotgun” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — double-barreled shotgun — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “double-barreled shotgun” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “Warmth Under Plastic” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “double-barreled shotgun” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “double-barreled shotgun” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: Warmth Under Plastic × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “Warmth Under Plastic” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: How can warmth be sensed without adding measurements or technical details?

#### Scene draft 016 — seedlings for mineral salt — Warmth Under Plastic

For “Warmth Under Plastic” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “seedlings for mineral salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “Warmth Under Plastic” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “Warmth Under Plastic” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “seedlings for mineral salt” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “seedlings for mineral salt” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — seedlings for mineral salt — Warmth Under Plastic

This proposed field-note fragment, beat 016 in “Warmth Under Plastic,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “seedlings for mineral salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “Warmth Under Plastic” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “Warmth Under Plastic.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “Warmth Under Plastic” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “seedlings for mineral salt” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “seedlings for mineral salt” during “Warmth Under Plastic” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — seedlings for mineral salt — Warmth Under Plastic

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “seedlings for mineral salt” during “Warmth Under Plastic,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “seedlings for mineral salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “Warmth Under Plastic” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “Warmth Under Plastic.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “Warmth Under Plastic” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “seedlings for mineral salt” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “seedlings for mineral salt” during “Warmth Under Plastic” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — seedlings for mineral salt — Warmth Under Plastic

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “Warmth Under Plastic” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “seedlings for mineral salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “Warmth Under Plastic” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Warmth Under Plastic.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “Warmth Under Plastic” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“How can warmth be sensed without adding measurements or technical details?”—as a revision test tied to “seedlings for mineral salt” during “Warmth Under Plastic.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “seedlings for mineral salt” during “Warmth Under Plastic” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Warmth Under Plastic” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: One Row of Pale Green × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “One Row of Pale Green” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 017 — shattered municipal greenhouse — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “shattered municipal greenhouse” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “One Row of Pale Green” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “shattered municipal greenhouse” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “shattered municipal greenhouse” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — shattered municipal greenhouse — One Row of Pale Green

This proposed field-note fragment, beat 017 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “shattered municipal greenhouse” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “One Row of Pale Green” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “shattered municipal greenhouse” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “shattered municipal greenhouse” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — shattered municipal greenhouse — One Row of Pale Green

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “shattered municipal greenhouse” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “shattered municipal greenhouse” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “One Row of Pale Green” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “shattered municipal greenhouse” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “shattered municipal greenhouse” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — shattered municipal greenhouse — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “shattered municipal greenhouse” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “One Row of Pale Green” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “shattered municipal greenhouse” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “shattered municipal greenhouse” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: One Row of Pale Green × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “One Row of Pale Green” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 018 — thick plastic sheeting — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “thick plastic sheeting” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “One Row of Pale Green” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “thick plastic sheeting” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “thick plastic sheeting” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — thick plastic sheeting — One Row of Pale Green

This proposed field-note fragment, beat 018 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “thick plastic sheeting” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “One Row of Pale Green” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “thick plastic sheeting” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “thick plastic sheeting” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — thick plastic sheeting — One Row of Pale Green

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “thick plastic sheeting” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “thick plastic sheeting.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “One Row of Pale Green” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “thick plastic sheeting” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “thick plastic sheeting” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — thick plastic sheeting — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “thick plastic sheeting” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “One Row of Pale Green” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “thick plastic sheeting” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “thick plastic sheeting” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: One Row of Pale Green × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “One Row of Pale Green” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 019 — warm microclimate — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “warm microclimate.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “One Row of Pale Green” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “warm microclimate” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “warm microclimate” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — warm microclimate — One Row of Pale Green

This proposed field-note fragment, beat 019 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “warm microclimate” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “One Row of Pale Green” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “warm microclimate” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “warm microclimate” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — warm microclimate — One Row of Pale Green

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “warm microclimate” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “warm microclimate” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “One Row of Pale Green” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “warm microclimate” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “warm microclimate” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — warm microclimate — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “warm microclimate” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “One Row of Pale Green” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “warm microclimate” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “warm microclimate” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: One Row of Pale Green × single row

**Beat question:** What can the writer say about “single row” during “One Row of Pale Green” while preserving this limit: a count that keeps the scene small. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 020 — single row — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “single row” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives A companion who sees only the future crop a distinct perspective on “single row” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “One Row of Pale Green” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “single row” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “single row” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — single row — One Row of Pale Green

This proposed field-note fragment, beat 020 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “single row” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A traveler who wants to bargain a distinct perspective on “single row” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “One Row of Pale Green” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “single row” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “single row” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — single row — One Row of Pale Green

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “single row” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “single row” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “One Row of Pale Green” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “single row” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “single row” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — single row — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “single row.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives A companion who sees only the future crop a distinct perspective on “single row” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “One Row of Pale Green” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “single row” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “single row” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: One Row of Pale Green × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “One Row of Pale Green” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 021 — pale green seedlings — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “pale green seedlings” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “One Row of Pale Green” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “pale green seedlings” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “pale green seedlings” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — pale green seedlings — One Row of Pale Green

This proposed field-note fragment, beat 021 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “pale green seedlings.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “One Row of Pale Green” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “pale green seedlings” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “pale green seedlings” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — pale green seedlings — One Row of Pale Green

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “pale green seedlings” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “pale green seedlings” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “One Row of Pale Green” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “pale green seedlings” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “pale green seedlings” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — pale green seedlings — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “pale green seedlings” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “One Row of Pale Green” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “pale green seedlings” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “pale green seedlings” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: One Row of Pale Green × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “One Row of Pale Green” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 022 — woman is tending — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “woman is tending” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “One Row of Pale Green” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “woman is tending” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “woman is tending” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — woman is tending — One Row of Pale Green

This proposed field-note fragment, beat 022 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “woman is tending” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “One Row of Pale Green” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “woman is tending” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “woman is tending” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — woman is tending — One Row of Pale Green

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “woman is tending” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “woman is tending” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “One Row of Pale Green” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “woman is tending” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “woman is tending” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — woman is tending — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “woman is tending” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “One Row of Pale Green” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “woman is tending” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “woman is tending” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: One Row of Pale Green × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “One Row of Pale Green” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 023 — double-barreled shotgun — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “double-barreled shotgun” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “One Row of Pale Green” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “double-barreled shotgun” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “double-barreled shotgun” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — double-barreled shotgun — One Row of Pale Green

This proposed field-note fragment, beat 023 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “double-barreled shotgun” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “One Row of Pale Green” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “double-barreled shotgun” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “double-barreled shotgun” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — double-barreled shotgun — One Row of Pale Green

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “double-barreled shotgun” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “double-barreled shotgun.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “One Row of Pale Green” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “double-barreled shotgun” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “double-barreled shotgun” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — double-barreled shotgun — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “double-barreled shotgun” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “One Row of Pale Green” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “double-barreled shotgun” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “double-barreled shotgun” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: One Row of Pale Green × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “One Row of Pale Green” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: What future can a scene imply without guaranteeing a harvest?

#### Scene draft 024 — seedlings for mineral salt — One Row of Pale Green

For “One Row of Pale Green” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “seedlings for mineral salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “One Row of Pale Green” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “One Row of Pale Green” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “seedlings for mineral salt” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “seedlings for mineral salt” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — seedlings for mineral salt — One Row of Pale Green

This proposed field-note fragment, beat 024 in “One Row of Pale Green,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “seedlings for mineral salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “One Row of Pale Green” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “One Row of Pale Green.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “One Row of Pale Green” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “seedlings for mineral salt” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “seedlings for mineral salt” during “One Row of Pale Green” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — seedlings for mineral salt — One Row of Pale Green

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “seedlings for mineral salt” during “One Row of Pale Green,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “seedlings for mineral salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “One Row of Pale Green” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “One Row of Pale Green.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “One Row of Pale Green” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “seedlings for mineral salt” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “seedlings for mineral salt” during “One Row of Pale Green” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — seedlings for mineral salt — One Row of Pale Green

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “One Row of Pale Green” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “seedlings for mineral salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “One Row of Pale Green” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “One Row of Pale Green.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “One Row of Pale Green” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“What future can a scene imply without guaranteeing a harvest?”—as a revision test tied to “seedlings for mineral salt” during “One Row of Pale Green.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “seedlings for mineral salt” during “One Row of Pale Green” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “One Row of Pale Green” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: A Keeper at Work × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “A Keeper at Work” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 025 — shattered municipal greenhouse — A Keeper at Work

For “A Keeper at Work” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “shattered municipal greenhouse” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “A Keeper at Work” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “shattered municipal greenhouse” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “shattered municipal greenhouse” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — shattered municipal greenhouse — A Keeper at Work

This proposed field-note fragment, beat 025 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “shattered municipal greenhouse.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “A Keeper at Work” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “shattered municipal greenhouse” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “shattered municipal greenhouse” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — shattered municipal greenhouse — A Keeper at Work

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “shattered municipal greenhouse” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “shattered municipal greenhouse” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “A Keeper at Work” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “shattered municipal greenhouse” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “shattered municipal greenhouse” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — shattered municipal greenhouse — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “shattered municipal greenhouse” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “A Keeper at Work” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “shattered municipal greenhouse” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “shattered municipal greenhouse” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: A Keeper at Work × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “A Keeper at Work” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 026 — thick plastic sheeting — A Keeper at Work

For “A Keeper at Work” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “thick plastic sheeting” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “A Keeper at Work” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “thick plastic sheeting” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “thick plastic sheeting” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — thick plastic sheeting — A Keeper at Work

This proposed field-note fragment, beat 026 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “thick plastic sheeting” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “A Keeper at Work” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “thick plastic sheeting” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “thick plastic sheeting” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — thick plastic sheeting — A Keeper at Work

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “thick plastic sheeting” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “thick plastic sheeting” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “A Keeper at Work” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “thick plastic sheeting” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “thick plastic sheeting” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — thick plastic sheeting — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “thick plastic sheeting” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “A Keeper at Work” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “thick plastic sheeting” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “thick plastic sheeting” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: A Keeper at Work × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “A Keeper at Work” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 027 — warm microclimate — A Keeper at Work

For “A Keeper at Work” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “warm microclimate” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “A Keeper at Work” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “warm microclimate” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “warm microclimate” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — warm microclimate — A Keeper at Work

This proposed field-note fragment, beat 027 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “warm microclimate” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “A Keeper at Work” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “warm microclimate” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “warm microclimate” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — warm microclimate — A Keeper at Work

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “warm microclimate” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “warm microclimate.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “A Keeper at Work” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “warm microclimate” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “warm microclimate” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — warm microclimate — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “warm microclimate” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “A Keeper at Work” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “warm microclimate” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “warm microclimate” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: A Keeper at Work × single row

**Beat question:** What can the writer say about “single row” during “A Keeper at Work” while preserving this limit: a count that keeps the scene small. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 028 — single row — A Keeper at Work

For “A Keeper at Work” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “single row.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “A Keeper at Work” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “single row” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “single row” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — single row — A Keeper at Work

This proposed field-note fragment, beat 028 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “single row” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives A companion who sees only the future crop a distinct perspective on “single row” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “A Keeper at Work” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “single row” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “single row” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — single row — A Keeper at Work

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “single row” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “single row” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A traveler who wants to bargain a distinct perspective on “single row” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “A Keeper at Work” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “single row” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “single row” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — single row — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “single row” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “A Keeper at Work” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “single row” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “single row” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: A Keeper at Work × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “A Keeper at Work” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 029 — pale green seedlings — A Keeper at Work

For “A Keeper at Work” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “pale green seedlings” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “A Keeper at Work” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “pale green seedlings” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “pale green seedlings” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — pale green seedlings — A Keeper at Work

This proposed field-note fragment, beat 029 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “pale green seedlings” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “A Keeper at Work” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “pale green seedlings” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “pale green seedlings” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — pale green seedlings — A Keeper at Work

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “pale green seedlings” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “pale green seedlings” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “A Keeper at Work” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “pale green seedlings” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “pale green seedlings” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — pale green seedlings — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “pale green seedlings.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “A Keeper at Work” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “pale green seedlings” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “pale green seedlings” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: A Keeper at Work × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “A Keeper at Work” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 030 — woman is tending — A Keeper at Work

For “A Keeper at Work” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “woman is tending” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “A Keeper at Work” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “woman is tending” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “woman is tending” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — woman is tending — A Keeper at Work

This proposed field-note fragment, beat 030 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “woman is tending.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “A Keeper at Work” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “woman is tending” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “woman is tending” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — woman is tending — A Keeper at Work

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “woman is tending” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “woman is tending” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “A Keeper at Work” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “woman is tending” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “woman is tending” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — woman is tending — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “woman is tending” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “A Keeper at Work” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “woman is tending” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “woman is tending” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: A Keeper at Work × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “A Keeper at Work” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 031 — double-barreled shotgun — A Keeper at Work

For “A Keeper at Work” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “double-barreled shotgun” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “A Keeper at Work” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “double-barreled shotgun” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “double-barreled shotgun” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — double-barreled shotgun — A Keeper at Work

This proposed field-note fragment, beat 031 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “double-barreled shotgun” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “A Keeper at Work” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “double-barreled shotgun” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “double-barreled shotgun” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — double-barreled shotgun — A Keeper at Work

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “double-barreled shotgun” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “double-barreled shotgun” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “A Keeper at Work” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “double-barreled shotgun” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “double-barreled shotgun” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — double-barreled shotgun — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “double-barreled shotgun” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “A Keeper at Work” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “double-barreled shotgun” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “double-barreled shotgun” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: A Keeper at Work × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “A Keeper at Work” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: How do work and boundary share the frame without turning her into a threat?

#### Scene draft 032 — seedlings for mineral salt — A Keeper at Work

For “A Keeper at Work” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “seedlings for mineral salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “A Keeper at Work” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “A Keeper at Work” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “seedlings for mineral salt” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “seedlings for mineral salt” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — seedlings for mineral salt — A Keeper at Work

This proposed field-note fragment, beat 032 in “A Keeper at Work,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “seedlings for mineral salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “A Keeper at Work” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “A Keeper at Work.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “A Keeper at Work” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “seedlings for mineral salt” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “seedlings for mineral salt” during “A Keeper at Work” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — seedlings for mineral salt — A Keeper at Work

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “seedlings for mineral salt” during “A Keeper at Work,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “seedlings for mineral salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “A Keeper at Work” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “A Keeper at Work.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “A Keeper at Work” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “seedlings for mineral salt” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “seedlings for mineral salt” during “A Keeper at Work” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — seedlings for mineral salt — A Keeper at Work

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “A Keeper at Work” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “seedlings for mineral salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “A Keeper at Work” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “A Keeper at Work.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “A Keeper at Work” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“How do work and boundary share the frame without turning her into a threat?”—as a revision test tied to “seedlings for mineral salt” during “A Keeper at Work.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “seedlings for mineral salt” during “A Keeper at Work” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Keeper at Work” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: Salt for Seedlings × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “Salt for Seedlings” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 033 — shattered municipal greenhouse — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “shattered municipal greenhouse” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “Salt for Seedlings” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “shattered municipal greenhouse” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “shattered municipal greenhouse” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — shattered municipal greenhouse — Salt for Seedlings

This proposed field-note fragment, beat 033 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “shattered municipal greenhouse” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “Salt for Seedlings” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “shattered municipal greenhouse” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “shattered municipal greenhouse” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — shattered municipal greenhouse — Salt for Seedlings

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “shattered municipal greenhouse” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “shattered municipal greenhouse” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “Salt for Seedlings” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “shattered municipal greenhouse” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “shattered municipal greenhouse” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — shattered municipal greenhouse — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “shattered municipal greenhouse.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “Salt for Seedlings” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “shattered municipal greenhouse” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “shattered municipal greenhouse” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: Salt for Seedlings × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “Salt for Seedlings” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 034 — thick plastic sheeting — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “thick plastic sheeting” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “Salt for Seedlings” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “thick plastic sheeting” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “thick plastic sheeting” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — thick plastic sheeting — Salt for Seedlings

This proposed field-note fragment, beat 034 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “thick plastic sheeting.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “Salt for Seedlings” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “thick plastic sheeting” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “thick plastic sheeting” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — thick plastic sheeting — Salt for Seedlings

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “thick plastic sheeting” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “thick plastic sheeting” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “Salt for Seedlings” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “thick plastic sheeting” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “thick plastic sheeting” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — thick plastic sheeting — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “thick plastic sheeting” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “Salt for Seedlings” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “thick plastic sheeting” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “thick plastic sheeting” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: Salt for Seedlings × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “Salt for Seedlings” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 035 — warm microclimate — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “warm microclimate” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “Salt for Seedlings” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “warm microclimate” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “warm microclimate” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — warm microclimate — Salt for Seedlings

This proposed field-note fragment, beat 035 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “warm microclimate” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “Salt for Seedlings” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “warm microclimate” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “warm microclimate” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — warm microclimate — Salt for Seedlings

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “warm microclimate” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “warm microclimate” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “Salt for Seedlings” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “warm microclimate” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “warm microclimate” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — warm microclimate — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “warm microclimate” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “Salt for Seedlings” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “warm microclimate” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “warm microclimate” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: Salt for Seedlings × single row

**Beat question:** What can the writer say about “single row” during “Salt for Seedlings” while preserving this limit: a count that keeps the scene small. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 036 — single row — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “single row” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A traveler who wants to bargain a distinct perspective on “single row” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “Salt for Seedlings” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “single row” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “single row” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — single row — Salt for Seedlings

This proposed field-note fragment, beat 036 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “single row” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “Salt for Seedlings” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “single row” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “single row” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — single row — Salt for Seedlings

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “single row” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “single row.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives A companion who sees only the future crop a distinct perspective on “single row” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “Salt for Seedlings” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “single row” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “single row” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — single row — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “single row” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A traveler who wants to bargain a distinct perspective on “single row” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “Salt for Seedlings” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “single row” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “single row” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: Salt for Seedlings × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “Salt for Seedlings” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 037 — pale green seedlings — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “pale green seedlings.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “Salt for Seedlings” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “pale green seedlings” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “pale green seedlings” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — pale green seedlings — Salt for Seedlings

This proposed field-note fragment, beat 037 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “pale green seedlings” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “Salt for Seedlings” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “pale green seedlings” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “pale green seedlings” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — pale green seedlings — Salt for Seedlings

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “pale green seedlings” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “pale green seedlings” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “Salt for Seedlings” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “pale green seedlings” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “pale green seedlings” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — pale green seedlings — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “pale green seedlings” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “Salt for Seedlings” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “pale green seedlings” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “pale green seedlings” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: Salt for Seedlings × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “Salt for Seedlings” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 038 — woman is tending — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “woman is tending” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “Salt for Seedlings” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “woman is tending” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “woman is tending” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — woman is tending — Salt for Seedlings

This proposed field-note fragment, beat 038 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “woman is tending” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “Salt for Seedlings” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “woman is tending” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “woman is tending” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — woman is tending — Salt for Seedlings

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “woman is tending” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “woman is tending” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “Salt for Seedlings” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “woman is tending” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “woman is tending” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — woman is tending — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “woman is tending.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “Salt for Seedlings” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “woman is tending” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “woman is tending” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: Salt for Seedlings × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “Salt for Seedlings” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 039 — double-barreled shotgun — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “double-barreled shotgun” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “Salt for Seedlings” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “double-barreled shotgun” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “double-barreled shotgun” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — double-barreled shotgun — Salt for Seedlings

This proposed field-note fragment, beat 039 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “double-barreled shotgun.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “Salt for Seedlings” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “double-barreled shotgun” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “double-barreled shotgun” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — double-barreled shotgun — Salt for Seedlings

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “double-barreled shotgun” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “double-barreled shotgun” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “Salt for Seedlings” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “double-barreled shotgun” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “double-barreled shotgun” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — double-barreled shotgun — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “double-barreled shotgun” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “Salt for Seedlings” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “double-barreled shotgun” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “double-barreled shotgun” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: Salt for Seedlings × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “Salt for Seedlings” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: Can dialogue honor barter without inventing a full market?

#### Scene draft 040 — seedlings for mineral salt — Salt for Seedlings

For “Salt for Seedlings” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “seedlings for mineral salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “Salt for Seedlings” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “Salt for Seedlings” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “seedlings for mineral salt” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “seedlings for mineral salt” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — seedlings for mineral salt — Salt for Seedlings

This proposed field-note fragment, beat 040 in “Salt for Seedlings,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “seedlings for mineral salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “Salt for Seedlings” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “Salt for Seedlings.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “Salt for Seedlings” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “seedlings for mineral salt” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “seedlings for mineral salt” during “Salt for Seedlings” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — seedlings for mineral salt — Salt for Seedlings

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “seedlings for mineral salt” during “Salt for Seedlings,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “seedlings for mineral salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “Salt for Seedlings” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “Salt for Seedlings.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “Salt for Seedlings” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “seedlings for mineral salt” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “seedlings for mineral salt” during “Salt for Seedlings” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — seedlings for mineral salt — Salt for Seedlings

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “Salt for Seedlings” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “seedlings for mineral salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “Salt for Seedlings” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Salt for Seedlings.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “Salt for Seedlings” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“Can dialogue honor barter without inventing a full market?”—as a revision test tied to “seedlings for mineral salt” during “Salt for Seedlings.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “seedlings for mineral salt” during “Salt for Seedlings” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Salt for Seedlings” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: Leaving the Row with Its Keeper × shattered municipal greenhouse

**Beat question:** What can the writer say about “shattered municipal greenhouse” during “Leaving the Row with Its Keeper” while preserving this limit: a damaged public structure whose full history is unknown. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 041 — shattered municipal greenhouse — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “shattered municipal greenhouse,” the candidate passage attends to A damaged public structure whose full history is unknown. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “shattered municipal greenhouse.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “Leaving the Row with Its Keeper” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — shattered municipal greenhouse — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 041 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “shattered municipal greenhouse” is the point of return. A damaged public structure whose full history is unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “shattered municipal greenhouse” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A traveler who wants to bargain a distinct perspective on “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “Leaving the Row with Its Keeper” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — shattered municipal greenhouse — Leaving the Row with Its Keeper

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “shattered municipal greenhouse” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “shattered municipal greenhouse” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives The keeper, in optional authored dialogue a distinct perspective on “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “Leaving the Row with Its Keeper” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — shattered municipal greenhouse — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “shattered municipal greenhouse” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “shattered municipal greenhouse” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “shattered municipal greenhouse” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives A companion who sees only the future crop a distinct perspective on “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “Leaving the Row with Its Keeper” × “shattered municipal greenhouse,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “shattered municipal greenhouse” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “shattered municipal greenhouse.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: Leaving the Row with Its Keeper × thick plastic sheeting

**Beat question:** What can the writer say about “thick plastic sheeting” during “Leaving the Row with Its Keeper” while preserving this limit: a source detail that defines the sheltered corner, not a how-to. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 042 — thick plastic sheeting — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “thick plastic sheeting,” the candidate passage attends to A source detail that defines the sheltered corner, not a how-to. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “thick plastic sheeting” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “Leaving the Row with Its Keeper” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “thick plastic sheeting” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — thick plastic sheeting — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 042 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “thick plastic sheeting” is the point of return. A source detail that defines the sheltered corner, not a how-to. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “thick plastic sheeting” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives A companion who sees only the future crop a distinct perspective on “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “Leaving the Row with Its Keeper” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “thick plastic sheeting” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — thick plastic sheeting — Leaving the Row with Its Keeper

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “thick plastic sheeting” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “thick plastic sheeting” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A traveler who wants to bargain a distinct perspective on “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “Leaving the Row with Its Keeper” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “thick plastic sheeting” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — thick plastic sheeting — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “thick plastic sheeting” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “thick plastic sheeting.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “thick plastic sheeting” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives The keeper, in optional authored dialogue a distinct perspective on “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “Leaving the Row with Its Keeper” × “thick plastic sheeting,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “thick plastic sheeting” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “thick plastic sheeting” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “thick plastic sheeting.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: Leaving the Row with Its Keeper × warm microclimate

**Beat question:** What can the writer say about “warm microclimate” during “Leaving the Row with Its Keeper” while preserving this limit: a felt contrast, not a measured growing environment. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 043 — warm microclimate — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “warm microclimate,” the candidate passage attends to A felt contrast, not a measured growing environment. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “warm microclimate” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “Leaving the Row with Its Keeper” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “warm microclimate” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “warm microclimate” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — warm microclimate — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 043 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “warm microclimate” is the point of return. A felt contrast, not a measured growing environment. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “warm microclimate.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives The keeper, in optional authored dialogue a distinct perspective on “warm microclimate” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “Leaving the Row with Its Keeper” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “warm microclimate” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “warm microclimate” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — warm microclimate — Leaving the Row with Its Keeper

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “warm microclimate” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “warm microclimate” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives A companion who sees only the future crop a distinct perspective on “warm microclimate” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “Leaving the Row with Its Keeper” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “warm microclimate” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “warm microclimate” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — warm microclimate — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “warm microclimate” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “warm microclimate” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “warm microclimate” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A traveler who wants to bargain a distinct perspective on “warm microclimate” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “Leaving the Row with Its Keeper” × “warm microclimate,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “warm microclimate” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “warm microclimate” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “warm microclimate.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: Leaving the Row with Its Keeper × single row

**Beat question:** What can the writer say about “single row” during “Leaving the Row with Its Keeper” while preserving this limit: a count that keeps the scene small. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 044 — single row — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “single row,” the candidate passage attends to A count that keeps the scene small. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “single row” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives A companion who sees only the future crop a distinct perspective on “single row” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “Leaving the Row with Its Keeper” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “single row” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “single row” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — single row — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 044 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “single row” is the point of return. A count that keeps the scene small. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “single row” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A traveler who wants to bargain a distinct perspective on “single row” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “Leaving the Row with Its Keeper” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “single row” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “single row” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — single row — Leaving the Row with Its Keeper

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “single row” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “single row” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives The keeper, in optional authored dialogue a distinct perspective on “single row” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “Leaving the Row with Its Keeper” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “single row” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “single row” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — single row — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “single row” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “single row” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “single row” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives A companion who sees only the future crop a distinct perspective on “single row” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “Leaving the Row with Its Keeper” × “single row,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “single row” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “single row” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “single row.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: Leaving the Row with Its Keeper × pale green seedlings

**Beat question:** What can the writer say about “pale green seedlings” during “Leaving the Row with Its Keeper” while preserving this limit: a color and stage, not a crop identity. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 045 — pale green seedlings — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “pale green seedlings,” the candidate passage attends to A color and stage, not a crop identity. The present action begins small: plastic shifting softly while the exchange pauses. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “pale green seedlings” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “Leaving the Row with Its Keeper” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “pale green seedlings” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “pale green seedlings” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — pale green seedlings — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 045 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “pale green seedlings” is the point of return. A color and stage, not a crop identity. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “pale green seedlings” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives A companion who sees only the future crop a distinct perspective on “pale green seedlings” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “Leaving the Row with Its Keeper” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “pale green seedlings” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “pale green seedlings” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — pale green seedlings — Leaving the Row with Its Keeper

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “pale green seedlings” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “pale green seedlings.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A traveler who wants to bargain a distinct perspective on “pale green seedlings” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “Leaving the Row with Its Keeper” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “pale green seedlings” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “pale green seedlings” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — pale green seedlings — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “pale green seedlings” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “pale green seedlings” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “pale green seedlings” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives The keeper, in optional authored dialogue a distinct perspective on “pale green seedlings” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “Leaving the Row with Its Keeper” × “pale green seedlings,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “pale green seedlings” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “pale green seedlings” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “pale green seedlings.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: Leaving the Row with Its Keeper × woman is tending

**Beat question:** What can the writer say about “woman is tending” during “Leaving the Row with Its Keeper” while preserving this limit: an ongoing action and a role in the present. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 046 — woman is tending — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “woman is tending,” the candidate passage attends to An ongoing action and a role in the present. The present action begins small: salt kept on the traveler’s side until terms are clear. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “woman is tending.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “Leaving the Row with Its Keeper” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “woman is tending” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “woman is tending” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — woman is tending — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 046 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “woman is tending” is the point of return. An ongoing action and a role in the present. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “woman is tending” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives The keeper, in optional authored dialogue a distinct perspective on “woman is tending” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “Leaving the Row with Its Keeper” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “woman is tending” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “woman is tending” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — woman is tending — Leaving the Row with Its Keeper

The proposed exchange gives a companion who sees only the future crop a distinct reason to speak. Its authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” The talk concerns “woman is tending” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “woman is tending” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives A companion who sees only the future crop a distinct perspective on “woman is tending” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “Leaving the Row with Its Keeper” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Do not touch the row to show me what you mean.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “woman is tending” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “woman is tending” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — woman is tending — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “woman is tending” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “woman is tending” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “woman is tending” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A traveler who wants to bargain a distinct perspective on “woman is tending” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “Leaving the Row with Its Keeper” × “woman is tending,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “woman is tending” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “woman is tending” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “woman is tending.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: Leaving the Row with Its Keeper × double-barreled shotgun

**Beat question:** What can the writer say about “double-barreled shotgun” during “Leaving the Row with Its Keeper” while preserving this limit: a visible boundary; no tactic or use is supplied. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 047 — double-barreled shotgun — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “double-barreled shotgun,” the candidate passage attends to A visible boundary; no tactic or use is supplied. The present action begins small: a hand hovering above the row only after permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “double-barreled shotgun” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “Leaving the Row with Its Keeper” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “double-barreled shotgun” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — double-barreled shotgun — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 047 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “double-barreled shotgun” is the point of return. A visible boundary; no tactic or use is supplied. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “double-barreled shotgun” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A traveler who wants to bargain a distinct perspective on “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “Leaving the Row with Its Keeper” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “double-barreled shotgun” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — double-barreled shotgun — Leaving the Row with Its Keeper

The proposed exchange gives the keeper, in optional authored dialogue a distinct reason to speak. Its authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” The talk concerns “double-barreled shotgun” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “double-barreled shotgun” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives The keeper, in optional authored dialogue a distinct perspective on “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “Leaving the Row with Its Keeper” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Those are my terms. You can answer or keep walking.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “double-barreled shotgun” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — double-barreled shotgun — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “double-barreled shotgun” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “double-barreled shotgun.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “double-barreled shotgun” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives A companion who sees only the future crop a distinct perspective on “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “Leaving the Row with Its Keeper” × “double-barreled shotgun,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “double-barreled shotgun” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “double-barreled shotgun” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “double-barreled shotgun.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: Leaving the Row with Its Keeper × seedlings for mineral salt

**Beat question:** What can the writer say about “seedlings for mineral salt” during “Leaving the Row with Its Keeper” while preserving this limit: the stated exchange, not a guarantee of either party’s future. The larger movement question is: How does the closing preserve the keeper’s ownership of what remains?

#### Scene draft 048 — seedlings for mineral salt — Leaving the Row with Its Keeper

For “Leaving the Row with Its Keeper” and the source phrase “seedlings for mineral salt,” the candidate passage attends to The stated exchange, not a guarantee of either party’s future. The present action begins small: the keeper continuing her work whether the offer is accepted or not. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “seedlings for mineral salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “Leaving the Row with Its Keeper” scene draft, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “Leaving the Row with Its Keeper” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “seedlings for mineral salt” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — seedlings for mineral salt — Leaving the Row with Its Keeper

This proposed field-note fragment, beat 048 in “Leaving the Row with Its Keeper,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “seedlings for mineral salt” is the point of return. The stated exchange, not a guarantee of either party’s future. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “seedlings for mineral salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “Leaving the Row with Its Keeper” field-note fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives A companion who sees only the future crop a distinct perspective on “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Learns not to turn a fragile row into a guaranteed promise.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “Leaving the Row with Its Keeper” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Salt for seedlings. If you have something else, ask first.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “seedlings for mineral salt” during “Leaving the Row with Its Keeper” in a changed register: “Those are my terms. You can answer or keep walking.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — seedlings for mineral salt — Leaving the Row with Its Keeper

The proposed exchange gives a traveler who wants to bargain a distinct reason to speak. Its authoring note is: “Makes a clear offer and accepts that the keeper may decline.” The talk concerns “seedlings for mineral salt” during “Leaving the Row with Its Keeper,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “seedlings for mineral salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “Leaving the Row with Its Keeper” conversation fragment, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A traveler who wants to bargain a distinct perspective on “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Makes a clear offer and accepts that the keeper may decline.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “Leaving the Row with Its Keeper” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Those are my terms. You can answer or keep walking.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Salt for seedlings. If you have something else, ask first.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “seedlings for mineral salt” during “Leaving the Row with Its Keeper” in a changed register: “Do not touch the row to show me what you mean.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — seedlings for mineral salt — Leaving the Row with Its Keeper

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “seedlings for mineral salt” through “Leaving the Row with Its Keeper” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “seedlings for mineral salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “Leaving the Row with Its Keeper” conditional return vignette, is narrow. The local description says: “A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “seedlings for mineral salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives The keeper, in optional authored dialogue a distinct perspective on “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The optional authoring note is: “Can name terms and stop the conversation without disclosing a name or history.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “Leaving the Row with Its Keeper” × “seedlings for mineral salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Do not touch the row to show me what you mean.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“How does the closing preserve the keeper’s ownership of what remains?”—as a revision test tied to “seedlings for mineral salt” during “Leaving the Row with Its Keeper.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “seedlings for mineral salt” during “Leaving the Row with Its Keeper” in a changed register: “Salt for seedlings. If you have something else, ask first.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Leaving the Row with Its Keeper” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “seedlings for mineral salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_greenhouse_keeper`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

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

The proposal is local to `enc_greenhouse_keeper` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_greenhouse_keeper` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | Catalog presence does not by itself show where prose is presented. |
| Description | A shattered municipal greenhouse. In one corner, thick plastic sheeting creates a warm microclimate. A woman is tending a single row of pale green seedlings. She leans on a double-barreled shotgun. She offers seedlings for mineral salt. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_greenhouse_keeper` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.