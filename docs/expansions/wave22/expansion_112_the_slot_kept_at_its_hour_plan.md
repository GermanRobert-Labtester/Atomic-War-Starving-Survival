# EXPANSION 112 — The Slot Kept at Its Hour

## Liva Kern, the relay log, and the difference between being heard and being answered.

### Wave 22: What the Account Cannot Hold

## 1. Expansion thesis

Write the relay as a disciplined human practice rather than a machine that solves distance. Liva offers scheduled slots and signal copies for cells that hold charge, and asks anyone who keys back to identify honestly. When the tower is mapped, the shelter learns first and must choose whether to warn her or sell the location. The prose can make that knowledge heavy without inventing what the band says. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character story or settlement dialogue surface. It adds no gameplay feature and does not claim that proposed text is already in the game.

## 2. Story in one sentence

A relay operator keeps the hour, the log, and the possibility that somebody on the other end is still counting.

## 3. Verified local anchor and current story

`characters.json` defines `npc_liva_kern` at `loc_radio_relay_mast`; she keeps the mast alive on scavenged cells, logs every contact, wants cells and someone who keys back, and will not broadcast unlogged. `npc_arcs.json` contains initial, evolved-threatened, evolved-stranger, recruited, late-intel-asset, late-broadcaster, late-sold, and deceased states. `narrative_encounters_npc_arcs.json` registers `enc_arc_liva_tower` and `enc_arc_liva_survey`; `quests_npc_arcs.json` registers `quest_arc_liva_01_tower`, `quest_arc_liva_02_seizure`, and `quest_arc_liva_03_signal`. The first encounter offers `liva_trade_cells` for a standing relay slot and signal copies, or `liva_send_it_properly` for a one-time identified slot. The survey choice is `liva_move_her_first`/`liva_warn` or `liva_sell_the_location`/`liva_bargain`. Warning moves her gear and the mast goes dark on schedule for the first time in its second life; sale gives the garrison the location, a finder’s fee, and a second reader on the log. The later broadcaster names moved-on camps at a fixed hour; the separate signal quest can send the shelter’s names. Keep camp names, shelter names, and individual identities distinct. Names, locations, quest IDs, choice IDs, and state summaries above come from current local data. The registered encounter or character record proves the authored premise and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing condition.

## 4. Fixed canon and proposed prose

Liva’s log is her sermon. She believes someone is still counting the stations that answer and operates as if being counted matters. The mast runs on scavenged cells; schedule, identification, and logging are central. A survey team’s fires are two valleys out and her map is better. The log is copied in two hands, one to send and one to lose. Preserve the warning and sale outcomes, the shelter’s knowledge, the deceased state’s unfinished log, and the possibility that the station on the other end may not answer. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to a character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Liva’s hope is practical enough to keep a shift on time and uncertain enough to make every reply matter. A schedule can help people plan; it can also make a name legible to whoever is listening. Her need for an honest call does not mean she owes the caller proof that someone is there. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–6 around `enc_arc_liva_tower` and `quest_arc_liva_01_tower`. Beats 7–13 depend on the standing-slot and survey choices: warning and sale stay separate. Beats 14–16 apply only when the corresponding broadcaster, signal, intel-asset, sold, or recruited state is current. The last two may support the initial or deceased state only as specified. Never print all outcomes in one visit. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character/settlement dialogue owner as the insertion point. Where the inspected data has no such route, keep the text as an editorial proposal and do not imply a new encounter has been approved.

## 8. Player agency and consequence

The current choices distinguish a standing slot paid with cells from a one-time identified message, then warning Liva before the survey team arrives from selling her location to the Garrison. A later choice can send the shelter’s names on her band. Make the identifying and naming costs plain; do not create an anonymous bypass or change what has already been sent. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before a choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep the band, schedule, and two-copy log as fictional story objects. No frequency, code, tracking instruction, or hidden location is added. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, electrical, navigation, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The mast hums on scavenged cells

At the relay mast, Liva checks the cell supply and opens a log already ruled for contacts. The hum is a working sound, not proof that another station is awake. She can hear the difference between a machine continuing and a person answering. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “The mast is live. That is what I can tell you. It is not the same as knowing who is there.”
Other voice: “The hour is on the board. The other end is still only a question.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not identify a distant station or add a new technical readout. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the distinction between transmission and response. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The mast hums on scavenged cells

