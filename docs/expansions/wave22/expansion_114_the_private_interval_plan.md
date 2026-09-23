# EXPANSION 114 — The Private Interval

## Mira Vos, a generator with stated hours, and the missed maintenance interval she keeps in her own hand.

### Wave 22: What the Account Cannot Hold

## 1. Expansion thesis

Keep Mira’s machine ordinary and her boundary exact. The generator runs to stated hours; a private log preserves a missed maintenance interval the official account does not. The player can bring the named parts and never mention the intake shed, or ask about the shed once. Neither path authorizes a second question about an accident the source does not explain. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character story or settlement dialogue surface. It adds no gameplay feature and does not claim that proposed text is already in the game.

## 2. Story in one sentence

A mechanic leaves the private log on the desk as a small opening, while the intake shed remains a door she does not ask the player to cross.

## 3. Verified local anchor and current story

`characters.json` defines `npc_mira_vos` as Generator Mechanic at `loc_pump_station_nine`; she wants copper wire and a brush set for a 12 kV unit, offers power at stated hours and boring maintenance, and will not skip an interval twice or work the intake shed alone. `npc_arcs.json` records initial, evolved, late-regular, late-withdrawn, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_mira_01_intervals`; `quests_npc_arcs.json` registers `quest_arc_mira_01_intervals`. The encounter keeps the official log and Mira’s private log open beside each other and the intake shed door shut. `mira_winder_parts` brings the brush set and copper and does not mention the shed; the private log later sits out and power becomes boringly reliable. `mira_ask_about_shed` asks once; she answers completely, then draws a line under the subject and most visits while the door stays shut. Do not invent the accident or the content of her answer. Names, locations, quest IDs, choice IDs, and state summaries above come from current local data. The registered encounter or character record proves the authored premise and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing condition.

## 4. Fixed canon and proposed prose

The private log records a missed maintenance interval in Mira’s handwriting. The character says the last failure was nobody’s fault, but her log keeps the interval someone needs to remember. The station’s generator runs to stated hours. Her boundary about the intake shed and not working alone is fixed. Later power schedules and off-peak rates belong only to `late_regular`; withdrawal remains a separate outcome. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to a character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Mira’s private log is not a confession the player has earned. It is a record she keeps because the machine needed someone to remember what it was saying. Her boundary about the shed deserves the same factual respect as a maintenance interval: it does not invite the player to test how many times she will repeat it. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–6 at the current encounter and choice. Beats 7–11 follow `mira_winder_parts` through `evolved` and `late_regular`; beats 12–15 follow `mira_ask_about_shed` into `late_withdrawn`. Final beats may fit the initial or late-unmet state without revealing the accident. The log on the desk remains Mira’s property in all cases. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character/settlement dialogue owner as the insertion point. Where the inspected data has no such route, keep the text as an editorial proposal and do not imply a new encounter has been approved.

## 8. Player agency and consequence

The choices are to bring the brush set and copper while never mentioning the shed, or ask about the shed once. Do not add a second chance to ask, a test of trust, or a route to the accident’s details. Show that respecting the first path is active consent to keep the subject closed, not failure to discover something. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before a choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep the machine non-operational in the prose. Do not describe opening, repairing, testing, or entering the intake shed. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, electrical, navigation, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The generator turns over clean

At Pump Station Nine the generator turns over clean. Mira’s official log and private log are open to two different pages on the desk, while the intake shed door stays shut. The visitor arrives while the machine is already doing its stated work. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “It is running. That is what the public page needs to say today.”
Other voice: “It turned over without the usual pause. I still checked the board.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No output amount, voltage reading, or operating instruction. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the two-page setup and closed door recognizable. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The generator turns over clean

Proposed diegetic text: “Station note: Generator operating to stated hours; both logs on the desk.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At Pump Station Nine the generator turns over clean. Mira’s official log and private log are open to two different pages on the desk, while the intake shed door stays shut. The visitor arrives while the machine is already doing its stated work. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No output amount, voltage reading, or operating instruction. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the two-page setup and closed door recognizable. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 003 — The generator turns over clean

At Pump Station Nine the generator turns over clean. Mira’s official log and private log are open to two different pages on the desk, while the intake shed door stays shut. The visitor arrives while the machine is already doing its stated work. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “It is running. That is what the public page needs to say today.”
Other voice: “It turned over without the usual pause. I still checked the board.”
Mira Vos: “Keep the two-page setup and closed door recognizable.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No output amount, voltage reading, or operating instruction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The generator turns over clean

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. At Pump Station Nine the generator turns over clean. Mira’s official log and private log are open to two different pages on the desk, while the intake shed door stays shut. The visitor arrives while the machine is already doing its stated work. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Keep the two-page setup and closed door recognizable. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No output amount, voltage reading, or operating instruction.

Mira Vos may say: “It is running. That is what the public page needs to say today.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Station note: Generator operating to stated hours; both logs on the desk. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — A missed interval remains in her hand

The private log records a maintenance interval that was missed. The prose may let the handwriting be visible without copying a date, describing a fault, or deciding who missed it. Mira keeps the line because the machine needed somebody to remember. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “Somebody has to remember what the machine was trying to say.”
Other voice: “I saw her looking at the same minute on both pages.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No date, cause, responsible person, or log contents beyond the source. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Do not turn the missed interval into a new fault diagnosis. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — A missed interval remains in her hand

Proposed diegetic text: “Private margin: Missed maintenance interval retained in Mira’s handwriting.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The private log records a maintenance interval that was missed. The prose may let the handwriting be visible without copying a date, describing a fault, or deciding who missed it. Mira keeps the line because the machine needed somebody to remember. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No date, cause, responsible person, or log contents beyond the source. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not turn the missed interval into a new fault diagnosis. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 007 — A missed interval remains in her hand

The private log records a maintenance interval that was missed. The prose may let the handwriting be visible without copying a date, describing a fault, or deciding who missed it. Mira keeps the line because the machine needed somebody to remember. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “Somebody has to remember what the machine was trying to say.”
Other voice: “I saw her looking at the same minute on both pages.”
Mira Vos: “Do not turn the missed interval into a new fault diagnosis.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No date, cause, responsible person, or log contents beyond the source. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — A missed interval remains in her hand

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The private log records a maintenance interval that was missed. The prose may let the handwriting be visible without copying a date, describing a fault, or deciding who missed it. Mira keeps the line because the machine needed somebody to remember. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Do not turn the missed interval into a new fault diagnosis. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No date, cause, responsible person, or log contents beyond the source.

Mira Vos may say: “Somebody has to remember what the machine was trying to say.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Private margin: Missed maintenance interval retained in Mira’s handwriting. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The official page is not called a lie

Mira keeps the story that the last failure was nobody’s fault and also keeps a private record of the missed interval. The scene does not label one page honest and the other false; the distinction is in what each page carries. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “The two pages answer different questions. I did not ask you to pick one to burn.”
Other voice: “The public number is there. I do not know why you need the private one.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not accuse Mira or an unnamed worker of lying. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the pages distinct across all states. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The official page is not called a lie

Proposed diegetic text: “Editorial note: Official account and private interval record remain separate.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mira keeps the story that the last failure was nobody’s fault and also keeps a private record of the missed interval. The scene does not label one page honest and the other false; the distinction is in what each page carries. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not accuse Mira or an unnamed worker of lying. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the pages distinct across all states. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 011 — The official page is not called a lie

Mira keeps the story that the last failure was nobody’s fault and also keeps a private record of the missed interval. The scene does not label one page honest and the other false; the distinction is in what each page carries. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “The two pages answer different questions. I did not ask you to pick one to burn.”
Other voice: “The public number is there. I do not know why you need the private one.”
Mira Vos: “Keep the pages distinct across all states.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not accuse Mira or an unnamed worker of lying. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The official page is not called a lie

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. Mira keeps the story that the last failure was nobody’s fault and also keeps a private record of the missed interval. The scene does not label one page honest and the other false; the distinction is in what each page carries. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Keep the pages distinct across all states. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not accuse Mira or an unnamed worker of lying.

Mira Vos may say: “The two pages answer different questions. I did not ask you to pick one to burn.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial note: Official account and private interval record remain separate. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — The intake shed stays shut

The intake shed door is shut and stays shut. Mira will not work there alone and has not asked the player to enter. The draft keeps the door in the background rather than making its handle a prompt. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “That door stays shut. We can keep talking here.”
Other voice: “She told me the intake shed was closed. We kept the work outside.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No access route, shed contents, or safety procedure. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Preserve the same boundary in every branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — The intake shed stays shut

Proposed diegetic text: “Access note: Intake shed door remains shut.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The intake shed door is shut and stays shut. Mira will not work there alone and has not asked the player to enter. The draft keeps the door in the background rather than making its handle a prompt. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No access route, shed contents, or safety procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the same boundary in every branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 015 — The intake shed stays shut

The intake shed door is shut and stays shut. Mira will not work there alone and has not asked the player to enter. The draft keeps the door in the background rather than making its handle a prompt. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “That door stays shut. We can keep talking here.”
Other voice: “She told me the intake shed was closed. We kept the work outside.”
Mira Vos: “The shed door stays closed. I will tell you what I can from here.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No access route, shed contents, or safety procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — The intake shed stays shut

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The intake shed door is shut and stays shut. Mira will not work there alone and has not asked the player to enter. The draft keeps the door in the background rather than making its handle a prompt. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Preserve the same boundary in every branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No access route, shed contents, or safety procedure.

Mira Vos may say: “That door stays shut. We can keep talking here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Access note: Intake shed door remains shut. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — Parts are easier to ask for than witnesses

The quest says parts are easier to ask for than witnesses. Mira names the brush set and copper, and the player can respond to that request without the prose inventing a witness or an incident report. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I asked for parts. Do not bring me a person to stand behind me.”
Other voice: “She asked for the part by name; she did not ask me to explain the missed hour.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new resource quantity or witness character. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use before the existing choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — Parts are easier to ask for than witnesses

Proposed diegetic text: “Supply request: Brush set and copper named; witness not requested by this choice.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The quest says parts are easier to ask for than witnesses. Mira names the brush set and copper, and the player can respond to that request without the prose inventing a witness or an incident report. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new resource quantity or witness character. Do not let the form claim authority that its keeper has not been given.

Later reading: Use before the existing choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 019 — Parts are easier to ask for than witnesses

The quest says parts are easier to ask for than witnesses. Mira names the brush set and copper, and the player can respond to that request without the prose inventing a witness or an incident report. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I asked for parts. Do not bring me a person to stand behind me.”
Other voice: “She asked for the part by name; she did not ask me to explain the missed hour.”
Mira Vos: “Ask me once if you need to. I will answer once.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new resource quantity or witness character. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — Parts are easier to ask for than witnesses

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The quest says parts are easier to ask for than witnesses. Mira names the brush set and copper, and the player can respond to that request without the prose inventing a witness or an incident report. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use before the existing choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new resource quantity or witness character.

Mira Vos may say: “I asked for parts. Do not bring me a person to stand behind me.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Supply request: Brush set and copper named; witness not requested by this choice. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — Bring the parts and leave the door alone

If `mira_winder_parts` is chosen, the player brings the brush set and copper and does not mention the shed. The passage shows the parts placed where Mira asked, without describing installation or electrical technique. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “You brought what I named. That is enough for this visit.”
Other voice: “I left it where she said. That is all I was asked to do.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No repair steps or shed access. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on the parts branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — Bring the parts and leave the door alone

Proposed diegetic text: “Bench note: Brush set and copper delivered; shed not discussed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `mira_winder_parts` is chosen, the player brings the brush set and copper and does not mention the shed. The passage shows the parts placed where Mira asked, without describing installation or electrical technique. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No repair steps or shed access. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on the parts branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 023 — Bring the parts and leave the door alone

If `mira_winder_parts` is chosen, the player brings the brush set and copper and does not mention the shed. The passage shows the parts placed where Mira asked, without describing installation or electrical technique. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “You brought what I named. That is enough for this visit.”
Other voice: “I left it where she said. That is all I was asked to do.”
Mira Vos: “You brought the parts. I have no reason to open the shed.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No repair steps or shed access. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — Bring the parts and leave the door alone

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. If `mira_winder_parts` is chosen, the player brings the brush set and copper and does not mention the shed. The passage shows the parts placed where Mira asked, without describing installation or electrical technique. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Condition on the parts branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No repair steps or shed access.

Mira Vos may say: “You brought what I named. That is enough for this visit.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Bench note: Brush set and copper delivered; shed not discussed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — Intervals kept like vows

The parts path says the intervals are kept like vows. The phrase belongs to Mira’s exacting work, not a new mechanic or promise that the machine can never fail. The private log sits out on the desk now, which for her is a door left open. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I left the page where you can see it. I did not leave every page.”
Other voice: “The board gets the shift. Her notebook stays in her coat.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No perfect reliability guarantee beyond the authored text. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only after `mira_winder_parts`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — Intervals kept like vows

Proposed diegetic text: “Return note: Maintenance intervals kept; private log left out by Mira.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The parts path says the intervals are kept like vows. The phrase belongs to Mira’s exacting work, not a new mechanic or promise that the machine can never fail. The private log sits out on the desk now, which for her is a door left open. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No perfect reliability guarantee beyond the authored text. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after `mira_winder_parts`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 027 — Intervals kept like vows

The parts path says the intervals are kept like vows. The phrase belongs to Mira’s exacting work, not a new mechanic or promise that the machine can never fail. The private log sits out on the desk now, which for her is a door left open. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I left the page where you can see it. I did not leave every page.”
Other voice: “The board gets the shift. Her notebook stays in her coat.”
Mira Vos: “You brought the parts I asked for. We can leave the door shut.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No perfect reliability guarantee beyond the authored text. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — Intervals kept like vows

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The parts path says the intervals are kept like vows. The phrase belongs to Mira’s exacting work, not a new mechanic or promise that the machine can never fail. The private log sits out on the desk now, which for her is a door left open. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only after `mira_winder_parts`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No perfect reliability guarantee beyond the authored text.

Mira Vos may say: “I left the page where you can see it. I did not leave every page.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Return note: Maintenance intervals kept; private log left out by Mira. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — Power becomes boringly reliable

The existing outcome calls the station’s power boringly, perfectly reliable. A later scene can make the absence of drama visible: no special alarm, just the stated hours proceeding as posted. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “If you stop noticing the generator, it is doing what I asked of it.”
Other voice: “If she says it is running, I write running.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add new output, schedule, or maintenance task. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only after the parts choice and matching state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — Power becomes boringly reliable

Proposed diegetic text: “Service note: Power described as boringly, perfectly reliable under current outcome.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The existing outcome calls the station’s power boringly, perfectly reliable. A later scene can make the absence of drama visible: no special alarm, just the stated hours proceeding as posted. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add new output, schedule, or maintenance task. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the parts choice and matching state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 031 — Power becomes boringly reliable

The existing outcome calls the station’s power boringly, perfectly reliable. A later scene can make the absence of drama visible: no special alarm, just the stated hours proceeding as posted. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “If you stop noticing the generator, it is doing what I asked of it.”
Other voice: “If she says it is running, I write running.”
Mira Vos: “It turns again. That is all I wanted to tell you about the machine.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add new output, schedule, or maintenance task. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — Power becomes boringly reliable

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The existing outcome calls the station’s power boringly, perfectly reliable. A later scene can make the absence of drama visible: no special alarm, just the stated hours proceeding as posted. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only after the parts choice and matching state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add new output, schedule, or maintenance task.

Mira Vos may say: “If you stop noticing the generator, it is doing what I asked of it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Service note: Power described as boringly, perfectly reliable under current outcome. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The log remains hers when visible

Mira leaving the private log out on the desk is a gesture she chooses. The player can read the visible page in the scene but receives no right to copy, remove, or publish it. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I left it here. That does not make it yours.”
Other voice: “She turned the page over when the door opened. I did not ask.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No item pickup or data-sharing state. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the distinction between visibility and ownership. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The log remains hers when visible

Proposed diegetic text: “Desk note: Private log visible by Mira’s choice; custody remains hers.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mira leaving the private log out on the desk is a gesture she chooses. The player can read the visible page in the scene but receives no right to copy, remove, or publish it. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No item pickup or data-sharing state. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the distinction between visibility and ownership. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 035 — The log remains hers when visible

Mira leaving the private log out on the desk is a gesture she chooses. The player can read the visible page in the scene but receives no right to copy, remove, or publish it. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I left it here. That does not make it yours.”
Other voice: “She turned the page over when the door opened. I did not ask.”
Mira Vos: “Keep the distinction between visibility and ownership.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No item pickup or data-sharing state. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The log remains hers when visible

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. Mira leaving the private log out on the desk is a gesture she chooses. The player can read the visible page in the scene but receives no right to copy, remove, or publish it. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Keep the distinction between visibility and ownership. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No item pickup or data-sharing state.

Mira Vos may say: “I left it here. That does not make it yours.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Desk note: Private log visible by Mira’s choice; custody remains hers. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — Off-peak rates are unrounded

In `late_regular`, the shelter buys off-peak load at rates set in Mira’s unrounded hand. The prose can show a bill with the rate left exactly as authored elsewhere; it must not invent a price or a settlement-wide schedule. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “The number is the number. I will not round it to make the sentence easier.”
Other voice: “That hour is off the public board because it is not a shift.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No cost, power trade mechanic, or additional customer. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on `late_regular`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — Off-peak rates are unrounded

Proposed diegetic text: “Late account: Off-peak load rate in Mira’s hand; numeric value not added here.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_regular`, the shelter buys off-peak load at rates set in Mira’s unrounded hand. The prose can show a bill with the rate left exactly as authored elsewhere; it must not invent a price or a settlement-wide schedule. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No cost, power trade mechanic, or additional customer. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `late_regular`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 039 — Off-peak rates are unrounded

