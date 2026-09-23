# EXPANSION 115 — Walk Until the Lines Change

## Janek Orel, two old winters, and the dawn when the map stops feeding the recovery yard.

### Wave 22: What the Account Cannot Hold

## 1. Expansion thesis

Treat Janek’s map as inherited knowledge that has begun to fail, not as a route-finding minigame. The player may walk new lines with him at dawn or ask him to quote the old routes as truth. The first path lets the map change out loud; the second preserves confidence until the settlement discovers which map he was quoting. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character story or settlement dialogue surface. It adds no gameplay feature and does not claim that proposed text is already in the game.

## 2. Story in one sentence

A hunter takes two sets of snowshoes to the recovery yard gate before first light and waits for someone willing to walk beside uncertainty.

## 3. Verified local anchor and current story

`characters.json` defines `npc_janek_orel`, Hunter at `loc_ordnance_shoulder`. Old lines taught by his father fed the recovery yard through two winters, but have stopped working; Janek blames weather, raiders, the queue, and ash in that order while quietly walking new lines at dawn. `npc_arcs.json` contains initial, evolved, late-scout, late-discredited, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_janek_01_dawn`; `quests_npc_arcs.json` registers `quest_arc_janek_01_dawn`. The current choice IDs are `janek_walk_dawn` and `janek_quote_old_lines`. Walking adds three routes to the dawn map and the meat comes back heavier this season; late scout teaches true migration routes and retires false ones out loud. Asking for the old lines as truth leads to two hungry weeks, the settlement learning which map he was quoting, and Janek walking alone at dawn correcting it in silence. Do not name the routes, species, or cause of the changed tracks. Names, locations, quest IDs, choice IDs, and state summaries above come from current local data. The registered encounter or character record proves the authored premise and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing condition.

## 4. Fixed canon and proposed prose

The old lines ran east; lately the tracks do not. Janek is mostly willing to say which old parts have stopped, but not before dawn. The map is wrong quietly and he knows it. Preserve the three added routes, heavier meat, hungry weeks, late teaching, and discredited result exactly as the relevant states specify. This plan supplies no route coordinates or hunting instructions. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to a character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Janek’s inherited map kept a settlement fed, so questioning it means questioning the years when it worked and the person who taught it. He is not foolish for trusting it, and the player is not wise merely for noticing change. Dawn gives him a time to say what the rest of the day cannot yet hear. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–6 with `enc_arc_janek_01_dawn`. Beats 7–11 require `janek_walk_dawn`, then evolved or late-scout states. Beats 12–15 follow `janek_quote_old_lines` into late-discredited. The final beats may accompany initial or late-unmet states. Keep the exact route lines undescribed and do not show both late outcomes together. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character/settlement dialogue owner as the insertion point. Where the inspected data has no such route, keep the text as an editorial proposal and do not imply a new encounter has been approved.

## 8. Player agency and consequence

The current choice is to walk the dawn routes and let the new map happen out loud, or ask Janek for the old lines as truth and let the settlement believe. Do not create a neutral map answer or rescue the hungry-week consequence through an invented side path. The player can understand the risk before choosing. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before a choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Route and hunting details stay at the level of authored narrative; no navigable instructions or weapon use is added. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, electrical, navigation, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when implementation later chooses a smaller, coherent subset.

### Scene draft 001 — Two sets of snowshoes before first light

At the recovery yard gate before first light, Janek has two sets of snowshoes and no speech prepared. The encounter’s quiet is his whole invitation. He waits for the player to decide whether to walk beside him. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “You came. We can walk before the yard starts asking for certainty.”
Other voice: “The snow cover is thin by the yard. I will mark what we see, not guess at what is under it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add a departure time or route direction. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the invitation brief and before the choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — Two sets of snowshoes before first light

Proposed diegetic text: “Gate note: Two sets of snowshoes present before dawn.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the recovery yard gate before first light, Janek has two sets of snowshoes and no speech prepared. The encounter’s quiet is his whole invitation. He waits for the player to decide whether to walk beside him. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a departure time or route direction. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the invitation brief and before the choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 003 — Two sets of snowshoes before first light

At the recovery yard gate before first light, Janek has two sets of snowshoes and no speech prepared. The encounter’s quiet is his whole invitation. He waits for the player to decide whether to walk beside him. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “You came. We can walk before the yard starts asking for certainty.”
Other voice: “The snow cover is thin by the yard. I will mark what we see, not guess at what is under it.”
Janek Orel: “The morning is short. Walk if you want to see the marks yourself.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a departure time or route direction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — Two sets of snowshoes before first light

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. At the recovery yard gate before first light, Janek has two sets of snowshoes and no speech prepared. The encounter’s quiet is his whole invitation. He waits for the player to decide whether to walk beside him. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Keep the invitation brief and before the choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a departure time or route direction.

Janek Orel may say: “You came. We can walk before the yard starts asking for certainty.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Gate note: Two sets of snowshoes present before dawn. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — The old lines ran east

Janek remembers that the old lines ran east. The story does not repeat a coordinate or explain what made them work before. A remembered direction is not current proof. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “They worked. I am not pretending they did not.”
Other voice: “That old line took us east. It is not a promise it will take us there now.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No route details or guarantee about the old path. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Preserve both past usefulness and present doubt. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — The old lines ran east

Proposed diegetic text: “Map margin: Old lines ran east; current tracks differ.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Janek remembers that the old lines ran east. The story does not repeat a coordinate or explain what made them work before. A remembered direction is not current proof. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No route details or guarantee about the old path. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve both past usefulness and present doubt. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 007 — The old lines ran east

Janek remembers that the old lines ran east. The story does not repeat a coordinate or explain what made them work before. A remembered direction is not current proof. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “They worked. I am not pretending they did not.”
Other voice: “That old line took us east. It is not a promise it will take us there now.”
Janek Orel: “Preserve both past usefulness and present doubt.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No route details or guarantee about the old path. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — The old lines ran east

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. Janek remembers that the old lines ran east. The story does not repeat a coordinate or explain what made them work before. A remembered direction is not current proof. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Preserve both past usefulness and present doubt. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No route details or guarantee about the old path.

Janek Orel may say: “They worked. I am not pretending they did not.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Map margin: Old lines ran east; current tracks differ. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The tracks lately do not

The tracks lately do not match the old lines. Janek can point to that absence without identifying an animal or giving a method to follow it. The map is quietly wrong, not suddenly absurd. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “The old line is on paper. The tracks are not where it says.”
Other voice: “I followed the tracks through the yard. They stop making sense by the empty bins.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No species, track pattern, or cause. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Do not convert the mismatch into a map mechanic. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The tracks lately do not

Proposed diegetic text: “Dawn note: Current tracks do not match the old lines.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The tracks lately do not match the old lines. Janek can point to that absence without identifying an animal or giving a method to follow it. The map is quietly wrong, not suddenly absurd. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No species, track pattern, or cause. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not convert the mismatch into a map mechanic. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 011 — The tracks lately do not

The tracks lately do not match the old lines. Janek can point to that absence without identifying an animal or giving a method to follow it. The map is quietly wrong, not suddenly absurd. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “The old line is on paper. The tracks are not where it says.”
Other voice: “I followed the tracks through the yard. They stop making sense by the empty bins.”
Janek Orel: “We can write the mismatch down. It does not make the old line true again.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No species, track pattern, or cause. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The tracks lately do not

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The tracks lately do not match the old lines. Janek can point to that absence without identifying an animal or giving a method to follow it. The map is quietly wrong, not suddenly absurd. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Do not convert the mismatch into a map mechanic. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No species, track pattern, or cause.

Janek Orel may say: “The old line is on paper. The tracks are not where it says.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Dawn note: Current tracks do not match the old lines. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — Blame arrives in a practiced order

Janek blames weather, raiders, the queue, and the column of ash on the horizon, in that order. The prose lets the list come out as a defense he has used before; it does not confirm any of those causes. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “I know the order I blame them in. I have not proved the order is right.”
Other voice: “We called them liars before we checked whose marks they were.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not make an accusation or confirm a cause. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep his habit and uncertainty side by side. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — Blame arrives in a practiced order

Proposed diegetic text: “Margin list: Weather / raiders / queue / ash; no cause established.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Janek blames weather, raiders, the queue, and the column of ash on the horizon, in that order. The prose lets the list come out as a defense he has used before; it does not confirm any of those causes. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not make an accusation or confirm a cause. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep his habit and uncertainty side by side. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 015 — Blame arrives in a practiced order

Janek blames weather, raiders, the queue, and the column of ash on the horizon, in that order. The prose lets the list come out as a defense he has used before; it does not confirm any of those causes. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “I know the order I blame them in. I have not proved the order is right.”
Other voice: “We called them liars before we checked whose marks they were.”
Janek Orel: “Keep his habit and uncertainty side by side.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not make an accusation or confirm a cause. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — Blame arrives in a practiced order

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. Janek blames weather, raiders, the queue, and the column of ash on the horizon, in that order. The prose lets the list come out as a defense he has used before; it does not confirm any of those causes. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Keep his habit and uncertainty side by side. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not make an accusation or confirm a cause.

Janek Orel may say: “I know the order I blame them in. I have not proved the order is right.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin list: Weather / raiders / queue / ash; no cause established. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The second opinion belongs at dawn

Janek wants a second opinion at dawn. The player can walk and look at the same ground, but the scene offers no hunting lesson or correction badge. Two people may leave with different confidence and still share the walk. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “Tell me what you see. Do not tell me what you think I want.”
Other voice: “I can walk the same stretch again. I will not swear the first look was enough.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No skill check, route score, or new character attribute. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use before `janek_walk_dawn`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The second opinion belongs at dawn

Proposed diegetic text: “Walk note: Second opinion requested; conclusion remains open.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Janek wants a second opinion at dawn. The player can walk and look at the same ground, but the scene offers no hunting lesson or correction badge. Two people may leave with different confidence and still share the walk. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No skill check, route score, or new character attribute. Do not let the form claim authority that its keeper has not been given.

Later reading: Use before `janek_walk_dawn`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 019 — The second opinion belongs at dawn

Janek wants a second opinion at dawn. The player can walk and look at the same ground, but the scene offers no hunting lesson or correction badge. Two people may leave with different confidence and still share the walk. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “Tell me what you see. Do not tell me what you think I want.”
Other voice: “I can walk the same stretch again. I will not swear the first look was enough.”
Janek Orel: “If you are coming, come before we lose the light.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No skill check, route score, or new character attribute. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The second opinion belongs at dawn

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. Janek wants a second opinion at dawn. The player can walk and look at the same ground, but the scene offers no hunting lesson or correction badge. Two people may leave with different confidence and still share the walk. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use before `janek_walk_dawn`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No skill check, route score, or new character attribute.

Janek Orel may say: “Tell me what you see. Do not tell me what you think I want.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Walk note: Second opinion requested; conclusion remains open. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — Truth before the settlement wakes

The quest says he will hear no word against the old lines before dawn, while the initial state invites an honest version at that hour. A proposed scene lets him hold both habits: he wants truth, but only when the day is still quiet enough to hear it. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “The rest of the sentence is easier before everyone is listening.”
Other voice: “He asked me to come because I remember the old map, not because I agree with it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not portray the settlement as already hostile. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Stay within the initial encounter. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — Truth before the settlement wakes

Proposed diegetic text: “Dawn margin: Old lines mostly true; remaining sentence spoken at dawn.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The quest says he will hear no word against the old lines before dawn, while the initial state invites an honest version at that hour. A proposed scene lets him hold both habits: he wants truth, but only when the day is still quiet enough to hear it. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not portray the settlement as already hostile. Do not let the form claim authority that its keeper has not been given.

Later reading: Stay within the initial encounter. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 023 — Truth before the settlement wakes

The quest says he will hear no word against the old lines before dawn, while the initial state invites an honest version at that hour. A proposed scene lets him hold both habits: he wants truth, but only when the day is still quiet enough to hear it. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “The rest of the sentence is easier before everyone is listening.”
Other voice: “He asked me to come because I remember the old map, not because I agree with it.”
Janek Orel: “First we walk. I can talk after I know what we saw.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not portray the settlement as already hostile. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — Truth before the settlement wakes

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The quest says he will hear no word against the old lines before dawn, while the initial state invites an honest version at that hour. A proposed scene lets him hold both habits: he wants truth, but only when the day is still quiet enough to hear it. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Stay within the initial encounter. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not portray the settlement as already hostile.

Janek Orel may say: “The rest of the sentence is easier before everyone is listening.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Dawn margin: Old lines mostly true; remaining sentence spoken at dawn. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — The new map happens out loud

If `janek_walk_dawn` is chosen, three new routes go onto the dawn map. The scene can show Janek crossing out an old line and leaving its history legible beneath the correction, without drawing the route itself. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “We can write the change where we both can see it.”
Other voice: “Write that we saw a change. Leave the cause blank.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No route coordinates or precise map image. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only after the walk choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — The new map happens out loud

Proposed diegetic text: “Map note: Three new routes added to the dawn map.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `janek_walk_dawn` is chosen, three new routes go onto the dawn map. The scene can show Janek crossing out an old line and leaving its history legible beneath the correction, without drawing the route itself. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No route coordinates or precise map image. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the walk choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 027 — The new map happens out loud

If `janek_walk_dawn` is chosen, three new routes go onto the dawn map. The scene can show Janek crossing out an old line and leaving its history legible beneath the correction, without drawing the route itself. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “We can write the change where we both can see it.”
Other voice: “Write that we saw a change. Leave the cause blank.”
Janek Orel: “Walk beside me. Let the ground correct us before the page does.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No route coordinates or precise map image. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — The new map happens out loud

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. If `janek_walk_dawn` is chosen, three new routes go onto the dawn map. The scene can show Janek crossing out an old line and leaving its history legible beneath the correction, without drawing the route itself. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use only after the walk choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No route coordinates or precise map image.

Janek Orel may say: “We can write the change where we both can see it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Map note: Three new routes added to the dawn map. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — Neither says admit

The evolved Dawn Walks state says neither person said the word admit. The prose lets them compare observations without forcing either to make a speech about being wrong. The added routes do not require the old map to be worthless. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “We added what we saw. That is enough words for now.”
Other voice: “I did not make the tracks. I did tell them what they meant.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add an apology or change the source’s silence. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use in the evolved state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — Neither says admit

Proposed diegetic text: “Walk record: New lines added; neither person names the act as an admission.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The evolved Dawn Walks state says neither person said the word admit. The prose lets them compare observations without forcing either to make a speech about being wrong. The added routes do not require the old map to be worthless. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add an apology or change the source’s silence. Do not let the form claim authority that its keeper has not been given.

Later reading: Use in the evolved state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 031 — Neither says admit

The evolved Dawn Walks state says neither person said the word admit. The prose lets them compare observations without forcing either to make a speech about being wrong. The added routes do not require the old map to be worthless. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “We added what we saw. That is enough words for now.”
Other voice: “I did not make the tracks. I did tell them what they meant.”
Janek Orel: “I changed the line where the ground changed it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add an apology or change the source’s silence. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — Neither says admit

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The evolved Dawn Walks state says neither person said the word admit. The prose lets them compare observations without forcing either to make a speech about being wrong. The added routes do not require the old map to be worthless. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use in the evolved state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add an apology or change the source’s silence.

Janek Orel may say: “We added what we saw. That is enough words for now.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Walk record: New lines added; neither person names the act as an admission. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The meat comes back heavier

After the dawn walk, the meat comes back heavier this season, which is its own kind of speech. The scene does not give a count or claim that each new route caused a particular catch. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “The yard ate through the hungry weeks. We can leave the sentence there.”
Other voice: “Do we still call it a route if it brought back less than we expected?”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No quantity, species, guarantee, or hunting method. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on `janek_walk_dawn`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The meat comes back heavier

Proposed diegetic text: “Yard note: Meat returned heavier this season after the dawn map changed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the dawn walk, the meat comes back heavier this season, which is its own kind of speech. The scene does not give a count or claim that each new route caused a particular catch. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No quantity, species, guarantee, or hunting method. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `janek_walk_dawn`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 035 — The meat comes back heavier

After the dawn walk, the meat comes back heavier this season, which is its own kind of speech. The scene does not give a count or claim that each new route caused a particular catch. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “The yard ate through the hungry weeks. We can leave the sentence there.”
Other voice: “Do we still call it a route if it brought back less than we expected?”
Janek Orel: “You walked with me. Tell me what your eye says, not mine.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No quantity, species, guarantee, or hunting method. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The meat comes back heavier

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. After the dawn walk, the meat comes back heavier this season, which is its own kind of speech. The scene does not give a count or claim that each new route caused a particular catch. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Condition on `janek_walk_dawn`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No quantity, species, guarantee, or hunting method.

Janek Orel may say: “The yard ate through the hungry weeks. We can leave the sentence there.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Yard note: Meat returned heavier this season after the dawn map changed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — Routes retired out loud

In `late_scout`, Janek teaches migration routes that are true and retires the ones that are not, out loud, in front of the young hunters. The prose keeps the public correction matter-of-fact rather than humiliating. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “You can keep the old name on the page and still stop sending people by it.”
Other voice: “I have sent people by that line before. I need to tell them it changed.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not list the actual routes or a hunting technique. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the late-scout state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — Routes retired out loud

Proposed diegetic text: “Teaching margin: True migration routes taught; false old lines retired aloud.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_scout`, Janek teaches migration routes that are true and retires the ones that are not, out loud, in front of the young hunters. The prose keeps the public correction matter-of-fact rather than humiliating. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not list the actual routes or a hunting technique. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the late-scout state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 039 — Routes retired out loud

