# EXPANSION 103 — Eight Beds, Three Kinds of Waiting

## A prose-led clinic expansion for Dr. Ilze Kaar’s inventory, training work, and the door she keeps open at stated odds.

### Wave 20: Useful Systems, Real Consequences — prose continuation: Terms of Staying

## 1. Expansion thesis

Write the clinic through what people can verify: a shelf count, a hand learning a task, a queue that has not been promised a cure. The existing arc already contains the supply choice, the fever outbreak, and the decision to extract Ilze or hold the clinic. New prose should deepen those people and consequences without adding diagnosis or medical procedure. This is a prose-first game-content plan: the main deliverable is scene text, dialogue, records, and state-aware return passages. Any proposed prose is supplemental to the current authored content and remains a proposal until accepted through the game's existing narrative owner.

## 2. Story in one sentence

Dr. Ilze Kaar keeps an eight-bed clinic honest by naming what it can do, training the hands it has, and leaving the door open when the answer is difficult.

## 3. Verified local anchor and current story

`characters.json` defines `npc_ilze_kaar` as a clinic physician who keeps an eight-bed almshouse clinic, inventories supplies twice a day, trains people whether or not they can read, and keeps one treatment ledger column: alive or not. `npc_arcs.json` describes initial, outbreak, late, recruited, and terminal states. `narrative_encounters_npc_arcs.json` contains `enc_arc_ilze_clinic` and `enc_arc_ilze_outbreak`. `quests_npc_arcs.json` contains `quest_arc_ilze_01_clinic`, `quest_arc_ilze_02_outbreak`, and `quest_arc_ilze_03_clinic`. The location is `loc_st_brigids_almshouse`. The current choices are supply or pass, then extract Ilze or send supplies and trained hands to hold the clinic. The existing branch text includes the fever breaking in nineteen days only on the hold path. Later states distinguish a kept clinic, a waystation ally, and a clinic that has lost its training work. This plan respects those conditions. The JSON record proves that these names, places, and story conditions are authored; it does not by itself prove a route is currently reachable in every campaign. Keep the plan's proposed insertions subordinate to the live content and its current-state conditions.

## 4. Fixed canon and proposed text

The almshouse was a hospice and its old rooms were left clean at the end of a shift. The current story establishes eight beds, dwindling shelves, a fever she can name but not stop, and sixteen bedrolls during the outbreak. It does not identify the fever here or authorize treatment advice. The 19-day outcome belongs only to the current fortify choice. Existing quest IDs, choice IDs, character states, names, location descriptions, and branch conditions remain the authority. Every additional visitor, line, paper, gesture, or callback below is proposed writing, not new established history.

## 5. Human center

Ilze refuses to let honesty become a reason to stop working. Her stated odds are not a disclaimer that makes the patient disposable. She shows inventory before asking, bills at cost when the existing choice says so, and teaches people who may never have been offered a classroom. The player can witness and respond, but the character's life does not become a demonstration designed for the player's moral growth. Let material detail carry emotion; do not tell the player which feeling to have.

## 6. Voice and point of view

Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Keep narration close to evidence: what someone counts, moves, refuses to sign, or leaves within reach. People speak from their job and their fatigue. Nobody explains their own symbolism.

## 7. Placement in the current story

Use early fragments around the existing inventory encounter, outbreak fragments only under the current fever-state entry, and later lines only under `npc_arcs.json` conditions. Keep the supply/pass choice, extract/fortify choice, and late treatment booking intact. None of these proposed fragments is a new diagnosis, cure, or clinic capacity upgrade. The sequence below proposes alternate or additional prose for existing content surfaces. It does not introduce a parallel quest chain, duplicate a registered encounter, or promise a new arrival. Use the current quest or location owner to decide where an accepted line belongs.

## 8. Player agency and consequence

The current choices let the player restock or walk past, then choose to bring Ilze out or leave her with enough supplies and trained hands to hold. The prose can make the patient and training consequences visible without declaring one choice morally pure. Preserve the possibility that the player does not come. Preserve the player's ability to help, refuse, wait, or leave. Consequence should be visible in an authored state that already exists or in text conditional on an existing branch; do not imply new state from a line alone.

## 9. Dignity, safety, and scope

Keep medical content non-prescriptive and grounded in the source. Use bedside details, training, consent, and accountable wording. Do not name a disease or imply that the player can safely diagnose or treat one from a scene. Keep every hazardous, medical, private, or coercive subject within the boundaries of the source. The prose can show limits and uncertainty without explaining a dangerous workaround, exposing a hidden person's identity, or making an unverified treatment promise.

## 10. Content integration boundary

This plan adds no production code, JSON, quest ID, encounter ID, location, faction, route, resource rule, score, or save field. If an editor later selects a fragment, verify the existing content schema and state consumer first, then place it with the current owning data. The plan itself is review material only.

## 11. Content bank: proposed prose

The following beats each have four editorial forms: a scene, a diegetic record, an optional conversation, and a return vignette. They are alternatives for the same moment, not four mandatory encounters. Select a coherent subset and keep each fragment inside the current character state described above.

### Scene draft 001 — The inventory is shown before the request

Ilze walks past the shelves without stopping so the visitor can see what is missing. She names medicine and clean water only after the count is visible. The request does not arrive as a test of generosity; the evidence is already on the table. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “You should know what is here before I ask what you can spare.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not invent item quantities or treatment recipes.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. On a return, show only the inventory version current to the existing branch.

