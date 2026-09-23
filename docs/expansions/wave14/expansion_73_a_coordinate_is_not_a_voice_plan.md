# EXPANSION 73 — A COORDINATE IS NOT A VOICE
## A Broadcast Bunker Echo Story Plan
### Wave 14: Authority Without a Witness

**Document type:** prose-first game-content expansion plan  
**Primary location anchor:** Broadcast Bunker Echo  
**Signal anchor:** Civil Defense Carrier fingerprint mapped to loc_broadcast_bunker_echo  
**Status:** proposed narrative content, subject to current route, source, and canon review  
**Wave theme:** records and signals can locate something without telling us who is there  
**Scope:** authored dialogue, journal prose, oral memory, and player choice through existing radio and narrative owners  
**Non-goal:** no new radio, triangulation, message-decoding, faction, expedition, or save system

## 1. The story in one sentence

A direction-finding result points toward Broadcast Bunker Echo, where emergency transceivers still hiss with static; the player helps a listener distinguish a mapped carrier from the human voice they wish to hear.

## 2. Creative promise

The player meets people listening to a signal that has a catalog identity and a mapped location, but no sentence in the current story. One character recognizes the phrase “Civil Defense” from old public language and assumes someone may be calling for help. Another insists that the signal’s class and position do not establish a speaker, message, or intention.

The story does not make hope foolish. It lets a person ache for a familiar voice and decide what to do with that hope without turning a radio fingerprint into a reunion.

The player’s visible outcome is a journal note and a conversation that keeps three things apart: a signal identity, a direction-finding location candidate, and a human message. The bunker does not deliver a new broadcast, a quest code, or guaranteed recovery.

## 3. Verified local location source

The location record names Broadcast Bunker Echo and describes a reinforced subsurface relay bunker beneath an antenna lattice on the central ridge. Its blast door is jammed half-open. High-gain receiver racks line the operational bay beneath a steady drip of condensation. The emergency transceivers still hiss with static.

This is the location’s current authored description. It does not say that a person is speaking, that a message can be decoded, or that the player has physically entered the bunker.

The world-map node is discoverable, not starting-unlocked, and marked high danger. The current expeditions catalog has no row for this location. This plan does not create one or imply that the rumor grants a travel destination.

## 4. Verified direction-finding source

The direction_finding_catalog contains a fingerprint:
- fingerprint_id: fp_civil_defense_carrier
- signal_id: sig_civil_defense
- display_name: Civil Defense Carrier
- mapped_location_id: loc_broadcast_bunker_echo
- identity_confidence: 0.85

The Core’s SignalTriangulationSystem keeps identity confidence distinct from triangulation confidence. The mapped location is used only when location confidence reaches its threshold and the minimum observation count is met. The 0.85 field is not an 85 percent chance that a person is speaking from the bunker.

## 5. Current host and map behavior

When the current triangulation event reveals a mapped location, Main.Narrative writes a journal entry stating that direction-finding telemetry confirmed active radio emissions at that location. The host then records a Medium-confidence map rumor through the existing world-map owner. This is not the same as a surveyed, physically visited location.

The current TriangulationPanel displays the signal ID, candidate location, calculated confidence, uncertainty radius, and discovery state. It does not expose the catalog’s identity_confidence value as a player-facing percentage.

This plan builds on the existing result and journal seam. It does not change how signals are observed, triangulated, rumored, or saved.

## 6. Distinguish this site from nearby signal lore

Broadcast Bunker Echo is loc_broadcast_bunker_echo. It is not Hidden Relay Bunker 09 (loc_hidden_relay_bunker), which has its own location description, surveyor’s log, distress fingerprint, and prior expansion coverage. This plan does not reuse the Relay Count, number-station decoding, distress-burst pattern, or the previous bunker’s story.

It is also distinct from loc_civil_defense_bunker, the station_civil_defense radio-station record, and Civil-Defense Emergency Transmitter distress content. Similar wording is not a continuity link.

The Sun Seekers’ territory data and the world-map node’s faction field are separate current records. This plan does not decide whether a faction controls the Echo bunker.

## 7. The human conflict

Ena Vol hears the catalog label “Civil Defense Carrier” and wants it to mean a person is still trying to reach someone. She has waited for news from a brother who left before the current settlement formed. The story does not claim that he used this signal or that he is near the bunker.

Daro Senn operates the group’s listening notes. He can explain that a carrier classification is not a transcript, but he fears that his careful wording will become another way to tell Ena not to hope.

Mira Tol keeps the community’s message board. She wants the location rumor to be useful, but refuses to repeat “someone is calling” without a message.

The player helps them find language that lets hope exist without putting words in the static.

## 8. Character: Ena Vol

Ena remembers the civil-defense phrase from printed notices and old recordings, but she does not claim to recognize this carrier. Her brother’s departure is a proposed piece of present-day character history, not a connection to the source signal.

Ena knows the difference between a familiar label and a familiar voice. She still wants the label to lead somewhere human.

> “If it says Civil Defense, somebody chose those words.”
>
> “Maybe someone chose a catalog label.”
>
> “That is a smaller somebody.”
>
> “It is still somebody.”
>
> “And if that is all we get?”
>
> “Then we say that is all we got.”

## 9. Character: Daro Senn

Daro writes down what the listening station actually shows. He does not claim to recognize the signal or decode its content. His instinct is to put every unknown in a separate field, which makes his notes technically tidy and emotionally difficult to hear.

Daro’s growth is learning that precision need not sound dismissive:
> “I cannot tell you who.”
>
> “Can you tell me where?”
>
> “The system gives a candidate location.”
>
> “Does that help?”
>
> “It tells us which question we are asking.”

Daro’s lines should remain ordinary, not an engineering tutorial.

## 10. Character: Mira Tol

Mira moderates the settlement message board and understands how a short sentence can outrun its source. She wants people to know about the rumor because the location might matter; she will not turn the discovery note into a rescue announcement.

Mira’s conflict is public responsibility. If she posts “active radio emissions,” people may hear “someone is transmitting.” If she posts only “map rumor,” people may miss the human stakes.

Her voice asks:
> “What will they repeat when we are not in the room?”
>
> “The short part.”
>
> “Then the short part has to be true by itself.”

## 11. Character: Jori Pell

Jori listens with Ena and has spent years identifying familiar patterns in household sounds. Jori is not a radio technician. They understand how grief can make a noise feel addressed to you.

Jori’s contribution is a gentle challenge:
> “I heard my name in a pump once.”
>
> “You did?”
>
> “No. I heard a pump. Then I heard what I wanted.”
>
> “Did it help?”
>
> “For a second. Then I had to decide what to do with the second.”

Jori’s story does not mock Ena. It gives her a companion who understands without confirming the signal.

## 12. Relationship map

Ena and Daro disagree about what the label can mean. Mira and Jori disagree about how public the rumor should be. Daro worries about precision becoming a wall; Mira worries about imprecision becoming a promise.

The player is not asked to decide whether Ena’s brother is alive or whether the signal is intended for the settlement. Their choice selects the public wording and which character’s line closes the journal scene.

The location rumor and signal state remain owned by the existing triangulation and map authorities.

## 13. Narrative arc

**Act I — A candidate appears.** The existing direction-finding result identifies a location candidate and the host writes its active-emissions journal entry.

**Act II — The label becomes a voice.** Ena hears “Civil Defense Carrier” as a possible appeal; the group separates a label, a signal, and a message.

**Act III — The rumor travels.** The player helps Mira write a short note that can be repeated without inventing a speaker.

**Act IV — Listening remains human.** Ena chooses what she wants to hope while the archive keeps the current result narrow.

The story can be delivered in the shelter after a triangulation rumor. It does not require the player to travel to Echo Bunker or hear an invented transmission.

## 14. Opening scene: the journal line

The player opens the journal after the current direction-finding event. The existing entry says:
> Direction-finding telemetry confirmed active radio emissions at loc_broadcast_bunker_echo.

Daro reads the line and says:
> “That is a location statement.”
>
> “It sounds like a person,” Ena says.
>
> “It might involve a person.”
>
> “Is that different?”
>
> “It is different until we have a message.”

Mira asks the player whether the journal line should be followed by an authored character note. The player may agree, ask for source clarification, or leave the automated entry alone.

## 15. Opening player choice: what does “active” mean?

The player may ask Daro whether “active” means someone is operating the transmitter. Daro says the host journal records active radio emissions when the existing triangulation event fires; it does not identify a human operator.

The player may ask whether the location description’s static is the same signal. Daro says the available records do not establish that exact relationship.

Mira notes:
> “A word can be accurate and still invite a story.”
>
> “Then what do we do?”
>
> “We keep the accurate word and tell the story as ours.”

This distinction is central to the entire expansion.

## 16. The first signal note

Daro drafts a present-day note:
> Signal ID: sig_civil_defense.
> Catalog fingerprint display name: Civil Defense Carrier.
> Mapped location: Broadcast Bunker Echo.
> Message content and speaker identity: not present in this readout.

The last line describes the current readout used for this story; it does not assert that no other radio content exists anywhere in the game. If a future source supplies a message, it belongs in its own source record.



## 17. A label is not a voiceprint

Ena asks if the carrier sounds like the old public-defense recordings she remembers from childhood. Daro answers that the catalog label can identify a class of signal without identifying an individual voice. He does not dismiss her ear. He asks her to separate the feeling of recognition from the evidence that would support recognition.

> “It has the shape of a familiar word.”
>
> “That is not what I asked.”
>
> “No. I know.”
>
> “Then answer the question I did ask.”
>
> “I cannot tell you it is your brother. I cannot tell you it is not.”
>
> “That answer does not leave much room.”
>
> “It leaves the room where we are standing.”

This exchange is not a puzzle with a hidden correct answer. The player may tell Daro that his answer is honest, tell him it is too cold, or ask him to let Ena decide what the word means to her. All three options keep the signal record unchanged. The difference is the emotional temperature of the next scene, not the result of a check.

## 18. The room in which they listen

The listening room is a table in the settlement’s common shelter, not a newly authored radio facility. The group has set out a folded map, Daro’s handwritten transcription of the host journal line, a kettle, and a piece of cloth Mira uses to cover the notice board while she writes. No one turns a dial to a new frequency. No one plays an audio asset. No one hears an incoming transmission during the scene.

A metal cup ticks as it cools. Ena watches the map as though the central ridge might move if she stares at it long enough. Daro keeps his pencil above the paper. Mira has brought a public notice card and has not yet taken the cloth away from the board. Jori pours tea into five cups, including one for the player, then asks who wants the cup that has a crack through its handle.

The small domestic question lets the story pause. Ena chooses the cracked cup because she has held it before. That familiarity is real and verifiable in the room. The carrier is not. The scene asks the player to feel the difference without turning that difference into a lecture.

## 19. Choice: what do you hear in the name?

When the player asks what “Civil Defense Carrier” means to each person, responses are personal interpretations, not catalog definitions.

**“I hear a warning.”** Ena answers that she hears a phrase from old notices: doors closed, neighbors counted, someone responsible for the list. Daro says that this is what the words bring back for her; it does not tell them who transmitted this carrier. Mira says a warning can be remembered without claiming one has arrived.

**“I hear a label.”** Daro agrees, then corrects himself when he sees Ena stop looking at the map. “I hear a label. You hear something that used to mean people would come back for each other.” Ena says that his correction is better than his first sentence, though it does not make the room easier.

**“I hear my own wish.”** Jori says the wish is not a fault. It is a fact about the listener, and facts about listeners belong in the story as clearly as facts about radios. Ena can accept this, say it sounds like being put on display, or ask Jori not to make her hope into an example.

Each response opens a different follow-up line, but none opens a signal-decoding route. The player’s selection is a conversational stance only.

## 20. Ena’s private note