In `late_regular`, the shelter buys off-peak load at rates set in Mira’s unrounded hand. The prose can show a bill with the rate left exactly as authored elsewhere; it must not invent a price or a settlement-wide schedule. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “The number is the number. I will not round it to make the sentence easier.”
Other voice: “That hour is off the public board because it is not a shift.”
Mira Vos: “It runs on the hours we agreed. Do not ask what else I kept.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No cost, power trade mechanic, or additional customer. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — Off-peak rates are unrounded

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. In `late_regular`, the shelter buys off-peak load at rates set in Mira’s unrounded hand. The prose can show a bill with the rate left exactly as authored elsewhere; it must not invent a price or a settlement-wide schedule. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Condition on `late_regular`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No cost, power trade mechanic, or additional customer.

Mira Vos may say: “The number is the number. I will not round it to make the sentence easier.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late account: Off-peak load rate in Mira’s hand; numeric value not added here. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — A second reader is chosen

The late regular state says the private log has a second reader she chose. The scene keeps that person unnamed unless current content names them. The choice belongs to Mira and is not inferred from player standing. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I chose who reads it. That is the whole line.”
Other voice: “She lets me read the number, not the rest of the sentence.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not assign the second reader or broaden access. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only under the late-regular state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — A second reader is chosen

Proposed diegetic text: “Log access note: Second reader chosen by Mira; identity not supplied here.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late regular state says the private log has a second reader she chose. The scene keeps that person unnamed unless current content names them. The choice belongs to Mira and is not inferred from player standing. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not assign the second reader or broaden access. Do not let the form claim authority that its keeper has not been given.