In `late_scout`, Janek teaches migration routes that are true and retires the ones that are not, out loud, in front of the young hunters. The prose keeps the public correction matter-of-fact rather than humiliating. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “You can keep the old name on the page and still stop sending people by it.”
Other voice: “I have sent people by that line before. I need to tell them it changed.”
Janek Orel: “I found a new line by looking. It is still only one day’s evidence.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not list the actual routes or a hunting technique. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — Routes retired out loud

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. In `late_scout`, Janek teaches migration routes that are true and retires the ones that are not, out loud, in front of the young hunters. The prose keeps the public correction matter-of-fact rather than humiliating. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use only in the late-scout state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not list the actual routes or a hunting technique.

Janek Orel may say: “You can keep the old name on the page and still stop sending people by it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Teaching margin: True migration routes taught; false old lines retired aloud. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — A winter the old map would not feed

The late scout state says the recovery yard eats regularly through a winter the old map would not have fed. A return may show a meal continuing while the annotated map stays open; it does not promise every winter will be the same. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “We did not make the winter easy. We made the map stop lying about the old line.”
Other voice: “I thought the marks were his. They could have been from the last group.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No universal food guarantee or new resource quantity. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep only in `late_scout`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — A winter the old map would not feed

Proposed diegetic text: “Late note: Yard fed regularly through the winter under the changed map.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late scout state says the recovery yard eats regularly through a winter the old map would not have fed. A return may show a meal continuing while the annotated map stays open; it does not promise every winter will be the same. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No universal food guarantee or new resource quantity. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep only in `late_scout`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 043 — A winter the old map would not feed

