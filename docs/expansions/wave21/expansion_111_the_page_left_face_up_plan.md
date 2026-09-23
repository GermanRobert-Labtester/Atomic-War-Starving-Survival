# EXPANSION 111 — The Page Left Face Up

## Mirael Tesk, a copy lesson held beside a page in another hand, and the choice to learn doubt or ask the archive to rule.

### Wave 21: Records Kept in Human Hands

## 1. Expansion thesis

Keep the altered page visible without making it a scavenger clue whose answer belongs to the player. Mirael teaches copy discipline with the page face up and names no one. The current choice is to sit the lessons or name the page in session and make the archive rule on it. Both choices are free in the sense that neither is hidden behind a trust test; both have different authored consequences. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character quest. It adds no gameplay feature and does not claim that any proposed text is already in the game.

## 2. Story in one sentence

A teacher leaves a disputed page on the desk and lets the student decide whether to learn from its presence or ask the archive to judge it.

## 3. Verified local anchor and current story

`characters.json` defines `npc_mirael_tesk` as a Record Teacher at `loc_municipal_archive`. She teaches copy discipline, and one page of the community record is in a different hand; the alteration protects someone now two winters gone. `npc_arcs.json` defines initial, evolved, late-keeper, late-disputed, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_mirael_01_page`; `quests_npc_arcs.json` registers `quest_arc_mirael_01_page`. The current encounter `The Page on the Desk` says the page is face up during the copy lessons and Mirael names nothing. `mirael_sit_the_lessons` spreads the discipline and doubt; the later keeper state names the Marek Voln school of honesty and a dated, signed margin note. `mirael_name_the_page` asks the archive to rule; the late disputed outcome acknowledges alteration, forgives the forger, and distrusts the teacher, as Mirael predicted in writing. Do not reveal who was protected or who altered the page. Names, locations, quest IDs, choice IDs, and state summaries above come from the current data. The encounter catalog proves the authored scene and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing state condition.

## 4. Fixed canon and proposed prose

Copy lessons are exact, the page is in a different hand, and the alteration protected someone now two winters gone. Mirael will not teach copying without teaching doubt and will not alter a second page. The copy desks are ruled in true lines; the encounter leaves the page visible. This plan does not invent handwriting tests, provenance rules, the protected person’s name, or the ruling’s process. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to the character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Mirael has chosen to teach in the presence of a page she cannot make simple without lying about what it protects. Her restraint is not an invitation to pry. The student may sit with the contradiction, or ask the archive to rule and accept the relationship the ruling leaves behind. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–5 around `enc_arc_mirael_01_page`. Beats 6–10 require `mirael_sit_the_lessons`, then `evolved` or `late_keeper`. Beats 11–14 require `mirael_name_the_page`, then `late_disputed`. Remaining beats can reflect the initial or unmet state. Keep all text at the copy desks and do not add a retrieval visit into the collapsed lower stacks. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character-location owner as the insertion point; do not create a parallel quest chain or a second mutable owner.

## 8. Player agency and consequence

The current choice offers the lessons or a ruling. Asking is free and not asking is free; do not put the identity behind a dialogue check or make the player’s curiosity the only route to future content. The archive’s ruling and its costs are already authored. Prose may clarify the difference but not soften the outcome. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before the choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep archival hazards as existing setting context. No instructions for recovering fused records, entering collapsed shelves, or producing a convincing forgery. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when an implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The ruled desks are not the page

The copy desks are ruled in true lines, but the page lies in the middle of them like a hole in the floor. Mirael begins the lesson with the orderly desk and lets the different hand remain visible without pointing at a name. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “The lines are exact. That page is not. Both things can be true at once.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not identify the hand or state what the alteration says. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the visible contrast without making it a puzzle marker. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The ruled desks are not the page

Proposed diegetic text: “Desk note: Copy desks ruled; disputed page remains face up.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The copy desks are ruled in true lines, but the page lies in the middle of them like a hole in the floor. Mirael begins the lesson with the orderly desk and lets the different hand remain visible without pointing at a name. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not identify the hand or state what the alteration says. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the visible contrast without making it a puzzle marker. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 003 — The ruled desks are not the page

The copy desks are ruled in true lines, but the page lies in the middle of them like a hole in the floor. Mirael begins the lesson with the orderly desk and lets the different hand remain visible without pointing at a name. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “The lines are exact. That page is not. Both things can be true at once.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep the visible contrast without making it a puzzle marker.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not identify the hand or state what the alteration says. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The ruled desks are not the page

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The copy desks are ruled in true lines, but the page lies in the middle of them like a hole in the floor. Mirael begins the lesson with the orderly desk and lets the different hand remain visible without pointing at a name. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep the visible contrast without making it a puzzle marker. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not identify the hand or state what the alteration says.

Mirael Tesk may say: “The lines are exact. That page is not. Both things can be true at once.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Desk note: Copy desks ruled; disputed page remains face up. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — Provenance is read aloud

Mirael reads the provenance attached to an ordinary copy and pauses where the record stops. The scene demonstrates that a copy carries a history without teaching a method for manufacturing a false one. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “We can read who carried this copy to the desk. We cannot invent who wrote what is missing.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No forgery checklist, technical handwriting test, or invented author. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the lesson focused on doubt and provenance, not detection steps. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — Provenance is read aloud

Proposed diegetic text: “Lesson margin: Provenance read aloud; unanswered field remains unanswered.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mirael reads the provenance attached to an ordinary copy and pauses where the record stops. The scene demonstrates that a copy carries a history without teaching a method for manufacturing a false one. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No forgery checklist, technical handwriting test, or invented author. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the lesson focused on doubt and provenance, not detection steps. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 007 — Provenance is read aloud

Mirael reads the provenance attached to an ordinary copy and pauses where the record stops. The scene demonstrates that a copy carries a history without teaching a method for manufacturing a false one. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “We can read who carried this copy to the desk. We cannot invent who wrote what is missing.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep the lesson focused on doubt and provenance, not detection steps.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No forgery checklist, technical handwriting test, or invented author. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — Provenance is read aloud

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Mirael reads the provenance attached to an ordinary copy and pauses where the record stops. The scene demonstrates that a copy carries a history without teaching a method for manufacturing a false one. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep the lesson focused on doubt and provenance, not detection steps. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No forgery checklist, technical handwriting test, or invented author.

Mirael Tesk may say: “We can read who carried this copy to the desk. We cannot invent who wrote what is missing.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Lesson margin: Provenance read aloud; unanswered field remains unanswered. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The page protects someone unnamed

The character record says the alteration protects somebody now two winters gone. Mirael leaves that person unnamed. The player may understand that a real person once mattered to the omission without acquiring a right to that person’s identity. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “Someone needed the page to read as it did. I will not tell you who.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No identity, biography, or motive beyond the source. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Preserve the unnamed person in every state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The page protects someone unnamed

Proposed diegetic text: “Private margin: Alteration protected an unnamed person; identity not copied.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The character record says the alteration protects somebody now two winters gone. Mirael leaves that person unnamed. The player may understand that a real person once mattered to the omission without acquiring a right to that person’s identity. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No identity, biography, or motive beyond the source. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the unnamed person in every state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 011 — The page protects someone unnamed

The character record says the alteration protects somebody now two winters gone. Mirael leaves that person unnamed. The player may understand that a real person once mattered to the omission without acquiring a right to that person’s identity. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “Someone needed the page to read as it did. I will not tell you who.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Preserve the unnamed person in every state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No identity, biography, or motive beyond the source. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The page protects someone unnamed

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The character record says the alteration protects somebody now two winters gone. Mirael leaves that person unnamed. The player may understand that a real person once mattered to the omission without acquiring a right to that person’s identity. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Preserve the unnamed person in every state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No identity, biography, or motive beyond the source.

Mirael Tesk may say: “Someone needed the page to read as it did. I will not tell you who.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Private margin: Alteration protected an unnamed person; identity not copied. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — Ask or do not ask

The encounter says asking is free and not asking is also free; the lesson is the same either way. The player can sit through the lesson without asking about the page. Mirael does not become warmer or colder to manufacture a reward. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “Ask me or do not ask me. The lesson is the same.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No trust gate, hidden bonus, or compulsory question. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep both actions valid before the registered choice. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — Ask or do not ask

Proposed diegetic text: “Session note: Question optional; lesson proceeds either way.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The encounter says asking is free and not asking is also free; the lesson is the same either way. The player can sit through the lesson without asking about the page. Mirael does not become warmer or colder to manufacture a reward. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No trust gate, hidden bonus, or compulsory question. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep both actions valid before the registered choice. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 015 — Ask or do not ask

The encounter says asking is free and not asking is also free; the lesson is the same either way. The player can sit through the lesson without asking about the page. Mirael does not become warmer or colder to manufacture a reward. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “Ask me or do not ask me. The lesson is the same.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep both actions valid before the registered choice.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No trust gate, hidden bonus, or compulsory question. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — Ask or do not ask

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The encounter says asking is free and not asking is also free; the lesson is the same either way. The player can sit through the lesson without asking about the page. Mirael does not become warmer or colder to manufacture a reward. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep both actions valid before the registered choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No trust gate, hidden bonus, or compulsory question.

Mirael Tesk may say: “Ask me or do not ask me. The lesson is the same.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Session note: Question optional; lesson proceeds either way. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The two choices stay visible

Mirael leaves the alternatives in plain language: sit the copy lessons and learn to doubt copies properly, or name the page in session and make the archive rule on it. The draft adds no third action and does not pretend the choices have the same consequence. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “You already know what each choice asks. I will not disguise either one.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Use only `mirael_sit_the_lessons` and `mirael_name_the_page`. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Place before the choice without adding cost or branching condition. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The two choices stay visible

Proposed diegetic text: “Choice card: Sit the lessons / name the page and request a ruling.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mirael leaves the alternatives in plain language: sit the copy lessons and learn to doubt copies properly, or name the page in session and make the archive rule on it. The draft adds no third action and does not pretend the choices have the same consequence. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Use only `mirael_sit_the_lessons` and `mirael_name_the_page`. Do not let the form claim authority that its keeper has not been given.

Later reading: Place before the choice without adding cost or branching condition. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 019 — The two choices stay visible

Mirael leaves the alternatives in plain language: sit the copy lessons and learn to doubt copies properly, or name the page in session and make the archive rule on it. The draft adds no third action and does not pretend the choices have the same consequence. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “You already know what each choice asks. I will not disguise either one.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Place before the choice without adding cost or branching condition.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Use only `mirael_sit_the_lessons` and `mirael_name_the_page`. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The two choices stay visible

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Mirael leaves the alternatives in plain language: sit the copy lessons and learn to doubt copies properly, or name the page in session and make the archive rule on it. The draft adds no third action and does not pretend the choices have the same consequence. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Place before the choice without adding cost or branching condition. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Use only `mirael_sit_the_lessons` and `mirael_name_the_page`.

Mirael Tesk may say: “You already know what each choice asks. I will not disguise either one.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Choice card: Sit the lessons / name the page and request a ruling. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — The lesson is exact, not simple

After `mirael_sit_the_lessons`, the player’s people learn the discipline and the doubt together. A student compares one copy with its source record under Mirael’s supervision, but no step-by-step authentication rule is included. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “You learned to copy it accurately. You also learned not to call accuracy the whole truth.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not create an audit procedure or make the student an archive authority. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only after the lessons choice. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — The lesson is exact, not simple

Proposed diegetic text: “Class note: Copy discipline and doubt taught together.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `mirael_sit_the_lessons`, the player’s people learn the discipline and the doubt together. A student compares one copy with its source record under Mirael’s supervision, but no step-by-step authentication rule is included. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not create an audit procedure or make the student an archive authority. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the lessons choice. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 023 — The lesson is exact, not simple

After `mirael_sit_the_lessons`, the player’s people learn the discipline and the doubt together. A student compares one copy with its source record under Mirael’s supervision, but no step-by-step authentication rule is included. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “You learned to copy it accurately. You also learned not to call accuracy the whole truth.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use only after the lessons choice.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not create an audit procedure or make the student an archive authority. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — The lesson is exact, not simple

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `mirael_sit_the_lessons`, the player’s people learn the discipline and the doubt together. A student compares one copy with its source record under Mirael’s supervision, but no step-by-step authentication rule is included. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use only after the lessons choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not create an audit procedure or make the student an archive authority.

Mirael Tesk may say: “You learned to copy it accurately. You also learned not to call accuracy the whole truth.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Class note: Copy discipline and doubt taught together. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — The desk stays open after class

In the evolved state, Mirael has started leaving the desk open after class, as close as the archive comes to confession. The prose may show the empty chair and an unclosed drawer, not a private file waiting to be searched. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “The desk is open. It does not mean every drawer is.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No private document access or new permission. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use with `evolved` and preserve the source’s limit. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — The desk stays open after class

Proposed diegetic text: “After-class note: Desk remains open; page access remains under current archive practice.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the evolved state, Mirael has started leaving the desk open after class, as close as the archive comes to confession. The prose may show the empty chair and an unclosed drawer, not a private file waiting to be searched. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No private document access or new permission. Do not let the form claim authority that its keeper has not been given.

Later reading: Use with `evolved` and preserve the source’s limit. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 027 — The desk stays open after class

In the evolved state, Mirael has started leaving the desk open after class, as close as the archive comes to confession. The prose may show the empty chair and an unclosed drawer, not a private file waiting to be searched. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “The desk is open. It does not mean every drawer is.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use with `evolved` and preserve the source’s limit.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No private document access or new permission. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — The desk stays open after class

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the evolved state, Mirael has started leaving the desk open after class, as close as the archive comes to confession. The prose may show the empty chair and an unclosed drawer, not a private file waiting to be searched. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use with `evolved` and preserve the source’s limit. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No private document access or new permission.

Mirael Tesk may say: “The desk is open. It does not mean every drawer is.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: After-class note: Desk remains open; page access remains under current archive practice. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — A margin note bears a date

In `late_keeper`, the disputed page finally receives a note in the margin, in Mirael’s hand, dated and signed. The note acknowledges the record’s history without adding the protected person’s name. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “The note says what I can stand behind. It does not say who needed the silence.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not invent the exact wording, date, or identity. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only show in `late_keeper`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — A margin note bears a date

Proposed diegetic text: “Margin note: Alteration acknowledged; dated and signed by Mirael; identity withheld.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_keeper`, the disputed page finally receives a note in the margin, in Mirael’s hand, dated and signed. The note acknowledges the record’s history without adding the protected person’s name. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent the exact wording, date, or identity. Do not let the form claim authority that its keeper has not been given.