Ena keeps a folded note in her coat pocket. It is a list of things she wants to tell her brother if she ever meets him: the settlement has moved the wash line twice; the old kettle is gone; she still dislikes pear jam; she never learned whether he took the blue scarf. The plan proposes this note as original character writing, not as a recovered message or a source attached to the fingerprint.

If the player asks to read it, Ena says yes only after a short silence and only in the private conversation. The player may read the note with her, ask her to put it away, or tell her the list does not need to become proof of anything. If the player has previously chosen a hard-edged reply, Ena can decline the offer to read it. That refusal is respected without penalty or hidden relationship score.

The note never contains the bunker name, the carrier ID, a frequency, a code, a date that lines up with telemetry, or a claim that her brother served in any organization. It is a human object whose meaning comes from the person who wrote it. Its dramatic force is that Ena has been preparing a conversation without knowing whether a conversation will occur.

> “I wrote down the small things because the large ones kept changing.”
>
> “Do you still want to tell him?”
>
> “I want to be able to want it.”
>
> “That is not the same?”
>
> “It is enough of the same for tonight.”

## 21. What the player may say about the brother

The player can ask whether Ena believes her brother is alive. She can answer that belief changes from day to day. She will not convert the direction-finding candidate into a sign. She may say she has no evidence that he used this carrier and no evidence that he did not. The player may sit with the uncertainty, offer a practical kindness such as returning the note, or ask whether she wants help searching the settlement’s existing records. The story does not create a new search action or promise a later result.

If the player suggests that the signal has found her, Ena asks what the player means by “found.” A carrier mapped toward one location has not selected her name. If the player says “I only meant that I hope,” Ena accepts the correction. If the player insists, Daro asks that the statement not be written into the public notice.

If the player says her brother is certainly gone, Ena asks what evidence made the player certain. There is none in this story. A humane response acknowledges that the player does not know. The player may be impatient, but the narrative does not reward a false certainty with a special outcome.

## 22. The distinction between a signal and an address

Mira writes three words on the notice card: “carrier,” “location,” “message.” The words are spaced apart. She asks the group which of them the current result establishes.

Daro places the catalog name under “carrier.” He places Broadcast Bunker Echo under “location candidate.” The message column remains empty.

> “A blank column looks like a dare,” Mira says.
>
> “Then we can label it,” Daro answers.
>
> “With what?”
>
> “Not in this readout.”
>
> “That sounds like a refusal.”
>
> “It is a boundary. It can be both.”

The empty column is not a gameplay slot the player can fill. It is a prop in the scene. It should be drawn or written as a blank field on the paper, not exposed as a new interactive device or signal-state UI.

## 23. The carrier’s title and the weight of old language

The phrase “Civil Defense” has a history for each listener. Ena remembers being told where to stand while adults checked names. Daro remembers finding the words printed on a damaged box without knowing what the box once held. Mira remembers a neighbor using the phrase as shorthand for any announcement that sounded official. Jori remembers none of that; to Jori it is a pair of words other people bring into the room.

These memories are compatible because they belong to the characters. The game should not present any one recollection as the canonical origin of the current catalog entry. The local source names a Civil Defense Carrier; it does not explain its maker, former operator, original audience, surviving infrastructure, or intended use. Character memory can make the language resonant without supplying missing metadata.

A player may ask whether the signal belongs to an old civil authority. Daro says the catalog does not answer that. Mira adds that a name can outlast the office that used it. Ena says that some names outlast the people who needed them. The exchange settles nothing about the signal and deepens what the phrase means at the table.

## 24. A public notice is not a rescue call

Mira drafts the first version of a notice:

> Direction-finding telemetry has recorded active radio emissions associated with a candidate at Broadcast Bunker Echo. The current entry does not provide a speaker or message.

She asks the player whether this is clear enough for people who have not sat through Daro’s explanation. The player may retain “candidate,” ask for a shorter version, or say the notice should not be posted yet.

The short version reads:

> A direction-finding rumor points toward Broadcast Bunker Echo. No message or speaker is confirmed by this entry.

If the player chooses not to post, Mira keeps the card beneath the cloth and notes that the group can revisit the wording when there is a reason. This choice does not suppress the underlying map rumor or alter the triangulation result. It governs only whether the authored community note appears in this narrative scene.

No version says “someone is waiting,” “a survivor is calling,” “Ena’s brother may be there,” or “the bunker has been found and cleared.” The notice is allowed to be emotionally significant without becoming a dispatch.

## 25. A note passed hand to hand

The card travels around the table. Jori reads it silently. Daro checks that “candidate” has not been removed. Ena reads it last and asks Mira whether people will think the phrase means they should go.

Mira answers that some people may think so regardless of the words. That is why she wants the words to say what the group knows, not what it fears or hopes. Daro points out that “no message confirmed by this entry” can sound as if another source might confirm one later. The player may keep the line because it is the exact limit of this entry, or choose “this notice carries no message from the bunker” because it describes the card rather than the whole world.

That second wording must stay carefully attached to the notice. It means the card does not carry a transmitted message; it does not assert that no message exists anywhere in the fiction. Mira can add a final line in her own voice: “Ask before repeating someone else’s hope as fact.”

## 26. Optional dialogue: the impatience of a listener

A survivor passing the common shelter hears the word “bunker” and asks whether the group is finally going to check it. This is an ambient conversation, not a quest offer and not a new travel route. The person has heard the rumor and wants a concrete action.

The player may answer that the map has a candidate, that they do not know, or that the rumor can wait. If they say the bunker is confirmed safe, Daro quietly corrects the record after the passerby leaves. The correction is not a punishment. It shows how fast a location statement can become a safety claim.

The passerby may reply:
> “A place on a map is better than a place in a story.”
>
> “Only if you can get there.”
>
> “Can we?”
>
> “That is a different question.”

The exchange should end without a travel prompt, danger estimate, loot teaser, door interaction, or promised expedition. It creates pressure around action while respecting the absent route.

## 27. The map on the table

Daro folds the map along the central ridge. The candidate mark is printed or represented in the existing map interface according to the current medium-confidence rumor behavior; the scene does not redraw the world map or add an icon that implies physical survey.

Ena traces the ridge in the air, stopping before she touches the page. She asks whether the uncertainty circle includes the old service road. Daro says the player-facing triangulation panel can display a calculated uncertainty radius, but this scene does not know more than the panel. It cannot turn the boundary into a route or point to a door.

The player can ask Daro to show the current display, ask Mira to put the map away, or ask Ena whether seeing the candidate helps. Each answer is emotional and practical:
- The display can clarify where the system points without identifying a person.
- Putting it away can keep the room from becoming an investigation the story cannot support.
- Ena may say it helps to have somewhere to look, then admit that somewhere is not the same as someone.

The map remains the existing map owner’s state. The prop scene reads what the current presentation already exposes; it does not own a second coordinate.

## 28. The word “Echo”

Jori asks why the location is called Broadcast Bunker Echo. No record inspected for this plan explains the word. Mira guesses it may be an old site name. Daro refuses to write the guess as fact. Ena says “echo” is what you call a sound after it has already happened, which is a character’s metaphor, not a location-history reveal.

The player may ask whether a place named Echo must have transmitted a message. Daro says names are not evidence of function. The existing location description mentions emergency transceivers and static, but it does not explain the name or tie the static to this signal fingerprint.

The story may let the word echo recur as an image: someone repeats a sentence; someone answers too late; an empty line on a card holds the shape of a response. The repetition belongs to the authored theme. It is not an explanation of the bunker’s infrastructure or the signal’s technical behavior.

## 29. A second listening scene, without a second transmission

Later that evening, the player finds Ena and Jori sitting near the shelter door. The group has not received a new message. There is no audio effect, no radio log, and no unseen voice. The scene opens on Ena asking Jori to repeat the sentence about the pump.

Jori says the pump made a sound that could be mistaken for a syllable if someone was already waiting to hear one. Ena says that she knows what the comparison means and dislikes what it does to her. Jori apologizes. Ena asks them not to apologize for understanding her; she only wants the story to leave room for the fact that a wish can be real even when its object is unknown.

The player can agree, say that Jori should have asked first, or invite Ena to name what she wants the room to hear. If the player chooses the invitation, Ena does not imitate a voice or invent a call sign. She says, “I want to hear that he is alive. I want that to be a thing I want, not a clue.”

## 30. Narrative delivery and timing

The opening journal scene becomes eligible only when the existing direction-finding reveal for the mapped Echo candidate has entered the current campaign’s narrative path. The authored follow-up may be delivered through the existing journal or shelter conversation pattern. The exact trigger must use the current owner and existing flags after premise recheck; this document does not name a new persisted flag or mandate a new trigger API.

If delivery is delayed, the story should not imply the rumor was recent. Characters can say “the entry” or “the map note” rather than “tonight’s signal.” If the player has already opened the current map rumor, Daro can point to it in dialogue. If not, the scene should not expose hidden map state in a way the UI does not.

The follow-up should be available once per ordinary conversation sequence, with repeat dialogue limited to a short reminder. Do not add an independent cooldown, queue, or persistent state machine. If the present narrative framework cannot express this timing cleanly, keep the plan as authored text for an existing route and report the missing route as a dependency instead of creating a new system.

## 31. Scene outline: act one, the room takes shape

The player enters on the tail end of a conversation. Daro has already copied the journal statement onto a scrap. Mira is holding the notice card. Ena has not yet asked for it. The first line belongs to Jori, who asks whether the card should be read aloud or placed face down.

The player chooses between reading the entry, asking Daro what the wording means, or asking Ena what she heard. All routes converge on the same distinction, but each gives a different character the first sustained line:
- Reading it aloud lets Daro explain the statement.
- Asking Daro lets him explain his care with categories and then notice Ena’s reaction.
- Asking Ena gives her first-person interpretation before the group narrows it.

The choice changes the order and emphasis of the scene. It does not change the source evidence.

## 32. Scene outline: act two, a wish enters the room

Once the entry is read, Ena says she wishes the signal were someone calling. The line is deliberately plain. The other characters do not immediately debate her. The pause gives the player room to answer in a human voice rather than choose an evidence label.

A supportive answer can say that wanting a voice does not make one appear. A guarded answer can say that the record does not say that. A blunt answer can say no one knows. Each response receives a different reaction:
- Ena accepts support if it does not promise an outcome.
- Daro appreciates the guarded wording but can recognize when it sounds like a correction before comfort.
- Mira asks whether a statement is being offered to Ena or written for the notice.
- Jori reminds the player that silence can be a response too.

The scene avoids a morality score. No response unlocks a more accurate result, a secret recording, or a different signal classification.

## 33. Scene outline: act three, write the note

Mira puts the cloth over the notice board. This is her habit when she drafts something that is not ready for the public. She asks the player to help compress the facts into a sentence that will survive being repeated.

The player chooses the long version, short version, private version, or no notice. The long version retains “active radio emissions” and “candidate location.” The short version says a direction-finding rumor points toward Echo and no speaker or message is confirmed by this entry. The private version stays in the room as a note for the group. The no-notice choice lets Mira wait.

If the player tries to write a rescue announcement, Daro asks whose evidence supports it. The player can revise, defend the guess as hope, or withdraw it. Defending hope is allowed as an emotional statement but not as a factual public notice. Mira offers two separate lines: “I hope someone is there” can be signed as a person’s hope; “someone is there” cannot be signed by the evidence.

## 34. Scene outline: act four, let the silence belong to them

After the note is chosen, the characters put away the map. The ending line changes by route. If the player chose a public notice, Mira says she will read it exactly as written. If the player kept it private, Daro leaves the paper on the table so Ena can choose whether to keep it. If the player chose no notice, Jori asks whether the group can still talk about the signal without announcing it.

Ena may read her pocket note aloud, keep it private, or tell the group she wants to write a new first line. The player cannot make the choice for her. A final silence follows. It is ordinary room tone described in prose—kettle, breath, chair leg—not radio static presented as an implied response.

