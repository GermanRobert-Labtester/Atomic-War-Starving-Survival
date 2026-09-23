# EXPANSION CW36-06 — Bread First, Seed by Rota

## A settlement story about shared bread, guarded seed, and the price of keeping both promises.

### Prose Wave 36: Accounts That Keep Their Limits

## Batch brief

**Content type:** prose-first playable-content plan with original scene, diegetic-record, conversation, and conditional-return drafts.
**Content bank:** 48 story beats with four alternative authored forms per beat (192 candidate passages).
**Current location anchor:** `loc_settlement_silo_burrow` — New Ceres Silo Collective.
**Tone:** grounded, restrained, practical, and explicit about uncertainty.
**Scope:** game content only; no production code, JSON, route, quest, flag, simulation, or save changes.

## 1. Expansion thesis

New Ceres is described as an agrarian refugee commune in three grain silos, raising winter grain and bartering seeds with passing caravans. Its settlement record adds earthen trenches, shared bread by rule, seed guarded by rota, an agriculture trade specialty, Grain Exchange allegiance, friendly attitude, and frequent night raids. This story follows the people who explain, carry, and question those rules. It does not add a government, faction, crop simulation, or settlement outcome.

## 2. Story question

How does a community keep a rule for sharing bread when the seed that makes tomorrow’s bread must be guarded?

## 3. Verified local anchor and source records

The location describes an agricultural commune in three concrete grain silos cultivating winter grain and bartering seeds with passing caravans. The settlement record calls it a refugee commune, says the silos are joined by earthen trenches, states “bread is shared by rule” and “seed is guarded by rota,” names mutual aid and defense of the seed crop as core values, links it to faction_grain_exchange, and lists frequent night raids as an internal tension. Catalog trade fields are not proof of current stock or a particular deal.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `loc_settlement_silo_burrow` | three concrete grain silos in the Verge; winter grain cultivation and seed barter with passing caravans |
| `Assets/StreamingAssets/Data/settlements.json` | `settlement_silo_burrow` | shared bread rule; seed rota; trenches; refugee commune; Grain Exchange allegiance; trade fields; frequent night raids listed as internal tension |

## 4. Fixed canon and open space

The location describes an agricultural commune in three concrete grain silos cultivating winter grain and bartering seeds with passing caravans. The settlement record calls it a refugee commune, says the silos are joined by earthen trenches, states “bread is shared by rule” and “seed is guarded by rota,” names mutual aid and defense of the seed crop as core values, links it to faction_grain_exchange, and lists frequent night raids as an internal tension. Catalog trade fields are not proof of current stock or a particular deal.
Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
Existing characters retain their authored identity and outcomes. New speakers and in-world documents below are editorial drafts until an authorized content owner accepts them. A prose plan does not establish location reachability, artifact placement, a runtime consumer, or an event trigger.

## 5. Human center

A baker wants today’s bread rule to remain legible. A seed-rotation keeper knows the next crop depends on a different kind of restraint. A caravan reader must discuss value without turning the commune’s obligation into a spectacle for outsiders.
The emotional weight should come from what someone records, withholds, repairs, asks, or declines to sign. No narrator announces what the player should feel.

## 6. Voice and point of view

Use anonymous working roles rather than adding canon survivors. Each voice knows only what the cited source, their own observation, or their own record gives them. Buyers, visitors, clerks, and returning workers speak in practical shorthand; none becomes a mouthpiece for the whole settlement. If an in-world document has no established author, make that absence visible.

## 7. Placement and current reachability

The location record is a verified content anchor; that fact alone does not prove a playable route or a prose consumer. Every passage below is a candidate. Before selection, confirm current map reachability, content schema, consumer, and any trigger with the existing owner. No new marker, route, inspection producer, journal type, or return flag is proposed for `loc_settlement_silo_burrow`.

## 8. Player agency

The player can read, ask, carry a line forward, decline to interpret it, or leave. These are candidate prose stances, not a promised choice menu. A player’s refusal must be a complete outcome; the text should never punish caution or force a moral verdict. Any branch that depends on runtime state must reuse the actual owner’s exposed state after verification.

## 9. Continuity, dignity, and safety

Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
Keep descriptions physically grounded. Do not turn unknown people into props, make a hazard into a puzzle tutorial, or use technical language as implied advice. Never promote a catalog note into a new fact about a named character.

## 10. Existing hooks and implementation boundary

Content-only connections: existing settlement, faction-standing, caravan/trade, inventory, food, and crop authorities retain their state and outcomes. The plan creates no extra ration ledger, seed reserve, governance vote, route, raid pressure, or market modifier. Any branch-specific prose must be tied to an already exposed owner state.

**Classification:** editorial game-content proposal; no implementation category is claimed until a current content owner and consumer are verified. Do not create parallel state, a new registry, save section, system, or generic UI callback to host these passages.

## 11. Narrative sequence

### 1. Three silos at the edge of the Verge

The location identifies three concrete silos, and the settlement description places trenches between them.
**Friction:** A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work.
Candidate player distinction: Describe the visible structure or let the resident supply the word “commune.”
Return possibility: A later note carries the distinction from building to community.

### 2. A loaf under a rule

The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.
**Friction:** The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed.
Candidate player distinction: Repeat the authored principle without inventing its administration, or leave the question open.
Return possibility: A later reader knows a rule is described, not that every moment is settled.

### 3. The seed rota

Seed is guarded by rota, and the source names defense of the seed crop as a core value.
**Friction:** The keeper treats the rota as work; an outsider mistakes it for a locked resource.
Candidate player distinction: Describe the responsibility or withhold details about shifts and storage.
Return possibility: The return note shows that a rota can be understood without turning it into a new duty system.

### 4. Winter grain is not a promise

The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.
**Friction:** A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered.
Candidate player distinction: Talk about the crop as present work or leave future quantity unstated.
Return possibility: A later exchange cannot turn cultivation into guaranteed stock.

### 5. A barter sentence

The location says seeds are bartered with passing caravans, not what any one caravan receives.
**Friction:** A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction.
Candidate player distinction: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.
Return possibility: A return callback remembers who spoke plainly, not a fabricated deal.

### 6. An export line in the catalog

The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.
**Friction:** A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count.
Candidate player distinction: Quote the record as a profile or do not use it to promise an item.
Return possibility: The next visitor gets a truthful expectation rather than an order sheet.

### 7. A caravan waits outside the sentence

The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.
**Friction:** The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler.
Candidate player distinction: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.
Return possibility: A future content owner can attach the draft only to a real route and consumer.

### 8. The night tension

Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.
**Friction:** A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident.
Candidate player distinction: Acknowledge the authored tension without staging a fresh raid or naming a raider.
Return possibility: A later passage can refer to concern without inventing a battle.

### 9. Bread and seed share a page

The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.
**Friction:** The two keepers hear different obligations in the same word “shared.”
Candidate player distinction: Let each explain a responsibility, or allow them to disagree about what the sentence means.
Return possibility: The next reader understands that mutual aid can contain distinct rules.

### 10. The visitor’s copy

A visitor writes a summary for people who have not seen the settlement record.
**Friction:** The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible.
Candidate player distinction: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.
Return possibility: The callback checks whether the copy still describes the settlement rather than selling an ideal.

### 11. A friendly field is not a guarantee

The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.
**Friction:** The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome.
Candidate player distinction: Use the data only as context for an authorized consumer or omit it from diegetic speech.
Return possibility: The next encounter must still belong to the actual faction/settlement owner.

### 12. A loaf and an unfilled column

A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.
**Friction:** The keepers accept that the story can close without declaring the rule perfect or the crop secure.
Candidate player distinction: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.
Return possibility: The content leaves New Ceres as a community, not a new economy feature or solved threat.

## 12. Creative variants

### Grounded

Keep the story close to the exact location facts and a single practical exchange. Let the question remain open: How does a community keep a rule for sharing bread when the seed that makes tomorrow’s bread must be guarded?

### Interlinked

Only select a callback if the existing content owner exposes a real source state. Connect narrative context to already-owned systems without changing their rules: Content-only connections: existing settlement, faction-standing, caravan/trade, inventory, food, and crop authorities retain their state and outcomes. The plan creates no extra ration ledger, seed reserve, governance vote, route, raid pressure, or market modifier. Any branch-specific prose must be tied to an already exposed owner state.

### Wild card

Let the same short phrase travel between two proposed authors who mean different things by it. Preserve physical realism and the source limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not a four-step quest. Scene drafts stage a physical observation; records give proposed words an author and audience; conversations make practical disagreement audible; consequence vignettes show how the same words might be carried or refused later. Select only forms that fit a verified current consumer.

A selected passage should identify who speaks, what they can know, why they use these words, and what remains outside the record. Cut any line that sounds like a feature pitch, tutorial, new rule, or universal moral. Keep the player’s option to leave intact. These drafts must be edited for distinct voice and source accuracy before any catalog integration.

## 14. Content bank

### Scene draft 001 — Three silos at the edge of the Verge — first witness

