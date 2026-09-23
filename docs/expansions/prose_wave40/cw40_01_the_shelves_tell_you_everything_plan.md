# EXPANSION CW40-01 — The Shelves Tell You Everything

## A prose-first location story about people, memory, and what the record cannot settle.

### Prose Wave 40: The Rooms That Kept Their Accounts

## Batch brief

**Content type:** original game-content proposal with scene, diegetic-record, conversation, and conditional-return drafts.
**Content bank:** 48 distinct beat pairings, each with four alternative authored forms (192 candidate passages).
**Current local anchor:** prewar_medical_cache — Pre-War Medical Cache.
**Tone:** grounded, human, restrained, material, and careful about what a damaged record can prove.
**Scope:** game content only; no production code, JSON data, route, quest, flag, simulation, or save changes.

## 1. Expansion thesis

The clinic basement is a small room carrying several kinds of urgency: antibiotics kept in foil, bandages still rolled, a medical kit with its instruction card, a cult that finds caches first, and two siblings whose distress broadcast is tied to the same location. A recovered Field Hospital Seven tape turns the cache into an echo of older triage, tags, and treatment owed. This prose bank follows a cache keeper, a listener, and a tape reader through the ethics of finding medicine without turning need into a scavenger’s shortcut.

## 2. Story question

When a sealed room preserves medicine and the people who need it arrive without a map, what does care require us to remember?

## 3. Verified local anchor and source records

The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| Assets/StreamingAssets/Data/locations.json | prewar_medical_cache | Sealed clinic basement; 15 rads/hour; antibiotics, bandages, a medical kit, and an instruction card; cults find caches first. |
| Assets/StreamingAssets/Data/expeditions.json | prewar_medical_cache | Current expedition identity, danger, travel, hospital scavenging table, and medical loot categories. |
| Assets/StreamingAssets/Data/moral_choice_quests_distress.json | quest_moral_distress_siblings_clinic | Ana and Miko broadcast from a clinic storm cellar; the authored choices include a secret knock, extraction, or leaving them. |
| Assets/StreamingAssets/Data/cassette_sets.json | field_hospital_7 | Field Hospital Seven cassette set whose hidden cache is this location and whose final tape preserves treatment tags and owed care. |

## 4. Fixed canon and open space

The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. These source statements are boundaries, not prompts to add a route or explain every blank. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.
All new speakers, props, memories, labels, and return passages are editorial drafts. A prose plan does not establish reachability, item placement, a consumer, a trigger, or a new state owner.

## 5. Human center

The emotional center is the difference between finding medicine and becoming responsible for the story attached to it. The cache keeper sees sealed shelves; the distress listener hears two children; the tape reader hears a hospital that counted names twice because counting was one of the few things it could still do.

## 6. Voice and point of view

- **Cache keeper:** The keeper knows the shelves and their scarcity, but refuses to make medical need into a route puzzle.
- **Distress listener:** The listener remembers the broadcast and the moral weight of whether anybody answered.
- **Tape reader:** The reader carries Field Hospital Seven’s tags and limits without inventing a cure or a missing patient.

Each speaker knows only what the cited source, their own proposed observation, or an explicitly attributed memory could give them. No proposed voice represents a whole faction or settlement.

## 7. Placement and current reachability

The source record verifies a content anchor, not a guaranteed playable route or passage consumer. Before selection, confirm current reachability, content schema, owner, and trigger. Candidate passages may be considered only through a verified existing consumer. Every bank entry remains a candidate, not a promise that all 48 beats occur.

## 8. Player agency

The player may read, ask, carry a sentence forward, leave it where it is, or decline to interpret it. These are editorial postures rather than a promised menu. A refusal or silence is a complete outcome. Any branch that needs runtime state must use the existing owner’s exposed state after verification.

## 9. Continuity, dignity, and safety

Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.
Do not make hazard, scarcity, illness, grief, or technical uncertainty into a puzzle tutorial. Do not make a person’s caution a moral failure. Keep proposed identity separate from authored identity and testimony separate from fact.

## 10. Existing hooks and implementation boundary

Any placement must use an existing location, expedition, distress quest, cassette, item, journal, or medical-content consumer. No new treatment system, cache respawn, rescue route, diagnosis authority, cult reputation track, or medical ledger is proposed.
The plan creates no parallel authority, new registry, save section, route graph, generic panel callback, or gameplay rule. A selected passage should attach only through a verified existing content owner.

## 11. Narrative sequence

The six movements below organize an editorial bank; they are not six required visits or a fixed quest chain. Beat order may change if the existing consumer calls for a shorter or non-linear presentation.

### Movement 1: The Basement Before the Knock

A keeper finds preserved supplies and hears the old instruction card as a promise to a person who expected to return. A sealed shelf can preserve medicine without preserving its intended patient.

001. **The dogged door — The Basement Before the Knock** — The keeper wants the absent person to remain more than a dramatic motive. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The door becomes a quiet expectation that outlived its owner.

002. **Antibiotics in foil — The Basement Before the Knock** — The tape reader hears medicine while the listener hears every person who may not receive it. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The foil becomes a thin boundary between supply and hope.

003. **The instruction card — The Basement Before the Knock** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The card remains a voice without a current body.

004. **The secret knock — The Basement Before the Knock** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The sound becomes an act of recognition.

005. **Ana and Miko — The Basement Before the Knock** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The names remain with the choice that carried them.

006. **Treatment given, treatment owed — The Basement Before the Knock** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The phrase travels as a duty to notice.

007. **The cult’s calm people — The Basement Before the Knock** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The blank around them remains deliberate.

008. **The shelf that tells you everything — The Basement Before the Knock** — The reader asks whether empty shelves tell a story or only show an absence. A sealed shelf can preserve medicine without preserving its intended patient. Candidate return: The empty room remains an honest ending.

### Movement 2: Two Voices in the Storm Cellar

The listener has to hold Ana and Miko’s fear beside the temptation to treat the signal as a clue to loot. A call for help is not a scavenger’s marker.

009. **The dogged door — Two Voices in the Storm Cellar** — The keeper wants the absent person to remain more than a dramatic motive. A call for help is not a scavenger’s marker. Candidate return: The door becomes a quiet expectation that outlived its owner.

010. **Antibiotics in foil — Two Voices in the Storm Cellar** — The tape reader hears medicine while the listener hears every person who may not receive it. A call for help is not a scavenger’s marker. Candidate return: The foil becomes a thin boundary between supply and hope.

