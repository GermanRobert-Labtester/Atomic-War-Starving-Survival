# EXPANSION 113 — The Morning the Ledger Missed

## Tomas Geret, the overflow clinic in dentist’s row, and three ledgers that do not agree on what arrived.

### Wave 22: What the Account Cannot Hold

## 1. Expansion thesis

Let Tomas’s choice turn on the difference between a discrepancy that kept people alive and a form that can count only stock. The player can vouch for the rounding or testify to the discrepancy and let procedure run. The prose must leave the mornings visible without falsifying a death record, diagnosing anyone, or making the third ledger public property. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character story or settlement dialogue surface. It adds no gameplay feature and does not claim that proposed text is already in the game.

## 2. Story in one sentence

A field medic reconciles the clinic after treating first, while an audit asks what the patients never had reason to ask.

## 3. Verified local anchor and current story

`characters.json` defines `npc_tomas_geret`, Field Medic at `loc_dentists_row`. The former dentist row’s chairs have been pulled out for cots. Tomas keeps two ledgers the Garrison knows about and a third recording the difference between what arrived and what was logged. `npc_arcs.json` contains initial, evolved, late-cleared, late-accused, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_tomas_01_rounding`; `quests_npc_arcs.json` registers `quest_arc_tomas_01_rounding`. The current choices are `tomas_vouch`—vouch for the rounding when the audit comes, because the mornings it bought were real—or `tomas_testify`—testify to the discrepancy and let procedure run. Vouching yields a line item instead of a charge and the clinic keeps its stock; testimony produces charges that fit the entries but miss the mornings. The late-cleared and late-accused states remain distinct. Names, locations, quest IDs, choice IDs, and state summaries above come from current local data. The registered encounter or character record proves the authored premise and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing condition.

## 4. Fixed canon and proposed prose

Tomas treats first and reconciles later, in the wrong order and the only order that works. He offers treatment without questions yet, first-aid training, and quiet prioritization; he will not let a queue die of protocol or falsify a death record. The audit asks different questions than patients do. The current data does not disclose patient names, diagnoses, exact stock counts, the third ledger’s full contents, or a procedural hearing transcript. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to a character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Tomas is not asking the player to call every unlogged item harmless. He asks that the audit count the mornings the difference bought. A patient does not need to know which ledger carries the clinic through a shift; Tomas does, and he expects to be answerable for it. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–6 in the current encounter and choice. Beats 7–10 follow `tomas_vouch` through `evolved` and `late_cleared`; beats 11–14 follow `tomas_testify` into `late_accused`. Remaining passages apply only to the initial or late-unmet states. Never show the third ledger’s details in an unconditioned scene or reveal a patient’s identity. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character/settlement dialogue owner as the insertion point. Where the inspected data has no such route, keep the text as an editorial proposal and do not imply a new encounter has been approved.

## 8. Player agency and consequence

The player is asked to vouch or testify; the plan adds no private compromise, new audit choice, or patient-disclosure option. A vouch changes how the discrepancy is recorded, not whether the two known ledgers exist. Testimony does not erase the mornings, but the authored charges and queue outcome must remain intact. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before a choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Protect patient confidentiality. The plan describes no treatment, stock-handling, audit procedure, or death-record workaround. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, electrical, navigation, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when implementation later chooses a smaller, coherent subset.

### Scene draft 001 — Dentist chairs pulled out for cots

The overflow clinic is in a former dentist’s row because the real one filled in the first winter. Chairs have been pulled out for cots. Tomas finishes the work in front of him before turning to the visitor and the audit question. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “The room was full before the form arrived. I treated the people who were here.”
Other voice: “He came in before the paper did. I can tell you that much.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No patient count or diagnosis is supplied. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the physical setup tied to the current encounter. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — Dentist chairs pulled out for cots

Proposed diegetic text: “Clinic note: Former dentist-row chairs pulled out for cots.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The overflow clinic is in a former dentist’s row because the real one filled in the first winter. Chairs have been pulled out for cots. Tomas finishes the work in front of him before turning to the visitor and the audit question. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No patient count or diagnosis is supplied. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the physical setup tied to the current encounter. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 003 — Dentist chairs pulled out for cots

The overflow clinic is in a former dentist’s row because the real one filled in the first winter. Chairs have been pulled out for cots. Tomas finishes the work in front of him before turning to the visitor and the audit question. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “The room was full before the form arrived. I treated the people who were here.”
Other voice: “He came in before the paper did. I can tell you that much.”
Tomas Geret: “There is not room for a third chair. We will do the sums standing.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No patient count or diagnosis is supplied. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — Dentist chairs pulled out for cots

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The overflow clinic is in a former dentist’s row because the real one filled in the first winter. Chairs have been pulled out for cots. Tomas finishes the work in front of him before turning to the visitor and the audit question. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Keep the physical setup tied to the current encounter. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No patient count or diagnosis is supplied.

Tomas Geret may say: “The room was full before the form arrived. I treated the people who were here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Clinic note: Former dentist-row chairs pulled out for cots. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — The first two ledgers are known

Two ledgers are the ones the Garrison knows about. Tomas can set them side by side without making the third appear as a reveal. The scene holds on the fact that official visibility and complete accounting are not the same thing. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “Those are the two they know. That is not the same as saying they contain every morning.”
Other voice: “I remember two names on the board. The third line stayed empty.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not imply a third ledger’s full contents. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only the source’s two-known/one-not distinction. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — The first two ledgers are known

Proposed diegetic text: “Table note: Two ledgers known to the Garrison; comparison remains open.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Two ledgers are the ones the Garrison knows about. Tomas can set them side by side without making the third appear as a reveal. The scene holds on the fact that official visibility and complete accounting are not the same thing. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not imply a third ledger’s full contents. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only the source’s two-known/one-not distinction. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 007 — The first two ledgers are known

Two ledgers are the ones the Garrison knows about. Tomas can set them side by side without making the third appear as a reveal. The scene holds on the fact that official visibility and complete accounting are not the same thing. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “Those are the two they know. That is not the same as saying they contain every morning.”
Other voice: “I remember two names on the board. The third line stayed empty.”
Tomas Geret: “Two entries match. One is still waiting for a person to tell us what the page missed.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not imply a third ledger’s full contents. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — The first two ledgers are known

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. Two ledgers are the ones the Garrison knows about. Tomas can set them side by side without making the third appear as a reveal. The scene holds on the fact that official visibility and complete accounting are not the same thing. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only the source’s two-known/one-not distinction. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not imply a third ledger’s full contents.

Tomas Geret may say: “Those are the two they know. That is not the same as saying they contain every morning.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Table note: Two ledgers known to the Garrison; comparison remains open. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The third ledger is named first

The encounter says Tomas tells the visitor about the third ledger before they find it. The prose honors that order: he gives the fact, not the hiding place or a guided search. He has not asked the player to steal or publish it. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “I would rather tell you it exists than have you find it and decide I lied.”
Other voice: “The queue was already at the door when the form arrived.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No hiding place, theft, or full ledger excerpt. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Do not turn the ledger into a collectible or investigation task. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The third ledger is named first

Proposed diegetic text: “Inventory note: Third ledger acknowledged by Tomas; contents not copied here.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The encounter says Tomas tells the visitor about the third ledger before they find it. The prose honors that order: he gives the fact, not the hiding place or a guided search. He has not asked the player to steal or publish it. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No hiding place, theft, or full ledger excerpt. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not turn the ledger into a collectible or investigation task. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 011 — The third ledger is named first

The encounter says Tomas tells the visitor about the third ledger before they find it. The prose honors that order: he gives the fact, not the hiding place or a guided search. He has not asked the player to steal or publish it. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “I would rather tell you it exists than have you find it and decide I lied.”
Other voice: “The queue was already at the door when the form arrived.”
Tomas Geret: “Do not turn the ledger into a collectible or investigation task.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No hiding place, theft, or full ledger excerpt. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The third ledger is named first

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The encounter says Tomas tells the visitor about the third ledger before they find it. The prose honors that order: he gives the fact, not the hiding place or a guided search. He has not asked the player to steal or publish it. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Do not turn the ledger into a collectible or investigation task. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No hiding place, theft, or full ledger excerpt.

Tomas Geret may say: “I would rather tell you it exists than have you find it and decide I lied.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Inventory note: Third ledger acknowledged by Tomas; contents not copied here. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — What arrived is not what was logged

The third ledger tracks the difference between what arrived and what was logged. The draft can show two headings with empty editorial space beneath them; no exact stock, date, or item is added. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “The difference kept matching people who were alive anyway. That is what I know.”
Other voice: “You have the box right. The hour written beside it is wrong.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No quantities, patient names, or supply list. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Preserve the source’s unquantified discrepancy. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — What arrived is not what was logged

Proposed diegetic text: “Ledger headings: Arrived / logged; discrepancy amount omitted.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The third ledger tracks the difference between what arrived and what was logged. The draft can show two headings with empty editorial space beneath them; no exact stock, date, or item is added. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No quantities, patient names, or supply list. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve the source’s unquantified discrepancy. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 015 — What arrived is not what was logged

The third ledger tracks the difference between what arrived and what was logged. The draft can show two headings with empty editorial space beneath them; no exact stock, date, or item is added. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “The difference kept matching people who were alive anyway. That is what I know.”
Other voice: “You have the box right. The hour written beside it is wrong.”
Tomas Geret: “The page is short by something. I do not know what to call it yet.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No quantities, patient names, or supply list. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — What arrived is not what was logged

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The third ledger tracks the difference between what arrived and what was logged. The draft can show two headings with empty editorial space beneath them; no exact stock, date, or item is added. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Preserve the source’s unquantified discrepancy. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No quantities, patient names, or supply list.

Tomas Geret may say: “The difference kept matching people who were alive anyway. That is what I know.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Ledger headings: Arrived / logged; discrepancy amount omitted. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The audit asks another question

The audit is coming and the Garrison asks different questions than patients do. The scene does not make either question stupid; it makes visible that only one of them can be asked in the clinic while a person is waiting. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “A form can wait for an answer. A person standing there cannot always wait with it.”
Other voice: “I can confirm what I saw. Do not ask me to balance your page.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No audit procedure or new authority. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the people-first tension without inventing an audit transcript. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The audit asks another question

Proposed diegetic text: “Audit margin: Record question remains; patient work continues.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The audit is coming and the Garrison asks different questions than patients do. The scene does not make either question stupid; it makes visible that only one of them can be asked in the clinic while a person is waiting. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No audit procedure or new authority. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the people-first tension without inventing an audit transcript. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 019 — The audit asks another question

The audit is coming and the Garrison asks different questions than patients do. The scene does not make either question stupid; it makes visible that only one of them can be asked in the clinic while a person is waiting. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “A form can wait for an answer. A person standing there cannot always wait with it.”
Other voice: “I can confirm what I saw. Do not ask me to balance your page.”
Tomas Geret: “Keep the people-first tension without inventing an audit transcript.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No audit procedure or new authority. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The audit asks another question

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The audit is coming and the Garrison asks different questions than patients do. The scene does not make either question stupid; it makes visible that only one of them can be asked in the clinic while a person is waiting. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Keep the people-first tension without inventing an audit transcript. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No audit procedure or new authority.

Tomas Geret may say: “A form can wait for an answer. A person standing there cannot always wait with it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Audit margin: Record question remains; patient work continues. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — Vouching names the mornings

Before `tomas_vouch`, the player hears the actual claim: the rounding bought mornings and those mornings were real. Tomas does not ask the player to swear that every entry is correct. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “You are not vouching for my handwriting. You are vouching that the mornings happened.”
Other voice: “I said he came that morning. I did not say I saw what left the shelf.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not turn a vouch into a blanket audit guarantee. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use before the existing vouch choice. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — Vouching names the mornings

Proposed diegetic text: “Witness note: Player may vouch for the mornings the rounding bought.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Before `tomas_vouch`, the player hears the actual claim: the rounding bought mornings and those mornings were real. Tomas does not ask the player to swear that every entry is correct. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not turn a vouch into a blanket audit guarantee. Do not let the form claim authority that its keeper has not been given.

Later reading: Use before the existing vouch choice. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 023 — Vouching names the mornings

Before `tomas_vouch`, the player hears the actual claim: the rounding bought mornings and those mornings were real. Tomas does not ask the player to swear that every entry is correct. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “You are not vouching for my handwriting. You are vouching that the mornings happened.”
Other voice: “I said he came that morning. I did not say I saw what left the shelf.”
Tomas Geret: “You saw the morning. Tell them only what you can stand behind.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not turn a vouch into a blanket audit guarantee. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — Vouching names the mornings

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. Before `tomas_vouch`, the player hears the actual claim: the rounding bought mornings and those mornings were real. Tomas does not ask the player to swear that every entry is correct. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use before the existing vouch choice. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not turn a vouch into a blanket audit guarantee.

Tomas Geret may say: “You are not vouching for my handwriting. You are vouching that the mornings happened.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Witness note: Player may vouch for the mornings the rounding bought. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — A word held in a room he cannot enter

After the vouch path, the player’s word holds in a room Tomas cannot enter. The clinic keeps its stock, the Garrison keeps its form, and the discrepancy gets a line item instead of a charge. The prose does not name the room or its members. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “Your word held there. I am not going to pretend I heard every part of it.”
Other voice: “I gave them my words. The rest was theirs to decide.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No hearing details or stock amount. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only under `tomas_vouch` and its branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — A word held in a room he cannot enter

Proposed diegetic text: “Outcome line: Discrepancy recorded as a line item rather than a charge.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the vouch path, the player’s word holds in a room Tomas cannot enter. The clinic keeps its stock, the Garrison keeps its form, and the discrepancy gets a line item instead of a charge. The prose does not name the room or its members. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No hearing details or stock amount. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only under `tomas_vouch` and its branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 027 — A word held in a room he cannot enter

After the vouch path, the player’s word holds in a room Tomas cannot enter. The clinic keeps its stock, the Garrison keeps its form, and the discrepancy gets a line item instead of a charge. The prose does not name the room or its members. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “Your word held there. I am not going to pretend I heard every part of it.”
Other voice: “I gave them my words. The rest was theirs to decide.”
Tomas Geret: “You stood behind what you saw, not every mark I made.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No hearing details or stock amount. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — A word held in a room he cannot enter

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. After the vouch path, the player’s word holds in a room Tomas cannot enter. The clinic keeps its stock, the Garrison keeps its form, and the discrepancy gets a line item instead of a charge. The prose does not name the room or its members. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only under `tomas_vouch` and its branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No hearing details or stock amount.

Tomas Geret may say: “Your word held there. I am not going to pretend I heard every part of it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Outcome line: Discrepancy recorded as a line item rather than a charge. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — He writes down what he owes

In evolved Ledger Season, Tomas has started writing down what he owes the player, which for him is a form of hope. The proposed note can remain in his hand without becoming a financial debt or a new reward ledger. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “I write it down because I do not want memory to do all the work.”
Other voice: “He asked me to repeat it twice. He was trying to get it right.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new debt, currency, or payment. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on the evolved vouch state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — He writes down what he owes

Proposed diegetic text: “Personal margin: Acknowledgment of help; no amount or repayment date.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In evolved Ledger Season, Tomas has started writing down what he owes the player, which for him is a form of hope. The proposed note can remain in his hand without becoming a financial debt or a new reward ledger. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new debt, currency, or payment. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on the evolved vouch state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 031 — He writes down what he owes

In evolved Ledger Season, Tomas has started writing down what he owes the player, which for him is a form of hope. The proposed note can remain in his hand without becoming a financial debt or a new reward ledger. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “I write it down because I do not want memory to do all the work.”
Other voice: “He asked me to repeat it twice. He was trying to get it right.”
Tomas Geret: “You told them what you knew. I will not ask you to add more.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new debt, currency, or payment. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — He writes down what he owes

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. In evolved Ledger Season, Tomas has started writing down what he owes the player, which for him is a form of hope. The proposed note can remain in his hand without becoming a financial debt or a new reward ledger. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Condition on the evolved vouch state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new debt, currency, or payment.

Tomas Geret may say: “I write it down because I do not want memory to do all the work.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Personal margin: Acknowledgment of help; no amount or repayment date. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The stock stays; the form stays

The late-cleared state says the clinic keeps its stock and the Garrison keeps its form. A return image gives each object a place on the table and refuses to make one erase the other. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “We kept the supplies. They kept the paper. Both facts fit here.”
Other voice: “The supplies were there. I never said the tally explained them.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new inventory or legal ruling. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only for `late_cleared`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The stock stays; the form stays

Proposed diegetic text: “Late desk: Clinic stock retained; Garrison form remains.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The late-cleared state says the clinic keeps its stock and the Garrison keeps its form. A return image gives each object a place on the table and refuses to make one erase the other. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new inventory or legal ruling. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only for `late_cleared`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 035 — The stock stays; the form stays

The late-cleared state says the clinic keeps its stock and the Garrison keeps its form. A return image gives each object a place on the table and refuses to make one erase the other. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “We kept the supplies. They kept the paper. Both facts fit here.”
Other voice: “The supplies were there. I never said the tally explained them.”
Tomas Geret: “The forms move faster now. The room still needs the same hands.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new inventory or legal ruling. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The stock stays; the form stays

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The late-cleared state says the clinic keeps its stock and the Garrison keeps its form. A return image gives each object a place on the table and refuses to make one erase the other. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only for `late_cleared`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new inventory or legal ruling.

Tomas Geret may say: “We kept the supplies. They kept the paper. Both facts fit here.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late desk: Clinic stock retained; Garrison form remains. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — A line item is not an acquittal

The discrepancy receives a line item instead of a charge. Tomas can say that without declaring himself cleared of every question. The phrase preserves accountability while retaining the outcome’s distinction. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “A line item is not a clean conscience. It is the form we reached.”
Other voice: “He brought the page back when he found the mismatch.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No extra charge, punishment, or total exoneration. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep within the vouch branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — A line item is not an acquittal

Proposed diegetic text: “Audit note: Discrepancy listed as an item; charge not filed under current outcome.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The discrepancy receives a line item instead of a charge. Tomas can say that without declaring himself cleared of every question. The phrase preserves accountability while retaining the outcome’s distinction. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No extra charge, punishment, or total exoneration. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep within the vouch branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 039 — A line item is not an acquittal

The discrepancy receives a line item instead of a charge. Tomas can say that without declaring himself cleared of every question. The phrase preserves accountability while retaining the outcome’s distinction. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “A line item is not a clean conscience. It is the form we reached.”
Other voice: “He brought the page back when he found the mismatch.”
Tomas Geret: “That was your account. Mine belongs beside it, not over it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No extra charge, punishment, or total exoneration. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — A line item is not an acquittal

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. The discrepancy receives a line item instead of a charge. Tomas can say that without declaring himself cleared of every question. The phrase preserves accountability while retaining the outcome’s distinction. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Keep within the vouch branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No extra charge, punishment, or total exoneration.

Tomas Geret may say: “A line item is not a clean conscience. It is the form we reached.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Audit note: Discrepancy listed as an item; charge not filed under current outcome. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — Charges fit entries and miss mornings

If `tomas_testify` is chosen, the charges fit the entries and miss the mornings. The prose does not accuse the testimony of being false; it lets the gap between a form’s categories and the clinic’s work remain painful. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “You told them what the entries say. They did not ask what the mornings were.”
Other voice: “I know who waited with me. I do not know which form they counted.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not invent a false charge or reverse testimony. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only on the testimony branch. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — Charges fit entries and miss mornings

Proposed diegetic text: “Testimony summary: Charges fit entries; mornings are not counted by procedure.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `tomas_testify` is chosen, the charges fit the entries and miss the mornings. The prose does not accuse the testimony of being false; it lets the gap between a form’s categories and the clinic’s work remain painful. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent a false charge or reverse testimony. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only on the testimony branch. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 043 — Charges fit entries and miss mornings

If `tomas_testify` is chosen, the charges fit the entries and miss the mornings. The prose does not accuse the testimony of being false; it lets the gap between a form’s categories and the clinic’s work remain painful. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “You told them what the entries say. They did not ask what the mornings were.”
Other voice: “I know who waited with me. I do not know which form they counted.”
Tomas Geret: “They heard what you saw. They did not ask you to speak for what you could not see.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent a false charge or reverse testimony. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — Charges fit entries and miss mornings

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. If `tomas_testify` is chosen, the charges fit the entries and miss the mornings. The prose does not accuse the testimony of being false; it lets the gap between a form’s categories and the clinic’s work remain painful. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only on the testimony branch. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent a false charge or reverse testimony.

Tomas Geret may say: “You told them what the entries say. They did not ask what the mornings were.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Testimony summary: Charges fit entries; mornings are not counted by procedure. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — The queue waits outside

In `late_accused`, the overflow queue waits outside a clinic with nobody willing to sign for the stock. The view stays with the door and the line, not a named person denied care. No one is made to perform panic for the player. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “I can tell you who is waiting. I cannot sign what nobody will take responsibility for.”
Other voice: “Do not mark me as a witness. I was outside with the others.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No patient fate, diagnosis, or stock handover is added. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Condition on `late_accused`. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — The queue waits outside

Proposed diegetic text: “Late clinic note: Queue outside; stock has no willing signer.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_accused`, the overflow queue waits outside a clinic with nobody willing to sign for the stock. The view stays with the door and the line, not a named person denied care. No one is made to perform panic for the player. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No patient fate, diagnosis, or stock handover is added. Do not let the form claim authority that its keeper has not been given.

