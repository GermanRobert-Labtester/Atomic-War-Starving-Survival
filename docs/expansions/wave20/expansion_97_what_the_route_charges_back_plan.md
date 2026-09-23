# EXPANSION 97 — What the Route Charges Back

## A prose-led extension for Mara Veln’s water-station trade, the wreck on the Cut Road, and the future written into a favor.

### Wave 20: Terms of Staying

## 1. Expansion thesis

Make Mara’s route feel like an obligation carried by people rather than a line on a map. The existing story already gives the player a fair trade or a profitable squeeze, then a rescue or a pass. The expansion adds texture to the same decisions and to the later pages that record what they cost. This is a prose-first game-content plan: the main deliverable is scene text, dialogue, records, and state-aware return passages. Any proposed prose is supplemental to the current authored content and remains a proposal until accepted through the game's existing narrative owner.

## 2. Story in one sentence

A caravan factor learns that the favor and the debt are separated by the hand that keeps the ledger, and the player has to live with what Mara remembers.

## 3. Verified local anchor and current story

`characters.json` defines `npc_mara_veln` as a Grain Exchange caravan factor who runs a two-wagon route between water stations, wants medicine and axle parts, offers caravan space and route credit, and will not carry contraband or name buyers. `npc_arcs.json` contains her initial, evolved, late, dead, and recruited states. `narrative_encounters_npc_arcs.json` authors the Water Station and Cut Road encounters. `quests_npc_arcs.json` contains `quest_arc_mara_01_waystation`, `quest_arc_mara_02_route`, and `quest_arc_mara_03_office`. The relevant locations include `loc_water_station`, `loc_grain_silo`, and `loc_cut_merchant_caravanserai`. The first encounter offers `mara_help` or `mara_exploit`; the wreck encounter offers `mara_rescue` or `mara_pass`. The existing arc describes distinct later states, including official coordination, an independent route, and an embargo response. This plan does not overwrite those branches or assume that any one is universal. The JSON record proves that these names, places, and story conditions are authored; it does not by itself prove a route is currently reachable in every campaign. Keep the plan's proposed insertions subordinate to the live content and its current-state conditions.

## 4. Fixed canon and proposed text

The caravan begins with two wagons and a shortage of medicine and axle parts after a bad crossing. The Cut Road wreck and its existing choices are fixed. The late state descriptions set the established outcomes; the plan may provide optional prose variants for those states but cannot grant route access or a trade rate by itself. No cargo quantity, buyer identity, or new route is established here. Existing quest IDs, choice IDs, character states, names, location descriptions, and branch conditions remain the authority. Every additional visitor, line, paper, gesture, or callback below is proposed writing, not new established history.

## 5. Human center

Mara keeps a ledger because memory is not a fair substitute for terms when one person has the wagons and another has the supplies. She can remember kindness without making it free, and she can remember exploitation without ceasing to trade. The story respects that practical distinction. The player can witness and respond, but the character's life does not become a demonstration designed for the player's moral growth. Let material detail carry emotion; do not tell the player which feeling to have.

## 6. Voice and point of view

Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Keep narration close to evidence: what someone counts, moves, refuses to sign, or leaves within reach. People speak from their job and their fatigue. Nobody explains their own symbolism.

## 7. Placement in the current story

Keep the first four beats beside the existing Water Station encounter, the next four beside the existing wreck choice, and later beats conditional on the current NPC arc state. For `mara_help` plus `mara_rescue`, use only the official or independent outcomes supported by the state data. For `mara_exploit`, preserve the remembered premium and any later embargo state. If she is dead or recruited, use only the already-authored terminal state. The sequence below proposes alternate or additional prose for existing content surfaces. It does not introduce a parallel quest chain, duplicate a registered encounter, or promise a new arrival. Use the current quest or location owner to decide where an accepted line belongs.

## 8. Player agency and consequence

The current choices already distinguish a fair exchange from buying a shortage at a premium and a costly rescue from taking loose cargo and leaving. Prose should make the trade understandable before the player chooses, without softening either cost or secretly substituting an answer. Later callbacks can show memory and standing only where the current arc says they apply. Preserve the player's ability to help, refuse, wait, or leave. Consequence should be visible in an authored state that already exists or in text conditional on an existing branch; do not imply new state from a line alone.

## 9. Dignity, safety, and scope

Keep commerce clear, avoid coercive romanticization, and never imply that the player can make Mara reveal a buyer or carry contraband. The road hazard remains a narrative condition, not a travel tutorial. Keep every hazardous, medical, private, or coercive subject within the boundaries of the source. The prose can show limits and uncertainty without explaining a dangerous workaround, exposing a hidden person's identity, or making an unverified treatment promise.

## 10. Content integration boundary

This plan adds no production code, JSON, quest ID, encounter ID, location, faction, route, resource rule, score, or save field. If an editor later selects a fragment, verify the existing content schema and state consumer first, then place it with the current owning data. The plan itself is review material only.

## 11. Content bank: proposed prose

The following beats each have four editorial forms: a scene, a diegetic record, an optional conversation, and a return vignette. They are alternatives for the same moment, not four mandatory encounters. Select a coherent subset and keep each fragment inside the current character state described above.

### Scene draft 001 — The ledger opens beside the water tank

Two tarped wagons stand in the lee of the pumping station. Mara does not offer a tour. She opens her ledger where the visitor can see the short lines for medicine and axle parts, then waits to learn whether the visitor came to trade or merely to look. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “If you came to look, look. If you came to trade, let us start with what is missing.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not add exact stock, quantities, or a buyer identity.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. On a return, leave the request legible unless Mara herself has revised the terms.

Scene close: Water Station copy: Medicine and axle parts requested; no quantity or buyer name entered. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 002 — The ledger opens beside the water tank