Later reading: Only under the late-regular state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 043 — A second reader is chosen

The late regular state says the private log has a second reader she chose. The scene keeps that person unnamed unless current content names them. The choice belongs to Mira and is not inferred from player standing. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I chose who reads it. That is the whole line.”
Other voice: “She lets me read the number, not the rest of the sentence.”
Mira Vos: “The board says when I work. That is enough for today.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not assign the second reader or broaden access. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — A second reader is chosen

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The late regular state says the private log has a second reader she chose. The scene keeps that person unnamed unless current content names them. The choice belongs to Mira and is not inferred from player standing. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Only under the late-regular state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not assign the second reader or broaden access.

Mira Vos may say: “I chose who reads it. That is the whole line.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Log access note: Second reader chosen by Mira; identity not supplied here. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The shed question is asked once

After `mira_ask_about_shed`, she answers once, completely, without stopping work. The actual answer is not quoted because the source does not state its words. The scene keeps the response complete without turning it into a teaser. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I answered you. Do not make me answer again because you disliked the first one.”
Other voice: “We can ask about the shed from here. There is no need to go to the door.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No accident details or second question. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only on the ask-about-shed branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The shed question is asked once

Proposed diegetic text: “Visit note: Question asked once; answer given; wording not supplied by this plan.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `mira_ask_about_shed`, she answers once, completely, without stopping work. The actual answer is not quoted because the source does not state its words. The scene keeps the response complete without turning it into a teaser. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No accident details or second question. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only on the ask-about-shed branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 047 — The shed question is asked once