At New Ceres Silo Collective, the location identifies three concrete silos, and the settlement description places trenches between them.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Proposed lines, with speaker assignment left to line-level review: “Three silos. I thought there would be one door.” “There are people here, not just doors.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the visible structure or let the resident supply the word “commune.” The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later note carries the distinction from building to community. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 002 — Three silos at the edge of the Verge — first witness

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Three silos. I thought there would be one door.”
> “The location identifies three concrete silos, and the settlement description places trenches between them.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location identifies three concrete silos, and the settlement description places trenches between them. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later note carries the distinction from building to community. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the visible structure or let the resident supply the word “commune.” If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 003 — Three silos at the edge of the Verge — first witness

The exchange starts before either person has agreed on what the object means. The location identifies three concrete silos, and the settlement description places trenches between them. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Three silos. I thought there would be one door.”
Second voice: “There are people here, not just doors.”
**Under the dialogue:** A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. The practical distinction is: Describe the visible structure or let the resident supply the word “commune.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later note carries the distinction from building to community. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 004 — Three silos at the edge of the Verge — first witness

This conditional vignette belongs after a player has encountered the question in ‘Three silos at the edge of the Verge’. It begins from the authored location fact, not from an invented mission result: The location identifies three concrete silos, and the settlement description places trenches between them. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the visible structure or let the resident supply the word “commune.” The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later note carries the distinction from building to community. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 005 — Three silos at the edge of the Verge — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the location identifies three concrete silos, and the settlement description places trenches between them.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Proposed lines, with speaker assignment left to line-level review: “Three silos. I thought there would be one door.” “There are people here, not just doors.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the visible structure or let the resident supply the word “commune.” The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later note carries the distinction from building to community. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 006 — Three silos at the edge of the Verge — the wording on the page

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The location identifies three concrete silos, and the settlement description places trenches between them.”
> “There are people here, not just doors.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location identifies three concrete silos, and the settlement description places trenches between them. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later note carries the distinction from building to community. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the visible structure or let the resident supply the word “commune.” If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 007 — Three silos at the edge of the Verge — the wording on the page

The page is already open; the argument is about what belongs in its margin. The location identifies three concrete silos, and the settlement description places trenches between them. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Three silos. I thought there would be one door.”
Second voice: “There are people here, not just doors.”
**Under the dialogue:** A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. The practical distinction is: Describe the visible structure or let the resident supply the word “commune.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later note carries the distinction from building to community. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 008 — Three silos at the edge of the Verge — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘Three silos at the edge of the Verge’. It begins from the authored location fact, not from an invented mission result: The location identifies three concrete silos, and the settlement description places trenches between them. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the visible structure or let the resident supply the word “commune.” The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later note carries the distinction from building to community. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 009 — Three silos at the edge of the Verge — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the location identifies three concrete silos, and the settlement description places trenches between them.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Proposed lines, with speaker assignment left to line-level review: “Three silos. I thought there would be one door.” “There are people here, not just doors.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the visible structure or let the resident supply the word “commune.” The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later note carries the distinction from building to community. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 010 — Three silos at the edge of the Verge — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “There are people here, not just doors.”
> “Describe the visible structure or let the resident supply the word “commune.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location identifies three concrete silos, and the settlement description places trenches between them. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later note carries the distinction from building to community. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the visible structure or let the resident supply the word “commune.” If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 011 — Three silos at the edge of the Verge — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The location identifies three concrete silos, and the settlement description places trenches between them. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Three silos. I thought there would be one door.”
Second voice: “There are people here, not just doors.”
**Under the dialogue:** A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. The practical distinction is: Describe the visible structure or let the resident supply the word “commune.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later note carries the distinction from building to community. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 012 — Three silos at the edge of the Verge — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘Three silos at the edge of the Verge’. It begins from the authored location fact, not from an invented mission result: The location identifies three concrete silos, and the settlement description places trenches between them. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the visible structure or let the resident supply the word “commune.” The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later note carries the distinction from building to community. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 013 — Three silos at the edge of the Verge — a return with context

The scene stays close to New Ceres Silo Collective. The location identifies three concrete silos, and the settlement description places trenches between them.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Proposed lines, with speaker assignment left to line-level review: “Three silos. I thought there would be one door.” “There are people here, not just doors.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the visible structure or let the resident supply the word “commune.” The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later note carries the distinction from building to community. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 014 — Three silos at the edge of the Verge — a return with context

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Describe the visible structure or let the resident supply the word “commune.”
> “Three silos. I thought there would be one door.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location identifies three concrete silos, and the settlement description places trenches between them. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later note carries the distinction from building to community. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the visible structure or let the resident supply the word “commune.” If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 015 — Three silos at the edge of the Verge — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The location identifies three concrete silos, and the settlement description places trenches between them. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Three silos. I thought there would be one door.”
Second voice: “There are people here, not just doors.”
**Under the dialogue:** A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. The practical distinction is: Describe the visible structure or let the resident supply the word “commune.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later note carries the distinction from building to community. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 016 — Three silos at the edge of the Verge — a return with context

This conditional vignette belongs after a player has encountered the question in ‘Three silos at the edge of the Verge’. It begins from the authored location fact, not from an invented mission result: The location identifies three concrete silos, and the settlement description places trenches between them. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the visible structure or let the resident supply the word “commune.” The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor calls the arrangement a storehouse; a resident voice calls it a place people live and work. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later note carries the distinction from building to community. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 017 — A loaf under a rule — first witness

At New Ceres Silo Collective, the settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Proposed lines, with speaker assignment left to line-level review: “Who gets the first piece?” “That is not written here.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Repeat the authored principle without inventing its administration, or leave the question open. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later reader knows a rule is described, not that every moment is settled. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 018 — A loaf under a rule — first witness

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Who gets the first piece?”
> “The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later reader knows a rule is described, not that every moment is settled. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Repeat the authored principle without inventing its administration, or leave the question open. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 019 — A loaf under a rule — first witness

The exchange starts before either person has agreed on what the object means. The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Who gets the first piece?”
Second voice: “That is not written here.”
**Under the dialogue:** The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. The practical distinction is: Repeat the authored principle without inventing its administration, or leave the question open.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later reader knows a rule is described, not that every moment is settled. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 020 — A loaf under a rule — first witness

This conditional vignette belongs after a player has encountered the question in ‘A loaf under a rule’. It begins from the authored location fact, not from an invented mission result: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Repeat the authored principle without inventing its administration, or leave the question open. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later reader knows a rule is described, not that every moment is settled. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 021 — A loaf under a rule — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Proposed lines, with speaker assignment left to line-level review: “Who gets the first piece?” “That is not written here.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Repeat the authored principle without inventing its administration, or leave the question open. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later reader knows a rule is described, not that every moment is settled. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 022 — A loaf under a rule — the wording on the page

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.”
> “That is not written here.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later reader knows a rule is described, not that every moment is settled. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Repeat the authored principle without inventing its administration, or leave the question open. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 023 — A loaf under a rule — the wording on the page

The page is already open; the argument is about what belongs in its margin. The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Who gets the first piece?”
Second voice: “That is not written here.”
**Under the dialogue:** The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. The practical distinction is: Repeat the authored principle without inventing its administration, or leave the question open.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later reader knows a rule is described, not that every moment is settled. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 024 — A loaf under a rule — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘A loaf under a rule’. It begins from the authored location fact, not from an invented mission result: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Repeat the authored principle without inventing its administration, or leave the question open. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later reader knows a rule is described, not that every moment is settled. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 025 — A loaf under a rule — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Proposed lines, with speaker assignment left to line-level review: “Who gets the first piece?” “That is not written here.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Repeat the authored principle without inventing its administration, or leave the question open. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later reader knows a rule is described, not that every moment is settled. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 026 — A loaf under a rule — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “That is not written here.”
> “Repeat the authored principle without inventing its administration, or leave the question open.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later reader knows a rule is described, not that every moment is settled. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Repeat the authored principle without inventing its administration, or leave the question open. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 027 — A loaf under a rule — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Who gets the first piece?”
Second voice: “That is not written here.”
**Under the dialogue:** The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. The practical distinction is: Repeat the authored principle without inventing its administration, or leave the question open.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later reader knows a rule is described, not that every moment is settled. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 028 — A loaf under a rule — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘A loaf under a rule’. It begins from the authored location fact, not from an invented mission result: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Repeat the authored principle without inventing its administration, or leave the question open. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later reader knows a rule is described, not that every moment is settled. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 029 — A loaf under a rule — a return with context

The scene stays close to New Ceres Silo Collective. The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Proposed lines, with speaker assignment left to line-level review: “Who gets the first piece?” “That is not written here.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Repeat the authored principle without inventing its administration, or leave the question open. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later reader knows a rule is described, not that every moment is settled. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 030 — A loaf under a rule — a return with context

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Repeat the authored principle without inventing its administration, or leave the question open.”
> “Who gets the first piece?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later reader knows a rule is described, not that every moment is settled. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Repeat the authored principle without inventing its administration, or leave the question open. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 031 — A loaf under a rule — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Who gets the first piece?”
Second voice: “That is not written here.”
**Under the dialogue:** The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. The practical distinction is: Repeat the authored principle without inventing its administration, or leave the question open.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later reader knows a rule is described, not that every moment is settled. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 032 — A loaf under a rule — a return with context