Proposed diegetic text: “Water Station copy: Medicine and axle parts requested; no quantity or buyer name entered.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Two tarped wagons stand in the lee of the pumping station. Mara does not offer a tour. She opens her ledger where the visitor can see the short lines for medicine and axle parts, then waits to learn whether the visitor came to trade or merely to look. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: On a return, leave the request legible unless Mara herself has revised the terms. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not add exact stock, quantities, or a buyer identity. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 003 — The ledger opens beside the water tank

Mara Veln is speaking with someone whose next decision is affected by the work. Two tarped wagons stand in the lee of the pumping station. Mara does not offer a tour. She opens her ledger where the visitor can see the short lines for medicine and axle parts, then waits to learn whether the visitor came to trade or merely to look. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “If you came to look, look. If you came to trade, let us start with what is missing.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “On a return, leave the request legible unless Mara herself has revised the terms.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not add exact stock, quantities, or a buyer identity. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Water Station copy: Medicine and axle parts requested; no quantity or buyer name entered. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 004 — The ledger opens beside the water tank

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Two tarped wagons stand in the lee of the pumping station. Mara does not offer a tour. She opens her ledger where the visitor can see the short lines for medicine and axle parts, then waits to learn whether the visitor came to trade or merely to look. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: On a return, leave the request legible unless Mara herself has revised the terms. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not add exact stock, quantities, or a buyer identity.

Mara Veln may say: “If you came to look, look. If you came to trade, let us start with what is missing.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Water Station copy: Medicine and axle parts requested; no quantity or buyer name entered. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 005 — The scale does not settle the whole deal

A fair weight can settle the goods in front of the scale without settling what either person owes later. Mara keeps the exchange narrow enough that the buyer can see the goods and the price on the same side of the page. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “A weight tells us what is here. It does not tell us what you will remember.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No new trade mechanics, price shock, or discount.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. At a later visit, distinguish the completed trade from any separate favor.

Scene close: Trade stub: Goods and asking price read aloud; later favor not entered as a charge. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 006 — The scale does not settle the whole deal

Proposed diegetic text: “Trade stub: Goods and asking price read aloud; later favor not entered as a charge.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A fair weight can settle the goods in front of the scale without settling what either person owes later. Mara keeps the exchange narrow enough that the buyer can see the goods and the price on the same side of the page. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: At a later visit, distinguish the completed trade from any separate favor. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No new trade mechanics, price shock, or discount. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 007 — The scale does not settle the whole deal

Mara Veln is speaking with someone whose next decision is affected by the work. A fair weight can settle the goods in front of the scale without settling what either person owes later. Mara keeps the exchange narrow enough that the buyer can see the goods and the price on the same side of the page. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “A weight tells us what is here. It does not tell us what you will remember.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “At a later visit, distinguish the completed trade from any separate favor.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No new trade mechanics, price shock, or discount. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Trade stub: Goods and asking price read aloud; later favor not entered as a charge. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 008 — The scale does not settle the whole deal

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A fair weight can settle the goods in front of the scale without settling what either person owes later. Mara keeps the exchange narrow enough that the buyer can see the goods and the price on the same side of the page. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: At a later visit, distinguish the completed trade from any separate favor. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No new trade mechanics, price shock, or discount.

Mara Veln may say: “A weight tells us what is here. It does not tell us what you will remember.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Trade stub: Goods and asking price read aloud; later favor not entered as a charge. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 009 — A shortage is not an invitation

A buyer recognizes that Mara needs medicine and tries to use the timing to push the price. She names the existing choice in plain terms: the offer is fair at her asking price, or it is a premium she cannot comfortably refuse. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You can call it a bargain. I will call it what it cost you to notice I was short.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not create a third answer or moral score.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the exploit branch is chosen, later text may remember the premium without inventing a new penalty.

Scene close: Margin note: Price reflects the stated exchange; pressure is not recorded as consent to future terms. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 010 — A shortage is not an invitation

Proposed diegetic text: “Margin note: Price reflects the stated exchange; pressure is not recorded as consent to future terms.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A buyer recognizes that Mara needs medicine and tries to use the timing to push the price. She names the existing choice in plain terms: the offer is fair at her asking price, or it is a premium she cannot comfortably refuse. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the exploit branch is chosen, later text may remember the premium without inventing a new penalty. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not create a third answer or moral score. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 011 — A shortage is not an invitation

Mara Veln is speaking with someone whose next decision is affected by the work. A buyer recognizes that Mara needs medicine and tries to use the timing to push the price. She names the existing choice in plain terms: the offer is fair at her asking price, or it is a premium she cannot comfortably refuse. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You can call it a bargain. I will call it what it cost you to notice I was short.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “If the exploit branch is chosen, later text may remember the premium without inventing a new penalty.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not create a third answer or moral score. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Margin note: Price reflects the stated exchange; pressure is not recorded as consent to future terms. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 012 — A shortage is not an invitation

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A buyer recognizes that Mara needs medicine and tries to use the timing to push the price. She names the existing choice in plain terms: the offer is fair at her asking price, or it is a premium she cannot comfortably refuse. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the exploit branch is chosen, later text may remember the premium without inventing a new penalty. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not create a third answer or moral score.

Mara Veln may say: “You can call it a bargain. I will call it what it cost you to notice I was short.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Margin note: Price reflects the stated exchange; pressure is not recorded as consent to future terms. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 013 — The favor is entered once

After the fair trade, Mara writes one brief note about what was handed over. She does not add a second column for the visitor’s character. Her promise to pay favors back with interest remains deliberately imprecise. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “I can remember the favor. I cannot price the spring before it arrives.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not name an exact spring delivery or guarantee the route.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the current `mara_help` choice is true, retain this uncertainty in the callback.