011. **The instruction card — Two Voices in the Storm Cellar** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. A call for help is not a scavenger’s marker. Candidate return: The card remains a voice without a current body.

012. **The secret knock — Two Voices in the Storm Cellar** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. A call for help is not a scavenger’s marker. Candidate return: The sound becomes an act of recognition.

013. **Ana and Miko — Two Voices in the Storm Cellar** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A call for help is not a scavenger’s marker. Candidate return: The names remain with the choice that carried them.

014. **Treatment given, treatment owed — Two Voices in the Storm Cellar** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. A call for help is not a scavenger’s marker. Candidate return: The phrase travels as a duty to notice.

015. **The cult’s calm people — Two Voices in the Storm Cellar** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. A call for help is not a scavenger’s marker. Candidate return: The blank around them remains deliberate.

016. **The shelf that tells you everything — Two Voices in the Storm Cellar** — The reader asks whether empty shelves tell a story or only show an absence. A call for help is not a scavenger’s marker. Candidate return: The empty room remains an honest ending.

### Movement 3: Foil, Roll, Card

The tape reader names objects one by one while the keeper resists turning nouns into treatment instructions. Inventory can be an act of care when it stays attached to people.

017. **The dogged door — Foil, Roll, Card** — The keeper wants the absent person to remain more than a dramatic motive. Inventory can be an act of care when it stays attached to people. Candidate return: The door becomes a quiet expectation that outlived its owner.

018. **Antibiotics in foil — Foil, Roll, Card** — The tape reader hears medicine while the listener hears every person who may not receive it. Inventory can be an act of care when it stays attached to people. Candidate return: The foil becomes a thin boundary between supply and hope.

019. **The instruction card — Foil, Roll, Card** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. Inventory can be an act of care when it stays attached to people. Candidate return: The card remains a voice without a current body.

020. **The secret knock — Foil, Roll, Card** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Inventory can be an act of care when it stays attached to people. Candidate return: The sound becomes an act of recognition.

021. **Ana and Miko — Foil, Roll, Card** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. Inventory can be an act of care when it stays attached to people. Candidate return: The names remain with the choice that carried them.

022. **Treatment given, treatment owed — Foil, Roll, Card** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Inventory can be an act of care when it stays attached to people. Candidate return: The phrase travels as a duty to notice.

023. **The cult’s calm people — Foil, Roll, Card** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. Inventory can be an act of care when it stays attached to people. Candidate return: The blank around them remains deliberate.

024. **The shelf that tells you everything — Foil, Roll, Card** — The reader asks whether empty shelves tell a story or only show an absence. Inventory can be an act of care when it stays attached to people. Candidate return: The empty room remains an honest ending.

### Movement 4: The Cult Arrives First

A rumor about calm, well-supplied people competes with the simple fact that the shelves may already be empty. A missing supply is not proof of a conspiracy.

025. **The dogged door — The Cult Arrives First** — The keeper wants the absent person to remain more than a dramatic motive. A missing supply is not proof of a conspiracy. Candidate return: The door becomes a quiet expectation that outlived its owner.

026. **Antibiotics in foil — The Cult Arrives First** — The tape reader hears medicine while the listener hears every person who may not receive it. A missing supply is not proof of a conspiracy. Candidate return: The foil becomes a thin boundary between supply and hope.

027. **The instruction card — The Cult Arrives First** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. A missing supply is not proof of a conspiracy. Candidate return: The card remains a voice without a current body.

028. **The secret knock — The Cult Arrives First** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. A missing supply is not proof of a conspiracy. Candidate return: The sound becomes an act of recognition.

029. **Ana and Miko — The Cult Arrives First** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A missing supply is not proof of a conspiracy. Candidate return: The names remain with the choice that carried them.

030. **Treatment given, treatment owed — The Cult Arrives First** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. A missing supply is not proof of a conspiracy. Candidate return: The phrase travels as a duty to notice.

031. **The cult’s calm people — The Cult Arrives First** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. A missing supply is not proof of a conspiracy. Candidate return: The blank around them remains deliberate.

032. **The shelf that tells you everything — The Cult Arrives First** — The reader asks whether empty shelves tell a story or only show an absence. A missing supply is not proof of a conspiracy. Candidate return: The empty room remains an honest ending.

### Movement 5: Forty-One Cots

The reader hears a hospital counting treatment given and treatment owed, and realizes that a cache can be a footnote to a much larger obligation. A list of patients is not a list of loot.

033. **The dogged door — Forty-One Cots** — The keeper wants the absent person to remain more than a dramatic motive. A list of patients is not a list of loot. Candidate return: The door becomes a quiet expectation that outlived its owner.

034. **Antibiotics in foil — Forty-One Cots** — The tape reader hears medicine while the listener hears every person who may not receive it. A list of patients is not a list of loot. Candidate return: The foil becomes a thin boundary between supply and hope.

035. **The instruction card — Forty-One Cots** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. A list of patients is not a list of loot. Candidate return: The card remains a voice without a current body.

036. **The secret knock — Forty-One Cots** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. A list of patients is not a list of loot. Candidate return: The sound becomes an act of recognition.

037. **Ana and Miko — Forty-One Cots** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A list of patients is not a list of loot. Candidate return: The names remain with the choice that carried them.

038. **Treatment given, treatment owed — Forty-One Cots** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. A list of patients is not a list of loot. Candidate return: The phrase travels as a duty to notice.

039. **The cult’s calm people — Forty-One Cots** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. A list of patients is not a list of loot. Candidate return: The blank around them remains deliberate.

040. **The shelf that tells you everything — Forty-One Cots** — The reader asks whether empty shelves tell a story or only show an absence. A list of patients is not a list of loot. Candidate return: The empty room remains an honest ending.

### Movement 6: The Shelves Afterward

The final voice lets the cache remain finite and the choice to answer remain human. Care is what remains when the supplies cannot answer every need.

041. **The dogged door — The Shelves Afterward** — The keeper wants the absent person to remain more than a dramatic motive. Care is what remains when the supplies cannot answer every need. Candidate return: The door becomes a quiet expectation that outlived its owner.

042. **Antibiotics in foil — The Shelves Afterward** — The tape reader hears medicine while the listener hears every person who may not receive it. Care is what remains when the supplies cannot answer every need. Candidate return: The foil becomes a thin boundary between supply and hope.

043. **The instruction card — The Shelves Afterward** — The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. Care is what remains when the supplies cannot answer every need. Candidate return: The card remains a voice without a current body.

