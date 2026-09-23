# EXPANSION 108 — Two Versions in Full View

## Sena Aris, the bridge corridor, and the incident report that comes back smaller than the log she filed.

### Wave 21: Records Kept in Human Hands

## 1. Expansion thesis

Let the crossing log be a working document rather than a magic fix. Sena keeps it in full view because a crossing can be made to mean something different once the report reaches command. The player’s existing choice to countersign the divergent page or stay clear should register as an act with a visible cost and no promise that the bridge becomes safe or politics become simple. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character quest. It adds no gameplay feature and does not claim that any proposed text is already in the game.

## 2. Story in one sentence

A patrol officer lays two accounts beside a queue that has been waiting long enough to recognize the difference without needing it explained.

## 3. Verified local anchor and current story

`characters.json` defines `npc_sena_aris` as a militia patrol soldier at `loc_bridge_seven`. She keeps a civilian crossing log in full view, wants it countersigned and witnessed, offers escorted crossings, the log as written, and warning before warrant sweeps, and will not lose an incident report or fire on a crossing queue. `npc_arcs.json` records initial, evolved, late-reformist, late-steadfast, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_sena_01_log`; `quests_npc_arcs.json` registers `quest_arc_sena_01_log`. `sena_countersign` puts the player’s mark on the page where versions diverge; the evolved state says she stops walking the corridor alone, not that she is safe. The late reformist state makes the log a two-signature standard with no acknowledged-only incidents, tolerated by command because the bridge works. `sena_stay_clear` leaves one signature; Sena initials both versions of the next incident herself. Keep these outcomes separate. Names, locations, quest IDs, choice IDs, and state summaries above come from the current data. The encounter catalog proves the authored scene and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing state condition.

## 4. Fixed canon and proposed prose

Bridge Seven is a four-lane intact bridge over a gorge, guarded at both ends, with charges visibly taped beneath the span. The current location says it saves a day while the alternate route takes a season. This plan writes only the log encounter and the ordinary people waiting to cross. It does not explain, touch, disable, or route around the charges, and it does not declare a crossing safe. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to the character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Sena stayed because somebody had to, but the prose should not turn that sentence into permission to spend her safety indefinitely. She has made the log public enough to be read and still needs another hand to witness it. Her attention stays on people moving through the corridor, not on the player’s wish to become the story’s brave signature. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use the first five beats with the existing `enc_arc_sena_01_log` scene. Beats 6–10 are conditional on `sena_countersign`, with later reformist lines only under `late_reformist`. Beats 11–14 belong to `sena_stay_clear` and `late_steadfast`. The remaining beats are return details for the current late state and must not be shown as universal bridge law. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character-location owner as the insertion point; do not create a parallel quest chain or a second mutable owner.

## 8. Player agency and consequence

The registered choice is to countersign the page where versions diverge or stay clear and cross on Sena’s terms. Do not convert witnessing into command authority, invent a vote, or imply that a signature proves every claim on the page. Let the player understand that staying clear also has a defined state and is not an invisible failure. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before the choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep the scene on the page, the queue, and the people waiting. The bridge hazard is existing environmental text, not a gameplay puzzle for this expansion. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when an implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The oilcloth holds both copies

At Bridge Seven the crossing log hangs open in its oilcloth. Sena puts the version she filed beside the version command acknowledged, then steps far enough back for the visitor to read without crowding the queue. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I am not asking you to guess which report is louder. I am asking whether you can read both.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not invent the incident’s details or a new report field. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the two versions together whenever the encounter is shown. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The oilcloth holds both copies

Proposed diegetic text: “Log heading: Filed account beside acknowledged account; divergence visible.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At Bridge Seven the crossing log hangs open in its oilcloth. Sena puts the version she filed beside the version command acknowledged, then steps far enough back for the visitor to read without crowding the queue. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent the incident’s details or a new report field. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the two versions together whenever the encounter is shown. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 003 — The oilcloth holds both copies

At Bridge Seven the crossing log hangs open in its oilcloth. Sena puts the version she filed beside the version command acknowledged, then steps far enough back for the visitor to read without crowding the queue. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I am not asking you to guess which report is louder. I am asking whether you can read both.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Keep the two versions together whenever the encounter is shown.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent the incident’s details or a new report field. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The oilcloth holds both copies

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At Bridge Seven the crossing log hangs open in its oilcloth. Sena puts the version she filed beside the version command acknowledged, then steps far enough back for the visitor to read without crowding the queue. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Keep the two versions together whenever the encounter is shown. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent the incident’s details or a new report field.

Sena Aris may say: “I am not asking you to guess which report is louder. I am asking whether you can read both.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Log heading: Filed account beside acknowledged account; divergence visible. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — The queue keeps its place

A person in line adjusts a strap while another checks how far the next crossing has moved. They have been waiting, and the log is open within their view. Nobody is asked to perform outrage or supply a dramatic witness statement. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “They were here before we opened the book. They will still be here when we close it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add a riot, delay penalty, or a new queue rule. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: On a return, show ordinary crossing work before commentary. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — The queue keeps its place

Proposed diegetic text: “Queue margin: Waiting continues while the two reports are read.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A person in line adjusts a strap while another checks how far the next crossing has moved. They have been waiting, and the log is open within their view. Nobody is asked to perform outrage or supply a dramatic witness statement. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a riot, delay penalty, or a new queue rule. Do not let the form claim authority that its keeper has not been given.

Later reading: On a return, show ordinary crossing work before commentary. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 007 — The queue keeps its place

A person in line adjusts a strap while another checks how far the next crossing has moved. They have been waiting, and the log is open within their view. Nobody is asked to perform outrage or supply a dramatic witness statement. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “They were here before we opened the book. They will still be here when we close it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “On a return, show ordinary crossing work before commentary.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a riot, delay penalty, or a new queue rule. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — The queue keeps its place

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A person in line adjusts a strap while another checks how far the next crossing has moved. They have been waiting, and the log is open within their view. Nobody is asked to perform outrage or supply a dramatic witness statement. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: On a return, show ordinary crossing work before commentary. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a riot, delay penalty, or a new queue rule.

Sena Aris may say: “They were here before we opened the book. They will still be here when we close it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Queue margin: Waiting continues while the two reports are read. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — Her signature is not the missing one

Sena points to her own filed report and the acknowledgment beneath it. The account she wants countersigned is already legible; what is absent is a second witness on the divergent page. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I wrote what I filed. I cannot sign in the place of someone who saw it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not write the player’s name before `sena_countersign`. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only the current registered choice may add the player’s mark. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — Her signature is not the missing one

Proposed diegetic text: “Witness line: Blank before choice; Sena’s entry remains hers.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Sena points to her own filed report and the acknowledgment beneath it. The account she wants countersigned is already legible; what is absent is a second witness on the divergent page. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not write the player’s name before `sena_countersign`. Do not let the form claim authority that its keeper has not been given.

Later reading: Only the current registered choice may add the player’s mark. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 011 — Her signature is not the missing one

Sena points to her own filed report and the acknowledgment beneath it. The account she wants countersigned is already legible; what is absent is a second witness on the divergent page. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I wrote what I filed. I cannot sign in the place of someone who saw it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Only the current registered choice may add the player’s mark.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not write the player’s name before `sena_countersign`. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — Her signature is not the missing one

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Sena points to her own filed report and the acknowledgment beneath it. The account she wants countersigned is already legible; what is absent is a second witness on the divergent page. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Only the current registered choice may add the player’s mark. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not write the player’s name before `sena_countersign`.

Sena Aris may say: “I wrote what I filed. I cannot sign in the place of someone who saw it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Witness line: Blank before choice; Sena’s entry remains hers. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — A report comes back smaller

The draft focuses on the difference between the incident report Sena filed and the version command acknowledged, without stating a new reason for the difference. The gap is allowed to remain a gap the player can see. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I can show you where the words stop matching. I cannot tell you what made them stop.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No motive, censorship order, or incident content is invented. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Preserve the source’s uncertainty about why the two documents diverge. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — A report comes back smaller

Proposed diegetic text: “Copy note: Filed and acknowledged versions do not match in full.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The draft focuses on the difference between the incident report Sena filed and the version command acknowledged, without stating a new reason for the difference. The gap is allowed to remain a gap the player can see. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No motive, censorship order, or incident content is invented. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the source’s uncertainty about why the two documents diverge. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 015 — A report comes back smaller

The draft focuses on the difference between the incident report Sena filed and the version command acknowledged, without stating a new reason for the difference. The gap is allowed to remain a gap the player can see. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I can show you where the words stop matching. I cannot tell you what made them stop.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Preserve the source’s uncertainty about why the two documents diverge.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No motive, censorship order, or incident content is invented. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — A report comes back smaller

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The draft focuses on the difference between the incident report Sena filed and the version command acknowledged, without stating a new reason for the difference. The gap is allowed to remain a gap the player can see. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Preserve the source’s uncertainty about why the two documents diverge. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No motive, censorship order, or incident content is invented.

Sena Aris may say: “I can show you where the words stop matching. I cannot tell you what made them stop.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy note: Filed and acknowledged versions do not match in full. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The obvious thing is still a choice

The encounter says Sena waits for someone to do the obvious thing. The scene makes visible that signing and staying clear are both choices already offered. The player can read the page without being pushed by the queue’s gaze into a false third option. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “You may leave the page as it is. I need you to know that leaving it is also an answer.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add an abstention flag or a hidden penalty. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use the exact existing two choice IDs. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The obvious thing is still a choice

Proposed diegetic text: “Choice reminder: Countersign the divergent page, or stay clear and cross on her terms.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The encounter says Sena waits for someone to do the obvious thing. The scene makes visible that signing and staying clear are both choices already offered. The player can read the page without being pushed by the queue’s gaze into a false third option. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add an abstention flag or a hidden penalty. Do not let the form claim authority that its keeper has not been given.

Later reading: Use the exact existing two choice IDs. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 019 — The obvious thing is still a choice

The encounter says Sena waits for someone to do the obvious thing. The scene makes visible that signing and staying clear are both choices already offered. The player can read the page without being pushed by the queue’s gaze into a false third option. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “You may leave the page as it is. I need you to know that leaving it is also an answer.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Use the exact existing two choice IDs.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add an abstention flag or a hidden penalty. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The obvious thing is still a choice

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The encounter says Sena waits for someone to do the obvious thing. The scene makes visible that signing and staying clear are both choices already offered. The player can read the page without being pushed by the queue’s gaze into a false third option. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Use the exact existing two choice IDs. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add an abstention flag or a hidden penalty.

Sena Aris may say: “You may leave the page as it is. I need you to know that leaving it is also an answer.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Choice reminder: Countersign the divergent page, or stay clear and cross on her terms. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — The first mark does not end the shift

After `sena_countersign`, the player’s mark appears on the divergent page. Sena finishes the patrol rather than announcing that the dispute is solved. People still have to cross under the current corridor conditions. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “Your mark is here. The bridge is still the bridge.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new crossing entitlement or hazard reduction. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use after `sena_countersign`, not on the initial visit. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — The first mark does not end the shift

Proposed diegetic text: “Log note: Player countersignature recorded on the page where versions diverge.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `sena_countersign`, the player’s mark appears on the divergent page. Sena finishes the patrol rather than announcing that the dispute is solved. People still have to cross under the current corridor conditions. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new crossing entitlement or hazard reduction. Do not let the form claim authority that its keeper has not been given.

Later reading: Use after `sena_countersign`, not on the initial visit. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 023 — The first mark does not end the shift

After `sena_countersign`, the player’s mark appears on the divergent page. Sena finishes the patrol rather than announcing that the dispute is solved. People still have to cross under the current corridor conditions. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “Your mark is here. The bridge is still the bridge.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Use after `sena_countersign`, not on the initial visit.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new crossing entitlement or hazard reduction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — The first mark does not end the shift

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `sena_countersign`, the player’s mark appears on the divergent page. Sena finishes the patrol rather than announcing that the dispute is solved. People still have to cross under the current corridor conditions. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Use after `sena_countersign`, not on the initial visit. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new crossing entitlement or hazard reduction.

Sena Aris may say: “Your mark is here. The bridge is still the bridge.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Log note: Player countersignature recorded on the page where versions diverge. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — She no longer walks alone

In the evolved `Countersigned` state, Sena has stopped walking the corridor alone. The prose shows another person visible at a distance but does not call that safety or establish who the person is. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “There is another set of steps behind me. That is not the same as safe.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not name an escort or claim protection. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the source’s explicit distinction between company and safety. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — She no longer walks alone

Proposed diegetic text: “Watch note: Sena no longer walks the corridor alone; the source does not call it safe.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the evolved `Countersigned` state, Sena has stopped walking the corridor alone. The prose shows another person visible at a distance but does not call that safety or establish who the person is. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not name an escort or claim protection. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the source’s explicit distinction between company and safety. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 027 — She no longer walks alone

In the evolved `Countersigned` state, Sena has stopped walking the corridor alone. The prose shows another person visible at a distance but does not call that safety or establish who the person is. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “There is another set of steps behind me. That is not the same as safe.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Keep the source’s explicit distinction between company and safety.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not name an escort or claim protection. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — She no longer walks alone

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the evolved `Countersigned` state, Sena has stopped walking the corridor alone. The prose shows another person visible at a distance but does not call that safety or establish who the person is. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Keep the source’s explicit distinction between company and safety. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not name an escort or claim protection.

Sena Aris may say: “There is another set of steps behind me. That is not the same as safe.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Watch note: Sena no longer walks the corridor alone; the source does not call it safe. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — A third signature below the copies

If `late_reformist` applies, command reads both versions with a third signature beneath them. A return page can show the alignment of three marks and leave the command’s actual opinion unstated. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “The record has more hands on it now. It still has two versions.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not state that the third signature endorses either account. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only show under `late_reformist`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — A third signature below the copies

Proposed diegetic text: “Late log: Two versions retained; third signature appears beneath them.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `late_reformist` applies, command reads both versions with a third signature beneath them. A return page can show the alignment of three marks and leave the command’s actual opinion unstated. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not state that the third signature endorses either account. Do not let the form claim authority that its keeper has not been given.

Later reading: Only show under `late_reformist`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 031 — A third signature below the copies

If `late_reformist` applies, command reads both versions with a third signature beneath them. A return page can show the alignment of three marks and leave the command’s actual opinion unstated. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “The record has more hands on it now. It still has two versions.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Only show under `late_reformist`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not state that the third signature endorses either account. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — A third signature below the copies

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If `late_reformist` applies, command reads both versions with a third signature beneath them. A return page can show the alignment of three marks and leave the command’s actual opinion unstated. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Only show under `late_reformist`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not state that the third signature endorses either account.

Sena Aris may say: “The record has more hands on it now. It still has two versions.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late log: Two versions retained; third signature appears beneath them. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — No acknowledged-only incidents

The reformist state says the standard has no acknowledged-only incidents. A proposed notice names that standard without explaining how command was made to tolerate it. The bridge works; that is the stated reason, not a legal precedent. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “Command tolerates the book because the bridge works. I will not make that sentence larger.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No policy mechanism, order, or command motive beyond current text. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Do not apply this standard to the steadfast or unmet states. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — No acknowledged-only incidents

Proposed diegetic text: “Notice: Two signatures, full view, no acknowledged-only incidents.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The reformist state says the standard has no acknowledged-only incidents. A proposed notice names that standard without explaining how command was made to tolerate it. The bridge works; that is the stated reason, not a legal precedent. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No policy mechanism, order, or command motive beyond current text. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not apply this standard to the steadfast or unmet states. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 035 — No acknowledged-only incidents

The reformist state says the standard has no acknowledged-only incidents. A proposed notice names that standard without explaining how command was made to tolerate it. The bridge works; that is the stated reason, not a legal precedent. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “Command tolerates the book because the bridge works. I will not make that sentence larger.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Do not apply this standard to the steadfast or unmet states.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No policy mechanism, order, or command motive beyond current text. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — No acknowledged-only incidents

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The reformist state says the standard has no acknowledged-only incidents. A proposed notice names that standard without explaining how command was made to tolerate it. The bridge works; that is the stated reason, not a legal precedent. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Do not apply this standard to the steadfast or unmet states. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No policy mechanism, order, or command motive beyond current text.

Sena Aris may say: “Command tolerates the book because the bridge works. I will not make that sentence larger.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Notice: Two signatures, full view, no acknowledged-only incidents. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — Her watch owns an hour

In `late_reformist`, the player’s column crosses at the hour Sena’s watch owns, and the log says so. The passage need not give a new clock time; the relevant detail is that the crossing is recorded on her watch. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “The hour is written down. You do not have to call it a promise for every hour.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add a schedule or guaranteed escort. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep this only within the late reformist state. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — Her watch owns an hour

Proposed diegetic text: “Crossing line: Player’s column recorded at Sena’s watch hour.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_reformist`, the player’s column crosses at the hour Sena’s watch owns, and the log says so. The passage need not give a new clock time; the relevant detail is that the crossing is recorded on her watch. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a schedule or guaranteed escort. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep this only within the late reformist state. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 039 — Her watch owns an hour