The act closes with Daro returning the map to its sleeve and Mira lowering the cloth. The world-map rumor remains whatever the existing system recorded; the scene’s household objects do not represent changes to that state.

## 35. End-state principles

The expansion has no success state defined by reaching the bunker, identifying the operator, finding Ena’s brother, or hearing a message. Its resolution is whether the characters can speak with care while the available source remains incomplete.

The player may leave the scene thinking the signal could matter. The player may leave it thinking the group knows very little. Both readings are supported by the story. Neither is stated as the official answer to the identity or intention of the carrier.

The end-state is conveyed through conversation and a short journal reflection. It does not grant an item, unlock a location, increase a faction score, create a radio contact, change the signal’s strength, modify confidence, or mark the bunker as explored. Existing map discovery, direction-finding, and journal behavior remain authoritative.


## 36. Ena’s three possible evenings

Ena’s closing beat has three variants, selected by how she wants to carry the conversation into the rest of the night. They are not mutually exclusive character destinies. They are small responses to one difficult evening.

**She keeps the note folded.** Ena puts the old list back in her coat. She says she does not want to make it a document everyone can discuss. The player may say that is fair or ask if she wants company. Ena asks for company without conversation. The scene ends with the player and Ena sitting at the same table while Jori clears cups.

**She reads the note aloud.** Ena reads the small things, not as a performance but as if checking whether the words still belong to her. The line about the blue scarf catches. Mira asks if she wants that line removed from any written account. Ena says there will be no account of the list. The player can honor that boundary, and the journal may record only that Ena chose to share a private memory, without reproducing the memory itself.

**She writes a new first line.** Ena adds, “I do not know where you are.” She keeps the line beside the list and does not connect it to the signal. The player may ask whether the sentence feels too hard. Ena says it feels true and asks to leave it there.

All three outcomes preserve her agency. They do not change the catalog, map, radio, or the absent brother’s status.

## 37. Daro’s correction and repair

Daro begins the story believing that a precise statement protects people from bad inference. He learns that the same statement can sound like a door being closed when spoken to someone who is frightened. His arc is not that he becomes less careful; it is that he learns to state care and limits in the same breath.

If the player asks why he corrected Ena so quickly, Daro says he was afraid the room would turn a location candidate into an answer. He admits that he heard his own fear as accuracy. The player may ask him to apologize directly, say that the correction was needed, or leave the repair to Ena. He then speaks to her:
> “I wanted to stop the story from outrunning the record.”
>
> “You stopped me.”
>
> “I did. I am sorry.”
>
> “You were right about the record.”
>
> “That is not all I was doing.”
>
> “I know. That is why I am still here.”

If the player pushes him to promise that nobody will repeat the rumor, he says he cannot promise that. He can promise to keep his own words accurate. This distinction makes his accountability concrete rather than magical.

## 38. Mira’s public responsibility

Mira’s conflict is not whether information should be hidden. It is whether public language should carry a certainty it does not own. The map rumor already belongs to the map system; the notice adds interpretation and social context. Mira understands that not posting a card cannot make the rumor disappear from everyone’s interface or memory.

If the player asks her to erase the rumor, she says that is not what the card does. If asked whether she is afraid of people acting on it, she says yes: some may set out for a dangerous place because a sentence sounded like a promise. She is also afraid that over-cautious wording can sound as though the group wants to keep meaningful information to itself.

Her final wording can include a humane invitation:
> “If this matters to you, talk with someone who can explain what the entry does and does not show.”

That line is Mira’s proposal, not a new game service or a feature promise. It should only be used if the settlement already has a plausible existing social conversation route; otherwise it remains an authored line in the scene.

## 39. Jori as a keeper of ordinary sound

Jori’s memory about the pump is a brief story about how attention reshapes sound. It must not become a claim about hearing loss, illness, hallucination, or radio expertise. Jori describes an ordinary mechanical noise that momentarily felt like a syllable because they were waiting for a familiar person to return.

The scene can reveal why Jori learned to pause before repeating what a sound seemed to say. A neighbor once heard a name in a wind-bent sign and carried it into a room as certainty. Jori does not condemn the neighbor. They remember the hurt caused when everyone else built plans around the supposed call. This is an invented personal memory and should be clearly attributed to Jori in dialogue.

The player may ask if Jori thinks Ena is doing the same thing. Jori says no; there is no comparison that fits perfectly. They can recognize a shape without judging what Ena should feel. The scene leaves Jori’s memory useful but limited.

## 40. Character voice guide

Ena speaks from particular objects and repeated daily habits. She names a cup, a scarf, a laundry line, and the way a note folds. Her sentences can circle a feeling before naming it. She is allowed to be sharp when someone claims to know what her hope means.

Daro starts with exact nouns: entry, candidate, field, observation. As he relaxes, he uses direct first-person statements: “I do not know,” “I got scared,” “I should have asked.” His precision should not be written as robotic speech or infallible authority.

Mira prefers sentences that remain clear when taken out of the room. She is alert to how a phrase might travel. She can be funny about the work of rewriting the same sentence for the fourth time, but humor never makes Ena’s hope the punchline.

Jori notices timing and physical noise: a chair pulled back too quickly, a cup cooling, a voice that stops. Jori uses a metaphor once, then asks whether it landed. The writing should avoid turning them into a universal therapist.

## 41. Branch ledger: response to hope

The first major choice lets the player answer Ena’s hope in one of three registers.

**Stay with her.** The player says, “I hope you hear him one day. I do not know if this is him.” Ena says the two clauses can sit beside each other, though they do not make a comfortable pair. Daro nods. Mira does not write the hope on the notice.

**Ask for evidence.** The player says, “What about this entry makes you think it is him?” This can be delivered gently or with impatience. Ena says nothing in the entry points to him; she heard the label and wanted it to mean more. If gentle, she thanks the player for asking instead of deciding for her. If impatient, she says she already knows what the paper says.

**Reject the interpretation.** The player says, “It is not your brother.” Daro answers that the current entry cannot establish that either. Ena asks the player not to turn the lack of evidence into a different certainty. The player may apologize or double down. Doubling down ends the topic without a reward, but it does not lock the rest of the expansion.

No response sets a belief variable. If the existing dialogue framework requires an outcome flag, it should represent the authored branch only and be reviewed as transient narrative routing, not a new world fact.

## 42. Branch ledger: who sees the notice

After Mira writes the card, the player chooses where the sentence belongs.

**Put it on the public board.** Mira posts the chosen careful wording. A later ambient line may show that a reader understands it correctly or asks for clarification. No ambient line should convert it into a rescue call.

**Keep it at the listening table.** The note stays private to the group. Mira says privacy is not the same as concealment when the note adds no new map information. Daro is relieved; Ena asks that the card not be used as evidence of what she believes.

**Do not write it yet.** Mira leaves the card blank and says she wants the room to settle before the settlement repeats it. This defers the social note; it does not suppress or roll back the existing medium-confidence rumor. The story should not imply that the player deleted or hid the map record.

These choices change only which authored card appears in the scene. They do not alter the source event or any settlement-level information owner.

## 43. Branch ledger: how much detail to share

If the notice is posted, a further choice selects its detail.

**Name the source type.** “A direction-finding entry places a candidate at Broadcast Bunker Echo.” This is compact but may sound official. Mira adds that the entry is a rumor.

**Name what is unknown.** “No speaker or message is identified in this entry.” Daro approves the narrow attribution. Ena says she is grateful that nobody wrote “empty,” which would claim more than the record says.

**Name why the group cares.** “Some of us have reasons to hope this matters; those hopes are not evidence.” Mira asks the player if they want to expose Ena’s private feeling. The player can choose to keep the sentence general, sign it personally as their own hope, or remove it.

The most personal version is never posted in Ena’s name without her explicit agreement inside the authored scene. Her brother is not identified to the settlement by default. The narrative respects the difference between speaking honestly and disclosing someone else.

## 44. Optional scene: the reader who asks a second question

If the public card is posted, a neighbor returns later to ask what “candidate” means. They are not written as foolish. They are tired and want to know whether the ridge is a place they can go. The player may explain that the current rumor is not a physical survey, ask Mira to explain the note, or say the player does not know more than the card.

The neighbor says they have a person in mind who used to work near an antenna site. They stop before saying a name. Mira asks whether they want that thought kept private. The neighbor says yes. This moment parallels Ena’s privacy without suggesting the person is connected to the signal.

If the player probes for identifying details, the neighbor declines. The scene proceeds without storing an identity, adding a contact, or tying another survivor to Broadcast Bunker Echo. The player can offer the ordinary courtesy of not repeating the almost-name.

## 45. Optional scene: a notice copied badly

If a public notice is posted, one reader later repeats only the phrase “active radio emissions.” The repeat is not framed as a villainous distortion. People shorten information because they are busy, anxious, or trying to tell someone else what happened.

Daro catches the shortened version and begins a correction. The player may help him say, “The entry records emissions and a location candidate; it does not name a speaker.” The reader responds that they thought someone was asking for help. Mira says that it is understandable to hear a plea in a phrase built from public-safety language.

The correction is a small social repair. Nobody is shamed for hoping. The narrative shows why the original notice carried its limits in plain language and why a careful sentence still cannot control every retelling.

## 46. Optional scene: the card stays blank

If the player chooses not to write a notice, Mira may later bring the blank card back to the table. She asks whether leaving it blank was a decision or a delay. The player may say it was a decision, say they are still unsure, or ask Mira what she thinks.

Mira replies that a blank card can protect a person from being spoken for. It can also leave other people without context. She is not trying to overturn the player's choice; she is naming its cost. Daro adds that the map rumor remains what it was. The player can ask them to keep the table note private or authorize a neutral explanation with no mention of Ena.

This scene should only appear if it can be supported by the existing narrative delivery path without a new persistent campaign flag. If not, it remains an alternate closing scene in the content package rather than an additional stateful callback.

## 47. Short-form journal reflections

The current host-generated telemetry entry remains untouched. This plan proposes one additional authored reflection through the existing journal content route, if available. It follows the conversation rather than replacing or rewriting the telemetry fact.

Possible reflection, public card posted:

> We wrote down where the direction-finding rumor points and left the message line blank. The room had more to say than the entry.

Private note retained:

> Ena kept her folded list. I did not ask what each line meant. We let the map remain a map for one evening.

No notice posted:

> We waited before adding our own words. The existing rumor stayed on the map; our answer to it stayed at the table.

If the journal system cannot distinguish authored reflection from system-generated discovery without a new owner, the content should remain dialogue-only. No parallel journal store should be created.

## 48. Longer journal reflection: if the player names hope

The player’s reflection may acknowledge that the signal has become personal without converting the signal into a personal message:

> I found myself wanting the name Civil Defense to mean a familiar voice. Wanting it did not make the entry say that. Ena knows the difference. So do I, though I had to hear the sentence aloud before I could keep it.

An alternate line if the player was guarded:

> We kept three things apart: the carrier name, the candidate on the map, and the message nobody in that room had. The separation did not comfort everyone. It kept our words from pretending to know more.

These are reflections authored in the player’s voice and should be checked against existing journal tone. They do not require new radio text or a simulated transcript.

## 49. Dialogue variations for the first question

When the player asks, “What exactly did the direction-finding entry tell us?”, Daro can answer:
> “That the telemetry confirmed active emissions and associated them with a candidate location.”
>
> “Can we say the bunker is transmitting?”
>
> “We can say the entry maps emissions there. The entry does not tell us who, what equipment, or why.”

If the player asks, “Does it mean there is somebody there?”, Mira answers:
> “It gives us a place to discuss. It does not give us a person.”
>
> “That sounds like no.”
>
> “It is not no. It is not yes. It is the edge of what this line can carry.”