Scene close: Inventory note: Shelf count shown before request; asking list read aloud. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 002 — The inventory is shown before the request

Proposed diegetic text: “Inventory note: Shelf count shown before request; asking list read aloud.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Ilze walks past the shelves without stopping so the visitor can see what is missing. She names medicine and clean water only after the count is visible. The request does not arrive as a test of generosity; the evidence is already on the table. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: On a return, show only the inventory version current to the existing branch. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not invent item quantities or treatment recipes. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 003 — The inventory is shown before the request

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. Ilze walks past the shelves without stopping so the visitor can see what is missing. She names medicine and clean water only after the count is visible. The request does not arrive as a test of generosity; the evidence is already on the table. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “You should know what is here before I ask what you can spare.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “On a return, show only the inventory version current to the existing branch.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not invent item quantities or treatment recipes. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Inventory note: Shelf count shown before request; asking list read aloud. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 004 — The inventory is shown before the request

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Ilze walks past the shelves without stopping so the visitor can see what is missing. She names medicine and clean water only after the count is visible. The request does not arrive as a test of generosity; the evidence is already on the table. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: On a return, show only the inventory version current to the existing branch. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not invent item quantities or treatment recipes.

Dr. Ilze Kaar may say: “You should know what is here before I ask what you can spare.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Inventory note: Shelf count shown before request; asking list read aloud. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 005 — Eight beds in a quiet room

The beds are made and the charts stop at the same date. The old ward is still clean, but the current clinic has an active physician and people waiting. Keep the source distinction visible; do not turn the previous hospice into an outbreak origin. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “A clean room is useful. It is not the same thing as a promise.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No patient names or assumed diagnoses.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the story reaches sixteen bedrolls, use that existing outbreak state rather than this quiet image.

Scene close: Ward card: Eight-bed clinic; current date entered by its keeper. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 006 — Eight beds in a quiet room

Proposed diegetic text: “Ward card: Eight-bed clinic; current date entered by its keeper.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The beds are made and the charts stop at the same date. The old ward is still clean, but the current clinic has an active physician and people waiting. Keep the source distinction visible; do not turn the previous hospice into an outbreak origin. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the story reaches sixteen bedrolls, use that existing outbreak state rather than this quiet image. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No patient names or assumed diagnoses. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 007 — Eight beds in a quiet room

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. The beds are made and the charts stop at the same date. The old ward is still clean, but the current clinic has an active physician and people waiting. Keep the source distinction visible; do not turn the previous hospice into an outbreak origin. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “A clean room is useful. It is not the same thing as a promise.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “If the story reaches sixteen bedrolls, use that existing outbreak state rather than this quiet image.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No patient names or assumed diagnoses. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Ward card: Eight-bed clinic; current date entered by its keeper. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 008 — Eight beds in a quiet room

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The beds are made and the charts stop at the same date. The old ward is still clean, but the current clinic has an active physician and people waiting. Keep the source distinction visible; do not turn the previous hospice into an outbreak origin. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the story reaches sixteen bedrolls, use that existing outbreak state rather than this quiet image. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No patient names or assumed diagnoses.

Dr. Ilze Kaar may say: “A clean room is useful. It is not the same thing as a promise.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Ward card: Eight-bed clinic; current date entered by its keeper. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 009 — The count is read twice

Ilze checks the same shelves in the morning and later in the day. A trainee asks whether the second count is mistrust. Ilze answers that a repeated count makes the change legible; it does not accuse the person who used the supply. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “A count can be careful without being suspicious.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not add an automatic inventory mechanic.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Let the second count differ only when the authored scene has a reason.

Scene close: Trainee note: Two daily inventories compared; difference left visible. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 010 — The count is read twice

Proposed diegetic text: “Trainee note: Two daily inventories compared; difference left visible.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Ilze checks the same shelves in the morning and later in the day. A trainee asks whether the second count is mistrust. Ilze answers that a repeated count makes the change legible; it does not accuse the person who used the supply. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Let the second count differ only when the authored scene has a reason. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not add an automatic inventory mechanic. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 011 — The count is read twice

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. Ilze checks the same shelves in the morning and later in the day. A trainee asks whether the second count is mistrust. Ilze answers that a repeated count makes the change legible; it does not accuse the person who used the supply. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “A count can be careful without being suspicious.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Let the second count differ only when the authored scene has a reason.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not add an automatic inventory mechanic. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Trainee note: Two daily inventories compared; difference left visible. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 012 — The count is read twice

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Ilze checks the same shelves in the morning and later in the day. A trainee asks whether the second count is mistrust. Ilze answers that a repeated count makes the change legible; it does not accuse the person who used the supply. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Let the second count differ only when the authored scene has a reason. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not add an automatic inventory mechanic.

Dr. Ilze Kaar may say: “A count can be careful without being suspicious.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Trainee note: Two daily inventories compared; difference left visible. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 013 — The goods are entered, the name is not

If the player restocks the clinic at cost, Ilze records the goods and does not enter the giver’s name in her care ledger. A later patient receives the same stated odds as everyone else. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “I know what came in. That does not buy a place ahead of the next bed.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Preserve the existing `ilze_supply` consequence; do not add a treatment discount.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. In the pass branch, do not show this receipt as if the shelves were restocked.

Scene close: Supply receipt: Goods accepted at cost; donor name not transferred to treatment ledger. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 014 — The goods are entered, the name is not