Proposed diegetic text: “Shift header: Mast operating on scavenged cells; contact log open.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the relay mast, Liva checks the cell supply and opens a log already ruled for contacts. The hum is a working sound, not proof that another station is awake. She can hear the difference between a machine continuing and a person answering. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not identify a distant station or add a new technical readout. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the distinction between transmission and response. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 003 — The mast hums on scavenged cells

At the relay mast, Liva checks the cell supply and opens a log already ruled for contacts. The hum is a working sound, not proof that another station is awake. She can hear the difference between a machine continuing and a person answering. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “The mast is live. That is what I can tell you. It is not the same as knowing who is there.”
Other voice: “The hour is on the board. The other end is still only a question.”
Liva Kern: “Keep the distinction between transmission and response.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not identify a distant station or add a new technical readout. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The mast hums on scavenged cells

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. At the relay mast, Liva checks the cell supply and opens a log already ruled for contacts. The hum is a working sound, not proof that another station is awake. She can hear the difference between a machine continuing and a person answering. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Keep the distinction between transmission and response. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not identify a distant station or add a new technical readout.

Liva Kern may say: “The mast is live. That is what I can tell you. It is not the same as knowing who is there.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Shift header: Mast operating on scavenged cells; contact log open. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — Every contact has a line

Liva records each contact in a hand too neat for the times. The visitor sees the habit of logging before being asked to answer honestly. No line is created for a signal that the source does not say occurred. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “If it happened, it belongs in the log. If it did not, a blank is better than a story.”
Other voice: “I heard the call clearly. I did not hear a name I could write down.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not invent a caller, message, or time. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Any return copy must retain only contacts established by the current content. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — Every contact has a line

Proposed diegetic text: “Log margin: Contact entered in the operator’s hand.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Liva records each contact in a hand too neat for the times. The visitor sees the habit of logging before being asked to answer honestly. No line is created for a signal that the source does not say occurred. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent a caller, message, or time. Do not let the form claim authority that its keeper has not been given.

Later reading: Any return copy must retain only contacts established by the current content. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 007 — Every contact has a line

Liva records each contact in a hand too neat for the times. The visitor sees the habit of logging before being asked to answer honestly. No line is created for a signal that the source does not say occurred. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “If it happened, it belongs in the log. If it did not, a blank is better than a story.”
Other voice: “I heard the call clearly. I did not hear a name I could write down.”
Liva Kern: “If it comes back, I want the words exactly as we sent them.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent a caller, message, or time. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — Every contact has a line

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. Liva records each contact in a hand too neat for the times. The visitor sees the habit of logging before being asked to answer honestly. No line is created for a signal that the source does not say occurred. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Any return copy must retain only contacts established by the current content. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent a caller, message, or time.

Liva Kern may say: “If it happened, it belongs in the log. If it did not, a blank is better than a story.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Log margin: Contact entered in the operator’s hand. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — A call identifies itself

The tower encounter asks that whoever keys back identify honestly. A proposed line states that condition before the player uses the one-time slot. Liva does not promise that an identified message will receive a reply. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “You can say who is calling. You cannot make the other end answer.”
Other voice: “The page says where the signal came from. It does not say who sent it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No call sign, frequency, or listener identity is supplied. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Bind to the one-time `liva_send_it_properly` choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — A call identifies itself

Proposed diegetic text: “Slot reminder: Caller identifies honestly; response not guaranteed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The tower encounter asks that whoever keys back identify honestly. A proposed line states that condition before the player uses the one-time slot. Liva does not promise that an identified message will receive a reply. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No call sign, frequency, or listener identity is supplied. Do not let the form claim authority that its keeper has not been given.

Later reading: Bind to the one-time `liva_send_it_properly` choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 011 — A call identifies itself

The tower encounter asks that whoever keys back identify honestly. A proposed line states that condition before the player uses the one-time slot. Liva does not promise that an identified message will receive a reply. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “You can say who is calling. You cannot make the other end answer.”
Other voice: “The page says where the signal came from. It does not say who sent it.”
Liva Kern: “I can send the message once you put your mark on it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No call sign, frequency, or listener identity is supplied. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — A call identifies itself

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. The tower encounter asks that whoever keys back identify honestly. A proposed line states that condition before the player uses the one-time slot. Liva does not promise that an identified message will receive a reply. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Bind to the one-time `liva_send_it_properly` choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No call sign, frequency, or listener identity is supplied.

Liva Kern may say: “You can say who is calling. You cannot make the other end answer.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Slot reminder: Caller identifies honestly; response not guaranteed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — Cells for a standing slot