This conditional vignette belongs after a player has encountered the question in ‘A loaf under a rule’. It begins from the authored location fact, not from an invented mission result: The settlement record says bread is shared by rule, but it gives no daily serving, author, or exception. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Repeat the authored principle without inventing its administration, or leave the question open. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The bread keeper wants a visitor to understand the rule; the visitor asks whether the rule has ever failed. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later reader knows a rule is described, not that every moment is settled. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 033 — The seed rota — first witness

At New Ceres Silo Collective, seed is guarded by rota, and the source names defense of the seed crop as a core value.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Proposed lines, with speaker assignment left to line-level review: “You guard seed like medicine.” “We guard tomorrow like something that can be lost.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the responsibility or withhold details about shifts and storage. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The return note shows that a rota can be understood without turning it into a new duty system. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 034 — The seed rota — first witness

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “You guard seed like medicine.”
> “Seed is guarded by rota, and the source names defense of the seed crop as a core value.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Seed is guarded by rota, and the source names defense of the seed crop as a core value. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The return note shows that a rota can be understood without turning it into a new duty system. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the responsibility or withhold details about shifts and storage. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 035 — The seed rota — first witness

The exchange starts before either person has agreed on what the object means. Seed is guarded by rota, and the source names defense of the seed crop as a core value. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “You guard seed like medicine.”
Second voice: “We guard tomorrow like something that can be lost.”
**Under the dialogue:** The keeper treats the rota as work; an outsider mistakes it for a locked resource. The practical distinction is: Describe the responsibility or withhold details about shifts and storage.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The return note shows that a rota can be understood without turning it into a new duty system. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 036 — The seed rota — first witness

This conditional vignette belongs after a player has encountered the question in ‘The seed rota’. It begins from the authored location fact, not from an invented mission result: Seed is guarded by rota, and the source names defense of the seed crop as a core value. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the responsibility or withhold details about shifts and storage. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The return note shows that a rota can be understood without turning it into a new duty system. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 037 — The seed rota — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: seed is guarded by rota, and the source names defense of the seed crop as a core value.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Proposed lines, with speaker assignment left to line-level review: “You guard seed like medicine.” “We guard tomorrow like something that can be lost.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the responsibility or withhold details about shifts and storage. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The return note shows that a rota can be understood without turning it into a new duty system. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 038 — The seed rota — the wording on the page

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Seed is guarded by rota, and the source names defense of the seed crop as a core value.”
> “We guard tomorrow like something that can be lost.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Seed is guarded by rota, and the source names defense of the seed crop as a core value. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The return note shows that a rota can be understood without turning it into a new duty system. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the responsibility or withhold details about shifts and storage. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 039 — The seed rota — the wording on the page

The page is already open; the argument is about what belongs in its margin. Seed is guarded by rota, and the source names defense of the seed crop as a core value. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “You guard seed like medicine.”
Second voice: “We guard tomorrow like something that can be lost.”
**Under the dialogue:** The keeper treats the rota as work; an outsider mistakes it for a locked resource. The practical distinction is: Describe the responsibility or withhold details about shifts and storage.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The return note shows that a rota can be understood without turning it into a new duty system. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 040 — The seed rota — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘The seed rota’. It begins from the authored location fact, not from an invented mission result: Seed is guarded by rota, and the source names defense of the seed crop as a core value. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the responsibility or withhold details about shifts and storage. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The return note shows that a rota can be understood without turning it into a new duty system. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 041 — The seed rota — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: seed is guarded by rota, and the source names defense of the seed crop as a core value.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Proposed lines, with speaker assignment left to line-level review: “You guard seed like medicine.” “We guard tomorrow like something that can be lost.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the responsibility or withhold details about shifts and storage. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The return note shows that a rota can be understood without turning it into a new duty system. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 042 — The seed rota — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “We guard tomorrow like something that can be lost.”
> “Describe the responsibility or withhold details about shifts and storage.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Seed is guarded by rota, and the source names defense of the seed crop as a core value. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The return note shows that a rota can be understood without turning it into a new duty system. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the responsibility or withhold details about shifts and storage. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 043 — The seed rota — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. Seed is guarded by rota, and the source names defense of the seed crop as a core value. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “You guard seed like medicine.”
Second voice: “We guard tomorrow like something that can be lost.”
**Under the dialogue:** The keeper treats the rota as work; an outsider mistakes it for a locked resource. The practical distinction is: Describe the responsibility or withhold details about shifts and storage.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The return note shows that a rota can be understood without turning it into a new duty system. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 044 — The seed rota — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘The seed rota’. It begins from the authored location fact, not from an invented mission result: Seed is guarded by rota, and the source names defense of the seed crop as a core value. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the responsibility or withhold details about shifts and storage. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The return note shows that a rota can be understood without turning it into a new duty system. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 045 — The seed rota — a return with context

The scene stays close to New Ceres Silo Collective. Seed is guarded by rota, and the source names defense of the seed crop as a core value.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Proposed lines, with speaker assignment left to line-level review: “You guard seed like medicine.” “We guard tomorrow like something that can be lost.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Describe the responsibility or withhold details about shifts and storage. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The return note shows that a rota can be understood without turning it into a new duty system. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 046 — The seed rota — a return with context

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Describe the responsibility or withhold details about shifts and storage.”
> “You guard seed like medicine.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Seed is guarded by rota, and the source names defense of the seed crop as a core value. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The return note shows that a rota can be understood without turning it into a new duty system. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Describe the responsibility or withhold details about shifts and storage. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 047 — The seed rota — a return with context

They return to the wording after some time has passed, without pretending the place has answered. Seed is guarded by rota, and the source names defense of the seed crop as a core value. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “You guard seed like medicine.”
Second voice: “We guard tomorrow like something that can be lost.”
**Under the dialogue:** The keeper treats the rota as work; an outsider mistakes it for a locked resource. The practical distinction is: Describe the responsibility or withhold details about shifts and storage.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The return note shows that a rota can be understood without turning it into a new duty system. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 048 — The seed rota — a return with context

This conditional vignette belongs after a player has encountered the question in ‘The seed rota’. It begins from the authored location fact, not from an invented mission result: Seed is guarded by rota, and the source names defense of the seed crop as a core value. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Describe the responsibility or withhold details about shifts and storage. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keeper treats the rota as work; an outsider mistakes it for a locked resource. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The return note shows that a rota can be understood without turning it into a new duty system. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 049 — Winter grain is not a promise — first witness

At New Ceres Silo Collective, the location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Proposed lines, with speaker assignment left to line-level review: “How much will you have?” “Ask the field when it is finished.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Talk about the crop as present work or leave future quantity unstated. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later exchange cannot turn cultivation into guaranteed stock. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 050 — Winter grain is not a promise — first witness

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “How much will you have?”
> “The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later exchange cannot turn cultivation into guaranteed stock. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Talk about the crop as present work or leave future quantity unstated. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 051 — Winter grain is not a promise — first witness

The exchange starts before either person has agreed on what the object means. The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “How much will you have?”
Second voice: “Ask the field when it is finished.”
**Under the dialogue:** A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. The practical distinction is: Talk about the crop as present work or leave future quantity unstated.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later exchange cannot turn cultivation into guaranteed stock. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 052 — Winter grain is not a promise — first witness

This conditional vignette belongs after a player has encountered the question in ‘Winter grain is not a promise’. It begins from the authored location fact, not from an invented mission result: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Talk about the crop as present work or leave future quantity unstated. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later exchange cannot turn cultivation into guaranteed stock. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 053 — Winter grain is not a promise — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Proposed lines, with speaker assignment left to line-level review: “How much will you have?” “Ask the field when it is finished.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Talk about the crop as present work or leave future quantity unstated. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later exchange cannot turn cultivation into guaranteed stock. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 054 — Winter grain is not a promise — the wording on the page

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.”
> “Ask the field when it is finished.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later exchange cannot turn cultivation into guaranteed stock. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Talk about the crop as present work or leave future quantity unstated. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 055 — Winter grain is not a promise — the wording on the page

The page is already open; the argument is about what belongs in its margin. The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “How much will you have?”
Second voice: “Ask the field when it is finished.”
**Under the dialogue:** A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. The practical distinction is: Talk about the crop as present work or leave future quantity unstated.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later exchange cannot turn cultivation into guaranteed stock. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 056 — Winter grain is not a promise — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘Winter grain is not a promise’. It begins from the authored location fact, not from an invented mission result: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Talk about the crop as present work or leave future quantity unstated. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later exchange cannot turn cultivation into guaranteed stock. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 057 — Winter grain is not a promise — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Proposed lines, with speaker assignment left to line-level review: “How much will you have?” “Ask the field when it is finished.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Talk about the crop as present work or leave future quantity unstated. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later exchange cannot turn cultivation into guaranteed stock. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 058 — Winter grain is not a promise — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Ask the field when it is finished.”
> “Talk about the crop as present work or leave future quantity unstated.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later exchange cannot turn cultivation into guaranteed stock. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Talk about the crop as present work or leave future quantity unstated. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 059 — Winter grain is not a promise — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “How much will you have?”
Second voice: “Ask the field when it is finished.”
**Under the dialogue:** A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. The practical distinction is: Talk about the crop as present work or leave future quantity unstated.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later exchange cannot turn cultivation into guaranteed stock. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 060 — Winter grain is not a promise — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘Winter grain is not a promise’. It begins from the authored location fact, not from an invented mission result: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Talk about the crop as present work or leave future quantity unstated. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later exchange cannot turn cultivation into guaranteed stock. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 061 — Winter grain is not a promise — a return with context

