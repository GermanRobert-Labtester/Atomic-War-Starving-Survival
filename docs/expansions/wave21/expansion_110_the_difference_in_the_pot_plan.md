# EXPANSION 110 — The Difference in the Pot

## Nadia Lem, the ration queue, and the portion ledger whose arithmetic is exact because children eat the difference.

### Wave 21: Records Kept in Human Hands

## 1. Expansion thesis

Tell Nadia’s story through the distance between a manifest and a meal. Her current choice is direct: front oil and salt without an audit or ledger, or audit the portions and let the manifest win. The prose should make the people in the queue visible without exposing which children receive the extra portions or turning their need into a reward for the player. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character quest. It adds no gameplay feature and does not claim that any proposed text is already in the game.

## 2. Story in one sentence

A cook serves from the pot while an official page insists it already knows what the queue was given.

## 3. Verified local anchor and current story

`characters.json` defines `npc_nadia_lem` as a Field Kitchen Cook at `loc_ration_queue_plaza`. She says the thin go first and the proud wait like everyone else; the portion ledger differs from the pot by exactly the amount the children eat. `npc_arcs.json` has initial, evolved, late-anchor, late-dismissed, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_nadia_01_pot`; `quests_npc_arcs.json` registers `quest_arc_nadia_01_pot`. `nadia_front_the_oil` fronts oil and salt with no audit and no ledger; bowls grow heavier without an announcement, and the player’s name is in no ledger. `nadia_audit_her` makes the audit find the arithmetic and lets the manifest win; it is served exactly and thinner. The queue continues to arrange itself by need in either late account, but only the fronted path becomes the late kitchen anchor with shorter sick lists and shelter meals when roads are bad. Names, locations, quest IDs, choice IDs, and state summaries above come from the current data. The encounter catalog proves the authored scene and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing state condition.

## 4. Fixed canon and proposed prose

The plaza description says its lines are repainted often and the queue forms at 0600 even without an announced distribution; the encounter describes a queue arranging itself by need. Nadia will not name who gets the extra portions and will not serve sick meat because a form arrived. This plan preserves that privacy, adds no amounts or recipes, and does not treat the audit as proof that the manifest is morally complete. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to the character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Nadia stands in front of the arithmetic because she knows who bears the difference between the paper and the pot. Her care is not an invitation for the player to identify its recipients. A heavier bowl can arrive without a public explanation; a thinner one can be served exactly and still leave the queue knowing what changed. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–5 with `enc_arc_nadia_01_pot`. Beats 6–10 follow `nadia_front_the_oil` through `evolved` and `late_anchor`; beats 11–14 follow `nadia_audit_her` and `late_dismissed`. The final beats may serve the initial or unmet states without revealing identities. The location can describe an unannounced morning line; do not turn the empty plaza description into a permanent queue event. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character-location owner as the insertion point; do not create a parallel quest chain or a second mutable owner.

## 8. Player agency and consequence

The existing choice is whether to front oil and salt without audit or to audit portions and let the manifest win. The text can clarify what each option means, but must not add a hidden donation, disclosure, or compromise. The player’s name stays out of the no-ledger branch exactly as the current outcome says. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before the choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep food and health references at the level of authored narrative. No recipes, spoilage tests, recipient list, dose, or medical outcome is added. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when an implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The line forms before the horn

At the ration plaza the queue line is painted on the pavement and its paint is renewed often. The encounter begins with people arranging themselves by need and pretending not to; the prose does not ask them to announce why they are there. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The line was here before I lit the fire. I do not get to pretend I arranged all of it.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add a distribution time or named person in the queue. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep any return consistent with the encounter’s current moment. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The line forms before the horn

Proposed diegetic text: “Queue note: Line forms at 0600 whether or not distribution has been announced.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the ration plaza the queue line is painted on the pavement and its paint is renewed often. The encounter begins with people arranging themselves by need and pretending not to; the prose does not ask them to announce why they are there. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a distribution time or named person in the queue. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep any return consistent with the encounter’s current moment. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 003 — The line forms before the horn

At the ration plaza the queue line is painted on the pavement and its paint is renewed often. The encounter begins with people arranging themselves by need and pretending not to; the prose does not ask them to announce why they are there. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The line was here before I lit the fire. I do not get to pretend I arranged all of it.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Keep any return consistent with the encounter’s current moment.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a distribution time or named person in the queue. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The line forms before the horn

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the ration plaza the queue line is painted on the pavement and its paint is renewed often. The encounter begins with people arranging themselves by need and pretending not to; the prose does not ask them to announce why they are there. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Keep any return consistent with the encounter’s current moment. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a distribution time or named person in the queue.

Nadia Lem may say: “The line was here before I lit the fire. I do not get to pretend I arranged all of it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Queue note: Line forms at 0600 whether or not distribution has been announced. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — The pot and the manifest disagree

Nadia serves while the manifest says something different from what the pot contains. A visitor can see her look over the people she feeds once, then return to the page without giving them a speech. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The manifest has its words. The pot has its work.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No portion count, recipe, or additional ingredient. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Preserve the distinction without writing a new inventory system. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — The pot and the manifest disagree

Proposed diegetic text: “Kitchen margin: Pot served; manifest not treated as a complete account.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Nadia serves while the manifest says something different from what the pot contains. A visitor can see her look over the people she feeds once, then return to the page without giving them a speech. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No portion count, recipe, or additional ingredient. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the distinction without writing a new inventory system. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 007 — The pot and the manifest disagree

Nadia serves while the manifest says something different from what the pot contains. A visitor can see her look over the people she feeds once, then return to the page without giving them a speech. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The manifest has its words. The pot has its work.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Preserve the distinction without writing a new inventory system.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No portion count, recipe, or additional ingredient. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — The pot and the manifest disagree

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Nadia serves while the manifest says something different from what the pot contains. A visitor can see her look over the people she feeds once, then return to the page without giving them a speech. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Preserve the distinction without writing a new inventory system. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No portion count, recipe, or additional ingredient.

Nadia Lem may say: “The manifest has its words. The pot has its work.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Kitchen margin: Pot served; manifest not treated as a complete account. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — Need changes the order

The thin go to the front of Nadia’s line and the proud wait like everyone else. The passage does not ask a person to prove either description to the player. An empty place moves forward because Nadia has made an ordinary decision, not because she has named a hidden category. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You do not have to tell me your whole life to stand where you are standing.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No eligibility test or public sorting procedure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Do not identify recipients in a later callback. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — Need changes the order

Proposed diegetic text: “Service note: Queue ordered by need; no private recipient list.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The thin go to the front of Nadia’s line and the proud wait like everyone else. The passage does not ask a person to prove either description to the player. An empty place moves forward because Nadia has made an ordinary decision, not because she has named a hidden category. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No eligibility test or public sorting procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not identify recipients in a later callback. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 011 — Need changes the order

The thin go to the front of Nadia’s line and the proud wait like everyone else. The passage does not ask a person to prove either description to the player. An empty place moves forward because Nadia has made an ordinary decision, not because she has named a hidden category. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You do not have to tell me your whole life to stand where you are standing.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Do not identify recipients in a later callback.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No eligibility test or public sorting procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — Need changes the order

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The thin go to the front of Nadia’s line and the proud wait like everyone else. The passage does not ask a person to prove either description to the player. An empty place moves forward because Nadia has made an ordinary decision, not because she has named a hidden category. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Do not identify recipients in a later callback. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No eligibility test or public sorting procedure.

Nadia Lem may say: “You do not have to tell me your whole life to stand where you are standing.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Service note: Queue ordered by need; no private recipient list. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — The ledger has a precise difference

The portion ledger disagrees with the pot by exactly the amount the children eat. The exact amount is not written in this plan and no child is named. Nadia stands in front of the arithmetic as the person who will answer for it. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The arithmetic is exact. I will not make the children into a column you can inspect.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No count, names, ages, or portion amounts. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the source’s exact-but-unquantified difference. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — The ledger has a precise difference

Proposed diegetic text: “Ledger note: Difference recorded without its numeric value or recipients.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The portion ledger disagrees with the pot by exactly the amount the children eat. The exact amount is not written in this plan and no child is named. Nadia stands in front of the arithmetic as the person who will answer for it. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No count, names, ages, or portion amounts. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the source’s exact-but-unquantified difference. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 015 — The ledger has a precise difference

The portion ledger disagrees with the pot by exactly the amount the children eat. The exact amount is not written in this plan and no child is named. Nadia stands in front of the arithmetic as the person who will answer for it. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The arithmetic is exact. I will not make the children into a column you can inspect.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Keep the source’s exact-but-unquantified difference.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No count, names, ages, or portion amounts. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — The ledger has a precise difference

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The portion ledger disagrees with the pot by exactly the amount the children eat. The exact amount is not written in this plan and no child is named. Nadia stands in front of the arithmetic as the person who will answer for it. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Keep the source’s exact-but-unquantified difference. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No count, names, ages, or portion amounts.

Nadia Lem may say: “The arithmetic is exact. I will not make the children into a column you can inspect.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Ledger note: Difference recorded without its numeric value or recipients. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — One look over the bowls

The encounter describes Nadia meeting the visitor’s eyes exactly once over the heads of those she is feeding. A proposed scene lets that single look remain a boundary: she is aware of the visitor, but her attention belongs to the people receiving the bowls. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You can ask me what the page says after this line moves.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not turn the eye contact into a trust score or invitation to question recipients. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use the gesture once, not as a repeated signature beat. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — One look over the bowls

Proposed diegetic text: “Observation: Cook meets visitor’s eyes once; service continues.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The encounter describes Nadia meeting the visitor’s eyes exactly once over the heads of those she is feeding. A proposed scene lets that single look remain a boundary: she is aware of the visitor, but her attention belongs to the people receiving the bowls. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not turn the eye contact into a trust score or invitation to question recipients. Do not let the form claim authority that its keeper has not been given.

Later reading: Use the gesture once, not as a repeated signature beat. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 019 — One look over the bowls

The encounter describes Nadia meeting the visitor’s eyes exactly once over the heads of those she is feeding. A proposed scene lets that single look remain a boundary: she is aware of the visitor, but her attention belongs to the people receiving the bowls. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You can ask me what the page says after this line moves.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use the gesture once, not as a repeated signature beat.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not turn the eye contact into a trust score or invitation to question recipients. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — One look over the bowls

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The encounter describes Nadia meeting the visitor’s eyes exactly once over the heads of those she is feeding. A proposed scene lets that single look remain a boundary: she is aware of the visitor, but her attention belongs to the people receiving the bowls. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use the gesture once, not as a repeated signature beat. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not turn the eye contact into a trust score or invitation to question recipients.

Nadia Lem may say: “You can ask me what the page says after this line moves.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Observation: Cook meets visitor’s eyes once; service continues. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — No sick meat from a form

A form arrives asking for a service the character record says Nadia will refuse: serving sick meat because the form arrived. The scene can show the page placed aside and the pot kept out of that instruction. No explanation of illness or food safety is added. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “A form can arrive. It cannot make me serve what I will not serve.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No medical or food-safety guidance and no new official procedure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only as character-consistent texture if an existing scene includes the form. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — No sick meat from a form

Proposed diegetic text: “Form margin: Request declined; no sick meat served under the form.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A form arrives asking for a service the character record says Nadia will refuse: serving sick meat because the form arrived. The scene can show the page placed aside and the pot kept out of that instruction. No explanation of illness or food safety is added. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No medical or food-safety guidance and no new official procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only as character-consistent texture if an existing scene includes the form. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 023 — No sick meat from a form

A form arrives asking for a service the character record says Nadia will refuse: serving sick meat because the form arrived. The scene can show the page placed aside and the pot kept out of that instruction. No explanation of illness or food safety is added. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “A form can arrive. It cannot make me serve what I will not serve.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use only as character-consistent texture if an existing scene includes the form.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No medical or food-safety guidance and no new official procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — No sick meat from a form

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A form arrives asking for a service the character record says Nadia will refuse: serving sick meat because the form arrived. The scene can show the page placed aside and the pot kept out of that instruction. No explanation of illness or food safety is added. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use only as character-consistent texture if an existing scene includes the form. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No medical or food-safety guidance and no new official procedure.

Nadia Lem may say: “A form can arrive. It cannot make me serve what I will not serve.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Form margin: Request declined; no sick meat served under the form. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — Fronting oil without a name

After `nadia_front_the_oil`, the kitchen has oil and salt and the player’s name appears in no ledger. The bowls grow heavier without announcement. The text should not replace that quiet act with a plaque, debt, or a public speech. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “If you need thanks, ask the bowls. I am busy with the next one.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add a debt, name, quantity, or ledger entry. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only after the fronted choice. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — Fronting oil without a name

Proposed diegetic text: “Supply note: Oil and salt fronted; player’s name not entered in a ledger.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `nadia_front_the_oil`, the kitchen has oil and salt and the player’s name appears in no ledger. The bowls grow heavier without announcement. The text should not replace that quiet act with a plaque, debt, or a public speech. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a debt, name, quantity, or ledger entry. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the fronted choice. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 027 — Fronting oil without a name

After `nadia_front_the_oil`, the kitchen has oil and salt and the player’s name appears in no ledger. The bowls grow heavier without announcement. The text should not replace that quiet act with a plaque, debt, or a public speech. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “If you need thanks, ask the bowls. I am busy with the next one.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use only after the fronted choice.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a debt, name, quantity, or ledger entry. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — Fronting oil without a name

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `nadia_front_the_oil`, the kitchen has oil and salt and the player’s name appears in no ledger. The bowls grow heavier without announcement. The text should not replace that quiet act with a plaque, debt, or a public speech. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use only after the fronted choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a debt, name, quantity, or ledger entry.

Nadia Lem may say: “If you need thanks, ask the bowls. I am busy with the next one.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Supply note: Oil and salt fronted; player’s name not entered in a ledger. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — A heavier bowl travels quietly

In the evolved state, the queue notices that bowls got heavier without anyone announcing it. The prose can follow a hand receiving the bowl and turning back to the line. It does not need a named child or a reaction that identifies who benefited. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The change does not need a witness with a title. It needs a bowl in someone’s hands.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No portion quantity or recipient identity. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the evolved consequence anonymous. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — A heavier bowl travels quietly

Proposed diegetic text: “Serving note: Bowls heavier; no public announcement made.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the evolved state, the queue notices that bowls got heavier without anyone announcing it. The prose can follow a hand receiving the bowl and turning back to the line. It does not need a named child or a reaction that identifies who benefited. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No portion quantity or recipient identity. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the evolved consequence anonymous. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 031 — A heavier bowl travels quietly

In the evolved state, the queue notices that bowls got heavier without anyone announcing it. The prose can follow a hand receiving the bowl and turning back to the line. It does not need a named child or a reaction that identifies who benefited. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The change does not need a witness with a title. It needs a bowl in someone’s hands.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Keep the evolved consequence anonymous.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No portion quantity or recipient identity. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — A heavier bowl travels quietly

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the evolved state, the queue notices that bowls got heavier without anyone announcing it. The prose can follow a hand receiving the bowl and turning back to the line. It does not need a named child or a reaction that identifies who benefited. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Keep the evolved consequence anonymous. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No portion quantity or recipient identity.

Nadia Lem may say: “The change does not need a witness with a title. It needs a bowl in someone’s hands.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Serving note: Bowls heavier; no public announcement made. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — A name stays out of the ledger

The player asks whether their name should be entered for the oil. Nadia points to the current outcome: the name is in no ledger. The refusal protects the quiet arrangement from becoming a claim that the player owns the meal or its recipients. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “Your name is not on the page. Let it stay that way.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not create donor credit or hidden recognition. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Match the existing fronted branch exactly. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — A name stays out of the ledger

Proposed diegetic text: “Ledger edge: No player name entered for the fronted oil.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The player asks whether their name should be entered for the oil. Nadia points to the current outcome: the name is in no ledger. The refusal protects the quiet arrangement from becoming a claim that the player owns the meal or its recipients. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not create donor credit or hidden recognition. Do not let the form claim authority that its keeper has not been given.

Later reading: Match the existing fronted branch exactly. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 035 — A name stays out of the ledger

The player asks whether their name should be entered for the oil. Nadia points to the current outcome: the name is in no ledger. The refusal protects the quiet arrangement from becoming a claim that the player owns the meal or its recipients. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “Your name is not on the page. Let it stay that way.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Match the existing fronted branch exactly.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not create donor credit or hidden recognition. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — A name stays out of the ledger

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The player asks whether their name should be entered for the oil. Nadia points to the current outcome: the name is in no ledger. The refusal protects the quiet arrangement from becoming a claim that the player owns the meal or its recipients. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Match the existing fronted branch exactly. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not create donor credit or hidden recognition.

Nadia Lem may say: “Your name is not on the page. Let it stay that way.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Ledger edge: No player name entered for the fronted oil. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — The queue mood is the report

In `late_anchor`, the queue’s mood is the truest report the shelter receives all year. A character can say that the people have learned the kitchen’s rhythm without surveying them or assigning a single public sentiment to each person. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You can hear a line move without asking every person to sign the sound.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No satisfaction metric, survey, or new mood system. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only use in the late kitchen-anchor state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — The queue mood is the report

Proposed diegetic text: “Late note: Queue mood described as the shelter’s report; no names collected.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_anchor`, the queue’s mood is the truest report the shelter receives all year. A character can say that the people have learned the kitchen’s rhythm without surveying them or assigning a single public sentiment to each person. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No satisfaction metric, survey, or new mood system. Do not let the form claim authority that its keeper has not been given.

