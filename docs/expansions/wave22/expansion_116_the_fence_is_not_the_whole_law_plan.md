# EXPANSION 116 — The Fence Is Not the Whole Law

## Warden Grimm, Tinker’s Notch, and the market peace held between the rule on the gate and the trade that pays its power bill.

### Wave 22: What the Account Cannot Hold

## 1. Expansion thesis

Build a prose expansion from the contradiction already written into Warden Grimm’s settlement profile. He prohibits weapons inside the perimeter and counterfeit chips at stalls, yet his profile says he ignores contraband when it generates enough gate tax. His personal thread concerns a counterfeit battery smuggler whose shorted cells damaged his own secure storage. The plan should expose that tension in optional lines without deciding a new case or adding a Warden quest. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character story or settlement dialogue surface. It adds no gameplay feature and does not claim that proposed text is already in the game.

## 2. Story in one sentence

At a crowded scrap market where the fence is the law everyone agrees on, the warden has to live with the difference between what he prohibits and what he lets pay the bill.

## 3. Verified local anchor and current story

`wasteland_settlement_npcs.json` defines `npc_market_warden_grimm` as Keeper of `settlement_tinkers_notch`, with low, neutral, and high-standing greetings, trade tells, a fear of an internal gang war, a contradiction about profitable contraband, and a personal thread about hunting a counterfeit-battery smuggler. `settlements.json` describes Tinker’s Notch as a free-trader scrap market with neutral arbitration, counterfeit-component tension, an electrified fence, and a power bill paid before anyone asks twice. The settlement names Grimm as keeper. The inspected files provide no registered NPC arc, narrative encounter, or Grimm sidework quest. The existing low-standing greeting asks for two volts or five brass cases at the gate and warns that trouble inside ends at the fence. The neutral greeting prohibits weapons inside and counterfeit chips at stalls. The high-standing greeting asks the player to report crooked deals. Grimm’s sidework quest ID is empty. This plan proposes prose alternatives for existing greeting/settlement text only; any triggered scene, investigation, verdict, reward, or state would need a separately authorized content owner and is not established here. Names, locations, quest IDs, choice IDs, and state summaries above come from current local data. The registered encounter or character record proves the authored premise and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing condition.

## 4. Fixed canon and proposed prose

The settlement has 140 people, open commerce, neutral arbitration, technical innovation, and a tension involving counterfeit components. The fence is described as the only law everyone agrees on; the market pays its power bill first. Grimm’s physical anchor includes stop-sign armor, an electrified prod, and a revolver, but this plan does not stage violence. His contradiction and damaged storage are profile facts; the smuggler’s identity, the fire’s circumstances, the fate of the storage contents, and any arbitration outcome are unknown. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to a character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Grimm keeps a market peace that is useful and compromised. He fears a gang war in crowded stalls; he also takes tax from sales he says should not happen. The prose should let people inside the market notice that contradiction without making any merchant into a disposable suspect or giving the player a clean way to force a confession. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use the existing three standing greetings in `wasteland_settlement_npcs.json` as the only current condition source. The proposed scenes, records, and conversations are editorial alternatives for settlement/NPC text, not a new encounter route. Later vignettes are conditional only on existing low/neutral/high standing being active. No proposed line turns Grimm’s personal thread into a quest or changes another settlement NPC’s role. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character/settlement dialogue owner as the insertion point. Where the inspected data has no such route, keep the text as an editorial proposal and do not imply a new encounter has been approved.

## 8. Player agency and consequence

The inspected current content offers no Grimm choice. Do not imply that the player can arrest a trader, force an arbitration result, seize contraband, or choose a new faction outcome. Keep the player’s role within current settlement interaction: hear the greeting, observe the market, and report a crooked deal only where the existing high-standing text already invites it. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before a choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Keep the market conflict non-instructional and nonviolent. Do not explain counterfeit detection, shorted-cell handling, gate electrical systems, or weapon use. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, electrical, navigation, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when implementation later chooses a smaller, coherent subset.

### Scene draft 001 — The gate speaks before the stalls

At Tinker’s Notch the fence surrounds shipping containers and bus chassis. Grimm’s existing greeting arrives before the visitor reaches the stalls. The scene can let the market noise continue behind the rule without claiming the greeting controls every person’s conduct. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “Welcome to the Notch. You heard the rule before you heard the prices.”
Other voice: “I sell under the fence too. I hear that rule before every price.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add a search or entry procedure. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the greeting within its existing neutral-standing context. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — The gate speaks before the stalls

Proposed diegetic text: “Gate line: No weapons inside the perimeter; counterfeit chips barred at stalls.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At Tinker’s Notch the fence surrounds shipping containers and bus chassis. Grimm’s existing greeting arrives before the visitor reaches the stalls. The scene can let the market noise continue behind the rule without claiming the greeting controls every person’s conduct. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a search or entry procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the greeting within its existing neutral-standing context. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 003 — The gate speaks before the stalls