If the player asks, “Could the signal still be a call?”, Ena answers:
> “It could. I do not know.”
>
> “Do you want it to be?”
>
> “Yes.”
>
> “And if it is not?”
>
> “Then wanting did not hurt anyone here. What we tell the others could.”

The dialogue treats uncertainty as a living condition rather than a locked terminal waiting for the correct prompt.

## 50. Dialogue variations for the public notice

If the player asks for a plain-language version, Mira says:
> “A map rumor points toward Broadcast Bunker Echo. This entry does not give us a voice or a message.”
>
> “People will still hear a voice in it.”
>
> “Then we have to make sure we do not pretend it came from the paper.”

If the player asks whether naming the source makes the notice too technical, Daro offers:
> “A direction-finding report points to a candidate near Echo Bunker. We have no confirmed speaker or message.”
>
> “That says the same thing?”
>
> “It carries the same limit. It carries a little less machinery.”

If the player asks to remove all caution, Mira asks:
> “Which fact do you want me to add?”
>
> “That somebody might need help.”
>
> “That is a reason to care. It is not a fact about the signal.”

This is a useful sentence for the expansion’s player-facing moral distinction: care can motivate attention without becoming evidence.


## 51. Dialogue variations when the player pushes for certainty

Some players will ask the same question more than once, hoping that repetition uncovers an answer hidden behind caution. The responses should remain patient but not repeat the same sentence mechanically.

**First ask: “Is anyone alive in that bunker?”**
Daro: “The location description and direction-finding entry do not answer that.”
Mira: “I know why you want the answer to be yes.”
Daro: “Wanting it does not make the site safe or occupied.”

**Second ask: “But do you think so?”**
Daro: “I think the entry supports a candidate location. I do not have a responsible guess about a person.”
Ena: “I can have a hope. I do not need you to call it your guess.”

**Third ask: “Then what good is the signal?”**
Mira: “It gives us something to notice. It does not give us a reason to invent the rest.”
Jori: “A light in a window can matter even before you know who lit it.”
Daro: “And this is not a light in a window. It is an entry about emissions.”
Jori: “Right. I was talking about why people look.”

The variation recognizes the player’s frustration and moves the characters rather than escalating into a hidden answer. The final exchange explicitly marks Jori’s image as metaphor so it cannot be mistaken for a description of the carrier.

## 52. Dialogue variations when the player asks whether to go

A player may ask whether the group should travel to Broadcast Bunker Echo. This is a natural response to a location rumor, but the content must not manufacture a destination route.

Ena asks what the player thinks would happen if they went. The player can say “we could learn more,” “we might find a person,” or “we would be following a rumor.” Daro points out that these are different levels of certainty. Mira says a dangerous place does not become an obligation just because the map gives it a name.

A possible exchange:
> “We could go and see.”
>
> “Can we?”
>
> “I do not know.”
>
> “Then we are not deciding to go. We are deciding that we want to know.”
>
> “Does that count for anything?”
>
> “It counts as wanting to know.”

If the current build later gains a legitimate route to this site, its travel and danger semantics must be authored and reviewed through the current expedition and map owners. This narrative plan does not pre-approve that route. Until then, the scene closes on the intention and leaves the map candidate in its current state.

## 53. Dialogue variations when the player asks whether the site is safe

The location record marks Echo as high danger. The narrative can let a character notice that word if it is already visible through an existing panel. The plan should not invent a precise danger score in dialogue or convert the rating into an explanation of hazards.

Mira says:
> “The map calls it dangerous.”
>
> “Do we know why?”
>
> “We know the rating. We do not have a site report from this conversation.”
>
> “Would you go?”
>
> “Not because I wanted the answer badly.”

Daro may add that the current entry does not establish access, power, occupancy, or conditions inside. That is an evidence boundary, not an operational briefing. If the interface does not expose danger in the player’s current context, the first line should be omitted rather than leaking hidden catalog data.

The story does not provide equipment recommendations, a route, or an expedition preparation checklist. Its tension is not that the player must solve a tactical approach. It is that a place can feel close once it has a dot on the map, even when the story has not given permission to claim knowledge about it.

## 54. The public notice after a day

If posted, the notice remains on the board for a day in narrative time only if the existing community-board route supports such a visible update. Otherwise it appears in one later conversation, without a new simulation rule.

The card’s corner curls. Someone has copied the first sentence in charcoal on a scrap and omitted the second. Mira turns the scrap face down. The player can help her rewrite the limit, ask whose hand copied it, or leave the card alone until the next ordinary board refresh.

Mira says that the copied sentence is not malicious. It may have been copied by someone trying to remember the location for later. The player can ask whether to add a second line in larger letters. Daro suggests repeating the word “candidate.” Ena asks whether that will make the notice sound like it is arguing with its readers.

The scene offers a final version:
> “Map rumor: candidate at Broadcast Bunker Echo. This entry contains no speaker or message.”

The phrase “this entry” matters. The notice describes the source that occasioned the discussion, not every possible source in the world.

## 55. A quiet variation for a player who has not read the map

If the player has not opened the triangulation panel or map rumor, the dialogue must not assume they know the location was revealed. The existing narrative event may still have written the host journal line, but the scene should introduce it in terms available in the journal.

Daro says, “Your journal has a direction-finding note about Broadcast Bunker Echo.” If the player asks where the name came from, he can explain that the entry records telemetry and a candidate. He does not reveal the underlying catalog confidence value or internal UI detail.

The player can ask to see the map. If the map screen is an existing navigation route, the game may open it through the current host seam. If no such route exists, the line remains conversational and the player can continue without viewing it. This plan does not create a map-opening action to serve its scene.

The narrative still proceeds because the human conflict is not gated on the player reading every field. The characters can talk about how they interpret a short journal line without exposing hidden information.

## 56. A quiet variation for a player who has already left the map open

If the map is already open when the player starts the shelter conversation, the scene may refer to “the mark” only if the current map representation visibly displays that exact rumor. Daro can say “the candidate on the map”; he cannot call the mark a confirmed site or an unlocked destination.

Ena may stand beside the table and keep a respectful distance from the panel. Mira asks the player to close the map while they talk, not because it changes state but because the conversation is about what the people know rather than how the system draws it.

If the interface cannot keep the dialogue and map visible without a new overlay, do not invent one. The authored scene can treat the panel as closed and refer to the current entry by name. Presentation convenience must not blur the distinction between opening a map and surveying a place.

## 57. Failure and interruption beats

The scene can be interrupted by ordinary settlement activity: a request to move a bench, a child calling from another room, a kettle boiling over. These beats keep the story embedded in daily life and prevent it from feeling like a technical hearing.

If the player leaves mid-conversation, the next conversation should resume at a natural topic rather than replaying every line. The current narrative framework’s existing delivery and completion behavior governs this; the plan does not demand a new resumable scene machine. If a safe resume point is unavailable, the scene can restart from a concise recap written as dialogue.

A possible interruption:
> “The kettle.”
>
> “I had it.”
>
> “It is making the sound.”
>
> “I hear it.”
>
> “You hear the carrier too?”
>
> “No. I hear the kettle. I am answering the kettle.”

Ena laughs once, then immediately apologizes for laughing. Jori says the kettle has finally earned its place at the table. The humor releases the room without making the story’s uncertainty a joke.

## 58. A conversation about old announcements

Mira asks why public announcements once used the phrase “Civil Defense” so often. Each character remembers something different. Daro remembers a heading on a page. Jori remembers adults lowering their voices. Ena remembers a neighbor counting people. The player may contribute a personal memory using a general choice such as “I remember the instructions,” “I remember the fear,” or “I remember none of it.”

These choices are authored player-character recollections only if the game’s existing dialogue voice supports them. They should not add a canon fact about the source catalog. The player may also say nothing.

Ena says:
> “Maybe the words survived because people needed them to mean someone would come.”
>
> “Did they?”
>
> “Sometimes.”
>
> “And sometimes?”
>
> “Sometimes someone just said them.”

The line is not a global historical verdict. It is Ena’s remembered experience, framed with the uncertainty of a person who was young and heard the phrase from others.

## 59. What a character may remember without becoming a source

This plan proposes personal memories as dialogue, never as forensic evidence. A memory can establish what the speaker felt, saw, or heard in their own life to the degree that the speaker says so. It cannot establish the origin of the carrier, the condition of the bunker, or the identity of an absent person.

For example, Ena may say she remembers waiting beside a school door while adults read names. She cannot say the current signal came from that school. Daro may remember a printed civil-defense heading. He cannot infer the catalog’s technical origin from that heading. Mira may remember a neighbor using the words to announce a ration schedule. She cannot identify the station behind the present fingerprint. Jori may remember a pump making a sound like a syllable. They cannot diagnose the receiver’s static.

The player can ask each character whether they are sure. The answer should clarify the scope of the memory rather than undermine the character: “I am sure I remember the words that way. I do not know whether my memory explains this.”

## 60. The cost of an empty column

The empty message column becomes a focal image in the second act. Ena asks Daro to stop leaving it in the middle of the page where everyone can see it. Daro asks whether she wants it crossed out. She says no; she wants it moved to the side.

The player may tell Daro to leave the column where it is, move it, or remove the label “message.” Each option has a small emotional consequence:
- Leaving it visible helps Daro preserve the distinction, but Ena says it makes absence look like a task.
- Moving it aside lets the conversation breathe, but Daro asks if the group is hiding uncertainty.
- Removing the label prevents the paper from resembling a form, but Mira notes the notice still needs a plain sentence about what was not recorded.

The final layout is not a data schema or UI mockup. It is staging for the scene. The script must not imply that the player can enter text into the signal record.


## 61. Sample scene script: the first table conversation

**Scene begins.** The map is folded shut. Daro has copied the telemetry line onto a scrap of paper. Mira holds an unmarked notice card. Ena stands rather than sitting; she has brought her coat inside but has not taken it off.

Jori: “Are we reading it?”
Daro: “It is one sentence.”
Jori: “That was not my question.”
Mira: “We can read it or let it sit.”
Ena: “Read it.”
Daro: “Direction-finding telemetry confirmed active radio emissions at loc_broadcast_bunker_echo.”
Ena: “That sounds like someone.”
Daro: “It says emissions.”
Ena: “I heard the sentence.”
Daro: “I know.”
Ena: “Then do not answer the sentence I did not ask.”
Daro: “You asked whether it sounds like someone.”
Ena: “I said it sounds like someone. I did not ask the paper to agree.”
Mira: “Do you want me to put the card away?”
Ena: “No. I want it to stop looking like a form.”
Jori: “I can turn it over.”
Daro: “Then we lose the line.”
Ena: “The line is in the journal.”
Daro: “Right.”
Player choices:
1. “We know where the entry points. We do not know who is there.”
2. “You can hope it is a voice without calling that hope evidence.”
3. “Maybe we should stop looking at it for tonight.”
4. “What does the name mean to you, Ena?”
5. Say nothing.
Each choice branches for two or three lines, then leads to Mira asking whether they want a public note. If the player remains silent, Jori turns the card over and Ena asks the player whether they want tea. The quiet option remains a real choice and does not count as failing the scene.

## 62. Sample scene script: the notice draft

Mira lifts the cloth from the notice board and lays the blank card on the table instead of pinning it up. The physical act signals that this is still a draft.

Mira: “First version: direction-finding telemetry records active emissions associated with a candidate at Broadcast Bunker Echo.”
Daro: “Keep ‘candidate.’”
Mira: “I did.”
Daro: “I wanted to hear it.”
Ena: “Can the line say that the message field is blank?”
Daro: “There is no message field on the map.”
Ena: “Then do not make me a field.”
Daro: “I did not mean—”
Ena: “I know what you meant.”
Mira: “We can write ‘this entry gives us no speaker or message.’”
Jori: “It sounds like the beginning of another form.”
Mira: “It sounds like the end of a rumor.”
Player choices:
1. “Use Mira’s sentence.”
2. “Make it shorter: a rumor points toward Echo.”
3. “Keep this note at the table.”
4. “Do not post anything.”
5. “Add that someone might need help.”
If choice five, Mira replies: “We can write that we hope there is someone to help. We cannot write that the signal asks us to.” The player may choose to sign that hope personally, generalize it, or leave it off.