If the player chooses `liva_trade_cells`, cells are traded for a standing relay slot and signal copies. The scene can show Liva entering the shelter’s place in the schedule without saying that the cell trade purchased ownership of the mast. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “You bought a place in the hour. The hour is still mine to keep.”
Other voice: “I can take the standing hour. I cannot promise what will be waiting in it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No cell count, schedule time, or mast ownership transfer. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only after the standing-slot trade. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — Cells for a standing slot

Proposed diegetic text: “Schedule note: Standing slot and signal copies exchanged for cells.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the player chooses `liva_trade_cells`, cells are traded for a standing relay slot and signal copies. The scene can show Liva entering the shelter’s place in the schedule without saying that the cell trade purchased ownership of the mast. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No cell count, schedule time, or mast ownership transfer. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the standing-slot trade. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 015 — Cells for a standing slot

If the player chooses `liva_trade_cells`, cells are traded for a standing relay slot and signal copies. The scene can show Liva entering the shelter’s place in the schedule without saying that the cell trade purchased ownership of the mast. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “You bought a place in the hour. The hour is still mine to keep.”
Other voice: “I can take the standing hour. I cannot promise what will be waiting in it.”
Liva Kern: “You bought a place in the hour. The hour is still mine to keep.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No cell count, schedule time, or mast ownership transfer. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — Cells for a standing slot

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. If the player chooses `liva_trade_cells`, cells are traded for a standing relay slot and signal copies. The scene can show Liva entering the shelter’s place in the schedule without saying that the cell trade purchased ownership of the mast. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only after the standing-slot trade. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No cell count, schedule time, or mast ownership transfer.

Liva Kern may say: “You bought a place in the hour. The hour is still mine to keep.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Schedule note: Standing slot and signal copies exchanged for cells. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — One message, properly sent

If the player chooses `liva_send_it_properly`, the message goes out logged and timed, with the shelter’s call in a neat hand. The final sentence leaves open whether anyone is there to hear it. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “I wrote down what went out. I did not write down what nobody answered.”
Other voice: “You gave us the cells. We can keep the watch without pretending it answered.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No message contents or acknowledgement from the other end. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep separate from the standing slot and later broadcaster path. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — One message, properly sent

Proposed diegetic text: “Call entry: One-time slot; identified and timed; response unknown.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the player chooses `liva_send_it_properly`, the message goes out logged and timed, with the shelter’s call in a neat hand. The final sentence leaves open whether anyone is there to hear it. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No message contents or acknowledgement from the other end. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep separate from the standing slot and later broadcaster path. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 019 — One message, properly sent

If the player chooses `liva_send_it_properly`, the message goes out logged and timed, with the shelter’s call in a neat hand. The final sentence leaves open whether anyone is there to hear it. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “I wrote down what went out. I did not write down what nobody answered.”
Other voice: “You gave us the cells. We can keep the watch without pretending it answered.”
Liva Kern: “Keep separate from the standing slot and later broadcaster path.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No message contents or acknowledgement from the other end. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — One message, properly sent

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. If the player chooses `liva_send_it_properly`, the message goes out logged and timed, with the shelter’s call in a neat hand. The final sentence leaves open whether anyone is there to hear it. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Keep separate from the standing slot and later broadcaster path. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No message contents or acknowledgement from the other end.

Liva Kern may say: “I wrote down what went out. I did not write down what nobody answered.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Call entry: One-time slot; identified and timed; response unknown. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — The weather copies arrive first

The standing-slot outcome says weather copies arrive before the weather does. A later desk scene can show a folded copy arriving while the day remains unchanged. It must not become a forecast guarantee or an unauthored warning about a specific hazard. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “The paper arrived first. That is useful. It is not a promise about every sky.”
Other voice: “The weather copy arrived first. That is all it proved.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add forecast values or new weather mechanics. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only use under the current standing-slot state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — The weather copies arrive first

Proposed diegetic text: “Copy label: Signal and weather copy; received before the weather it describes.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The standing-slot outcome says weather copies arrive before the weather does. A later desk scene can show a folded copy arriving while the day remains unchanged. It must not become a forecast guarantee or an unauthored warning about a specific hazard. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add forecast values or new weather mechanics. Do not let the form claim authority that its keeper has not been given.

Later reading: Only use under the current standing-slot state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 023 — The weather copies arrive first