After `mira_ask_about_shed`, she answers once, completely, without stopping work. The actual answer is not quoted because the source does not state its words. The scene keeps the response complete without turning it into a teaser. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I answered you. Do not make me answer again because you disliked the first one.”
Other voice: “We can ask about the shed from here. There is no need to go to the door.”
Mira Vos: “We can talk about the shed from here. Do not ask me to open it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No accident details or second question. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The shed question is asked once

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. After `mira_ask_about_shed`, she answers once, completely, without stopping work. The actual answer is not quoted because the source does not state its words. The scene keeps the response complete without turning it into a teaser. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only on the ask-about-shed branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No accident details or second question.

Mira Vos may say: “I answered you. Do not make me answer again because you disliked the first one.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Visit note: Question asked once; answer given; wording not supplied by this plan. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — The line under the subject

After the answer, Mira draws a line under the subject and under most of the visits. The generator still runs and the door stays shut. A return scene can show fewer words in the margin without making withdrawal a puzzle to solve. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “I drew the line where I meant it. That is not an invitation to find another way around.”
Other voice: “I would rather read a short log than write a long apology.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new trust value or secret invitation. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on `late_withdrawn`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — The line under the subject

Proposed diegetic text: “Private note: Subject closed; visits reduced under the current late outcome.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the answer, Mira draws a line under the subject and under most of the visits. The generator still runs and the door stays shut. A return scene can show fewer words in the margin without making withdrawal a puzzle to solve. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new trust value or secret invitation. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `late_withdrawn`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 051 — The line under the subject