The script ends with the card still on the table. Posting it, if selected, happens after the player confirms the exact wording. No auto-post follows a dialogue option whose referent is unclear.

## 63. Sample scene script: Ena’s private list

This scene is optional and explicitly consent-gated in dialogue. It is not available as a rummage interaction.

Ena: “There is a note in my coat.”
Player: “Do you want to show it to me?”
Ena: “I might. I do not know if I want to read it.”
Player:
1. “We can leave it there.”
2. “I can listen if you decide to.”
3. “It does not have to be about the signal.”
Ena: “It has nothing to do with the signal.”
If the player chose one or two, Ena may open the fold but keep the writing turned toward herself. The player sees no readable text unless the current content presentation supports a deliberate shared reading. The scene can still convey the lines through Ena’s spoken version if she gives permission.

Ena: “I wrote down what I would say if I had time. The kettle. The scarf. The jam.”
Player: “The things you missed?”
Ena: “The things I could remember without making the list longer.”
Player: “Do you want me to keep it in the journal?”
Ena: “No.”
Player: “Then it stays with you.”
Ena: “Thank you.”

The journal does not quote the private note. If the player declined to read it, the scene does not appear later by default. Respect for the choice is expressed through absence, not a follow-up that asks again.

## 64. Sample scene script: an interruption

When the kettle begins to whistle, Jori stands. Ena reaches for the handle before Jori can.

Jori: “I was getting it.”
Ena: “You were getting the noise.”
Jori: “That is part of getting it.”
Mira: “Would anyone like tea?”
Daro: “I can take the card.”
Ena: “You can take your pencil. Leave the card.”
Daro: “Okay.”
Player choice: help with the kettle, take a cup, or stay at the table.
If the player helps, Ena points to the cloth and asks them not to touch it because it is wet. If the player takes a cup, Jori offers the cracked one. If the player stays, Daro asks whether they want the journal scrap back.
The scene moves the characters through simple actions. None of the action choices adjusts trust, skill, resources, needs, or hidden relationship state. The domestic texture is there to keep the story embodied without inventing a new simulation mechanic.

## 65. Sample scene script: the copied sentence

A neighbor holds up the copied scrap:
> Active radio emissions at Broadcast Bunker Echo.

Neighbor: “I thought it meant someone was calling.”
Mira: “I can see why.”
Daro: “The entry says the emissions were mapped to a candidate. It does not name a speaker.”
Neighbor: “So there may not be anyone.”
Daro: “It does not settle that either.”
Neighbor: “You can say what it does not know. Can you tell me what to do?”
Player choices:
1. “Do not go because of one rumor.”
2. “Talk with us before deciding.”
3. “The choice is yours, but understand the entry.”
4. “We do not know what is there.”
The neighbor says that they wanted a direction, not a guarantee. Mira admits that the map is a direction in one sense, but not a safe route or an invitation. The player can let the neighbor keep the copied scrap with the full clarification written beneath it, or ask them to leave it at the table.

This scene should not imply that the neighbor’s decision controls another expedition, faction, or settlement event. It is a human conversation about how information travels.

## 66. Sample scene script: the final room

This is the last authored conversation of the expansion. It should be short enough to leave the player in the room rather than explaining the whole theme again.

Ena holds the folded note without opening it. Mira has pinned the notice, kept it at the table, or left it unwritten according to the player’s prior choice. Daro closes the journal. Jori sets the cups together.

Ena: “I still want it to be him.”
Player: “I know.”
Ena: “You do not know.”
Player: “I know you told me.”
Ena: “That is not the same as knowing me.”
Player:
1. “You are right.”
2. “I want to be careful with what you told me.”
3. “Then tell me what you want me to remember.”
Ena: “Remember that I wanted to hear him. Do not remember that I thought I did.”
Mira: “I can keep that part off the board.”
Daro: “It will not go in the report.”
Jori: “It can stay here.”
Ena: “For tonight.”

If the player did not receive the private-list scene, the line about remembering changes to: “Remember that I can hope without making a report out of it.” The exchange ends with an ordinary sound from the room and no signal cue.

## 67. Endings by notice choice

The narrative ending is keyed to the notice choice, not to signal confidence or an imagined outcome.

**Public note.** The closing journal reflection emphasizes that the sentence is visible and limited. Mira says she will correct her own wording if someone asks. Daro agrees to answer questions with the same distinction. Ena does not endorse the notice as her story.

**Private note.** The closing reflection emphasizes that the group talked without publishing Ena’s wish. Mira folds the card and hands it to Daro, who places it with the journal scrap. The story does not imply that private is the only ethical choice.

**No note.** The closing reflection emphasizes that the player left the group’s interpretation unwritten. The map rumor and telemetry entry remain present in their current authorities. Mira says waiting was also a choice, and the player may agree or say they are not sure.

Each ending receives one distinct final image:
- The public card flattens under a wooden pin.
- The private card rests inside Daro’s notebook beside, not on top of, the system entry.
- The blank card is set under the cloth, where it can be found again but does not claim a conclusion.

## 68. Endings by player response to Ena

A second axis provides a short line of variation at the close, without creating independent endings or permanent belief variables.

If the player supported hope carefully, Ena says:
> “Thank you for not making me prove it.”

If the player asked for evidence gently, she says:
> “I know what the line does not say. I needed you to ask like I was still here.”

If the player was blunt and then repaired, she says:
> “You sounded certain when you did not know. I heard you take it back.”

If the player was blunt and did not repair, she says:
> “I am done talking about him tonight.”

The last line closes the scene and respects her boundary. It does not block the player from later ordinary conversation with Ena or create an ongoing hostility state. The game should not turn one difficult dialogue into a durable personality punishment.

## 69. Optional branch: the player offers their own memory

The player may say that a sound once reminded them of someone they missed. This option should appear only if the game’s existing protagonist voice allows an authored personal recollection. It is not an open-ended text box and must not require the player to disclose real personal details.

The memory is intentionally broad:
> “I once heard a familiar name in a machine.”
>
> “Did it sound like them?”
>
> “For a moment.”
>
> “What happened after?”
>
> “The machine stopped. The person was still gone.”

Ena can answer that this helps, or that it makes the moment too close. The player may apologize and let the topic go. This branch can create mutual recognition without implying the protagonists share the same history or world event.

If no stable protagonist voice exists, omit this branch rather than inventing a canonical past for the player character.

## 70. Optional branch: a child asks what the signal said

A younger settlement member asks whether the radio said anything. The question is not a tutorial prompt and does not introduce a new NPC system. It lets the adult characters demonstrate how to be clear without transferring adult anxiety to a child.

Mira answers:
> “The map showed us a place connected to a radio signal. It did not show us a spoken message.”
>
> “Could somebody be there?”
>
> “We do not know.”
>
> “Can we find out?”
>
> “Not from this line alone.”

The player can ask Mira to simplify the answer or let the child return to play. Ena might add that wanting someone to be safe is a kind wish, even when they do not know where the person is. Daro should not list technical metadata to the child. The exchange keeps the same evidence boundary in age-appropriate language.

## 71. Narrative pacing map

The expansion should unfold over one long evening or two short visits, depending on the current narrative framework.

- The first journal scene introduces the event and gets the group into the room.
- The main table conversation puts hope beside the signal record.
- The notice draft turns private interpretation into a public-language decision.
- The optional private-list beat gives Ena control over what the player witnesses.
- A later copied-notice scene, if supported, shows the cost of compression.
- The final room scene ends without an answer from the bunker.

The story must not demand that every optional beat fire for the main arc to make sense. The critical facts are repeated naturally: a carrier label is present; the map has a candidate rumor; no speaker or message is established by this entry. Repetition should occur through character reactions and choices, not recurring explanatory paragraphs.

If the narrative delivery system cannot support all callbacks without new state, prefer the first table conversation, notice choice, and one ending. The remaining scenes are optional writing variants, not a mandate for a broader quest framework.

## 72. Emotional rhythm

The story opens on a clear system-generated sentence. Its clarity feels almost merciless because it names a place and leaves out the person Ena wants. The next movement lets her ask for the sentence to be heard as a sentence, not as a diagnosis of her hope.

The middle introduces the notice card. A blank field can feel like an accusation; a public sentence can become a promise. The player navigates between those pressures while the characters disagree about tone, audience, and care.

The final movement returns from public language to a private room. The group does not solve the carrier. They decide what words they can stand behind and what personal memory should remain in the hands of its owner. The ending is a modest form of care, not a revelation.

## 73. Sensory prose limits

The writing may describe the warm rim of a cup, the damp edge of Mira’s cloth, the faint smell of hot metal from a kettle, the scratch of Daro’s pencil, or the folded map’s worn crease. These details belong to the shelter room and character action.

The story should not describe a new audible sequence from Broadcast Bunker Echo. The location’s static exists in the authored location description, but the player does not hear it in this expansion. No new waveform, call sign, voice fragment, coded pulse, carrier modulation, or hidden word is supplied.

The distinction between hearing static in a location description and hearing a signal in a scene is important. Prose should not accidentally turn an environmental description into an encounter. If art or audio later portrays the bunker, it must be reviewed against the same source limit.


## 74. What “active emissions” means in dialogue

Characters may repeat the host-generated wording only as attribution to the entry. They should not paraphrase it into “the bunker is on,” “someone is broadcasting,” “the receiver is answering,” or “the signal is live” unless another verified source is added through a separately reviewed canon change.

Daro can explain the difference in simple terms:
> “The line tells us the telemetry registered emissions and associated them with a place.”
>
> “That still sounds like a transmitter.”
>
> “It is a signal-related reading. It is not a person-shaped reading.”
>
> “You keep trying to make it sound small.”
>
> “I am trying to make it the size it is.”

The final phrase is Daro’s metaphor. It should not be read as technical scale or signal strength. The writing may use image to clarify a character’s intent, but the content notes should keep the source claim precise.

## 75. What “identity confidence” does not mean

The catalog’s 0.85 identity-confidence value is an internal classification property associated with the fingerprint, not a player-facing probability of a person, voice, bunker occupant, or successful interpretation. The current panel does not display this value. This plan never proposes dialogue in which a character says “eighty-five percent,” and it never turns the number into a die roll or branch threshold.

If internal design notes need to mention the value, they must state its source and explain that it is distinct from the calculated location confidence presented by the current triangulation flow. The authored scenes should not mention the numeric value at all. It carries no dramatic value for these characters and would invite a false statistical reading.

A content reviewer should search the final story package for “85,” “eighty-five,” “percent,” “probability,” and “chance” to make sure none are used to describe the likelihood of a speaker being at Echo.

## 76. What “medium-confidence rumor” does not mean

The host uses the existing map owner to record a medium-confidence rumor when the current mapped-location reveal event fires. The story may call it “the rumor” or “the candidate on the map” in conversation. It does not translate the enum into a verbal percentage or a character’s personal confidence.

Mira can say:
> “The map is willing to show a rumor.”
>
> “Does that mean it is probably right?”
>
> “It means the map has recorded it with that status. It is not a witness.”

The line is suitable only if the player already understands the map’s rumor language. Otherwise the shorter exchange is:
> “The map points there.”
>
> “Does that prove it?”
>
> “No. It records where this information points.”

The wording should reflect the existing UI exactly. If the map changes its confidence label or reveal presentation before implementation, revise the authored text against the live owner rather than preserving this plan’s phrasing.

## 77. No hierarchy of evidence is hidden in the prose