Later reading: Condition on `late_accused`. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 047 — The queue waits outside

In `late_accused`, the overflow queue waits outside a clinic with nobody willing to sign for the stock. The view stays with the door and the line, not a named person denied care. No one is made to perform panic for the player. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “I can tell you who is waiting. I cannot sign what nobody will take responsibility for.”
Other voice: “Do not mark me as a witness. I was outside with the others.”
Tomas Geret: “They have accused me. I have not changed a number to answer them.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No patient fate, diagnosis, or stock handover is added. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — The queue waits outside

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. In `late_accused`, the overflow queue waits outside a clinic with nobody willing to sign for the stock. The view stays with the door and the line, not a named person denied care. No one is made to perform panic for the player. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Condition on `late_accused`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No patient fate, diagnosis, or stock handover is added.

Tomas Geret may say: “I can tell you who is waiting. I cannot sign what nobody will take responsibility for.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late clinic note: Queue outside; stock has no willing signer. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — A death record stays exact

Tomas will not falsify a death record. A proposed scene can show him leave a completed record untouched after the audit discussion. The boundary is ethical and factual; there is no sensational description of a patient. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “I will not make a record say someone lived when they did not.”
Other voice: “You do not need to tell the queue we are checking the book again.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No patient identity or death detail. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Preserve this rule in every outcome. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — A death record stays exact