The standing-slot outcome says weather copies arrive before the weather does. A later desk scene can show a folded copy arriving while the day remains unchanged. It must not become a forecast guarantee or an unauthored warning about a specific hazard. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “The paper arrived first. That is useful. It is not a promise about every sky.”
Other voice: “The weather copy arrived first. That is all it proved.”
Liva Kern: “You paid for that hour. I will keep it clear.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add forecast values or new weather mechanics. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — The weather copies arrive first

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. The standing-slot outcome says weather copies arrive before the weather does. A later desk scene can show a folded copy arriving while the day remains unchanged. It must not become a forecast guarantee or an unauthored warning about a specific hazard. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Only use under the current standing-slot state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add forecast values or new weather mechanics.

Liva Kern may say: “The paper arrived first. That is useful. It is not a promise about every sky.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy label: Signal and weather copy; received before the weather it describes. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — The shelter answers on time

A returned log shows the shelter’s slot has never been missed. The passage centers the people who kept the watch, not a claim that the call has established a relationship with a known listener. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “We kept the hour. We still do not know who kept the other end.”
Other voice: “We heard the shelter answer. Nobody here knows who heard it beyond us.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No named responder or new radio contact. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only when the current slot state says it is standing. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — The shelter answers on time

Proposed diegetic text: “Schedule margin: Shelter slot answered on time; listener unnamed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A returned log shows the shelter’s slot has never been missed. The passage centers the people who kept the watch, not a claim that the call has established a relationship with a known listener. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No named responder or new radio contact. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when the current slot state says it is standing. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 027 — The shelter answers on time

A returned log shows the shelter’s slot has never been missed. The passage centers the people who kept the watch, not a claim that the call has established a relationship with a known listener. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “We kept the hour. We still do not know who kept the other end.”
Other voice: “We heard the shelter answer. Nobody here knows who heard it beyond us.”
Liva Kern: “The mast is live. That is what I can tell you; it is not the same as knowing who is there.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No named responder or new radio contact. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — The shelter answers on time

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. A returned log shows the shelter’s slot has never been missed. The passage centers the people who kept the watch, not a claim that the call has established a relationship with a known listener. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only when the current slot state says it is standing. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No named responder or new radio contact.

Liva Kern may say: “We kept the hour. We still do not know who kept the other end.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Schedule margin: Shelter slot answered on time; listener unnamed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — The survey fires are two valleys out

In `enc_arc_liva_survey`, the survey team’s fires are two valleys out and Liva’s map is better. The prose keeps that information at the level the encounter gives it, without adding a team name, route, or approach time. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “They are two valleys out. I have not told you what they intend.”
Other voice: “Those lights were two valleys out. I never heard a voice from them.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No route, force size, or exact arrival date. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the survey encounter. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — The survey fires are two valleys out

Proposed diegetic text: “Survey note: Fires two valleys out; Liva’s map is better; timing otherwise unstated.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `enc_arc_liva_survey`, the survey team’s fires are two valleys out and Liva’s map is better. The prose keeps that information at the level the encounter gives it, without adding a team name, route, or approach time. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No route, force size, or exact arrival date. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the survey encounter. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 031 — The survey fires are two valleys out

In `enc_arc_liva_survey`, the survey team’s fires are two valleys out and Liva’s map is better. The prose keeps that information at the level the encounter gives it, without adding a team name, route, or approach time. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “They are two valleys out. I have not told you what they intend.”
Other voice: “Those lights were two valleys out. I never heard a voice from them.”
Liva Kern: “Those lights are two valleys out. I have not told you what they intend.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No route, force size, or exact arrival date. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — The survey fires are two valleys out

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. In `enc_arc_liva_survey`, the survey team’s fires are two valleys out and Liva’s map is better. The prose keeps that information at the level the encounter gives it, without adding a team name, route, or approach time. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only in the survey encounter. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No route, force size, or exact arrival date.

Liva Kern may say: “They are two valleys out. I have not told you what they intend.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Survey note: Fires two valleys out; Liva’s map is better; timing otherwise unstated. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — Two copies, one to send

Watched by a faction that maps towers, Liva has begun copying the log in two hands: one to send and one to lose. A draft can show the two sheets separated by a ruler without suggesting how to locate the second copy. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “You know there are two. That is all I am choosing to tell you.”
Other voice: “Keep the sent copy and the received copy apart; they are not the same account.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not reveal a hiding place or add a copying method. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on the threatened operator state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — Two copies, one to send

Proposed diegetic text: “Copy mark: One log copy designated to send; one to lose.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Watched by a faction that maps towers, Liva has begun copying the log in two hands: one to send and one to lose. A draft can show the two sheets separated by a ruler without suggesting how to locate the second copy. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not reveal a hiding place or add a copying method. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on the threatened operator state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 035 — Two copies, one to send