Later reading: Only show in `late_keeper`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 031 — A margin note bears a date

In `late_keeper`, the disputed page finally receives a note in the margin, in Mirael’s hand, dated and signed. The note acknowledges the record’s history without adding the protected person’s name. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “The note says what I can stand behind. It does not say who needed the silence.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Only show in `late_keeper`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent the exact wording, date, or identity. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — A margin note bears a date

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_keeper`, the disputed page finally receives a note in the margin, in Mirael’s hand, dated and signed. The note acknowledges the record’s history without adding the protected person’s name. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Only show in `late_keeper`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent the exact wording, date, or identity.

Mirael Tesk may say: “The note says what I can stand behind. It does not say who needed the silence.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin note: Alteration acknowledged; dated and signed by Mirael; identity withheld. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The standard travels carefully

The late keeper state says the copy discipline spread to two settlements and the shelter’s records follow her standard. The prose can show a request for a lesson by correspondence without naming the settlements or defining a new curriculum. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “If they use it, they have to learn the doubt with the copy.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new settlement, curriculum, or training mechanic. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the count at two only where the late state applies. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The standard travels carefully

Proposed diegetic text: “Teaching copy: Standard adopted in two settlements; names omitted here.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late keeper state says the copy discipline spread to two settlements and the shelter’s records follow her standard. The prose can show a request for a lesson by correspondence without naming the settlements or defining a new curriculum. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new settlement, curriculum, or training mechanic. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the count at two only where the late state applies. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 035 — The standard travels carefully

The late keeper state says the copy discipline spread to two settlements and the shelter’s records follow her standard. The prose can show a request for a lesson by correspondence without naming the settlements or defining a new curriculum. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “If they use it, they have to learn the doubt with the copy.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep the count at two only where the late state applies.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new settlement, curriculum, or training mechanic. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The standard travels carefully

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The late keeper state says the copy discipline spread to two settlements and the shelter’s records follow her standard. The prose can show a request for a lesson by correspondence without naming the settlements or defining a new curriculum. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep the count at two only where the late state applies. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new settlement, curriculum, or training mechanic.

Mirael Tesk may say: “If they use it, they have to learn the doubt with the copy.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Teaching copy: Standard adopted in two settlements; names omitted here. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — The school name remains attached

The archivists call it the Marek Voln school of honesty. Mirael can accept the name while clarifying that the standard is the work people practice, not a personal credential the player can award. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “A name can travel farther than the practice. Make sure the practice arrives too.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not expand Marek’s role or create a new institutional title. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only in `late_keeper` and keep source phrasing recognizable. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — The school name remains attached

Proposed diegetic text: “Archive label: Marek Voln school of honesty, as current late state names it.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The archivists call it the Marek Voln school of honesty. Mirael can accept the name while clarifying that the standard is the work people practice, not a personal credential the player can award. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not expand Marek’s role or create a new institutional title. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in `late_keeper` and keep source phrasing recognizable. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 039 — The school name remains attached

The archivists call it the Marek Voln school of honesty. Mirael can accept the name while clarifying that the standard is the work people practice, not a personal credential the player can award. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “A name can travel farther than the practice. Make sure the practice arrives too.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use only in `late_keeper` and keep source phrasing recognizable.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not expand Marek’s role or create a new institutional title. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — The school name remains attached

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The archivists call it the Marek Voln school of honesty. Mirael can accept the name while clarifying that the standard is the work people practice, not a personal credential the player can award. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use only in `late_keeper` and keep source phrasing recognizable. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not expand Marek’s role or create a new institutional title.

Mirael Tesk may say: “A name can travel farther than the practice. Make sure the practice arrives too.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Archive label: Marek Voln school of honesty, as current late state names it. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The session names the page

After `mirael_name_the_page`, the page is named in session and the archive has to rule. The prose lets the room hear the request without exposing the page’s contents. Naming the page is not the same as naming the person it protected. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “You asked the archive to rule on the page. You did not ask me to name the person.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No disclosed text, person, or ruling procedure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only after `mirael_name_the_page`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The session names the page

Proposed diegetic text: “Session record: Page named for a ruling; protected person not identified.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `mirael_name_the_page`, the page is named in session and the archive has to rule. The prose lets the room hear the request without exposing the page’s contents. Naming the page is not the same as naming the person it protected. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No disclosed text, person, or ruling procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after `mirael_name_the_page`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 043 — The session names the page

After `mirael_name_the_page`, the page is named in session and the archive has to rule. The prose lets the room hear the request without exposing the page’s contents. Naming the page is not the same as naming the person it protected. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “You asked the archive to rule on the page. You did not ask me to name the person.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use only after `mirael_name_the_page`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No disclosed text, person, or ruling procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The session names the page

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `mirael_name_the_page`, the page is named in session and the archive has to rule. The prose lets the room hear the request without exposing the page’s contents. Naming the page is not the same as naming the person it protected. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use only after `mirael_name_the_page`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No disclosed text, person, or ruling procedure.

Mirael Tesk may say: “You asked the archive to rule on the page. You did not ask me to name the person.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Session record: Page named for a ruling; protected person not identified. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The ruling changes trust in two directions

The late disputed outcome acknowledges the alteration, forgives the forger, and distrusts the teacher. Mirael predicted that response in writing before the vote. The draft does not tell the player that the ruling was fair or unfair; it shows what it did to the room. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “I wrote what I thought the ruling would do. I did not write that it would feel good.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not soften, reverse, or add to the current outcome. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Condition on `late_disputed`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The ruling changes trust in two directions

Proposed diegetic text: “Ruling summary: Alteration acknowledged; forger forgiven; teacher distrusted.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late disputed outcome acknowledges the alteration, forgives the forger, and distrusts the teacher. Mirael predicted that response in writing before the vote. The draft does not tell the player that the ruling was fair or unfair; it shows what it did to the room. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not soften, reverse, or add to the current outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `late_disputed`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 047 — The ruling changes trust in two directions

The late disputed outcome acknowledges the alteration, forgives the forger, and distrusts the teacher. Mirael predicted that response in writing before the vote. The draft does not tell the player that the ruling was fair or unfair; it shows what it did to the room. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “I wrote what I thought the ruling would do. I did not write that it would feel good.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Condition on `late_disputed`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not soften, reverse, or add to the current outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The ruling changes trust in two directions

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The late disputed outcome acknowledges the alteration, forgives the forger, and distrusts the teacher. Mirael predicted that response in writing before the vote. The draft does not tell the player that the ruling was fair or unfair; it shows what it did to the room. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Condition on `late_disputed`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not soften, reverse, or add to the current outcome.

Mirael Tesk may say: “I wrote what I thought the ruling would do. I did not write that it would feel good.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Ruling summary: Alteration acknowledged; forger forgiven; teacher distrusted. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — The desk is clear and smaller

After the ruling, the desk is clear now, and smaller. The page no longer occupies its center. Mirael continues the lesson; the changed room does not mean the dispute has become simple or the protected person can be named. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “We cleared the page away. We did not clear away what it protected.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new archive disposition or destruction of the page. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only in the late disputed state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — The desk is clear and smaller

Proposed diegetic text: “Return note: Desk cleared after ruling; copy lesson continues.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the ruling, the desk is clear now, and smaller. The page no longer occupies its center. Mirael continues the lesson; the changed room does not mean the dispute has become simple or the protected person can be named. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new archive disposition or destruction of the page. Do not let the form claim authority that its keeper has not been given.

Later reading: Only in the late disputed state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 051 — The desk is clear and smaller

After the ruling, the desk is clear now, and smaller. The page no longer occupies its center. Mirael continues the lesson; the changed room does not mean the dispute has become simple or the protected person can be named. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “We cleared the page away. We did not clear away what it protected.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Only in the late disputed state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new archive disposition or destruction of the page. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — The desk is clear and smaller

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After the ruling, the desk is clear now, and smaller. The page no longer occupies its center. Mirael continues the lesson; the changed room does not mean the dispute has become simple or the protected person can be named. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Only in the late disputed state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new archive disposition or destruction of the page.

Mirael Tesk may say: “We cleared the page away. We did not clear away what it protected.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Return note: Desk cleared after ruling; copy lesson continues. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — The copy is not the original

A student asks whether making a copy preserves everything the original carried. Mirael answers that a copy can preserve wording and still lose the circumstances of the writing. The exchange stays conceptual and does not supply a forgery recipe. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “You can carry the words. You may not carry the whole room they were written in.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No extra provenance rule or technical guide. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use as a lesson fragment only where the current session permits it. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — The copy is not the original

Proposed diegetic text: “Margin: Copy preserves wording; circumstances may remain outside it.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A student asks whether making a copy preserves everything the original carried. Mirael answers that a copy can preserve wording and still lose the circumstances of the writing. The exchange stays conceptual and does not supply a forgery recipe. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No extra provenance rule or technical guide. Do not let the form claim authority that its keeper has not been given.

Later reading: Use as a lesson fragment only where the current session permits it. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 055 — The copy is not the original

A student asks whether making a copy preserves everything the original carried. Mirael answers that a copy can preserve wording and still lose the circumstances of the writing. The exchange stays conceptual and does not supply a forgery recipe. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “You can carry the words. You may not carry the whole room they were written in.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use as a lesson fragment only where the current session permits it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No extra provenance rule or technical guide. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — The copy is not the original

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A student asks whether making a copy preserves everything the original carried. Mirael answers that a copy can preserve wording and still lose the circumstances of the writing. The exchange stays conceptual and does not supply a forgery recipe. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use as a lesson fragment only where the current session permits it. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No extra provenance rule or technical guide.

Mirael Tesk may say: “You can carry the words. You may not carry the whole room they were written in.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin: Copy preserves wording; circumstances may remain outside it. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — Lower shelves stay outside the lesson

The municipal archive description says the rolling stacks have collapsed and much lower paper fused after fire suppression. The scene remains at the copy desks; no student is sent below waist height to recover proof. The desk lesson does not need a hazardous retrieval task. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “We are teaching what is in the room. We are not going beneath the fallen shelves.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No traversal route, salvage instruction, or chemical hazard procedure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep every passage at the existing encounter’s copy desks. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — Lower shelves stay outside the lesson

Proposed diegetic text: “Editorial boundary: Copy lesson remains at the desks; no lower-stack retrieval proposed.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The municipal archive description says the rolling stacks have collapsed and much lower paper fused after fire suppression. The scene remains at the copy desks; no student is sent below waist height to recover proof. The desk lesson does not need a hazardous retrieval task. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No traversal route, salvage instruction, or chemical hazard procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep every passage at the existing encounter’s copy desks. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 059 — Lower shelves stay outside the lesson

The municipal archive description says the rolling stacks have collapsed and much lower paper fused after fire suppression. The scene remains at the copy desks; no student is sent below waist height to recover proof. The desk lesson does not need a hazardous retrieval task. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “We are teaching what is in the room. We are not going beneath the fallen shelves.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep every passage at the existing encounter’s copy desks.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No traversal route, salvage instruction, or chemical hazard procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — Lower shelves stay outside the lesson

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The municipal archive description says the rolling stacks have collapsed and much lower paper fused after fire suppression. The scene remains at the copy desks; no student is sent below waist height to recover proof. The desk lesson does not need a hazardous retrieval task. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep every passage at the existing encounter’s copy desks. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No traversal route, salvage instruction, or chemical hazard procedure.

Mirael Tesk may say: “We are teaching what is in the room. We are not going beneath the fallen shelves.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial boundary: Copy lesson remains at the desks; no lower-stack retrieval proposed. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — The record is taught as it stands

Mirael offers the record as it stands. A student may ask for provenance to be read aloud, but the teacher does not alter the page or remove the uncertainty to make class more comfortable. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “I can read it aloud. I cannot make its history disappear.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No second alteration or hidden correction. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep consistent with her refusal to alter another page. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — The record is taught as it stands

Proposed diegetic text: “Class note: Record presented as it stands; uncertainty named.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mirael offers the record as it stands. A student may ask for provenance to be read aloud, but the teacher does not alter the page or remove the uncertainty to make class more comfortable. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No second alteration or hidden correction. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep consistent with her refusal to alter another page. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 063 — The record is taught as it stands

Mirael offers the record as it stands. A student may ask for provenance to be read aloud, but the teacher does not alter the page or remove the uncertainty to make class more comfortable. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “I can read it aloud. I cannot make its history disappear.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Keep consistent with her refusal to alter another page.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No second alteration or hidden correction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — The record is taught as it stands

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Mirael offers the record as it stands. A student may ask for provenance to be read aloud, but the teacher does not alter the page or remove the uncertainty to make class more comfortable. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Keep consistent with her refusal to alter another page. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No second alteration or hidden correction.

Mirael Tesk may say: “I can read it aloud. I cannot make its history disappear.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Class note: Record presented as it stands; uncertainty named. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The page can leave the center

In the initial or unmet state, the page stays face-up through each lesson. A proposed closing lets the class finish and leaves the sheet in its place. No one is compelled to ask, and no one is forced to be the witness who names it. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “You can go now. The page will still be here when I teach again.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No schedule, personal appointment, or required follow-up. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only when initial or late-unmet remains current. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The page can leave the center

Proposed diegetic text: “End note: Lesson ends; page remains face up.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the initial or unmet state, the page stays face-up through each lesson. A proposed closing lets the class finish and leaves the sheet in its place. No one is compelled to ask, and no one is forced to be the witness who names it. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No schedule, personal appointment, or required follow-up. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when initial or late-unmet remains current. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 067 — The page can leave the center

In the initial or unmet state, the page stays face-up through each lesson. A proposed closing lets the class finish and leaves the sheet in its place. No one is compelled to ask, and no one is forced to be the witness who names it. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “You can go now. The page will still be here when I teach again.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use only when initial or late-unmet remains current.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No schedule, personal appointment, or required follow-up. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The page can leave the center

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the initial or unmet state, the page stays face-up through each lesson. A proposed closing lets the class finish and leaves the sheet in its place. No one is compelled to ask, and no one is forced to be the witness who names it. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use only when initial or late-unmet remains current. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No schedule, personal appointment, or required follow-up.

Mirael Tesk may say: “You can go now. The page will still be here when I teach again.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: End note: Lesson ends; page remains face up. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — Forgiveness does not restore trust

In `late_disputed`, the archive forgives the forger and distrusts Mirael. The proposed return keeps those judgments separate: forgiveness does not make the alteration disappear, and distrust does not revoke the lesson she continues to teach. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “Those are the words the ruling chose. You do not have to pretend they add up to one feeling.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add another verdict or reverse the existing judgment. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only in the disputed late state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — Forgiveness does not restore trust

Proposed diegetic text: “Ruling margin: Forger forgiven; teacher distrusted; alteration acknowledged.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_disputed`, the archive forgives the forger and distrusts Mirael. The proposed return keeps those judgments separate: forgiveness does not make the alteration disappear, and distrust does not revoke the lesson she continues to teach. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add another verdict or reverse the existing judgment. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the disputed late state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 071 — Forgiveness does not restore trust