In `late_reformist`, the player’s column crosses at the hour Sena’s watch owns, and the log says so. The passage need not give a new clock time; the relevant detail is that the crossing is recorded on her watch. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “The hour is written down. You do not have to call it a promise for every hour.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Keep this only within the late reformist state.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a schedule or guaranteed escort. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — Her watch owns an hour

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_reformist`, the player’s column crosses at the hour Sena’s watch owns, and the log says so. The passage need not give a new clock time; the relevant detail is that the crossing is recorded on her watch. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Keep this only within the late reformist state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a schedule or guaranteed escort.

Sena Aris may say: “The hour is written down. You do not have to call it a promise for every hour.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Crossing line: Player’s column recorded at Sena’s watch hour. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The one signature stays one

If the player chose `sena_stay_clear`, the log remains countersigned by nobody. Sena initials both versions of the next incident herself. The initials are a record of what she did, not a substitute for the witness she once asked for. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I wrote my initials on both. I did not pretend they belonged to anyone else.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No unnamed witness or extra signature is added. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only for `late_steadfast`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The one signature stays one

Proposed diegetic text: “Late log: One signature remains; Sena initials both versions of the next report.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the player chose `sena_stay_clear`, the log remains countersigned by nobody. Sena initials both versions of the next incident herself. The initials are a record of what she did, not a substitute for the witness she once asked for. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No unnamed witness or extra signature is added. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only for `late_steadfast`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 043 — The one signature stays one

If the player chose `sena_stay_clear`, the log remains countersigned by nobody. Sena initials both versions of the next incident herself. The initials are a record of what she did, not a substitute for the witness she once asked for. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I wrote my initials on both. I did not pretend they belonged to anyone else.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Use only for `late_steadfast`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No unnamed witness or extra signature is added. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The one signature stays one

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If the player chose `sena_stay_clear`, the log remains countersigned by nobody. Sena initials both versions of the next incident herself. The initials are a record of what she did, not a substitute for the witness she once asked for. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Use only for `late_steadfast`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No unnamed witness or extra signature is added.

Sena Aris may say: “I wrote my initials on both. I did not pretend they belonged to anyone else.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late log: One signature remains; Sena initials both versions of the next report. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — Orderly is not the same as agreed

The steadfast state says the corridor is orderly, crossings honest, and reports still arrive in two versions. A person waiting may see the order and still notice the split documents. The prose does not treat visible order as proof that everyone agrees. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “The line is moving. That tells us the line is moving. It does not answer the page.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No claim that disagreement has ended. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the later state’s calm and its unresolved record together. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — Orderly is not the same as agreed

Proposed diegetic text: “Margin: Queue orderly; report versions remain separate.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The steadfast state says the corridor is orderly, crossings honest, and reports still arrive in two versions. A person waiting may see the order and still notice the split documents. The prose does not treat visible order as proof that everyone agrees. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No claim that disagreement has ended. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the later state’s calm and its unresolved record together. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 047 — Orderly is not the same as agreed

The steadfast state says the corridor is orderly, crossings honest, and reports still arrive in two versions. A person waiting may see the order and still notice the split documents. The prose does not treat visible order as proof that everyone agrees. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “The line is moving. That tells us the line is moving. It does not answer the page.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Keep the later state’s calm and its unresolved record together.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No claim that disagreement has ended. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — Orderly is not the same as agreed

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The steadfast state says the corridor is orderly, crossings honest, and reports still arrive in two versions. A person waiting may see the order and still notice the split documents. The prose does not treat visible order as proof that everyone agrees. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Keep the later state’s calm and its unresolved record together. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No claim that disagreement has ended.

Sena Aris may say: “The line is moving. That tells us the line is moving. It does not answer the page.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin: Queue orderly; report versions remain separate. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — A warning before a warrant sweep

Sena’s character record says she offers warning before warrant sweeps. If the current content invokes that offer, a brief line may warn that a sweep is coming without inventing a name, destination, or date. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I can tell you a sweep is coming. I will not tell you whose door to open.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add evasion guidance or expose a protected person. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only place if an existing event makes the warning relevant. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — A warning before a warrant sweep

Proposed diegetic text: “Warning stub: A warrant sweep is approaching; no person named in this draft.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Sena’s character record says she offers warning before warrant sweeps. If the current content invokes that offer, a brief line may warn that a sweep is coming without inventing a name, destination, or date. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add evasion guidance or expose a protected person. Do not let the form claim authority that its keeper has not been given.

Later reading: Only place if an existing event makes the warning relevant. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 051 — A warning before a warrant sweep

Sena’s character record says she offers warning before warrant sweeps. If the current content invokes that offer, a brief line may warn that a sweep is coming without inventing a name, destination, or date. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I can tell you a sweep is coming. I will not tell you whose door to open.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Only place if an existing event makes the warning relevant.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add evasion guidance or expose a protected person. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — A warning before a warrant sweep

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Sena’s character record says she offers warning before warrant sweeps. If the current content invokes that offer, a brief line may warn that a sweep is coming without inventing a name, destination, or date. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Only place if an existing event makes the warning relevant. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add evasion guidance or expose a protected person.

Sena Aris may say: “I can tell you a sweep is coming. I will not tell you whose door to open.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Warning stub: A warrant sweep is approaching; no person named in this draft. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — Witnesses write what they saw

A civilian who chooses to write down a crossing is not asked to certify the entire incident report. Their words can remain limited to the time they joined the queue and the movement they personally observed. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “Write what you saw. Leave the rest in the book for whoever can answer it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not create a witness intake feature or official evidentiary rule. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Any proposed note needs a current encounter or record surface. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — Witnesses write what they saw

Proposed diegetic text: “Witness note: Personal observation only; no conclusion copied from another report.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A civilian who chooses to write down a crossing is not asked to certify the entire incident report. Their words can remain limited to the time they joined the queue and the movement they personally observed. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not create a witness intake feature or official evidentiary rule. Do not let the form claim authority that its keeper has not been given.

Later reading: Any proposed note needs a current encounter or record surface. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 055 — Witnesses write what they saw

A civilian who chooses to write down a crossing is not asked to certify the entire incident report. Their words can remain limited to the time they joined the queue and the movement they personally observed. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “Write what you saw. Leave the rest in the book for whoever can answer it.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Any proposed note needs a current encounter or record surface.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not create a witness intake feature or official evidentiary rule. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — Witnesses write what they saw

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A civilian who chooses to write down a crossing is not asked to certify the entire incident report. Their words can remain limited to the time they joined the queue and the movement they personally observed. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Any proposed note needs a current encounter or record surface. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not create a witness intake feature or official evidentiary rule.

Sena Aris may say: “Write what you saw. Leave the rest in the book for whoever can answer it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Witness note: Personal observation only; no conclusion copied from another report. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — The charges remain outside this story

A player glances toward the visible bridge wiring described by `loc_bridge_seven`. Sena turns attention back to the queue and the log. The prose does not explain the wiring or use it to force a choice about her report. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “I am keeping this book. I am not asking you to touch anything under the span.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No tactical or defusal content. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the scene on the authored log encounter. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — The charges remain outside this story

Proposed diegetic text: “Editorial note: Bridge hazard remains environmental text; this encounter concerns the crossing log.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A player glances toward the visible bridge wiring described by `loc_bridge_seven`. Sena turns attention back to the queue and the log. The prose does not explain the wiring or use it to force a choice about her report. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No tactical or defusal content. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the scene on the authored log encounter. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 059 — The charges remain outside this story

A player glances toward the visible bridge wiring described by `loc_bridge_seven`. Sena turns attention back to the queue and the log. The prose does not explain the wiring or use it to force a choice about her report. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “I am keeping this book. I am not asking you to touch anything under the span.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Keep the scene on the authored log encounter.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No tactical or defusal content. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — The charges remain outside this story

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A player glances toward the visible bridge wiring described by `loc_bridge_seven`. Sena turns attention back to the queue and the log. The prose does not explain the wiring or use it to force a choice about her report. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Keep the scene on the authored log encounter. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No tactical or defusal content.

Sena Aris may say: “I am keeping this book. I am not asking you to touch anything under the span.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial note: Bridge hazard remains environmental text; this encounter concerns the crossing log. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — A copied report needs its own label

If an account is copied for later reading, the page says whether it is Sena’s filed account or the command-acknowledged version. A copy should not silently merge them into a single paragraph that looks more authoritative. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “If you copy it, copy the difference too.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new archive or cross-faction filing system. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only use an existing content surface that can preserve the source labels. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — A copied report needs its own label

Proposed diegetic text: “Copy heading: Filed by Sena / acknowledged by command; versions kept distinct.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If an account is copied for later reading, the page says whether it is Sena’s filed account or the command-acknowledged version. A copy should not silently merge them into a single paragraph that looks more authoritative. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new archive or cross-faction filing system. Do not let the form claim authority that its keeper has not been given.

Later reading: Only use an existing content surface that can preserve the source labels. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 063 — A copied report needs its own label

If an account is copied for later reading, the page says whether it is Sena’s filed account or the command-acknowledged version. A copy should not silently merge them into a single paragraph that looks more authoritative. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “If you copy it, copy the difference too.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Only use an existing content surface that can preserve the source labels.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new archive or cross-faction filing system. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — A copied report needs its own label

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If an account is copied for later reading, the page says whether it is Sena’s filed account or the command-acknowledged version. A copy should not silently merge them into a single paragraph that looks more authoritative. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Only use an existing content surface that can preserve the source labels. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new archive or cross-faction filing system.

Sena Aris may say: “If you copy it, copy the difference too.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Copy heading: Filed by Sena / acknowledged by command; versions kept distinct. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The bridge keeps the hour

At the end of the visit, the next person enters the corridor under the current state. Sena closes no record that the existing content leaves open. The final image is a log in its oilcloth and the queue moving as it did before the player arrived. Begin with the work already under way, before the visitor is asked to decide what it means. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Sena Aris: “The next person is waiting. Let me do my work.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new crossing outcome, danger change, or faction standing. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Close only with the currently active state’s log. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The bridge keeps the hour

Proposed diegetic text: “Closing line: Crossing continues under the current watch; log state unchanged.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the end of the visit, the next person enters the corridor under the current state. Sena closes no record that the existing content leaves open. The final image is a log in its oilcloth and the queue moving as it did before the player arrived. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new crossing outcome, danger change, or faction standing. Do not let the form claim authority that its keeper has not been given.

Later reading: Close only with the currently active state’s log. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 067 — The bridge keeps the hour

At the end of the visit, the next person enters the corridor under the current state. Sena closes no record that the existing content leaves open. The final image is a log in its oilcloth and the queue moving as it did before the player arrived. Let Sena Aris speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Sena Aris: “The next person is waiting. Let me do my work.”
Other voice: “A person in the queue says, “I can tell you when I crossed. I cannot tell you what command wrote down afterward.””
Sena Aris: “Close only with the currently active state’s log.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new crossing outcome, danger change, or faction standing. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The bridge keeps the hour

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the end of the visit, the next person enters the corridor under the current state. Sena closes no record that the existing content leaves open. The final image is a log in its oilcloth and the queue moving as it did before the player arrived. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Sena speaks in complete, practical sentences. She distinguishes a report she filed from the report command acknowledged without raising her voice to make one sound more true. The queue has its own pace and does not become a chorus. She refuses violence at the crossing; she does not promise that every authority will agree with her record.

Observable return: Close only with the currently active state’s log. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new crossing outcome, danger change, or faction standing.

Sena Aris may say: “The next person is waiting. Let me do my work.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Closing line: Crossing continues under the current watch; log state unchanged. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- I am not asking you to guess which report is louder. I am asking whether you can read both.
- They were here before we opened the book. They will still be here when we close it.
- I wrote what I filed. I cannot sign in the place of someone who saw it.
- I can show you where the words stop matching. I cannot tell you what made them stop.
- You may leave the page as it is. I need you to know that leaving it is also an answer.
- Your mark is here. The bridge is still the bridge.
- There is another set of steps behind me. That is not the same as safe.
- The record has more hands on it now. It still has two versions.
- Command tolerates the book because the bridge works. I will not make that sentence larger.
- The hour is written down. You do not have to call it a promise for every hour.
- I wrote my initials on both. I did not pretend they belonged to anyone else.
- The line is moving. That tells us the line is moving. It does not answer the page.
- I can tell you a sweep is coming. I will not tell you whose door to open.
- Write what you saw. Leave the rest in the book for whoever can answer it.
- I am keeping this book. I am not asking you to touch anything under the span.
- If you copy it, copy the difference too.
- The next person is waiting. Let me do my work.

## 13. Continuity and editorial review

Check every state-conditioned callback against `npc_sena_aris`, `quest_arc_sena_01_log`, and `enc_arc_sena_01_log`. Preserve her full-view civilian log, the two versions, both registered choices, the evolved not-the-same-as-safe wording, and separate late outcomes. Do not turn a countersignature into a weapon, legal judgment, or safety guarantee. Confirm current content before placement. Keep the first-visit encounter recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposed only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the existing character and encounter content, each callback matches the authored choice, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