Scene close: Ledger line: Medicine and axle parts received at fair price; future route mentioned, value not yet stated. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 014 — The favor is entered once

Proposed diegetic text: “Ledger line: Medicine and axle parts received at fair price; future route mentioned, value not yet stated.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: After the fair trade, Mara writes one brief note about what was handed over. She does not add a second column for the visitor’s character. Her promise to pay favors back with interest remains deliberately imprecise. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the current `mara_help` choice is true, retain this uncertainty in the callback. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not name an exact spring delivery or guarantee the route. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 015 — The favor is entered once

Mara Veln is speaking with someone whose next decision is affected by the work. After the fair trade, Mara writes one brief note about what was handed over. She does not add a second column for the visitor’s character. Her promise to pay favors back with interest remains deliberately imprecise. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “I can remember the favor. I cannot price the spring before it arrives.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “If the current `mara_help` choice is true, retain this uncertainty in the callback.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not name an exact spring delivery or guarantee the route. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Ledger line: Medicine and axle parts received at fair price; future route mentioned, value not yet stated. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 016 — The favor is entered once

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. After the fair trade, Mara writes one brief note about what was handed over. She does not add a second column for the visitor’s character. Her promise to pay favors back with interest remains deliberately imprecise. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the current `mara_help` choice is true, retain this uncertainty in the callback. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not name an exact spring delivery or guarantee the route.

Mara Veln may say: “I can remember the favor. I cannot price the spring before it arrives.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Ledger line: Medicine and axle parts received at fair price; future route mentioned, value not yet stated. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 017 — The route hand wants a number

A route hand asks whether the promised spring route is a real promise. Mara answers that she named a route she intends to run, not a delivery date or a guaranteed place on a wagon. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “I told you where I mean to go. I did not tell you that I can carry everything.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No schedule, capacity, or route edge is added.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. A later notice may add only what the current route owner has authored.

Scene close: Route note: Intended spring run; date and capacity remain unstated. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 018 — The route hand wants a number

Proposed diegetic text: “Route note: Intended spring run; date and capacity remain unstated.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A route hand asks whether the promised spring route is a real promise. Mara answers that she named a route she intends to run, not a delivery date or a guaranteed place on a wagon. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: A later notice may add only what the current route owner has authored. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No schedule, capacity, or route edge is added. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 019 — The route hand wants a number

Mara Veln is speaking with someone whose next decision is affected by the work. A route hand asks whether the promised spring route is a real promise. Mara answers that she named a route she intends to run, not a delivery date or a guaranteed place on a wagon. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “I told you where I mean to go. I did not tell you that I can carry everything.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “A later notice may add only what the current route owner has authored.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No schedule, capacity, or route edge is added. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Route note: Intended spring run; date and capacity remain unstated. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 020 — The route hand wants a number

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A route hand asks whether the promised spring route is a real promise. Mara answers that she named a route she intends to run, not a delivery date or a guaranteed place on a wagon. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: A later notice may add only what the current route owner has authored. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No schedule, capacity, or route edge is added.

Mara Veln may say: “I told you where I mean to go. I did not tell you that I can carry everything.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Route note: Intended spring run; date and capacity remain unstated. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 021 — A price said out loud

A second buyer has heard that Mara’s prices are fast and assumes a quick answer is a private answer. Mara repeats the asking price at a volume that lets the next person in line hear the same terms. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “Fast is not the same thing as hidden. I can say it again.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not imply a fixed market-wide rate.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. On the return, only Mara’s own terms may be quoted as her terms.

Scene close: Public copy: Asking price spoken where both sides of the exchange can hear it. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 022 — A price said out loud

Proposed diegetic text: “Public copy: Asking price spoken where both sides of the exchange can hear it.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A second buyer has heard that Mara’s prices are fast and assumes a quick answer is a private answer. Mara repeats the asking price at a volume that lets the next person in line hear the same terms. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: On the return, only Mara’s own terms may be quoted as her terms. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not imply a fixed market-wide rate. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 023 — A price said out loud

Mara Veln is speaking with someone whose next decision is affected by the work. A second buyer has heard that Mara’s prices are fast and assumes a quick answer is a private answer. Mara repeats the asking price at a volume that lets the next person in line hear the same terms. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “Fast is not the same thing as hidden. I can say it again.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “On the return, only Mara’s own terms may be quoted as her terms.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not imply a fixed market-wide rate. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Public copy: Asking price spoken where both sides of the exchange can hear it. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 024 — A price said out loud

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A second buyer has heard that Mara’s prices are fast and assumes a quick answer is a private answer. Mara repeats the asking price at a volume that lets the next person in line hear the same terms. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: On the return, only Mara’s own terms may be quoted as her terms. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not imply a fixed market-wide rate.

Mara Veln may say: “Fast is not the same thing as hidden. I can say it again.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Public copy: Asking price spoken where both sides of the exchange can hear it. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 025 — The crossing is remembered without a story

A visitor asks what happened on the bad crossing. Mara refuses to turn the loss into a story that would expose the people who traveled with her. She records the shortage and the effect on the route, not the private details. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You need to know what I need. You do not need their names.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No accident details or new casualties.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Keep the omission in any later copy unless its author chooses otherwise.

Scene close: Ledger note: Bad crossing; medicine and axle parts short; other names omitted. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 026 — The crossing is remembered without a story

Proposed diegetic text: “Ledger note: Bad crossing; medicine and axle parts short; other names omitted.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A visitor asks what happened on the bad crossing. Mara refuses to turn the loss into a story that would expose the people who traveled with her. She records the shortage and the effect on the route, not the private details. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Keep the omission in any later copy unless its author chooses otherwise. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No accident details or new casualties. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 027 — The crossing is remembered without a story