The scene stays close to New Ceres Silo Collective. The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Proposed lines, with speaker assignment left to line-level review: “How much will you have?” “Ask the field when it is finished.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Talk about the crop as present work or leave future quantity unstated. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later exchange cannot turn cultivation into guaranteed stock. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 062 — Winter grain is not a promise — a return with context

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Talk about the crop as present work or leave future quantity unstated.”
> “How much will you have?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later exchange cannot turn cultivation into guaranteed stock. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Talk about the crop as present work or leave future quantity unstated. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 063 — Winter grain is not a promise — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “How much will you have?”
Second voice: “Ask the field when it is finished.”
**Under the dialogue:** A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. The practical distinction is: Talk about the crop as present work or leave future quantity unstated.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later exchange cannot turn cultivation into guaranteed stock. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 064 — Winter grain is not a promise — a return with context

This conditional vignette belongs after a player has encountered the question in ‘Winter grain is not a promise’. It begins from the authored location fact, not from an invented mission result: The location says farmers cultivate winter grain, while the settlement describes winter rye adaptation; neither source states a future harvest total. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Talk about the crop as present work or leave future quantity unstated. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A caravan reader wants to quote a volume; the grower refuses to promise what has not been gathered. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later exchange cannot turn cultivation into guaranteed stock. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 065 — A barter sentence — first witness

At New Ceres Silo Collective, the location says seeds are bartered with passing caravans, not what any one caravan receives.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Proposed lines, with speaker assignment left to line-level review: “What do you want for seed?” “First tell us what you think seed is worth.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A return callback remembers who spoke plainly, not a fabricated deal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 066 — A barter sentence — first witness

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “What do you want for seed?”
> “The location says seeds are bartered with passing caravans, not what any one caravan receives.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says seeds are bartered with passing caravans, not what any one caravan receives. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A return callback remembers who spoke plainly, not a fabricated deal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 067 — A barter sentence — first witness

The exchange starts before either person has agreed on what the object means. The location says seeds are bartered with passing caravans, not what any one caravan receives. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do you want for seed?”
Second voice: “First tell us what you think seed is worth.”
**Under the dialogue:** A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. The practical distinction is: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A return callback remembers who spoke plainly, not a fabricated deal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 068 — A barter sentence — first witness

This conditional vignette belongs after a player has encountered the question in ‘A barter sentence’. It begins from the authored location fact, not from an invented mission result: The location says seeds are bartered with passing caravans, not what any one caravan receives. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A return callback remembers who spoke plainly, not a fabricated deal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 069 — A barter sentence — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the location says seeds are bartered with passing caravans, not what any one caravan receives.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Proposed lines, with speaker assignment left to line-level review: “What do you want for seed?” “First tell us what you think seed is worth.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A return callback remembers who spoke plainly, not a fabricated deal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 070 — A barter sentence — the wording on the page

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The location says seeds are bartered with passing caravans, not what any one caravan receives.”
> “First tell us what you think seed is worth.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says seeds are bartered with passing caravans, not what any one caravan receives. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A return callback remembers who spoke plainly, not a fabricated deal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 071 — A barter sentence — the wording on the page

The page is already open; the argument is about what belongs in its margin. The location says seeds are bartered with passing caravans, not what any one caravan receives. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do you want for seed?”
Second voice: “First tell us what you think seed is worth.”
**Under the dialogue:** A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. The practical distinction is: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A return callback remembers who spoke plainly, not a fabricated deal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 072 — A barter sentence — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘A barter sentence’. It begins from the authored location fact, not from an invented mission result: The location says seeds are bartered with passing caravans, not what any one caravan receives. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A return callback remembers who spoke plainly, not a fabricated deal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 073 — A barter sentence — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the location says seeds are bartered with passing caravans, not what any one caravan receives.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Proposed lines, with speaker assignment left to line-level review: “What do you want for seed?” “First tell us what you think seed is worth.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A return callback remembers who spoke plainly, not a fabricated deal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 074 — A barter sentence — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “First tell us what you think seed is worth.”
> “Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says seeds are bartered with passing caravans, not what any one caravan receives. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A return callback remembers who spoke plainly, not a fabricated deal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 075 — A barter sentence — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The location says seeds are bartered with passing caravans, not what any one caravan receives. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do you want for seed?”
Second voice: “First tell us what you think seed is worth.”
**Under the dialogue:** A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. The practical distinction is: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A return callback remembers who spoke plainly, not a fabricated deal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 076 — A barter sentence — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘A barter sentence’. It begins from the authored location fact, not from an invented mission result: The location says seeds are bartered with passing caravans, not what any one caravan receives. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A return callback remembers who spoke plainly, not a fabricated deal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 077 — A barter sentence — a return with context

The scene stays close to New Ceres Silo Collective. The location says seeds are bartered with passing caravans, not what any one caravan receives.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Proposed lines, with speaker assignment left to line-level review: “What do you want for seed?” “First tell us what you think seed is worth.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A return callback remembers who spoke plainly, not a fabricated deal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 078 — A barter sentence — a return with context

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.”
> “What do you want for seed?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The location says seeds are bartered with passing caravans, not what any one caravan receives. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A return callback remembers who spoke plainly, not a fabricated deal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 079 — A barter sentence — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The location says seeds are bartered with passing caravans, not what any one caravan receives. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do you want for seed?”
Second voice: “First tell us what you think seed is worth.”
**Under the dialogue:** A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. The practical distinction is: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A return callback remembers who spoke plainly, not a fabricated deal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 080 — A barter sentence — a return with context

This conditional vignette belongs after a player has encountered the question in ‘A barter sentence’. It begins from the authored location fact, not from an invented mission result: The location says seeds are bartered with passing caravans, not what any one caravan receives. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the dialogue at the level of terms being discussed, or let the negotiation remain incomplete. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants a fixed exchange; the store clerk points out that the catalog goods are not a particular transaction. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A return callback remembers who spoke plainly, not a fabricated deal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 081 — An export line in the catalog — first witness

At New Ceres Silo Collective, the settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Proposed lines, with speaker assignment left to line-level review: “The list says food.” “The list is not the store room.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Quote the record as a profile or do not use it to promise an item. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next visitor gets a truthful expectation rather than an order sheet. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 082 — An export line in the catalog — first witness

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The list says food.”
> “The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next visitor gets a truthful expectation rather than an order sheet. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Quote the record as a profile or do not use it to promise an item. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 083 — An export line in the catalog — first witness

The exchange starts before either person has agreed on what the object means. The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The list says food.”
Second voice: “The list is not the store room.”
**Under the dialogue:** A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. The practical distinction is: Quote the record as a profile or do not use it to promise an item.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next visitor gets a truthful expectation rather than an order sheet. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 084 — An export line in the catalog — first witness

This conditional vignette belongs after a player has encountered the question in ‘An export line in the catalog’. It begins from the authored location fact, not from an invented mission result: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Quote the record as a profile or do not use it to promise an item. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next visitor gets a truthful expectation rather than an order sheet. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 085 — An export line in the catalog — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Proposed lines, with speaker assignment left to line-level review: “The list says food.” “The list is not the store room.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Quote the record as a profile or do not use it to promise an item. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next visitor gets a truthful expectation rather than an order sheet. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 086 — An export line in the catalog — the wording on the page

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.”
> “The list is not the store room.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next visitor gets a truthful expectation rather than an order sheet. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Quote the record as a profile or do not use it to promise an item. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 087 — An export line in the catalog — the wording on the page

The page is already open; the argument is about what belongs in its margin. The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The list says food.”
Second voice: “The list is not the store room.”
**Under the dialogue:** A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. The practical distinction is: Quote the record as a profile or do not use it to promise an item.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next visitor gets a truthful expectation rather than an order sheet. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 088 — An export line in the catalog — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘An export line in the catalog’. It begins from the authored location fact, not from an invented mission result: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Quote the record as a profile or do not use it to promise an item. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next visitor gets a truthful expectation rather than an order sheet. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 089 — An export line in the catalog — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Proposed lines, with speaker assignment left to line-level review: “The list says food.” “The list is not the store room.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Quote the record as a profile or do not use it to promise an item. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next visitor gets a truthful expectation rather than an order sheet. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 090 — An export line in the catalog — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The list is not the store room.”
> “Quote the record as a profile or do not use it to promise an item.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next visitor gets a truthful expectation rather than an order sheet. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Quote the record as a profile or do not use it to promise an item. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 091 — An export line in the catalog — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The list says food.”
Second voice: “The list is not the store room.”
**Under the dialogue:** A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. The practical distinction is: Quote the record as a profile or do not use it to promise an item.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next visitor gets a truthful expectation rather than an order sheet. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 092 — An export line in the catalog — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘An export line in the catalog’. It begins from the authored location fact, not from an invented mission result: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Quote the record as a profile or do not use it to promise an item. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next visitor gets a truthful expectation rather than an order sheet. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 093 — An export line in the catalog — a return with context

