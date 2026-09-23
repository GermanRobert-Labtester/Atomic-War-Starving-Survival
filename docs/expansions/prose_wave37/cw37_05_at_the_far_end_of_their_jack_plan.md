# EXPANSION CW37-05 — At the Far End of Their Jack

## A prose-first location story about people, memory, and what the record cannot settle.

### Prose Wave 37: Rooms with Unfinished Returns

## Batch brief

**Content type:** original game-content proposal with scene, diegetic-record, conversation, and conditional-return drafts.
**Content bank:** 48 distinct beat pairings, each with four alternative authored forms (192 candidate passages).
**Current local anchor:** loc_warehouse_district — Warehouse District.
**Tone:** grounded, human, restrained, and specific about what remains unknown.
**Scope:** game content only; no production code, JSON data, route, quest, flag, simulation, or save changes.

## 1. Expansion thesis

A three-person salvage crew is already described as pinned beneath a collapsed mezzanine, with its own jacks and slings intact and its channel open. This prose bank follows the aftermath of the existing choice: not to alter whether help was sent, but to ask what those who heard the call are able to say after the channel is either answered or lost.

## 2. Story question

After a rescue decision, who owns the version of the story that survives?

## 3. Verified local anchor and source records

The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| Assets/StreamingAssets/Data/locations.json | loc_warehouse_district | Freight-terminal storage buildings; broken pallets, catwalks, and holding pens used by scavenger gangs. |
| Assets/StreamingAssets/Data/moral_choice_quests_distress.json | quest_moral_distress_convoy_sos | Three-person salvage crew pinned under a collapsed mezzanine; Crew Two cut the wrong column; their own jacks and slings remain intact; channel open. |
| Assets/StreamingAssets/Data/moral_choice_quests_distress.json | quest_moral_distress_convoy_sos choices | Existing dispatch and disregard outcomes; existing flags flag_responded_distress and flag_ignored_distress; existing epitaph text. |

## 4. Fixed canon and open space

The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state.
Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.
All new speakers, props, memories, labels, and return passages are editorial drafts. Existing named characters retain their authored identity and outcomes. A prose plan does not establish reachability, item placement, a consumer, or a trigger.

## 5. Human center

The center is not the moment of collapse but the difficult retelling afterward. The proposed voices include a radio listener, a witness to the choice, and a member of the settlement who heard only the later account. They disagree over credit, blame, and whether repeating an epitaph helps anyone.

## 6. Voice and point of view

- **Channel listener:** The listener remembers the sound and the moment the channel stayed open or fell quiet, without claiming to know what was outside the radio’s reach.
- **After-account keeper:** This proposed voice records what was authorized by the existing outcome and refuses to improve it for a cleaner story.
- **Later hearer:** The hearer was not present for the decision and wants a truthful account without inheriting a verdict.

Each speaker knows only what the cited source, their own proposed observation, or an explicitly attributed memory could give them. No proposed voice represents a whole faction or settlement.

## 7. Placement and current reachability

The source record verifies a content anchor, not a guaranteed playable route or passage consumer. Before selection, confirm current reachability, content schema, owner, and trigger. This is the only plan in the batch that explicitly names existing branch flags. Candidate return prose may be considered only through the current moral-choice and narrative consumer after checking both branches. It adds no flags, quests, rescue-system behavior, trust delta, memorial entry, or crew state. Every bank entry remains a candidate, not a promise that all 48 beats occur.

## 8. Player agency

The player may read, ask, carry a sentence forward, leave it where it is, or decline to interpret it. These are editorial postures rather than a promised menu. A refusal or silence is a complete outcome. Any branch that needs runtime state must use the existing owner’s exposed state after verification.

## 9. Continuity, dignity, and safety

Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.
Do not make hazard, scarcity, or grief into a puzzle tutorial. Do not make the player’s caution a moral failure. Keep proposed identity separate from authored identity and testimony separate from fact.

## 10. Existing hooks and implementation boundary

This is the only plan in the batch that explicitly names existing branch flags. Candidate return prose may be considered only through the current moral-choice and narrative consumer after checking both branches. It adds no flags, quests, rescue-system behavior, trust delta, memorial entry, or crew state.
The plan creates no parallel authority, new registry, save section, route graph, generic panel callback, or gameplay rule. A selected passage should attach only through a verified existing content owner.

## 11. Narrative sequence

The six movements below organize an editorial bank; they are not six required visits or a fixed quest chain. Beat order may change if the existing consumer calls for a shorter or non-linear presentation.

### Movement 1: The Channel Before the Choice

The listening room is quiet enough for the caller’s words to arrive without embellishment. A listener can hear a request without knowing the full scene.

001. **The first syllable of the call — The Channel Before the Choice** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
002. **The wrong column — The Channel Before the Choice** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
003. **The intact slings — The Channel Before the Choice** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
004. **The choice heard by others — The Channel Before the Choice** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
005. **A channel left open — The Channel Before the Choice** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
006. **The column that held — The Channel Before the Choice** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
007. **Two epitaphs, two choices — The Channel Before the Choice** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
008. **The far end of a jack — The Channel Before the Choice** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.
### Movement 2: The Wrong Column

The story shows a mistake without reducing the crew to a cautionary example. The error matters; it does not explain every person in the room.

009. **The first syllable of the call — The Wrong Column** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
010. **The wrong column — The Wrong Column** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
011. **The intact slings — The Wrong Column** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
012. **The choice heard by others — The Wrong Column** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
013. **A channel left open — The Wrong Column** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
014. **The column that held — The Wrong Column** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
015. **Two epitaphs, two choices — The Wrong Column** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
016. **The far end of a jack — The Wrong Column** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.
### Movement 3: The Tools They Kept

A proposed reader mistakes equipment for certainty, and another returns attention to the people asking for help. Having tools is not the same thing as being able to leave.

017. **The first syllable of the call — The Tools They Kept** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
018. **The wrong column — The Tools They Kept** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
019. **The intact slings — The Tools They Kept** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
020. **The choice heard by others — The Tools They Kept** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
021. **A channel left open — The Tools They Kept** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
022. **The column that held — The Tools They Kept** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
023. **Two epitaphs, two choices — The Tools They Kept** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
024. **The far end of a jack — The Tools They Kept** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.
### Movement 4: After the Choice

The next account differs according to whether help was dispatched or the transmission disregarded. Keep each authored branch outcome and epitaph together; never blend them.