Mara Veln is speaking with someone whose next decision is affected by the work. A visitor asks what happened on the bad crossing. Mara refuses to turn the loss into a story that would expose the people who traveled with her. She records the shortage and the effect on the route, not the private details. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You need to know what I need. You do not need their names.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “Keep the omission in any later copy unless its author chooses otherwise.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No accident details or new casualties. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Ledger note: Bad crossing; medicine and axle parts short; other names omitted. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 028 — The crossing is remembered without a story

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A visitor asks what happened on the bad crossing. Mara refuses to turn the loss into a story that would expose the people who traveled with her. She records the shortage and the effect on the route, not the private details. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Keep the omission in any later copy unless its author chooses otherwise. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No accident details or new casualties.

Mara Veln may say: “You need to know what I need. You do not need their names.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Ledger note: Bad crossing; medicine and axle parts short; other names omitted. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 029 — The road crust takes the second wagon

At the Cut Road, the wagon is already through the crust and the harness remains on. Mara is camped on the wreck with the cargo she can guard. The scene stops at the visible problem; nobody demonstrates a way to recover the vehicle. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “This one is not moving because you ask it to. We can talk about the people first.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No recovery procedure, path advice, or certainty about cargo.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. At return, leave the wagon’s state as the existing encounter and branch define it.

Scene close: Route card: Second wagon lost to the road crust; owner present; cargo status unresolved. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 030 — The road crust takes the second wagon

Proposed diegetic text: “Route card: Second wagon lost to the road crust; owner present; cargo status unresolved.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: At the Cut Road, the wagon is already through the crust and the harness remains on. Mara is camped on the wreck with the cargo she can guard. The scene stops at the visible problem; nobody demonstrates a way to recover the vehicle. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: At return, leave the wagon’s state as the existing encounter and branch define it. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No recovery procedure, path advice, or certainty about cargo. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 031 — The road crust takes the second wagon

Mara Veln is speaking with someone whose next decision is affected by the work. At the Cut Road, the wagon is already through the crust and the harness remains on. Mara is camped on the wreck with the cargo she can guard. The scene stops at the visible problem; nobody demonstrates a way to recover the vehicle. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “This one is not moving because you ask it to. We can talk about the people first.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “At return, leave the wagon’s state as the existing encounter and branch define it.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No recovery procedure, path advice, or certainty about cargo. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Route card: Second wagon lost to the road crust; owner present; cargo status unresolved. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 032 — The road crust takes the second wagon

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. At the Cut Road, the wagon is already through the crust and the harness remains on. Mara is camped on the wreck with the cargo she can guard. The scene stops at the visible problem; nobody demonstrates a way to recover the vehicle. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: At return, leave the wagon’s state as the existing encounter and branch define it. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No recovery procedure, path advice, or certainty about cargo.

Mara Veln may say: “This one is not moving because you ask it to. We can talk about the people first.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Route card: Second wagon lost to the road crust; owner present; cargo status unresolved. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 033 — Prices before faces

Mara starts naming prices before the arriving column is close enough to see her face clearly. A player can hear that as calculation or self-protection; the scene does not choose an interpretation for them. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You are close enough to hear me. That is close enough to trade.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not portray the rifle as combat bait or add a combat solution.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the player passed, keep the later account consistent with loose cargo and the road left behind.

Scene close: Observer note: Owner quoted terms before the visitors reached the wreck. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 034 — Prices before faces

Proposed diegetic text: “Observer note: Owner quoted terms before the visitors reached the wreck.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Mara starts naming prices before the arriving column is close enough to see her face clearly. A player can hear that as calculation or self-protection; the scene does not choose an interpretation for them. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the player passed, keep the later account consistent with loose cargo and the road left behind. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not portray the rifle as combat bait or add a combat solution. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 035 — Prices before faces

Mara Veln is speaking with someone whose next decision is affected by the work. Mara starts naming prices before the arriving column is close enough to see her face clearly. A player can hear that as calculation or self-protection; the scene does not choose an interpretation for them. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You are close enough to hear me. That is close enough to trade.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “If the player passed, keep the later account consistent with loose cargo and the road left behind.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not portray the rifle as combat bait or add a combat solution. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Observer note: Owner quoted terms before the visitors reached the wreck. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 036 — Prices before faces

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Mara starts naming prices before the arriving column is close enough to see her face clearly. A player can hear that as calculation or self-protection; the scene does not choose an interpretation for them. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the player passed, keep the later account consistent with loose cargo and the road left behind. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not portray the rifle as combat bait or add a combat solution.

Mara Veln may say: “You are close enough to hear me. That is close enough to trade.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Observer note: Owner quoted terms before the visitors reached the wreck. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 037 — Water and a day and a half

The rescue choice names its cost: a day and a half of water, the wagon pulled out, and Mara’s people walked to the caravanserai. The prose shows the waiting and the altered pace without adding a tally or making a thank-you the reward. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You paid in the thing you had to carry. I have written that down.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not change the authored cost or add another resource.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. For `mara_rescue`, use a recognition that does not become a free rate.

Scene close: Rescue copy: Water and time spent; people walked in; standing recorded by Mara. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 038 — Water and a day and a half

Proposed diegetic text: “Rescue copy: Water and time spent; people walked in; standing recorded by Mara.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The rescue choice names its cost: a day and a half of water, the wagon pulled out, and Mara’s people walked to the caravanserai. The prose shows the waiting and the altered pace without adding a tally or making a thank-you the reward. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: For `mara_rescue`, use a recognition that does not become a free rate. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not change the authored cost or add another resource. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 039 — Water and a day and a half

