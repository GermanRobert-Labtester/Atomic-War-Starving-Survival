# EXPANSION 107 — The Figure in Both Hands

## Oskar Ruut, a contract he reads twice, and the buyout figure that can become either an advance or a route taken from him.

### Wave 21: Records Kept in Human Hands

## 1. Expansion thesis

Keep Oskar’s debt legible without making the debt itself a villain that excuses every choice around it. His contract and buyout figure are both visible at the encounter. The player’s existing answer—help at fair interest in writing, or press the debt and buy the routes—should be understood as a choice between terms and power, not a puzzle about arithmetic. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character quest. It adds no gameplay feature and does not claim that any proposed text is already in the game.

## 2. Story in one sentence

A caravan debtor who keeps every signed copy asks the visitor to look at the same number he is looking at, then waits to see which hand moves first.

## 3. Verified local anchor and current story

`characters.json` defines `npc_oskar_ruut`, Caravan Debtor, at `loc_cut_merchant_caravanserai`: he owns one good wagon and half of another, with the other half belonging to the Underwrite; he wants fuel and the buyout figure in writing, offers caravan access and honest arithmetic, and will not sign a term he has not read twice. `npc_arcs.json` gives initial, evolved, late-independent, late-coerced, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_oskar_01_debt`; `quests_npc_arcs.json` registers `quest_arc_oskar_01_debt`. The existing choice IDs are `oskar_settle` and `oskar_press`. Settling leads to Working the Buyout and, later, the independent organizer with the Underwrite-stamped buyout and founding-member rate. Pressing leads to the coerced intermediary whose routes were bought cheap and whose contract copies are not shown. The no-choice late state remains the Crossing Factor with the debt at its own pace. No exact debt figure, interest percentage, fuel amount, or cargo schedule is authored in the reviewed text. Names, locations, quest IDs, choice IDs, and state summaries above come from the current data. The encounter catalog proves the authored scene and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing state condition.

## 4. Fixed canon and proposed prose

The encounter places the contract and buyout figure together. The fair-interest option is written and twice read; the pressing option buys his routes while the debt is cheap. The late independent state says the Underwrite stamps the buyout, Oskar frames that stamp, and wagons run on schedules he owns outright. The coerced state says he carries what the credit office needs and keeps his contract copies out of view. Preserve each outcome without adding an amount, deadline, breach, or new legal right. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to the character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Oskar is not naïve about debt. He has read, signed, and saved the terms because the cost of a copy that disagrees with his is paid by the person with less power. His careful arithmetic is a way to remain a participant in a transaction whose leverage is uneven. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–5 at `enc_arc_oskar_01_debt`, with the character at the caravanserai. Beats 6–10 belong only after `oskar_settle`, first in the evolved state and later in `late_independent`. Beats 11–13 apply only after `oskar_press` in `late_coerced`; beats 14–17 can reflect the no-choice `late_unmet` or current independent terms as individually marked. Do not show the independent and coerced callbacks together. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character-location owner as the insertion point; do not create a parallel quest chain or a second mutable owner.

## 8. Player agency and consequence

The current question is whether to front the buyout at fair interest under written, twice-read terms or to press the debt while it is cheap and buy the routes. Do not add a third answer, imply that one line of dialogue signs on the player’s behalf, or conceal how the coerced state changes who owns the routes. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before the choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

No instruction for avoiding a creditor or forcing a signature. Let the contract’s terms be read plainly, with room to refuse and no claim that fair arithmetic makes unequal leverage disappear. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when an implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The contract and the figure share a page

At the caravanserai Oskar holds the contract open and indicates the buyout figure on the same page. He does not begin by asking for help. He waits while the visitor reads the two lines in the order they appear, and watches for the moment one number is treated as the whole story. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “Read both. One tells you what I owe; the other tells you what would end the question.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not supply an amount, due date, or new clause. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: On return, preserve the paired presentation of contract and buyout. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The contract and the figure share a page

Proposed diegetic text: “Reading note: Contract shown beside the written buyout figure; no amount copied into this draft.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the caravanserai Oskar holds the contract open and indicates the buyout figure on the same page. He does not begin by asking for help. He waits while the visitor reads the two lines in the order they appear, and watches for the moment one number is treated as the whole story. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not supply an amount, due date, or new clause. Do not let the form claim authority that its keeper has not been given.

Later reading: On return, preserve the paired presentation of contract and buyout. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 003 — The contract and the figure share a page

At the caravanserai Oskar holds the contract open and indicates the buyout figure on the same page. He does not begin by asking for help. He waits while the visitor reads the two lines in the order they appear, and watches for the moment one number is treated as the whole story. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “Read both. One tells you what I owe; the other tells you what would end the question.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “On return, preserve the paired presentation of contract and buyout.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not supply an amount, due date, or new clause. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The contract and the figure share a page

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the caravanserai Oskar holds the contract open and indicates the buyout figure on the same page. He does not begin by asking for help. He waits while the visitor reads the two lines in the order they appear, and watches for the moment one number is treated as the whole story. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: On return, preserve the paired presentation of contract and buyout. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not supply an amount, due date, or new clause.

Oskar Ruut may say: “Read both. One tells you what I owe; the other tells you what would end the question.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Reading note: Contract shown beside the written buyout figure; no amount copied into this draft. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — Wax keeps a copy from becoming a rumor

Oskar’s waxed roll holds copies of contracts he has signed. He loosens the tie enough to show that the roll is a habit of record keeping, not a threat to produce every private term on demand. The visitor sees a folded edge and the careful order, not a gallery of other people’s debt. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “A memory can change in a room. Paper tends to stay where it was put.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No other debtor, contract, or signature is exposed. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: His late-coerced state explicitly keeps copies somewhere the player is never shown. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — Wax keeps a copy from becoming a rumor

Proposed diegetic text: “Roll label: Signed copies retained by Oskar; other parties’ terms not displayed.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Oskar’s waxed roll holds copies of contracts he has signed. He loosens the tie enough to show that the roll is a habit of record keeping, not a threat to produce every private term on demand. The visitor sees a folded edge and the careful order, not a gallery of other people’s debt. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No other debtor, contract, or signature is exposed. Do not let the form claim authority that its keeper has not been given.

Later reading: His late-coerced state explicitly keeps copies somewhere the player is never shown. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 007 — Wax keeps a copy from becoming a rumor

Oskar’s waxed roll holds copies of contracts he has signed. He loosens the tie enough to show that the roll is a habit of record keeping, not a threat to produce every private term on demand. The visitor sees a folded edge and the careful order, not a gallery of other people’s debt. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “A memory can change in a room. Paper tends to stay where it was put.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “His late-coerced state explicitly keeps copies somewhere the player is never shown.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No other debtor, contract, or signature is exposed. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — Wax keeps a copy from becoming a rumor

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Oskar’s waxed roll holds copies of contracts he has signed. He loosens the tie enough to show that the roll is a habit of record keeping, not a threat to produce every private term on demand. The visitor sees a folded edge and the careful order, not a gallery of other people’s debt. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: His late-coerced state explicitly keeps copies somewhere the player is never shown. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No other debtor, contract, or signature is exposed.

Oskar Ruut may say: “A memory can change in a room. Paper tends to stay where it was put.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Roll label: Signed copies retained by Oskar; other parties’ terms not displayed. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — A buyout figure is not a promise

Someone asks whether the written figure means the debt ends today. Oskar distinguishes a figure from a completed buyout and does not promise that either choice has already been made. He moves no token across the table until the player chooses through the registered quest. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “A number is not a hand on the other side of it.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not present the debt as paid before the current quest choice. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: The callback may change only when the corresponding state becomes current. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — A buyout figure is not a promise

Proposed diegetic text: “Margin: Figure exists in writing; settlement remains a choice.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Someone asks whether the written figure means the debt ends today. Oskar distinguishes a figure from a completed buyout and does not promise that either choice has already been made. He moves no token across the table until the player chooses through the registered quest. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not present the debt as paid before the current quest choice. Do not let the form claim authority that its keeper has not been given.

Later reading: The callback may change only when the corresponding state becomes current. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 011 — A buyout figure is not a promise

Someone asks whether the written figure means the debt ends today. Oskar distinguishes a figure from a completed buyout and does not promise that either choice has already been made. He moves no token across the table until the player chooses through the registered quest. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “A number is not a hand on the other side of it.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “The callback may change only when the corresponding state becomes current.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not present the debt as paid before the current quest choice. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — A buyout figure is not a promise

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Someone asks whether the written figure means the debt ends today. Oskar distinguishes a figure from a completed buyout and does not promise that either choice has already been made. He moves no token across the table until the player chooses through the registered quest. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: The callback may change only when the corresponding state becomes current. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not present the debt as paid before the current quest choice.

Oskar Ruut may say: “A number is not a hand on the other side of it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin: Figure exists in writing; settlement remains a choice. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — One good wagon, one divided share

Oskar speaks about the wagon as something he can use and the half of another as something he cannot describe as wholly his. The scene gives the ownership split enough room to matter without staging repossession or inventing a damage history. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “I can tell you which half is mine. I cannot make the other half mine by saying it faster.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Keep the exact ownership description; add no seizure or repair event. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Later route prose must preserve the Underwrite’s share until the buyout state says otherwise. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — One good wagon, one divided share

Proposed diegetic text: “Caravan note: One wagon owned; half of a second belongs to the Underwrite.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Oskar speaks about the wagon as something he can use and the half of another as something he cannot describe as wholly his. The scene gives the ownership split enough room to matter without staging repossession or inventing a damage history. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Keep the exact ownership description; add no seizure or repair event. Do not let the form claim authority that its keeper has not been given.

Later reading: Later route prose must preserve the Underwrite’s share until the buyout state says otherwise. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 015 — One good wagon, one divided share

Oskar speaks about the wagon as something he can use and the half of another as something he cannot describe as wholly his. The scene gives the ownership split enough room to matter without staging repossession or inventing a damage history. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “I can tell you which half is mine. I cannot make the other half mine by saying it faster.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Later route prose must preserve the Underwrite’s share until the buyout state says otherwise.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Keep the exact ownership description; add no seizure or repair event. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — One good wagon, one divided share

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Oskar speaks about the wagon as something he can use and the half of another as something he cannot describe as wholly his. The scene gives the ownership split enough room to matter without staging repossession or inventing a damage history. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Later route prose must preserve the Underwrite’s share until the buyout state says otherwise. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Keep the exact ownership description; add no seizure or repair event.

Oskar Ruut may say: “I can tell you which half is mine. I cannot make the other half mine by saying it faster.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Caravan note: One wagon owned; half of a second belongs to the Underwrite. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The twice-read term

Before the fair-interest choice, Oskar asks that the term be read once, then read back. There is no trick clause revealed after the player agrees; the emphasis is that everyone in the room heard the same writing. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “If it is fair, it will survive being read aloud twice.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not invent a percentage or make the player choose an undocumented condition. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only as clarity around `oskar_settle`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The twice-read term

Proposed diegetic text: “Receipt: Fair interest offered in writing and read twice.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Before the fair-interest choice, Oskar asks that the term be read once, then read back. There is no trick clause revealed after the player agrees; the emphasis is that everyone in the room heard the same writing. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent a percentage or make the player choose an undocumented condition. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only as clarity around `oskar_settle`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 019 — The twice-read term

Before the fair-interest choice, Oskar asks that the term be read once, then read back. There is no trick clause revealed after the player agrees; the emphasis is that everyone in the room heard the same writing. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “If it is fair, it will survive being read aloud twice.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Use only as clarity around `oskar_settle`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent a percentage or make the player choose an undocumented condition. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The twice-read term

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Before the fair-interest choice, Oskar asks that the term be read once, then read back. There is no trick clause revealed after the player agrees; the emphasis is that everyone in the room heard the same writing. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Use only as clarity around `oskar_settle`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent a percentage or make the player choose an undocumented condition.

Oskar Ruut may say: “If it is fair, it will survive being read aloud twice.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Receipt: Fair interest offered in writing and read twice. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — The route hand carries fuel

Oskar’s request includes fuel, but the prose does not translate that want into an exact quantity or a promise of range. A driver asks what the fuel would change. Oskar answers in terms of keeping the wagon working, not a newly specified route schedule. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “I asked for fuel. I did not tell you it buys a particular mile.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No travel distance, cargo capacity, or delivery amount is added. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the request a character want unless the current quest assigns a quantity. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — The route hand carries fuel

Proposed diegetic text: “Request card: Fuel wanted; quantity and delivery not stated.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Oskar’s request includes fuel, but the prose does not translate that want into an exact quantity or a promise of range. A driver asks what the fuel would change. Oskar answers in terms of keeping the wagon working, not a newly specified route schedule. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No travel distance, cargo capacity, or delivery amount is added. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the request a character want unless the current quest assigns a quantity. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 023 — The route hand carries fuel

Oskar’s request includes fuel, but the prose does not translate that want into an exact quantity or a promise of range. A driver asks what the fuel would change. Oskar answers in terms of keeping the wagon working, not a newly specified route schedule. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “I asked for fuel. I did not tell you it buys a particular mile.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Keep the request a character want unless the current quest assigns a quantity.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No travel distance, cargo capacity, or delivery amount is added. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — The route hand carries fuel

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Oskar’s request includes fuel, but the prose does not translate that want into an exact quantity or a promise of range. A driver asks what the fuel would change. Oskar answers in terms of keeping the wagon working, not a newly specified route schedule. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Keep the request a character want unless the current quest assigns a quantity. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No travel distance, cargo capacity, or delivery amount is added.

Oskar Ruut may say: “I asked for fuel. I did not tell you it buys a particular mile.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Request card: Fuel wanted; quantity and delivery not stated. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — The advance moves the figure within reach

After `oskar_settle`, the evolved state says the player’s advance moved the buyout within reach. Oskar has not yet become the independent organizer. He quotes the player’s rate first when quoting anyone, with no extra promise attached. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “You moved it closer. I have not told you it is already behind me.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not skip the evolved state or award the late rate early. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only show after `oskar_settle` when `evolved` is current. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — The advance moves the figure within reach

Proposed diegetic text: “Working copy: Advance moved the figure within reach; rate quoted first.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `oskar_settle`, the evolved state says the player’s advance moved the buyout within reach. Oskar has not yet become the independent organizer. He quotes the player’s rate first when quoting anyone, with no extra promise attached. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not skip the evolved state or award the late rate early. Do not let the form claim authority that its keeper has not been given.

Later reading: Only show after `oskar_settle` when `evolved` is current. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 027 — The advance moves the figure within reach

After `oskar_settle`, the evolved state says the player’s advance moved the buyout within reach. Oskar has not yet become the independent organizer. He quotes the player’s rate first when quoting anyone, with no extra promise attached. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “You moved it closer. I have not told you it is already behind me.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Only show after `oskar_settle` when `evolved` is current.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not skip the evolved state or award the late rate early. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — The advance moves the figure within reach

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `oskar_settle`, the evolved state says the player’s advance moved the buyout within reach. Oskar has not yet become the independent organizer. He quotes the player’s rate first when quoting anyone, with no extra promise attached. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Only show after `oskar_settle` when `evolved` is current. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not skip the evolved state or award the late rate early.

Oskar Ruut may say: “You moved it closer. I have not told you it is already behind me.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Working copy: Advance moved the figure within reach; rate quoted first. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — A rate can be spoken without being changed

At a later exchange in the evolved state, Oskar states the player’s rate before discussing other terms. The line demonstrates his memory, not a new discount or automatic entitlement. Another trader can hear the rate without being made a witness to the entire contract. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “I said your rate first. I did not say the wagon carries free.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No rate amount, schedule, or exemption is added. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep this separate from the founding-member rate in `late_independent`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — A rate can be spoken without being changed

Proposed diegetic text: “Rate note: Player’s rate quoted first; no new value written here.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At a later exchange in the evolved state, Oskar states the player’s rate before discussing other terms. The line demonstrates his memory, not a new discount or automatic entitlement. Another trader can hear the rate without being made a witness to the entire contract. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No rate amount, schedule, or exemption is added. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep this separate from the founding-member rate in `late_independent`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 031 — A rate can be spoken without being changed

At a later exchange in the evolved state, Oskar states the player’s rate before discussing other terms. The line demonstrates his memory, not a new discount or automatic entitlement. Another trader can hear the rate without being made a witness to the entire contract. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “I said your rate first. I did not say the wagon carries free.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Keep this separate from the founding-member rate in `late_independent`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No rate amount, schedule, or exemption is added. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — A rate can be spoken without being changed

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At a later exchange in the evolved state, Oskar states the player’s rate before discussing other terms. The line demonstrates his memory, not a new discount or automatic entitlement. Another trader can hear the rate without being made a witness to the entire contract. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Keep this separate from the founding-member rate in `late_independent`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No rate amount, schedule, or exemption is added.

Oskar Ruut may say: “I said your rate first. I did not say the wagon carries free.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Rate note: Player’s rate quoted first; no new value written here. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The stamp hangs in a frame

In `late_independent`, the Underwrite has stamped the buyout and Oskar framed the stamp. The proposed prose shows a clean frame with ordinary dust gathering at its lower edge. It does not make the stamp a trophy the player may handle or revoke. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “I framed the stamp. I did not frame you.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add authority over the stamp or a revocation scene. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only with `late_independent`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The stamp hangs in a frame

Proposed diegetic text: “Wall card: Underwrite-stamped buyout framed by Oskar.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_independent`, the Underwrite has stamped the buyout and Oskar framed the stamp. The proposed prose shows a clean frame with ordinary dust gathering at its lower edge. It does not make the stamp a trophy the player may handle or revoke. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add authority over the stamp or a revocation scene. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only with `late_independent`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 035 — The stamp hangs in a frame