044. **The secret knock — The Shelves Afterward** — The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Care is what remains when the supplies cannot answer every need. Candidate return: The sound becomes an act of recognition.

045. **Ana and Miko — The Shelves Afterward** — The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. Care is what remains when the supplies cannot answer every need. Candidate return: The names remain with the choice that carried them.

046. **Treatment given, treatment owed — The Shelves Afterward** — The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Care is what remains when the supplies cannot answer every need. Candidate return: The phrase travels as a duty to notice.

047. **The cult’s calm people — The Shelves Afterward** — The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. Care is what remains when the supplies cannot answer every need. Candidate return: The blank around them remains deliberate.

048. **The shelf that tells you everything — The Shelves Afterward** — The reader asks whether empty shelves tell a story or only show an absence. Care is what remains when the supplies cannot answer every need. Candidate return: The empty room remains an honest ending.
## 12. Creative variants

### Grounded
Keep the selected passage near an ordinary task: a note read aloud, a price repeated, a warning copied, or a tool set down. Let physical detail carry the weight. The current source remains visible in the wording.

### Interlinked
If an existing consumer already exposes the anchor, a later journal, radio, codex, item, faction, or expedition passage may echo one sentence with its original attribution. The echo changes who hears it, not what the source says.

### Wild card
A proposed reader misremembers a detail and is corrected by somebody who was there. The correction should remain small and human. Never use the wild card to reveal a hidden route, secret operator, miracle device, or new canon casualty.

## 13. Alternative forms and editorial rubric

The four forms are a bank of alternatives, not four mandatory encounters. Choose a **scene draft** when a verified consumer can stage an observable moment; choose a **record and return** when a reader-facing owner can preserve attribution; choose a **conversation fragment** when two people can disagree without a forced verdict; choose a **consequence vignette** only when the existing route exposes the condition that selects it. Remove a candidate when the consumer cannot keep the source boundary legible.

A selected passage should be specific about who saw, heard, paid, copied, or refused. It should leave room for a player to decline interpretation. It should not smuggle a new rule into a metaphor, a new item into a prop description, or a new flag into a sentence about memory.

## 14. Content bank

### Scene drafts

### Scene drafts

#### Scene draft 001 — The dogged door — The Basement Before the Knock

At the sealed clinic basement and its dogged door, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 002 — Antibiotics in foil — The Basement Before the Knock

The scene stays close to an ordinary action near the sealed clinic basement and its dogged door. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: A sealed shelf can preserve medicine without preserving its intended patient.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 003 — The instruction card — The Basement Before the Knock

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A sealed shelf can preserve medicine without preserving its intended patient. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 004 — The secret knock — The Basement Before the Knock

The proposed scene begins at the sealed clinic basement and its dogged door, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. A sealed shelf can preserve medicine without preserving its intended patient. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 005 — Ana and Miko — The Basement Before the Knock

At the sealed clinic basement and its dogged door, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 006 — Treatment given, treatment owed — The Basement Before the Knock

The scene stays close to an ordinary action near the sealed clinic basement and its dogged door. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: A sealed shelf can preserve medicine without preserving its intended patient.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 007 — The cult’s calm people — The Basement Before the Knock

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A sealed shelf can preserve medicine without preserving its intended patient. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 008 — The shelf that tells you everything — The Basement Before the Knock

The proposed scene begins at the sealed clinic basement and its dogged door, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. A sealed shelf can preserve medicine without preserving its intended patient. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 009 — The dogged door — Two Voices in the Storm Cellar

At the distress quest’s broadcast from the clinic cellar, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 010 — Antibiotics in foil — Two Voices in the Storm Cellar

The scene stays close to an ordinary action near the distress quest’s broadcast from the clinic cellar. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: A call for help is not a scavenger’s marker.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 011 — The instruction card — Two Voices in the Storm Cellar

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A call for help is not a scavenger’s marker. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 012 — The secret knock — Two Voices in the Storm Cellar

The proposed scene begins at the distress quest’s broadcast from the clinic cellar, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. A call for help is not a scavenger’s marker. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 013 — Ana and Miko — Two Voices in the Storm Cellar

At the distress quest’s broadcast from the clinic cellar, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 014 — Treatment given, treatment owed — Two Voices in the Storm Cellar

The scene stays close to an ordinary action near the distress quest’s broadcast from the clinic cellar. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: A call for help is not a scavenger’s marker.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 015 — The cult’s calm people — Two Voices in the Storm Cellar

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A call for help is not a scavenger’s marker. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 016 — The shelf that tells you everything — Two Voices in the Storm Cellar

The proposed scene begins at the distress quest’s broadcast from the clinic cellar, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. A call for help is not a scavenger’s marker. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 017 — The dogged door — Foil, Roll, Card

At the cache’s antibiotics, bandages, and medical kit, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 018 — Antibiotics in foil — Foil, Roll, Card

The scene stays close to an ordinary action near the cache’s antibiotics, bandages, and medical kit. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: Inventory can be an act of care when it stays attached to people.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 019 — The instruction card — Foil, Roll, Card

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. Inventory can be an act of care when it stays attached to people. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 020 — The secret knock — Foil, Roll, Card

The proposed scene begins at the cache’s antibiotics, bandages, and medical kit, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. Inventory can be an act of care when it stays attached to people. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 021 — Ana and Miko — Foil, Roll, Card

At the cache’s antibiotics, bandages, and medical kit, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 022 — Treatment given, treatment owed — Foil, Roll, Card

The scene stays close to an ordinary action near the cache’s antibiotics, bandages, and medical kit. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: Inventory can be an act of care when it stays attached to people.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 023 — The cult’s calm people — Foil, Roll, Card

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. Inventory can be an act of care when it stays attached to people. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 024 — The shelf that tells you everything — Foil, Roll, Card

The proposed scene begins at the cache’s antibiotics, bandages, and medical kit, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. Inventory can be an act of care when it stays attached to people. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 025 — The dogged door — The Cult Arrives First

At the location record’s warning about the cult finding caches, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 026 — Antibiotics in foil — The Cult Arrives First

The scene stays close to an ordinary action near the location record’s warning about the cult finding caches. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: A missing supply is not proof of a conspiracy.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 027 — The instruction card — The Cult Arrives First

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A missing supply is not proof of a conspiracy. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 028 — The secret knock — The Cult Arrives First

The proposed scene begins at the location record’s warning about the cult finding caches, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. A missing supply is not proof of a conspiracy. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 029 — Ana and Miko — The Cult Arrives First