After the answer, Mira draws a line under the subject and under most of the visits. The generator still runs and the door stays shut. A return scene can show fewer words in the margin without making withdrawal a puzzle to solve. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “I drew the line where I meant it. That is not an invitation to find another way around.”
Other voice: “I would rather read a short log than write a long apology.”
Mira Vos: “I took my hours off the board. Let the space remain empty.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new trust value or secret invitation. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — The line under the subject

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. After the answer, Mira draws a line under the subject and under most of the visits. The generator still runs and the door stays shut. A return scene can show fewer words in the margin without making withdrawal a puzzle to solve. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Condition on `late_withdrawn`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new trust value or secret invitation.

Mira Vos may say: “I drew the line where I meant it. That is not an invitation to find another way around.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Private note: Subject closed; visits reduced under the current late outcome. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — Most visits are not all visits

The withdrawn state says most visits end under a line; it does not say Mira has vanished or stopped maintaining the generator. A return may show work at the station while leaving conversation brief. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “The machine is still here. I am still working. That is all this visit needs.”
Other voice: “I know which parts she wants. I do not know what she keeps in that room.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not make her disappearance or isolation a new outcome. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only with `late_withdrawn`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — Most visits are not all visits

Proposed diegetic text: “Station note: Generator runs; visit pattern reduced under the current state.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The withdrawn state says most visits end under a line; it does not say Mira has vanished or stopped maintaining the generator. A return may show work at the station while leaving conversation brief. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not make her disappearance or isolation a new outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only with `late_withdrawn`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 055 — Most visits are not all visits