Mara Veln is speaking with someone whose next decision is affected by the work. The rescue choice names its cost: a day and a half of water, the wagon pulled out, and Mara’s people walked to the caravanserai. The prose shows the waiting and the altered pace without adding a tally or making a thank-you the reward. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You paid in the thing you had to carry. I have written that down.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “For `mara_rescue`, use a recognition that does not become a free rate.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not change the authored cost or add another resource. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Rescue copy: Water and time spent; people walked in; standing recorded by Mara. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 040 — Water and a day and a half

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The rescue choice names its cost: a day and a half of water, the wagon pulled out, and Mara’s people walked to the caravanserai. The prose shows the waiting and the altered pace without adding a tally or making a thank-you the reward. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: For `mara_rescue`, use a recognition that does not become a free rate. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not change the authored cost or add another resource.

Mara Veln may say: “You paid in the thing you had to carry. I have written that down.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Rescue copy: Water and time spent; people walked in; standing recorded by Mara. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 041 — Loose cargo and the road left due

In the pass version, the trade is limited to what is loose and the road is left to itself. Mara remembers the choice as a story that travels, but she does not tell the player what every buyer will say. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “Take what you bought. Leave the rest of the account where it is.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No inventory reward beyond the current quest outcome.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. For `mara_pass`, do not later write a rescue into the record.

Scene close: Route copy: Loose goods exchanged; wagon and route unresolved. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 042 — Loose cargo and the road left due

Proposed diegetic text: “Route copy: Loose goods exchanged; wagon and route unresolved.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: In the pass version, the trade is limited to what is loose and the road is left to itself. Mara remembers the choice as a story that travels, but she does not tell the player what every buyer will say. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: For `mara_pass`, do not later write a rescue into the record. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No inventory reward beyond the current quest outcome. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 043 — Loose cargo and the road left due

Mara Veln is speaking with someone whose next decision is affected by the work. In the pass version, the trade is limited to what is loose and the road is left to itself. Mara remembers the choice as a story that travels, but she does not tell the player what every buyer will say. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “Take what you bought. Leave the rest of the account where it is.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “For `mara_pass`, do not later write a rescue into the record.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No inventory reward beyond the current quest outcome. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Route copy: Loose goods exchanged; wagon and route unresolved. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 044 — Loose cargo and the road left due

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. In the pass version, the trade is limited to what is loose and the road is left to itself. Mara remembers the choice as a story that travels, but she does not tell the player what every buyer will say. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: For `mara_pass`, do not later write a rescue into the record. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No inventory reward beyond the current quest outcome.

Mara Veln may say: “Take what you bought. Leave the rest of the account where it is.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Route copy: Loose goods exchanged; wagon and route unresolved. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 045 — One wagon lighter at the caravanserai

The next version depends on the current arc state: the route may continue on schedule one wagon lighter, rebuild independently, or move under an official stamp. Mara speaks from the state that actually applies, not from the most dramatic alternative. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “The route has a state. It does not have every state at once.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not merge late states or invent a fourth branch.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Use the exact `npc_arcs.json` state condition before displaying any variant.

Scene close: Caravanserai note: Current route state only; inactive outcomes not copied into this page. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 046 — One wagon lighter at the caravanserai

Proposed diegetic text: “Caravanserai note: Current route state only; inactive outcomes not copied into this page.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: The next version depends on the current arc state: the route may continue on schedule one wagon lighter, rebuild independently, or move under an official stamp. Mara speaks from the state that actually applies, not from the most dramatic alternative. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Use the exact `npc_arcs.json` state condition before displaying any variant. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not merge late states or invent a fourth branch. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 047 — One wagon lighter at the caravanserai

Mara Veln is speaking with someone whose next decision is affected by the work. The next version depends on the current arc state: the route may continue on schedule one wagon lighter, rebuild independently, or move under an official stamp. Mara speaks from the state that actually applies, not from the most dramatic alternative. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “The route has a state. It does not have every state at once.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “Use the exact `npc_arcs.json` state condition before displaying any variant.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not merge late states or invent a fourth branch. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Caravanserai note: Current route state only; inactive outcomes not copied into this page. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 048 — One wagon lighter at the caravanserai

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. The next version depends on the current arc state: the route may continue on schedule one wagon lighter, rebuild independently, or move under an official stamp. Mara speaks from the state that actually applies, not from the most dramatic alternative. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Use the exact `npc_arcs.json` state condition before displaying any variant. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not merge late states or invent a fourth branch.

Mara Veln may say: “The route has a state. It does not have every state at once.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Caravanserai note: Current route state only; inactive outcomes not copied into this page. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 049 — A stamp has an audience

If the official route-coordinator state applies, the stamp makes access easier and the rate sheet has a friends line. Mara never needs to pretend the water-station start did not happen; a careful callback can hold both facts in the same scene. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “The stamp opens a column. It does not erase the road that got me here.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not grant coordinator authority outside the current late state.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. For independent or embargo outcomes, do not show this stamp or its terms.

Scene close: Rate sheet margin: Coordinator’s rate applies only to the existing listed case; hand and date visible. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 050 — A stamp has an audience

Proposed diegetic text: “Rate sheet margin: Coordinator’s rate applies only to the existing listed case; hand and date visible.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the official route-coordinator state applies, the stamp makes access easier and the rate sheet has a friends line. Mara never needs to pretend the water-station start did not happen; a careful callback can hold both facts in the same scene. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: For independent or embargo outcomes, do not show this stamp or its terms. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not grant coordinator authority outside the current late state. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 051 — A stamp has an audience