Proposed diegetic text: “Supply receipt: Goods accepted at cost; donor name not transferred to treatment ledger.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the player restocks the clinic at cost, Ilze records the goods and does not enter the giver’s name in her care ledger. A later patient receives the same stated odds as everyone else. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: In the pass branch, do not show this receipt as if the shelves were restocked. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Preserve the existing `ilze_supply` consequence; do not add a treatment discount. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 015 — The goods are entered, the name is not

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. If the player restocks the clinic at cost, Ilze records the goods and does not enter the giver’s name in her care ledger. A later patient receives the same stated odds as everyone else. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “I know what came in. That does not buy a place ahead of the next bed.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “In the pass branch, do not show this receipt as if the shelves were restocked.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Preserve the existing `ilze_supply` consequence; do not add a treatment discount. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Supply receipt: Goods accepted at cost; donor name not transferred to treatment ledger. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 016 — The goods are entered, the name is not

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the player restocks the clinic at cost, Ilze records the goods and does not enter the giver’s name in her care ledger. A later patient receives the same stated odds as everyone else. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: In the pass branch, do not show this receipt as if the shelves were restocked. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Preserve the existing `ilze_supply` consequence; do not add a treatment discount.

Dr. Ilze Kaar may say: “I know what came in. That does not buy a place ahead of the next bed.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Supply receipt: Goods accepted at cost; donor name not transferred to treatment ledger. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 017 — Walking past has a long sound

If the player declines, Ilze does not argue. The prose follows the queue rather than giving her a speech about blame. A later page can show the queue lengthening by the week, as the current consequence says, without assigning a number. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “You answered. I am not going to make you answer again.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not add a guilt score or exact queue count.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If `ilze_pass` applies, preserve her refusal to argue and the current queue consequence.

Scene close: Waiting-room slip: Request not supplied; queue continues. No name attributed. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 018 — Walking past has a long sound

Proposed diegetic text: “Waiting-room slip: Request not supplied; queue continues. No name attributed.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the player declines, Ilze does not argue. The prose follows the queue rather than giving her a speech about blame. A later page can show the queue lengthening by the week, as the current consequence says, without assigning a number. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If `ilze_pass` applies, preserve her refusal to argue and the current queue consequence. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not add a guilt score or exact queue count. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 019 — Walking past has a long sound

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. If the player declines, Ilze does not argue. The prose follows the queue rather than giving her a speech about blame. A later page can show the queue lengthening by the week, as the current consequence says, without assigning a number. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “You answered. I am not going to make you answer again.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “If `ilze_pass` applies, preserve her refusal to argue and the current queue consequence.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not add a guilt score or exact queue count. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Waiting-room slip: Request not supplied; queue continues. No name attributed. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 020 — Walking past has a long sound

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the player declines, Ilze does not argue. The prose follows the queue rather than giving her a speech about blame. A later page can show the queue lengthening by the week, as the current consequence says, without assigning a number. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If `ilze_pass` applies, preserve her refusal to argue and the current queue consequence. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not add a guilt score or exact queue count.

Dr. Ilze Kaar may say: “You answered. I am not going to make you answer again.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Waiting-room slip: Request not supplied; queue continues. No name attributed. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 021 — The lesson is spoken, not simplified

A trainee who cannot read receives a spoken demonstration of a routine responsibility. Ilze asks them to show the task back in their own sequence, then corrects one omission without turning literacy into a condition for care. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “Show me the part you remember. We can work from there.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not describe a medical procedure or invent a certification.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the clinic remains open, later training may acknowledge the same learner without a new skill score.

Scene close: Training card: Task taught aloud; trainee demonstrated the steps; text copy optional. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 022 — The lesson is spoken, not simplified

Proposed diegetic text: “Training card: Task taught aloud; trainee demonstrated the steps; text copy optional.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A trainee who cannot read receives a spoken demonstration of a routine responsibility. Ilze asks them to show the task back in their own sequence, then corrects one omission without turning literacy into a condition for care. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the clinic remains open, later training may acknowledge the same learner without a new skill score. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not describe a medical procedure or invent a certification. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 023 — The lesson is spoken, not simplified

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. A trainee who cannot read receives a spoken demonstration of a routine responsibility. Ilze asks them to show the task back in their own sequence, then corrects one omission without turning literacy into a condition for care. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “Show me the part you remember. We can work from there.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “If the clinic remains open, later training may acknowledge the same learner without a new skill score.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not describe a medical procedure or invent a certification. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Training card: Task taught aloud; trainee demonstrated the steps; text copy optional. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 024 — The lesson is spoken, not simplified

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A trainee who cannot read receives a spoken demonstration of a routine responsibility. Ilze asks them to show the task back in their own sequence, then corrects one omission without turning literacy into a condition for care. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the clinic remains open, later training may acknowledge the same learner without a new skill score. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not describe a medical procedure or invent a certification.

Dr. Ilze Kaar may say: “Show me the part you remember. We can work from there.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Training card: Task taught aloud; trainee demonstrated the steps; text copy optional. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 025 — A word is not a chart

Someone asks Ilze to put a confident label beside a patient. She writes only what the current record supports. The fever remains a fever in public text; uncertainty stays visible rather than hidden behind a technical-sounding name. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “A name we cannot support would make the page look more certain than the room.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No disease identity, diagnosis, or transmission route.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. A later record changes only if the source story supplies a new fact.

Scene close: Chart margin: Symptom described; cause not identified in this note. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 026 — A word is not a chart