Proposed diegetic text: “Record note: Death entry not altered.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Tomas will not falsify a death record. A proposed scene can show him leave a completed record untouched after the audit discussion. The boundary is ethical and factual; there is no sensational description of a patient. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No patient identity or death detail. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve this rule in every outcome. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 051 — A death record stays exact

Tomas will not falsify a death record. A proposed scene can show him leave a completed record untouched after the audit discussion. The boundary is ethical and factual; there is no sensational description of a patient. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “I will not make a record say someone lived when they did not.”
Other voice: “You do not need to tell the queue we are checking the book again.”
Tomas Geret: “Preserve this rule in every outcome.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No patient identity or death detail. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — A death record stays exact

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. Tomas will not falsify a death record. A proposed scene can show him leave a completed record untouched after the audit discussion. The boundary is ethical and factual; there is no sensational description of a patient. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Preserve this rule in every outcome. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No patient identity or death detail.

Tomas Geret may say: “I will not make a record say someone lived when they did not.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Record note: Death entry not altered. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — Treatment first, reconciliation later

In the initial and late-unmet states, he treats first and reconciles later. The proposed dialogue refuses to frame that order as a cheerful habit; it is the only order he believes has worked while the queue waits. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “The ledger can have its turn when the room has had its turn.”
Other voice: “He did what he could with the room in front of him.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No treatment method or extra clinic capacity. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only while current state remains initial or late-unmet. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — Treatment first, reconciliation later