In `late_disputed`, the archive forgives the forger and distrusts Mirael. The proposed return keeps those judgments separate: forgiveness does not make the alteration disappear, and distrust does not revoke the lesson she continues to teach. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “Those are the words the ruling chose. You do not have to pretend they add up to one feeling.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Use only in the disputed late state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add another verdict or reverse the existing judgment. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — Forgiveness does not restore trust

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_disputed`, the archive forgives the forger and distrusts Mirael. The proposed return keeps those judgments separate: forgiveness does not make the alteration disappear, and distrust does not revoke the lesson she continues to teach. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Use only in the disputed late state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add another verdict or reverse the existing judgment.

Mirael Tesk may say: “Those are the words the ruling chose. You do not have to pretend they add up to one feeling.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Ruling margin: Forger forgiven; teacher distrusted; alteration acknowledged. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 073 — The blank space keeps its boundary

A student points to the blank space beside the altered line and asks what could be written there now. Mirael leaves it blank. The page can receive a dated, signed note in the keeper branch, but no proposed line fills in the protected person’s identity. Begin with the work already under way, before the visitor is asked to decide what it means. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Mirael Tesk: “I can write what happened to the record. I will not write the person into it.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No identity, replacement wording, or newly authored correction. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Distinguish the keeper’s note from completing the missing content. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 074 — The blank space keeps its boundary

Proposed diegetic text: “Page margin: Context may be acknowledged; protected name remains absent.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A student points to the blank space beside the altered line and asks what could be written there now. Mirael leaves it blank. The page can receive a dated, signed note in the keeper branch, but no proposed line fills in the protected person’s identity. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No identity, replacement wording, or newly authored correction. Do not let the form claim authority that its keeper has not been given.

Later reading: Distinguish the keeper’s note from completing the missing content. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 075 — The blank space keeps its boundary

A student points to the blank space beside the altered line and asks what could be written there now. Mirael leaves it blank. The page can receive a dated, signed note in the keeper branch, but no proposed line fills in the protected person’s identity. Let Mirael Tesk speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mirael Tesk: “I can write what happened to the record. I will not write the person into it.”
Other voice: “A student answers, “I can see the page is different. I cannot tell you who needed it to be.””
Mirael Tesk: “Distinguish the keeper’s note from completing the missing content.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No identity, replacement wording, or newly authored correction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 076 — The blank space keeps its boundary

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A student points to the blank space beside the altered line and asks what could be written there now. Mirael leaves it blank. The page can receive a dated, signed note in the keeper branch, but no proposed line fills in the protected person’s identity. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Mirael is precise and polite even when she dares a student to ask. She can read provenance aloud without turning the lesson into a trick. Her instructions make doubt a discipline, not a license to distrust everyone. She does not defend the alteration or name the person it protected.

Observable return: Distinguish the keeper’s note from completing the missing content. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No identity, replacement wording, or newly authored correction.

Mirael Tesk may say: “I can write what happened to the record. I will not write the person into it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Page margin: Context may be acknowledged; protected name remains absent. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- The lines are exact. That page is not. Both things can be true at once.
- We can read who carried this copy to the desk. We cannot invent who wrote what is missing.
- Someone needed the page to read as it did. I will not tell you who.
- Ask me or do not ask me. The lesson is the same.
- You already know what each choice asks. I will not disguise either one.
- You learned to copy it accurately. You also learned not to call accuracy the whole truth.
- The desk is open. It does not mean every drawer is.
- The note says what I can stand behind. It does not say who needed the silence.
- If they use it, they have to learn the doubt with the copy.
- A name can travel farther than the practice. Make sure the practice arrives too.
- You asked the archive to rule on the page. You did not ask me to name the person.
- I wrote what I thought the ruling would do. I did not write that it would feel good.
- We cleared the page away. We did not clear away what it protected.
- You can carry the words. You may not carry the whole room they were written in.
- We are teaching what is in the room. We are not going beneath the fallen shelves.
- I can read it aloud. I cannot make its history disappear.
- You can go now. The page will still be here when I teach again.
- Those are the words the ruling chose. You do not have to pretend they add up to one feeling.
- I can write what happened to the record. I will not write the person into it.

## 13. Continuity and editorial review

Check `npc_mirael_tesk`, `enc_arc_mirael_01_page`, `quest_arc_mirael_01_page`, and both corresponding state chains. Keep the page in a different hand, preserve that it protected someone two winters gone, and do not name either the protected person or the forger. Preserve the late disputed outcome and the late keeper’s reference to the Marek Voln school of honesty. Confirm current content before placement. Keep the first-visit encounter recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposed only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the existing character and encounter content, each callback matches the authored choice, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