Watched by a faction that maps towers, Liva has begun copying the log in two hands: one to send and one to lose. A draft can show the two sheets separated by a ruler without suggesting how to locate the second copy. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “You know there are two. That is all I am choosing to tell you.”
Other voice: “Keep the sent copy and the received copy apart; they are not the same account.”
Liva Kern: “I will not put anyone on that tower because a voice asked me to.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not reveal a hiding place or add a copying method. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — Two copies, one to send

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. Watched by a faction that maps towers, Liva has begun copying the log in two hands: one to send and one to lose. A draft can show the two sheets separated by a ruler without suggesting how to locate the second copy. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Condition on the threatened operator state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not reveal a hiding place or add a copying method.

Liva Kern may say: “You know there are two. That is all I am choosing to tell you.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy mark: One log copy designated to send; one to lose. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — The shelter knows first

The encounter says the shelter knows first and that what it does with the knowing is the entry that matters. The scene allows a pause between learning about the survey and selecting either existing choice. It does not invent another report channel. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “Do not write the answer for me while the page is still open.”
Other voice: “We got the warning before the rumor. I would like the next one to be as plain.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No added response choice or deadline. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the space before the registered warn/sell choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — The shelter knows first

Proposed diegetic text: “Margin: Shelter has learned of the survey; response not yet entered.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The encounter says the shelter knows first and that what it does with the knowing is the entry that matters. The scene allows a pause between learning about the survey and selecting either existing choice. It does not invent another report channel. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No added response choice or deadline. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the space before the registered warn/sell choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 039 — The shelter knows first

The encounter says the shelter knows first and that what it does with the knowing is the entry that matters. The scene allows a pause between learning about the survey and selecting either existing choice. It does not invent another report channel. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “Do not write the answer for me while the page is still open.”
Other voice: “We got the warning before the rumor. I would like the next one to be as plain.”
Liva Kern: “You can still tell me what you think. I have not packed yet.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No added response choice or deadline. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — The shelter knows first

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. The encounter says the shelter knows first and that what it does with the knowing is the entry that matters. The scene allows a pause between learning about the survey and selecting either existing choice. It does not invent another report channel. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Keep the space before the registered warn/sell choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No added response choice or deadline.

Liva Kern may say: “Do not write the answer for me while the page is still open.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin: Shelter has learned of the survey; response not yet entered. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The mast goes dark on schedule

After `liva_warn`, the mast goes dark on schedule for the first time in its second life while she moves her gear. The line does not describe how to move or hide a transmitter. It keeps the cost visible: a schedule is interrupted to protect the operator. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “The quiet is in the log too. I will write it down.”
Other voice: “The mast went quiet on time. The log can say that much.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No relocation route, concealment method, or new outage length. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only after the warning choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The mast goes dark on schedule

Proposed diegetic text: “Log entry: Mast dark on schedule for the first time in its second life.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `liva_warn`, the mast goes dark on schedule for the first time in its second life while she moves her gear. The line does not describe how to move or hide a transmitter. It keeps the cost visible: a schedule is interrupted to protect the operator. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No relocation route, concealment method, or new outage length. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only after the warning choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 043 — The mast goes dark on schedule

After `liva_warn`, the mast goes dark on schedule for the first time in its second life while she moves her gear. The line does not describe how to move or hide a transmitter. It keeps the cost visible: a schedule is interrupted to protect the operator. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “The quiet is in the log too. I will write it down.”
Other voice: “The mast went quiet on time. The log can say that much.”
Liva Kern: “Good. I would rather move a crate than lose its owner.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No relocation route, concealment method, or new outage length. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The mast goes dark on schedule

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. After `liva_warn`, the mast goes dark on schedule for the first time in its second life while she moves her gear. The line does not describe how to move or hide a transmitter. It keeps the cost visible: a schedule is interrupted to protect the operator. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only after the warning choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No relocation route, concealment method, or new outage length.

Liva Kern may say: “The quiet is in the log too. I will write it down.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Log entry: Mast dark on schedule for the first time in its second life. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — A new site under a flag

The warning outcome says Liva keys again from a new site under a flag that leaves her alone. The plan leaves the site unnamed and avoids turning the flag into a faction credential the player can claim. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “I can tell you that I am transmitting. I will not tell you where from.”
Other voice: “We saw a new light. Nobody here called it a settlement.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Never reveal a new site or route. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on the late-intel-asset state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — A new site under a flag