Proposed diegetic text: “Shift note: Treatment precedes reconciliation in the current account.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the initial and late-unmet states, he treats first and reconciles later. The proposed dialogue refuses to frame that order as a cheerful habit; it is the only order he believes has worked while the queue waits. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No treatment method or extra clinic capacity. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only while current state remains initial or late-unmet. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 055 — Treatment first, reconciliation later

In the initial and late-unmet states, he treats first and reconciles later. The proposed dialogue refuses to frame that order as a cheerful habit; it is the only order he believes has worked while the queue waits. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “The ledger can have its turn when the room has had its turn.”
Other voice: “He did what he could with the room in front of him.”
Tomas Geret: “We have not closed the question. Do not let an empty line pretend we have.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No treatment method or extra clinic capacity. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — Treatment first, reconciliation later

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. In the initial and late-unmet states, he treats first and reconciles later. The proposed dialogue refuses to frame that order as a cheerful habit; it is the only order he believes has worked while the queue waits. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only while current state remains initial or late-unmet. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No treatment method or extra clinic capacity.

Tomas Geret may say: “The ledger can have its turn when the room has had its turn.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Shift note: Treatment precedes reconciliation in the current account. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — The arithmetic has no patient names

A visitor asks to see who matched the discrepancy. Tomas keeps patient names out of the conversation and returns to the ledger headings. The scene does not identify a person by a cot, morning, or amount. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “You do not need their names to understand the difference.”
Other voice: “I remember faces. I am not writing their names on a page you leave here.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not disclose or infer patient identities. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Maintain confidentiality in every line and return. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — The arithmetic has no patient names