025. **The first syllable of the call — After the Choice** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
026. **The wrong column — After the Choice** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
027. **The intact slings — After the Choice** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
028. **The choice heard by others — After the Choice** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
029. **A channel left open — After the Choice** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
030. **The column that held — After the Choice** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
031. **Two epitaphs, two choices — After the Choice** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
032. **The far end of a jack — After the Choice** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.
### Movement 5: The Words That Remain

One listener remembers the dispatch text; another remembers the channel’s silence. The account cannot add a body, survivor, injury, or final line.

033. **The first syllable of the call — The Words That Remain** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
034. **The wrong column — The Words That Remain** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
035. **The intact slings — The Words That Remain** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
036. **The choice heard by others — The Words That Remain** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
037. **A channel left open — The Words That Remain** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
038. **The column that held — The Words That Remain** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
039. **Two epitaphs, two choices — The Words That Remain** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
040. **The far end of a jack — The Words That Remain** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.
### Movement 6: The Epitaphs Kept Apart

A later reader asks how to quote an epitaph without erasing which branch it belongs to. Credit, grief, and blame remain distinct.

041. **The first syllable of the call — The Epitaphs Kept Apart** — Someone asks the listener to make the voice sound braver. The listener declines. The candidate return is The later account keeps a voice human without dramatizing it.
042. **The wrong column — The Epitaphs Kept Apart** — A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The candidate return is The mistake remains serious without becoming a verdict on an invented person.
043. **The intact slings — The Epitaphs Kept Apart** — A later hearer assumes that intact tools made the situation manageable. The candidate return is The tools remain evidence of effort, not an escape guide.
044. **The choice heard by others — The Epitaphs Kept Apart** — A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The candidate return is The retelling distinguishes consequence from a lecture.
045. **A channel left open — The Epitaphs Kept Apart** — A listener remembers waiting for a voice that never came back. The candidate return is The reader meets the limit the original text set.
046. **The column that held — The Epitaphs Kept Apart** — A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The candidate return is The success remains meaningful without spectacle.
047. **Two epitaphs, two choices — The Epitaphs Kept Apart** — A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The candidate return is A future reader can understand why the language differs.
048. **The far end of a jack — The Epitaphs Kept Apart** — A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The candidate return is The last reading honors the authored words by not enlarging them.

## 12. Creative variants

### Grounded
Keep the prose close to the cited source and the ordinary human exchange. Let the passage turn on this question: “After a rescue decision, who owns the version of the story that survives?”

### Interlinked
Only connect a callback where the actual content owner exposes a compatible source state. This is the only plan in the batch that explicitly names existing branch flags. Candidate return prose may be considered only through the current moral-choice and narrative consumer after checking both branches. It adds no flags, quests, rescue-system behavior, trust delta, memorial entry, or crew state. A thematic association does not prove a technical route or a shared event.

### Wild card
Let two proposed readers remember the same phrase differently while preserving the known facts. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

## 13. Alternative forms and editorial rubric

The four forms under every beat are alternatives, not a four-step quest. Scene drafts stage a physical observation; records give a proposed sentence an author and audience; conversations let practical disagreement be heard; consequence vignettes show how a wording might be carried, kept, or refused later. Select only forms that fit a verified consumer.

A selected passage should make clear who speaks, what they can know, why they use these words, and what remains outside the record. Cut any line that sounds like a feature pitch, tutorial, universal verdict, or unsupported catalog fact. Keep the player’s option to leave intact.

## 14. Content bank

### Scene drafts

#### Scene draft 001 — The first syllable of the call — The Channel Before the Choice

The proposed encounter begins near the quest’s authored distress transmission, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 002 — The wrong column — The Channel Before the Choice

Nothing announces the importance of this moment at the quest’s authored distress transmission. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: A listener can hear a request without knowing the full scene.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 003 — The intact slings — The Channel Before the Choice

The after-account keeper is quiet at the quest’s authored distress transmission. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. A listener can hear a request without knowing the full scene.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 004 — The choice heard by others — The Channel Before the Choice

During the channel before the choice, the scene stays with the quest’s authored distress transmission. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. A listener can hear a request without knowing the full scene. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 005 — A channel left open — The Channel Before the Choice

The proposed encounter begins near the quest’s authored distress transmission, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 006 — The column that held — The Channel Before the Choice

Nothing announces the importance of this moment at the quest’s authored distress transmission. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: A listener can hear a request without knowing the full scene.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 007 — Two epitaphs, two choices — The Channel Before the Choice

The after-account keeper is quiet at the quest’s authored distress transmission. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. A listener can hear a request without knowing the full scene.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 008 — The far end of a jack — The Channel Before the Choice

During the channel before the choice, the scene stays with the quest’s authored distress transmission. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. A listener can hear a request without knowing the full scene. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 009 — The first syllable of the call — The Wrong Column

The proposed encounter begins near the quest’s exact account of Crew Two’s cut and the resulting collapse, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 010 — The wrong column — The Wrong Column

Nothing announces the importance of this moment at the quest’s exact account of Crew Two’s cut and the resulting collapse. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: The error matters; it does not explain every person in the room.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 011 — The intact slings — The Wrong Column

The after-account keeper is quiet at the quest’s exact account of Crew Two’s cut and the resulting collapse. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. The error matters; it does not explain every person in the room.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 012 — The choice heard by others — The Wrong Column

During the wrong column, the scene stays with the quest’s exact account of Crew Two’s cut and the resulting collapse. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. The error matters; it does not explain every person in the room. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 013 — A channel left open — The Wrong Column

The proposed encounter begins near the quest’s exact account of Crew Two’s cut and the resulting collapse, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 014 — The column that held — The Wrong Column

Nothing announces the importance of this moment at the quest’s exact account of Crew Two’s cut and the resulting collapse. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: The error matters; it does not explain every person in the room.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 015 — Two epitaphs, two choices — The Wrong Column

The after-account keeper is quiet at the quest’s exact account of Crew Two’s cut and the resulting collapse. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. The error matters; it does not explain every person in the room.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 016 — The far end of a jack — The Wrong Column

During the wrong column, the scene stays with the quest’s exact account of Crew Two’s cut and the resulting collapse. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. The error matters; it does not explain every person in the room. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 017 — The first syllable of the call — The Tools They Kept

The proposed encounter begins near the authored jacks and slings, described as intact, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 018 — The wrong column — The Tools They Kept

Nothing announces the importance of this moment at the authored jacks and slings, described as intact. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: Having tools is not the same thing as being able to leave.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 019 — The intact slings — The Tools They Kept

The after-account keeper is quiet at the authored jacks and slings, described as intact. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Having tools is not the same thing as being able to leave.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 020 — The choice heard by others — The Tools They Kept