The late scout state says the recovery yard eats regularly through a winter the old map would not have fed. A return may show a meal continuing while the annotated map stays open; it does not promise every winter will be the same. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “We did not make the winter easy. We made the map stop lying about the old line.”
Other voice: “I thought the marks were his. They could have been from the last group.”
Janek Orel: “I have a new line and one day of evidence. That is all.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No universal food guarantee or new resource quantity. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — A winter the old map would not feed

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The late scout state says the recovery yard eats regularly through a winter the old map would not have fed. A return may show a meal continuing while the annotated map stays open; it does not promise every winter will be the same. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Keep only in `late_scout`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No universal food guarantee or new resource quantity.

Janek Orel may say: “We did not make the winter easy. We made the map stop lying about the old line.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late note: Yard fed regularly through the winter under the changed map. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The old lines are requested as truth

If `janek_quote_old_lines` is chosen, the player asks for the old lines as truth and lets the settlement believe. The scene must state that clearly before the player acts; Janek does not silently substitute a caveat. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “You asked for the old lines without the rest of the sentence. I can say them.”
Other voice: “He showed me the new lines before anyone started asking if he was wrong.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No ambiguous alternate interpretation of the choice. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use immediately before the quote-old-lines choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The old lines are requested as truth

Proposed diegetic text: “Choice note: Old lines requested as truth; settlement allowed to believe them.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `janek_quote_old_lines` is chosen, the player asks for the old lines as truth and lets the settlement believe. The scene must state that clearly before the player acts; Janek does not silently substitute a caveat. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No ambiguous alternate interpretation of the choice. Do not let the form claim authority that its keeper has not been given.