Proposed diegetic text: “Privacy margin: No patient names copied into the audit note.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A visitor asks to see who matched the discrepancy. Tomas keeps patient names out of the conversation and returns to the ledger headings. The scene does not identify a person by a cot, morning, or amount. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not disclose or infer patient identities. Do not let the form claim authority that its keeper has not been given.

Later reading: Maintain confidentiality in every line and return. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 059 — The arithmetic has no patient names

A visitor asks to see who matched the discrepancy. Tomas keeps patient names out of the conversation and returns to the ledger headings. The scene does not identify a person by a cot, morning, or amount. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “You do not need their names to understand the difference.”
Other voice: “I remember faces. I am not writing their names on a page you leave here.”
Tomas Geret: “Maintain confidentiality in every line and return.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not disclose or infer patient identities. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — The arithmetic has no patient names

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. A visitor asks to see who matched the discrepancy. Tomas keeps patient names out of the conversation and returns to the ledger headings. The scene does not identify a person by a cot, morning, or amount. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Maintain confidentiality in every line and return. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not disclose or infer patient identities.

Tomas Geret may say: “You do not need their names to understand the difference.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Privacy margin: No patient names copied into the audit note. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — Training without an invented credential

Tomas offers first-aid training. A proposed line can show a learner listening and asking whether to repeat a term, without assigning them clinical authority or a license absent from the data. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “Knowing a little more does not make you the person who signs for this room.”
Other voice: “If he is teaching now, maybe the new hands will not wait for an empty room.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No medical procedure, certification, or substitute medic role. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep this an offer, not an extra quest reward. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — Training without an invented credential