Proposed diegetic text: “Return note: New site under a protective flag; exact location omitted.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The warning outcome says Liva keys again from a new site under a flag that leaves her alone. The plan leaves the site unnamed and avoids turning the flag into a faction credential the player can claim. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Never reveal a new site or route. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on the late-intel-asset state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 047 — A new site under a flag

The warning outcome says Liva keys again from a new site under a flag that leaves her alone. The plan leaves the site unnamed and avoids turning the flag into a faction credential the player can claim. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “I can tell you that I am transmitting. I will not tell you where from.”
Other voice: “We saw a new light. Nobody here called it a settlement.”
Liva Kern: “That note has a place on it. Do not mistake it for a name.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Never reveal a new site or route. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — A new site under a flag

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. The warning outcome says Liva keys again from a new site under a flag that leaves her alone. The plan leaves the site unnamed and avoids turning the flag into a faction credential the player can claim. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Condition on the late-intel-asset state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Never reveal a new site or route.

Liva Kern may say: “I can tell you that I am transmitting. I will not tell you where from.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Return note: New site under a protective flag; exact location omitted. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — The schedules were never logged

In the intel-asset state, the shelter gets weather copies and patrol schedules of people who are not its friends, unasked and unlogged. The prose does not authorize the player to publish or use those schedules as a tactical tool. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “I did not put them in the log. You still have to decide what that means.”
Other voice: “The schedule did not get written down. I will not fill in an hour from memory.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No operation, target, identity, or patrol route is provided. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the current intel-asset state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — The schedules were never logged

Proposed diegetic text: “Copy label: Weather and patrol schedules received outside the log; no action specified.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the intel-asset state, the shelter gets weather copies and patrol schedules of people who are not its friends, unasked and unlogged. The prose does not authorize the player to publish or use those schedules as a tactical tool. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No operation, target, identity, or patrol route is provided. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the current intel-asset state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 051 — The schedules were never logged

In the intel-asset state, the shelter gets weather copies and patrol schedules of people who are not its friends, unasked and unlogged. The prose does not authorize the player to publish or use those schedules as a tactical tool. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “I did not put them in the log. You still have to decide what that means.”
Other voice: “The schedule did not get written down. I will not fill in an hour from memory.”
Liva Kern: “A note about a place is not permission to name who is there.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No operation, target, identity, or patrol route is provided. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — The schedules were never logged

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. In the intel-asset state, the shelter gets weather copies and patrol schedules of people who are not its friends, unasked and unlogged. The prose does not authorize the player to publish or use those schedules as a tactical tool. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only in the current intel-asset state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No operation, target, identity, or patrol route is provided.

Liva Kern may say: “I did not put them in the log. You still have to decide what that means.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy label: Weather and patrol schedules received outside the log; no action specified. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — The location is sold

After `liva_sell_the_location`, the garrison pays what it promised and the station answers on time again, with a second reader on the log. The finder’s fee spends fine; the frequency has a name attached, and the name is the player’s. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “The fee has a line. So does the name that went with it.”
Other voice: “They sold a location. I still do not know what happened there afterward.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No fee amount, Garrison operation, or new punishment. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only show in the late-sold state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — The location is sold

Proposed diegetic text: “Receipt: Finder’s fee paid; station answers; second reader added to log.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `liva_sell_the_location`, the garrison pays what it promised and the station answers on time again, with a second reader on the log. The finder’s fee spends fine; the frequency has a name attached, and the name is the player’s. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No fee amount, Garrison operation, or new punishment. Do not let the form claim authority that its keeper has not been given.

Later reading: Only show in the late-sold state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 055 — The location is sold

After `liva_sell_the_location`, the garrison pays what it promised and the station answers on time again, with a second reader on the log. The finder’s fee spends fine; the frequency has a name attached, and the name is the player’s. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “The fee has a line. So does the name that went with it.”
Other voice: “They sold a location. I still do not know what happened there afterward.”
Liva Kern: “The paper says what changed hands. It does not say what happened next.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No fee amount, Garrison operation, or new punishment. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — The location is sold

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. After `liva_sell_the_location`, the garrison pays what it promised and the station answers on time again, with a second reader on the log. The finder’s fee spends fine; the frequency has a name attached, and the name is the player’s. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Only show in the late-sold state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No fee amount, Garrison operation, or new punishment.

Liva Kern may say: “The fee has a line. So does the name that went with it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Receipt: Finder’s fee paid; station answers; second reader added to log. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — A name can mean a camp