At Tinker’s Notch the fence surrounds shipping containers and bus chassis. Grimm’s existing greeting arrives before the visitor reaches the stalls. The scene can let the market noise continue behind the rule without claiming the greeting controls every person’s conduct. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “Welcome to the Notch. You heard the rule before you heard the prices.”
Other voice: “I sell under the fence too. I hear that rule before every price.”
Warden Grimm: “Bring both sides to the board. I can listen before I decide what the rule says.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a search or entry procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — The gate speaks before the stalls

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. At Tinker’s Notch the fence surrounds shipping containers and bus chassis. Grimm’s existing greeting arrives before the visitor reaches the stalls. The scene can let the market noise continue behind the rule without claiming the greeting controls every person’s conduct. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep the greeting within its existing neutral-standing context. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a search or entry procedure.

Warden Grimm may say: “Welcome to the Notch. You heard the rule before you heard the prices.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Gate line: No weapons inside the perimeter; counterfeit chips barred at stalls. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — A market pays its power bill

The settlement description says the Notch pays its power bill before anyone asks twice. A proposed notice can show that line beside the open-commerce value without turning it into a new tax schedule. The wording should sound like the settlement speaking, not Grimm taking sole ownership of the decision. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “The lights stay on because someone pays first. Ask the board who decided the order.”
Other voice: “Ask the board why the lights stay on before you ask the stalls to explain.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No exact bill, generator rule, or payment amount. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only as optional settlement-facing prose. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — A market pays its power bill

Proposed diegetic text: “Market notice: Power bill paid before other accounts are called.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement description says the Notch pays its power bill before anyone asks twice. A proposed notice can show that line beside the open-commerce value without turning it into a new tax schedule. The wording should sound like the settlement speaking, not Grimm taking sole ownership of the decision. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No exact bill, generator rule, or payment amount. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only as optional settlement-facing prose. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 007 — A market pays its power bill

The settlement description says the Notch pays its power bill before anyone asks twice. A proposed notice can show that line beside the open-commerce value without turning it into a new tax schedule. The wording should sound like the settlement speaking, not Grimm taking sole ownership of the decision. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “The lights stay on because someone pays first. Ask the board who decided the order.”
Other voice: “Ask the board why the lights stay on before you ask the stalls to explain.”
Warden Grimm: “Bring me the deal as you saw it. The name can wait.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No exact bill, generator rule, or payment amount. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — A market pays its power bill

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The settlement description says the Notch pays its power bill before anyone asks twice. A proposed notice can show that line beside the open-commerce value without turning it into a new tax schedule. The wording should sound like the settlement speaking, not Grimm taking sole ownership of the decision. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Use only as optional settlement-facing prose. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No exact bill, generator rule, or payment amount.

Warden Grimm may say: “The lights stay on because someone pays first. Ask the board who decided the order.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Market notice: Power bill paid before other accounts are called. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The fence is the law everyone agrees on

The settlement says the fence is the only law everyone agrees on. Grimm can repeat that phrase without declaring the fence a complete answer to every dispute in the market. A trader at a container counter may disagree about a component while still accepting the perimeter. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “Everyone knows where the fence is. That does not mean everyone agrees about the stall.”
Other voice: “Everyone agrees on the fence until a cart is late.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new law, jurisdiction, or punishment. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Let this sentence describe the settlement, not a new system. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The fence is the law everyone agrees on

Proposed diegetic text: “Editorial line: Fence named as common rule; other disputes remain open.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement says the fence is the only law everyone agrees on. Grimm can repeat that phrase without declaring the fence a complete answer to every dispute in the market. A trader at a container counter may disagree about a component while still accepting the perimeter. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new law, jurisdiction, or punishment. Do not let the form claim authority that its keeper has not been given.

Later reading: Let this sentence describe the settlement, not a new system. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 011 — The fence is the law everyone agrees on

The settlement says the fence is the only law everyone agrees on. Grimm can repeat that phrase without declaring the fence a complete answer to every dispute in the market. A trader at a container counter may disagree about a component while still accepting the perimeter. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “Everyone knows where the fence is. That does not mean everyone agrees about the stall.”
Other voice: “Everyone agrees on the fence until a cart is late.”
Warden Grimm: “Let this sentence describe the settlement, not a new system.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new law, jurisdiction, or punishment. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The fence is the law everyone agrees on

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The settlement says the fence is the only law everyone agrees on. Grimm can repeat that phrase without declaring the fence a complete answer to every dispute in the market. A trader at a container counter may disagree about a component while still accepting the perimeter. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Let this sentence describe the settlement, not a new system. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new law, jurisdiction, or punishment.

Warden Grimm may say: “Everyone knows where the fence is. That does not mean everyone agrees about the stall.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial line: Fence named as common rule; other disputes remain open. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — Two volts or five brass cases

The low-standing greeting names a gate tithe of two volts or five brass cases. The scene may echo that wording without adding a conversion rate, a negotiation, or a physical inspection of the visitor’s goods. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “That is the toll at this standing. Do not make me bargain with the sign.”
Other voice: “Two volts or five brass cases. Then you may complain about my manners.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not define what a volt is as currency or add other payment forms. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the current low-standing greeting. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — Two volts or five brass cases