During the tools they kept, the scene stays with the authored jacks and slings, described as intact. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. Having tools is not the same thing as being able to leave. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 021 — A channel left open — The Tools They Kept

The proposed encounter begins near the authored jacks and slings, described as intact, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 022 — The column that held — The Tools They Kept

Nothing announces the importance of this moment at the authored jacks and slings, described as intact. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: Having tools is not the same thing as being able to leave.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 023 — Two epitaphs, two choices — The Tools They Kept

The after-account keeper is quiet at the authored jacks and slings, described as intact. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Having tools is not the same thing as being able to leave.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 024 — The far end of a jack — The Tools They Kept

During the tools they kept, the scene stays with the authored jacks and slings, described as intact. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. Having tools is not the same thing as being able to leave. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 025 — The first syllable of the call — After the Choice

The proposed encounter begins near a proposed reading after the existing distress choice, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 026 — The wrong column — After the Choice

Nothing announces the importance of this moment at a proposed reading after the existing distress choice. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: Keep each authored branch outcome and epitaph together; never blend them.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 027 — The intact slings — After the Choice

The after-account keeper is quiet at a proposed reading after the existing distress choice. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Keep each authored branch outcome and epitaph together; never blend them.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 028 — The choice heard by others — After the Choice

During after the choice, the scene stays with a proposed reading after the existing distress choice. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. Keep each authored branch outcome and epitaph together; never blend them. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 029 — A channel left open — After the Choice

The proposed encounter begins near a proposed reading after the existing distress choice, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 030 — The column that held — After the Choice

Nothing announces the importance of this moment at a proposed reading after the existing distress choice. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: Keep each authored branch outcome and epitaph together; never blend them.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 031 — Two epitaphs, two choices — After the Choice

The after-account keeper is quiet at a proposed reading after the existing distress choice. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Keep each authored branch outcome and epitaph together; never blend them.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 032 — The far end of a jack — After the Choice

During after the choice, the scene stays with a proposed reading after the existing distress choice. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. Keep each authored branch outcome and epitaph together; never blend them. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 033 — The first syllable of the call — The Words That Remain

The proposed encounter begins near a proposed later conversation about the authored outcome, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 034 — The wrong column — The Words That Remain

Nothing announces the importance of this moment at a proposed later conversation about the authored outcome. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: The account cannot add a body, survivor, injury, or final line.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 035 — The intact slings — The Words That Remain

The after-account keeper is quiet at a proposed later conversation about the authored outcome. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. The account cannot add a body, survivor, injury, or final line.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 036 — The choice heard by others — The Words That Remain

During the words that remain, the scene stays with a proposed later conversation about the authored outcome. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. The account cannot add a body, survivor, injury, or final line. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 037 — A channel left open — The Words That Remain

The proposed encounter begins near a proposed later conversation about the authored outcome, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 038 — The column that held — The Words That Remain

Nothing announces the importance of this moment at a proposed later conversation about the authored outcome. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: The account cannot add a body, survivor, injury, or final line.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 039 — Two epitaphs, two choices — The Words That Remain

The after-account keeper is quiet at a proposed later conversation about the authored outcome. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. The account cannot add a body, survivor, injury, or final line.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 040 — The far end of a jack — The Words That Remain

During the words that remain, the scene stays with a proposed later conversation about the authored outcome. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. The account cannot add a body, survivor, injury, or final line. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 041 — The first syllable of the call — The Epitaphs Kept Apart

The proposed encounter begins near the separate authored epitaphs for the existing choices, after the day’s practical work has paused. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard.

Someone asks the listener to make the voice sound braver. The listener declines. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: Did they sound frightened?

The first line sounds sharper than its speaker intends: “Did they sound frightened?” The reply comes without heat: “They sounded like people keeping a channel open.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. They sounded like people keeping a channel open.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The later account keeps a voice human without dramatizing it. The people move on to the next ordinary need.

#### Scene draft 042 — The wrong column — The Epitaphs Kept Apart

Nothing announces the importance of this moment at the separate authored epitaphs for the existing choices. The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter.

The later hearer watches the exchange rather than interrupting it. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “Whose hand made the cut?” The after-account keeper answers, “The record says Crew Two. It does not give us one hand to blame.” Their difference is practical: Credit, grief, and blame remain distinct.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The mistake remains serious without becoming a verdict on an invented person. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 043 — The intact slings — The Epitaphs Kept Apart

The after-account keeper is quiet at the separate authored epitaphs for the existing choices. The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use.

A later hearer assumes that intact tools made the situation manageable. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Credit, grief, and blame remain distinct.

“Didn’t they have what they needed?” says the channel listener. The after-account keeper answers after a breath: “They had equipment. The call says they still needed another end of it.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at the intact slings. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest says the crew kept its tools dry and the slings and jacks were intact; no passage describes their use. The voices move on, carrying different parts of the exchange. The tools remain evidence of effort, not an escape guide.

#### Scene draft 044 — The choice heard by others — The Epitaphs Kept Apart

During the epitaphs kept apart, the scene stays with the separate authored epitaphs for the existing choices. The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route.

The detail draws the channel listener into a question and the after-account keeper into a memory. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Can we tell only the part that lets us sleep?” The channel listener replies, “We can tell the part we chose. The rest is already written.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest.

The retelling distinguishes consequence from a lecture. Credit, grief, and blame remain distinct. The detail has not become a route, reward, or proof of a history the source does not give.

#### Scene draft 045 — A channel left open — The Epitaphs Kept Apart

The proposed encounter begins near the separate authored epitaphs for the existing choices, after the day’s practical work has paused. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase.

A listener remembers waiting for a voice that never came back. For a moment, the people present disagree about the object’s importance. Then they recognize the more immediate question: What should I write after the last signal?

The first line sounds sharper than its speaker intends: “What should I write after the last signal?” The reply comes without heat: “Only that the channel closed.” Both people know what part they cannot supply.

The player may ask who first used the phrase, keep an account attached to its speaker, or leave the exchange where it stands. The player may repeat the first wording, preserve the correction beside it, or decline to decide between them.

The channel listener repeats the key phrase under their breath, not to make it official but to hear whether it still sounds like the speaker meant it. Only that the channel closed.

Afterward, the later hearer remembers the exchange without claiming it settled anything. The reader meets the limit the original text set. The people move on to the next ordinary need.

#### Scene draft 046 — The column that held — The Epitaphs Kept Apart

Nothing announces the importance of this moment at the separate authored epitaphs for the existing choices. The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them.

The later hearer watches the exchange rather than interrupting it. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. What each speaker can claim stays narrower than what either of them feels.