Proposed diegetic text: “Chart margin: Symptom described; cause not identified in this note.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Someone asks Ilze to put a confident label beside a patient. She writes only what the current record supports. The fever remains a fever in public text; uncertainty stays visible rather than hidden behind a technical-sounding name. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: A later record changes only if the source story supplies a new fact. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No disease identity, diagnosis, or transmission route. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 027 — A word is not a chart

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. Someone asks Ilze to put a confident label beside a patient. She writes only what the current record supports. The fever remains a fever in public text; uncertainty stays visible rather than hidden behind a technical-sounding name. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “A name we cannot support would make the page look more certain than the room.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “A later record changes only if the source story supplies a new fact.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No disease identity, diagnosis, or transmission route. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Chart margin: Symptom described; cause not identified in this note. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 028 — A word is not a chart

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Someone asks Ilze to put a confident label beside a patient. She writes only what the current record supports. The fever remains a fever in public text; uncertainty stays visible rather than hidden behind a technical-sounding name. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: A later record changes only if the source story supplies a new fact. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No disease identity, diagnosis, or transmission route.

Dr. Ilze Kaar may say: “A name we cannot support would make the page look more certain than the room.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Chart margin: Symptom described; cause not identified in this note. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 029 — Sixteen bedrolls in the aisle

The outbreak version of the almshouse has bedrolls in the aisles, a rhythm on the slate, and a cordon rumor already two days old. Ilze has written to the player without asking them to come. The letter leaves the choice open. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “I have written because you should know. I have not written because you owe me.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Keep the source outbreak and avoid infection-control directions.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Use only after the existing `quest_arc_ilze_02_outbreak` condition is met.

Scene close: Letter opening: The aisle count is changing; the fever rhythm is on the slate; no arrival requested. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 030 — Sixteen bedrolls in the aisle

Proposed diegetic text: “Letter opening: The aisle count is changing; the fever rhythm is on the slate; no arrival requested.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The outbreak version of the almshouse has bedrolls in the aisles, a rhythm on the slate, and a cordon rumor already two days old. Ilze has written to the player without asking them to come. The letter leaves the choice open. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Use only after the existing `quest_arc_ilze_02_outbreak` condition is met. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Keep the source outbreak and avoid infection-control directions. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 031 — Sixteen bedrolls in the aisle

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. The outbreak version of the almshouse has bedrolls in the aisles, a rhythm on the slate, and a cordon rumor already two days old. Ilze has written to the player without asking them to come. The letter leaves the choice open. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “I have written because you should know. I have not written because you owe me.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Use only after the existing `quest_arc_ilze_02_outbreak` condition is met.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Keep the source outbreak and avoid infection-control directions. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Letter opening: The aisle count is changing; the fever rhythm is on the slate; no arrival requested. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 032 — Sixteen bedrolls in the aisle

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The outbreak version of the almshouse has bedrolls in the aisles, a rhythm on the slate, and a cordon rumor already two days old. Ilze has written to the player without asking them to come. The letter leaves the choice open. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Use only after the existing `quest_arc_ilze_02_outbreak` condition is met. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Keep the source outbreak and avoid infection-control directions.

Dr. Ilze Kaar may say: “I have written because you should know. I have not written because you owe me.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Letter opening: The aisle count is changing; the fever rhythm is on the slate; no arrival requested. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 033 — The slate does not name a cure

The fever pattern on the slate is described only as a rhythm Ilze is tracking. A visitor wants to interpret it. Ilze says the record can show when it changes, not why. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “I can keep watch over the pattern. I cannot call that control.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No clinical timetable or prediction is added.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. The current branch controls the outcome; the slate cannot foreshadow an unsupported cure.

Scene close: Slate caption: Observed rhythm recorded; cause and outcome remain open. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 034 — The slate does not name a cure

Proposed diegetic text: “Slate caption: Observed rhythm recorded; cause and outcome remain open.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The fever pattern on the slate is described only as a rhythm Ilze is tracking. A visitor wants to interpret it. Ilze says the record can show when it changes, not why. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: The current branch controls the outcome; the slate cannot foreshadow an unsupported cure. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No clinical timetable or prediction is added. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 035 — The slate does not name a cure

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. The fever pattern on the slate is described only as a rhythm Ilze is tracking. A visitor wants to interpret it. Ilze says the record can show when it changes, not why. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “I can keep watch over the pattern. I cannot call that control.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “The current branch controls the outcome; the slate cannot foreshadow an unsupported cure.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No clinical timetable or prediction is added. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Slate caption: Observed rhythm recorded; cause and outcome remain open. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 036 — The slate does not name a cure

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The fever pattern on the slate is described only as a rhythm Ilze is tracking. A visitor wants to interpret it. Ilze says the record can show when it changes, not why. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: The current branch controls the outcome; the slate cannot foreshadow an unsupported cure. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No clinical timetable or prediction is added.

Dr. Ilze Kaar may say: “I can keep watch over the pattern. I cannot call that control.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Slate caption: Observed rhythm recorded; cause and outcome remain open. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 037 — Come for her or come for the clinic

The letter offers distinct requests: bring Ilze out before the cordon closes, or bring enough supplies and hands to hold the clinic and leave her there. The prose gives equal clarity to both without making the patients scenery. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “Do not make the choice easier by pretending either side is empty.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Maintain the existing `ilze_extract` and `ilze_fortify` choices exactly.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Callback language must differ by which existing choice was selected.