The withdrawn state says most visits end under a line; it does not say Mira has vanished or stopped maintaining the generator. A return may show work at the station while leaving conversation brief. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “The machine is still here. I am still working. That is all this visit needs.”
Other voice: “I know which parts she wants. I do not know what she keeps in that room.”
Mira Vos: “The board has the hours. I do not owe it the rest.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not make her disappearance or isolation a new outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — Most visits are not all visits

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The withdrawn state says most visits end under a line; it does not say Mira has vanished or stopped maintaining the generator. A return may show work at the station while leaving conversation brief. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only with `late_withdrawn`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not make her disappearance or isolation a new outcome.

Mira Vos may say: “The machine is still here. I am still working. That is all this visit needs.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Station note: Generator runs; visit pattern reduced under the current state. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — Stated hours remain public

Mira offers power at stated hours and the late regular schedule is published and kept. A copy can be read without adding operational instructions or an hour not provided by source content. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “Read the hours from the board. I will not improve them by making them up.”
Other voice: “The schedule hangs where people can see it; her notes do not.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new time, outage, or grid schedule. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only when current content exposes the existing schedule. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — Stated hours remain public

Proposed diegetic text: “Posted line: Power available at the stated schedule; no time inserted in draft.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Mira offers power at stated hours and the late regular schedule is published and kept. A copy can be read without adding operational instructions or an hour not provided by source content. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new time, outage, or grid schedule. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when current content exposes the existing schedule. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 059 — Stated hours remain public