At the location record’s warning about the cult finding caches, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 030 — Treatment given, treatment owed — The Cult Arrives First

The scene stays close to an ordinary action near the location record’s warning about the cult finding caches. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: A missing supply is not proof of a conspiracy.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 031 — The cult’s calm people — The Cult Arrives First

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A missing supply is not proof of a conspiracy. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 032 — The shelf that tells you everything — The Cult Arrives First

The proposed scene begins at the location record’s warning about the cult finding caches, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. A missing supply is not proof of a conspiracy. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 033 — The dogged door — Forty-One Cots

At the Field Hospital Seven tape set and its treatment tags, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 034 — Antibiotics in foil — Forty-One Cots

The scene stays close to an ordinary action near the Field Hospital Seven tape set and its treatment tags. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: A list of patients is not a list of loot.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 035 — The instruction card — Forty-One Cots

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A list of patients is not a list of loot. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 036 — The secret knock — Forty-One Cots

The proposed scene begins at the Field Hospital Seven tape set and its treatment tags, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. A list of patients is not a list of loot. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 037 — Ana and Miko — Forty-One Cots

At the Field Hospital Seven tape set and its treatment tags, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 038 — Treatment given, treatment owed — Forty-One Cots

The scene stays close to an ordinary action near the Field Hospital Seven tape set and its treatment tags. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: A list of patients is not a list of loot.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 039 — The cult’s calm people — Forty-One Cots

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. A list of patients is not a list of loot. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 040 — The shelf that tells you everything — Forty-One Cots

The proposed scene begins at the Field Hospital Seven tape set and its treatment tags, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. A list of patients is not a list of loot. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 041 — The dogged door — The Shelves Afterward

At a later reading of the location, quest, and tape records, the distress listener watches the dogged door as if it might answer a question. The keeper wants the absent person to remain more than a dramatic motive. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Who were the boxes waiting for? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Who were the boxes waiting for?” The reply comes without heat: “Someone whose name the shelves did not keep.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Someone whose name the shelves did not keep.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The door becomes a quiet expectation that outlived its owner. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 042 — Antibiotics in foil — The Shelves Afterward

The scene stays close to an ordinary action near a later reading of the location, quest, and tape records. Someone sets down a page, waits for a sound, or looks at a mark. The location preserves antibiotics in foil strips as a material fact.

The tape reader watches the exchange rather than interrupting it. The tape reader hears medicine while the listener hears every person who may not receive it. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the foil remember?” The distress listener answers, “Only that the seal held longer than the plan.” Their difference is practical: Care is what remains when the supplies cannot answer every need.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The foil becomes a thin boundary between supply and hope. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 043 — The instruction card — The Shelves Afterward

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. Care is what remains when the supplies cannot answer every need. Neither speaker claims the right to finish the source.

“Who is the card speaking to?” says the cache keeper. The distress listener answers after a breath: “A person who is not here to answer back.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the instruction card. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The medical kit remains in its case with an instruction card. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 044 — The secret knock — The Shelves Afterward

The proposed scene begins at a later reading of the location, quest, and tape records, after the immediate work has paused. The distress quest records a secret knock as part of the authored extraction choice. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What did the knock promise?” The cache keeper replies, “That somebody on the other side might still be listening.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code.

The sound becomes an act of recognition. Care is what remains when the supplies cannot answer every need. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 045 — Ana and Miko — The Shelves Afterward

At a later reading of the location, quest, and tape records, the distress listener watches the Ana and Miko as if it might answer a question. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The place remains what the source describes.

For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Whose names belong on the tag? The answer stays attached to the person who says it.

The first line sounds sharper than its speaker intends: “Whose names belong on the tag?” The reply comes without heat: “The names of the people the current quest actually names.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. A quiet departure is a complete response.

The cache keeper repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. The names of the people the current quest actually names.

Afterward, the tape reader remembers the exchange without claiming it settled anything. The names remain with the choice that carried them. The people move on to the next ordinary need.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 046 — Treatment given, treatment owed — The Shelves Afterward

The scene stays close to an ordinary action near a later reading of the location, quest, and tape records. Someone sets down a page, waits for a sound, or looks at a mark. The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care.

The tape reader watches the exchange rather than interrupting it. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. What each speaker can claim stays narrower than what either of them feels.

The cache keeper asks, “What does the owed treatment ask of us?” The distress listener answers, “Attention, not a promise that supplies will be enough.” Their difference is practical: Care is what remains when the supplies cannot answer every need.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The phrase travels as a duty to notice. The choice changes what can be repeated, not the authored condition of Pre-War Medical Cache.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 047 — The cult’s calm people — The Shelves Afterward

A proposed encounter opens with the people who are present, not with an explanation of the whole Pre-War Medical Cache. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The detail is allowed to remain smaller than the story around it.

The cache keeper notices what the action leaves out, while the distress listener remembers why that omission matters. Care is what remains when the supplies cannot answer every need. Neither speaker claims the right to finish the source.

“What do we actually know about them?” says the cache keeper. The distress listener answers after a breath: “That the record says they find caches first and do not say where.” Neither tries to make the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be complete.

The distress listener looks once more at the cult’s calm people. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The location describes the cult’s people as calm, careful, and not poor, without explaining their cache network. The voices move on, carrying different parts of the exchange. The source boundary remains visible.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

#### Scene draft 048 — The shelf that tells you everything — The Shelves Afterward

The proposed scene begins at a later reading of the location, quest, and tape records, after the immediate work has paused. The location says that after the cult reaches the cache, the shelves will tell you everything. Nothing in the moment confirms more than the cited record.

The detail draws the cache keeper into a question and the distress listener into a memory. The reader asks whether empty shelves tell a story or only show an absence. Their disagreement is about responsibility, not about winning an argument.

The distress listener lays the question between them: “What do the shelves know?” The cache keeper replies, “What was taken, what was left, and how little that can explain.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain.

For a breath, the tape reader almost adds a detail, then hears how little it would prove. The people stay with what they have: The reader asks whether empty shelves tell a story or only show an absence.

The empty room remains an honest ending. Care is what remains when the supplies cannot answer every need. The detail has not become a route, reward, diagnosis, or proof of a history the source does not give.

Editorial check: retain this passage only if its speaker, audience, and uncertainty remain legible through the current content owner. It adds no new state.

### Record and returns

#### Record and return 001 — The dogged door — The Basement Before the Knock