The scene stays close to New Ceres Silo Collective. The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Proposed lines, with speaker assignment left to line-level review: “The list says food.” “The list is not the store room.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Quote the record as a profile or do not use it to promise an item. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next visitor gets a truthful expectation rather than an order sheet. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 094 — An export line in the catalog — a return with context

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Quote the record as a profile or do not use it to promise an item.”
> “The list says food.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next visitor gets a truthful expectation rather than an order sheet. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Quote the record as a profile or do not use it to promise an item. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 095 — An export line in the catalog — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The list says food.”
Second voice: “The list is not the store room.”
**Under the dialogue:** A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. The practical distinction is: Quote the record as a profile or do not use it to promise an item.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next visitor gets a truthful expectation rather than an order sheet. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 096 — An export line in the catalog — a return with context

This conditional vignette belongs after a player has encountered the question in ‘An export line in the catalog’. It begins from the authored location fact, not from an invented mission result: The settlement catalog names canned food as a primary export and scrap metal as a primary import; it does not guarantee either is available today. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Quote the record as a profile or do not use it to promise an item. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A buyer reads a catalog specialty as inventory; the clerk distinguishes a broad profile from a shelf count. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next visitor gets a truthful expectation rather than an order sheet. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 097 — A caravan waits outside the sentence — first witness

At New Ceres Silo Collective, the settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Proposed lines, with speaker assignment left to line-level review: “They are due before dark.” “The record does not tell us who is due.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A future content owner can attach the draft only to a real route and consumer. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 098 — A caravan waits outside the sentence — first witness

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “They are due before dark.”
> “The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A future content owner can attach the draft only to a real route and consumer. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 099 — A caravan waits outside the sentence — first witness

The exchange starts before either person has agreed on what the object means. The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “They are due before dark.”
Second voice: “The record does not tell us who is due.”
**Under the dialogue:** The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. The practical distinction is: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A future content owner can attach the draft only to a real route and consumer. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 100 — A caravan waits outside the sentence — first witness

This conditional vignette belongs after a player has encountered the question in ‘A caravan waits outside the sentence’. It begins from the authored location fact, not from an invented mission result: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A future content owner can attach the draft only to a real route and consumer. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 101 — A caravan waits outside the sentence — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Proposed lines, with speaker assignment left to line-level review: “They are due before dark.” “The record does not tell us who is due.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A future content owner can attach the draft only to a real route and consumer. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 102 — A caravan waits outside the sentence — the wording on the page

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.”
> “The record does not tell us who is due.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A future content owner can attach the draft only to a real route and consumer. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 103 — A caravan waits outside the sentence — the wording on the page

The page is already open; the argument is about what belongs in its margin. The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “They are due before dark.”
Second voice: “The record does not tell us who is due.”
**Under the dialogue:** The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. The practical distinction is: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A future content owner can attach the draft only to a real route and consumer. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 104 — A caravan waits outside the sentence — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘A caravan waits outside the sentence’. It begins from the authored location fact, not from an invented mission result: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A future content owner can attach the draft only to a real route and consumer. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 105 — A caravan waits outside the sentence — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Proposed lines, with speaker assignment left to line-level review: “They are due before dark.” “The record does not tell us who is due.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A future content owner can attach the draft only to a real route and consumer. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 106 — A caravan waits outside the sentence — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The record does not tell us who is due.”
> “Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A future content owner can attach the draft only to a real route and consumer. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 107 — A caravan waits outside the sentence — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “They are due before dark.”
Second voice: “The record does not tell us who is due.”
**Under the dialogue:** The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. The practical distinction is: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A future content owner can attach the draft only to a real route and consumer. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 108 — A caravan waits outside the sentence — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘A caravan waits outside the sentence’. It begins from the authored location fact, not from an invented mission result: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A future content owner can attach the draft only to a real route and consumer. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 109 — A caravan waits outside the sentence — a return with context

The scene stays close to New Ceres Silo Collective. The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Proposed lines, with speaker assignment left to line-level review: “They are due before dark.” “The record does not tell us who is due.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A future content owner can attach the draft only to a real route and consumer. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 110 — A caravan waits outside the sentence — a return with context

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.”
> “They are due before dark.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A future content owner can attach the draft only to a real route and consumer. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 111 — A caravan waits outside the sentence — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “They are due before dark.”
Second voice: “The record does not tell us who is due.”
**Under the dialogue:** The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. The practical distinction is: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A future content owner can attach the draft only to a real route and consumer. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 112 — A caravan waits outside the sentence — a return with context

This conditional vignette belongs after a player has encountered the question in ‘A caravan waits outside the sentence’. It begins from the authored location fact, not from an invented mission result: The settlement is linked in data to a caravan route node, but that link alone does not establish current runtime reachability or a meeting. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Keep the caravan as the audience of a proposed note, or defer the encounter until placement is verified. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants a scene to begin with arrival; the note-taker will not invent a specific traveler. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A future content owner can attach the draft only to a real route and consumer. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 113 — The night tension — first witness

At New Ceres Silo Collective, frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Proposed lines, with speaker assignment left to line-level review: “Was last night another one?” “The record says frequent. It does not say last night.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Acknowledge the authored tension without staging a fresh raid or naming a raider. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later passage can refer to concern without inventing a battle. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 114 — The night tension — first witness

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Was last night another one?”
> “Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later passage can refer to concern without inventing a battle. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Acknowledge the authored tension without staging a fresh raid or naming a raider. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 115 — The night tension — first witness

The exchange starts before either person has agreed on what the object means. Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Was last night another one?”
Second voice: “The record says frequent. It does not say last night.”
**Under the dialogue:** A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. The practical distinction is: Acknowledge the authored tension without staging a fresh raid or naming a raider.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later passage can refer to concern without inventing a battle. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 116 — The night tension — first witness

This conditional vignette belongs after a player has encountered the question in ‘The night tension’. It begins from the authored location fact, not from an invented mission result: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Acknowledge the authored tension without staging a fresh raid or naming a raider. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later passage can refer to concern without inventing a battle. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 117 — The night tension — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Proposed lines, with speaker assignment left to line-level review: “Was last night another one?” “The record says frequent. It does not say last night.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Acknowledge the authored tension without staging a fresh raid or naming a raider. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later passage can refer to concern without inventing a battle. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 118 — The night tension — the wording on the page

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.”
> “The record says frequent. It does not say last night.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later passage can refer to concern without inventing a battle. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Acknowledge the authored tension without staging a fresh raid or naming a raider. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 119 — The night tension — the wording on the page

The page is already open; the argument is about what belongs in its margin. Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Was last night another one?”
Second voice: “The record says frequent. It does not say last night.”
**Under the dialogue:** A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. The practical distinction is: Acknowledge the authored tension without staging a fresh raid or naming a raider.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later passage can refer to concern without inventing a battle. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 120 — The night tension — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘The night tension’. It begins from the authored location fact, not from an invented mission result: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Acknowledge the authored tension without staging a fresh raid or naming a raider. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later passage can refer to concern without inventing a battle. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 121 — The night tension — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Proposed lines, with speaker assignment left to line-level review: “Was last night another one?” “The record says frequent. It does not say last night.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Acknowledge the authored tension without staging a fresh raid or naming a raider. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later passage can refer to concern without inventing a battle. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 122 — The night tension — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The record says frequent. It does not say last night.”
> “Acknowledge the authored tension without staging a fresh raid or naming a raider.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later passage can refer to concern without inventing a battle. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Acknowledge the authored tension without staging a fresh raid or naming a raider. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 123 — The night tension — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Was last night another one?”
Second voice: “The record says frequent. It does not say last night.”
**Under the dialogue:** A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. The practical distinction is: Acknowledge the authored tension without staging a fresh raid or naming a raider.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later passage can refer to concern without inventing a battle. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 124 — The night tension — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘The night tension’. It begins from the authored location fact, not from an invented mission result: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Acknowledge the authored tension without staging a fresh raid or naming a raider. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later passage can refer to concern without inventing a battle. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 125 — The night tension — a return with context

The scene stays close to New Ceres Silo Collective. Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Proposed lines, with speaker assignment left to line-level review: “Was last night another one?” “The record says frequent. It does not say last night.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Acknowledge the authored tension without staging a fresh raid or naming a raider. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: A later passage can refer to concern without inventing a battle. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 126 — The night tension — a return with context

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Acknowledge the authored tension without staging a fresh raid or naming a raider.”
> “Was last night another one?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: A later passage can refer to concern without inventing a battle. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Acknowledge the authored tension without staging a fresh raid or naming a raider. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 127 — The night tension — a return with context

They return to the wording after some time has passed, without pretending the place has answered. Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Was last night another one?”
Second voice: “The record says frequent. It does not say last night.”
**Under the dialogue:** A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. The practical distinction is: Acknowledge the authored tension without staging a fresh raid or naming a raider.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: A later passage can refer to concern without inventing a battle. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 128 — The night tension — a return with context