Proposed diegetic text: “Low-standing greeting: Two volts or five brass cases at the gate.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The low-standing greeting names a gate tithe of two volts or five brass cases. The scene may echo that wording without adding a conversion rate, a negotiation, or a physical inspection of the visitor’s goods. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not define what a volt is as currency or add other payment forms. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the current low-standing greeting. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 015 — Two volts or five brass cases

The low-standing greeting names a gate tithe of two volts or five brass cases. The scene may echo that wording without adding a conversion rate, a negotiation, or a physical inspection of the visitor’s goods. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “That is the toll at this standing. Do not make me bargain with the sign.”
Other voice: “Two volts or five brass cases. Then you may complain about my manners.”
Warden Grimm: “Two volts or five brass cases. Those are the prices; the gate does not haggle.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not define what a volt is as currency or add other payment forms. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — Two volts or five brass cases

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The low-standing greeting names a gate tithe of two volts or five brass cases. The scene may echo that wording without adding a conversion rate, a negotiation, or a physical inspection of the visitor’s goods. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Use only in the current low-standing greeting. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not define what a volt is as currency or add other payment forms.

Warden Grimm may say: “That is the toll at this standing. Do not make me bargain with the sign.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Low-standing greeting: Two volts or five brass cases at the gate. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — Neutral is not intimate

The neutral greeting welcomes the visitor and states the weapons and counterfeit-chip rules. Grimm is polite without pretending to know the player. The market remains a shared place, not a private favor. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “You can trade here. You do not have to like my rules to read them.”
Other voice: “If he says neutral, let him stand back from the counter and hear both sides.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not add a personal history or relationship flag. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the neutral greeting’s plain boundary. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — Neutral is not intimate

Proposed diegetic text: “Neutral greeting: Welcome; no weapons inside and no counterfeit chips at stalls.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The neutral greeting welcomes the visitor and states the weapons and counterfeit-chip rules. Grimm is polite without pretending to know the player. The market remains a shared place, not a private favor. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add a personal history or relationship flag. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the neutral greeting’s plain boundary. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 019 — Neutral is not intimate

The neutral greeting welcomes the visitor and states the weapons and counterfeit-chip rules. Grimm is polite without pretending to know the player. The market remains a shared place, not a private favor. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “You can trade here. You do not have to like my rules to read them.”
Other voice: “If he says neutral, let him stand back from the counter and hear both sides.”
Warden Grimm: “Keep the neutral greeting’s plain boundary.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add a personal history or relationship flag. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — Neutral is not intimate

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The neutral greeting welcomes the visitor and states the weapons and counterfeit-chip rules. Grimm is polite without pretending to know the player. The market remains a shared place, not a private favor. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep the neutral greeting’s plain boundary. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add a personal history or relationship flag.

Warden Grimm may say: “You can trade here. You do not have to like my rules to read them.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Neutral greeting: Welcome; no weapons inside and no counterfeit chips at stalls. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — A crooked deal is a report, not a verdict

At high standing Grimm asks the player to report crooked deals in the stalls. A proposed exchange can show a visitor deciding whether to speak, but Grimm does not announce guilt before hearing anything. The profile gives him arbitration as a value, not a particular procedure. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “Tell me what you saw. Do not tell me what you want me to call it yet.”
Other voice: “I will tell you the price. I will not tell you what the seller meant.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No formal arbitration procedure or conviction. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only when the existing high-standing greeting is active. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — A crooked deal is a report, not a verdict

Proposed diegetic text: “High-standing note: Crooked deal may be reported; no verdict entered.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At high standing Grimm asks the player to report crooked deals in the stalls. A proposed exchange can show a visitor deciding whether to speak, but Grimm does not announce guilt before hearing anything. The profile gives him arbitration as a value, not a particular procedure. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No formal arbitration procedure or conviction. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only when the existing high-standing greeting is active. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 023 — A crooked deal is a report, not a verdict

At high standing Grimm asks the player to report crooked deals in the stalls. A proposed exchange can show a visitor deciding whether to speak, but Grimm does not announce guilt before hearing anything. The profile gives him arbitration as a value, not a particular procedure. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “Tell me what you saw. Do not tell me what you want me to call it yet.”
Other voice: “I will tell you the price. I will not tell you what the seller meant.”
Warden Grimm: “If the deal looks crooked, bring me the words they used, not a rumor.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No formal arbitration procedure or conviction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — A crooked deal is a report, not a verdict

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. At high standing Grimm asks the player to report crooked deals in the stalls. A proposed exchange can show a visitor deciding whether to speak, but Grimm does not announce guilt before hearing anything. The profile gives him arbitration as a value, not a particular procedure. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Use only when the existing high-standing greeting is active. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No formal arbitration procedure or conviction.