Later reading: Use immediately before the quote-old-lines choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 047 — The old lines are requested as truth

If `janek_quote_old_lines` is chosen, the player asks for the old lines as truth and lets the settlement believe. The scene must state that clearly before the player acts; Janek does not silently substitute a caveat. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “You asked for the old lines without the rest of the sentence. I can say them.”
Other voice: “He showed me the new lines before anyone started asking if he was wrong.”
Janek Orel: “You can hear the old words. You still have to decide what they are worth.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No ambiguous alternate interpretation of the choice. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The old lines are requested as truth

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. If `janek_quote_old_lines` is chosen, the player asks for the old lines as truth and lets the settlement believe. The scene must state that clearly before the player acts; Janek does not silently substitute a caveat. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use immediately before the quote-old-lines choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No ambiguous alternate interpretation of the choice.

Janek Orel may say: “You asked for the old lines without the rest of the sentence. I can say them.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Choice note: Old lines requested as truth; settlement allowed to believe them. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — Two hungry weeks are named

The result says the yard went hungry twice in the hungry weeks. The writing does not turn that into two dated incidents or portray hunger as a deserved lesson. It remains a consequence of asking for old routes as truth. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “I told them what you asked me to tell them. They learned which map it was.”
Other voice: “Those two trips came back short on meat. The page should not hide that.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No dates, casualties, or rescue outcome. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in `late_discredited`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — Two hungry weeks are named