This conditional vignette belongs after a player has encountered the question in ‘The night tension’. It begins from the authored location fact, not from an invented mission result: Frequent night raids testing the perimeter berms appear in the settlement’s internal-tension field. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Acknowledge the authored tension without staging a fresh raid or naming a raider. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: A visitor wants an attacker and a date; a resident voice refuses to turn a recurring pressure into a specific incident. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: A later passage can refer to concern without inventing a battle. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 129 — Bread and seed share a page — first witness

At New Ceres Silo Collective, the story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The two keepers hear different obligations in the same word “shared.” Proposed lines, with speaker assignment left to line-level review: “Bread goes out now.” “Seed has to stay for later.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Let each explain a responsibility, or allow them to disagree about what the sentence means. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next reader understands that mutual aid can contain distinct rules. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 130 — Bread and seed share a page — first witness

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Bread goes out now.”
> “The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next reader understands that mutual aid can contain distinct rules. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The two keepers hear different obligations in the same word “shared.” Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Let each explain a responsibility, or allow them to disagree about what the sentence means. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 131 — Bread and seed share a page — first witness

The exchange starts before either person has agreed on what the object means. The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Bread goes out now.”
Second voice: “Seed has to stay for later.”
**Under the dialogue:** The two keepers hear different obligations in the same word “shared. The practical distinction is: Let each explain a responsibility, or allow them to disagree about what the sentence means.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next reader understands that mutual aid can contain distinct rules. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 132 — Bread and seed share a page — first witness

This conditional vignette belongs after a player has encountered the question in ‘Bread and seed share a page’. It begins from the authored location fact, not from an invented mission result: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Let each explain a responsibility, or allow them to disagree about what the sentence means. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The two keepers hear different obligations in the same word “shared.” Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next reader understands that mutual aid can contain distinct rules. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 133 — Bread and seed share a page — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The two keepers hear different obligations in the same word “shared.” Proposed lines, with speaker assignment left to line-level review: “Bread goes out now.” “Seed has to stay for later.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Let each explain a responsibility, or allow them to disagree about what the sentence means. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next reader understands that mutual aid can contain distinct rules. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 134 — Bread and seed share a page — the wording on the page

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.”
> “Seed has to stay for later.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next reader understands that mutual aid can contain distinct rules. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The two keepers hear different obligations in the same word “shared.” Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Let each explain a responsibility, or allow them to disagree about what the sentence means. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 135 — Bread and seed share a page — the wording on the page

The page is already open; the argument is about what belongs in its margin. The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Bread goes out now.”
Second voice: “Seed has to stay for later.”
**Under the dialogue:** The two keepers hear different obligations in the same word “shared. The practical distinction is: Let each explain a responsibility, or allow them to disagree about what the sentence means.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next reader understands that mutual aid can contain distinct rules. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 136 — Bread and seed share a page — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘Bread and seed share a page’. It begins from the authored location fact, not from an invented mission result: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Let each explain a responsibility, or allow them to disagree about what the sentence means. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The two keepers hear different obligations in the same word “shared.” Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next reader understands that mutual aid can contain distinct rules. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 137 — Bread and seed share a page — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The two keepers hear different obligations in the same word “shared.” Proposed lines, with speaker assignment left to line-level review: “Bread goes out now.” “Seed has to stay for later.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Let each explain a responsibility, or allow them to disagree about what the sentence means. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next reader understands that mutual aid can contain distinct rules. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 138 — Bread and seed share a page — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Seed has to stay for later.”
> “Let each explain a responsibility, or allow them to disagree about what the sentence means.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next reader understands that mutual aid can contain distinct rules. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The two keepers hear different obligations in the same word “shared.” Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Let each explain a responsibility, or allow them to disagree about what the sentence means. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 139 — Bread and seed share a page — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Bread goes out now.”
Second voice: “Seed has to stay for later.”
**Under the dialogue:** The two keepers hear different obligations in the same word “shared. The practical distinction is: Let each explain a responsibility, or allow them to disagree about what the sentence means.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next reader understands that mutual aid can contain distinct rules. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 140 — Bread and seed share a page — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘Bread and seed share a page’. It begins from the authored location fact, not from an invented mission result: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Let each explain a responsibility, or allow them to disagree about what the sentence means. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The two keepers hear different obligations in the same word “shared.” Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next reader understands that mutual aid can contain distinct rules. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 141 — Bread and seed share a page — a return with context

The scene stays close to New Ceres Silo Collective. The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The two keepers hear different obligations in the same word “shared.” Proposed lines, with speaker assignment left to line-level review: “Bread goes out now.” “Seed has to stay for later.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Let each explain a responsibility, or allow them to disagree about what the sentence means. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next reader understands that mutual aid can contain distinct rules. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 142 — Bread and seed share a page — a return with context

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Let each explain a responsibility, or allow them to disagree about what the sentence means.”
> “Bread goes out now.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next reader understands that mutual aid can contain distinct rules. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The two keepers hear different obligations in the same word “shared.” Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Let each explain a responsibility, or allow them to disagree about what the sentence means. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 143 — Bread and seed share a page — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Bread goes out now.”
Second voice: “Seed has to stay for later.”
**Under the dialogue:** The two keepers hear different obligations in the same word “shared. The practical distinction is: Let each explain a responsibility, or allow them to disagree about what the sentence means.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next reader understands that mutual aid can contain distinct rules. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 144 — Bread and seed share a page — a return with context

This conditional vignette belongs after a player has encountered the question in ‘Bread and seed share a page’. It begins from the authored location fact, not from an invented mission result: The story places the bread rule and seed rota side by side without claiming they compete in a specific ration decision. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Let each explain a responsibility, or allow them to disagree about what the sentence means. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The two keepers hear different obligations in the same word “shared.” Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next reader understands that mutual aid can contain distinct rules. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 145 — The visitor’s copy — first witness

At New Ceres Silo Collective, a visitor writes a summary for people who have not seen the settlement record.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Proposed lines, with speaker assignment left to line-level review: “Should I make it sound generous?” “Make it sound true.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The callback checks whether the copy still describes the settlement rather than selling an ideal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 146 — The visitor’s copy — first witness

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Should I make it sound generous?”
> “A visitor writes a summary for people who have not seen the settlement record.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A visitor writes a summary for people who have not seen the settlement record. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The callback checks whether the copy still describes the settlement rather than selling an ideal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 147 — The visitor’s copy — first witness

The exchange starts before either person has agreed on what the object means. A visitor writes a summary for people who have not seen the settlement record. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Should I make it sound generous?”
Second voice: “Make it sound true.”
**Under the dialogue:** The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. The practical distinction is: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The callback checks whether the copy still describes the settlement rather than selling an ideal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 148 — The visitor’s copy — first witness

This conditional vignette belongs after a player has encountered the question in ‘The visitor’s copy’. It begins from the authored location fact, not from an invented mission result: A visitor writes a summary for people who have not seen the settlement record. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The callback checks whether the copy still describes the settlement rather than selling an ideal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 149 — The visitor’s copy — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: a visitor writes a summary for people who have not seen the settlement record.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Proposed lines, with speaker assignment left to line-level review: “Should I make it sound generous?” “Make it sound true.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The callback checks whether the copy still describes the settlement rather than selling an ideal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 150 — The visitor’s copy — the wording on the page

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “A visitor writes a summary for people who have not seen the settlement record.”
> “Make it sound true.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A visitor writes a summary for people who have not seen the settlement record. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The callback checks whether the copy still describes the settlement rather than selling an ideal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 151 — The visitor’s copy — the wording on the page

The page is already open; the argument is about what belongs in its margin. A visitor writes a summary for people who have not seen the settlement record. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Should I make it sound generous?”
Second voice: “Make it sound true.”
**Under the dialogue:** The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. The practical distinction is: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The callback checks whether the copy still describes the settlement rather than selling an ideal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 152 — The visitor’s copy — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘The visitor’s copy’. It begins from the authored location fact, not from an invented mission result: A visitor writes a summary for people who have not seen the settlement record. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The callback checks whether the copy still describes the settlement rather than selling an ideal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 153 — The visitor’s copy — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: a visitor writes a summary for people who have not seen the settlement record.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Proposed lines, with speaker assignment left to line-level review: “Should I make it sound generous?” “Make it sound true.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The callback checks whether the copy still describes the settlement rather than selling an ideal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 154 — The visitor’s copy — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Make it sound true.”
> “Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A visitor writes a summary for people who have not seen the settlement record. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The callback checks whether the copy still describes the settlement rather than selling an ideal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 155 — The visitor’s copy — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. A visitor writes a summary for people who have not seen the settlement record. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Should I make it sound generous?”
Second voice: “Make it sound true.”
**Under the dialogue:** The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. The practical distinction is: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The callback checks whether the copy still describes the settlement rather than selling an ideal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 156 — The visitor’s copy — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘The visitor’s copy’. It begins from the authored location fact, not from an invented mission result: A visitor writes a summary for people who have not seen the settlement record. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The callback checks whether the copy still describes the settlement rather than selling an ideal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 157 — The visitor’s copy — a return with context