The story does not set up one character as the truth-teller and another as a deluded believer. Daro can be correct about what the data establishes while still missing what his tone does to Ena. Ena can make a wish without asserting it as evidence. Mira can worry about public repetition without claiming that posting a notice controls all rumor. Jori can offer an analogy that does not explain the carrier.

The player’s role is not to award an epistemic victory. It is to choose how to speak when every character has a partial view:
- Daro sees the fields and fears overclaim.
- Ena hears a personal possibility in a public name.
- Mira sees the sentence moving beyond the room.
- Jori sees how familiar sounds can collect a listener’s longing.
- The player sees the same thin entry through their own expectations.

The story rewards attention by making later dialogue responsive, not by displaying a hidden alignment score.

## 78. Fail-forward behavior

No dialogue choice can permanently prevent the player from hearing the central distinctions. If the player chooses a blunt line, another character may restate the limit in their own words. If the player chooses silence, the scene can still conclude with the notice choice or a simple opt-out. If the player declines to post, the public-board callback is skipped.

The story does not require a successful persuasion check, high empathy stat, radio skill, faction rank, or special item. Those mechanics are not in the verified source path for this narrative. If a future implementation route offers such checks, it must be reviewed as new scope and should not make factual accuracy depend on character statistics.

A failed or interrupted conversation can end with:
> “We did not settle what it means.”
>
> “We settled what we are allowed to write.”
>
> “For tonight.”
>
> “For tonight.”

That is a complete emotional outcome. It does not imply the unknown has become a quest objective.

## 79. Player-character tone

The player-character voice should remain modest and responsive. The player can ask, reflect, apologize, set a boundary, or acknowledge hope. The player should not deliver a monologue that explains the theme more clearly than the characters have lived it.

Possible short lines:
- “I do not know.”
- “I want it to be a person too.”
- “That is not a fact I can give you.”
- “We can leave it unanswered for tonight.”
- “Tell me if you want me to keep this private.”
- “I spoke too quickly.”
- “I cannot promise what the entry means.”
- “I can promise not to say it means more than it does.”

The game should avoid making every choice a polished maxim. A plain “I’m sorry” or “I hear you” can be more human than a thesis statement.

## 80. Repeated-play variation

On replay, the scenes may use compact variations in room business rather than alternate truths. On one playthrough, Jori pours the tea. On another, Mira closes the cloth over the board first. A different first line can let Daro ask whether the player read the journal entry. These changes preserve pacing without implying different transmissions or operator states.

Variation should not be randomized using a new deterministic stream or gameplay seed. If the current narrative system already selects dialogue variants deterministically, use its existing contract. Otherwise choose a single stable variant. No variation should use wall-clock time, randomize whether the signal has a voice, or produce different hidden facts.

Repeated lines should be avoided. If the player reopens the same conversation, a brief line can summarize the prior choice: “The card is still on the board,” “We left it at the table,” or “We chose not to write one.” If no persistent record exists for that choice, do not pretend the game remembers it; use a neutral greeting.

## 81. Quest-log wording

If the existing quest or journal view uses objective-style headings, this expansion should not create a directive such as “Investigate Broadcast Bunker Echo.” That wording would imply travel authorization and a concrete investigative route that the current data does not provide.

A safe optional heading for the authored conversation, if the current journal owner supports descriptive entries, is:
> The Name on the Map

Potential summary:
> We discussed a direction-finding rumor associated with Broadcast Bunker Echo. The current entry records a candidate location and active emissions, but does not identify a speaker or message.

The entry should not appear as a required objective. If the current journal cannot store a non-objective reflection, use conversation content only. No duplicate quest ledger or parallel completion tracking should be introduced.

## 82. Map and journal wording alignment

The map continues to use its existing medium-confidence rumor presentation. The host-generated journal line about active emissions remains unchanged. This expansion may add a character-authored follow-up but should not silently rewrite the map label or telemetry statement.

The two surfaces can use different lengths while preserving source attribution:
- Map: existing candidate and rumor wording.
- Host journal: existing active-emissions statement.
- Character note: a discussion of what the current entry does not identify.
- Notice card: a direction-finding rumor points toward Echo; no speaker or message is confirmed by this entry.

A reviewer should be able to trace each line back to the appropriate owner. If a line cannot be attributed to an existing source or a clearly named character’s opinion, it should be revised or removed.

## 83. Content packaging proposal

The authored narrative can be packaged as one short expansion record or a small set of dialogue entries under the project’s existing narrative data authority, depending on current schema and routing. The content author should not create a new top-level data category for this story.

Potential authored content units:
- Opening table scene.
- Three or four variants for the initial response.
- Notice draft and final wording variants.
- Optional private-note conversation.
- Optional public-card callback.
- Closing scene variants.
- A short journal reflection if supported.

All IDs, references, lengths, line constraints, and flag use must be checked against the current schema and catalog validators before implementation. The plan does not invent exact JSON keys or presumed save semantics. Existing data ownership and content delivery rules remain authoritative.

## 84. Proposed narrative dependencies

The minimum dependency is the existing location reveal and active-emissions journal event for loc_broadcast_bunker_echo. The story may also use the existing map presentation and existing shelter dialogue route if their current consumers support this content.

The optional callbacks depend on whether the narrative owner can represent:
- a public card choice,
- a private-only card choice,
- a no-card choice,
- the optional consent to share Ena’s note,
- a later copied-card conversation.

Before implementation, each dependency must be verified in the current narrative data and host. If a dependency does not exist, reduce the scene to a one-session conversation with no new save state. This prose plan does not authorize building a message-board simulation or journal mutation API.

## 85. Explicitly out of scope

This expansion does not add:
- a radio receiver, transceiver, signal source, frequency, station, or message decoder;
- a new signal catalog record or change to fp_civil_defense_carrier;
- a new location, map reveal, route, travel destination, expedition, or bunker interaction;
- a physically visited or surveyed state for Echo;
- a transmitter operator, resident, rescue target, faction controller, or signal origin;
- a link to Ena’s brother, a civil-defense station, Hidden Relay Bunker, or another distress signal;
- a new confidence value, probability, triangulation threshold, or UI readout;
- a new relationship meter, consent flag framework, settlement-board authority, save section, or rumor store;
- a new audio cue, signal playback, radio subtitle, waveform, encoded language, or environmental audio event;
- a reward, item, loot cache, combat encounter, danger-reduction benefit, or faction reputation change.

These boundaries preserve the scale of the story: one existing map rumor becomes a conversation among people who have to decide what to say about it.


## 86. Continuity table for current evidence

| Story statement | Local support | Safe narrative use |
|---|---|---|
| A fingerprint is named Civil Defense Carrier | Current direction-finding catalog entry | Daro may read the display name and explain that it is a catalog label |
| The fingerprint maps to Broadcast Bunker Echo | Current catalog mapping | The story may discuss a candidate associated with that place |
| The entry has identity confidence 0.85 | Internal catalog field | Keep in author notes only; do not turn it into speaker probability |
| The map reveal uses a calculated location confidence | Existing Core triangulation behavior | Refer to the candidate or rumor, not a percentage invented for the story |
| The host writes an active-emissions journal line | Existing Main.Narrative event subscription | Use the current line as the scene catalyst; do not rewrite its claim |
| The host records a medium-confidence map rumor | Existing host/map behavior | Explain that the map recorded a rumor; do not call it a surveyed site |
| Echo has emergency transceivers and static in its description | Current location description | May be quoted in an author note; do not stage new audio or prove a live voice |
| The map node is discoverable, not start-unlocked; danger is high | Current world-map data | Do not imply starting access, safety, travel, or exploration |
| There is no current expedition row for Echo | Current expeditions data | No route, travel cost, or expedition objective in this plan |
| Hidden Relay Bunker is another site | Existing map and prior expansion source | Explicitly keep the sites separate |

This table is a review aid, not a new authority. If source records change, the story must be reconciled against current data before implementation.

## 87. A short exchange on the word “confirmed”

The player asks whether the location is confirmed.

Daro: “The telemetry confirmed an observation. The map has a candidate.”
Player: “That is a careful answer.”
Daro: “It is a frustrating one.”
Mira: “Which part do you want me to put on the board?”
Player:
1. “The observation.”
2. “The candidate.”
3. “That we do not know what it means.”

If the player chooses observation, Daro reads the existing host wording exactly and Mira notes that the map is not a physical report. If the player chooses candidate, Mira adds “rumor” to the sentence. If the player chooses the uncertainty, Ena asks whether that makes their hope sound silly. The player may say that uncertainty is not a judgment.

This branch teaches the distinction through the word the player wants to repeat, not through a technical tutorial or diagnostic popup.

## 88. A longer scene of the notice being posted

If the player posts the notice, the board is in a shared corridor where people pin supply lists, requests for help, and notes about repairs. The expansion does not create a new board UI. This is authored staging within an existing settlement space.

Mira aligns the card with the top edge of another notice. She reads the sentence once before pinning it. Her thumb holds the corner down while the wooden pin passes through the paper. Daro waits for her to finish before he looks at the line.

A passerby slows, reads it, then turns back.
> “Is this the bunker on the ridge?”
>
> “It is the candidate named by the direction-finding rumor,” Mira says.
>
> “Can I go there?”
>
> “The card does not give you a route.”
>
> “Does it say there is a person?”
>
> “It says what the entry says.”
>
> “That is a long way around.”
>
> “It is the way we know.”

The passerby thanks her, not because the answer is satisfying but because she did not make the paper sound like a promise. After the person leaves, Mira asks the player whether she should have been shorter. The player can say the wording was clear, say it was still hard to hear, or say the passerby wanted a different kind of answer. Mira says that the notice cannot become the answer someone wants without ceasing to be a notice.

## 89. A longer scene of the private card

If the player keeps the note at the table, Mira folds it once and asks who should keep it. Daro says that he wrote the technical line and can store it with the journal. Ena says she does not want her hope filed beside a telemetry record. Daro agrees that those are separate papers.

The player can give the card to Mira, ask Daro to keep only the source line, or leave the card blank and return it to Mira. Mira says:
> “I can keep a sentence without making it the settlement’s sentence.”
>
> “Does that help?”
>
> “It helps me know who I wrote it for.”

The card is never an inventory item. It is not collectible, loot, quest evidence, or a reusable message object. If the narrative interface needs to track the selected variant, use only the current conversation's existing route and lifecycle. The paper’s custody in prose must not be modeled as a new item ownership system.

## 90. A longer scene of no notice

If the player chooses no notice, the group has to decide whether silence feels like caution or evasion. Daro says he would rather not post an unfinished sentence. Mira says people may still discuss the rumor without their card. Ena says she does not want to become a reason the group withholds information.

The player can reassure her that the map already carries the current rumor, or say they do not know whether waiting was the right call. Mira answers:
> “We did not make the map quieter.”
>
> “We chose not to add our own sentence.”
>
> “That is a choice too.”
>
> “It does not feel like enough.”
>
> “It was never going to answer the thing you asked.”

This ending acknowledges the cost of restraint. It does not present non-publication as perfect, safe, or morally superior.

## 91. A separate exchange about safety

A player may use “confirmed” to mean “safe to approach.” Daro catches the shift and clarifies it. The scene should not use the opportunity to recite the location danger rating unless the player has already seen it on the map.

> “If it is confirmed, can we reach it?”
>
> “Those are two questions.”
>
> “One map mark.”
>
> “A mark does not tell us the route.”
>
> “What would?”
>
> “A route record and an assessment from whoever owns that work.”
>
> “And until then?”
>
> “Until then, we do not send someone because of a sentence they hoped meant more.”

Mira may disagree with Daro’s phrasing, saying that a careful answer should not sound like an order. The player can say the group can choose later if a route exists or tell Daro to keep the discussion hypothetical. This is a narrative boundary, not a tactical system.

## 92. A separate exchange about audio