The broadcaster’s separate story names moved-on camps at a fixed hour. A scene must distinguish those camp names from individual household names and from the shelter’s own names in `quest_arc_liva_03_signal`. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “These are camp names. Do not make them into a list of people.”
Other voice: “A name can be a camp without being an invitation to count its people.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not disclose individual identities or merge two signal quests. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only when the current broadcaster condition is active. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — A name can mean a camp

Proposed diegetic text: “Broadcast header: Moved-on camp names read at a fixed hour.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The broadcaster’s separate story names moved-on camps at a fixed hour. A scene must distinguish those camp names from individual household names and from the shelter’s own names in `quest_arc_liva_03_signal`. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not disclose individual identities or merge two signal quests. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when the current broadcaster condition is active. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 059 — A name can mean a camp

The broadcaster’s separate story names moved-on camps at a fixed hour. A scene must distinguish those camp names from individual household names and from the shelter’s own names in `quest_arc_liva_03_signal`. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “These are camp names. Do not make them into a list of people.”
Other voice: “A name can be a camp without being an invitation to count its people.”
Liva Kern: “You can carry the message. I cannot promise what it means.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not disclose individual identities or merge two signal quests. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — A name can mean a camp

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. The broadcaster’s separate story names moved-on camps at a fixed hour. A scene must distinguish those camp names from individual household names and from the shelter’s own names in `quest_arc_liva_03_signal`. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only when the current broadcaster condition is active. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not disclose individual identities or merge two signal quests.

Liva Kern may say: “These are camp names. Do not make them into a list of people.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Broadcast header: Moved-on camp names read at a fixed hour. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — The shelter name goes out

If the existing signal choice `liva_book_signal` is selected, the shelter’s names go out properly at a fixed hour and the log shows the shelter answered on time, every time. The writing must make that disclosure clear before the choice and never imply that the names can be recalled. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “You are choosing to send the names. I can write down that we did it.”
Other voice: “We sent the shelter name. Nobody agreed to send anyone’s name with it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No anonymous alternative, recall, or individual-name expansion. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Display only with the exact signal choice result. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — The shelter name goes out

Proposed diegetic text: “Signal note: Shelter’s names sent on the band at the fixed hour; logged.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the existing signal choice `liva_book_signal` is selected, the shelter’s names go out properly at a fixed hour and the log shows the shelter answered on time, every time. The writing must make that disclosure clear before the choice and never imply that the names can be recalled. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No anonymous alternative, recall, or individual-name expansion. Do not let the form claim authority that its keeper has not been given.

Later reading: Display only with the exact signal choice result. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 063 — The shelter name goes out

If the existing signal choice `liva_book_signal` is selected, the shelter’s names go out properly at a fixed hour and the log shows the shelter answered on time, every time. The writing must make that disclosure clear before the choice and never imply that the names can be recalled. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “You are choosing to send the names. I can write down that we did it.”
Other voice: “We sent the shelter name. Nobody agreed to send anyone’s name with it.”
Liva Kern: “We sent the name you chose. I will not add people to the signal.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No anonymous alternative, recall, or individual-name expansion. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — The shelter name goes out

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. If the existing signal choice `liva_book_signal` is selected, the shelter’s names go out properly at a fixed hour and the log shows the shelter answered on time, every time. The writing must make that disclosure clear before the choice and never imply that the names can be recalled. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Display only with the exact signal choice result. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No anonymous alternative, recall, or individual-name expansion.

Liva Kern may say: “You are choosing to send the names. I can write down that we did it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Signal note: Shelter’s names sent on the band at the fixed hour; logged. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — A listening watch has teachers

If the recruited Shelter Signals Keeper state applies, Liva teaches two of the shelter’s people the discipline. The prose can show her leaving room for them to repeat the log rule in their own words; it does not describe radio operation. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “The mast is wire. The part that matters is what you write beside it.”
Other voice: “You taught us how to keep the watch. You did not teach us to invent replies.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No extra trainee, new skill mechanic, or operating instructions. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only when recruited state is current. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — A listening watch has teachers

Proposed diegetic text: “Training note: Two learners; discipline named as the whole machine.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the recruited Shelter Signals Keeper state applies, Liva teaches two of the shelter’s people the discipline. The prose can show her leaving room for them to repeat the log rule in their own words; it does not describe radio operation. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No extra trainee, new skill mechanic, or operating instructions. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when recruited state is current. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 067 — A listening watch has teachers