The channel listener asks, “How long did it hold?” The after-account keeper answers, “Long enough for the outcome the record gives us.” Their difference is practical: Credit, grief, and blame remain distinct.

There is room for the player to listen without taking ownership of the disagreement. They can carry one attributed sentence forward, ask what the speaker saw, or leave. No verdict is required.

The scene gives the silence a human shape: someone smooths a folded corner, waits for an answer, or looks toward the person who has not spoken. No one fills that pause for them.

One voice folds the sentence into the work of the day. The success remains meaningful without spectacle. The choice changes what can be repeated, not the authored condition of Warehouse District.

#### Scene draft 047 — Two epitaphs, two choices — The Epitaphs Kept Apart

The after-account keeper is quiet at the separate authored epitaphs for the existing choices. The quest supplies a different epitaph for each of its two authored outcomes.

A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The channel listener notices what the action leaves out, while the after-account keeper remembers why it mattered. Credit, grief, and blame remain distinct.

“Which line belongs to the story?” says the channel listener. The after-account keeper answers after a breath: “The one tied to the choice that happened in that version.” Neither tries to win by making the other sound foolish.

The player’s presence is felt in the pause, not in a demand for a correct answer. A question, refusal to interpret, or quiet departure can each be a complete response.

The after-account keeper looks once more at two epitaphs, two choices. One person keeps looking at the detail; the other waits instead of supplying an explanation on their behalf.

The final image returns to the detail: The quest supplies a different epitaph for each of its two authored outcomes. The voices move on, carrying different parts of the exchange. A future reader can understand why the language differs.

#### Scene draft 048 — The far end of a jack — The Epitaphs Kept Apart

During the epitaphs kept apart, the scene stays with the separate authored epitaphs for the existing choices. The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system.

The detail draws the channel listener into a question and the after-account keeper into a memory. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. Neither offers a full account of Warehouse District.

The after-account keeper lays the question between them: “Who stood at the other end?” The channel listener replies, “The outcome says what the player did. It does not name a new crew member.” The answer does not close the matter.

No one asks the player to make an absent person speak. The player can carry the first account, carry the correction, or let both remain. The speakers still have their own words afterward.

For a breath, the later hearer almost adds a detail, then hears how little it would prove. The people stay with what they have: A reader repeats the phrase and another asks who is allowed to claim credit for standing there.

The last reading honors the authored words by not enlarging them. Credit, grief, and blame remain distinct. The detail has not become a route, reward, or proof of a history the source does not give.

### Record and return

#### Record and return 001 — The first syllable of the call — The Channel Before the Choice

A possible copy is made after the exchange at the quest’s authored distress transmission. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 002 — The wrong column — The Channel Before the Choice

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 003 — The intact slings — The Channel Before the Choice

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A listener can hear a request without knowing the full scene. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 004 — The choice heard by others — The Channel Before the Choice

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. A listener can hear a request without knowing the full scene.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 005 — A channel left open — The Channel Before the Choice

A possible copy is made after the exchange at the quest’s authored distress transmission. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 006 — The column that held — The Channel Before the Choice

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 007 — Two epitaphs, two choices — The Channel Before the Choice

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. A listener can hear a request without knowing the full scene. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 008 — The far end of a jack — The Channel Before the Choice

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. A listener can hear a request without knowing the full scene.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 009 — The first syllable of the call — The Wrong Column

A possible copy is made after the exchange at the quest’s exact account of Crew Two’s cut and the resulting collapse. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 010 — The wrong column — The Wrong Column

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 011 — The intact slings — The Wrong Column

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. The error matters; it does not explain every person in the room. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 012 — The choice heard by others — The Wrong Column

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. The error matters; it does not explain every person in the room.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 013 — A channel left open — The Wrong Column

A possible copy is made after the exchange at the quest’s exact account of Crew Two’s cut and the resulting collapse. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 014 — The column that held — The Wrong Column

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 015 — Two epitaphs, two choices — The Wrong Column

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. The error matters; it does not explain every person in the room. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 016 — The far end of a jack — The Wrong Column

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. The error matters; it does not explain every person in the room.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 017 — The first syllable of the call — The Tools They Kept

A possible copy is made after the exchange at the authored jacks and slings, described as intact. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 018 — The wrong column — The Tools They Kept

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 019 — The intact slings — The Tools They Kept

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Having tools is not the same thing as being able to leave. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 020 — The choice heard by others — The Tools They Kept

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. Having tools is not the same thing as being able to leave.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 021 — A channel left open — The Tools They Kept

A possible copy is made after the exchange at the authored jacks and slings, described as intact. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 022 — The column that held — The Tools They Kept

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 023 — Two epitaphs, two choices — The Tools They Kept

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Having tools is not the same thing as being able to leave. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 024 — The far end of a jack — The Tools They Kept

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. Having tools is not the same thing as being able to leave.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 025 — The first syllable of the call — After the Choice

A possible copy is made after the exchange at a proposed reading after the existing distress choice. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 026 — The wrong column — After the Choice

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 027 — The intact slings — After the Choice

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Keep each authored branch outcome and epitaph together; never blend them. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 028 — The choice heard by others — After the Choice

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. Keep each authored branch outcome and epitaph together; never blend them.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 029 — A channel left open — After the Choice

A possible copy is made after the exchange at a proposed reading after the existing distress choice. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 030 — The column that held — After the Choice

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 031 — Two epitaphs, two choices — After the Choice

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Keep each authored branch outcome and epitaph together; never blend them. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 032 — The far end of a jack — After the Choice

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. Keep each authored branch outcome and epitaph together; never blend them.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 033 — The first syllable of the call — The Words That Remain

A possible copy is made after the exchange at a proposed later conversation about the authored outcome. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 034 — The wrong column — The Words That Remain

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 035 — The intact slings — The Words That Remain

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. The account cannot add a body, survivor, injury, or final line. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 036 — The choice heard by others — The Words That Remain

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. The account cannot add a body, survivor, injury, or final line.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 037 — A channel left open — The Words That Remain

A possible copy is made after the exchange at a proposed later conversation about the authored outcome. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 038 — The column that held — The Words That Remain

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 039 — Two epitaphs, two choices — The Words That Remain

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. The account cannot add a body, survivor, injury, or final line. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 040 — The far end of a jack — The Words That Remain

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. The account cannot add a body, survivor, injury, or final line.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 041 — The first syllable of the call — The Epitaphs Kept Apart