A possible copy is made after the exchange at the sealed clinic basement and its dogged door. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 002 — Antibiotics in foil — The Basement Before the Knock

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 003 — The instruction card — The Basement Before the Knock

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A sealed shelf can preserve medicine without preserving its intended patient. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 004 — The secret knock — The Basement Before the Knock

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. A sealed shelf can preserve medicine without preserving its intended patient.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 005 — Ana and Miko — The Basement Before the Knock

A possible copy is made after the exchange at the sealed clinic basement and its dogged door. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 006 — Treatment given, treatment owed — The Basement Before the Knock

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 007 — The cult’s calm people — The Basement Before the Knock

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A sealed shelf can preserve medicine without preserving its intended patient. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 008 — The shelf that tells you everything — The Basement Before the Knock

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. A sealed shelf can preserve medicine without preserving its intended patient.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 009 — The dogged door — Two Voices in the Storm Cellar

A possible copy is made after the exchange at the distress quest’s broadcast from the clinic cellar. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 010 — Antibiotics in foil — Two Voices in the Storm Cellar

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 011 — The instruction card — Two Voices in the Storm Cellar

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A call for help is not a scavenger’s marker. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 012 — The secret knock — Two Voices in the Storm Cellar

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. A call for help is not a scavenger’s marker.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 013 — Ana and Miko — Two Voices in the Storm Cellar

A possible copy is made after the exchange at the distress quest’s broadcast from the clinic cellar. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 014 — Treatment given, treatment owed — Two Voices in the Storm Cellar

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 015 — The cult’s calm people — Two Voices in the Storm Cellar

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A call for help is not a scavenger’s marker. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 016 — The shelf that tells you everything — Two Voices in the Storm Cellar

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. A call for help is not a scavenger’s marker.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 017 — The dogged door — Foil, Roll, Card

A possible copy is made after the exchange at the cache’s antibiotics, bandages, and medical kit. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 018 — Antibiotics in foil — Foil, Roll, Card

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 019 — The instruction card — Foil, Roll, Card

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Inventory can be an act of care when it stays attached to people. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 020 — The secret knock — Foil, Roll, Card

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. Inventory can be an act of care when it stays attached to people.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 021 — Ana and Miko — Foil, Roll, Card

A possible copy is made after the exchange at the cache’s antibiotics, bandages, and medical kit. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 022 — Treatment given, treatment owed — Foil, Roll, Card

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 023 — The cult’s calm people — Foil, Roll, Card

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Inventory can be an act of care when it stays attached to people. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 024 — The shelf that tells you everything — Foil, Roll, Card

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. Inventory can be an act of care when it stays attached to people.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 025 — The dogged door — The Cult Arrives First

A possible copy is made after the exchange at the location record’s warning about the cult finding caches. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 026 — Antibiotics in foil — The Cult Arrives First

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 027 — The instruction card — The Cult Arrives First

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A missing supply is not proof of a conspiracy. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 028 — The secret knock — The Cult Arrives First

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. A missing supply is not proof of a conspiracy.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 029 — Ana and Miko — The Cult Arrives First

A possible copy is made after the exchange at the location record’s warning about the cult finding caches. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 030 — Treatment given, treatment owed — The Cult Arrives First

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 031 — The cult’s calm people — The Cult Arrives First

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A missing supply is not proof of a conspiracy. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 032 — The shelf that tells you everything — The Cult Arrives First

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. A missing supply is not proof of a conspiracy.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 033 — The dogged door — Forty-One Cots

A possible copy is made after the exchange at the Field Hospital Seven tape set and its treatment tags. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 034 — Antibiotics in foil — Forty-One Cots

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 035 — The instruction card — Forty-One Cots

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A list of patients is not a list of loot. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 036 — The secret knock — Forty-One Cots

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. A list of patients is not a list of loot.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 037 — Ana and Miko — Forty-One Cots

A possible copy is made after the exchange at the Field Hospital Seven tape set and its treatment tags. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 038 — Treatment given, treatment owed — Forty-One Cots

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 039 — The cult’s calm people — Forty-One Cots

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A list of patients is not a list of loot. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 040 — The shelf that tells you everything — Forty-One Cots

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. A list of patients is not a list of loot.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 041 — The dogged door — The Shelves Afterward

A possible copy is made after the exchange at a later reading of the location, quest, and tape records. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Who were the boxes waiting for?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The keeper wants the absent person to remain more than a dramatic motive. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The door becomes a quiet expectation that outlived its owner. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 042 — Antibiotics in foil — The Shelves Afterward

The top line names the subject as antibiotics in foil. The next line gives the reason for writing: The tape reader hears medicine while the listener hears every person who may not receive it. The author leaves room for a later reader to disagree.

> “Only that the seal held longer than the plan.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The tape reader hears medicine while the listener hears every person who may not receive it. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The foil becomes a thin boundary between supply and hope. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The foil becomes a thin boundary between supply and hope.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 043 — The instruction card — The Shelves Afterward

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Who is the card speaking to?

> “Who is the card speaking to?”
>
> “A person who is not here to answer back.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Care is what remains when the supplies cannot answer every need. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The card remains a voice without a current body. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 044 — The secret knock — The Shelves Afterward

Proposed reader’s note, from the cache keeper to the tape reader: The distress quest records a secret knock as part of the authored extraction choice. The writer keeps the account narrow enough that another person can check it.

> “That somebody on the other side might still be listening.”
>
> The first copy made this sound settled. It was not. Care is what remains when the supplies cannot answer every need.

Beside the excerpt, the author distinguishes a witness from a writer. The listener remembers the knock as a human signal, while the keeper refuses to make it a universal access code. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 045 — Ana and Miko — The Shelves Afterward

A possible copy is made after the exchange at a later reading of the location, quest, and tape records. Its proposed author is the distress listener; its reader knows Pre-War Medical Cache only by what others have said.

> “Whose names belong on the tag?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The names remain with the choice that carried them. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 046 — Treatment given, treatment owed — The Shelves Afterward

The top line names the subject as treatment given, treatment owed. The next line gives the reason for writing: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. The author leaves room for a later reader to disagree.

> “Attention, not a promise that supplies will be enough.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Pre-War Medical Cache.

The note preserves a limit: The reader wants the cache to settle the debt; the keeper knows a shelf cannot. It does not turn a recollection into a map, inventory, diagnosis, procedure, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The phrase travels as a duty to notice. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.” The final line is The phrase travels as a duty to notice.

Source check: The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 047 — The cult’s calm people — The Shelves Afterward

The cache keeper writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: What do we actually know about them?