Proposed diegetic text: “Training note: First-aid lesson offered; no credential or role assigned.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Tomas offers first-aid training. A proposed line can show a learner listening and asking whether to repeat a term, without assigning them clinical authority or a license absent from the data. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No medical procedure, certification, or substitute medic role. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep this an offer, not an extra quest reward. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 063 — Training without an invented credential

Tomas offers first-aid training. A proposed line can show a learner listening and asking whether to repeat a term, without assigning them clinical authority or a license absent from the data. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “Knowing a little more does not make you the person who signs for this room.”
Other voice: “If he is teaching now, maybe the new hands will not wait for an empty room.”
Tomas Geret: “You can look at the book here. I cannot promise you anything beyond the page.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No medical procedure, certification, or substitute medic role. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — Training without an invented credential

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. Tomas offers first-aid training. A proposed line can show a learner listening and asking whether to repeat a term, without assigning them clinical authority or a license absent from the data. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Keep this an offer, not an extra quest reward. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No medical procedure, certification, or substitute medic role.

Tomas Geret may say: “Knowing a little more does not make you the person who signs for this room.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Training note: First-aid lesson offered; no credential or role assigned. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — Reconciled after the room empties

If the current state permits a quiet later image, Tomas reconciles the day after the room has emptied. The page does not announce a perfect match. His fatigue appears in a repeated calculation, not a newly invented diagnosis or breakdown. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “I can do the sum again. I cannot give the morning back to the page.”
Other voice: “I saw the room after. He had finally stopped writing.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No insomnia diagnosis or new outcome. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only as non-branch-specific texture when the current content allows. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — Reconciled after the room empties