A possible copy is made after the exchange at the separate authored epitaphs for the existing choices. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “Did they sound frightened?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. Someone asks the listener to make the voice sound braver. The listener declines. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The later account keeps a voice human without dramatizing it. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 042 — The wrong column — The Epitaphs Kept Apart

The top line names the subject as the wrong column. The next line gives the reason for writing: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. The author leaves room for a later reader to disagree.

> “The record says Crew Two. It does not give us one hand to blame.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The mistake remains serious without becoming a verdict on an invented person. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The mistake remains serious without becoming a verdict on an invented person.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 043 — The intact slings — The Epitaphs Kept Apart

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Didn’t they have what they needed?

> “Didn’t they have what they needed?”
>
> “They had equipment. The call says they still needed another end of it.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Credit, grief, and blame remain distinct. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. The tools remain evidence of effort, not an escape guide. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 044 — The choice heard by others — The Epitaphs Kept Apart

Proposed reader’s note, from the channel listener to the later hearer: The two existing options are dispatch rescue or disregard the transmission; this beat does not add a third moral route. The writer keeps the account narrow enough that another person can check it.

> “We can tell the part we chose. The rest is already written.”
>
> The first copy made this sound settled. It was not. Credit, grief, and blame remain distinct.

Beside the excerpt, the author distinguishes a witness from a copyist. A witness wants the settlement to remember only the better choice. Another says that would make the existing choice less honest. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

#### Record and return 045 — A channel left open — The Epitaphs Kept Apart

A possible copy is made after the exchange at the separate authored epitaphs for the existing choices. Its proposed author is the after-account keeper; its reader knows Warehouse District only by what others have said.

> “What should I write after the last signal?”
>
> I heard a different answer later. I have kept both because each person was present for a different part of the truth.

The page holds both voices without pretending they reached agreement. A listener remembers waiting for a voice that never came back. A later reader can question the record, compare it with its source, or set it aside.

A later reader may carry the note into another conversation. The reader meets the limit the original text set. The callback changes the audience, not the source fact.

One last sentence keeps the note from becoming an order: “Use this account only for what it says.” The writer has left a way to disagree beside the words.

The artifact’s physical form, author, and audience remain editorial until a current content owner selects them. Nothing here asserts that a paper copy already exists.

#### Record and return 046 — The column that held — The Epitaphs Kept Apart

The top line names the subject as the column that held. The next line gives the reason for writing: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. The author leaves room for a later reader to disagree.

> “Long enough for the outcome the record gives us.”
>
> A reader may carry this sentence or leave it here. Neither choice changes what this page knows about Warehouse District.

The note preserves a limit: A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. It does not turn a recollection into a map, inventory, diagnosis, or universal statement about the people who used this place.

If this passage is selected, the next reader sees the words and their limits together. The success remains meaningful without spectacle. No date, owner, or outcome is supplied beyond the cited record.

Postscript, in the same proposed hand: “I do not know whether this will help. I know I would have wanted someone to keep the difference clear for me.”

The success remains meaningful without spectacle.

Source check: The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. The note may deepen an interpretation of those records; it cannot replace or silently amend them.

#### Record and return 047 — Two epitaphs, two choices — The Epitaphs Kept Apart

The channel listener writes after hearing both accounts. The note is meant for one reader, not a public authority. Its first sentence is: Which line belongs to the story?

> “Which line belongs to the story?”
>
> “The one tied to the choice that happened in that version.”
>
> I am leaving the question beside the answer. One belongs to the person who asked; the other belongs to the person who lived it.

The writer’s reason is modest. Credit, grief, and blame remain distinct. A clean sentence can travel farther than its author, so the margin keeps the speaker attached to the words.

The note ends without an instruction. A future reader can understand why the language differs. Its reader remains free to ask a question, keep the attribution, or stop reading.

At the foot of the page, the writer adds a question for the next reader: “What part of this belongs to the person who was there?” The answer is not supplied by the copyist.

Keep this draft only if its author, audience, and attribution are legible in the final content. Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

#### Record and return 048 — The far end of a jack — The Epitaphs Kept Apart

Proposed reader’s note, from the channel listener to the later hearer: The title phrase comes from the authored rescue epitaph; this beat uses it as a remembered sentence rather than a new memorial system. The writer keeps the account narrow enough that another person can check it.

> “The outcome says what the player did. It does not name a new crew member.”
>
> The first copy made this sound settled. It was not. Credit, grief, and blame remain distinct.

Beside the excerpt, the author distinguishes a witness from a copyist. A reader repeats the phrase and another asks who is allowed to claim credit for standing there. The point is not to settle a dispute; it is to show which part belongs to whom.

The copy gives one person room to preserve the first account while another remains free to leave the disagreement unresolved.

The recipient’s margin stays empty in this version. That blank can mean the person has not read the note yet; it cannot be treated as agreement, refusal, or a saved campaign state.

Before placement, verify the existing consumer and preserve the cited source wording. This proposed note adds no route, flag, reward, or system.

### Conversation fragments

#### Conversation fragment 001 — The first syllable of the call — The Channel Before the Choice

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 002 — The wrong column — The Channel Before the Choice

At the quest’s authored distress transmission, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: A listener can hear a request without knowing the full scene.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A listener can hear a request without knowing the full scene. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 003 — The intact slings — The Channel Before the Choice

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 004 — The choice heard by others — The Channel Before the Choice

The two proposed speakers meet over the choice heard by others at the quest’s authored distress transmission. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 005 — A channel left open — The Channel Before the Choice

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 006 — The column that held — The Channel Before the Choice

At the quest’s authored distress transmission, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: A listener can hear a request without knowing the full scene.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. A listener can hear a request without knowing the full scene. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 007 — Two epitaphs, two choices — The Channel Before the Choice

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 008 — The far end of a jack — The Channel Before the Choice

The two proposed speakers meet over the far end of a jack at the quest’s authored distress transmission. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

#### Conversation fragment 009 — The first syllable of the call — The Wrong Column

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 010 — The wrong column — The Wrong Column

At the quest’s exact account of Crew Two’s cut and the resulting collapse, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: The error matters; it does not explain every person in the room.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. The error matters; it does not explain every person in the room. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 011 — The intact slings — The Wrong Column

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 012 — The choice heard by others — The Wrong Column

The two proposed speakers meet over the choice heard by others at the quest’s exact account of Crew Two’s cut and the resulting collapse. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 013 — A channel left open — The Wrong Column

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 014 — The column that held — The Wrong Column

At the quest’s exact account of Crew Two’s cut and the resulting collapse, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: The error matters; it does not explain every person in the room.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. The error matters; it does not explain every person in the room. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 015 — Two epitaphs, two choices — The Wrong Column

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 016 — The far end of a jack — The Wrong Column