Warden Grimm may say: “Tell me what you saw. Do not tell me what you want me to call it yet.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: High-standing note: Crooked deal may be reported; no verdict entered. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — Counterfeit components make arguments spark

The settlement’s internal tension names counterfeit components sparking disputes between merchant factors. The scene stays with a raised voice and a halted sale, not a fire or an attack. The player can notice that both parties want the market to remain open. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “The argument can wait at the counter. The market does not get to catch fire over a word.”
Other voice: “The chip looks wrong to me. I do not know whose hand brought it here.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not invent an incident, injury, or named factor. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the conflict a stated tension, not a new event. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — Counterfeit components make arguments spark

Proposed diegetic text: “Market margin: Counterfeit-component dispute noted; parties unnamed.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement’s internal tension names counterfeit components sparking disputes between merchant factors. The scene stays with a raised voice and a halted sale, not a fire or an attack. The player can notice that both parties want the market to remain open. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent an incident, injury, or named factor. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the conflict a stated tension, not a new event. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 027 — Counterfeit components make arguments spark

The settlement’s internal tension names counterfeit components sparking disputes between merchant factors. The scene stays with a raised voice and a halted sale, not a fire or an attack. The player can notice that both parties want the market to remain open. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “The argument can wait at the counter. The market does not get to catch fire over a word.”
Other voice: “The chip looks wrong to me. I do not know whose hand brought it here.”
Warden Grimm: “Keep the conflict a stated tension, not a new event.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent an incident, injury, or named factor. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — Counterfeit components make arguments spark

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The settlement’s internal tension names counterfeit components sparking disputes between merchant factors. The scene stays with a raised voice and a halted sale, not a fire or an attack. The player can notice that both parties want the market to remain open. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep the conflict a stated tension, not a new event. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent an incident, injury, or named factor.

Warden Grimm may say: “The argument can wait at the counter. The market does not get to catch fire over a word.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Market margin: Counterfeit-component dispute noted; parties unnamed. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — The warden’s contradiction is visible

Grimm’s profile says he enforces market rules and actively ignores contraband sales if they generate sufficient gate tax. A proposed line can let a merchant notice the gap between the rule and the receipt. It cannot decide which unnamed sale is contraband or add an amount. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “I can hear what the rule says. I can also hear the gate ring.”
Other voice: “He takes the fee and says the rule is clear. The rest of us hear both sentences.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No bribe, amount, seller, or case is established. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the contradiction unresolved in every standing tier. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — The warden’s contradiction is visible

Proposed diegetic text: “Editorial note: Rule and profitable exception coexist in the character profile.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Grimm’s profile says he enforces market rules and actively ignores contraband sales if they generate sufficient gate tax. A proposed line can let a merchant notice the gap between the rule and the receipt. It cannot decide which unnamed sale is contraband or add an amount. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No bribe, amount, seller, or case is established. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the contradiction unresolved in every standing tier. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 031 — The warden’s contradiction is visible

Grimm’s profile says he enforces market rules and actively ignores contraband sales if they generate sufficient gate tax. A proposed line can let a merchant notice the gap between the rule and the receipt. It cannot decide which unnamed sale is contraband or add an amount. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “I can hear what the rule says. I can also hear the gate ring.”
Other voice: “He takes the fee and says the rule is clear. The rest of us hear both sentences.”
Warden Grimm: “Keep the contradiction unresolved in every standing tier.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No bribe, amount, seller, or case is established. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — The warden’s contradiction is visible

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. Grimm’s profile says he enforces market rules and actively ignores contraband sales if they generate sufficient gate tax. A proposed line can let a merchant notice the gap between the rule and the receipt. It cannot decide which unnamed sale is contraband or add an amount. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep the contradiction unresolved in every standing tier. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No bribe, amount, seller, or case is established.

Warden Grimm may say: “I can hear what the rule says. I can also hear the gate ring.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial note: Rule and profitable exception coexist in the character profile. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The secure storage is his own stake

Grimm’s personal thread says a counterfeit-battery smuggler sold shorted cells and ensuing fire damaged Grimm’s secure storage. The prose may show him checking the storage latch once, but cannot name the smuggler or describe the fire. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “That damage was mine. It does not make every seller guilty.”
Other voice: “That storage belonged to him. I saw the mark; I do not know what happened before it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No fire details, loss list, identity, or accusation. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only use as a personal-thread line, not an investigation trigger. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The secure storage is his own stake

Proposed diegetic text: “Keeper note: Storage damaged after shorted cells; culprit unnamed in this proposal.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Grimm’s personal thread says a counterfeit-battery smuggler sold shorted cells and ensuing fire damaged Grimm’s secure storage. The prose may show him checking the storage latch once, but cannot name the smuggler or describe the fire. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No fire details, loss list, identity, or accusation. Do not let the form claim authority that its keeper has not been given.

Later reading: Only use as a personal-thread line, not an investigation trigger. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 035 — The secure storage is his own stake