If the recruited Shelter Signals Keeper state applies, Liva teaches two of the shelter’s people the discipline. The prose can show her leaving room for them to repeat the log rule in their own words; it does not describe radio operation. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “The mast is wire. The part that matters is what you write beside it.”
Other voice: “You taught us how to keep the watch. You did not teach us to invent replies.”
Liva Kern: “I can sit with your watch. I still need the page to stay honest.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No extra trainee, new skill mechanic, or operating instructions. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — A listening watch has teachers

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. If the recruited Shelter Signals Keeper state applies, Liva teaches two of the shelter’s people the discipline. The prose can show her leaving room for them to repeat the log rule in their own words; it does not describe radio operation. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only when recruited state is current. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No extra trainee, new skill mechanic, or operating instructions.

Liva Kern may say: “The mast is wire. The part that matters is what you write beside it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Training note: Two learners; discipline named as the whole machine. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — The log ends mid-entry

If Liva is deceased, the mast still stands and the log ends mid-entry, on schedule, in her neat hand. Someone out there keeps trying to raise the station that answered on time. The page does not invent the voice or explain the silence. Begin with the work already under way, before the visitor is asked to decide what it means. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Liva Kern: “Do not complete the sentence for her.”
Other voice: “The log ends here. I will not make her last blank into an answer.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No last message, cause of death, or identity of caller. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the existing deceased state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — The log ends mid-entry

Proposed diegetic text: “Archive note: Log ends mid-entry; unanswered station call continues elsewhere.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If Liva is deceased, the mast still stands and the log ends mid-entry, on schedule, in her neat hand. Someone out there keeps trying to raise the station that answered on time. The page does not invent the voice or explain the silence. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No last message, cause of death, or identity of caller. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the existing deceased state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 071 — The log ends mid-entry

If Liva is deceased, the mast still stands and the log ends mid-entry, on schedule, in her neat hand. Someone out there keeps trying to raise the station that answered on time. The page does not invent the voice or explain the silence. Let Liva Kern speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Liva Kern: “Do not complete the sentence for her.”
Other voice: “The log ends here. I will not make her last blank into an answer.”
Watch clerk: “The shift log stops there. I will not make the blank into an answer.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No last message, cause of death, or identity of caller. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — The log ends mid-entry

On a later visit permitted by the exact existing Liva choice and the current NPC state; for Grimm below, the existing low/neutral/high standing greeting only, the player may notice what the earlier choice left in view. If Liva is deceased, the mast still stands and the log ends mid-entry, on schedule, in her neat hand. Someone out there keeps trying to raise the station that answered on time. The page does not invent the voice or explain the silence. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Liva writes neatly and speaks in time, band, call, and log. She does not mistake repetition for certainty. Her pauses are deliberate, not mystical. She can be warm about a properly identified message while refusing an unlogged broadcast or a question whose answer belongs to the other end.

Observable return: Use only in the existing deceased state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No last message, cause of death, or identity of caller.

Liva Kern may say: “Do not complete the sentence for her.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Archive note: Log ends mid-entry; unanswered station call continues elsewhere. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- The mast is live. That is what I can tell you. It is not the same as knowing who is there.
- If it happened, it belongs in the log. If it did not, a blank is better than a story.
- You can say who is calling. You cannot make the other end answer.
- You bought a place in the hour. The hour is still mine to keep.
- I wrote down what went out. I did not write down what nobody answered.
- The paper arrived first. That is useful. It is not a promise about every sky.
- We kept the hour. We still do not know who kept the other end.
- They are two valleys out. I have not told you what they intend.
- You know there are two. That is all I am choosing to tell you.
- Do not write the answer for me while the page is still open.
- The quiet is in the log too. I will write it down.
- I can tell you that I am transmitting. I will not tell you where from.
- I did not put them in the log. You still have to decide what that means.
- The fee has a line. So does the name that went with it.
- These are camp names. Do not make them into a list of people.
- You are choosing to send the names. I can write down that we did it.
- The mast is wire. The part that matters is what you write beside it.
- Do not complete the sentence for her.

## 13. Continuity and editorial review

Check `enc_arc_liva_tower`, `enc_arc_liva_survey`, all three Liva quests, and their `npc_arcs.json` conditions. Keep the initial cell trade, the one-time identified message, survey warning, location sale, later camp naming, shelter-name broadcast, intel-asset information, and late-sold state distinct. Do not infer a real responder from the log. Confirm current content before placement. Keep the first-visit premise recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposals only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the current character or settlement content, each callback matches an authored condition, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