The two proposed speakers meet over the far end of a jack at the quest’s exact account of Crew Two’s cut and the resulting collapse. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

#### Conversation fragment 017 — The first syllable of the call — The Tools They Kept

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 018 — The wrong column — The Tools They Kept

At the authored jacks and slings, described as intact, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: Having tools is not the same thing as being able to leave.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Having tools is not the same thing as being able to leave. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 019 — The intact slings — The Tools They Kept

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 020 — The choice heard by others — The Tools They Kept

The two proposed speakers meet over the choice heard by others at the authored jacks and slings, described as intact. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 021 — A channel left open — The Tools They Kept

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 022 — The column that held — The Tools They Kept

At the authored jacks and slings, described as intact, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: Having tools is not the same thing as being able to leave.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Having tools is not the same thing as being able to leave. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 023 — Two epitaphs, two choices — The Tools They Kept

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 024 — The far end of a jack — The Tools They Kept

The two proposed speakers meet over the far end of a jack at the authored jacks and slings, described as intact. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

#### Conversation fragment 025 — The first syllable of the call — After the Choice

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 026 — The wrong column — After the Choice

At a proposed reading after the existing distress choice, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: Keep each authored branch outcome and epitaph together; never blend them.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Keep each authored branch outcome and epitaph together; never blend them. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 027 — The intact slings — After the Choice

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 028 — The choice heard by others — After the Choice

The two proposed speakers meet over the choice heard by others at a proposed reading after the existing distress choice. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 029 — A channel left open — After the Choice

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 030 — The column that held — After the Choice

At a proposed reading after the existing distress choice, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: Keep each authored branch outcome and epitaph together; never blend them.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Keep each authored branch outcome and epitaph together; never blend them. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 031 — Two epitaphs, two choices — After the Choice

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 032 — The far end of a jack — After the Choice

The two proposed speakers meet over the far end of a jack at a proposed reading after the existing distress choice. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

#### Conversation fragment 033 — The first syllable of the call — The Words That Remain

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 034 — The wrong column — The Words That Remain

At a proposed later conversation about the authored outcome, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: The account cannot add a body, survivor, injury, or final line.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. The account cannot add a body, survivor, injury, or final line. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 035 — The intact slings — The Words That Remain

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 036 — The choice heard by others — The Words That Remain

The two proposed speakers meet over the choice heard by others at a proposed later conversation about the authored outcome. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 037 — A channel left open — The Words That Remain

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 038 — The column that held — The Words That Remain

At a proposed later conversation about the authored outcome, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: The account cannot add a body, survivor, injury, or final line.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. The account cannot add a body, survivor, injury, or final line. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 039 — Two epitaphs, two choices — The Words That Remain

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 040 — The far end of a jack — The Words That Remain

The two proposed speakers meet over the far end of a jack at a proposed later conversation about the authored outcome. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

#### Conversation fragment 041 — The first syllable of the call — The Epitaphs Kept Apart

The room has gone quiet. Someone asks the listener to make the voice sound braver. The listener declines. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “Did they sound frightened?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “They sounded like people keeping a channel open.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 042 — The wrong column — The Epitaphs Kept Apart

At the separate authored epitaphs for the existing choices, one person looks again at the detail: The source states that Crew Two cut the wrong column and the mezzanine answered; the scene does not explain a method or name a cutter. The channel listener has a different reason for staying: Credit, grief, and blame remain distinct.

After-account keeper: “The record says Crew Two. It does not give us one hand to blame.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Credit, grief, and blame remain distinct. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 043 — The intact slings — The Epitaphs Kept Apart

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer assumes that intact tools made the situation manageable. They are trying to say what this one detail means to them.

Channel listener: “Didn’t they have what they needed?”

After-account keeper: “They had equipment. The call says they still needed another end of it.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer assumes that intact tools made the situation manageable. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 044 — The choice heard by others — The Epitaphs Kept Apart

The two proposed speakers meet over the choice heard by others at the separate authored epitaphs for the existing choices. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “We can tell the part we chose. The rest is already written.”

Channel listener: “Can we tell only the part that lets us sleep?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The retelling distinguishes consequence from a lecture. That callback changes the audience, not the source fact.

#### Conversation fragment 045 — A channel left open — The Epitaphs Kept Apart

The room has gone quiet. A listener remembers waiting for a voice that never came back. The later hearer lets the other two decide whether the same words can hold what they remember about Warehouse District.

Channel listener: “What should I write after the last signal?”

Later hearer: “What would you need before you wrote that down?”

After-account keeper: “Only that the channel closed.”

The final exchange is practical rather than grand. Someone asks what needs doing next; someone else names an ordinary task. The unresolved point does not own the whole day.

Later hearer: “I will remember that you said it that way.”

The line does not mean the listener agrees. It means the speaker has been heard without being made into a source for more than they know.

The speakers separate without a winner. One repeats the question once, then lets it go. The other does not have to agree before the scene can close.

#### Conversation fragment 046 — The column that held — The Epitaphs Kept Apart

At the separate authored epitaphs for the existing choices, one person looks again at the detail: The dispatch outcome says the column holds long enough for Crew Two to leave with the rest of them. The channel listener has a different reason for staying: Credit, grief, and blame remain distinct.

After-account keeper: “Long enough for the outcome the record gives us.”

Later hearer: “Would you let somebody else repeat it?”

Channel listener: “Only if they kept the question beside it.”

The pause after the last line is long enough for someone to change the subject. Credit, grief, and blame remain distinct. The disagreement belongs to these speakers; it is not a hidden answer the player has to unlock.

The after-account keeper turns toward the next task but does not withdraw the answer. The channel listener lets the sentence stand without writing a conclusion beneath it.

The player may carry one line forward or none. The proposed speakers keep their agency and the account remains attached to what they could know.

#### Conversation fragment 047 — Two epitaphs, two choices — The Epitaphs Kept Apart

The conversation starts with the people who are here, not with a speech about everyone else. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. They are trying to say what this one detail means to them.

Channel listener: “Which line belongs to the story?”

After-account keeper: “The one tied to the choice that happened in that version.”

Channel listener: “Then I’ll write what I saw and leave the rest with you.”

The later hearer does not rush to decide which account is more useful. A later hearer finds both lines and asks whether they are competing accounts or branch-specific writing. The silence lets both speakers hear how much they have assumed.

A practical question interrupts the silence: “Do you want the page back?” The answer may be yes, no, or not yet. That small choice gives the speakers a life beyond the argument.

