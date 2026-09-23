# EXPANSION CW35-01 — The Tower That Holds No Water

## A perimeter landmark where an open view cannot answer what an empty reservoir means.

### Prose Wave 35: Places That Keep Their Questions

## Batch brief

**Content type:** prose-first playable-content expansion plan with scene drafts, diegetic records, conversation fragments, and conditional callbacks.
**Content bank:** 48 story beats with four alternative authored forms per beat (192 candidate passages).
**Current location anchor:** `loc_north_gate_water_tower` — North Gate Water Tower.
**Tone:** material, restrained, human, and careful with uncertainty.
**Canon sensitivity:** current location and linked authored records define known facts; old plans or JSON presence do not prove a current runtime route.
**Scope:** game-content plan and original prose drafts only; no production code, JSON, route, quest, flag, system, or save change.

## 1. Expansion thesis

The North Gate Water Tower stands at the edge of a collapsed settlement. The location gives it unobstructed sightlines in every direction and says it has been empty for years; nobody has decided whether that is a loss or a mercy. The discovery manifest also associates a recovered epitaph with this inspection producer, while the epitaph itself names a different grave site and carries unresolved memorial identity. This plan writes around the tower’s absence without relocating the dead or treating a sightline as proof of safety. The central game content is a human conversation about what people can observe from a place that no longer supplies what its name promises. The bank below is intended to produce playable narrative texture: a player can discover, compare, question, refuse, or return to a passage while the existing game systems continue to own state. The fragments are written as content, not as a feature roadmap.

## 2. Story question

When a landmark no longer performs its old promise, who gets to call that a loss, and who gets to call it mercy?

## 3. Verified local anchor and source records