Mara Veln is speaking with someone whose next decision is affected by the work. If the official route-coordinator state applies, the stamp makes access easier and the rate sheet has a friends line. Mara never needs to pretend the water-station start did not happen; a careful callback can hold both facts in the same scene. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “The stamp opens a column. It does not erase the road that got me here.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “For independent or embargo outcomes, do not show this stamp or its terms.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not grant coordinator authority outside the current late state. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Rate sheet margin: Coordinator’s rate applies only to the existing listed case; hand and date visible. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 052 — A stamp has an audience

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the official route-coordinator state applies, the stamp makes access easier and the rate sheet has a friends line. Mara never needs to pretend the water-station start did not happen; a careful callback can hold both facts in the same scene. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: For independent or embargo outcomes, do not show this stamp or its terms. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not grant coordinator authority outside the current late state.

Mara Veln may say: “The stamp opens a column. It does not erase the road that got me here.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Rate sheet margin: Coordinator’s rate applies only to the existing listed case; hand and date visible. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 053 — The drawer after the route

If the current terminal state says Mara is dead, the grain exchange keeps her ledgers in a drawer nobody opens. A brief proposed image can show the drawer being left shut; no new posthumous message is invented. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You can keep a record without making it speak for her.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not add a last letter or override the dead state.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. This callback is only for the existing deceased state; omit it in all living branches.

Scene close: Archive label: Caravan ledgers retained; contents not summarized. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 054 — The drawer after the route

Proposed diegetic text: “Archive label: Caravan ledgers retained; contents not summarized.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the current terminal state says Mara is dead, the grain exchange keeps her ledgers in a drawer nobody opens. A brief proposed image can show the drawer being left shut; no new posthumous message is invented. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: This callback is only for the existing deceased state; omit it in all living branches. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not add a last letter or override the dead state. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 055 — The drawer after the route

Mara Veln is speaking with someone whose next decision is affected by the work. If the current terminal state says Mara is dead, the grain exchange keeps her ledgers in a drawer nobody opens. A brief proposed image can show the drawer being left shut; no new posthumous message is invented. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You can keep a record without making it speak for her.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “This callback is only for the existing deceased state; omit it in all living branches.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not add a last letter or override the dead state. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Archive label: Caravan ledgers retained; contents not summarized. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 056 — The drawer after the route

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the current terminal state says Mara is dead, the grain exchange keeps her ledgers in a drawer nobody opens. A brief proposed image can show the drawer being left shut; no new posthumous message is invented. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: This callback is only for the existing deceased state; omit it in all living branches. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not add a last letter or override the dead state.

Mara Veln may say: “You can keep a record without making it speak for her.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Archive label: Caravan ledgers retained; contents not summarized. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 057 — The second copy stays with the keeper

At the water station a visitor asks Mara to hand over the ledger page as proof of the trade. She reads the relevant line aloud, then closes the book and keeps it in her own hands. The exchange is legible without making her records public property. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “You can remember what I agreed to. You do not need to own the page that says it.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. No ledger item, theft branch, or new permission is created.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. On later visits, show the same distinction between a readable term and a transferable record.

Scene close: Keeper’s note: Terms read aloud; original retained by Mara. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 058 — The second copy stays with the keeper

Proposed diegetic text: “Keeper’s note: Terms read aloud; original retained by Mara.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: At the water station a visitor asks Mara to hand over the ledger page as proof of the trade. She reads the relevant line aloud, then closes the book and keeps it in her own hands. The exchange is legible without making her records public property. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: On later visits, show the same distinction between a readable term and a transferable record. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. No ledger item, theft branch, or new permission is created. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 059 — The second copy stays with the keeper

Mara Veln is speaking with someone whose next decision is affected by the work. At the water station a visitor asks Mara to hand over the ledger page as proof of the trade. She reads the relevant line aloud, then closes the book and keeps it in her own hands. The exchange is legible without making her records public property. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “You can remember what I agreed to. You do not need to own the page that says it.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “On later visits, show the same distinction between a readable term and a transferable record.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. No ledger item, theft branch, or new permission is created. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Keeper’s note: Terms read aloud; original retained by Mara. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 060 — The second copy stays with the keeper

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. At the water station a visitor asks Mara to hand over the ledger page as proof of the trade. She reads the relevant line aloud, then closes the book and keeps it in her own hands. The exchange is legible without making her records public property. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: On later visits, show the same distinction between a readable term and a transferable record. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. No ledger item, theft branch, or new permission is created.

Mara Veln may say: “You can remember what I agreed to. You do not need to own the page that says it.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Keeper’s note: Terms read aloud; original retained by Mara. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 061 — A passenger place has a limit

A traveler hears that Mara offers space in a caravan and assumes that the offer has no conditions. She points to the actual wagons and asks what the traveler expects to bring. The line becomes a conversation about capacity, not a guaranteed seat or a new booking system. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “I offered room to travel. I did not promise room for everything you own.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not define capacity, passenger eligibility, or an additional route reward.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Keep any later invitation within the current character state and written offer.

Scene close: Route margin: Space offered under existing terms; capacity not stated here. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 062 — A passenger place has a limit

Proposed diegetic text: “Route margin: Space offered under existing terms; capacity not stated here.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: A traveler hears that Mara offers space in a caravan and assumes that the offer has no conditions. She points to the actual wagons and asks what the traveler expects to bring. The line becomes a conversation about capacity, not a guaranteed seat or a new booking system. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Keep any later invitation within the current character state and written offer. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not define capacity, passenger eligibility, or an additional route reward. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 063 — A passenger place has a limit

Mara Veln is speaking with someone whose next decision is affected by the work. A traveler hears that Mara offers space in a caravan and assumes that the offer has no conditions. She points to the actual wagons and asks what the traveler expects to bring. The line becomes a conversation about capacity, not a guaranteed seat or a new booking system. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “I offered room to travel. I did not promise room for everything you own.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “Keep any later invitation within the current character state and written offer.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not define capacity, passenger eligibility, or an additional route reward. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Route margin: Space offered under existing terms; capacity not stated here. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 064 — A passenger place has a limit

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. A traveler hears that Mara offers space in a caravan and assumes that the offer has no conditions. She points to the actual wagons and asks what the traveler expects to bring. The line becomes a conversation about capacity, not a guaranteed seat or a new booking system. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Keep any later invitation within the current character state and written offer. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not define capacity, passenger eligibility, or an additional route reward.