The scene can end on a small action: a page turned face down, a cup set back on the table, or the listener asking a new question. It need not announce what the player should feel.

#### Conversation fragment 048 — The far end of a jack — The Epitaphs Kept Apart

The two proposed speakers meet over the far end of a jack at the separate authored epitaphs for the existing choices. The channel listener is trying to keep the account useful; the after-account keeper is trying to keep it honest.

After-account keeper: “The outcome says what the player did. It does not name a new crew member.”

Channel listener: “Who stood at the other end?”

After-account keeper: “That is not the whole answer. It is the part I can stand behind.”

The player need not side with either voice. They can ask who supplied the first account, repeat one sentence with its author, or leave the room.

The channel listener starts to reply, then lets the after-account keeper finish. Nobody wins the exchange; one person has simply been allowed to complete a thought.

One listener carries this away: The last reading honors the authored words by not enlarging them. That callback changes the audience, not the source fact.

### Consequence vignettes

#### Consequence vignette 001 — The first syllable of the call — The Channel Before the Choice

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A listener can hear a request without knowing the full scene. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A listener can hear a request without knowing the full scene. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 002 — The wrong column — The Channel Before the Choice

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 003 — The intact slings — The Channel Before the Choice

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 004 — The choice heard by others — The Channel Before the Choice

This return vignette begins after the player has encountered the choice heard by others. The setting is the quest’s authored distress transmission, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. A listener can hear a request without knowing the full scene. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 005 — A channel left open — The Channel Before the Choice

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. A listener can hear a request without knowing the full scene. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. A listener can hear a request without knowing the full scene. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 006 — The column that held — The Channel Before the Choice

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 007 — Two epitaphs, two choices — The Channel Before the Choice

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 008 — The far end of a jack — The Channel Before the Choice

This return vignette begins after the player has encountered the far end of a jack. The setting is the quest’s authored distress transmission, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. A listener can hear a request without knowing the full scene. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 009 — The first syllable of the call — The Wrong Column

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. The error matters; it does not explain every person in the room. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. The error matters; it does not explain every person in the room. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 010 — The wrong column — The Wrong Column

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 011 — The intact slings — The Wrong Column

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 012 — The choice heard by others — The Wrong Column

This return vignette begins after the player has encountered the choice heard by others. The setting is the quest’s exact account of Crew Two’s cut and the resulting collapse, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. The error matters; it does not explain every person in the room. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 013 — A channel left open — The Wrong Column

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. The error matters; it does not explain every person in the room. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. The error matters; it does not explain every person in the room. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 014 — The column that held — The Wrong Column

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 015 — Two epitaphs, two choices — The Wrong Column

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 016 — The far end of a jack — The Wrong Column

This return vignette begins after the player has encountered the far end of a jack. The setting is the quest’s exact account of Crew Two’s cut and the resulting collapse, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. The error matters; it does not explain every person in the room. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 017 — The first syllable of the call — The Tools They Kept

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Having tools is not the same thing as being able to leave. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Having tools is not the same thing as being able to leave. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 018 — The wrong column — The Tools They Kept

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 019 — The intact slings — The Tools They Kept

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 020 — The choice heard by others — The Tools They Kept

This return vignette begins after the player has encountered the choice heard by others. The setting is the authored jacks and slings, described as intact, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. Having tools is not the same thing as being able to leave. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 021 — A channel left open — The Tools They Kept

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Having tools is not the same thing as being able to leave. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Having tools is not the same thing as being able to leave. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 022 — The column that held — The Tools They Kept

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 023 — Two epitaphs, two choices — The Tools They Kept

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 024 — The far end of a jack — The Tools They Kept

This return vignette begins after the player has encountered the far end of a jack. The setting is the authored jacks and slings, described as intact, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. Having tools is not the same thing as being able to leave. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 025 — The first syllable of the call — After the Choice

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Keep each authored branch outcome and epitaph together; never blend them. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Keep each authored branch outcome and epitaph together; never blend them. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 026 — The wrong column — After the Choice

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 027 — The intact slings — After the Choice

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 028 — The choice heard by others — After the Choice

This return vignette begins after the player has encountered the choice heard by others. The setting is a proposed reading after the existing distress choice, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. Keep each authored branch outcome and epitaph together; never blend them. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 029 — A channel left open — After the Choice

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Keep each authored branch outcome and epitaph together; never blend them. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Keep each authored branch outcome and epitaph together; never blend them. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 030 — The column that held — After the Choice

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 031 — Two epitaphs, two choices — After the Choice

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 032 — The far end of a jack — After the Choice

This return vignette begins after the player has encountered the far end of a jack. The setting is a proposed reading after the existing distress choice, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. Keep each authored branch outcome and epitaph together; never blend them. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 033 — The first syllable of the call — The Words That Remain

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. The account cannot add a body, survivor, injury, or final line. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. The account cannot add a body, survivor, injury, or final line. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 034 — The wrong column — The Words That Remain

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 035 — The intact slings — The Words That Remain

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 036 — The choice heard by others — The Words That Remain

This return vignette begins after the player has encountered the choice heard by others. The setting is a proposed later conversation about the authored outcome, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. The account cannot add a body, survivor, injury, or final line. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 037 — A channel left open — The Words That Remain

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. The account cannot add a body, survivor, injury, or final line. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. The account cannot add a body, survivor, injury, or final line. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 038 — The column that held — The Words That Remain

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 039 — Two epitaphs, two choices — The Words That Remain

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 040 — The far end of a jack — The Words That Remain

This return vignette begins after the player has encountered the far end of a jack. The setting is a proposed later conversation about the authored outcome, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. The account cannot add a body, survivor, injury, or final line. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 041 — The first syllable of the call — The Epitaphs Kept Apart

Later, the later hearer hears one version of what happened at Warehouse District. The quest’s discovery text says the channel is open; this proposed scene begins with the listener repeating only what they heard. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Credit, grief, and blame remain distinct. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Credit, grief, and blame remain distinct. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 042 — The wrong column — The Epitaphs Kept Apart

The callback comes in an ordinary conversation. A listener wants to name the mistake after one person; the record keeper refuses a single culprit the source does not provide. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “The record says Crew Two. It does not give us one hand to blame.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 043 — The intact slings — The Epitaphs Kept Apart

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. The tools remain evidence of effort, not an escape guide.

If the player carried the first account forward, the listener receives: “Didn’t they have what they needed?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 044 — The choice heard by others — The Epitaphs Kept Apart

This return vignette begins after the player has encountered the choice heard by others. The setting is the separate authored epitaphs for the existing choices, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “We can tell the part we chose. The rest is already written.”