Proposed diegetic text: “Late note: Yard went hungry twice in the hungry weeks.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The result says the yard went hungry twice in the hungry weeks. The writing does not turn that into two dated incidents or portray hunger as a deserved lesson. It remains a consequence of asking for old routes as truth. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No dates, casualties, or rescue outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in `late_discredited`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 051 — Two hungry weeks are named

The result says the yard went hungry twice in the hungry weeks. The writing does not turn that into two dated incidents or portray hunger as a deserved lesson. It remains a consequence of asking for old routes as truth. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “I told them what you asked me to tell them. They learned which map it was.”
Other voice: “Those two trips came back short on meat. The page should not hide that.”
Janek Orel: “The yard did not move because my map did. The marks are still wrong.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No dates, casualties, or rescue outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — Two hungry weeks are named

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The result says the yard went hungry twice in the hungry weeks. The writing does not turn that into two dated incidents or portray hunger as a deserved lesson. It remains a consequence of asking for old routes as truth. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use only in `late_discredited`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No dates, casualties, or rescue outcome.

Janek Orel may say: “I told them what you asked me to tell them. They learned which map it was.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late note: Yard went hungry twice in the hungry weeks. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — The settlement learns which map

The settlement finds out which map Janek was quoting. The proposed passage can keep the revelation in a quiet look at the map left on the table; it does not invent a public tribunal or a named accuser. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “They know which map I gave them. That is the part I cannot correct quietly.”
Other voice: “I heard the argument. No one asked me what I saw.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new hearing, punishment, or named critic. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Stay within the late-discredited branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — The settlement learns which map