Scene close: Decision note: Extract physician, or support clinic and leave physician in place. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 038 — Come for her or come for the clinic

Proposed diegetic text: “Decision note: Extract physician, or support clinic and leave physician in place.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The letter offers distinct requests: bring Ilze out before the cordon closes, or bring enough supplies and hands to hold the clinic and leave her there. The prose gives equal clarity to both without making the patients scenery. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Callback language must differ by which existing choice was selected. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Maintain the existing `ilze_extract` and `ilze_fortify` choices exactly. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 039 — Come for her or come for the clinic

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. The letter offers distinct requests: bring Ilze out before the cordon closes, or bring enough supplies and hands to hold the clinic and leave her there. The prose gives equal clarity to both without making the patients scenery. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “Do not make the choice easier by pretending either side is empty.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Callback language must differ by which existing choice was selected.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Maintain the existing `ilze_extract` and `ilze_fortify` choices exactly. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Decision note: Extract physician, or support clinic and leave physician in place. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 040 — Come for her or come for the clinic

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The letter offers distinct requests: bring Ilze out before the cordon closes, or bring enough supplies and hands to hold the clinic and leave her there. The prose gives equal clarity to both without making the patients scenery. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Callback language must differ by which existing choice was selected. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Maintain the existing `ilze_extract` and `ilze_fortify` choices exactly.

Dr. Ilze Kaar may say: “Do not make the choice easier by pretending either side is empty.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Decision note: Extract physician, or support clinic and leave physician in place. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 041 — A cart at dawn

On the extract branch, Ilze argues for an hour, treats through the night, and is on the cart by dawn because the work needs hands more than her signature. Keep the movement brief and practical; do not add a rescue method or claim that every patient can travel. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “If I stay, I am a name on a door. If I go, I can still work. Today I am going.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No patient transport protocol or guarantee.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Use only on the current extraction path.

Scene close: Transfer note: Ilze departed at dawn; clinic patients remain in the building. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 042 — A cart at dawn

Proposed diegetic text: “Transfer note: Ilze departed at dawn; clinic patients remain in the building.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: On the extract branch, Ilze argues for an hour, treats through the night, and is on the cart by dawn because the work needs hands more than her signature. Keep the movement brief and practical; do not add a rescue method or claim that every patient can travel. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Use only on the current extraction path. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No patient transport protocol or guarantee. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 043 — A cart at dawn

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. On the extract branch, Ilze argues for an hour, treats through the night, and is on the cart by dawn because the work needs hands more than her signature. Keep the movement brief and practical; do not add a rescue method or claim that every patient can travel. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “If I stay, I am a name on a door. If I go, I can still work. Today I am going.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Use only on the current extraction path.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No patient transport protocol or guarantee. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Transfer note: Ilze departed at dawn; clinic patients remain in the building. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 044 — A cart at dawn

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. On the extract branch, Ilze argues for an hour, treats through the night, and is on the cart by dawn because the work needs hands more than her signature. Keep the movement brief and practical; do not add a rescue method or claim that every patient can travel. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Use only on the current extraction path. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No patient transport protocol or guarantee.

Dr. Ilze Kaar may say: “If I stay, I am a name on a door. If I go, I can still work. Today I am going.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Transfer note: Ilze departed at dawn; clinic patients remain in the building. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 045 — Nineteen days belongs to one branch

On the clinic-held path, the source says the fever breaks in nineteen days. The proposed return shows an updated date in the existing log and keeps the cost visible; it does not celebrate a miracle or change what the player supplied. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “We held the clinic. That is what the page says. It does not say the work was easy.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not apply this outcome to the extract or pass branch.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. This date is displayed only when the existing fortify branch owns it.

Scene close: Return entry: Fever-break date recorded on day nineteen after the existing hold choice. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 046 — Nineteen days belongs to one branch

Proposed diegetic text: “Return entry: Fever-break date recorded on day nineteen after the existing hold choice.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: On the clinic-held path, the source says the fever breaks in nineteen days. The proposed return shows an updated date in the existing log and keeps the cost visible; it does not celebrate a miracle or change what the player supplied. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: This date is displayed only when the existing fortify branch owns it. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not apply this outcome to the extract or pass branch. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 047 — Nineteen days belongs to one branch

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. On the clinic-held path, the source says the fever breaks in nineteen days. The proposed return shows an updated date in the existing log and keeps the cost visible; it does not celebrate a miracle or change what the player supplied. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “We held the clinic. That is what the page says. It does not say the work was easy.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “This date is displayed only when the existing fortify branch owns it.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not apply this outcome to the extract or pass branch. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Return entry: Fever-break date recorded on day nineteen after the existing hold choice. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 048 — Nineteen days belongs to one branch

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. On the clinic-held path, the source says the fever breaks in nineteen days. The proposed return shows an updated date in the existing log and keeps the cost visible; it does not celebrate a miracle or change what the player supplied. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: This date is displayed only when the existing fortify branch owns it. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not apply this outcome to the extract or pass branch.

Dr. Ilze Kaar may say: “We held the clinic. That is what the page says. It does not say the work was easy.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Return entry: Fever-break date recorded on day nineteen after the existing hold choice. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 049 — The door at the appointed hour