Proposed diegetic text: “End-of-shift note: Reconciliation deferred until clinic work allows.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If the current state permits a quiet later image, Tomas reconciles the day after the room has emptied. The page does not announce a perfect match. His fatigue appears in a repeated calculation, not a newly invented diagnosis or breakdown. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No insomnia diagnosis or new outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only as non-branch-specific texture when the current content allows. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 067 — Reconciled after the room empties

If the current state permits a quiet later image, Tomas reconciles the day after the room has emptied. The page does not announce a perfect match. His fatigue appears in a repeated calculation, not a newly invented diagnosis or breakdown. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “I can do the sum again. I cannot give the morning back to the page.”
Other voice: “I saw the room after. He had finally stopped writing.”
Tomas Geret: “I can finish this page later. I could not ask that person to wait.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No insomnia diagnosis or new outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — Reconciled after the room empties

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. If the current state permits a quiet later image, Tomas reconciles the day after the room has emptied. The page does not announce a perfect match. His fatigue appears in a repeated calculation, not a newly invented diagnosis or breakdown. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Use only as non-branch-specific texture when the current content allows. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No insomnia diagnosis or new outcome.

Tomas Geret may say: “I can do the sum again. I cannot give the morning back to the page.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: End-of-shift note: Reconciliation deferred until clinic work allows. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — The form answers fast now