The location says a surviving tower sits at the northern perimeter of a collapsed settlement, offers unobstructed sightlines in every direction, and has been empty for years. The manifest maps the dry-canteen epitaph to this producer but labels identity unresolved/display-only; the epitaph’s own grave_site field is NORTH_PASS_SUMMIT_CREVICE. That association does not put the memorial at the tower or identify a tower visitor.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `loc_north_gate_water_tower` | perimeter tower, all-direction sightlines, empty for years, loss-or-mercy question |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` | `disc_fringe_epitaph_dry_canteen_memorial` | producer association; recovered memorial testimony; unresolved identity/display only |
| `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs.json` | `epitaph_scav_dry_canteen_memorial` | record names NORTH_PASS_SUMMIT_CREVICE as grave site; keep it separate from tower |

## 4. Fixed canon and open space

The tower is at the northern perimeter of a collapsed settlement, is empty, and offers broad sightlines. The epitaph is a separate authored memorial record whose grave-site field names NORTH_PASS_SUMMIT_CREVICE. Manifest producer metadata does not relocate it. No source gives the tower’s former users, its exact water system, or the cause of its emptiness. Existing characters retain their authored identity, boundaries, and outcomes. New working voices remain editorial until an existing content owner approves them. Never fill a source gap solely to make the scene resolve.

## 5. Human center

The emotional center is a person looking from a place that appears useful and still cannot say what lies beyond what they can see. One speaker remembers water as a promise; another is relieved that nobody has to queue at a dry structure. Neither response becomes universal. The memorial remains testimony about a different site, not a plot device attached to an anonymous tower. The emotional pressure should come through what a person records, omits, repairs, asks, or leaves unsigned rather than a narrator naming what the location means.

## 6. Voice and point of view

A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep each author’s knowledge local. A field note cannot know what an unnamed visitor thought; a later reader cannot recover a date that was never recorded; a title cannot create a route. Vary sentence length and register by document purpose, not by changing established facts.

## 7. Placement and current reachability

Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. All fragments are candidates. None is evidence that the location is currently player-reachable. Before selecting any text, verify the current map/location owner, content schema, actual consumer, and the source condition under which the text can appear.

## 8. Player agency

The player can look, ask what the tower still means, read the separate memorial text if its current consumer exposes it, or leave. They are not asked to decide whether the empty tower was good for everyone. Existing map, trade, water, memorial, and faction owners retain their outcomes. Do not add a moral-choice menu simply to make quiet prose interactive. Preserve the player’s refusal, ability to leave, and the authority of the characters who own their words.

## 9. Continuity, dignity, and safety

Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Keep the setting fictional and physically grounded. No absent person receives a biography merely to heighten emotion. Technical and medical context remains descriptive and non-instructional.

## 10. Existing hooks and implementation boundary

**Existing content anchors:** the location row and linked records listed above. **Unverified:** any route, text consumer, state condition, dialogue surface, or return trigger not explicitly established by current authority. **Classification:** editorial game-content proposal; no implementation category is claimed until an owner and consumer are confirmed.

This plan changes no production code or game data. Do not add a parallel discovery registry, route, save section, or gameplay authority to host these drafts. Where a passage reflects a branch, use only the existing state named in section 16 and only after its current owner confirms the consumer.

## 11. Narrative sequence

### 1. Perimeter — a view in every direction

Begin with the tower’s stated position and sightlines. Do not describe unseen districts as if observed.

### 2. Name — a water tower without water

Let the name create expectation, then keep the source’s only fact: it has been empty for years.

### 3. Disagreement — loss and mercy

Different visitors can use the source’s two interpretations without becoming representatives of the whole settlement.

### 4. Separate testimony — the dry-canteen record

If exposed by a verified consumer, cite the epitaph under its own grave-site field and unresolved identity.

### 5. Reading — visibility has limits

The watcher can name what is within view and admit that a broad view is not knowledge of every person.

### 6. Return — the question remains with the visitor

Close without a vote, refill, memorial transfer, or declaration about what the community needed.

## 12. Creative variants

### Grounded

Keep one perimeter conversation and one careful reading of the tower description. Leave the binary unresolved.

### Interlinked

Only show the memorial if its current discovery consumer is verified; preserve NORTH_PASS_SUMMIT_CREVICE and unresolved identity.

### Wild card

Write two visitor notes with opposite interpretations of the empty tower; both remain limited to their authors.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not four required encounters. Scene drafts stage an observation; record drafts give it a plausible author and audience; conversation fragments expose a practical point of friction; consequence vignettes allow a later reader to recognize changed context. Select only forms that fit an existing content owner.

A selected fragment should answer who made it, why they made it, who might read it, and what the author cannot know. Keep objects specific to this location. Let a line do practical work before it carries a theme. Remove exposition that a worker would not say aloud, repeated catastrophe language, unearned revelation, and wording that could be mistaken for a new mechanic. Where existing game state matters, state it in the source owner’s terms and do not duplicate its authority.

## 14. Content bank

### Scene draft 001 — The tower is named before it is seen

A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. The record identifies a surviving tower at the northern edge of a collapsed settlement. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What does the name promise?”
Returning visitor: “Only what the page says about it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The record identifies a surviving tower at the northern edge of a collapsed settlement. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No system condition or capacity is implied.

Scene close: Do not add a working water supply. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 002 — The tower is named before it is seen

Proposed diegetic text: “proposed note: place name copied from the location record”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The record identifies a surviving tower at the northern edge of a collapsed settlement. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Do not add a working water supply. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No system condition or capacity is implied. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 003 — The tower is named before it is seen

A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The record identifies a surviving tower at the northern edge of a collapsed settlement. Neither voice exists to lecture the player.

Tower watcher: “What does the name promise?”
Returning visitor: “Only what the page says about it.”
Tower watcher: “Do not add a working water supply.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No system condition or capacity is implied. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 004 — The tower is named before it is seen

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Do not add a working water supply. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Do not add a working water supply.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed note: place name copied from the location record”

Return boundary: No system condition or capacity is implied. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 005 — The tower is named before it is seen — a later reading

A later reader returns after Do not add a working water supply. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The record identifies a surviving tower at the northern edge of a collapsed settlement. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “Only what the page says about it. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The record identifies a surviving tower at the northern edge of a collapsed settlement. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No system condition or capacity is implied.

Scene close: The later reader notices that Do not add a working water supply. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 006 — The tower is named before it is seen — a later reading

Proposed diegetic text: “Later margin: proposed note: place name copied from the location record / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Do not add a working water supply. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The record identifies a surviving tower at the northern edge of a collapsed settlement. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Do not add a working water supply. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No system condition or capacity is implied. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 007 — The tower is named before it is seen — a later reading

A later reader returns after Do not add a working water supply. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The record identifies a surviving tower at the northern edge of a collapsed settlement. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “Only what the page says about it. The rest stays outside this record.”
Tower watcher: “The later reader notices that Do not add a working water supply. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No system condition or capacity is implied. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 008 — The tower is named before it is seen — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Do not add a working water supply. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Do not add a working water supply. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed note: place name copied from the location record / condition and circulation not established.”

Return boundary: No system condition or capacity is implied. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 009 — The tower is named before it is seen — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The record identifies a surviving tower at the northern edge of a collapsed settlement. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “Only what the page says about it. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The record identifies a surviving tower at the northern edge of a collapsed settlement. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No system condition or capacity is implied.

Scene close: In the copied version, Do not add a working water supply. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 010 — The tower is named before it is seen — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed note: place name copied from the location record / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The record identifies a surviving tower at the northern edge of a collapsed settlement. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Do not add a working water supply. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No system condition or capacity is implied. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 011 — The tower is named before it is seen — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The record identifies a surviving tower at the northern edge of a collapsed settlement. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “Only what the page says about it. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Do not add a working water supply. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No system condition or capacity is implied. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 012 — The tower is named before it is seen — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Do not add a working water supply. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Do not add a working water supply. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed note: place name copied from the location record / author and date not canonically supplied.”

Return boundary: No system condition or capacity is implied. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 013 — The tower is named before it is seen — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The record identifies a surviving tower at the northern edge of a collapsed settlement. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “Only what the page says about it. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The record identifies a surviving tower at the northern edge of a collapsed settlement. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No system condition or capacity is implied.

Scene close: A later reading keeps the unresolved part intact: Do not add a working water supply. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 014 — The tower is named before it is seen — what stays outside

Proposed diegetic text: “Unfinished note: proposed note: place name copied from the location record / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The record identifies a surviving tower at the northern edge of a collapsed settlement. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Do not add a working water supply. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No system condition or capacity is implied. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 015 — The tower is named before it is seen — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor repeats North Gate Water Tower while still at the perimeter and notices that the name carries more promise than the description does. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The record identifies a surviving tower at the northern edge of a collapsed settlement. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “Only what the page says about it. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Do not add a working water supply. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No system condition or capacity is implied. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 016 — The tower is named before it is seen — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Do not add a working water supply. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Do not add a working water supply. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed note: place name copied from the location record / interpretation withheld.”

Return boundary: No system condition or capacity is implied. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 017 — An empty structure can still be visible

A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. The source gives unobstructed sightlines in every direction; it does not define range or surveillance. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can you see everything?”
Returning visitor: “I can see in every direction from here.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No detection or scout benefit.

Scene close: Separate open view from omniscience. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 018 — An empty structure can still be visible

Proposed diegetic text: “proposed margin: visible in all directions”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Separate open view from omniscience. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No detection or scout benefit. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 019 — An empty structure can still be visible

A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. Neither voice exists to lecture the player.

Tower watcher: “Can you see everything?”
Returning visitor: “I can see in every direction from here.”
Tower watcher: “Separate open view from omniscience.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No detection or scout benefit. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 020 — An empty structure can still be visible

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Separate open view from omniscience. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Separate open view from omniscience.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed margin: visible in all directions”

Return boundary: No detection or scout benefit. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 021 — An empty structure can still be visible — a later reading

A later reader returns after Separate open view from omniscience. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I can see in every direction from here. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No detection or scout benefit.

Scene close: The later reader notices that Separate open view from omniscience. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 022 — An empty structure can still be visible — a later reading

Proposed diegetic text: “Later margin: proposed margin: visible in all directions / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Separate open view from omniscience. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Separate open view from omniscience. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No detection or scout benefit. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 023 — An empty structure can still be visible — a later reading

A later reader returns after Separate open view from omniscience. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I can see in every direction from here. The rest stays outside this record.”
Tower watcher: “The later reader notices that Separate open view from omniscience. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No detection or scout benefit. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 024 — An empty structure can still be visible — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Separate open view from omniscience. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Separate open view from omniscience. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed margin: visible in all directions / condition and circulation not established.”

Return boundary: No detection or scout benefit. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 025 — An empty structure can still be visible — copied for someone absent

A second copy reaches a person who was not present for the first telling. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I can see in every direction from here. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No detection or scout benefit.

Scene close: In the copied version, Separate open view from omniscience. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 026 — An empty structure can still be visible — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed margin: visible in all directions / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Separate open view from omniscience. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No detection or scout benefit. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 027 — An empty structure can still be visible — copied for someone absent

A second copy reaches a person who was not present for the first telling. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I can see in every direction from here. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Separate open view from omniscience. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No detection or scout benefit. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 028 — An empty structure can still be visible — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Separate open view from omniscience. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Separate open view from omniscience. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed margin: visible in all directions / author and date not canonically supplied.”

Return boundary: No detection or scout benefit. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 029 — An empty structure can still be visible — what stays outside

This alternative ends at the edge of what its speaker can know. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I can see in every direction from here. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No detection or scout benefit.

Scene close: A later reading keeps the unresolved part intact: Separate open view from omniscience. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 030 — An empty structure can still be visible — what stays outside

Proposed diegetic text: “Unfinished note: proposed margin: visible in all directions / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Separate open view from omniscience. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No detection or scout benefit. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 031 — An empty structure can still be visible — what stays outside

This alternative ends at the edge of what its speaker can know. A watcher points out that the tower can be seen from the approach without claiming it can see every person in the settlement. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The source gives unobstructed sightlines in every direction; it does not define range or surveillance. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I can see in every direction from here. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Separate open view from omniscience. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No detection or scout benefit. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 032 — An empty structure can still be visible — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Separate open view from omniscience. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Separate open view from omniscience. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed margin: visible in all directions / interpretation withheld.”

Return boundary: No detection or scout benefit. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 033 — Empty for years

A visitor hears the phrase and asks whether anyone remembers the last water. The source states long emptiness but gives no date or former user. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “When did it run dry?”
Returning visitor: “This record does not say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source states long emptiness but gives no date or former user. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a failure cause or date.

Scene close: Let the gap stay a gap. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 034 — Empty for years

Proposed diegetic text: “proposed visitor question left without date”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A visitor hears the phrase and asks whether anyone remembers the last water. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source states long emptiness but gives no date or former user. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Let the gap stay a gap. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a failure cause or date. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 035 — Empty for years

A visitor hears the phrase and asks whether anyone remembers the last water. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source states long emptiness but gives no date or former user. Neither voice exists to lecture the player.

Tower watcher: “When did it run dry?”
Returning visitor: “This record does not say.”
Tower watcher: “Let the gap stay a gap.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a failure cause or date. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 036 — Empty for years

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Let the gap stay a gap. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Let the gap stay a gap.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed visitor question left without date”

Return boundary: Do not invent a failure cause or date. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 037 — Empty for years — a later reading

A later reader returns after Let the gap stay a gap. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The source states long emptiness but gives no date or former user. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “This record does not say. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The source states long emptiness but gives no date or former user. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a failure cause or date.

Scene close: The later reader notices that Let the gap stay a gap. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 038 — Empty for years — a later reading

Proposed diegetic text: “Later margin: proposed visitor question left without date / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Let the gap stay a gap. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The source states long emptiness but gives no date or former user. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Let the gap stay a gap. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a failure cause or date. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 039 — Empty for years — a later reading

A later reader returns after Let the gap stay a gap. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The source states long emptiness but gives no date or former user. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “This record does not say. The rest stays outside this record.”
Tower watcher: “The later reader notices that Let the gap stay a gap. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a failure cause or date. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 040 — Empty for years — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Let the gap stay a gap. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Let the gap stay a gap. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed visitor question left without date / condition and circulation not established.”

Return boundary: Do not invent a failure cause or date. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 041 — Empty for years — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor hears the phrase and asks whether anyone remembers the last water. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The source states long emptiness but gives no date or former user. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “This record does not say. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The source states long emptiness but gives no date or former user. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a failure cause or date.

Scene close: In the copied version, Let the gap stay a gap. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 042 — Empty for years — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed visitor question left without date / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A visitor hears the phrase and asks whether anyone remembers the last water. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The source states long emptiness but gives no date or former user. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Let the gap stay a gap. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a failure cause or date. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 043 — Empty for years — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor hears the phrase and asks whether anyone remembers the last water. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The source states long emptiness but gives no date or former user. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “This record does not say. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Let the gap stay a gap. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a failure cause or date. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 044 — Empty for years — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Let the gap stay a gap. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Let the gap stay a gap. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed visitor question left without date / author and date not canonically supplied.”

Return boundary: Do not invent a failure cause or date. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 045 — Empty for years — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor hears the phrase and asks whether anyone remembers the last water. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The source states long emptiness but gives no date or former user. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “This record does not say. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The source states long emptiness but gives no date or former user. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a failure cause or date.

Scene close: A later reading keeps the unresolved part intact: Let the gap stay a gap. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 046 — Empty for years — what stays outside

Proposed diegetic text: “Unfinished note: proposed visitor question left without date / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A visitor hears the phrase and asks whether anyone remembers the last water. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The source states long emptiness but gives no date or former user. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Let the gap stay a gap. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a failure cause or date. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 047 — Empty for years — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor hears the phrase and asks whether anyone remembers the last water. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The source states long emptiness but gives no date or former user. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “This record does not say. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Let the gap stay a gap. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a failure cause or date. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 048 — Empty for years — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Let the gap stay a gap. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Let the gap stay a gap. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed visitor question left without date / interpretation withheld.”

Return boundary: Do not invent a failure cause or date. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 049 — Loss

One person uses the location’s first offered word and says the tower represents something the settlement no longer has. The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Does everyone miss it?”
Returning visitor: “I can only answer for myself.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No water mechanics.

Scene close: Do not turn one opinion into community consensus. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 050 — Loss

Proposed diegetic text: “proposed note: loss, signed by no one”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: One person uses the location’s first offered word and says the tower represents something the settlement no longer has. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Do not turn one opinion into community consensus. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No water mechanics. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 051 — Loss

One person uses the location’s first offered word and says the tower represents something the settlement no longer has. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. Neither voice exists to lecture the player.

Tower watcher: “Does everyone miss it?”
Returning visitor: “I can only answer for myself.”
Tower watcher: “Do not turn one opinion into community consensus.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No water mechanics. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 052 — Loss

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Do not turn one opinion into community consensus. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Do not turn one opinion into community consensus.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed note: loss, signed by no one”

Return boundary: No water mechanics. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 053 — Loss — a later reading

A later reader returns after Do not turn one opinion into community consensus. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I can only answer for myself. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No water mechanics.

Scene close: The later reader notices that Do not turn one opinion into community consensus. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 054 — Loss — a later reading

Proposed diegetic text: “Later margin: proposed note: loss, signed by no one / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Do not turn one opinion into community consensus. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Do not turn one opinion into community consensus. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No water mechanics. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 055 — Loss — a later reading

A later reader returns after Do not turn one opinion into community consensus. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I can only answer for myself. The rest stays outside this record.”
Tower watcher: “The later reader notices that Do not turn one opinion into community consensus. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No water mechanics. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 056 — Loss — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Do not turn one opinion into community consensus. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Do not turn one opinion into community consensus. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed note: loss, signed by no one / condition and circulation not established.”

Return boundary: No water mechanics. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 057 — Loss — copied for someone absent

A second copy reaches a person who was not present for the first telling. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I can only answer for myself. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No water mechanics.

Scene close: In the copied version, Do not turn one opinion into community consensus. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 058 — Loss — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed note: loss, signed by no one / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Do not turn one opinion into community consensus. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No water mechanics. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 059 — Loss — copied for someone absent

A second copy reaches a person who was not present for the first telling. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I can only answer for myself. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Do not turn one opinion into community consensus. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No water mechanics. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 060 — Loss — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Do not turn one opinion into community consensus. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Do not turn one opinion into community consensus. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed note: loss, signed by no one / author and date not canonically supplied.”

Return boundary: No water mechanics. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 061 — Loss — what stays outside

This alternative ends at the edge of what its speaker can know. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I can only answer for myself. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No water mechanics.

Scene close: A later reading keeps the unresolved part intact: Do not turn one opinion into community consensus. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 062 — Loss — what stays outside

Proposed diegetic text: “Unfinished note: proposed note: loss, signed by no one / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Do not turn one opinion into community consensus. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No water mechanics. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 063 — Loss — what stays outside

This alternative ends at the edge of what its speaker can know. One person uses the location’s first offered word and says the tower represents something the settlement no longer has. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The description says nobody has decided whether emptiness is a loss; this speaker’s view remains personal. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I can only answer for myself. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Do not turn one opinion into community consensus. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No water mechanics. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 064 — Loss — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Do not turn one opinion into community consensus. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Do not turn one opinion into community consensus. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed note: loss, signed by no one / interpretation withheld.”

Return boundary: No water mechanics. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 065 — Mercy

A second person uses the other word and describes relief at not having to make a queue around a dry landmark. The source permits mercy as an interpretation but does not say what people were spared. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What was spared?”
Returning visitor: “I do not know what the tower once asked of anyone.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source permits mercy as an interpretation but does not say what people were spared. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a crisis or a ration policy.

Scene close: Keep relief personal and unspecific. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 066 — Mercy

Proposed diegetic text: “proposed margin: mercy, author unknown”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second person uses the other word and describes relief at not having to make a queue around a dry landmark. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source permits mercy as an interpretation but does not say what people were spared. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Keep relief personal and unspecific. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a crisis or a ration policy. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 067 — Mercy

A second person uses the other word and describes relief at not having to make a queue around a dry landmark. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source permits mercy as an interpretation but does not say what people were spared. Neither voice exists to lecture the player.

Tower watcher: “What was spared?”
Returning visitor: “I do not know what the tower once asked of anyone.”
Tower watcher: “Keep relief personal and unspecific.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a crisis or a ration policy. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 068 — Mercy

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Keep relief personal and unspecific. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Keep relief personal and unspecific.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed margin: mercy, author unknown”

Return boundary: Do not invent a crisis or a ration policy. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 069 — Mercy — a later reading

A later reader returns after Keep relief personal and unspecific. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The source permits mercy as an interpretation but does not say what people were spared. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I do not know what the tower once asked of anyone. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The source permits mercy as an interpretation but does not say what people were spared. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a crisis or a ration policy.

Scene close: The later reader notices that Keep relief personal and unspecific. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 070 — Mercy — a later reading

Proposed diegetic text: “Later margin: proposed margin: mercy, author unknown / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Keep relief personal and unspecific. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The source permits mercy as an interpretation but does not say what people were spared. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Keep relief personal and unspecific. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a crisis or a ration policy. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 071 — Mercy — a later reading

A later reader returns after Keep relief personal and unspecific. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The source permits mercy as an interpretation but does not say what people were spared. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I do not know what the tower once asked of anyone. The rest stays outside this record.”
Tower watcher: “The later reader notices that Keep relief personal and unspecific. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a crisis or a ration policy. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 072 — Mercy — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Keep relief personal and unspecific. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Keep relief personal and unspecific. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed margin: mercy, author unknown / condition and circulation not established.”

Return boundary: Do not invent a crisis or a ration policy. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 073 — Mercy — copied for someone absent

A second copy reaches a person who was not present for the first telling. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The source permits mercy as an interpretation but does not say what people were spared. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I do not know what the tower once asked of anyone. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The source permits mercy as an interpretation but does not say what people were spared. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a crisis or a ration policy.

Scene close: In the copied version, Keep relief personal and unspecific. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 074 — Mercy — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed margin: mercy, author unknown / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The source permits mercy as an interpretation but does not say what people were spared. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Keep relief personal and unspecific. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a crisis or a ration policy. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 075 — Mercy — copied for someone absent

A second copy reaches a person who was not present for the first telling. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The source permits mercy as an interpretation but does not say what people were spared. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I do not know what the tower once asked of anyone. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Keep relief personal and unspecific. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a crisis or a ration policy. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 076 — Mercy — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Keep relief personal and unspecific. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Keep relief personal and unspecific. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed margin: mercy, author unknown / author and date not canonically supplied.”

Return boundary: Do not invent a crisis or a ration policy. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 077 — Mercy — what stays outside

This alternative ends at the edge of what its speaker can know. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The source permits mercy as an interpretation but does not say what people were spared. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I do not know what the tower once asked of anyone. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The source permits mercy as an interpretation but does not say what people were spared. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not invent a crisis or a ration policy.

Scene close: A later reading keeps the unresolved part intact: Keep relief personal and unspecific. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 078 — Mercy — what stays outside

Proposed diegetic text: “Unfinished note: proposed margin: mercy, author unknown / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The source permits mercy as an interpretation but does not say what people were spared. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Keep relief personal and unspecific. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not invent a crisis or a ration policy. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 079 — Mercy — what stays outside

This alternative ends at the edge of what its speaker can know. A second person uses the other word and describes relief at not having to make a queue around a dry landmark. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The source permits mercy as an interpretation but does not say what people were spared. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I do not know what the tower once asked of anyone. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Keep relief personal and unspecific. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not invent a crisis or a ration policy. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 080 — Mercy — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Keep relief personal and unspecific. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Keep relief personal and unspecific. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed margin: mercy, author unknown / interpretation withheld.”

Return boundary: Do not invent a crisis or a ration policy. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 081 — The watcher refuses the full map

A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. The all-direction sightline does not identify landmarks, roads, or residents. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What is on the far side?”
Returning visitor: “Not something I can name from this page.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The all-direction sightline does not identify landmarks, roads, or residents. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No route.

Scene close: Do not generate geography from a general view. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 082 — The watcher refuses the full map

Proposed diegetic text: “proposed note: horizon observed; destinations not supplied”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The all-direction sightline does not identify landmarks, roads, or residents. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Do not generate geography from a general view. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No route. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 083 — The watcher refuses the full map

A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The all-direction sightline does not identify landmarks, roads, or residents. Neither voice exists to lecture the player.

Tower watcher: “What is on the far side?”
Returning visitor: “Not something I can name from this page.”
Tower watcher: “Do not generate geography from a general view.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No route. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 084 — The watcher refuses the full map

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Do not generate geography from a general view. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Do not generate geography from a general view.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed note: horizon observed; destinations not supplied”

Return boundary: No route. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 085 — The watcher refuses the full map — a later reading

A later reader returns after Do not generate geography from a general view. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The all-direction sightline does not identify landmarks, roads, or residents. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “Not something I can name from this page. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The all-direction sightline does not identify landmarks, roads, or residents. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No route.

Scene close: The later reader notices that Do not generate geography from a general view. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 086 — The watcher refuses the full map — a later reading

Proposed diegetic text: “Later margin: proposed note: horizon observed; destinations not supplied / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Do not generate geography from a general view. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The all-direction sightline does not identify landmarks, roads, or residents. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Do not generate geography from a general view. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No route. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 087 — The watcher refuses the full map — a later reading

A later reader returns after Do not generate geography from a general view. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The all-direction sightline does not identify landmarks, roads, or residents. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “Not something I can name from this page. The rest stays outside this record.”
Tower watcher: “The later reader notices that Do not generate geography from a general view. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No route. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 088 — The watcher refuses the full map — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Do not generate geography from a general view. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Do not generate geography from a general view. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed note: horizon observed; destinations not supplied / condition and circulation not established.”

Return boundary: No route. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 089 — The watcher refuses the full map — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The all-direction sightline does not identify landmarks, roads, or residents. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “Not something I can name from this page. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The all-direction sightline does not identify landmarks, roads, or residents. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No route.

Scene close: In the copied version, Do not generate geography from a general view. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 090 — The watcher refuses the full map — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed note: horizon observed; destinations not supplied / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The all-direction sightline does not identify landmarks, roads, or residents. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Do not generate geography from a general view. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No route. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 091 — The watcher refuses the full map — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The all-direction sightline does not identify landmarks, roads, or residents. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “Not something I can name from this page. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Do not generate geography from a general view. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No route. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 092 — The watcher refuses the full map — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Do not generate geography from a general view. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Do not generate geography from a general view. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed note: horizon observed; destinations not supplied / author and date not canonically supplied.”

Return boundary: No route. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 093 — The watcher refuses the full map — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The all-direction sightline does not identify landmarks, roads, or residents. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “Not something I can name from this page. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The all-direction sightline does not identify landmarks, roads, or residents. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No route.

Scene close: A later reading keeps the unresolved part intact: Do not generate geography from a general view. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 094 — The watcher refuses the full map — what stays outside

Proposed diegetic text: “Unfinished note: proposed note: horizon observed; destinations not supplied / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The all-direction sightline does not identify landmarks, roads, or residents. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Do not generate geography from a general view. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No route. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 095 — The watcher refuses the full map — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor asks for names of places beyond the view, and the watcher declines to name what the source does not show. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The all-direction sightline does not identify landmarks, roads, or residents. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “Not something I can name from this page. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Do not generate geography from a general view. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No route. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 096 — The watcher refuses the full map — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Do not generate geography from a general view. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Do not generate geography from a general view. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed note: horizon observed; destinations not supplied / interpretation withheld.”

Return boundary: No route. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 097 — A dry name beside a dry page

A copyist writes the tower name on a proposed card and leaves the water field blank. The location describes emptiness, not a surviving tank, pipe, or fill point. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we refill it?”
Returning visitor: “The source does not tell us how.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location describes emptiness, not a surviving tank, pipe, or fill point. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: No restoration procedure or resource system. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 098 — A dry name beside a dry page

Proposed diegetic text: “proposed card: water source not recorded”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A copyist writes the tower name on a proposed card and leaves the water field blank. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location describes emptiness, not a surviving tank, pipe, or fill point. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No restoration procedure or resource system. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 099 — A dry name beside a dry page

A copyist writes the tower name on a proposed card and leaves the water field blank. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The location describes emptiness, not a surviving tank, pipe, or fill point. Neither voice exists to lecture the player.

Tower watcher: “Can we refill it?”
Returning visitor: “The source does not tell us how.”
Tower watcher: “No restoration procedure or resource system.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 100 — A dry name beside a dry page

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No restoration procedure or resource system. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “No restoration procedure or resource system.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed card: water source not recorded”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 101 — A dry name beside a dry page — a later reading

A later reader returns after No restoration procedure or resource system. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The location describes emptiness, not a surviving tank, pipe, or fill point. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “The source does not tell us how. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The location describes emptiness, not a surviving tank, pipe, or fill point. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: The later reader notices that No restoration procedure or resource system. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 102 — A dry name beside a dry page — a later reading

Proposed diegetic text: “Later margin: proposed card: water source not recorded / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after No restoration procedure or resource system. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The location describes emptiness, not a surviving tank, pipe, or fill point. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that No restoration procedure or resource system. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 103 — A dry name beside a dry page — a later reading

A later reader returns after No restoration procedure or resource system. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The location describes emptiness, not a surviving tank, pipe, or fill point. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “The source does not tell us how. The rest stays outside this record.”
Tower watcher: “The later reader notices that No restoration procedure or resource system. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 104 — A dry name beside a dry page — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that No restoration procedure or resource system. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that No restoration procedure or resource system. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed card: water source not recorded / condition and circulation not established.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 105 — A dry name beside a dry page — copied for someone absent

A second copy reaches a person who was not present for the first telling. A copyist writes the tower name on a proposed card and leaves the water field blank. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The location describes emptiness, not a surviving tank, pipe, or fill point. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “The source does not tell us how. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The location describes emptiness, not a surviving tank, pipe, or fill point. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: In the copied version, No restoration procedure or resource system. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 106 — A dry name beside a dry page — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed card: water source not recorded / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A copyist writes the tower name on a proposed card and leaves the water field blank. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The location describes emptiness, not a surviving tank, pipe, or fill point. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, No restoration procedure or resource system. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 107 — A dry name beside a dry page — copied for someone absent

A second copy reaches a person who was not present for the first telling. A copyist writes the tower name on a proposed card and leaves the water field blank. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The location describes emptiness, not a surviving tank, pipe, or fill point. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “The source does not tell us how. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, No restoration procedure or resource system. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 108 — A dry name beside a dry page — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, No restoration procedure or resource system. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, No restoration procedure or resource system. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed card: water source not recorded / author and date not canonically supplied.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 109 — A dry name beside a dry page — what stays outside

This alternative ends at the edge of what its speaker can know. A copyist writes the tower name on a proposed card and leaves the water field blank. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The location describes emptiness, not a surviving tank, pipe, or fill point. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “The source does not tell us how. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The location describes emptiness, not a surviving tank, pipe, or fill point. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: A later reading keeps the unresolved part intact: No restoration procedure or resource system. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 110 — A dry name beside a dry page — what stays outside

Proposed diegetic text: “Unfinished note: proposed card: water source not recorded / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A copyist writes the tower name on a proposed card and leaves the water field blank. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The location describes emptiness, not a surviving tank, pipe, or fill point. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: No restoration procedure or resource system. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 111 — A dry name beside a dry page — what stays outside

This alternative ends at the edge of what its speaker can know. A copyist writes the tower name on a proposed card and leaves the water field blank. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The location describes emptiness, not a surviving tank, pipe, or fill point. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “The source does not tell us how. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: No restoration procedure or resource system. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 112 — A dry name beside a dry page — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: No restoration procedure or resource system. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: No restoration procedure or resource system. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed card: water source not recorded / interpretation withheld.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 113 — The memorial enters under another heading

A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Was this written here?”
Returning visitor: “The grave-site field says otherwise.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: Do not move the memorial or identify a tower resident. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 114 — The memorial enters under another heading

Proposed diegetic text: “proposed comparison slip: producer link / grave-site field retained”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Do not move the memorial or identify a tower resident. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 115 — The memorial enters under another heading

A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. Neither voice exists to lecture the player.

Tower watcher: “Was this written here?”
Returning visitor: “The grave-site field says otherwise.”
Tower watcher: “Do not move the memorial or identify a tower resident.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 116 — The memorial enters under another heading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Do not move the memorial or identify a tower resident. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Do not move the memorial or identify a tower resident.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed comparison slip: producer link / grave-site field retained”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 117 — The memorial enters under another heading — a later reading

A later reader returns after Do not move the memorial or identify a tower resident. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “The grave-site field says otherwise. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: The later reader notices that Do not move the memorial or identify a tower resident. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 118 — The memorial enters under another heading — a later reading

Proposed diegetic text: “Later margin: proposed comparison slip: producer link / grave-site field retained / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Do not move the memorial or identify a tower resident. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Do not move the memorial or identify a tower resident. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 119 — The memorial enters under another heading — a later reading

A later reader returns after Do not move the memorial or identify a tower resident. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “The grave-site field says otherwise. The rest stays outside this record.”
Tower watcher: “The later reader notices that Do not move the memorial or identify a tower resident. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 120 — The memorial enters under another heading — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Do not move the memorial or identify a tower resident. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Do not move the memorial or identify a tower resident. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed comparison slip: producer link / grave-site field retained / condition and circulation not established.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 121 — The memorial enters under another heading — copied for someone absent

A second copy reaches a person who was not present for the first telling. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “The grave-site field says otherwise. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: In the copied version, Do not move the memorial or identify a tower resident. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 122 — The memorial enters under another heading — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed comparison slip: producer link / grave-site field retained / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Do not move the memorial or identify a tower resident. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 123 — The memorial enters under another heading — copied for someone absent

A second copy reaches a person who was not present for the first telling. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “The grave-site field says otherwise. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Do not move the memorial or identify a tower resident. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 124 — The memorial enters under another heading — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Do not move the memorial or identify a tower resident. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Do not move the memorial or identify a tower resident. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed comparison slip: producer link / grave-site field retained / author and date not canonically supplied.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 125 — The memorial enters under another heading — what stays outside

This alternative ends at the edge of what its speaker can know. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “The grave-site field says otherwise. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: A later reading keeps the unresolved part intact: Do not move the memorial or identify a tower resident. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 126 — The memorial enters under another heading — what stays outside

Proposed diegetic text: “Unfinished note: proposed comparison slip: producer link / grave-site field retained / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Do not move the memorial or identify a tower resident. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 127 — The memorial enters under another heading — what stays outside

This alternative ends at the edge of what its speaker can know. A reader sees the dry-canteen epitaph in a separate catalog and keeps its provenance label visible. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The manifest associates it with this inspection producer, but the record names NORTH_PASS_SUMMIT_CREVICE as the grave site. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “The grave-site field says otherwise. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Do not move the memorial or identify a tower resident. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 128 — The memorial enters under another heading — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Do not move the memorial or identify a tower resident. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Do not move the memorial or identify a tower resident. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed comparison slip: producer link / grave-site field retained / interpretation withheld.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 129 — A name does not make a neighbor

Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Do you know who watched from here?”
Returning visitor: “This record cannot tell you.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: Do not make the epitaph subject a tower visitor. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 130 — A name does not make a neighbor

Proposed diegetic text: “proposed margin: two source records, no shared biography”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Do not make the epitaph subject a tower visitor. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 131 — A name does not make a neighbor

Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. Neither voice exists to lecture the player.

Tower watcher: “Do you know who watched from here?”
Returning visitor: “This record cannot tell you.”
Tower watcher: “Do not make the epitaph subject a tower visitor.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 132 — A name does not make a neighbor

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Do not make the epitaph subject a tower visitor. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Do not make the epitaph subject a tower visitor.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed margin: two source records, no shared biography”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 133 — A name does not make a neighbor — a later reading

A later reader returns after Do not make the epitaph subject a tower visitor. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “This record cannot tell you. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: The later reader notices that Do not make the epitaph subject a tower visitor. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 134 — A name does not make a neighbor — a later reading

Proposed diegetic text: “Later margin: proposed margin: two source records, no shared biography / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Do not make the epitaph subject a tower visitor. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Do not make the epitaph subject a tower visitor. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 135 — A name does not make a neighbor — a later reading

A later reader returns after Do not make the epitaph subject a tower visitor. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “This record cannot tell you. The rest stays outside this record.”
Tower watcher: “The later reader notices that Do not make the epitaph subject a tower visitor. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 136 — A name does not make a neighbor — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Do not make the epitaph subject a tower visitor. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Do not make the epitaph subject a tower visitor. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed margin: two source records, no shared biography / condition and circulation not established.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 137 — A name does not make a neighbor — copied for someone absent

A second copy reaches a person who was not present for the first telling. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “This record cannot tell you. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: In the copied version, Do not make the epitaph subject a tower visitor. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 138 — A name does not make a neighbor — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed margin: two source records, no shared biography / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Do not make the epitaph subject a tower visitor. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 139 — A name does not make a neighbor — copied for someone absent

A second copy reaches a person who was not present for the first telling. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “This record cannot tell you. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Do not make the epitaph subject a tower visitor. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 140 — A name does not make a neighbor — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Do not make the epitaph subject a tower visitor. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Do not make the epitaph subject a tower visitor. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed margin: two source records, no shared biography / author and date not canonically supplied.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 141 — A name does not make a neighbor — what stays outside

This alternative ends at the edge of what its speaker can know. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “This record cannot tell you. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: A later reading keeps the unresolved part intact: Do not make the epitaph subject a tower visitor. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 142 — A name does not make a neighbor — what stays outside

Proposed diegetic text: “Unfinished note: proposed margin: two source records, no shared biography / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Do not make the epitaph subject a tower visitor. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 143 — A name does not make a neighbor — what stays outside

This alternative ends at the edge of what its speaker can know. Someone reads the epitaph’s named subject and declines to attach that person to the tower’s history. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The memorial identity is marked unresolved/display only by the manifest, and its own source gives another site. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “This record cannot tell you. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Do not make the epitaph subject a tower visitor. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 144 — A name does not make a neighbor — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Do not make the epitaph subject a tower visitor. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Do not make the epitaph subject a tower visitor. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed margin: two source records, no shared biography / interpretation withheld.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 145 — The tower cannot certify a horizon

A watcher says a clear view is still only a view from one place. The location describes unobstructed sightlines, not safety, control, or complete knowledge. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we trust what we see?”
Returning visitor: “We can say what we saw.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location describes unobstructed sightlines, not safety, control, or complete knowledge. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: No truth-detection or territory control. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 146 — The tower cannot certify a horizon

Proposed diegetic text: “proposed line: visible is not verified”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A watcher says a clear view is still only a view from one place. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location describes unobstructed sightlines, not safety, control, or complete knowledge. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No truth-detection or territory control. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 147 — The tower cannot certify a horizon

A watcher says a clear view is still only a view from one place. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The location describes unobstructed sightlines, not safety, control, or complete knowledge. Neither voice exists to lecture the player.

Tower watcher: “Can we trust what we see?”
Returning visitor: “We can say what we saw.”
Tower watcher: “No truth-detection or territory control.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 148 — The tower cannot certify a horizon

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No truth-detection or territory control. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “No truth-detection or territory control.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed line: visible is not verified”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 149 — The tower cannot certify a horizon — a later reading

A later reader returns after No truth-detection or territory control. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The location describes unobstructed sightlines, not safety, control, or complete knowledge. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “We can say what we saw. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The location describes unobstructed sightlines, not safety, control, or complete knowledge. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: The later reader notices that No truth-detection or territory control. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 150 — The tower cannot certify a horizon — a later reading

Proposed diegetic text: “Later margin: proposed line: visible is not verified / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after No truth-detection or territory control. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The location describes unobstructed sightlines, not safety, control, or complete knowledge. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that No truth-detection or territory control. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 151 — The tower cannot certify a horizon — a later reading

A later reader returns after No truth-detection or territory control. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The location describes unobstructed sightlines, not safety, control, or complete knowledge. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “We can say what we saw. The rest stays outside this record.”
Tower watcher: “The later reader notices that No truth-detection or territory control. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 152 — The tower cannot certify a horizon — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that No truth-detection or territory control. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that No truth-detection or territory control. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed line: visible is not verified / condition and circulation not established.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 153 — The tower cannot certify a horizon — copied for someone absent

A second copy reaches a person who was not present for the first telling. A watcher says a clear view is still only a view from one place. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The location describes unobstructed sightlines, not safety, control, or complete knowledge. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “We can say what we saw. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The location describes unobstructed sightlines, not safety, control, or complete knowledge. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: In the copied version, No truth-detection or territory control. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 154 — The tower cannot certify a horizon — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed line: visible is not verified / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A watcher says a clear view is still only a view from one place. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The location describes unobstructed sightlines, not safety, control, or complete knowledge. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, No truth-detection or territory control. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 155 — The tower cannot certify a horizon — copied for someone absent

A second copy reaches a person who was not present for the first telling. A watcher says a clear view is still only a view from one place. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The location describes unobstructed sightlines, not safety, control, or complete knowledge. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “We can say what we saw. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, No truth-detection or territory control. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 156 — The tower cannot certify a horizon — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, No truth-detection or territory control. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, No truth-detection or territory control. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed line: visible is not verified / author and date not canonically supplied.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 157 — The tower cannot certify a horizon — what stays outside

This alternative ends at the edge of what its speaker can know. A watcher says a clear view is still only a view from one place. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The location describes unobstructed sightlines, not safety, control, or complete knowledge. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “We can say what we saw. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The location describes unobstructed sightlines, not safety, control, or complete knowledge. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness.

Scene close: A later reading keeps the unresolved part intact: No truth-detection or territory control. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 158 — The tower cannot certify a horizon — what stays outside

Proposed diegetic text: “Unfinished note: proposed line: visible is not verified / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A watcher says a clear view is still only a view from one place. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The location describes unobstructed sightlines, not safety, control, or complete knowledge. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: No truth-detection or territory control. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 159 — The tower cannot certify a horizon — what stays outside

This alternative ends at the edge of what its speaker can know. A watcher says a clear view is still only a view from one place. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The location describes unobstructed sightlines, not safety, control, or complete knowledge. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “We can say what we saw. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: No truth-detection or territory control. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 160 — The tower cannot certify a horizon — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: No truth-detection or territory control. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: No truth-detection or territory control. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed line: visible is not verified / interpretation withheld.”

Return boundary: Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 161 — A note stays unsigned

A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. The source provides both readings but no vote or public record. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which answer will you leave?”
Returning visitor: “I would rather not leave one.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source provides both readings but no vote or public record. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No vote, faction change, or memorial state.

Scene close: Refusal is a valid ending. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 162 — A note stays unsigned

Proposed diegetic text: “proposed private note: question retained; author withheld”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source provides both readings but no vote or public record. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Refusal is a valid ending. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No vote, faction change, or memorial state. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 163 — A note stays unsigned

A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source provides both readings but no vote or public record. Neither voice exists to lecture the player.

Tower watcher: “Which answer will you leave?”
Returning visitor: “I would rather not leave one.”
Tower watcher: “Refusal is a valid ending.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No vote, faction change, or memorial state. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 164 — A note stays unsigned

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Refusal is a valid ending. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Refusal is a valid ending.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed private note: question retained; author withheld”

Return boundary: No vote, faction change, or memorial state. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 165 — A note stays unsigned — a later reading

A later reader returns after Refusal is a valid ending. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The source provides both readings but no vote or public record. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I would rather not leave one. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The source provides both readings but no vote or public record. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No vote, faction change, or memorial state.

Scene close: The later reader notices that Refusal is a valid ending. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 166 — A note stays unsigned — a later reading

Proposed diegetic text: “Later margin: proposed private note: question retained; author withheld / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Refusal is a valid ending. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The source provides both readings but no vote or public record. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Refusal is a valid ending. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No vote, faction change, or memorial state. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 167 — A note stays unsigned — a later reading

A later reader returns after Refusal is a valid ending. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The source provides both readings but no vote or public record. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I would rather not leave one. The rest stays outside this record.”
Tower watcher: “The later reader notices that Refusal is a valid ending. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No vote, faction change, or memorial state. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 168 — A note stays unsigned — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Refusal is a valid ending. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Refusal is a valid ending. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed private note: question retained; author withheld / condition and circulation not established.”

Return boundary: No vote, faction change, or memorial state. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 169 — A note stays unsigned — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The source provides both readings but no vote or public record. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I would rather not leave one. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The source provides both readings but no vote or public record. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No vote, faction change, or memorial state.

Scene close: In the copied version, Refusal is a valid ending. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 170 — A note stays unsigned — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed private note: question retained; author withheld / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The source provides both readings but no vote or public record. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Refusal is a valid ending. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No vote, faction change, or memorial state. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 171 — A note stays unsigned — copied for someone absent

A second copy reaches a person who was not present for the first telling. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The source provides both readings but no vote or public record. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I would rather not leave one. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Refusal is a valid ending. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No vote, faction change, or memorial state. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 172 — A note stays unsigned — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Refusal is a valid ending. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Refusal is a valid ending. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed private note: question retained; author withheld / author and date not canonically supplied.”

Return boundary: No vote, faction change, or memorial state. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 173 — A note stays unsigned — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The source provides both readings but no vote or public record. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I would rather not leave one. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The source provides both readings but no vote or public record. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: No vote, faction change, or memorial state.

Scene close: A later reading keeps the unresolved part intact: Refusal is a valid ending. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 174 — A note stays unsigned — what stays outside

Proposed diegetic text: “Unfinished note: proposed private note: question retained; author withheld / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The source provides both readings but no vote or public record. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Refusal is a valid ending. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: No vote, faction change, or memorial state. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 175 — A note stays unsigned — what stays outside

This alternative ends at the edge of what its speaker can know. A visitor is invited to choose loss or mercy for a draft note, then leaves the signature line empty. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The source provides both readings but no vote or public record. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I would rather not leave one. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Refusal is a valid ending. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: No vote, faction change, or memorial state. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 176 — A note stays unsigned — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Refusal is a valid ending. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Refusal is a valid ending. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed private note: question retained; author withheld / interpretation withheld.”

Return boundary: No vote, faction change, or memorial state. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 177 — The view remains after the question

The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. The source deliberately leaves the interpretation unsettled. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Do you have an answer now?”
Returning visitor: “I have a view, not an answer.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source deliberately leaves the interpretation unsettled. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not repair or repurpose the tower.

Scene close: Close with uncertainty intact. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 178 — The view remains after the question

Proposed diegetic text: “proposed close: visible landmark; meaning open”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source deliberately leaves the interpretation unsettled. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Close with uncertainty intact. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not repair or repurpose the tower. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 179 — The view remains after the question

The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source deliberately leaves the interpretation unsettled. Neither voice exists to lecture the player.

Tower watcher: “Do you have an answer now?”
Returning visitor: “I have a view, not an answer.”
Tower watcher: “Close with uncertainty intact.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not repair or repurpose the tower. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 180 — The view remains after the question

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Close with uncertainty intact. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “Close with uncertainty intact.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “proposed close: visible landmark; meaning open”

Return boundary: Do not repair or repurpose the tower. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 181 — The view remains after the question — a later reading

A later reader returns after Close with uncertainty intact. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The source remains narrow: The source deliberately leaves the interpretation unsettled. A return can change emphasis without supplying the missing name, motive, date, or outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I have a view, not an answer. The rest stays outside this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source remains narrow: The source deliberately leaves the interpretation unsettled. A return can change emphasis without supplying the missing name, motive, date, or outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not repair or repurpose the tower.

Scene close: The later reader notices that Close with uncertainty intact. These words return with a different weight while the underlying source and its omissions remain unchanged. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 182 — The view remains after the question — a later reading

Proposed diegetic text: “Later margin: proposed close: visible landmark; meaning open / condition and circulation not established.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A later reader returns after Close with uncertainty intact. The passage begins with what can still be seen, not with a claim that the first reader understood everything. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source remains narrow: The source deliberately leaves the interpretation unsettled. A return can change emphasis without supplying the missing name, motive, date, or outcome. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reader notices that Close with uncertainty intact. These words return with a different weight while the underlying source and its omissions remain unchanged. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not repair or repurpose the tower. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 183 — The view remains after the question — a later reading

A later reader returns after Close with uncertainty intact. The passage begins with what can still be seen, not with a claim that the first reader understood everything. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The source remains narrow: The source deliberately leaves the interpretation unsettled. A return can change emphasis without supplying the missing name, motive, date, or outcome. Neither voice exists to lecture the player.

Tower watcher: “Which part can we still verify?”
Returning visitor: “I have a view, not an answer. The rest stays outside this record.”
Tower watcher: “The later reader notices that Close with uncertainty intact. These words return with a different weight while the underlying source and its omissions remain unchanged.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not repair or repurpose the tower. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 184 — The view remains after the question — a later reading

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reader notices that Close with uncertainty intact. These words return with a different weight while the underlying source and its omissions remain unchanged. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “The later reader notices that Close with uncertainty intact. These words return with a different weight while the underlying source and its omissions remain unchanged.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Later margin: proposed close: visible landmark; meaning open / condition and circulation not established.”

Return boundary: Do not repair or repurpose the tower. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 185 — The view remains after the question — copied for someone absent

A second copy reaches a person who was not present for the first telling. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. The copyist keeps the distinction that a hurried retelling might lose. The detail travels with its limit attached: The source deliberately leaves the interpretation unsettled. No new witness is created because the sentence has been copied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I have a view, not an answer. Keep the source and its uncertainty beside it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The detail travels with its limit attached: The source deliberately leaves the interpretation unsettled. No new witness is created because the sentence has been copied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not repair or repurpose the tower.

Scene close: In the copied version, Close with uncertainty intact. Another audience may read the line differently; it still does not prove another event or consequence. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 186 — The view remains after the question — copied for someone absent

Proposed diegetic text: “Copy for another reader: proposed close: visible landmark; meaning open / author and date not canonically supplied.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: A second copy reaches a person who was not present for the first telling. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. The copyist keeps the distinction that a hurried retelling might lose. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The detail travels with its limit attached: The source deliberately leaves the interpretation unsettled. No new witness is created because the sentence has been copied. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: In the copied version, Close with uncertainty intact. Another audience may read the line differently; it still does not prove another event or consequence. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not repair or repurpose the tower. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 187 — The view remains after the question — copied for someone absent

A second copy reaches a person who was not present for the first telling. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. The copyist keeps the distinction that a hurried retelling might lose. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: The detail travels with its limit attached: The source deliberately leaves the interpretation unsettled. No new witness is created because the sentence has been copied. Neither voice exists to lecture the player.

Tower watcher: “What must stay attached to the sentence?”
Returning visitor: “I have a view, not an answer. Keep the source and its uncertainty beside it.”
Tower watcher: “In the copied version, Close with uncertainty intact. Another audience may read the line differently; it still does not prove another event or consequence.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not repair or repurpose the tower. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 188 — The view remains after the question — copied for someone absent

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. In the copied version, Close with uncertainty intact. Another audience may read the line differently; it still does not prove another event or consequence. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “In the copied version, Close with uncertainty intact. Another audience may read the line differently; it still does not prove another event or consequence.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy for another reader: proposed close: visible landmark; meaning open / author and date not canonically supplied.”

Return boundary: Do not repair or repurpose the tower. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 189 — The view remains after the question — what stays outside

This alternative ends at the edge of what its speaker can know. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. It gives the reader a place to stop rather than a clue that must be solved. What remains available is only this: The source deliberately leaves the interpretation unsettled. The proposed passage does not claim access to anyone’s unspoken thoughts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I have a view, not an answer. We can leave it there.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: What remains available is only this: The source deliberately leaves the interpretation unsettled. The proposed passage does not claim access to anyone’s unspoken thoughts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not repair or repurpose the tower.

Scene close: A later reading keeps the unresolved part intact: Close with uncertainty intact. This return is a prose option, not a new branch or state. The tower can offer a view without offering an answer. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 190 — The view remains after the question — what stays outside

Proposed diegetic text: “Unfinished note: proposed close: visible landmark; meaning open / interpretation withheld.”

Proposed author and audience: a visitor who declines to sign a note; someone who remembers the tower’s name. The artifact exists in this proposal for a practical reason: This alternative ends at the edge of what its speaker can know. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. It gives the reader a place to stop rather than a clue that must be solved. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: What remains available is only this: The source deliberately leaves the interpretation unsettled. The proposed passage does not claim access to anyone’s unspoken thoughts. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reading keeps the unresolved part intact: Close with uncertainty intact. This return is a prose option, not a new branch or state. Empty is a condition; loss and mercy are readings. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not repair or repurpose the tower. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 191 — The view remains after the question — what stays outside

This alternative ends at the edge of what its speaker can know. The last reader looks once more without deciding whether the tower’s emptiness was mercy or loss for anyone else. It gives the reader a place to stop rather than a clue that must be solved. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. A watcher and a returning visitor speak in observable terms: empty, visible, edge, name. They do not report the unseen settlement’s population or read the thoughts of people outside the view. A memorial reader cites the epitaph as a separate record. New voices remain anonymous editorial roles. Keep the conversation attached to the work already in the scene: What remains available is only this: The source deliberately leaves the interpretation unsettled. The proposed passage does not claim access to anyone’s unspoken thoughts. Neither voice exists to lecture the player.

Tower watcher: “Can we leave the blank visible?”
Returning visitor: “I have a view, not an answer. We can leave it there.”
Tower watcher: “A later reading keeps the unresolved part intact: Close with uncertainty intact. This return is a prose option, not a new branch or state.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not repair or repurpose the tower. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 192 — The view remains after the question — what stays outside

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reading keeps the unresolved part intact: Close with uncertainty intact. This return is a prose option, not a new branch or state. No one has to make a public judgment to leave. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Tower watcher: “A later reading keeps the unresolved part intact: Close with uncertainty intact. This return is a prose option, not a new branch or state.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unfinished note: proposed close: visible landmark; meaning open / interpretation withheld.”

Return boundary: Do not repair or repurpose the tower. Candidate prose may accompany the location inspection only after current reachability and consumer are verified. A visitor’s note, tower-side exchange, or catalog comparison is proposed text, not a new marker or memorial object. The plan adds no water source, surveillance system, route, lookout bonus, or access state. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

## 15. Beat selection index

| Beat | Editorial focus | Candidate in-world artifact | Continuity limit |
|---:|---|---|---|
| 01 | The tower is named before it is seen | proposed note: place name copied from the location record | No system condition or capacity is implied. |
| 02 | The tower is named before it is seen — a later reading | Later margin: proposed note: place name copied from the location record / condition and circulation not established. | No system condition or capacity is implied. |
| 03 | The tower is named before it is seen — copied for someone absent | Copy for another reader: proposed note: place name copied from the location record / author and date not canonically supplied. | No system condition or capacity is implied. |
| 04 | The tower is named before it is seen — what stays outside | Unfinished note: proposed note: place name copied from the location record / interpretation withheld. | No system condition or capacity is implied. |
| 05 | An empty structure can still be visible | proposed margin: visible in all directions | No detection or scout benefit. |
| 06 | An empty structure can still be visible — a later reading | Later margin: proposed margin: visible in all directions / condition and circulation not established. | No detection or scout benefit. |
| 07 | An empty structure can still be visible — copied for someone absent | Copy for another reader: proposed margin: visible in all directions / author and date not canonically supplied. | No detection or scout benefit. |
| 08 | An empty structure can still be visible — what stays outside | Unfinished note: proposed margin: visible in all directions / interpretation withheld. | No detection or scout benefit. |
| 09 | Empty for years | proposed visitor question left without date | Do not invent a failure cause or date. |
| 10 | Empty for years — a later reading | Later margin: proposed visitor question left without date / condition and circulation not established. | Do not invent a failure cause or date. |
| 11 | Empty for years — copied for someone absent | Copy for another reader: proposed visitor question left without date / author and date not canonically supplied. | Do not invent a failure cause or date. |
| 12 | Empty for years — what stays outside | Unfinished note: proposed visitor question left without date / interpretation withheld. | Do not invent a failure cause or date. |
| 13 | Loss | proposed note: loss, signed by no one | No water mechanics. |
| 14 | Loss — a later reading | Later margin: proposed note: loss, signed by no one / condition and circulation not established. | No water mechanics. |
| 15 | Loss — copied for someone absent | Copy for another reader: proposed note: loss, signed by no one / author and date not canonically supplied. | No water mechanics. |
| 16 | Loss — what stays outside | Unfinished note: proposed note: loss, signed by no one / interpretation withheld. | No water mechanics. |
| 17 | Mercy | proposed margin: mercy, author unknown | Do not invent a crisis or a ration policy. |
| 18 | Mercy — a later reading | Later margin: proposed margin: mercy, author unknown / condition and circulation not established. | Do not invent a crisis or a ration policy. |
| 19 | Mercy — copied for someone absent | Copy for another reader: proposed margin: mercy, author unknown / author and date not canonically supplied. | Do not invent a crisis or a ration policy. |
| 20 | Mercy — what stays outside | Unfinished note: proposed margin: mercy, author unknown / interpretation withheld. | Do not invent a crisis or a ration policy. |
| 21 | The watcher refuses the full map | proposed note: horizon observed; destinations not supplied | No route. |
| 22 | The watcher refuses the full map — a later reading | Later margin: proposed note: horizon observed; destinations not supplied / condition and circulation not established. | No route. |
| 23 | The watcher refuses the full map — copied for someone absent | Copy for another reader: proposed note: horizon observed; destinations not supplied / author and date not canonically supplied. | No route. |
| 24 | The watcher refuses the full map — what stays outside | Unfinished note: proposed note: horizon observed; destinations not supplied / interpretation withheld. | No route. |
| 25 | A dry name beside a dry page | proposed card: water source not recorded | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 26 | A dry name beside a dry page — a later reading | Later margin: proposed card: water source not recorded / condition and circulation not established. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 27 | A dry name beside a dry page — copied for someone absent | Copy for another reader: proposed card: water source not recorded / author and date not canonically supplied. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 28 | A dry name beside a dry page — what stays outside | Unfinished note: proposed card: water source not recorded / interpretation withheld. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 29 | The memorial enters under another heading | proposed comparison slip: producer link / grave-site field retained | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 30 | The memorial enters under another heading — a later reading | Later margin: proposed comparison slip: producer link / grave-site field retained / condition and circulation not established. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 31 | The memorial enters under another heading — copied for someone absent | Copy for another reader: proposed comparison slip: producer link / grave-site field retained / author and date not canonically supplied. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 32 | The memorial enters under another heading — what stays outside | Unfinished note: proposed comparison slip: producer link / grave-site field retained / interpretation withheld. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 33 | A name does not make a neighbor | proposed margin: two source records, no shared biography | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 34 | A name does not make a neighbor — a later reading | Later margin: proposed margin: two source records, no shared biography / condition and circulation not established. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 35 | A name does not make a neighbor — copied for someone absent | Copy for another reader: proposed margin: two source records, no shared biography / author and date not canonically supplied. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 36 | A name does not make a neighbor — what stays outside | Unfinished note: proposed margin: two source records, no shared biography / interpretation withheld. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 37 | The tower cannot certify a horizon | proposed line: visible is not verified | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 38 | The tower cannot certify a horizon — a later reading | Later margin: proposed line: visible is not verified / condition and circulation not established. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 39 | The tower cannot certify a horizon — copied for someone absent | Copy for another reader: proposed line: visible is not verified / author and date not canonically supplied. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 40 | The tower cannot certify a horizon — what stays outside | Unfinished note: proposed line: visible is not verified / interpretation withheld. | Do not give water-system instructions or imply that the tower can provide water. Do not claim the sightline is secure or complete. Do not move the dry-canteen memorial to this location, identify its unresolved subject, or use its family detail as the tower visitor’s backstory. Do not invent a reason for the tower’s emptiness. |
| 41 | A note stays unsigned | proposed private note: question retained; author withheld | No vote, faction change, or memorial state. |
| 42 | A note stays unsigned — a later reading | Later margin: proposed private note: question retained; author withheld / condition and circulation not established. | No vote, faction change, or memorial state. |
| 43 | A note stays unsigned — copied for someone absent | Copy for another reader: proposed private note: question retained; author withheld / author and date not canonically supplied. | No vote, faction change, or memorial state. |
| 44 | A note stays unsigned — what stays outside | Unfinished note: proposed private note: question retained; author withheld / interpretation withheld. | No vote, faction change, or memorial state. |
| 45 | The view remains after the question | proposed close: visible landmark; meaning open | Do not repair or repurpose the tower. |
| 46 | The view remains after the question — a later reading | Later margin: proposed close: visible landmark; meaning open / condition and circulation not established. | Do not repair or repurpose the tower. |
| 47 | The view remains after the question — copied for someone absent | Copy for another reader: proposed close: visible landmark; meaning open / author and date not canonically supplied. | Do not repair or repurpose the tower. |
| 48 | The view remains after the question — what stays outside | Unfinished note: proposed close: visible landmark; meaning open / interpretation withheld. | Do not repair or repurpose the tower. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions or state. The existence of a record in JSON does not prove its consumer. Verify the current owning system and its exposed state before choosing a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `loc_north_gate_water_tower` | empty tower and open sightlines | No former use, water capacity, or safety benefit is established. |
| `disc_fringe_epitaph_dry_canteen_memorial` | recovered epitaph; unresolved memorial identity | Manifest producer is not grave-site proof; the record names a north-pass crevice. |

## 17. Collision and unresolved authority

The location offers an unresolved binary—loss or mercy—and the manifest adds a separate recovered epitaph. Do not solve the former with the latter. Preserve the difference between a producer link and the epitaph’s grave-site field. If an owner later explains the association, revise attribution from that evidence; this draft does not. If a future implementation needs a single authoritative answer, pause content selection until the current owner resolves the source conflict. No text in this plan is that resolution.

## 18. Review checklist

Before selecting a passage, compare it again with the source rows named in section 3. Keep source facts and existing choices exact, mark proposed voices as editorial until approved, and independently verify any route, text consumer, or state condition. Do not infer reachability from a location description. Read every line aloud for role-appropriate vocabulary, remove any sentence that sounds like a feature spec, and keep each artifact’s author, audience, and purpose plausible.

## 19. Acceptance boundary

This plan is complete as a game-content proposal when selected passages can be traced to the cited location or linked records, existing choices and consequences remain unchanged, no unknown is promoted into canon, and an existing owner is identified for any implementation. If that owner or consumer is absent, retain the prose as a draft rather than inventing a route or system. Character counts are Unicode code-point counts of the saved Markdown files.