In the clinic-kept late state, a traveler finds the door open at the appointed hour. A trainee answers first and refers a question they cannot safely answer. The scene makes the training visible without creating a new clinic service. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “I know who to ask. That is part of what she taught.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No new service schedule beyond the current authored line.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Only show this under `late_clinic_kept` or the exact current equivalent.

Scene close: Door card: Clinic open at its appointed hour; question passed to the responsible hand. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 050 — The door at the appointed hour

Proposed diegetic text: “Door card: Clinic open at its appointed hour; question passed to the responsible hand.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: In the clinic-kept late state, a traveler finds the door open at the appointed hour. A trainee answers first and refers a question they cannot safely answer. The scene makes the training visible without creating a new clinic service. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Only show this under `late_clinic_kept` or the exact current equivalent. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No new service schedule beyond the current authored line. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 051 — The door at the appointed hour

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. In the clinic-kept late state, a traveler finds the door open at the appointed hour. A trainee answers first and refers a question they cannot safely answer. The scene makes the training visible without creating a new clinic service. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “I know who to ask. That is part of what she taught.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Only show this under `late_clinic_kept` or the exact current equivalent.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No new service schedule beyond the current authored line. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Door card: Clinic open at its appointed hour; question passed to the responsible hand. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 052 — The door at the appointed hour

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. In the clinic-kept late state, a traveler finds the door open at the appointed hour. A trainee answers first and refers a question they cannot safely answer. The scene makes the training visible without creating a new clinic service. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Only show this under `late_clinic_kept` or the exact current equivalent. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No new service schedule beyond the current authored line.

Dr. Ilze Kaar may say: “I know who to ask. That is part of what she taught.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Door card: Clinic open at its appointed hour; question passed to the responsible hand. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 053 — The sign without the training roster

If the late clinic-lost state applies, Ilze still answers the door but the training has stopped. The proposed text does not declare the clinic closed or list the camp’s sick; it lets the missing training show in one unfinished handoff. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “The door still opens. Do not mistake that for the work being the same.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No additional death, withdrawal, or diagnosis.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Keep this separate from the clinic-kept and ally outcomes.

Scene close: Training margin: No current learner assigned; prior notes retained. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 054 — The sign without the training roster

Proposed diegetic text: “Training margin: No current learner assigned; prior notes retained.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the late clinic-lost state applies, Ilze still answers the door but the training has stopped. The proposed text does not declare the clinic closed or list the camp’s sick; it lets the missing training show in one unfinished handoff. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Keep this separate from the clinic-kept and ally outcomes. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No additional death, withdrawal, or diagnosis. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 055 — The sign without the training roster

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. If the late clinic-lost state applies, Ilze still answers the door but the training has stopped. The proposed text does not declare the clinic closed or list the camp’s sick; it lets the missing training show in one unfinished handoff. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “The door still opens. Do not mistake that for the work being the same.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Keep this separate from the clinic-kept and ally outcomes.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No additional death, withdrawal, or diagnosis. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Training margin: No current learner assigned; prior notes retained. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 056 — The sign without the training roster

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the late clinic-lost state applies, Ilze still answers the door but the training has stopped. The proposed text does not declare the clinic closed or list the camp’s sick; it lets the missing training show in one unfinished handoff. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Keep this separate from the clinic-kept and ally outcomes. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No additional death, withdrawal, or diagnosis.

Dr. Ilze Kaar may say: “The door still opens. Do not mistake that for the work being the same.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Training margin: No current learner assigned; prior notes retained. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 057 — The queue is not a diagnosis

A person in the waiting room asks whether the order of the beds says who is most ill. Ilze explains only what the order on this particular page means: where someone is waiting to be seen. No symptoms or diagnosis are added to make the exchange more dramatic. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “The line tells me who waits. It does not tell me what is wrong with them.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No triage protocol, diagnosis, or medical instruction is proposed.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Keep later copies from turning the queue mark into a prognosis.

Scene close: Queue margin: Order marks waiting only; clinical assessment remains with Ilze. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 058 — The queue is not a diagnosis

Proposed diegetic text: “Queue margin: Order marks waiting only; clinical assessment remains with Ilze.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A person in the waiting room asks whether the order of the beds says who is most ill. Ilze explains only what the order on this particular page means: where someone is waiting to be seen. No symptoms or diagnosis are added to make the exchange more dramatic. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Keep later copies from turning the queue mark into a prognosis. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No triage protocol, diagnosis, or medical instruction is proposed. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 059 — The queue is not a diagnosis

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. A person in the waiting room asks whether the order of the beds says who is most ill. Ilze explains only what the order on this particular page means: where someone is waiting to be seen. No symptoms or diagnosis are added to make the exchange more dramatic. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “The line tells me who waits. It does not tell me what is wrong with them.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Keep later copies from turning the queue mark into a prognosis.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No triage protocol, diagnosis, or medical instruction is proposed. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Queue margin: Order marks waiting only; clinical assessment remains with Ilze. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 060 — The queue is not a diagnosis

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A person in the waiting room asks whether the order of the beds says who is most ill. Ilze explains only what the order on this particular page means: where someone is waiting to be seen. No symptoms or diagnosis are added to make the exchange more dramatic. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Keep later copies from turning the queue mark into a prognosis. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No triage protocol, diagnosis, or medical instruction is proposed.

Dr. Ilze Kaar may say: “The line tells me who waits. It does not tell me what is wrong with them.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Queue margin: Order marks waiting only; clinical assessment remains with Ilze. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 061 — Twice daily is still a count