The first exchange goes uncarried. Credit, grief, and blame remain distinct. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The retelling distinguishes consequence from a lecture. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

#### Consequence vignette 045 — A channel left open — The Epitaphs Kept Apart

Later, the later hearer hears one version of what happened at Warehouse District. The disregard outcome says the channel stays open until it does not; the scene does not invent a final phrase. The detail is familiar; its meaning is not.

The first voice gets one more chance to explain why the wording mattered. Credit, grief, and blame remain distinct. The later reader accepts the explanation without being forced to share it.

If the player declines to interpret the line, the proposed speakers still decide what they themselves will say. Their agency continues after the player leaves.

The later hearer asks one plain question: “Will you tell me who said it?” The answer decides whether the next retelling can keep its source attached.

The two continuations are editorial alternatives and should not be merged into one canon event without a verified source. Credit, grief, and blame remain distinct. Either can close on a human consequence rather than a mechanic.

#### Consequence vignette 046 — The column that held — The Epitaphs Kept Apart

The callback comes in an ordinary conversation. A returnee is tempted to call it a miracle; the keeper insists on the plain sentence already authored. Someone repeats a phrase from the earlier scene and waits to see whether the next reader will accept it as fact.

A person asks for the phrase’s author. The player can preserve that attribution or leave the question open. The conversation remains human; no one turns it into a public verdict.

If the player left the first account where it was, the second reader encounters the blank as a blank. They ask whether it was deliberate. A nearby voice answers: “Long enough for the outcome the record gives us.” Nobody fills the space with a guessed identity.

The new reader repeats the line slowly, then corrects one word. The original speaker is allowed to say whether the correction fits. The scene ends before their answer becomes a rule for anyone else.

Let the callback end here. It establishes no new reward, relationship delta, memorial record, route, or saved memory. The reader leaves with a sentence they can question.

#### Consequence vignette 047 — Two epitaphs, two choices — The Epitaphs Kept Apart

A second reader arrives with no special knowledge. They know only the sentence that traveled and the part that was left blank. A future reader can understand why the language differs.

If the player carried the first account forward, the listener receives: “Which line belongs to the story?” They ask who said it and what that person saw. The answer is small but useful: the person who supplied the sentence is named before it is repeated.

The alternative return leaves the account where it was. The next person is allowed to feel dissatisfied. Restraint is not presented as success, and silence does not erase the original speaker.

Someone sets the copied phrase down between them. It is not evidence of agreement; it is evidence that two people were willing to keep talking after they disagreed.

The final image is the new reader taking a moment before repeating what they heard. That pause is enough consequence for this passage; no larger outcome is promised.

#### Consequence vignette 048 — The far end of a jack — The Epitaphs Kept Apart

This return vignette begins after the player has encountered the far end of a jack. The setting is the separate authored epitaphs for the existing choices, but the new scene belongs to someone who was not present for the first exchange.

One possible continuation follows the player carrying the first account forward. The next reader may disagree, but cannot mistake the sentence for an anonymous fact. Their reply is: “The outcome says what the player did. It does not name a new crew member.”

The first exchange goes uncarried. Credit, grief, and blame remain distinct. The later reader can ask someone else, accept uncertainty, or return to the ordinary work waiting nearby.

The room turns back to the ordinary work of the day. The detail remains available to memory, but no character is required to make it their whole identity.

Both branches return to the same human detail. The last reading honors the authored words by not enlarging them. The place and its catalog facts are unchanged; only the listener’s relationship to the words has shifted.

## 15. Character and relationship continuity

The proposed voices are roles created for editorial exploration. Do not silently promote them into named survivors, faction leaders, quest givers, or permanent settlement residents. Their relationships remain within each selected passage unless another current source establishes a return. The story can leave a relationship unresolved without creating a hidden standing change.

## 16. Passage-selection map

**Use a scene** when a verified consumer can stage an observable moment without implying unsafe traversal.
**Use a record** when the game already has an appropriate reader-facing content owner and attribution can be preserved.
**Use a conversation** when the passage benefits from two people who disagree in good faith.
**Use a consequence vignette** only when the existing route exposes the condition that selects it; otherwise use a non-conditional close or omit the variant.
These are editorial selection notes, not proposed runtime features.

## 17. Content boundaries and open questions

| Topic | Current evidence | Editorial limit |
|---|---|---|
| Anchor | loc_warehouse_district in locations.json | Do not infer a new route, encounter, or exact physical staging from presence in the catalog. |
| Existing prose/state | loc_warehouse_district | Attribute exact source text; do not silently amend it. |
| Proposed people | Anonymous roles listed above | Keep each role editorial unless a verified source names an existing character. |
| Player response | Four prose alternatives per beat | Do not claim a new menu, flag, score, reward, or save behavior. |
| Hazard or scarcity | The location record describes corrugated freight-terminal warehouses, broken pallets, shadowed catwalks, and holding pens used by scavenger gangs. The distress quest says Crew Two cut the wrong column, the mezzanine collapsed, and three people are pinned with their own jacks and slings intact. Its two choices already set flag_responded_distress or flag_ignored_distress and supply their own outcome text and epitaphs. This plan preserves those outcomes and adds no new branch state. | Do not give instructions, safety guarantees, or unverified outcomes. |

Do not change, soften, or enlarge either authored outcome. Do not invent an extra survivor, a body, a rescue technique, a new moral score, or a third choice. If a passage depends on the response flag, verify the current owner and consumer before selecting it. No rescue is staged as a tutorial; the crew’s equipment is a source fact, not an instruction.

## 18. Local-canon and collision audit

The direct anchor loc_warehouse_district was not used as the location anchor of the existing prose-plan files inspected for this batch. This is a narrow collision check, not a claim that no related theme exists anywhere in the project. Search again before implementation. The cited source records above are the canon boundary; all proposed scenes, voices, and artifacts must be checked against any newer narrative data before they are selected.

No real-world country, war, person, copied art, copied text, or real-world interface layout is introduced. The prose is original and specific to the local fictional records.

## 19. Acceptance and handoff

- Keep the 48 beats as an optional content bank; do not implement all 192 candidates by default.
- Preserve the current source facts and named-character outcomes.
- Verify a real content consumer and route before selecting a passage.
- Keep new voices and props editorial until an authorized owner accepts them.
- Do not change production code or authoritative game data as part of this plan.
- Review voice distinction, factual boundaries, and branch attribution before content placement.

This file is a complete prose expansion proposal. It contains no implementation claim and no assertion that any candidate passage is already reachable.