In `late_cleared`, the Garrison has learned which forms Tomas answers fast. The prose leaves the forms unnamed and treats speed as a practical relationship, not a new administrative skill. He trains medics for three neighborhoods, as the state says. Begin with the work already under way, before the visitor is asked to decide what it means. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Tomas Geret: “The form comes quicker now. The work did not become smaller.”
Other voice: “He remembers the form numbers. I remember the wait.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No additional neighborhood count or form list. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Show only with the late-cleared state. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — The form answers fast now

Proposed diegetic text: “Late training note: Overflow clinic trains medics for three neighborhoods.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_cleared`, the Garrison has learned which forms Tomas answers fast. The prose leaves the forms unnamed and treats speed as a practical relationship, not a new administrative skill. He trains medics for three neighborhoods, as the state says. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No additional neighborhood count or form list. Do not let the form claim authority that its keeper has not been given.

Later reading: Show only with the late-cleared state. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 071 — The form answers fast now

In `late_cleared`, the Garrison has learned which forms Tomas answers fast. The prose leaves the forms unnamed and treats speed as a practical relationship, not a new administrative skill. He trains medics for three neighborhoods, as the state says. Let Tomas Geret speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Tomas Geret: “The form comes quicker now. The work did not become smaller.”
Other voice: “He remembers the form numbers. I remember the wait.”
Tomas Geret: “They cleared the account. The queue is still waiting.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No additional neighborhood count or form list. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — The form answers fast now

On a later visit permitted by the exact existing Tomas choice and the corresponding initial, evolved, late-cleared, late-accused, or late-unmet NPC state, the player may notice what the earlier choice left in view. In `late_cleared`, the Garrison has learned which forms Tomas answers fast. The prose leaves the forms unnamed and treats speed as a practical relationship, not a new administrative skill. He trains medics for three neighborhoods, as the state says. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Tomas is composed after doing the hard arithmetic three times. He treats before he explains and does not use a patient as evidence. His language distinguishes an entry from a morning, stock from a person, and testimony from a verdict. He will talk about the audit without falsifying what happened.

Observable return: Show only with the late-cleared state. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No additional neighborhood count or form list.

Tomas Geret may say: “The form comes quicker now. The work did not become smaller.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late training note: Overflow clinic trains medics for three neighborhoods. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- The room was full before the form arrived. I treated the people who were here.
- Those are the two they know. That is not the same as saying they contain every morning.
- I would rather tell you it exists than have you find it and decide I lied.
- The difference kept matching people who were alive anyway. That is what I know.
- A form can wait for an answer. A person standing there cannot always wait with it.
- You are not vouching for my handwriting. You are vouching that the mornings happened.
- Your word held there. I am not going to pretend I heard every part of it.
- I write it down because I do not want memory to do all the work.
- We kept the supplies. They kept the paper. Both facts fit here.
- A line item is not a clean conscience. It is the form we reached.
- You told them what the entries say. They did not ask what the mornings were.
- I can tell you who is waiting. I cannot sign what nobody will take responsibility for.
- I will not make a record say someone lived when they did not.
- The ledger can have its turn when the room has had its turn.
- You do not need their names to understand the difference.
- Knowing a little more does not make you the person who signs for this room.
- I can do the sum again. I cannot give the morning back to the page.
- The form comes quicker now. The work did not become smaller.

## 13. Continuity and editorial review

Check `enc_arc_tomas_01_rounding`, `quest_arc_tomas_01_rounding`, and `npc_tomas_geret` state requirements. Preserve the two known Garrison ledgers and the third unexplained ledger, the two current choices, and the exact distinction between late-cleared and late-accused. Do not invent patient names or clinical particulars. Confirm current content before placement. Keep the first-visit premise recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposals only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the current character or settlement content, each callback matches an authored condition, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