Grimm’s personal thread says a counterfeit-battery smuggler sold shorted cells and ensuing fire damaged Grimm’s secure storage. The prose may show him checking the storage latch once, but cannot name the smuggler or describe the fire. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “That damage was mine. It does not make every seller guilty.”
Other voice: “That storage belonged to him. I saw the mark; I do not know what happened before it.”
Warden Grimm: “Those cells shorted my storage. I know what they did to my lock; I do not know who carried them in.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No fire details, loss list, identity, or accusation. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The secure storage is his own stake

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. Grimm’s personal thread says a counterfeit-battery smuggler sold shorted cells and ensuing fire damaged Grimm’s secure storage. The prose may show him checking the storage latch once, but cannot name the smuggler or describe the fire. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Only use as a personal-thread line, not an investigation trigger. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No fire details, loss list, identity, or accusation.

Warden Grimm may say: “That damage was mine. It does not make every seller guilty.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Keeper note: Storage damaged after shorted cells; culprit unnamed in this proposal. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — Fear of a gang war stays a fear

Grimm fears an internal gang war in crowded stalls. A return vignette can show him watching two separate groups use the same aisle without claiming that a gang war has begun. The fear matters without needing a body or a battle. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “I am paid to notice how close people stand before they start calling it a side.”
Other voice: “A crowded market can split over less than a name. We have seen it come close.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No gang names, weapons event, or combat encounter. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep the source’s fear hypothetical. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — Fear of a gang war stays a fear

Proposed diegetic text: “Watch note: Crowded stalls observed; no gang conflict recorded.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Grimm fears an internal gang war in crowded stalls. A return vignette can show him watching two separate groups use the same aisle without claiming that a gang war has begun. The fear matters without needing a body or a battle. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No gang names, weapons event, or combat encounter. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the source’s fear hypothetical. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 039 — Fear of a gang war stays a fear

Grimm fears an internal gang war in crowded stalls. A return vignette can show him watching two separate groups use the same aisle without claiming that a gang war has begun. The fear matters without needing a body or a battle. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “I am paid to notice how close people stand before they start calling it a side.”
Other voice: “A crowded market can split over less than a name. We have seen it come close.”
Warden Grimm: “If the stalls split into sides, we will not have room to keep the peace.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No gang names, weapons event, or combat encounter. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — Fear of a gang war stays a fear

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. Grimm fears an internal gang war in crowded stalls. A return vignette can show him watching two separate groups use the same aisle without claiming that a gang war has begun. The fear matters without needing a body or a battle. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep the source’s fear hypothetical. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No gang names, weapons event, or combat encounter.

Warden Grimm may say: “I am paid to notice how close people stand before they start calling it a side.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Watch note: Crowded stalls observed; no gang conflict recorded. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — A test bench belongs to the broker

Solomon’s greeting invites traders to test boards on his bench meter before quoting a price. Grimm’s story can acknowledge that the market has a component broker without taking over the technical trade. The prose gives no diagnostic instructions. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “I keep the peace. Solomon knows what he will buy. Those are not the same job.”
Other voice: “Tess says the bench is hers. Grimm can keep his hands off it.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. Do not transfer the bench or electronics authority to Grimm. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only mention Solomon where current settlement content has him present. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — A test bench belongs to the broker

Proposed diegetic text: “Stall note: Solomon’s bench is the broker’s; Warden does not certify the test.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Solomon’s greeting invites traders to test boards on his bench meter before quoting a price. Grimm’s story can acknowledge that the market has a component broker without taking over the technical trade. The prose gives no diagnostic instructions. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not transfer the bench or electronics authority to Grimm. Do not let the form claim authority that its keeper has not been given.

Later reading: Only mention Solomon where current settlement content has him present. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 043 — A test bench belongs to the broker

Solomon’s greeting invites traders to test boards on his bench meter before quoting a price. Grimm’s story can acknowledge that the market has a component broker without taking over the technical trade. The prose gives no diagnostic instructions. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “I keep the peace. Solomon knows what he will buy. Those are not the same job.”
Other voice: “Tess says the bench is hers. Grimm can keep his hands off it.”
Warden Grimm: “The broker owns the test bench. Let them answer for the parts they sell.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not transfer the bench or electronics authority to Grimm. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — A test bench belongs to the broker

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. Solomon’s greeting invites traders to test boards on his bench meter before quoting a price. Grimm’s story can acknowledge that the market has a component broker without taking over the technical trade. The prose gives no diagnostic instructions. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Only mention Solomon where current settlement content has him present. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not transfer the bench or electronics authority to Grimm.

Warden Grimm may say: “I keep the peace. Solomon knows what he will buy. Those are not the same job.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Stall note: Solomon’s bench is the broker’s; Warden does not certify the test. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — Tess’s repair is not a warden’s case

The settlement assigns Tess the engine-tech fixture and its repeatable alternator salvage quest. A line from Grimm may acknowledge the turbine network without claiming responsibility for Tess’s repair or quest. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “If the turbine is quiet, talk to Tess. I keep the gate, not the wrench.”
Other voice: “That was a repair. Do not turn it into evidence.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new quest or ownership over Tess’s task. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Maintain the existing settlement NPC roles. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — Tess’s repair is not a warden’s case