Later reading: Only use in the late kitchen-anchor state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 039 — The queue mood is the report

In `late_anchor`, the queue’s mood is the truest report the shelter receives all year. A character can say that the people have learned the kitchen’s rhythm without surveying them or assigning a single public sentiment to each person. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You can hear a line move without asking every person to sign the sound.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Only use in the late kitchen-anchor state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No satisfaction metric, survey, or new mood system. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — The queue mood is the report

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_anchor`, the queue’s mood is the truest report the shelter receives all year. A character can say that the people have learned the kitchen’s rhythm without surveying them or assigning a single public sentiment to each person. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Only use in the late kitchen-anchor state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No satisfaction metric, survey, or new mood system.

Nadia Lem may say: “You can hear a line move without asking every person to sign the sound.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late note: Queue mood described as the shelter’s report; no names collected. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The audit finds arithmetic

After `nadia_audit_her`, the audit finds the arithmetic and never asks what it bought. The proposed text distinguishes a correct addition from a complete account of who ate. Nobody tears a page or hides a record. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The sum is right. The page still does not know the whole meal.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not invent audit steps, a fraud accusation, or a new offender. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only after the audit choice. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The audit finds arithmetic

Proposed diegetic text: “Audit note: Arithmetic found; what it bought remains outside the report.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `nadia_audit_her`, the audit finds the arithmetic and never asks what it bought. The proposed text distinguishes a correct addition from a complete account of who ate. Nobody tears a page or hides a record. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent audit steps, a fraud accusation, or a new offender. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the audit choice. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 043 — The audit finds arithmetic

After `nadia_audit_her`, the audit finds the arithmetic and never asks what it bought. The proposed text distinguishes a correct addition from a complete account of who ate. Nobody tears a page or hides a record. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The sum is right. The page still does not know the whole meal.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use only after the audit choice.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent audit steps, a fraud accusation, or a new offender. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The audit finds arithmetic

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `nadia_audit_her`, the audit finds the arithmetic and never asks what it bought. The proposed text distinguishes a correct addition from a complete account of who ate. Nobody tears a page or hides a record. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use only after the audit choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent audit steps, a fraud accusation, or a new offender.

Nadia Lem may say: “The sum is right. The page still does not know the whole meal.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Audit note: Arithmetic found; what it bought remains outside the report. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The manifest wins exactly

In `late_dismissed`, the kitchen serves the manifest exactly and thinner. Nadia can set each bowl down without assigning blame to an unnamed clerk or announcing a private recipient. The queue still arranges itself by need out of habit. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “The page is being followed. Look at what that means in the bowl.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No numerical portion decrease or named author of the manifest. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only for `late_dismissed`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The manifest wins exactly

Proposed diegetic text: “Late serving line: Manifest served exactly; bowls thinner; queue still orders by need.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_dismissed`, the kitchen serves the manifest exactly and thinner. Nadia can set each bowl down without assigning blame to an unnamed clerk or announcing a private recipient. The queue still arranges itself by need out of habit. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No numerical portion decrease or named author of the manifest. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only for `late_dismissed`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 047 — The manifest wins exactly