Mira offers power at stated hours and the late regular schedule is published and kept. A copy can be read without adding operational instructions or an hour not provided by source content. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “Read the hours from the board. I will not improve them by making them up.”
Other voice: “The schedule hangs where people can see it; her notes do not.”
Mira Vos: “Put the shift on the board. Leave my notes where I left them.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new time, outage, or grid schedule. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — Stated hours remain public

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. Mira offers power at stated hours and the late regular schedule is published and kept. A copy can be read without adding operational instructions or an hour not provided by source content. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only when current content exposes the existing schedule. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new time, outage, or grid schedule.

Mira Vos may say: “Read the hours from the board. I will not improve them by making them up.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Posted line: Power available at the stated schedule; no time inserted in draft. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — No one stands behind her at the shed

Her character record says she wants nobody standing behind her at the shed. A proposed beat can make that wish a plain piece of blocking: anyone speaking to her stays where she can see them, without explaining the accident or turning it into a safety procedure. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “Stay where I can see you. We can speak from here.”
Other voice: “Nobody had to stand behind her. She had the list in her hand.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No operational safety advice or shed entry. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only if the current staging can honor her stated boundary. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — No one stands behind her at the shed

Proposed diegetic text: “Blocking note: Visitor remains in Mira’s view; no one enters the shed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Her character record says she wants nobody standing behind her at the shed. A proposed beat can make that wish a plain piece of blocking: anyone speaking to her stays where she can see them, without explaining the accident or turning it into a safety procedure. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No operational safety advice or shed entry. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only if the current staging can honor her stated boundary. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 063 — No one stands behind her at the shed

Her character record says she wants nobody standing behind her at the shed. A proposed beat can make that wish a plain piece of blocking: anyone speaking to her stays where she can see them, without explaining the accident or turning it into a safety procedure. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “Stay where I can see you. We can speak from here.”
Other voice: “Nobody had to stand behind her. She had the list in her hand.”
Mira Vos: “Thank you for waiting here. I said what I needed to say.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No operational safety advice or shed entry. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — No one stands behind her at the shed

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. Her character record says she wants nobody standing behind her at the shed. A proposed beat can make that wish a plain piece of blocking: anyone speaking to her stays where she can see them, without explaining the accident or turning it into a safety procedure. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only if the current staging can honor her stated boundary. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No operational safety advice or shed entry.

Mira Vos may say: “Stay where I can see you. We can speak from here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Blocking note: Visitor remains in Mira’s view; no one enters the shed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The machine is easier than the job

The late-unmet state says the machine is the easy part of the job. A quiet closing can leave the public log accurate and the private log at the edge of the desk. No resolution is attached to the space between them. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “The machine is the easy part. I have not said the rest is yours.”
Other voice: “This job is easier when the motor stays boring.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No resolution of the accident or additional work. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only while late-unmet is active. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The machine is easier than the job

Proposed diegetic text: “Closing note: Stated hours continue; private record remains in its current custody.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late-unmet state says the machine is the easy part of the job. A quiet closing can leave the public log accurate and the private log at the edge of the desk. No resolution is attached to the space between them. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No resolution of the accident or additional work. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only while late-unmet is active. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 067 — The machine is easier than the job