Proposed diegetic text: “Settlement note: Tess owns the engine-work thread; no Grimm sidework ID.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement assigns Tess the engine-tech fixture and its repeatable alternator salvage quest. A line from Grimm may acknowledge the turbine network without claiming responsibility for Tess’s repair or quest. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new quest or ownership over Tess’s task. Do not let the form claim authority that its keeper has not been given.

Later reading: Maintain the existing settlement NPC roles. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 047 — Tess’s repair is not a warden’s case

The settlement assigns Tess the engine-tech fixture and its repeatable alternator salvage quest. A line from Grimm may acknowledge the turbine network without claiming responsibility for Tess’s repair or quest. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “If the turbine is quiet, talk to Tess. I keep the gate, not the wrench.”
Other voice: “That was a repair. Do not turn it into evidence.”
Warden Grimm: “I keep the gate. The bench belongs to someone else.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new quest or ownership over Tess’s task. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — Tess’s repair is not a warden’s case

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The settlement assigns Tess the engine-tech fixture and its repeatable alternator salvage quest. A line from Grimm may acknowledge the turbine network without claiming responsibility for Tess’s repair or quest. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Maintain the existing settlement NPC roles. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new quest or ownership over Tess’s task.

Warden Grimm may say: “If the turbine is quiet, talk to Tess. I keep the gate, not the wrench.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Settlement note: Tess owns the engine-work thread; no Grimm sidework ID. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — High standing clears the gate, not the market

The high-standing greeting says the Warden gate is clear and asks the player to report crooked deals. A return line can distinguish easy entry from guaranteed fair treatment at every stall. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “You can come through. That does not mean every counter is honest.”
Other voice: “I reported the deal. I did not ask him to make an example of anyone.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No immunity, special market access, or arbitration result. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only under the current high-standing greeting. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — High standing clears the gate, not the market

Proposed diegetic text: “High-standing margin: Gate clear; stall disputes remain reportable.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The high-standing greeting says the Warden gate is clear and asks the player to report crooked deals. A return line can distinguish easy entry from guaranteed fair treatment at every stall. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No immunity, special market access, or arbitration result. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only under the current high-standing greeting. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 051 — High standing clears the gate, not the market

The high-standing greeting says the Warden gate is clear and asks the player to report crooked deals. A return line can distinguish easy entry from guaranteed fair treatment at every stall. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “You can come through. That does not mean every counter is honest.”
Other voice: “I reported the deal. I did not ask him to make an example of anyone.”
Warden Grimm: “You can tell me what happened. I decide what the rule says.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No immunity, special market access, or arbitration result. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — High standing clears the gate, not the market

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The high-standing greeting says the Warden gate is clear and asks the player to report crooked deals. A return line can distinguish easy entry from guaranteed fair treatment at every stall. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Use only under the current high-standing greeting. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No immunity, special market access, or arbitration result.

Warden Grimm may say: “You can come through. That does not mean every counter is honest.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: High-standing margin: Gate clear; stall disputes remain reportable. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — Low standing ends at the fence

The low-standing greeting says trouble inside means leaving through the fence. The prose can preserve the hard boundary without describing an expulsion scene or how anyone enforces it. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “You know the terms. I will not make a show out of repeating them.”
Other voice: “The fence keeps people out. It cannot make them agree.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No force, search, or ban length. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Use only in the current low-standing context. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — Low standing ends at the fence

Proposed diegetic text: “Low-standing margin: Trouble inside ends at the fence, as the current greeting says.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The low-standing greeting says trouble inside means leaving through the fence. The prose can preserve the hard boundary without describing an expulsion scene or how anyone enforces it. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No force, search, or ban length. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only in the current low-standing context. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 055 — Low standing ends at the fence

The low-standing greeting says trouble inside means leaving through the fence. The prose can preserve the hard boundary without describing an expulsion scene or how anyone enforces it. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “You know the terms. I will not make a show out of repeating them.”
Other voice: “The fence keeps people out. It cannot make them agree.”
Warden Grimm: “Read the board before you spend. I will not repeat a price after the trade.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No force, search, or ban length. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — Low standing ends at the fence

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The low-standing greeting says trouble inside means leaving through the fence. The prose can preserve the hard boundary without describing an expulsion scene or how anyone enforces it. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Use only in the current low-standing context. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No force, search, or ban length.

Warden Grimm may say: “You know the terms. I will not make a show out of repeating them.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Low-standing margin: Trouble inside ends at the fence, as the current greeting says. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — A report does not name its source aloud

If a player reports a crooked deal under the existing high-standing invitation, the Warden can acknowledge that the report was heard without revealing who brought it to him. No formal confidentiality guarantee is invented; the text simply does not expose a name. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “I heard you. I do not need to announce your name across the stalls.”
Other voice: “I gave him the report at the gate. He kept my name to himself.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No witness-protection rule or hidden flag. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Only include if the existing dialogue surface accepts such a reply. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — A report does not name its source aloud