In `late_dismissed`, the kitchen serves the manifest exactly and thinner. Nadia can set each bowl down without assigning blame to an unnamed clerk or announcing a private recipient. The queue still arranges itself by need out of habit. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “The page is being followed. Look at what that means in the bowl.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use only for `late_dismissed`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No numerical portion decrease or named author of the manifest. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The manifest wins exactly

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_dismissed`, the kitchen serves the manifest exactly and thinner. Nadia can set each bowl down without assigning blame to an unnamed clerk or announcing a private recipient. The queue still arranges itself by need out of habit. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use only for `late_dismissed`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No numerical portion decrease or named author of the manifest.

Nadia Lem may say: “The page is being followed. Look at what that means in the bowl.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late serving line: Manifest served exactly; bowls thinner; queue still orders by need. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — Habit remains in the line

After the audit branch, the queue still arranges itself by need, out of habit. The people have not been rewritten by the manifest, but the prose does not pretend that habit can replace food. A painted line waits beside them. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “People remember where to stand. That does not make the bowl heavier.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No promise that the queue’s habit restores the food. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Separate the human continuity from the dismissed kitchen outcome. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — Habit remains in the line

Proposed diegetic text: “Plaza note: Queue arrangement persists by habit; kitchen outcome follows current state.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the audit branch, the queue still arranges itself by need, out of habit. The people have not been rewritten by the manifest, but the prose does not pretend that habit can replace food. A painted line waits beside them. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No promise that the queue’s habit restores the food. Do not let the form claim authority that its keeper has not been given.

Later reading: Separate the human continuity from the dismissed kitchen outcome. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 051 — Habit remains in the line

After the audit branch, the queue still arranges itself by need, out of habit. The people have not been rewritten by the manifest, but the prose does not pretend that habit can replace food. A painted line waits beside them. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “People remember where to stand. That does not make the bowl heavier.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Separate the human continuity from the dismissed kitchen outcome.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No promise that the queue’s habit restores the food. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — Habit remains in the line

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After the audit branch, the queue still arranges itself by need, out of habit. The people have not been rewritten by the manifest, but the prose does not pretend that habit can replace food. A painted line waits beside them. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Separate the human continuity from the dismissed kitchen outcome. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No promise that the queue’s habit restores the food.

Nadia Lem may say: “People remember where to stand. That does not make the bowl heavier.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Plaza note: Queue arrangement persists by habit; kitchen outcome follows current state. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — Shorter sick lists stay in the late account

Only the late anchor state says the settlement’s sick lists got shorter two winters running. A return may refer to that authored consequence as a community record, without attributing it to one person’s bowl or making a medical guarantee. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “I can tell you what the list says. I cannot tell you a single bowl did it alone.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No diagnosis, causal medical claim, or individual outcome. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Show only under `late_anchor`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — Shorter sick lists stay in the late account

Proposed diegetic text: “Late record: Sick lists shorter two winters running under the existing anchor state.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Only the late anchor state says the settlement’s sick lists got shorter two winters running. A return may refer to that authored consequence as a community record, without attributing it to one person’s bowl or making a medical guarantee. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No diagnosis, causal medical claim, or individual outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: Show only under `late_anchor`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 055 — Shorter sick lists stay in the late account

Only the late anchor state says the settlement’s sick lists got shorter two winters running. A return may refer to that authored consequence as a community record, without attributing it to one person’s bowl or making a medical guarantee. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “I can tell you what the list says. I cannot tell you a single bowl did it alone.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Show only under `late_anchor`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No diagnosis, causal medical claim, or individual outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — Shorter sick lists stay in the late account

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Only the late anchor state says the settlement’s sick lists got shorter two winters running. A return may refer to that authored consequence as a community record, without attributing it to one person’s bowl or making a medical guarantee. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Show only under `late_anchor`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No diagnosis, causal medical claim, or individual outcome.

Nadia Lem may say: “I can tell you what the list says. I cannot tell you a single bowl did it alone.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late record: Sick lists shorter two winters running under the existing anchor state. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — Roads bad, a fire open

In `late_anchor`, the player’s people eat at Nadia’s fire when the roads are bad. The writing keeps this as an existing relationship, not an invitation to new travel or a guaranteed service in every weather state. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You came in from the road. Sit, if there is room, and let me serve.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new location, route, or capacity. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only with the matching late state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — Roads bad, a fire open

Proposed diegetic text: “Kitchen note: Player’s people eat at the fire when roads are bad, as the late state says.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_anchor`, the player’s people eat at Nadia’s fire when the roads are bad. The writing keeps this as an existing relationship, not an invitation to new travel or a guaranteed service in every weather state. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new location, route, or capacity. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only with the matching late state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 059 — Roads bad, a fire open