Mara Veln may say: “I offered room to travel. I did not promise room for everything you own.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Route margin: Space offered under existing terms; capacity not stated here. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 065 — A helper keeps their own account

Someone who helped unload speaks about the work in the first person rather than being folded into Mara’s gratitude. Mara lets them correct the record if the words fail to match what they remember. The player is present for a disagreement that does not need to become a new branch. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “I can tell you what I saw. I cannot tell you what it meant to them.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not add a companion NPC identity, labor total, or hidden relationship value.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. If the passage returns, let the helper’s own account remain distinct from Mara’s.

Scene close: Margin correction: Ask the person who carried it; do not speak for them. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 066 — A helper keeps their own account

Proposed diegetic text: “Margin correction: Ask the person who carried it; do not speak for them.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: Someone who helped unload speaks about the work in the first person rather than being folded into Mara’s gratitude. Mara lets them correct the record if the words fail to match what they remember. The player is present for a disagreement that does not need to become a new branch. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: If the passage returns, let the helper’s own account remain distinct from Mara’s. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not add a companion NPC identity, labor total, or hidden relationship value. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 067 — A helper keeps their own account

Mara Veln is speaking with someone whose next decision is affected by the work. Someone who helped unload speaks about the work in the first person rather than being folded into Mara’s gratitude. Mara lets them correct the record if the words fail to match what they remember. The player is present for a disagreement that does not need to become a new branch. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “I can tell you what I saw. I cannot tell you what it meant to them.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “If the passage returns, let the helper’s own account remain distinct from Mara’s.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not add a companion NPC identity, labor total, or hidden relationship value. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Margin correction: Ask the person who carried it; do not speak for them. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 068 — A helper keeps their own account

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. Someone who helped unload speaks about the work in the first person rather than being folded into Mara’s gratitude. Mara lets them correct the record if the words fail to match what they remember. The player is present for a disagreement that does not need to become a new branch. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: If the passage returns, let the helper’s own account remain distinct from Mara’s. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not add a companion NPC identity, labor total, or hidden relationship value.

Mara Veln may say: “I can tell you what I saw. I cannot tell you what it meant to them.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Margin correction: Ask the person who carried it; do not speak for them. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 069 — Independent is not the same as untroubled

If the independent late state applies, the route runs without the official stamp. Mara can be relieved by that fact and still count what it costs to do without institutional help. The scene does not imply that independence resolves the shortages or guarantees safety. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “I can keep my own terms. That does not make the road generous.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Use only the authored independent state; do not combine it with the coordinator outcome.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. A return may acknowledge autonomy while leaving current material limits intact.

Scene close: Route copy: Independent operation recorded; no guarantee of supply or passage. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 070 — Independent is not the same as untroubled

Proposed diegetic text: “Route copy: Independent operation recorded; no guarantee of supply or passage.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the independent late state applies, the route runs without the official stamp. Mara can be relieved by that fact and still count what it costs to do without institutional help. The scene does not imply that independence resolves the shortages or guarantees safety. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: A return may acknowledge autonomy while leaving current material limits intact. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Use only the authored independent state; do not combine it with the coordinator outcome. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 071 — Independent is not the same as untroubled

Mara Veln is speaking with someone whose next decision is affected by the work. If the independent late state applies, the route runs without the official stamp. Mara can be relieved by that fact and still count what it costs to do without institutional help. The scene does not imply that independence resolves the shortages or guarantees safety. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “I can keep my own terms. That does not make the road generous.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “A return may acknowledge autonomy while leaving current material limits intact.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Use only the authored independent state; do not combine it with the coordinator outcome. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Route copy: Independent operation recorded; no guarantee of supply or passage. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 072 — Independent is not the same as untroubled

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the independent late state applies, the route runs without the official stamp. Mara can be relieved by that fact and still count what it costs to do without institutional help. The scene does not imply that independence resolves the shortages or guarantees safety. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: A return may acknowledge autonomy while leaving current material limits intact. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Use only the authored independent state; do not combine it with the coordinator outcome.

Mara Veln may say: “I can keep my own terms. That does not make the road generous.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Route copy: Independent operation recorded; no guarantee of supply or passage. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

### Scene draft 073 — The embargo line has no buyer attached

If the existing embargo response applies, a short notice says that a buyer has refused the route’s terms. It names neither the buyer nor a workaround. Mara can remove the notice when it is no longer useful without turning the page into a permanent public accusation. Let the first image belong to the place and to the people who were already working there. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. The visitor is not the reason the scene exists; they arrive while the work is in progress and can learn by watching before anyone asks for help. No one has to deliver a history lesson. A correction, a pause, or a paper moved within reach can carry the information.

Mara Veln: “A closed door is information. It is not permission to find another person to blame.” Let the line sound as if its speaker has said a version of it before, under less convenient circumstances. Give the other person room to answer without changing the immediate task into a test of loyalty. A small response may be enough: a hand held over the page, a chair turned toward the speaker, a tool put down until the sentence is finished. Do not introduce a named buyer, smuggling option, or new embargo consequence.

The human question beneath the exchange is what makes a favor different from a debt when both people can remember the exchange? Keep that question in the objects and the timing, not in an explanatory speech. If a player asks what happens next, the answer should stay within what this person can know today. Display only while the matching state is active; remove it when the current content says so.

Scene close: Notice: Trade refused under the current embargo response; no party named. Leave the final action with the person who owns the record or task. The player can offer, wait, refuse, or leave. The scene does not convert a refusal into secret suspicion or a private act into public permission.