Proposed diegetic text: “Map note: Settlement knows the quoted lines were the old map.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement finds out which map Janek was quoting. The proposed passage can keep the revelation in a quiet look at the map left on the table; it does not invent a public tribunal or a named accuser. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new hearing, punishment, or named critic. Do not let the form claim authority that its keeper has not been given.

Later reading: Stay within the late-discredited branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 055 — The settlement learns which map

The settlement finds out which map Janek was quoting. The proposed passage can keep the revelation in a quiet look at the map left on the table; it does not invent a public tribunal or a named accuser. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “They know which map I gave them. That is the part I cannot correct quietly.”
Other voice: “I heard the argument. No one asked me what I saw.”
Janek Orel: “Nobody has to follow a mark that has failed them.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new hearing, punishment, or named critic. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — The settlement learns which map

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. The settlement finds out which map Janek was quoting. The proposed passage can keep the revelation in a quiet look at the map left on the table; it does not invent a public tribunal or a named accuser. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Stay within the late-discredited branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new hearing, punishment, or named critic.

Janek Orel may say: “They know which map I gave them. That is the part I cannot correct quietly.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Map note: Settlement knows the quoted lines were the old map. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — He corrects it alone

In the discredited late state, Janek walks at dawn alone, correcting the map in silence. The image does not say whether he is forgiven or abandoned. It shows only the authored solitude and the unfinished task. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “I am still changing the line. You do not have to come.”
Other voice: “He changed it alone. The marks still look like his handwriting.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No reconciliation flag or new companion. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only under `late_discredited`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — He corrects it alone