> “What do we actually know about them?”
>
> “That the record says they find caches first and do not say where.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Care is what remains when the supplies cannot answer every need. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The blank around them remains deliberate. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the writer.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

#### Record and return 048 — The shelf that tells you everything — The Shelves Afterward

Proposed reader’s note, from the cache keeper to the tape reader: The location says that after the cult reaches the cache, the shelves will tell you everything. The writer keeps the account narrow enough that another person can check it.

> “What was taken, what was left, and how little that can explain.”
>
> The first copy made this sound settled. It was not. Care is what remains when the supplies cannot answer every need.

Beside the excerpt, the author distinguishes a witness from a writer. The reader asks whether empty shelves tell a story or only show an absence. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

Record boundary: the proposed page is not a new archive authority, item, flag, or save section.

### Conversation fragments

#### Conversation fragment 001 — The dogged door — The Basement Before the Knock

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 002 — Antibiotics in foil — The Basement Before the Knock

At the sealed clinic basement and its dogged door, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: A sealed shelf can preserve medicine without preserving its intended patient.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A sealed shelf can preserve medicine without preserving its intended patient. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 003 — The instruction card — The Basement Before the Knock

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 004 — The secret knock — The Basement Before the Knock

The two proposed speakers meet over the secret knock at the sealed clinic basement and its dogged door. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 005 — Ana and Miko — The Basement Before the Knock

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 006 — Treatment given, treatment owed — The Basement Before the Knock

At the sealed clinic basement and its dogged door, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: A sealed shelf can preserve medicine without preserving its intended patient.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A sealed shelf can preserve medicine without preserving its intended patient. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 007 — The cult’s calm people — The Basement Before the Knock

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 008 — The shelf that tells you everything — The Basement Before the Knock

The two proposed speakers meet over the shelf that tells you everything at the sealed clinic basement and its dogged door. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 009 — The dogged door — Two Voices in the Storm Cellar

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 010 — Antibiotics in foil — Two Voices in the Storm Cellar

At the distress quest’s broadcast from the clinic cellar, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: A call for help is not a scavenger’s marker.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A call for help is not a scavenger’s marker. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 011 — The instruction card — Two Voices in the Storm Cellar

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 012 — The secret knock — Two Voices in the Storm Cellar

The two proposed speakers meet over the secret knock at the distress quest’s broadcast from the clinic cellar. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 013 — Ana and Miko — Two Voices in the Storm Cellar

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 014 — Treatment given, treatment owed — Two Voices in the Storm Cellar

At the distress quest’s broadcast from the clinic cellar, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: A call for help is not a scavenger’s marker.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A call for help is not a scavenger’s marker. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 015 — The cult’s calm people — Two Voices in the Storm Cellar

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 016 — The shelf that tells you everything — Two Voices in the Storm Cellar

The two proposed speakers meet over the shelf that tells you everything at the distress quest’s broadcast from the clinic cellar. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 017 — The dogged door — Foil, Roll, Card

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 018 — Antibiotics in foil — Foil, Roll, Card

At the cache’s antibiotics, bandages, and medical kit, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: Inventory can be an act of care when it stays attached to people.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Inventory can be an act of care when it stays attached to people. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 019 — The instruction card — Foil, Roll, Card

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 020 — The secret knock — Foil, Roll, Card

The two proposed speakers meet over the secret knock at the cache’s antibiotics, bandages, and medical kit. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 021 — Ana and Miko — Foil, Roll, Card

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 022 — Treatment given, treatment owed — Foil, Roll, Card

At the cache’s antibiotics, bandages, and medical kit, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: Inventory can be an act of care when it stays attached to people.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Inventory can be an act of care when it stays attached to people. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 023 — The cult’s calm people — Foil, Roll, Card

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 024 — The shelf that tells you everything — Foil, Roll, Card

The two proposed speakers meet over the shelf that tells you everything at the cache’s antibiotics, bandages, and medical kit. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 025 — The dogged door — The Cult Arrives First

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 026 — Antibiotics in foil — The Cult Arrives First

At the location record’s warning about the cult finding caches, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: A missing supply is not proof of a conspiracy.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A missing supply is not proof of a conspiracy. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 027 — The instruction card — The Cult Arrives First

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 028 — The secret knock — The Cult Arrives First

The two proposed speakers meet over the secret knock at the location record’s warning about the cult finding caches. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 029 — Ana and Miko — The Cult Arrives First

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 030 — Treatment given, treatment owed — The Cult Arrives First

At the location record’s warning about the cult finding caches, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: A missing supply is not proof of a conspiracy.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A missing supply is not proof of a conspiracy. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 031 — The cult’s calm people — The Cult Arrives First

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 032 — The shelf that tells you everything — The Cult Arrives First

The two proposed speakers meet over the shelf that tells you everything at the location record’s warning about the cult finding caches. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 033 — The dogged door — Forty-One Cots

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 034 — Antibiotics in foil — Forty-One Cots

At the Field Hospital Seven tape set and its treatment tags, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: A list of patients is not a list of loot.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A list of patients is not a list of loot. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 035 — The instruction card — Forty-One Cots

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 036 — The secret knock — Forty-One Cots

The two proposed speakers meet over the secret knock at the Field Hospital Seven tape set and its treatment tags. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 037 — Ana and Miko — Forty-One Cots

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 038 — Treatment given, treatment owed — Forty-One Cots

At the Field Hospital Seven tape set and its treatment tags, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: A list of patients is not a list of loot.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A list of patients is not a list of loot. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 039 — The cult’s calm people — Forty-One Cots

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 040 — The shelf that tells you everything — Forty-One Cots

The two proposed speakers meet over the shelf that tells you everything at the Field Hospital Seven tape set and its treatment tags. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 041 — The dogged door — The Shelves Afterward

The room has gone quiet. The keeper wants the absent person to remain more than a dramatic motive. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Who were the boxes waiting for?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “Someone whose name the shelves did not keep.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 042 — Antibiotics in foil — The Shelves Afterward

At a later reading of the location, quest, and tape records, one person looks again at the detail: The location preserves antibiotics in foil strips as a material fact. The cache keeper has a different reason for staying: Care is what remains when the supplies cannot answer every need.

Distress listener: “Only that the seal held longer than the plan.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Care is what remains when the supplies cannot answer every need. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 043 — The instruction card — The Shelves Afterward

The conversation starts with the people who are here, not with a speech about everyone else. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. They are trying to say what this one detail means to them.

Cache keeper: “Who is the card speaking to?”