In `late_independent`, the Underwrite has stamped the buyout and Oskar framed the stamp. The proposed prose shows a clean frame with ordinary dust gathering at its lower edge. It does not make the stamp a trophy the player may handle or revoke. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “I framed the stamp. I did not frame you.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Use only with `late_independent`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add authority over the stamp or a revocation scene. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The stamp hangs in a frame

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_independent`, the Underwrite has stamped the buyout and Oskar framed the stamp. The proposed prose shows a clean frame with ordinary dust gathering at its lower edge. It does not make the stamp a trophy the player may handle or revoke. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Use only with `late_independent`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add authority over the stamp or a revocation scene.

Oskar Ruut may say: “I framed the stamp. I did not frame you.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Wall card: Underwrite-stamped buyout framed by Oskar. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — The founding rate lasts

The independent state says the player’s cargo moves at the founding-member rate forever. The line appears as a written term Oskar can repeat without converting it into a universal price for every traveler. He does not promise a place on every wagon. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “Forever is a long word. It is still one line in the agreement.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not set a numeric rate or generalize it to other parties. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: The wording belongs only to the existing independent organizer state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — The founding rate lasts

Proposed diegetic text: “Rate margin: Founding-member rate applies to the player’s cargo under the existing late state.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The independent state says the player’s cargo moves at the founding-member rate forever. The line appears as a written term Oskar can repeat without converting it into a universal price for every traveler. He does not promise a place on every wagon. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not set a numeric rate or generalize it to other parties. Do not let the form claim authority that its keeper has not been given.

Later reading: The wording belongs only to the existing independent organizer state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 039 — The founding rate lasts

The independent state says the player’s cargo moves at the founding-member rate forever. The line appears as a written term Oskar can repeat without converting it into a universal price for every traveler. He does not promise a place on every wagon. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “Forever is a long word. It is still one line in the agreement.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “The wording belongs only to the existing independent organizer state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not set a numeric rate or generalize it to other parties. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — The founding rate lasts

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The independent state says the player’s cargo moves at the founding-member rate forever. The line appears as a written term Oskar can repeat without converting it into a universal price for every traveler. He does not promise a place on every wagon. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: The wording belongs only to the existing independent organizer state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not set a numeric rate or generalize it to other parties.

Oskar Ruut may say: “Forever is a long word. It is still one line in the agreement.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Rate margin: Founding-member rate applies to the player’s cargo under the existing late state. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The cheap route has a person on it

If `oskar_press` is current, the later scene does not celebrate a bargain. Oskar carries what the credit office needs carried now; the prose stays with his hand on the strap and does not invent the package or its destination. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “You got the route. I am still the one expected to walk it.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No cargo, employer procedure, or new coercion mechanism is specified. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Condition on `late_coerced`; do not show beside the independent state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The cheap route has a person on it

Proposed diegetic text: “Office slip: Oskar is carrying what the credit office needs; contents and destination not supplied.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `oskar_press` is current, the later scene does not celebrate a bargain. Oskar carries what the credit office needs carried now; the prose stays with his hand on the strap and does not invent the package or its destination. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No cargo, employer procedure, or new coercion mechanism is specified. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `late_coerced`; do not show beside the independent state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 043 — The cheap route has a person on it

If `oskar_press` is current, the later scene does not celebrate a bargain. Oskar carries what the credit office needs carried now; the prose stays with his hand on the strap and does not invent the package or its destination. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “You got the route. I am still the one expected to walk it.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Condition on `late_coerced`; do not show beside the independent state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No cargo, employer procedure, or new coercion mechanism is specified. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The cheap route has a person on it

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If `oskar_press` is current, the later scene does not celebrate a bargain. Oskar carries what the credit office needs carried now; the prose stays with his hand on the strap and does not invent the package or its destination. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Condition on `late_coerced`; do not show beside the independent state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No cargo, employer procedure, or new coercion mechanism is specified.

Oskar Ruut may say: “You got the route. I am still the one expected to walk it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Office slip: Oskar is carrying what the credit office needs; contents and destination not supplied. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The copies are somewhere else

In the coerced state, the player asks to inspect Oskar’s copies. He refuses to show where they are kept. The boundary is complete on the page; the plan does not follow him or explain how another person could search for them. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “You do not need to see where I keep them to understand why I kept them.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Never reveal the location or a route to the hidden copies. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Preserve the source’s explicit withholding in every coerced callback. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The copies are somewhere else

Proposed diegetic text: “Copy note: Contract copies retained; storage location undisclosed.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the coerced state, the player asks to inspect Oskar’s copies. He refuses to show where they are kept. The boundary is complete on the page; the plan does not follow him or explain how another person could search for them. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Never reveal the location or a route to the hidden copies. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the source’s explicit withholding in every coerced callback. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 047 — The copies are somewhere else

In the coerced state, the player asks to inspect Oskar’s copies. He refuses to show where they are kept. The boundary is complete on the page; the plan does not follow him or explain how another person could search for them. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “You do not need to see where I keep them to understand why I kept them.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Preserve the source’s explicit withholding in every coerced callback.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Never reveal the location or a route to the hidden copies. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The copies are somewhere else

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the coerced state, the player asks to inspect Oskar’s copies. He refuses to show where they are kept. The boundary is complete on the page; the plan does not follow him or explain how another person could search for them. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Preserve the source’s explicit withholding in every coerced callback. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Never reveal the location or a route to the hidden copies.

Oskar Ruut may say: “You do not need to see where I keep them to understand why I kept them.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy note: Contract copies retained; storage location undisclosed. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — No one makes the keys speak

A memory of the route transfer can focus on the keys resting between two people before the choice. The proposed passage does not add dialogue from the keys, a handover ritual, or a scene in which the player can secretly undo a settled outcome. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “Metal is easy to pass. Terms are harder to take back.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add item transfer, secret reversal, or new quest result. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only as a reflection after the current route state is known. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — No one makes the keys speak

Proposed diegetic text: “Handover note: Route ownership follows the existing choice; no separate key event.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A memory of the route transfer can focus on the keys resting between two people before the choice. The proposed passage does not add dialogue from the keys, a handover ritual, or a scene in which the player can secretly undo a settled outcome. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add item transfer, secret reversal, or new quest result. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only as a reflection after the current route state is known. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 051 — No one makes the keys speak

A memory of the route transfer can focus on the keys resting between two people before the choice. The proposed passage does not add dialogue from the keys, a handover ritual, or a scene in which the player can secretly undo a settled outcome. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “Metal is easy to pass. Terms are harder to take back.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Use only as a reflection after the current route state is known.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add item transfer, secret reversal, or new quest result. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — No one makes the keys speak

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A memory of the route transfer can focus on the keys resting between two people before the choice. The proposed passage does not add dialogue from the keys, a handover ritual, or a scene in which the player can secretly undo a settled outcome. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Use only as a reflection after the current route state is known. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add item transfer, secret reversal, or new quest result.

Oskar Ruut may say: “Metal is easy to pass. Terms are harder to take back.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Handover note: Route ownership follows the existing choice; no separate key event. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — The debt at its own pace

If `late_unmet` is current, Oskar continues to negotiate first and the debt walks with him at its own pace. A page on the scale desk records no new agreement. His life has not paused because the player did not settle or press. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “You did not choose for me. I can keep moving from here.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not imply a hidden third resolution or unchanged exact balance. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only while `late_unmet` remains the active state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — The debt at its own pace

Proposed diegetic text: “Open account: No new settlement recorded; negotiation continues.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `late_unmet` is current, Oskar continues to negotiate first and the debt walks with him at its own pace. A page on the scale desk records no new agreement. His life has not paused because the player did not settle or press. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not imply a hidden third resolution or unchanged exact balance. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only while `late_unmet` remains the active state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 055 — The debt at its own pace

If `late_unmet` is current, Oskar continues to negotiate first and the debt walks with him at its own pace. A page on the scale desk records no new agreement. His life has not paused because the player did not settle or press. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “You did not choose for me. I can keep moving from here.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Use only while `late_unmet` remains the active state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not imply a hidden third resolution or unchanged exact balance. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — The debt at its own pace

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If `late_unmet` is current, Oskar continues to negotiate first and the debt walks with him at its own pace. A page on the scale desk records no new agreement. His life has not paused because the player did not settle or press. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Use only while `late_unmet` remains the active state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not imply a hidden third resolution or unchanged exact balance.

Oskar Ruut may say: “You did not choose for me. I can keep moving from here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Open account: No new settlement recorded; negotiation continues. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — A schedule he owns outright

The independent organizer’s wagons run on schedules he owns outright along the crossing roads. One notice can show a route day as a piece of Oskar’s ordinary work, without introducing the schedule’s hours or all the roads it serves. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “I own the schedule. That is the sentence I wanted to be able to say.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No departure time, new route, or guaranteed cargo slot. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only in the independent late state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — A schedule he owns outright

Proposed diegetic text: “Route notice: Crossing-road schedule owned by Oskar; detailed timetable not reproduced.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The independent organizer’s wagons run on schedules he owns outright along the crossing roads. One notice can show a route day as a piece of Oskar’s ordinary work, without introducing the schedule’s hours or all the roads it serves. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No departure time, new route, or guaranteed cargo slot. Do not let the form claim authority that its keeper has not been given.

Later reading: Only in the independent late state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 059 — A schedule he owns outright

The independent organizer’s wagons run on schedules he owns outright along the crossing roads. One notice can show a route day as a piece of Oskar’s ordinary work, without introducing the schedule’s hours or all the roads it serves. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “I own the schedule. That is the sentence I wanted to be able to say.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Only in the independent late state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No departure time, new route, or guaranteed cargo slot. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — A schedule he owns outright

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The independent organizer’s wagons run on schedules he owns outright along the crossing roads. One notice can show a route day as a piece of Oskar’s ordinary work, without introducing the schedule’s hours or all the roads it serves. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Only in the independent late state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No departure time, new route, or guaranteed cargo slot.

Oskar Ruut may say: “I own the schedule. That is the sentence I wanted to be able to say.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Route notice: Crossing-road schedule owned by Oskar; detailed timetable not reproduced. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — Both copies stay readable

When Oskar and another party read the same term, the draft leaves room for each to point to the line they mean. It does not declare that a future disagreement is impossible. His copies protect memory; they do not make every deal fair by themselves. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “Paper helps us see where we disagree. It does not make us agree.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No universal contract validation rule is introduced. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: A return should preserve his carefulness without making him infallible. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — Both copies stay readable

Proposed diegetic text: “Two-copy note: Both parties can read the same current term; dispute not presumed resolved forever.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: When Oskar and another party read the same term, the draft leaves room for each to point to the line they mean. It does not declare that a future disagreement is impossible. His copies protect memory; they do not make every deal fair by themselves. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No universal contract validation rule is introduced. Do not let the form claim authority that its keeper has not been given.

Later reading: A return should preserve his carefulness without making him infallible. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 063 — Both copies stay readable

When Oskar and another party read the same term, the draft leaves room for each to point to the line they mean. It does not declare that a future disagreement is impossible. His copies protect memory; they do not make every deal fair by themselves. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “Paper helps us see where we disagree. It does not make us agree.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “A return should preserve his carefulness without making him infallible.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No universal contract validation rule is introduced. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — Both copies stay readable

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. When Oskar and another party read the same term, the draft leaves room for each to point to the line they mean. It does not declare that a future disagreement is impossible. His copies protect memory; they do not make every deal fair by themselves. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: A return should preserve his carefulness without making him infallible. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No universal contract validation rule is introduced.

Oskar Ruut may say: “Paper helps us see where we disagree. It does not make us agree.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Two-copy note: Both parties can read the same current term; dispute not presumed resolved forever. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The last answer is still his

At the end of a visit, Oskar winds the waxed tie around the roll and decides whether to keep talking. The player is not required to receive a quotation or a moral. His contract remains his to store, and his answer remains his to give. Begin with the work already under way, before the visitor is asked to decide what it means. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Oskar Ruut: “If you have another question, ask it next time. I have to count what is here.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No forced confession, gratitude, or repeated trade reward. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Close within whichever current state is true. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The last answer is still his

Proposed diegetic text: “Closing note: Oskar retains his copies and closes the roll.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the end of a visit, Oskar winds the waxed tie around the roll and decides whether to keep talking. The player is not required to receive a quotation or a moral. His contract remains his to store, and his answer remains his to give. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No forced confession, gratitude, or repeated trade reward. Do not let the form claim authority that its keeper has not been given.

Later reading: Close within whichever current state is true. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 067 — The last answer is still his

At the end of a visit, Oskar winds the waxed tie around the roll and decides whether to keep talking. The player is not required to receive a quotation or a moral. His contract remains his to store, and his answer remains his to give. Let Oskar Ruut speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Oskar Ruut: “If you have another question, ask it next time. I have to count what is here.”
Other voice: “A weigh-yard hand answers, “I can read a number. I cannot tell you which person gets to keep it.””
Oskar Ruut: “Close within whichever current state is true.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No forced confession, gratitude, or repeated trade reward. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The last answer is still his

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the end of a visit, Oskar winds the waxed tie around the roll and decides whether to keep talking. The player is not required to receive a quotation or a moral. His contract remains his to store, and his answer remains his to give. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Oskar is patient in the way of a person who knows the other party may try to hurry the page. He names a figure, waits for it to be read back, and dislikes metaphors that hide a term. He can be dry without becoming a comic miser. His warmth appears in the courtesy of making the same copy available to both hands.

Observable return: Close within whichever current state is true. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No forced confession, gratitude, or repeated trade reward.

Oskar Ruut may say: “If you have another question, ask it next time. I have to count what is here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Closing note: Oskar retains his copies and closes the roll. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- Read both. One tells you what I owe; the other tells you what would end the question.
- A memory can change in a room. Paper tends to stay where it was put.
- A number is not a hand on the other side of it.
- I can tell you which half is mine. I cannot make the other half mine by saying it faster.
- If it is fair, it will survive being read aloud twice.
- I asked for fuel. I did not tell you it buys a particular mile.
- You moved it closer. I have not told you it is already behind me.
- I said your rate first. I did not say the wagon carries free.
- I framed the stamp. I did not frame you.
- Forever is a long word. It is still one line in the agreement.
- You got the route. I am still the one expected to walk it.
- You do not need to see where I keep them to understand why I kept them.
- Metal is easy to pass. Terms are harder to take back.
- You did not choose for me. I can keep moving from here.
- I own the schedule. That is the sentence I wanted to be able to say.
- Paper helps us see where we disagree. It does not make us agree.
- If you have another question, ask it next time. I have to count what is here.

## 13. Continuity and editorial review

Verify each line against `quest_arc_oskar_01_debt`, `enc_arc_oskar_01_debt`, and the corresponding `npc_arcs.json` state. The current data establishes the half-owned wagon, the written buyout, the two choices, and the later state summaries. Exact figures, contract clauses, fuel quantities, route timetables, and the location of hidden copies are not established and must stay unspecified. Confirm current content before placement. Keep the first-visit encounter recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposed only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the existing character and encounter content, each callback matches the authored choice, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