Ilze completes the second inventory while another person reads each item aloud. The count is a working record, not a promise that every listed thing will last until morning. A missing supply is recorded as missing rather than explained with an invented theft or allocation rule. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “The page can tell us what is gone. It cannot tell us why without evidence.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not create stock totals, a medicine catalog, or a restocking mechanic.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. A later count may change only when the existing data or accepted content supports it.

Scene close: Inventory edge: Second daily count completed; shortages named as observed. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 062 — Twice daily is still a count

Proposed diegetic text: “Inventory edge: Second daily count completed; shortages named as observed.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Ilze completes the second inventory while another person reads each item aloud. The count is a working record, not a promise that every listed thing will last until morning. A missing supply is recorded as missing rather than explained with an invented theft or allocation rule. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: A later count may change only when the existing data or accepted content supports it. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not create stock totals, a medicine catalog, or a restocking mechanic. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 063 — Twice daily is still a count

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. Ilze completes the second inventory while another person reads each item aloud. The count is a working record, not a promise that every listed thing will last until morning. A missing supply is recorded as missing rather than explained with an invented theft or allocation rule. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “The page can tell us what is gone. It cannot tell us why without evidence.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “A later count may change only when the existing data or accepted content supports it.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not create stock totals, a medicine catalog, or a restocking mechanic. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Inventory edge: Second daily count completed; shortages named as observed. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 064 — Twice daily is still a count

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Ilze completes the second inventory while another person reads each item aloud. The count is a working record, not a promise that every listed thing will last until morning. A missing supply is recorded as missing rather than explained with an invented theft or allocation rule. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: A later count may change only when the existing data or accepted content supports it. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not create stock totals, a medicine catalog, or a restocking mechanic.

Dr. Ilze Kaar may say: “The page can tell us what is gone. It cannot tell us why without evidence.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Inventory edge: Second daily count completed; shortages named as observed. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 065 — The learner turns the page

A trainee has been following Ilze’s demonstration by watching the placement of marks and tools, not by reading the page aloud. Ilze lets them turn the page themselves. The prose shows instruction adapting to literacy without specifying a procedure or making the learner a licensed substitute. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “Show me what you understood. We can start there.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No treatment steps, credential, or substitute-clinician status is added.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. In a late clinic-kept state, training may continue only in the broad terms already authored.

Scene close: Training note: Learner followed the handoff by demonstration; no independent authority assigned. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 066 — The learner turns the page

Proposed diegetic text: “Training note: Learner followed the handoff by demonstration; no independent authority assigned.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A trainee has been following Ilze’s demonstration by watching the placement of marks and tools, not by reading the page aloud. Ilze lets them turn the page themselves. The prose shows instruction adapting to literacy without specifying a procedure or making the learner a licensed substitute. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: In a late clinic-kept state, training may continue only in the broad terms already authored. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No treatment steps, credential, or substitute-clinician status is added. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 067 — The learner turns the page

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. A trainee has been following Ilze’s demonstration by watching the placement of marks and tools, not by reading the page aloud. Ilze lets them turn the page themselves. The prose shows instruction adapting to literacy without specifying a procedure or making the learner a licensed substitute. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “Show me what you understood. We can start there.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “In a late clinic-kept state, training may continue only in the broad terms already authored.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No treatment steps, credential, or substitute-clinician status is added. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Training note: Learner followed the handoff by demonstration; no independent authority assigned. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 068 — The learner turns the page

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A trainee has been following Ilze’s demonstration by watching the placement of marks and tools, not by reading the page aloud. Ilze lets them turn the page themselves. The prose shows instruction adapting to literacy without specifying a procedure or making the learner a licensed substitute. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: In a late clinic-kept state, training may continue only in the broad terms already authored. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No treatment steps, credential, or substitute-clinician status is added.

Dr. Ilze Kaar may say: “Show me what you understood. We can start there.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Training note: Learner followed the handoff by demonstration; no independent authority assigned. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 069 — Sixteen bedrolls beside eight beds

During the outbreak visit, the room contains sixteen bedrolls while the clinic has eight beds. The count makes the pressure visible but does not decide who receives a bed, whether a person is sick, or what the next outcome will be. Ilze walks past the count to check the current work. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “Count what you can see. Leave the part you cannot know in the margin.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not infer sixteen patients, assign symptoms, or alter the eight-bed fact.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Keep this image tied to the current outbreak encounter and its branch.

Scene close: Room count: Sixteen bedrolls visible; eight beds in the established clinic. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 070 — Sixteen bedrolls beside eight beds

Proposed diegetic text: “Room count: Sixteen bedrolls visible; eight beds in the established clinic.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: During the outbreak visit, the room contains sixteen bedrolls while the clinic has eight beds. The count makes the pressure visible but does not decide who receives a bed, whether a person is sick, or what the next outcome will be. Ilze walks past the count to check the current work. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Keep this image tied to the current outbreak encounter and its branch. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not infer sixteen patients, assign symptoms, or alter the eight-bed fact. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 071 — Sixteen bedrolls beside eight beds

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. During the outbreak visit, the room contains sixteen bedrolls while the clinic has eight beds. The count makes the pressure visible but does not decide who receives a bed, whether a person is sick, or what the next outcome will be. Ilze walks past the count to check the current work. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “Count what you can see. Leave the part you cannot know in the margin.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Keep this image tied to the current outbreak encounter and its branch.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not infer sixteen patients, assign symptoms, or alter the eight-bed fact. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Room count: Sixteen bedrolls visible; eight beds in the established clinic. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 072 — Sixteen bedrolls beside eight beds

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. During the outbreak visit, the room contains sixteen bedrolls while the clinic has eight beds. The count makes the pressure visible but does not decide who receives a bed, whether a person is sick, or what the next outcome will be. Ilze walks past the count to check the current work. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Keep this image tied to the current outbreak encounter and its branch. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not infer sixteen patients, assign symptoms, or alter the eight-bed fact.