Distress listener: “A person who is not here to answer back.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The keeper wants to read the card aloud; the reader says a found instruction is not authority for a new patient. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 044 — The secret knock — The Shelves Afterward

The two proposed speakers meet over the secret knock at a later reading of the location, quest, and tape records. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “That somebody on the other side might still be listening.”

Cache keeper: “What did the knock promise?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The sound becomes an act of recognition. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 045 — Ana and Miko — The Shelves Afterward

The room has gone quiet. The tape reader wants their story folded into the old hospital record; the listener keeps their present choice separate. The tape reader lets the other two decide whether the same words can hold what they remember about Pre-War Medical Cache.

Cache keeper: “Whose names belong on the tag?”

Tape reader: “What would you need before you wrote that down?”

Distress listener: “The names of the people the current quest actually names.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Tape reader: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 046 — Treatment given, treatment owed — The Shelves Afterward

At a later reading of the location, quest, and tape records, one person looks again at the detail: The Field Hospital Seven final cassette leaves the phrase as an account of care and unfinished care. The cache keeper has a different reason for staying: Care is what remains when the supplies cannot answer every need.

Distress listener: “Attention, not a promise that supplies will be enough.”

Tape reader: “Would you let somebody else repeat it?”

Cache keeper: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Care is what remains when the supplies cannot answer every need. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The distress listener turns toward the next task but does not withdraw the answer. The cache keeper lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 047 — The cult’s calm people — The Shelves Afterward

The conversation starts with the people who are here, not with a speech about everyone else. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. They are trying to say what this one detail means to them.

Cache keeper: “What do we actually know about them?”

Distress listener: “That the record says they find caches first and do not say where.”

Cache keeper: “Then I’ll write what I saw and leave the rest with you.”

The tape reader does not rush to decide which account is more useful. The listener hears suspicion; the keeper hears the danger of making scarcity into a stereotype. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

#### Conversation fragment 048 — The shelf that tells you everything — The Shelves Afterward

The two proposed speakers meet over the shelf that tells you everything at a later reading of the location, quest, and tape records. The cache keeper is trying to keep the account useful; the distress listener is trying to keep it honest.

Distress listener: “What was taken, what was left, and how little that can explain.”

Cache keeper: “What do the shelves know?”

Distress listener: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The cache keeper starts to reply, then lets the distress listener finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The empty room remains an honest ending. That callback changes the audience, not the source fact.

The content boundary stays explicit: this dialogue adds no faction rule, item function, route, flag, or treatment. It gives existing words another human audience.

Review note: if the current consumer cannot preserve speaker attribution, use a non-conditional close or omit this candidate.

### Consequence vignettes

#### Consequence vignette 001 — The dogged door — The Basement Before the Knock

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A sealed shelf can preserve medicine without preserving its intended patient. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A sealed shelf can preserve medicine without preserving its intended patient. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 002 — Antibiotics in foil — The Basement Before the Knock

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 003 — The instruction card — The Basement Before the Knock

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 004 — The secret knock — The Basement Before the Knock

This return vignette begins after the player has encountered the secret knock. The setting is the sealed clinic basement and its dogged door, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. A sealed shelf can preserve medicine without preserving its intended patient. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 005 — Ana and Miko — The Basement Before the Knock

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A sealed shelf can preserve medicine without preserving its intended patient. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A sealed shelf can preserve medicine without preserving its intended patient. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 006 — Treatment given, treatment owed — The Basement Before the Knock

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 007 — The cult’s calm people — The Basement Before the Knock

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 008 — The shelf that tells you everything — The Basement Before the Knock

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is the sealed clinic basement and its dogged door, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. A sealed shelf can preserve medicine without preserving its intended patient. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 009 — The dogged door — Two Voices in the Storm Cellar

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A call for help is not a scavenger’s marker. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A call for help is not a scavenger’s marker. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 010 — Antibiotics in foil — Two Voices in the Storm Cellar

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 011 — The instruction card — Two Voices in the Storm Cellar

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 012 — The secret knock — Two Voices in the Storm Cellar

This return vignette begins after the player has encountered the secret knock. The setting is the distress quest’s broadcast from the clinic cellar, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. A call for help is not a scavenger’s marker. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 013 — Ana and Miko — Two Voices in the Storm Cellar

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A call for help is not a scavenger’s marker. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A call for help is not a scavenger’s marker. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 014 — Treatment given, treatment owed — Two Voices in the Storm Cellar

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 015 — The cult’s calm people — Two Voices in the Storm Cellar

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 016 — The shelf that tells you everything — Two Voices in the Storm Cellar

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is the distress quest’s broadcast from the clinic cellar, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. A call for help is not a scavenger’s marker. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 017 — The dogged door — Foil, Roll, Card

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Inventory can be an act of care when it stays attached to people. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Inventory can be an act of care when it stays attached to people. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 018 — Antibiotics in foil — Foil, Roll, Card

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 019 — The instruction card — Foil, Roll, Card

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 020 — The secret knock — Foil, Roll, Card

This return vignette begins after the player has encountered the secret knock. The setting is the cache’s antibiotics, bandages, and medical kit, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. Inventory can be an act of care when it stays attached to people. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 021 — Ana and Miko — Foil, Roll, Card

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Inventory can be an act of care when it stays attached to people. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Inventory can be an act of care when it stays attached to people. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 022 — Treatment given, treatment owed — Foil, Roll, Card

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 023 — The cult’s calm people — Foil, Roll, Card

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 024 — The shelf that tells you everything — Foil, Roll, Card

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is the cache’s antibiotics, bandages, and medical kit, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. Inventory can be an act of care when it stays attached to people. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 025 — The dogged door — The Cult Arrives First

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A missing supply is not proof of a conspiracy. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A missing supply is not proof of a conspiracy. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 026 — Antibiotics in foil — The Cult Arrives First

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 027 — The instruction card — The Cult Arrives First

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 028 — The secret knock — The Cult Arrives First

This return vignette begins after the player has encountered the secret knock. The setting is the location record’s warning about the cult finding caches, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. A missing supply is not proof of a conspiracy. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 029 — Ana and Miko — The Cult Arrives First

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A missing supply is not proof of a conspiracy. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A missing supply is not proof of a conspiracy. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 030 — Treatment given, treatment owed — The Cult Arrives First

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 031 — The cult’s calm people — The Cult Arrives First

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 032 — The shelf that tells you everything — The Cult Arrives First

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is the location record’s warning about the cult finding caches, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. A missing supply is not proof of a conspiracy. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 033 — The dogged door — Forty-One Cots

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A list of patients is not a list of loot. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A list of patients is not a list of loot. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 034 — Antibiotics in foil — Forty-One Cots

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 035 — The instruction card — Forty-One Cots

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 036 — The secret knock — Forty-One Cots