Proposed diegetic text: “Keeper margin: Report received; source not written in public-facing note.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If a player reports a crooked deal under the existing high-standing invitation, the Warden can acknowledge that the report was heard without revealing who brought it to him. No formal confidentiality guarantee is invented; the text simply does not expose a name. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No witness-protection rule or hidden flag. Do not let the form claim authority that its keeper has not been given.

Later reading: Only include if the existing dialogue surface accepts such a reply. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 059 — A report does not name its source aloud

If a player reports a crooked deal under the existing high-standing invitation, the Warden can acknowledge that the report was heard without revealing who brought it to him. No formal confidentiality guarantee is invented; the text simply does not expose a name. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “I heard you. I do not need to announce your name across the stalls.”
Other voice: “I gave him the report at the gate. He kept my name to himself.”
Warden Grimm: “If you want to report a deal, say it here. I can listen.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No witness-protection rule or hidden flag. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — A report does not name its source aloud

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. If a player reports a crooked deal under the existing high-standing invitation, the Warden can acknowledge that the report was heard without revealing who brought it to him. No formal confidentiality guarantee is invented; the text simply does not expose a name. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Only include if the existing dialogue surface accepts such a reply. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No witness-protection rule or hidden flag.

Warden Grimm may say: “I heard you. I do not need to announce your name across the stalls.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Keeper margin: Report received; source not written in public-facing note. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — Neutral arbitration is a value, not a shortcut

The settlement lists neutral arbitration as a core value. A market conversation can let Grimm repeat that value while refusing to announce a resolution before both sides have spoken. No new hearing order, board vote, or evidence standard is supplied. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “Neutral does not mean I have already decided you are right.”
Other voice: “Arbitration can hear a dispute. It cannot make the missing piece appear.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No arbitration system, vote, or verdict. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Keep as an optional line, not a new encounter. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — Neutral arbitration is a value, not a shortcut

Proposed diegetic text: “Board note: Neutral arbitration named as a value; no ruling entered.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement lists neutral arbitration as a core value. A market conversation can let Grimm repeat that value while refusing to announce a resolution before both sides have spoken. No new hearing order, board vote, or evidence standard is supplied. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No arbitration system, vote, or verdict. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep as an optional line, not a new encounter. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 063 — Neutral arbitration is a value, not a shortcut

The settlement lists neutral arbitration as a core value. A market conversation can let Grimm repeat that value while refusing to announce a resolution before both sides have spoken. No new hearing order, board vote, or evidence standard is supplied. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “Neutral does not mean I have already decided you are right.”
Other voice: “Arbitration can hear a dispute. It cannot make the missing piece appear.”
Warden Grimm: “The stalls are open. That does not make every bargain fair.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No arbitration system, vote, or verdict. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — Neutral arbitration is a value, not a shortcut

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The settlement lists neutral arbitration as a core value. A market conversation can let Grimm repeat that value while refusing to announce a resolution before both sides have spoken. No new hearing order, board vote, or evidence standard is supplied. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Keep as an optional line, not a new encounter. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No arbitration system, vote, or verdict.

Warden Grimm may say: “Neutral does not mean I have already decided you are right.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Board note: Neutral arbitration named as a value; no ruling entered. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — The power bill does not erase the rule

The Notch’s need to pay for power helps explain why profitable commerce matters, but the prose should not excuse every contraband sale as necessary. The keeper can look at the market lights and return to the posted rule without reconciling the contradiction for the player. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “One account does not erase the other. I can read both signs.”
Other voice: “The bill gets paid. The rule still has to fit the people beneath the lights.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No new tax or revenue mechanic. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Preserve both facts from `settlements.json` and Grimm’s profile. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — The power bill does not erase the rule

Proposed diegetic text: “Notice margin: Power bill remains due; gate rule remains posted.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The Notch’s need to pay for power helps explain why profitable commerce matters, but the prose should not excuse every contraband sale as necessary. The keeper can look at the market lights and return to the posted rule without reconciling the contradiction for the player. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new tax or revenue mechanic. Do not let the form claim authority that its keeper has not been given.

Later reading: Preserve both facts from `settlements.json` and Grimm’s profile. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 067 — The power bill does not erase the rule

The Notch’s need to pay for power helps explain why profitable commerce matters, but the prose should not excuse every contraband sale as necessary. The keeper can look at the market lights and return to the posted rule without reconciling the contradiction for the player. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “One account does not erase the other. I can read both signs.”
Other voice: “The bill gets paid. The rule still has to fit the people beneath the lights.”
Warden Grimm: “The fence goes around everybody. The bill gets handed to somebody.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new tax or revenue mechanic. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — The power bill does not erase the rule

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. The Notch’s need to pay for power helps explain why profitable commerce matters, but the prose should not excuse every contraband sale as necessary. The keeper can look at the market lights and return to the posted rule without reconciling the contradiction for the player. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Preserve both facts from `settlements.json` and Grimm’s profile. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new tax or revenue mechanic.