### Record and return 074 — The embargo line has no buyer attached

Proposed diegetic text: “Notice: Trade refused under the current embargo response; no party named.” Keep the in-world text brief enough to read on the surface where it would plausibly appear. This expanded version is a drafting bank: an editor can take the heading, one sentence, or the whole note. The artifact should identify its intended audience and its author only when the author has chosen to sign it. A blank field can remain blank.

Context for placement: If the existing embargo response applies, a short notice says that a buyer has refused the route’s terms. It names neither the buyer nor a workaround. Mara can remove the notice when it is no longer useful without turning the page into a permanent public accusation. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. A public copy should not expose a person's private account merely because the words fit on the page. If someone asks to take the record away, the keeper can say no without the scene forcing a theft or turning the refusal into a branch.

Later reading: Display only while the matching state is active; remove it when the current content says so. Preserve the earlier wording when a correction matters. A changed paper should make clear what the author now knows and what remains uncertain. Do not introduce a named buyer, smuggling option, or new embargo consequence. The new text is a proposed use of the current story surface, not a new database, quest flag, save field, or permission rule.

### Conversation fragment 075 — The embargo line has no buyer attached

Mara Veln is speaking with someone whose next decision is affected by the work. If the existing embargo response applies, a short notice says that a buyer has refused the route’s terms. It names neither the buyer nor a workaround. Mara can remove the notice when it is no longer useful without turning the page into a permanent public accusation. Keep the talk close to what each person has actually seen. They may share a room without sharing a conclusion. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her.

Mara Veln: “A closed door is information. It is not permission to find another person to blame.”
Other voice: “A route hand looks at the same line from the other side of the wagon: “I remember what we carried. I do not remember agreeing to owe for the weather.””
Mara Veln: “Display only while the matching state is active; remove it when the current content says so.”

A player can answer with a practical question, offer help, ask for time, or remain silent. The reply should address the words just spoken; it should not reveal a hidden score or make consent a price of progress. Do not introduce a named buyer, smuggling option, or new embargo consequence. The proposed fragment belongs only at a point where its existing story condition is already true. It does not add a second version of an established choice.

Close on a work detail: Notice: Trade refused under the current embargo response; no party named. Let the people return to their own tasks before the conversation feels complete. The unfinished part is allowed to remain in the room.

### Consequence vignette 076 — The embargo line has no buyer attached

On a later visit permitted by the current story state, the player can see what the earlier exchange left behind. If the existing embargo response applies, a short notice says that a buyer has refused the route’s terms. It names neither the buyer nor a workaround. Mara can remove the notice when it is no longer useful without turning the page into a permanent public accusation. The return does not need a speech announcing its meaning. Mara is exact, quick, and unsentimental about costs. Her speech uses weight, time, and what remains after a deal. She does not beg, explain a buyer, or turn a favor into a debt without saying what she is doing. Other route workers can be tired and specific without becoming named spokespeople for her. Let the changed detail be small enough that a person might miss it on a first pass, but clear enough that someone who remembers the earlier wording can recognize it.

Observable return: Display only while the matching state is active; remove it when the current content says so. The follow-up can change who answers a question, what wording is trusted, or whether an offered place remains available. It must not silently change an existing route, quest result, resource amount, faction standing, or character state. Do not introduce a named buyer, smuggling option, or new embargo consequence.

Mara Veln may say: “A closed door is information. It is not permission to find another person to blame.” The response from the other person can be agreement, correction, silence, or a different practical concern. Do not force gratitude. The scene is complete when the keeper resumes the work or decides not to.

Record left in view: Notice: Trade refused under the current embargo response; no party named. This is candidate prose for an authored content surface. Place it only under the state conditions named in this plan; otherwise leave the current encounter text authoritative.

## 12. Short line bank

- If you came to look, look. If you came to trade, let us start with what is missing.
- A weight tells us what is here. It does not tell us what you will remember.
- You can call it a bargain. I will call it what it cost you to notice I was short.
- I can remember the favor. I cannot price the spring before it arrives.
- I told you where I mean to go. I did not tell you that I can carry everything.
- Fast is not the same thing as hidden. I can say it again.
- You need to know what I need. You do not need their names.
- This one is not moving because you ask it to. We can talk about the people first.
- You are close enough to hear me. That is close enough to trade.
- You paid in the thing you had to carry. I have written that down.
- Take what you bought. Leave the rest of the account where it is.
- The route has a state. It does not have every state at once.
- The stamp opens a column. It does not erase the road that got me here.
- You can keep a record without making it speak for her.
- You can remember what I agreed to. You do not need to own the page that says it.
- I offered room to travel. I did not promise room for everything you own.
- I can tell you what I saw. I cannot tell you what it meant to them.
- I can keep my own terms. That does not make the road generous.
- A closed door is information. It is not permission to find another person to blame.

## 13. Continuity and editorial review

Check every Mara fragment against the exact current choice path. Do not use a late-state line before the corresponding conditions hold. Keep the dead and recruited terminal states separate. Her two-wagon route, water-station start, Cut Road wreck, and eventual route outcomes are source facts; all additional named hands, marks, and follow-up scenes remain proposed. Keep all proposed versions clearly mapped to their existing branch condition. Confirm each source fact against the current JSON before implementation; do not treat a long prose draft as permission to rewrite a canon choice, route, or save contract. A shorter selected set is expected in the game.

## 14. Acceptance boundary

This content plan is ready for editorial review when a writer can select the fragments that fit the existing campaign state, preserve the character's agency and voice, and stage the result without inventing a new game system or contradicting authored data. It does not authorize production implementation. The character count is the Unicode character count of the saved Markdown file.