Proposed diegetic text: “Dawn record: Janek walks alone and corrects the map in silence.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the discredited late state, Janek walks at dawn alone, correcting the map in silence. The image does not say whether he is forgiven or abandoned. It shows only the authored solitude and the unfinished task. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No reconciliation flag or new companion. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only under `late_discredited`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 059 — He corrects it alone

In the discredited late state, Janek walks at dawn alone, correcting the map in silence. The image does not say whether he is forgiven or abandoned. It shows only the authored solitude and the unfinished task. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “I am still changing the line. You do not have to come.”
Other voice: “He changed it alone. The marks still look like his handwriting.”
Janek Orel: “Do not take my word as a route. Check what is there when you arrive.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No reconciliation flag or new companion. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — He corrects it alone

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. In the discredited late state, Janek walks at dawn alone, correcting the map in silence. The image does not say whether he is forgiven or abandoned. It shows only the authored solitude and the unfinished task. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use only under `late_discredited`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No reconciliation flag or new companion.

Janek Orel may say: “I am still changing the line. You do not have to come.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Dawn record: Janek walks alone and corrects the map in silence. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — Salt for the hides stays a want

Janek wants salt for the hides. The proposed line names that request without inventing a barter, a number, or a second purpose for the material. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “I will not promise meat before the walk. Snow decides what we bring home.”
Other voice: “I can carry salt when we have it. I cannot promise a hide will be worth it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No quantity or hide-processing method. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the want separate from the dawn choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — Salt for the hides stays a want

Proposed diegetic text: “Supply card: Salt for hides requested; quantity not supplied.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Janek wants salt for the hides. The proposed line names that request without inventing a barter, a number, or a second purpose for the material. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No quantity or hide-processing method. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the want separate from the dawn choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 063 — Salt for the hides stays a want

Janek wants salt for the hides. The proposed line names that request without inventing a barter, a number, or a second purpose for the material. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “I will not promise meat before the walk. Snow decides what we bring home.”
Other voice: “I can carry salt when we have it. I cannot promise a hide will be worth it.”
Janek Orel: “We can talk about the hides after the walk. First, let us see what is there.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No quantity or hide-processing method. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — Salt for the hides stays a want

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. Janek wants salt for the hides. The proposed line names that request without inventing a barter, a number, or a second purpose for the material. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Keep the want separate from the dawn choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No quantity or hide-processing method.

Janek Orel may say: “If you have salt, bring what the current trade asks for. I will not make up a number.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Supply card: Salt for hides requested; quantity not supplied. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The old map still fed two winters

A young hunter asks whether the old lines were foolish. Janek answers that they fed the yard for two winters and no longer do. That answer protects neither the map from correction nor the past from being true. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “A thing can have been true and stop being true. Keep both facts.”
Other voice: “The old map fed us twice. That does not make it current.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not shame the person who taught or followed the old map. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use as a late-scout teaching fragment. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The old map still fed two winters

Proposed diegetic text: “Lesson note: Old lines fed two winters; current lines have changed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A young hunter asks whether the old lines were foolish. Janek answers that they fed the yard for two winters and no longer do. That answer protects neither the map from correction nor the past from being true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not shame the person who taught or followed the old map. Do not let the form claim authority that its keeper has not been given.

Later reading: Use as a late-scout teaching fragment. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 067 — The old map still fed two winters