If the player asks whether the static described at the location is the same signal, Daro says the current sources do not establish that. Mira asks what made the player ask. They may say the catalog name, the location description, or that they wanted the two pieces to join.

Daro can answer:
> “Wanting two records to belong together is not the same as finding a link.”
>
> “Could they still belong together?”
>
> “Yes.”
>
> “So we leave it open?”
>
> “We leave it open where it is open. We do not write the join for it.”

No sound plays during this dialogue. The characters can refer to the location’s description as text read from a catalog, but not as something they heard through the receiver. If the current game later adds a playable audio source, that will need separate canon and implementation review.

## 93. A separate exchange about the word “identity”

The player may ask why the catalog has an identity confidence if it cannot tell them who is speaking. Daro explains the term carefully:
> “It is identity for the fingerprint classification.”
>
> “Not a person?”
>
> “Not a person. It says how confidently the pattern belongs to that catalog entry.”
>
> “And where it is?”
>
> “That comes from the location calculation.”
>
> “And who sent it?”
>
> “Not answered by either of those.”

This explanatory exchange is for a player who has accessed technical context. It must not expose raw internal terminology if the game’s UI has not made it available. If the player has not seen a confidence field, Daro simply says, “The catalog identifies a kind of pattern. It does not name an individual.”

## 94. The order of labels on Daro’s page

Daro rewrites the three terms in a new order at the end of the argument: “pattern,” “place,” “speaker.” He leaves space beside the last one but does not draw a box.

Ena asks whether he is making another form. Daro says he is trying to remember what he cannot fill in. She tells him he does not need to leave a space for a person she has not named. He tears the page in half, keeps the side with the first two words, and gives her the blank side. She folds it into a small square and hands it back.

The exchange is not a ritual that changes the signal. It is a quiet decision to stop treating the absence as a job for Ena to complete. The torn paper is a scene prop and is not collected or consumed as an item.

## 95. The player’s relation to incomplete records

The player may have encountered many incomplete records in the broader game. This expansion should not recite a catalog of other content or assume a particular campaign path beyond the current Echo rumor. Instead, it lets the player recognize a familiar experience: information can be both useful and incomplete.

If the player says, “We keep finding scraps,” Daro can answer, “A scrap can still point somewhere.” Mira adds, “And still fail to tell us who it belonged to.” Ena says, “I do not want him to become a scrap before I know.” This line expresses her fear without declaring anything about the brother’s status.

If the player has no prior archival context, the dialogue still works as a single-story exchange. It does not require completion of Expansion 71 or 72, even though those companion plans share Wave 14’s theme of limited records.

## 96. Tone and restraint checklist for dialogue

Every line that mentions the signal should be reviewed for the person it assigns, the action it implies, and the certainty it claims.

Questions for editorial review:
- Does the line name a speaker when the source names only a carrier classification?
- Does “at Echo” sound like a physical survey instead of a map candidate?
- Does “active” imply a deliberate operator?
- Does “signal” imply readable language?
- Does “Civil Defense” imply a surviving organization?
- Does “the bunker” imply travel access or player arrival?
- Does a character’s memory become a source explanation?
- Does a wish become a quest clue?
- Does a blank field become an invitation to decode?
- Does a safety phrase imply the site’s current condition?

A line can keep emotional ambiguity while being clear about evidence. If the line cannot pass both reviews, rewrite it with attribution: “I hope,” “I remember,” “the entry says,” or “I do not know.”

## 97. Avoiding a tutorial disguised as empathy

Daro should not teach the player every detail of triangulation. The technical distinctions enter only when a character asks a question that makes them relevant. His explanation can be brief and interrupted. The scene’s primary action remains listening, writing, disagreeing, and deciding what to share.

A bad version would have Daro lecture about fingerprint fields, thresholds, observation counts, map confidence enums, and signal classes while Ena waits. The plan rejects that presentation. Internal mechanics belong in this document’s evidence note for authors; the player hears only the amount needed to keep the scene honest.

Mira often translates Daro’s abstraction into a social question: “What will they repeat?” Ena translates it into a personal question: “Can I still want it?” Jori helps the conversation pause. These transformations make the story playable as human content rather than documentation.

## 98. Avoiding an empathy test

The dialogue options should not imply that there is one compassionate answer and all others are wrong. A player can be gentle, quiet, skeptical, or clumsy. The story can react to the exact words without scoring the player.

If the player says “I know he is alive,” Ena may say that they cannot know that. If the player says “He is gone,” she may say they cannot know that either. The game can distinguish unsupported certainty from careful support without labeling the player cruel or unkind.

If the player apologizes, the conversation can repair. If they do not, Ena can end the topic. Neither path should rewrite her long-term disposition or create a hidden social punishment. The encounter is complete when Ena’s boundary is heard.


## 99. Avoiding a “mystery box” ending

The absence of a message is not a tease for an inevitable reveal. The ending should not add a final beep, a cutaway to a lit receiver, a narrator’s hint, or a character looking toward the ridge at the exact moment the player closes the journal. Any of those devices would imply a signal response the existing source does not provide.

The story can remain open because the characters remain open. Ena may keep hoping. Daro may continue to take notes. Mira may leave the card pinned. The map may still point toward the candidate. These facts let the player imagine future possibilities without promising that the game has secretly selected one.

A later expansion could introduce new evidence if a future author establishes it through the proper source and owner. This plan does not reserve, foreshadow, or pre-author that reveal. The right to add future canon is not the same as an obligation to plant a clue now.

## 100. Avoiding a false choice between science and feeling

Daro’s precision does not stand for all rational thought, and Ena’s longing does not stand for ignorance. Mira’s caution is not censorship. Jori’s analogy is not a replacement for evidence. The player should be able to understand every point of view without being asked to choose one person as the voice of the game.

At the end, Daro can still say “candidate.” Ena can still want the signal to be her brother. Mira can post a careful notice. Jori can sit with the room’s silence. The story’s answer is not that feeling should be suppressed until certainty appears. It is that feeling can be named as feeling and still deserve care.

This framing keeps the expansion from becoming a lecture about scientific literacy. It makes epistemic limits a source of interpersonal friction and tenderness, where the characters themselves must decide how to live with the limits.

## 101. Alternate opening if the player asks first

If the player approaches Daro before the journal event, the story should not preemptively reveal the current map rumor. Daro may be sorting old notes or checking a board. He says he has nothing new to report. Once the existing event fires, the authored conversation becomes eligible through the current route.

If the event has occurred but the player chooses a different shelter conversation first, the expansion can open with Mira asking whether they saw the journal line. The player may say yes, no, or only its name. The dialogue then gives a short recap without forcing a screen transition.

This variation should be implemented only if the current narrative routing already supports conditional dialogue against existing event state. If it does not, keep a single safe opening after the journal entry. Do not add a new observer, polling loop, or saved “heard about Echo” state to manage the scene.

## 102. Alternate opening for a player who distrusts the map

Some players may say that the map is often wrong or ask whether the rumor can be trusted. Mira does not defend the map as a person would. She says the map is showing the information with its current status. Daro says that status is part of the record and not a promise.

Ena responds:
> “I did not say I trust it.”
>
> “What did you say?”
>
> “That I hope it is pointing somewhere.”
>
> “That is different.”
>
> “It has to be.”

This branch validates skepticism and hope as compatible. It does not need to debate the broader reliability of map rumors or add a global trust statistic.

## 103. Alternate opening for a player who trusts the map

If the player says the map would not show the location unless it knew, Daro asks what the map label actually says. The player may remember “rumor,” “candidate,” or may not recall. Mira points out that the interface’s wording is there for a reason.

Daro avoids calling the player gullible. He says:
> “I trust the map to show what the map knows.”
>
> “That is not everything.”
>
> “No. It is what it knows.”

If the map’s present wording differs from “rumor,” this line must be rechecked at implementation. The point is not to assert a universal epistemology. It is to respect the current presentation without inflating it.

## 104. The distinction between a carrier label and a person’s name

A player may ask if “Civil Defense Carrier” was chosen by the old operator or generated by the current catalog. The inspected local record identifies it as the display name associated with the fingerprint. It does not answer who chose the phrase, when it was coined, or whether it reflects an original transmission.

Daro says:
> “I know the name attached to the fingerprint.”
>
> “Not who named it?”
>
> “Not from this record.”
>
> “Could it still be what the transmitter called itself?”
>
> “Could be. Could be ours. We do not have that history here.”

The phrase “ours” means the current catalog’s naming context as Daro imagines it, not a factual claim about who authored the source file. To avoid confusion, an alternate line can say “the name attached to the fingerprint” and stop there.

## 105. A line for the player who wants to help Ena

The player may ask what would help Ena. She names actions that do not require supernatural access to the signal:
- Sit with her while the room quiets.
- Let her keep the folded note private.
- Help her find the old kettle’s replacement lid.
- Ask before repeating anything about her brother.
- Talk about another subject if she is done.

These are conversational offers, not branching rewards or resource exchanges. If the player asks to search for her brother, Ena says she has no specific lead to follow from this entry. She may want to revisit the question someday, but this signal is not that lead.

A tender line:
> “You can stay.”
>
> “I can.”
>
> “You do not have to stay because of the radio.”
>
> “I know.”
>
> “Then stay because you are here.”

The scene turns attention toward present companionship without dismissing the missing person.

## 106. A line for the player who wants to leave

The player may tell the group they do not want to continue discussing the signal. The characters should let them leave. Mira says the notice can wait. Daro says he will not call them back to repeat the explanation. Ena may ask whether they are leaving because of what she said; the player can answer honestly or simply say they need a moment.

There should be no forced final dialogue or mandatory summary popup. If the current system requires a scene completion action, the farewell can be short:
> “I will put the paper away.”
>
> “Thank you.”
>
> “You do not have to thank me for stopping.”
>
> “I know. I wanted to.”

This branch ends the conversation in a way that still acknowledges the people in it.

## 107. A line for the player who wants to stay

If the player chooses to stay after the conversation is done, the scene should not hold control hostage. The player can choose a brief closing beat:
- Help Jori clear the table.
- Take the notice to Mira.
- Ask Daro to return the journal scrap.
- Sit quietly beside Ena.
- Say goodbye.

These are authored endings or dialogue options, not simulated task actions. They should not consume resources or change system state. If no interaction affordance exists, the closing can be rendered as a single cinematic or dialogue exit.

## 108. The map rumor after the conversation

No matter what the player says, the existing map rumor remains owned by the world-map system. The narrative should never imply that the public notice created it, deleted it, upgraded it, or made it more accurate. If a later scene references the map, it should read the current map state through existing presentation.

If the player chooses no notice, Mira can still say “the rumor is on the map.” If the rumor is no longer present due to a legitimate current-system change, the authored callback must update to match rather than restore it through dialogue. The plan does not hard-code a second persistent copy for convenience.

The same rule applies to the host journal entry. The characters may discuss its wording, but dialogue does not overwrite the system-generated record. Any authored reflection is a separate piece of authored writing and must remain visibly distinct if the journal UI supports that distinction.

## 109. Relationship after the story

The story leaves the characters available for ordinary later conversations. Ena can discuss daily life, but the game does not force the player to ask about her brother again. Daro can continue taking notes without turning every later exchange into a correction. Mira can moderate other community writing without repeating the same notice debate. Jori can be funny, quiet, or preoccupied with their own concerns.

If a later story revisits the missing brother, it must establish its own evidence and ask whether it is appropriate to refer to this conversation. The signal rumor alone cannot carry the later plot. No persistent relationship score or belief flag should be inferred from this expansion unless the current narrative owner already has a clear, approved way to record the authored branch.

The story's care should be visible in how characters remember boundaries, not in a new mechanical meter.

## 110. Reusable line variants

The following variants can help avoid a single repeated phrasing across choices while preserving the same evidence limit:

For “we do not know who”:
- “The entry gives us a pattern and a place, not a name.”
- “Nobody is named in this line.”
- “The map has a candidate, not a person.”
- “That is not enough to tell us who.”

For “we do not know what was said”:
- “There is no message in this readout.”
- “This line does not carry words from the bunker.”
- “No spoken content is attached here.”
- “The note reports emissions, not a sentence.”

For “we do not know if the site is safe”:
- “The rumor does not come with a route.”
- “A candidate on the map is not a safety report.”
- “We have not surveyed it in this conversation.”
- “I cannot turn that mark into a safe journey.”

For “hope is not proof”:
- “You can want it without writing it as fact.”
- “Your hope belongs to you; the report belongs to what it measured.”
- “The wish is real. The answer is unknown.”
- “We do not need to make your hope carry the map.”

Writers should use these as a palette, not stack them into every scene. Too many reminders will make the characters sound like they are reading a policy aloud.

## 111. Scene transition language

Transitions should be grounded in simple actions:
- Daro turns the scrap face down so the conversation can continue.
- Mira moves the blank card away from the folded map.
- Ena removes her coat but keeps the private note in its pocket.
- Jori takes the kettle from the edge of the table.
- The player sets the map sleeve back where it belongs.

These actions do not need to be implemented as inventory or world-state interactions. They are stage directions for dialogue and animation if the existing scene presentation supports them.

Avoid transitions such as the radio suddenly clicking, a light flashing at the ridge, or the journal adding a new line by itself. Those would imply a new signal event and pull the ending toward a reveal the plan does not establish.

## 112. If the existing journal line is revised

The current host event’s wording has been verified for this plan. If that line changes before implementation, preserve the story’s source boundary but update all direct quotations. Do not silently keep a stale quote that contradicts the current journal. The story may refer generally to “the direction-finding entry” if exact text is unstable.

If the host no longer records active radio emissions at this location, the narrative catalyst must be re-evaluated from scratch. This plan depends on that current event and should not be repurposed to imply the same observation through a different system without a premise review.

## 113. If the location description changes

The current location description mentions a jammed half-open blast door, high-gain receiver racks, condensation, and static. The main story does not rely on any of those details beyond the general fact that the location is authored as a relay bunker with emergency transceivers. It does not depict entry or use these elements as proof of an active human presence.

If the static or transceiver description is removed, the central story still works because it is anchored in the map and journal event. If a future source adds an operator, message, or access state, the narrative must be reviewed rather than extended by implication. The plan’s original unresolved ending is not a veto on future evidence; it is a statement of what this expansion can honestly say now.

## 114. If the site later gains an expedition

A future expedition record would be a separate current authority. It might make travel possible, but it would not retroactively prove that a voice caused the direction-finding result or that Ena’s brother is involved. A later version of this expansion could add a separate transition only after its premise is rechecked.

The present content may refer to the player wanting to go, but it must stop short of assigning stamina cost, encounter odds, path, loot, or a successful return. If an expedition is added, its data and outcome should live in the existing expedition and map owners. This expansion would not create an alternate travel definition through narrative text.


## 115. Content review questions for the final scene

Before this plan is approved for implementation, an editor should answer:

1. Can every factual sentence be traced to the current catalog, host event, or explicitly attributed character memory?
2. Does the story make it clear when a character is hoping, guessing, remembering, or quoting the journal?
3. Are any dialogue options misleadingly framed as ways to make the signal answer?
4. Does the ending avoid implying the player physically visited or surveyed Echo?
5. Does any line link Ena’s brother to the carrier, bunker, faction, or civil-defense station?
6. Does the notice reveal Ena’s private history without her consent?
7. Does any route assign the player a new expedition or encourage unsafe travel based solely on the rumor?
8. Has the numerical identity-confidence field been kept out of player-facing dialogue?
9. Does the prose preserve the map rumor as medium confidence without translating it into a fabricated probability?
10. Can every required beat run through an existing narrative and journal owner?

A “no” or “uncertain” answer is a revision task, not a reason to improvise a system. The goal is for the final content to be emotionally legible and source-accurate in the same pass.

## 116. Editorial pass: attribution and pronouns

The source phrase “active radio emissions” can tempt the prose into personified verbs. During review, mark every sentence where the signal “calls,” “asks,” “answers,” “waits,” or “reaches.” Most such verbs should be assigned to a character’s interpretation rather than the signal itself.

For example:
- “Ena feels as though the carrier is calling” is her feeling.
- “The carrier calls to Ena” implies intention and should be removed.
- “Mira worries that people will hear a call in the notice” names a social interpretation.
- “A call comes from the bunker” invents message content and should be removed.

The same attribution test applies to “found,” “confirmed,” and “identified.” The telemetry event may confirm an observation; the character may identify a label; the map may record a candidate; none of those verbs should quietly attach to a person.

## 117. Editorial pass: the absent brother

Every mention of Ena’s brother should remain a statement about her life or uncertainty. The writer should not use the brother as a suspense object in the signal plot. Do not add an age, last known location, organization, uniform, radio equipment, phrase, or reason for leaving unless a separate source already establishes it and the story is authorized to use it.

The strongest lines are the ones that preserve Ena’s ownership:
> “I miss him.”
>
> “Do you think he is alive?”
>
> “I do not know.”
>
> “Do you want to?”
>
> “I want the question to have somewhere to go.”

This does not imply the game will eventually answer the question. It gives Ena a way to speak about an unresolved absence now.

## 118. Editorial pass: no hidden antagonist

The story has no person deliberately feeding false information, no official cover-up, and no maliciously altered signal. The notice can be shortened by an ordinary reader, but the story does not turn that mistake into a sabotage plot. Daro can be brusque, but he is not hiding a truth. Mira can wait to post, but she is not suppressing evidence.

If a future quest introduces an antagonist or intentional signal manipulation, it should do so with its own canon support. This story’s conflict arises from limited information and human interpretation, not from a secret the player can uncover through the correct dialogue path.

## 119. Optional coda: the folded map

Several days later, if the existing narrative route supports a brief return conversation, the player can find Daro folding the map to put it away. He stops at the ridge and asks whether the player wants to look at the candidate again. The map remains unchanged.

Daro says:
> “I thought putting the paper away would make it stop being a question.”
>
> “Did it?”
>
> “No. It made the table easier to use.”
>
> “That matters.”
>
> “It does.”

If the player asks whether he still thinks there may be someone at Echo, Daro says he has no better basis for a guess. If the player asks whether he hopes, he says he hopes the group does not turn an unknown into a command. That is his wish about the people, not a prediction about the bunker.

This coda is optional and should be omitted if current narrative state cannot deliver it without a new persistent flag. The main ending remains complete without it.

## 120. Optional coda: the private note remains private

If Ena shared the note, she may mention it later only with her own consent and without reproducing its contents. She can say, “I am glad you did not write it down,” or “I decided to keep that list in my coat.” She does not ask whether the signal might be her brother.

If the player did not read it, Ena does not return to ask them to. She may talk about the kettle or the laundry line instead. This variation shows that consent is not a one-time gate followed by repeated pressure. The story treats the private memory as private after the scene ends.

## 121. Optional coda: the notice ages

If the notice was posted, a later scene may show the card curled at one corner or moved beneath another ordinary announcement. The writing is still legible. Nobody has added a secret response. No new mark appears from the bunker.

Mira asks the player whether she should take it down. The player may leave it, remove it, or ask her to decide. If removed, she folds it and keeps it with her own notes. If left, the current board owner determines when it is refreshed; the story does not create a timer. If the current UI cannot show board text persistently, this coda should remain dialogue-only.

The card’s aging is a visual metaphor for the difference between a temporary human note and the durable map record. It must not imply that the rumor has expired or that the signal has stopped.

## 122. Quiet final prose

A final optional paragraph may accompany the closing journal note:

> Outside, the ridge was only a dark line beyond the settlement lights. No one at the table could see the bunker from there. We had a name on the map, a sentence in the journal, and a blank where a voice might have gone. Ena folded her note smaller. Mira left the card where the group had agreed. Daro closed his book. The night did not answer for them.

This passage should only be used if the narrative camera is authorized to describe the ridge from the shelter. “No one could see the bunker” is proposed scene staging, not a confirmed environmental line of sight; if the location layout makes it visible, revise the detail. The final sentence is metaphorical quiet, not a radio event.

An alternate ending avoids physical geography:
> The kettle cooled. The map went back into its sleeve. The journal kept its sentence. The rest belonged to the people who had heard it.

## 123. Acceptance criteria for authored content

The content package is ready for implementation when:
- the exact source event and mapped ID have been rechecked;
- the story uses only the current existing route for delivery;
- no unverified site access, transmitter operator, voice, or message appears;
- the notice branches retain the distinction between a map rumor and a rescue announcement;
- private-memory consent is honored across all dialogue branches;
- all ending variants leave the existing map and signal state untouched;
- the game’s actual journal and narrative schemas accept the content without creating parallel authority;
- any required new state has been removed or separately approved through the project’s governance path;
- the final text reads as character drama rather than a technical manual;
- the location’s high danger and lack of current expedition row are not contradicted by dialogue.

These criteria describe an authored-content handoff. They do not claim implementation has occurred.

## 124. Focused verification after implementation

For a later implementation, verification should be limited to the changed content route and its current validators. Confirm that:
- the story becomes eligible from the existing Echo location-reveal event;
- the host journal line remains the current system-generated line;
- the world map still records the rumor through its existing owner;
- no dialogue branch changes signal identity confidence, calculated location confidence, map confidence, or discovery state;
- the public, private, and no-notice outcomes display the intended authored text;
- the private note is never journaled without Ena’s chosen consent;
- restarting or revisiting the dialogue does not invent a saved belief or duplicate a journal entry;
- no travel interaction or signal playback has been introduced by the narrative package.

This plan itself changes documentation only. No production implementation or tests are included in this deliverable.

## 125. Handoff note for future implementers

Treat this document as a prose and content plan grounded in the currently inspected local source. Before coding, recheck the active narrative owner, direction-finding event, map rumor consumer, journal schema, location and expedition catalogs, and current package ownership. List exact files and claims before edits under the project’s workflow. Use the smallest content path that can carry the scene. Do not create an architectural owner for the notice or private note just to preserve every optional branch.

The authored center is a conversation about a carrier name, a mapped candidate, and a human wish. If a current owner cannot support a branch, remove that branch and preserve the center rather than expanding the runtime to match every line in this plan.

## 126. Final content statement

“A Coordinate Is Not a Voice” is a story about the way a map can point toward a place and still leave a room full of people deciding what the point means to them. The player cannot make the signal answer by choosing carefully. The player can decide not to speak for Ena, not to turn a rumor into a rescue call, and not to confuse a wish with a report.

Ena is allowed to keep hoping. Daro is allowed to keep the record narrow. Mira is allowed to make a public sentence or leave the card blank. Jori is allowed to remember how an ordinary sound once seemed addressed to them. The player may sit with them, disagree, apologize, or leave. The carrier remains what the current sources say it is: a cataloged fingerprint associated with a location candidate through the existing direction-finding path.

**End of the main scene.**

## 127. Last light at the table

When the player leaves the room, the notice and folded map are no longer arranged like evidence. Mira has stacked the unused cards beneath the cloth. Daro has closed his notebook on the page with the two words he could support. Ena has put her private note back into her own coat. None of those gestures changes the map or the direction-finding entry; they return the conversation’s objects to the people who brought them.

If the player pauses at the doorway, Jori asks whether they are waiting for another sound. The player may say no, say they are listening to the room, or simply leave. Jori answers, “Then let it be the room.” A cup touches wood. Someone folds a chair. The night continues without a transmission cue.

This closing image leaves the player with the difference between attention and proof. The characters have listened carefully to one another. The catalog remains a catalog, the candidate remains a map rumor, and the message column remains empty in this story.

**End of Expansion 73.**