Warden Grimm may say: “One account does not erase the other. I can read both signs.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Notice margin: Power bill remains due; gate rule remains posted. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 069 — No culprit is awarded to curiosity

A visitor asks whether the counterfeit-battery smuggler has been found. The profile says Grimm is hunting for the person; it does not give a name or a current result. The reply can be a refusal to turn suspicion into a public accusation. Begin with the work already under way, before the visitor is asked to decide what it means. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: a relay key lifted, a ledger page settling, a motor ticking cool, snowshoes waiting by the gate, or market talk losing its edge.

Warden Grimm: “I am looking. That is not the same as having found the right person.”
Other voice: “If he has not named anyone, maybe he does not know. Let that stay the truth for now.”

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the current content. No culprit, arrest, or quest completion. The pause after the line may hold more than a speech could: a hand stays beside a page, a machine completes its stated hour, the next person waits at the fence, or the distant band remains silent.

Scene close: Do not imply progress beyond the authored personal thread. Keep the final action with the person who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 070 — No culprit is awarded to curiosity

Proposed diegetic text: “Thread note: Smuggler still unnamed in the inspected profile; no result recorded here.” Treat this as a compact in-world object: a log margin, page copy, schedule, field note, or short gate notice. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A visitor asks whether the counterfeit-battery smuggler has been found. The profile says Grimm is hunting for the person; it does not give a name or a current result. The reply can be a refusal to turn suspicion into a public accusation. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No culprit, arrest, or quest completion. Do not let the form claim authority that its keeper has not been given.

Later reading: Do not imply progress beyond the authored personal thread. A changed copy should name what changed and who changed it only where current content supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative or settlement-content owner.

### Conversation fragment 071 — No culprit is awarded to curiosity

A visitor asks whether the counterfeit-battery smuggler has been found. The profile says Grimm is hunting for the person; it does not give a name or a current result. The reply can be a refusal to turn suspicion into a public accusation. Let Warden Grimm speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Warden Grimm: “I am looking. That is not the same as having found the right person.”
Other voice: “If he has not named anyone, maybe he does not know. Let that stay the truth for now.”
Warden Grimm: “The storage was mine. I have nothing else about that seller to give you.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No culprit, arrest, or quest completion. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 072 — No culprit is awarded to curiosity

On a later visit permitted by the current low, neutral, or high-standing greeting for Warden Grimm; no NPC arc state or quest branch is registered, the player may notice what the earlier choice left in view. A visitor asks whether the counterfeit-battery smuggler has been found. The profile says Grimm is hunting for the person; it does not give a name or a current result. The reply can be a refusal to turn suspicion into a public accusation. Do not display this passage as a universal epilogue: the corresponding existing choice or content condition must already be true. Grimm speaks in concise gate terms: who enters, what cannot be done inside, and what to report if a deal is crooked. The low-standing greeting is stern, the neutral one is plain, and the high-standing one is respectful. He does not explain away his contradiction, and he does not confess a new crime to make the player feel perceptive.

Observable return: Do not imply progress beyond the authored personal thread. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No culprit, arrest, or quest completion.

Warden Grimm may say: “I am looking. That is not the same as having found the right person.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Thread note: Smuggler still unnamed in the inspected profile; no result recorded here. Keep every returning detail inside the condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- Welcome to the Notch. You heard the rule before you heard the prices.
- The lights stay on because someone pays first. Ask the board who decided the order.
- Everyone knows where the fence is. That does not mean everyone agrees about the stall.
- That is the toll at this standing. Do not make me bargain with the sign.
- You can trade here. You do not have to like my rules to read them.
- Tell me what you saw. Do not tell me what you want me to call it yet.
- The argument can wait at the counter. The market does not get to catch fire over a word.
- I can hear what the rule says. I can also hear the gate ring.
- That damage was mine. It does not make every seller guilty.
- I am paid to notice how close people stand before they start calling it a side.
- I keep the peace. Solomon knows what he will buy. Those are not the same job.
- If the turbine is quiet, talk to Tess. I keep the gate, not the wrench.
- You can come through. That does not mean every counter is honest.
- You know the terms. I will not make a show out of repeating them.
- I heard you. I do not need to announce your name across the stalls.
- Neutral does not mean I have already decided you are right.
- One account does not erase the other. I can read both signs.
- I am looking. That is not the same as having found the right person.

## 13. Continuity and editorial review

Recheck the Tinker’s Notch entry in `settlements.json` and Grimm’s profile in `wasteland_settlement_npcs.json`. Keep his three greeting tiers, neutrality claim, counterfeit-component tension, contradiction, and storage-damage personal thread. Do not create a smuggler identity, a fire account, an arbitration case, an encounter ID, or a sidework quest. Confirm current content before placement. Keep the first-visit premise recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposals only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the current character or settlement content, each callback matches an authored condition, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