In `late_anchor`, the player’s people eat at Nadia’s fire when the roads are bad. The writing keeps this as an existing relationship, not an invitation to new travel or a guaranteed service in every weather state. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You came in from the road. Sit, if there is room, and let me serve.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Use only with the matching late state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new location, route, or capacity. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — Roads bad, a fire open

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_anchor`, the player’s people eat at Nadia’s fire when the roads are bad. The writing keeps this as an existing relationship, not an invitation to new travel or a guaranteed service in every weather state. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Use only with the matching late state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new location, route, or capacity.

Nadia Lem may say: “You came in from the road. Sit, if there is room, and let me serve.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Kitchen note: Player’s people eat at the fire when roads are bad, as the late state says. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — The cook stands in front

The closing image returns to Nadia standing in front of the arithmetic, not in front of the player for judgment. She serves what the current branch permits and does not name who receives the difference. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “I know what I served. That is enough for tonight.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No forced confession or public disclosure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: End with her current state, not a generic reconciliation. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — The cook stands in front

Proposed diegetic text: “Closing note: Cook remains answerable for her choice; recipient names omitted.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The closing image returns to Nadia standing in front of the arithmetic, not in front of the player for judgment. She serves what the current branch permits and does not name who receives the difference. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No forced confession or public disclosure. Do not let the form claim authority that its keeper has not been given.

Later reading: End with her current state, not a generic reconciliation. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 063 — The cook stands in front

The closing image returns to Nadia standing in front of the arithmetic, not in front of the player for judgment. She serves what the current branch permits and does not name who receives the difference. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “I know what I served. That is enough for tonight.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “End with her current state, not a generic reconciliation.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No forced confession or public disclosure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — The cook stands in front

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The closing image returns to Nadia standing in front of the arithmetic, not in front of the player for judgment. She serves what the current branch permits and does not name who receives the difference. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: End with her current state, not a generic reconciliation. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No forced confession or public disclosure.

Nadia Lem may say: “I know what I served. That is enough for tonight.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Closing note: Cook remains answerable for her choice; recipient names omitted. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The bowl does not testify

A bowl can be heavier or thinner and still not tell the entire story of the person holding it. The final proposed fragment keeps the scene at the level of what the visitor can see: the rim, the steam, the next place in line. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You saw a bowl. Do not pretend you saw the whole person.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No symbolic certainty or hidden information. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the recipients private in every branch. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The bowl does not testify

Proposed diegetic text: “Surface note: Bowl observed; no conclusion about its recipient recorded.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A bowl can be heavier or thinner and still not tell the entire story of the person holding it. The final proposed fragment keeps the scene at the level of what the visitor can see: the rim, the steam, the next place in line. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No symbolic certainty or hidden information. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the recipients private in every branch. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 067 — The bowl does not testify

A bowl can be heavier or thinner and still not tell the entire story of the person holding it. The final proposed fragment keeps the scene at the level of what the visitor can see: the rim, the steam, the next place in line. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You saw a bowl. Do not pretend you saw the whole person.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Keep the recipients private in every branch.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No symbolic certainty or hidden information. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The bowl does not testify

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A bowl can be heavier or thinner and still not tell the entire story of the person holding it. The final proposed fragment keeps the scene at the level of what the visitor can see: the rim, the steam, the next place in line. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Keep the recipients private in every branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No symbolic certainty or hidden information.

Nadia Lem may say: “You saw a bowl. Do not pretend you saw the whole person.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Surface note: Bowl observed; no conclusion about its recipient recorded. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — No signature is required to eat

In the fronted branch, the oil and salt arrive without the player’s name being entered. A person receives a bowl and turns toward the next part of the plaza; the scene does not ask them to sign for being fed or to praise the person who supplied the ingredients. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “You are not signing for a meal. Take it while it is here.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add a ration record or public attribution. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Condition on the no-ledger branch and preserve the recipient’s privacy. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — No signature is required to eat

Proposed diegetic text: “Service note: Bowl served; no recipient or donor signature requested.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the fronted branch, the oil and salt arrive without the player’s name being entered. A person receives a bowl and turns toward the next part of the plaza; the scene does not ask them to sign for being fed or to praise the person who supplied the ingredients. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a ration record or public attribution. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on the no-ledger branch and preserve the recipient’s privacy. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 071 — No signature is required to eat

In the fronted branch, the oil and salt arrive without the player’s name being entered. A person receives a bowl and turns toward the next part of the plaza; the scene does not ask them to sign for being fed or to praise the person who supplied the ingredients. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “You are not signing for a meal. Take it while it is here.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Condition on the no-ledger branch and preserve the recipient’s privacy.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a ration record or public attribution. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — No signature is required to eat

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the fronted branch, the oil and salt arrive without the player’s name being entered. A person receives a bowl and turns toward the next part of the plaza; the scene does not ask them to sign for being fed or to praise the person who supplied the ingredients. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Condition on the no-ledger branch and preserve the recipient’s privacy. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a ration record or public attribution.

Nadia Lem may say: “You are not signing for a meal. Take it while it is here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Service note: Bowl served; no recipient or donor signature requested. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 073 — A true sum can still be a partial account

The audit can report correct arithmetic while leaving out what the extra portions bought for children. Nadia does not call the arithmetic false; she refuses the claim that arithmetic alone contains the whole story. Begin with the work already under way, before the visitor is asked to decide what it means. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Nadia Lem: “A true sum can be a small answer to a much larger question.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No number, recipient, or accusation is invented. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep this distinction in the dismissed branch without softening its outcome. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 074 — A true sum can still be a partial account

Proposed diegetic text: “Audit margin: Addition correct; human reason for the difference remains unlisted.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The audit can report correct arithmetic while leaving out what the extra portions bought for children. Nadia does not call the arithmetic false; she refuses the claim that arithmetic alone contains the whole story. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No number, recipient, or accusation is invented. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep this distinction in the dismissed branch without softening its outcome. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 075 — A true sum can still be a partial account

The audit can report correct arithmetic while leaving out what the extra portions bought for children. Nadia does not call the arithmetic false; she refuses the claim that arithmetic alone contains the whole story. Let Nadia Lem speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Nadia Lem: “A true sum can be a small answer to a much larger question.”
Other voice: “A person waiting near the painted line says, “I know what I received. I do not know what the paper says about the next bowl.””
Nadia Lem: “Keep this distinction in the dismissed branch without softening its outcome.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No number, recipient, or accusation is invented. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 076 — A true sum can still be a partial account

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The audit can report correct arithmetic while leaving out what the extra portions bought for children. Nadia does not call the arithmetic false; she refuses the claim that arithmetic alone contains the whole story. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Nadia is direct, alert to who is standing and who is being made to wait, and does not dramatize the pot. She speaks in portions, queue order, and the words written on a manifest. She does not use a child as an argument the player can win. A sentence about need is enough.

Observable return: Keep this distinction in the dismissed branch without softening its outcome. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No number, recipient, or accusation is invented.

Nadia Lem may say: “A true sum can be a small answer to a much larger question.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Audit margin: Addition correct; human reason for the difference remains unlisted. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- The line was here before I lit the fire. I do not get to pretend I arranged all of it.
- The manifest has its words. The pot has its work.
- You do not have to tell me your whole life to stand where you are standing.
- The arithmetic is exact. I will not make the children into a column you can inspect.
- You can ask me what the page says after this line moves.
- A form can arrive. It cannot make me serve what I will not serve.
- If you need thanks, ask the bowls. I am busy with the next one.
- The change does not need a witness with a title. It needs a bowl in someone’s hands.
- Your name is not on the page. Let it stay that way.
- You can hear a line move without asking every person to sign the sound.
- The sum is right. The page still does not know the whole meal.
- The page is being followed. Look at what that means in the bowl.
- People remember where to stand. That does not make the bowl heavier.
- I can tell you what the list says. I cannot tell you a single bowl did it alone.
- You came in from the road. Sit, if there is room, and let me serve.
- I know what I served. That is enough for tonight.
- You saw a bowl. Do not pretend you saw the whole person.
- You are not signing for a meal. Take it while it is here.
- A true sum can be a small answer to a much larger question.

## 13. Continuity and editorial review

Verify the two current choice IDs and corresponding `npc_arcs.json` outcomes. Keep the amount of the discrepancy unspecified, protect the children’s identities, distinguish late anchor from dismissed, and keep the player’s name out of the fronted branch’s ledger. No new ration amount or kitchen rule is proposed. Confirm current content before placement. Keep the first-visit encounter recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposed only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the existing character and encounter content, each callback matches the authored choice, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