A young hunter asks whether the old lines were foolish. Janek answers that they fed the yard for two winters and no longer do. That answer protects neither the map from correction nor the past from being true. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “A thing can have been true and stop being true. Keep both facts.”
Other voice: “The old map fed us twice. That does not make it current.”
Janek Orel: “Use as a late-scout teaching fragment.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not shame the person who taught or followed the old map. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The old map still fed two winters

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. A young hunter asks whether the old lines were foolish. Janek answers that they fed the yard for two winters and no longer do. That answer protects neither the map from correction nor the past from being true. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Use as a late-scout teaching fragment. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not shame the person who taught or followed the old map.

Janek Orel may say: “A thing can have been true and stop being true. Keep both facts.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Lesson note: Old lines fed two winters; current lines have changed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — The gate closes on the walk

At the end of the proposed scene, Janek returns to the gate with the same two sets of snowshoes. The player leaves with the current branch and no new route. The day begins for the recovery yard without a narrator explaining what everyone should believe. Begin with the work already under way, before the visitor is asked to decide what it means. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Janek Orel: “We can talk after the yard has eaten.”
Other voice: “We made it back. The line has to wait for another look.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new path unlock or travel operation. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Close in the currently active Janek state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — The gate closes on the walk

Proposed diegetic text: “Closing note: Dawn walk ends; map state remains the existing branch.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the end of the proposed scene, Janek returns to the gate with the same two sets of snowshoes. The player leaves with the current branch and no new route. The day begins for the recovery yard without a narrator explaining what everyone should believe. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new path unlock or travel operation. Do not let the form claim authority that its keeper has not been given.

Later reading: Close in the currently active Janek state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 071 — The gate closes on the walk

At the end of the proposed scene, Janek returns to the gate with the same two sets of snowshoes. The player leaves with the current branch and no new route. The day begins for the recovery yard without a narrator explaining what everyone should believe. Let Janek Orel speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Janek Orel: “We can talk after the yard has eaten.”
Other voice: “We made it back. The line has to wait for another look.”
Janek Orel: “I will tell you when I have checked the page again.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new path unlock or travel operation. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — The gate closes on the walk

On a later visit permitted by the exact existing Janek choice and the corresponding initial, evolved, late-scout, late-discredited, or late-unmet NPC state, the player may notice what the earlier choice left in view. At the end of the proposed scene, Janek returns to the gate with the same two sets of snowshoes. The player leaves with the current branch and no new route. The day begins for the recovery yard without a narrator explaining what everyone should believe. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Janek blames in an order that shows how he protects the old pattern before admitting it has changed. He is quiet in the hour he chose for walking. His language is track, line, dawn, and what the yard ate. He does not give the player a precise map or call a hungry week a lesson.

Observable return: Close in the currently active Janek state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new path unlock or travel operation.

Janek Orel may say: “We can talk after the yard has eaten.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Closing note: Dawn walk ends; map state remains the existing branch. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- You came. We can walk before the yard starts asking for certainty.
- They worked. I am not pretending they did not.
- The old line is on paper. The tracks are not where it says.
- I know the order I blame them in. I have not proved the order is right.
- Tell me what you see. Do not tell me what you think I want.
- The rest of the sentence is easier before everyone is listening.
- We can write the change where we both can see it.
- We added what we saw. That is enough words for now.
- The yard ate through the hungry weeks. We can leave the sentence there.
- You can keep the old name on the page and still stop sending people by it.
- We did not make the winter easy. We made the map stop lying about the old line.
- You asked for the old lines without the rest of the sentence. I can say them.
- I told them what you asked me to tell them. They learned which map it was.
- They know which map I gave them. That is the part I cannot correct quietly.
- I am still changing the line. You do not have to come.
- If you have salt, bring what the current trade asks for. I will not make up a number.
- A thing can have been true and stop being true. Keep both facts.
- We can talk after the yard has eaten.

## 13. Continuity and editorial review

Verify `enc_arc_janek_01_dawn`, `quest_arc_janek_01_dawn`, and the late state requirements. Keep the old lines’ earlier value and current failure, the three new routes on the walk path, and the hungry weeks on the quote-old-lines path. Do not identify prey, route coordinates, or a cause for the altered tracks. Confirm current content before placement. Keep the first-visit premise recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposals only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the current character or settlement content, each callback matches an authored condition, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