The late-unmet state says the machine is the easy part of the job. A quiet closing can leave the public log accurate and the private log at the edge of the desk. No resolution is attached to the space between them. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “The machine is the easy part. I have not said the rest is yours.”
Other voice: “This job is easier when the motor stays boring.”
Mira Vos: “I have not decided what to tell you. Let the silence keep its place.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No resolution of the accident or additional work. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The machine is easier than the job

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The late-unmet state says the machine is the easy part of the job. A quiet closing can leave the public log accurate and the private log at the edge of the desk. No resolution is attached to the space between them. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Use only while late-unmet is active. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No resolution of the accident or additional work.

Mira Vos may say: “The machine is the easy part. I have not said the rest is yours.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Closing note: Stated hours continue; private record remains in its current custody. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — A door can remain a door

The final scene refuses to make the shut intake shed stand for a hidden truth that must be discovered. It is a door Mira has closed. The player may leave the station with the schedule, the visible log, and the boundary intact. Begin with the work already under way, before the visitor is asked to decide what it means. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Mira Vos: “You have what I chose to show you. Let that be enough for today.”
Other voice: “She closed the door herself. That was the answer.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No key, clue, shed contents, or follow-up quest. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Close within the current Mira state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — A door can remain a door

Proposed diegetic text: “Exit note: Shed remains shut; visit ends at the public work area.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The final scene refuses to make the shut intake shed stand for a hidden truth that must be discovered. It is a door Mira has closed. The player may leave the station with the schedule, the visible log, and the boundary intact. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No key, clue, shed contents, or follow-up quest. Do not let the form claim authority that its keeper has not been given.

Later reading: Close within the current Mira state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 071 — A door can remain a door

The final scene refuses to make the shut intake shed stand for a hidden truth that must be discovered. It is a door Mira has closed. The player may leave the station with the schedule, the visible log, and the boundary intact. Let Mira Vos speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Mira Vos: “You have what I chose to show you. Let that be enough for today.”
Other voice: “She closed the door herself. That was the answer.”
Mira Vos: “I will tell you when there is something I want to say.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No key, clue, shed contents, or follow-up quest. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — A door can remain a door

On a later visit permitted by the exact existing Mira choice and the corresponding initial, evolved, late-regular, late-withdrawn, or late-unmet NPC state, the player may notice what the earlier choice left in view. The final scene refuses to make the shut intake shed stand for a hidden truth that must be discovered. It is a door Mira has closed. The player may leave the station with the schedule, the visible log, and the boundary intact. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Mira is precise about hours, intervals, and what she has decided to leave on the desk. She treats reliable power as boring, which is praise. She will answer a direct question once when the registered choice asks it, then stop returning to the subject. Do not make silence sound like a clue.

Observable return: Close within the current Mira state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No key, clue, shed contents, or follow-up quest.

Mira Vos may say: “You have what I chose to show you. Let that be enough for today.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Exit note: Shed remains shut; visit ends at the public work area. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- It is running. That is what the public page needs to say today.
- Somebody has to remember what the machine was trying to say.
- The two pages answer different questions. I did not ask you to pick one to burn.
- That door stays shut. We can keep talking here.
- I asked for parts. Do not bring me a person to stand behind me.
- You brought what I named. That is enough for this visit.
- I left the page where you can see it. I did not leave every page.
- If you stop noticing the generator, it is doing what I asked of it.
- I left it here. That does not make it yours.
- The number is the number. I will not round it to make the sentence easier.
- I chose who reads it. That is the whole line.
- I answered you. Do not make me answer again because you disliked the first one.
- I drew the line where I meant it. That is not an invitation to find another way around.
- The machine is still here. I am still working. That is all this visit needs.
- Read the hours from the board. I will not improve them by making them up.
- Stay where I can see you. We can speak from here.
- The machine is the easy part. I have not said the rest is yours.
- You have what I chose to show you. Let that be enough for today.

## 13. Continuity and editorial review

Verify `enc_arc_mira_01_intervals`, `quest_arc_mira_01_intervals`, and the state requirements. Preserve the official/private log distinction, named component wants, single shed question, parts branch, late regular schedule, and withdrawn boundary. Do not infer or write what caused the accident. Confirm current content before placement. Keep the first-visit premise recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposals only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the current character or settlement content, each callback matches an authored condition, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