The scene stays close to New Ceres Silo Collective. A visitor writes a summary for people who have not seen the settlement record.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Proposed lines, with speaker assignment left to line-level review: “Should I make it sound generous?” “Make it sound true.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The callback checks whether the copy still describes the settlement rather than selling an ideal. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 158 — The visitor’s copy — a return with context

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.”
> “Should I make it sound generous?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A visitor writes a summary for people who have not seen the settlement record. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The callback checks whether the copy still describes the settlement rather than selling an ideal. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 159 — The visitor’s copy — a return with context

They return to the wording after some time has passed, without pretending the place has answered. A visitor writes a summary for people who have not seen the settlement record. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “Should I make it sound generous?”
Second voice: “Make it sound true.”
**Under the dialogue:** The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. The practical distinction is: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The callback checks whether the copy still describes the settlement rather than selling an ideal. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 160 — The visitor’s copy — a return with context

This conditional vignette belongs after a player has encountered the question in ‘The visitor’s copy’. It begins from the authored location fact, not from an invented mission result: A visitor writes a summary for people who have not seen the settlement record. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Copy the shared-bread and seed-rota facts together, or leave the interpretation to the reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The committee note-taker wants the copy to sound welcoming; the seed keeper wants its obligations left visible. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The callback checks whether the copy still describes the settlement rather than selling an ideal. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 161 — A friendly field is not a guarantee — first witness

At New Ceres Silo Collective, the settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Proposed lines, with speaker assignment left to line-level review: “The record says friendly.” “Then do not promise what a person will say.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Use the data only as context for an authorized consumer or omit it from diegetic speech. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next encounter must still belong to the actual faction/settlement owner. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 162 — A friendly field is not a guarantee — first witness

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The record says friendly.”
> “The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next encounter must still belong to the actual faction/settlement owner. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Use the data only as context for an authorized consumer or omit it from diegetic speech. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 163 — A friendly field is not a guarantee — first witness

The exchange starts before either person has agreed on what the object means. The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The record says friendly.”
Second voice: “Then do not promise what a person will say.”
**Under the dialogue:** The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. The practical distinction is: Use the data only as context for an authorized consumer or omit it from diegetic speech.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next encounter must still belong to the actual faction/settlement owner. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 164 — A friendly field is not a guarantee — first witness

This conditional vignette belongs after a player has encountered the question in ‘A friendly field is not a guarantee’. It begins from the authored location fact, not from an invented mission result: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Use the data only as context for an authorized consumer or omit it from diegetic speech. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next encounter must still belong to the actual faction/settlement owner. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 165 — A friendly field is not a guarantee — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: the settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Proposed lines, with speaker assignment left to line-level review: “The record says friendly.” “Then do not promise what a person will say.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Use the data only as context for an authorized consumer or omit it from diegetic speech. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next encounter must still belong to the actual faction/settlement owner. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 166 — A friendly field is not a guarantee — the wording on the page

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.”
> “Then do not promise what a person will say.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next encounter must still belong to the actual faction/settlement owner. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Use the data only as context for an authorized consumer or omit it from diegetic speech. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 167 — A friendly field is not a guarantee — the wording on the page

The page is already open; the argument is about what belongs in its margin. The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The record says friendly.”
Second voice: “Then do not promise what a person will say.”
**Under the dialogue:** The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. The practical distinction is: Use the data only as context for an authorized consumer or omit it from diegetic speech.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next encounter must still belong to the actual faction/settlement owner. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 168 — A friendly field is not a guarantee — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘A friendly field is not a guarantee’. It begins from the authored location fact, not from an invented mission result: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Use the data only as context for an authorized consumer or omit it from diegetic speech. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next encounter must still belong to the actual faction/settlement owner. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 169 — A friendly field is not a guarantee — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: the settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Proposed lines, with speaker assignment left to line-level review: “The record says friendly.” “Then do not promise what a person will say.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Use the data only as context for an authorized consumer or omit it from diegetic speech. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next encounter must still belong to the actual faction/settlement owner. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 170 — A friendly field is not a guarantee — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Then do not promise what a person will say.”
> “Use the data only as context for an authorized consumer or omit it from diegetic speech.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next encounter must still belong to the actual faction/settlement owner. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Use the data only as context for an authorized consumer or omit it from diegetic speech. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 171 — A friendly field is not a guarantee — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The record says friendly.”
Second voice: “Then do not promise what a person will say.”
**Under the dialogue:** The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. The practical distinction is: Use the data only as context for an authorized consumer or omit it from diegetic speech.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next encounter must still belong to the actual faction/settlement owner. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 172 — A friendly field is not a guarantee — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘A friendly field is not a guarantee’. It begins from the authored location fact, not from an invented mission result: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Use the data only as context for an authorized consumer or omit it from diegetic speech. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next encounter must still belong to the actual faction/settlement owner. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 173 — A friendly field is not a guarantee — a return with context

The scene stays close to New Ceres Silo Collective. The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Proposed lines, with speaker assignment left to line-level review: “The record says friendly.” “Then do not promise what a person will say.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: Use the data only as context for an authorized consumer or omit it from diegetic speech. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The next encounter must still belong to the actual faction/settlement owner. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 174 — A friendly field is not a guarantee — a return with context

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Use the data only as context for an authorized consumer or omit it from diegetic speech.”
> “The record says friendly.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The next encounter must still belong to the actual faction/settlement owner. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: Use the data only as context for an authorized consumer or omit it from diegetic speech. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 175 — A friendly field is not a guarantee — a return with context

They return to the wording after some time has passed, without pretending the place has answered. The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “The record says friendly.”
Second voice: “Then do not promise what a person will say.”
**Under the dialogue:** The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. The practical distinction is: Use the data only as context for an authorized consumer or omit it from diegetic speech.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The next encounter must still belong to the actual faction/settlement owner. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 176 — A friendly field is not a guarantee — a return with context

This conditional vignette belongs after a player has encountered the question in ‘A friendly field is not a guarantee’. It begins from the authored location fact, not from an invented mission result: The settlement data lists a friendly attitude and a standing gate at zero, but this does not authorize a particular entry or current response. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: Use the data only as context for an authorized consumer or omit it from diegetic speech. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The caravan reader wants to reassure a traveler; the clerk does not want catalog fields treated as a personal welcome. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The next encounter must still belong to the actual faction/settlement owner. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 177 — A loaf and an unfilled column — first witness

At New Ceres Silo Collective, a final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Proposed lines, with speaker assignment left to line-level review: “What do I write at the bottom?” “Write what we did. Leave the harvest blank.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The content leaves New Ceres as a community, not a new economy feature or solved threat. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 178 — A loaf and an unfilled column — first witness

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “What do I write at the bottom?”
> “A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The content leaves New Ceres as a community, not a new economy feature or solved threat. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 179 — A loaf and an unfilled column — first witness

The exchange starts before either person has agreed on what the object means. A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do I write at the bottom?”
Second voice: “Write what we did. Leave the harvest blank.”
**Under the dialogue:** The keepers accept that the story can close without declaring the rule perfect or the crop secure. The practical distinction is: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The content leaves New Ceres as a community, not a new economy feature or solved threat. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 180 — A loaf and an unfilled column — first witness

This conditional vignette belongs after a player has encountered the question in ‘A loaf and an unfilled column’. It begins from the authored location fact, not from an invented mission result: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The content leaves New Ceres as a community, not a new economy feature or solved threat. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 181 — A loaf and an unfilled column — the wording on the page

The first verified thing at New Ceres Silo Collective is simple: a final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Proposed lines, with speaker assignment left to line-level review: “What do I write at the bottom?” “Write what we did. Leave the harvest blank.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The content leaves New Ceres as a community, not a new economy feature or solved threat. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 182 — A loaf and an unfilled column — the wording on the page

Proposed artifact, not an existing catalog record: **proposed handoff record**. Possible author: the committee note-taker; possible reader: the seed-rotation keeper. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.”
> “Write what we did. Leave the harvest blank.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The content leaves New Ceres as a community, not a new economy feature or solved threat. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 183 — A loaf and an unfilled column — the wording on the page

The page is already open; the argument is about what belongs in its margin. A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do I write at the bottom?”
Second voice: “Write what we did. Leave the harvest blank.”
**Under the dialogue:** The keepers accept that the story can close without declaring the rule perfect or the crop secure. The practical distinction is: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The content leaves New Ceres as a community, not a new economy feature or solved threat. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 184 — A loaf and an unfilled column — the wording on the page

This conditional vignette belongs after a player has encountered the question in ‘A loaf and an unfilled column’. It begins from the authored location fact, not from an invented mission result: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The content leaves New Ceres as a community, not a new economy feature or solved threat. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 185 — A loaf and an unfilled column — the disagreement made plain

A visitor at New Ceres Silo Collective begins with the detail, not an explanation: a final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Proposed lines, with speaker assignment left to line-level review: “What do I write at the bottom?” “Write what we did. Leave the harvest blank.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The content leaves New Ceres as a community, not a new economy feature or solved threat. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 186 — A loaf and an unfilled column — the disagreement made plain

Proposed artifact, not an existing catalog record: **proposed seed rota note**. Possible author: the seed-rotation keeper; possible reader: the store clerk. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “Write what we did. Leave the harvest blank.”
> “End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The content leaves New Ceres as a community, not a new economy feature or solved threat. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 187 — A loaf and an unfilled column — the disagreement made plain