This return vignette begins after the player has encountered the secret knock. The setting is the Field Hospital Seven tape set and its treatment tags, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. A list of patients is not a list of loot. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 037 — Ana and Miko — Forty-One Cots

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A list of patients is not a list of loot. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A list of patients is not a list of loot. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 038 — Treatment given, treatment owed — Forty-One Cots

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 039 — The cult’s calm people — Forty-One Cots

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 040 — The shelf that tells you everything — Forty-One Cots

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is the Field Hospital Seven tape set and its treatment tags, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. A list of patients is not a list of loot. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 041 — The dogged door — The Shelves Afterward

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The location record says the clinic door was dogged down before the exchange and the boxes were stacked by someone who thought they might return. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Care is what remains when the supplies cannot answer every need. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Care is what remains when the supplies cannot answer every need. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 042 — Antibiotics in foil — The Shelves Afterward

The callback comes in an ordinary conversation. The tape reader hears medicine while the listener hears every person who may not receive it. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Only that the seal held longer than the plan.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 043 — The instruction card — The Shelves Afterward

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The card remains a voice without a current body.

If the player carried the first account forward, the listener receives: “Who is the card speaking to?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 044 — The secret knock — The Shelves Afterward

This return vignette begins after the player has encountered the secret knock. The setting is a later reading of the location, quest, and tape records, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “That somebody on the other side might still be listening.”

The first exchange goes uncarried. Care is what remains when the supplies cannot answer every need. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The sound becomes an act of recognition. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 045 — Ana and Miko — The Shelves Afterward

Later, the tape reader hears one version of what happened at Pre-War Medical Cache. The quest names two siblings and records branch outcomes that depend on whether help arrives. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Care is what remains when the supplies cannot answer every need. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The tape reader asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Care is what remains when the supplies cannot answer every need. Either can close on a human consequence rather than a mechanic.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 046 — Treatment given, treatment owed — The Shelves Afterward

The callback comes in an ordinary conversation. The reader wants the cache to settle the debt; the keeper knows a shelf cannot. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Attention, not a promise that supplies will be enough.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, treatment, or saved memory. The reader leaves with a sentence they can question.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 047 — The cult’s calm people — The Shelves Afterward

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The blank around them remains deliberate.

If the player carried the first account forward, the listener receives: “What do we actually know about them?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

#### Consequence vignette 048 — The shelf that tells you everything — The Shelves Afterward

This return vignette begins after the player has encountered the shelf that tells you everything. The setting is a later reading of the location, quest, and tape records, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “What was taken, what was left, and how little that can explain.”

The first exchange goes uncarried. Care is what remains when the supplies cannot answer every need. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The empty room remains an honest ending. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

Boundary check: this conditional language is editorial. It must use an existing verified condition if selected and must not create a hidden flag for Pre-War Medical Cache.

The source remains the authority; the callback changes the audience, not the catalog, hazard, item, faction, or person named by the record.

## 15. Character and relationship continuity

The proposed voices are roles created for editorial exploration. Do not silently promote them into named survivors, faction leaders, quest givers, permanent settlement residents, operators, patients, or victims. Their relationships remain within each selected passage unless another current source establishes a return. The story can leave a relationship unresolved without creating a hidden standing change.

## 16. Passage-selection map

**Use a scene** when a verified consumer can stage an observable moment without implying unsafe traversal or a technical procedure.
**Use a record** when the game already has an appropriate reader-facing content owner and attribution can be preserved.
**Use a conversation** when the passage benefits from two people who disagree in good faith.
**Use a consequence vignette** only when the existing route exposes the condition that selects it; otherwise use a non-conditional close or omit the variant.
These are editorial selection notes, not proposed runtime features.

## 17. Content boundaries and open questions

| Topic | Current evidence | Editorial limit |
|---|---|---|
| Anchor | prewar_medical_cache in current local data | Do not infer a new route, encounter, exact staging, or consumer from catalog presence. |
| Existing prose/state | Current location and cited companion records | Attribute exact source text; do not silently amend it. |
| Proposed people | Anonymous roles listed above | Keep each role editorial unless a verified source names an existing character. |
| Player response | Four prose alternatives per beat | Do not claim a new menu, flag, score, reward, treatment, or save behavior. |
| Hazard or scarcity | The location record describes a clinic basement sealed in the last week before the exchange, fifteen rads per hour, preserved antibiotics, bandages, and a medical kit. It says the cult finds these caches first and leaves no explanation of where. The expedition record identifies the cache as a low-danger hospital loot site. The distress quest places Ana and Miko in a storm cellar beneath an abandoned clinic, with a secret knock and branch outcomes. The Field Hospital Seven cassette set names the cache as its hidden location and leaves a record of forty-one cots, treatment given, and treatment owed. | Do not give instructions, safety guarantees, or unverified outcomes. |

Do not provide diagnosis, dosage, extraction, radiation, lock, secret-knock, route, or medical handling instructions. Do not replace the distress quest’s choices or its outcomes for Ana and Miko. Do not turn the cassette into a treatment recipe, the cache into an unlimited supply, or the cult’s secrecy into a new faction mechanic. Existing medical item authority and current quest branches remain the boundary.
Every object, warning, price, voice, and return below remains proposed prose. A selected passage must be checked against newer narrative data before content placement.

## 18. Local-canon and collision audit

The direct anchor prewar_medical_cache was selected after searching the existing expansion documentation for anchor collisions. This is a bounded content audit, not a claim that no related theme exists anywhere in the project. Search again before implementation. The cited source records above are the canon boundary; all proposed scenes, voices, and artifacts must be checked against any newer narrative data before they are selected.

No real-world country, war, person, copied art, copied text, or real-world interface layout is introduced. The prose is original and specific to the local fictional records.

## 19. Acceptance and handoff

- Keep the 48 beats as an optional content bank; do not implement all 192 candidates by default.
- Preserve current source facts, hazard language, item descriptions, and named-character outcomes.
- Verify a real content consumer and route before selecting a passage.
- Keep new voices and props editorial until an authorized owner accepts them.
- Do not change production code or authoritative game data as part of this plan.
- Review voice distinction, factual boundaries, attribution, and branch handling before content placement.

This file is a complete prose expansion proposal. It contains no implementation claim and no assertion that any candidate passage is already reachable.