Dr. Ilze Kaar may say: “Count what you can see. Leave the part you cannot know in the margin.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Room count: Sixteen bedrolls visible; eight beds in the established clinic. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 073 — Nineteen days belongs to one path

If the clinic-held outcome is current, a later wall note may refer to the fever breaking after nineteen days. It does not call the result a cure for each person or suggest that the same timeline follows the extraction branch. Ilze leaves the ledger column as it was designed: alive or not. Let the first image belong to the place and to the people who were already working there. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Dr. Ilze Kaar: “The fever broke. I am not going to let one sentence erase the days before it.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Never apply the nineteen-day event to another choice path or promise individual recovery.

The human question beneath the exchange is what does it mean to tell someone the odds and then stay for the work? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Show only after the matching existing branch is reached.

Scene close: Return note: Fever broke after nineteen days on the clinic-held branch; individual outcomes remain as recorded. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 074 — Nineteen days belongs to one path

Proposed diegetic text: “Return note: Fever broke after nineteen days on the clinic-held branch; individual outcomes remain as recorded.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the clinic-held outcome is current, a later wall note may refer to the fever breaking after nineteen days. It does not call the result a cure for each person or suggest that the same timeline follows the extraction branch. Ilze leaves the ledger column as it was designed: alive or not. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Show only after the matching existing branch is reached. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Never apply the nineteen-day event to another choice path or promise individual recovery. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 075 — Nineteen days belongs to one path

Dr. Ilze Kaar is speaking with someone whose next decision is affected by the work. If the clinic-held outcome is current, a later wall note may refer to the fever breaking after nineteen days. It does not call the result a cure for each person or suggest that the same timeline follows the extraction branch. Ilze leaves the ledger column as it was designed: alive or not. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson.

Dr. Ilze Kaar: “The fever broke. I am not going to let one sentence erase the days before it.”
Other voice: “A trainee looks at the page, then at the bed: “I can keep the count. I am not sure I can say it without making it sound final.””
Dr. Ilze Kaar: “Show only after the matching existing branch is reached.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Never apply the nineteen-day event to another choice path or promise individual recovery. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Return note: Fever broke after nineteen days on the clinic-held branch; individual outcomes remain as recorded. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 076 — Nineteen days belongs to one path

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the clinic-held outcome is current, a later wall note may refer to the fever breaking after nineteen days. It does not call the result a cure for each person or suggest that the same timeline follows the extraction branch. Ilze leaves the ledger column as it was designed: alive or not. The return does not need a speech announcing its meaning. Ilze speaks in observed facts and narrow promises. She states what she has counted and what she cannot claim. A trainee can hesitate without being mocked; a patient can decline a conversation without losing a bed in the scene. Avoid a heroic doctor speech and avoid calling illness a lesson. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Show only after the matching existing branch is reached. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Never apply the nineteen-day event to another choice path or promise individual recovery.

Dr. Ilze Kaar may say: “The fever broke. I am not going to let one sentence erase the days before it.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Return note: Fever broke after nineteen days on the clinic-held branch; individual outcomes remain as recorded. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

## 12. Short line bank

- You should know what is here before I ask what you can spare.
- A clean room is useful. It is not the same thing as a promise.
- A count can be careful without being suspicious.
- I know what came in. That does not buy a place ahead of the next bed.
- You answered. I am not going to make you answer again.
- Show me the part you remember. We can work from there.
- A name we cannot support would make the page look more certain than the room.
- I have written because you should know. I have not written because you owe me.
- I can keep watch over the pattern. I cannot call that control.
- Do not make the choice easier by pretending either side is empty.
- If I stay, I am a name on a door. If I go, I can still work. Today I am going.
- We held the clinic. That is what the page says. It does not say the work was easy.
- I know who to ask. That is part of what she taught.
- The door still opens. Do not mistake that for the work being the same.
- The line tells me who waits. It does not tell me what is wrong with them.
- The page can tell us what is gone. It cannot tell us why without evidence.
- Show me what you understood. We can start there.
- Count what you can see. Leave the part you cannot know in the margin.
- The fever broke. I am not going to let one sentence erase the days before it.

## 13. Continuity and editorial review

Check the exact current state before using each passage: initial eight-bed inventory, fever outbreak, held clinic, extracted clinic, late clinic-kept, late ally, clinic-lost, dead, or recruited. Keep the 19-day fever resolution on the branch that already owns it. No extra bed, treatment outcome, or medical log system is proposed. Keep all proposed versions clearly mapped to their existing branch condition. Confirm each source fact against the current JSON before implementation; do not treat a long prose draft as permission to rewrite a canon choice, route, or save contract. A shorter selected set is expected in the game.

## 14. Acceptance boundary

This content plan is ready for editorial review when a writer can select the fragments that fit the existing campaign state, preserve the character's agency and voice, and stage the result without inventing a new game system or contradicting authored data. It does not authorize production implementation. The character count is the Unicode character count of the saved Markdown file.