Both speakers know the same source fact and draw different practical limits from it. A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do I write at the bottom?”
Second voice: “Write what we did. Leave the harvest blank.”
**Under the dialogue:** The keepers accept that the story can close without declaring the rule perfect or the crop secure. The practical distinction is: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The content leaves New Ceres as a community, not a new economy feature or solved threat. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 188 — A loaf and an unfilled column — the disagreement made plain

This conditional vignette belongs after a player has encountered the question in ‘A loaf and an unfilled column’. It begins from the authored location fact, not from an invented mission result: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The content leaves New Ceres as a community, not a new economy feature or solved threat. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Scene draft 189 — A loaf and an unfilled column — a return with context

The scene stays close to New Ceres Silo Collective. A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome.

Two proposed voices meet over wording. The first wants the observation to remain useful; the second keeps its limits visible. Their friction is concrete: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Proposed lines, with speaker assignment left to line-level review: “What do I write at the bottom?” “Write what we did. Leave the harvest blank.”

Player-facing prose can leave room to read, ask a narrow question, carry the wording onward, or refuse to turn the moment into a decision. Candidate distinction: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The passage does not add a marker, route, reward, inventory result, or persistent flag. If an existing content owner later exposes a compatible choice, this text can follow that owner’s state; otherwise it remains a standalone draft.

A later reader may meet the same language in a different context: The content leaves New Ceres as a community, not a new economy feature or solved threat. That return is an editorial callback, not a claim that the location changes or that a character is present on a particular day. Keep the source fact, a speaker’s interpretation, and any future observation in separate sentences.

Draft limit: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Record and return 190 — A loaf and an unfilled column — a return with context

Proposed artifact, not an existing catalog record: **proposed store margin**. Possible author: the store clerk; possible reader: the committee note-taker. Leave date, circulation, physical condition, and signature blank unless a current source establishes them.

Draft excerpt:

> “End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.”
> “What do I write at the bottom?”

The note exists here as a candidate form for the beat, not as evidence that this paper, tag, tally, or message already sits at New Ceres Silo Collective. Its purpose is ordinary: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. A reader needs enough provenance to know whether the words were witnessed, copied, or only proposed.

Return margin: The content leaves New Ceres as a community, not a new economy feature or solved threat. The later reader should not promote that sentence into a verified event. The conflict the record can carry is limited to what a speaker had reason to say: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Keep disagreement attributed. A copied statement is not a second witness, and a neat line is not a new source of truth.

Editorial use: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. If selected, this record can be read, left behind, or declined without adding an objective. Its wording should not silently create a new archive, discovery key, barter contract, journal category, or settlement rule. Existing data and system owners remain authoritative for those concerns.

Source boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

### Conversation fragment 191 — A loaf and an unfilled column — a return with context

They return to the wording after some time has passed, without pretending the place has answered. A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The speakers are unnamed editorial roles, so their lines do not assign a history to an existing survivor or faction member. Their register should fit the work in front of them: practical, incomplete, and willing to stop when the evidence stops.

First voice: “What do I write at the bottom?”
Second voice: “Write what we did. Leave the harvest blank.”
**Under the dialogue:** The keepers accept that the story can close without declaring the rule perfect or the crop secure. The practical distinction is: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader.
The pause is useful: neither person fills in an unknown date, author, route, or outcome by talking longer.

Possible player response: ask what was actually seen; ask who wrote the earlier line; say nothing; or leave the exchange. None of those responses is treated as consent to an unverified action. If the player does not answer, the scene should still close on the speakers’ own decision to keep the wording bounded.

A later callback can reuse the disagreement without repeating the same dialogue: The content leaves New Ceres as a community, not a new economy feature or solved threat. The content change is in context and attribution. It does not imply a new relationship score, faction standing shift, hazard result, transaction, or location state. The existing owner must provide any such consequence before a branch-specific passage is selected.

Continuity boundary: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.
### Consequence vignette 192 — A loaf and an unfilled column — a return with context

This conditional vignette belongs after a player has encountered the question in ‘A loaf and an unfilled column’. It begins from the authored location fact, not from an invented mission result: A final proposed tally leaves today’s bread, tomorrow’s seed, and any caravan exchange as separate questions unless an owner supplies a real outcome. The roles remain editorial, and no branch below becomes canon until an existing content owner selects it.

**Candidate reading A:** If the player carries the first speaker’s wording forward, the next reader can encounter this limit: End with a witnessed loaf, an unsigned seed line, or a note passed to the next rota reader. The immediate result is a clearer account of what that person meant, not a guaranteed resource, safe passage, treatment, or change in settlement policy. A practical line can travel without settling the disagreement.

**Candidate reading B:** If the player leaves the wording behind or declines to choose between the speakers, the second speaker may keep the question open: The keepers accept that the story can close without declaring the rule perfect or the crop secure. Nothing is counted as lost or gained solely because a candidate passage is not read. Refusal is a complete authored response, and the story can continue through another existing surface if one is verified.

Delayed return: The content leaves New Ceres as a community, not a new economy feature or solved threat. A later record can acknowledge that the earlier language circulated, was ignored, or was never copied—but only as one of these alternative drafts, not as three events that all happened. Choose one form that fits a real trigger, or use none. Do not add a hidden flag just to make the callback possible.

The strongest ending is proportionate: one person can leave with a more exact sentence while the location remains unresolved. Guardrail: Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice.

## 15. Beat selection index

| Beat | Editorial focus | Candidate forms | Source boundary |
|---:|---|---|---|
| 01 | Three silos at the edge of the Verge — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 02 | Three silos at the edge of the Verge — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 03 | Three silos at the edge of the Verge — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 04 | Three silos at the edge of the Verge — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 05 | A loaf under a rule — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 06 | A loaf under a rule — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 07 | A loaf under a rule — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 08 | A loaf under a rule — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 09 | The seed rota — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 10 | The seed rota — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 11 | The seed rota — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 12 | The seed rota — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 13 | Winter grain is not a promise — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 14 | Winter grain is not a promise — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 15 | Winter grain is not a promise — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 16 | Winter grain is not a promise — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 17 | A barter sentence — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 18 | A barter sentence — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 19 | A barter sentence — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 20 | A barter sentence — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 21 | An export line in the catalog — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 22 | An export line in the catalog — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 23 | An export line in the catalog — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 24 | An export line in the catalog — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 25 | A caravan waits outside the sentence — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 26 | A caravan waits outside the sentence — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 27 | A caravan waits outside the sentence — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 28 | A caravan waits outside the sentence — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 29 | The night tension — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 30 | The night tension — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 31 | The night tension — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 32 | The night tension — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 33 | Bread and seed share a page — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 34 | Bread and seed share a page — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 35 | Bread and seed share a page — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 36 | Bread and seed share a page — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 37 | The visitor’s copy — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 38 | The visitor’s copy — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 39 | The visitor’s copy — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 40 | The visitor’s copy — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 41 | A friendly field is not a guarantee — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 42 | A friendly field is not a guarantee — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 43 | A friendly field is not a guarantee — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 44 | A friendly field is not a guarantee — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 45 | A loaf and an unfilled column — first witness | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 46 | A loaf and an unfilled column — the wording on the page | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 47 | A loaf and an unfilled column — the disagreement made plain | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |
| 48 | A loaf and an unfilled column — a return with context | Scene / record / conversation / consequence | Proposed wording only; verify consumer and state owner. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions, outcomes, or state. A JSON row or catalog description does not prove a current consumer. Verify the owning system before selecting a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `loc_settlement_silo_burrow` | three concrete grain silos in the Verge; winter grain cultivation and seed barter with passing caravans | Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice. |
| `settlement_silo_burrow` | shared bread rule; seed rota; trenches; refugee commune; Grain Exchange allegiance; trade fields; frequent night raids listed as internal tension | Do not create a new faction, governance mechanic, crop-yield rule, raid encounter, or caravan route. Do not imply that a specific raid happened during this story or identify attackers beyond the source. Treat population and trade values as authored catalog fields, not a live count or guaranteed inventory. Keep any new resident or visitor as an editorial voice. |

## 17. Collision and unresolved authority

The settlement’s shared-bread rule and seed rota are already authored facts; do not present them as new mechanics. Grain Exchange is an existing allegiance, not a faction to duplicate. The listed night raids are an internal tension, not permission to stage an unrecorded raid as canon.
If an implementation needs an answer not present in current evidence, pause and return that question to the content owner. This document does not resolve it. The anchor is `loc_settlement_silo_burrow`; no prior expansion plan in the scanned plan roots uses this anchor ID or display name.

## 18. Review checklist

Before selecting a passage, compare it with every cited source row; preserve source wording and units; keep proposed characters and artifacts visibly editorial; verify current reachability, schema, consumer, and trigger independently; and read dialogue aloud for role-appropriate vocabulary. Remove any sentence that could be mistaken for a route, recipe, diagnosis, safety instruction, or new feature.

## 19. Acceptance boundary

This plan is complete as a content proposal when each selected passage traces to its cited source, existing character and system outcomes remain unchanged, and an existing owner is identified for placement. If reachability or the consumer is absent, keep the prose as a draft. Counts below use Python Unicode code-point lengths of the saved Markdown files.
