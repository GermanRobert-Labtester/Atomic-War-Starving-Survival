# EXPANSION 157 — The Key Behind the Diploma

## A hospital-pharmacy expansion about a locked room, broken preservation, and a warning that must survive the radio.

### Wave 30: What a Record Cannot Settle

## Batch brief

**Content type:** prose-first playable-content expansion plan with scene drafts, diegetic records, conversation fragments, and conditional callbacks.
**Content bank:** 32 story beats with four alternative authored forms per beat (128 candidate passages).
**Current location anchor:** `hospital_pharmacy` — Hospital Pharmacy.
**Tone:** material, restrained, human, and careful with uncertainty.
**Canon sensitivity:** current location and linked authored records define known facts; old plans or JSON presence do not prove a current runtime route.
**Scope:** game-content plan and original prose drafts only; no production code, JSON, route, quest, flag, system, or save change.

## 1. Expansion thesis

The old pharmacy remains behind an intact door and a collapsed stairwell. One tape leaves a key behind a doctor’s diploma and tells a finder to take everything; a later expedition report records a much narrower distinction between sealed, stable supplies and a back room whose cold chain failed. Elsewhere, a medical evacuation signal uses the wrong terms and is identified as a raider false flag. The expansion is not a new scavenging mechanic or a second rescue quest. It is a set of records about how a generous instruction becomes dangerous when separated from the evidence that followed it. The bank below is intended to produce playable narrative texture: a player can discover, compare, question, refuse, or return to a passage while the existing game systems continue to own state. The fragments are written as content, not as a feature roadmap.

## 2. Story question

What does a generous instruction mean after the conditions that made it safe have disappeared?

## 3. Verified local anchor and source records

The location row describes an intact pharmacy door behind a collapsed stairwell and shelves that looters could not reach. The Saint Maren cassette set contains a three-part account of triage and a key behind a diploma. The Day 21 expedition report distinguishes sealed stable salvage from warm vaccines and a decayed back room. A radio distress record explicitly identifies a false medical evacuation signal and documents its terminology. These are distinct sources with different purposes; the report is not an amendment secretly inserted into the tape.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `hospital_pharmacy` | intact door, collapsed stairwell, shelves of unreachable vials and boxes |
| `Assets/StreamingAssets/Data/cassette_sets.json` | `hospital_saint_maren` | three recordings: triage, a difficult decision, pharmacy key |
| `Assets/StreamingAssets/Data/narrative/field_reports_expansion.json` | `exp_report_pharmacy` | Day 21 report, one-person expedition, cold-chain warning, specific salvage |
| `Assets/StreamingAssets/Data/radio_distress_signals.json` | `freq_distress_478_2 / fu_478_2_trap_fallen_for` | false-flag medical signal, terminology clues, ambush follow-up |
| `Assets/StreamingAssets/Data/expeditions.json` | `hospital_pharmacy` | current expedition risk and scavenging categories |
| `Assets/StreamingAssets/Data/final_wishes.json` | `reach_the_pharmacy` | existing character-linked destination step |

## 4. Fixed canon and open space

The door, stairwell, shelves, and broad tape instruction are authored. The field report supplies the specific later observation: the cold chain is broken, vaccines are warm, and only named sealed stable items are described as recovered. The false-flag broadcast is a separate hostile signal, not proof that all medical calls are false. The existing final-wish step refers to an unnamed character placeholder and does not establish a new pharmacist NPC. Existing characters retain their authored identity, boundaries, and outcomes. New working voices remain editorial until an existing content owner approves them. Never fill a source gap solely to make the scene resolve.

## 5. Human center

The story belongs to people who leave instructions for strangers and to later readers who have to decide how narrowly they can trust a sentence. Let a tape’s final generosity remain generous and let the field report remain careful. Neither needs a narrator to declare the other foolish. The emotional pressure should come through what a person records, omits, repairs, asks, or leaves unsigned rather than a narrator naming what the location means.

## 6. Voice and point of view

The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep each author’s knowledge local. A field note cannot know what an unnamed visitor thought; a later reader cannot recover a date that was never recorded; a title cannot create a route. Vary sentence length and register by document purpose, not by changing established facts.

## 7. Placement and current reachability

Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. All fragments are candidates. None is evidence that the location is currently player-reachable. Before selecting any text, verify the current map/location owner, content schema, actual consumer, and the source condition under which the text can appear.

## 8. Player agency

Existing radio and expedition choices remain the player’s real decisions. A proposed passage may let the player ask who authored the report, distinguish a tape from a field note, or decline to carry a copy. No additional item choice or trust test is added. Do not add a moral-choice menu simply to make quiet prose interactive. Preserve the player’s refusal, ability to leave, and the authority of the characters who own their words.

## 9. Continuity, dignity, and safety

Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Keep the setting fictional and physically grounded. No absent person receives a biography merely to heighten emotion. Technical and medical context remains descriptive and non-instructional.

## 10. Existing hooks and implementation boundary

**Existing content anchors:** the location row and linked records listed above. **Unverified:** any route, text consumer, state condition, dialogue surface, or return trigger not explicitly established by current authority. **Classification:** editorial game-content proposal; no implementation category is claimed until an owner and consumer are confirmed.

This plan changes no production code or game data. Do not add a parallel discovery registry, route, save section, or gameplay authority to host these drafts. Where a passage reflects a branch, use only the existing state named in section 16 and only after its current owner confirms the consumer.

## 11. Narrative sequence

### 1. Disturbance — the locked door

The player reaches an intact pharmacy door behind collapsed access and finds a set whose final recording promises a key.

### 2. Discovery — two voices

The tape offers a broad instruction; the expedition report provides later, location-specific evidence. Keep the voices separate.

### 3. Interpretation — a false medical call

The existing radio record teaches that compassion can be exploited in one documented event, without making medicine itself suspect.

### 4. Complication — preserved words, changed conditions

A line meant for a future user survives longer than the conditions it describes. Do not create a new safety answer beyond the current report.

### 5. Choice — carry a narrow copy

The player may carry the report or leave it with the room only if an existing content owner exposes such a choice; no inventory effect is implied.

### 6. Callback — a name left blank

The final-wish destination remains placeholder-driven; later text can respect a former worker without inventing their biography.

## 12. Creative variants

### Grounded

A brief three-source comparison: tape, report, radio warning.

### Interlinked

A later line can reflect the existing false-flag resolution and the cassette completion state only if those owners expose them.

### Wild card

Tell the return scene through three headings on a card—Recorded, Observed, Unknown—without an omniscient narrator.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not four required encounters. Scene drafts stage an observation; record drafts give it a plausible author and audience; conversation fragments expose a practical point of friction; consequence vignettes allow a later reader to recognize changed context. Select only forms that fit an existing content owner.

A selected fragment should answer who made it, why they made it, who might read it, and what the author cannot know. Keep objects specific to this location. Let a line do practical work before it carries a theme. Remove exposition that a worker would not say aloud, repeated catastrophe language, unearned revelation, and wording that could be mistaken for a new mechanic. Where existing game state matters, state it in the source owner’s terms and do not duplicate its authority.

## 14. Content bank

### Scene draft 001 — Door behind the stair

The pharmacy door is still intact where the stair collapsed in front of it. The scene stops at the threshold and does not invent an alternate entrance. The source gives one blocked approach and one intact door. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The shelves are still there.”
Radio listener: “That is not the same as being safe to take from.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source gives one blocked approach and one intact door. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A later note preserves the distinction between access and condition. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 002 — Door behind the stair

Proposed diegetic text: “Door intact. Stair access collapsed. No alternate entry recorded.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The pharmacy door is still intact where the stair collapsed in front of it. The scene stops at the threshold and does not invent an alternate entrance. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source gives one blocked approach and one intact door. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later note preserves the distinction between access and condition. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 003 — Door behind the stair

The pharmacy door is still intact where the stair collapsed in front of it. The scene stops at the threshold and does not invent an alternate entrance. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The source gives one blocked approach and one intact door. Neither voice exists to lecture the player.

Field reporter: “The shelves are still there.”
Radio listener: “That is not the same as being safe to take from.”
Field reporter: “A later note preserves the distinction between access and condition.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 004 — Door behind the stair

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later note preserves the distinction between access and condition. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A later note preserves the distinction between access and condition.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Door intact. Stair access collapsed. No alternate entry recorded.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 005 — The shelf seen through the gap

Boxes are visible beyond the counter; their labels cannot all be read from where the visitor stands. The prose does not turn visibility into an inventory. The location describes vials and boxes left beyond looter reach. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Can you tell what is on the shelf?”
Radio listener: “Only what the report actually lists.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location describes vials and boxes left beyond looter reach. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The uncounted boxes remain uncounted. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 006 — The shelf seen through the gap

Proposed diegetic text: “Visible stock is not counted in this note.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: Boxes are visible beyond the counter; their labels cannot all be read from where the visitor stands. The prose does not turn visibility into an inventory. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location describes vials and boxes left beyond looter reach. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The uncounted boxes remain uncounted. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 007 — The shelf seen through the gap

Boxes are visible beyond the counter; their labels cannot all be read from where the visitor stands. The prose does not turn visibility into an inventory. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The location describes vials and boxes left beyond looter reach. Neither voice exists to lecture the player.

Field reporter: “Can you tell what is on the shelf?”
Radio listener: “Only what the report actually lists.”
Field reporter: “The uncounted boxes remain uncounted.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 008 — The shelf seen through the gap

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The uncounted boxes remain uncounted. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The uncounted boxes remain uncounted.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Visible stock is not counted in this note.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 009 — The final tape begins

A player hears the third Saint Maren tape after the first two; the room remains the pharmacy, not a reconstructed hospital ward. The set has three recordings, with the pharmacy key in the final entry. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The last message was meant for a finder.”
Radio listener: “It was meant for a finder then.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The set has three recordings, with the pharmacy key in the final entry. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A later reader can hear the past tense. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 010 — The final tape begins

Proposed diegetic text: “Tape set: part three. Key location described in source recording.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A player hears the third Saint Maren tape after the first two; the room remains the pharmacy, not a reconstructed hospital ward. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The set has three recordings, with the pharmacy key in the final entry. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader can hear the past tense. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 011 — The final tape begins

A player hears the third Saint Maren tape after the first two; the room remains the pharmacy, not a reconstructed hospital ward. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The set has three recordings, with the pharmacy key in the final entry. Neither voice exists to lecture the player.

Field reporter: “The last message was meant for a finder.”
Radio listener: “It was meant for a finder then.”
Field reporter: “A later reader can hear the past tense.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 012 — The final tape begins

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader can hear the past tense. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A later reader can hear the past tense.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Tape set: part three. Key location described in source recording.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 013 — A diploma face down

The proposed scene shows only the reverse of a framed diploma after the existing key has been found. No doctor name is supplied in this plan. The cassette places the key behind the diploma in the doctor’s office. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The key was hidden where another worker might look.”
Radio listener: “That was the instruction.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The cassette places the key behind the diploma in the doctor’s office. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The report does not claim who followed it first. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 014 — A diploma face down

Proposed diegetic text: “Key location copied from the tape. Name of diploma holder not added.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The proposed scene shows only the reverse of a framed diploma after the existing key has been found. No doctor name is supplied in this plan. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The cassette places the key behind the diploma in the doctor’s office. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The report does not claim who followed it first. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 015 — A diploma face down

The proposed scene shows only the reverse of a framed diploma after the existing key has been found. No doctor name is supplied in this plan. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The cassette places the key behind the diploma in the doctor’s office. Neither voice exists to lecture the player.

Field reporter: “The key was hidden where another worker might look.”
Radio listener: “That was the instruction.”
Field reporter: “The report does not claim who followed it first.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 016 — A diploma face down

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The report does not claim who followed it first. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The report does not claim who followed it first.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Key location copied from the tape. Name of diploma holder not added.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 017 — The key and its wording

A copyist records the key instruction exactly enough to recognize it, then sets the sentence beside the later field note. A broad take everything line exists in the tape, but the later report narrows what was salvaged. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “It says everything.”
Radio listener: “The report says what was actually stable.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: A broad take everything line exists in the tape, but the later report narrows what was salvaged. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The margin carries both source labels forward. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 018 — The key and its wording

Proposed diegetic text: “Tape wording retained as tape wording; present condition requires a later source.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A copyist records the key instruction exactly enough to recognize it, then sets the sentence beside the later field note. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: A broad take everything line exists in the tape, but the later report narrows what was salvaged. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The margin carries both source labels forward. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 019 — The key and its wording

A copyist records the key instruction exactly enough to recognize it, then sets the sentence beside the later field note. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: A broad take everything line exists in the tape, but the later report narrows what was salvaged. Neither voice exists to lecture the player.

Field reporter: “It says everything.”
Radio listener: “The report says what was actually stable.”
Field reporter: “The margin carries both source labels forward.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 020 — The key and its wording

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The margin carries both source labels forward. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The margin carries both source labels forward.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Tape wording retained as tape wording; present condition requires a later source.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 021 — Triage at six hours

The first recording gives a time marker and a triage protocol; the plan does not add patient totals or a hospital map. Saint Maren tape one is a doctor’s account at Day Zero plus six hours. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “They were already making categories.”
Radio listener: “The tape says they had to.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Saint Maren tape one is a doctor’s account at Day Zero plus six hours. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The record remains an account, not a current triage rule. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 022 — Triage at six hours

Proposed diegetic text: “Recorded: Day Zero plus six hours. No count copied.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The first recording gives a time marker and a triage protocol; the plan does not add patient totals or a hospital map. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Saint Maren tape one is a doctor’s account at Day Zero plus six hours. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The record remains an account, not a current triage rule. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 023 — Triage at six hours

The first recording gives a time marker and a triage protocol; the plan does not add patient totals or a hospital map. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: Saint Maren tape one is a doctor’s account at Day Zero plus six hours. Neither voice exists to lecture the player.

Field reporter: “They were already making categories.”
Radio listener: “The tape says they had to.”
Field reporter: “The record remains an account, not a current triage rule.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 024 — Triage at six hours

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The record remains an account, not a current triage rule. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The record remains an account, not a current triage rule.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Recorded: Day Zero plus six hours. No count copied.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 025 — The taped pause

The tape’s speaker pauses over the part of the decision they regret. The listener does not fill the pause with a judgment. The second cassette describes choosing some patients and keeping others comfortable. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The silence is part of the recording.”
Radio listener: “Do not put a new sentence into it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The second cassette describes choosing some patients and keeping others comfortable. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The later copy preserves the pause rather than explaining it. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 026 — The taped pause

Proposed diegetic text: “Tape transcript note: pause retained; no motive added beyond the speaker’s words.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The tape’s speaker pauses over the part of the decision they regret. The listener does not fill the pause with a judgment. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The second cassette describes choosing some patients and keeping others comfortable. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later copy preserves the pause rather than explaining it. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 027 — The taped pause

The tape’s speaker pauses over the part of the decision they regret. The listener does not fill the pause with a judgment. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The second cassette describes choosing some patients and keeping others comfortable. Neither voice exists to lecture the player.

Field reporter: “The silence is part of the recording.”
Radio listener: “Do not put a new sentence into it.”
Field reporter: “The later copy preserves the pause rather than explaining it.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 028 — The taped pause

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later copy preserves the pause rather than explaining it. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The later copy preserves the pause rather than explaining it.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Tape transcript note: pause retained; no motive added beyond the speaker’s words.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 029 — Warm air in the pharmacy

A field reporter touches the air only as an observed condition, then turns to the items the report actually describes. Atmosphere and expedition records state the cold chain broke and the vaccines were warm. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Warm is a condition, not a label.”
Radio listener: “Then keep both in separate columns.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Atmosphere and expedition records state the cold chain broke and the vaccines were warm. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The follow-up still cites the Day 21 report. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 030 — Warm air in the pharmacy

Proposed diegetic text: “Cold-chain condition: failed in the observed report. Do not mark unspecified stock usable.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A field reporter touches the air only as an observed condition, then turns to the items the report actually describes. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Atmosphere and expedition records state the cold chain broke and the vaccines were warm. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The follow-up still cites the Day 21 report. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 031 — Warm air in the pharmacy

A field reporter touches the air only as an observed condition, then turns to the items the report actually describes. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: Atmosphere and expedition records state the cold chain broke and the vaccines were warm. Neither voice exists to lecture the player.

Field reporter: “Warm is a condition, not a label.”
Radio listener: “Then keep both in separate columns.”
Field reporter: “The follow-up still cites the Day 21 report.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 032 — Warm air in the pharmacy

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The follow-up still cites the Day 21 report. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The follow-up still cites the Day 21 report.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Cold-chain condition: failed in the observed report. Do not mark unspecified stock usable.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 033 — The hum is gone

A reader notices that no refrigeration hum is present. The report does not ask the reader to diagnose why a particular machine failed. The atmosphere record calls stillness the warning left by the absent hum. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “You can hear when the room is no longer working.”
Radio listener: “You can hear one thing that is no longer working.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere record calls stillness the warning left by the absent hum. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: No new machinery condition is inferred. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 034 — The hum is gone

Proposed diegetic text: “Refrigeration hum absent at inspection. Cause not entered.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A reader notices that no refrigeration hum is present. The report does not ask the reader to diagnose why a particular machine failed. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere record calls stillness the warning left by the absent hum. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new machinery condition is inferred. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 035 — The hum is gone

A reader notices that no refrigeration hum is present. The report does not ask the reader to diagnose why a particular machine failed. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The atmosphere record calls stillness the warning left by the absent hum. Neither voice exists to lecture the player.

Field reporter: “You can hear when the room is no longer working.”
Radio listener: “You can hear one thing that is no longer working.”
Field reporter: “No new machinery condition is inferred.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 036 — The hum is gone

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new machinery condition is inferred. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “No new machinery condition is inferred.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Refrigeration hum absent at inspection. Cause not entered.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 037 — Sweetness from the back room

A sweetness remains under the disinfectant; the reporter’s note stops before the threshold to the back room. The atmosphere record locates the smell in the back room and the expedition report says not to enter it without a mask. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The smell does not need a name.”
Radio listener: “The report already says where it comes from.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere record locates the smell in the back room and the expedition report says not to enter it without a mask. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A later reader sees the warning before the door. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 038 — Sweetness from the back room

Proposed diegetic text: “Back-room warning retained from source. No entry scene proposed.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A sweetness remains under the disinfectant; the reporter’s note stops before the threshold to the back room. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere record locates the smell in the back room and the expedition report says not to enter it without a mask. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader sees the warning before the door. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 039 — Sweetness from the back room

A sweetness remains under the disinfectant; the reporter’s note stops before the threshold to the back room. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The atmosphere record locates the smell in the back room and the expedition report says not to enter it without a mask. Neither voice exists to lecture the player.

Field reporter: “The smell does not need a name.”
Radio listener: “The report already says where it comes from.”
Field reporter: “A later reader sees the warning before the door.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 040 — Sweetness from the back room

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader sees the warning before the door. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A later reader sees the warning before the door.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Back-room warning retained from source. No entry scene proposed.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 041 — A sealed course

The field report records a sealed course from the front cabinet and calls it stable. The plan does not add a use instruction or a patient. The Day 21 report lists sealed broad-spectrum antibiotics among recovered items. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The package held.”
Radio listener: “That is what this note can say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The Day 21 report lists sealed broad-spectrum antibiotics among recovered items. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The next reader gets a provenance claim, not advice. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 042 — A sealed course

Proposed diegetic text: “Sealed item reported recovered; no treatment instruction in this expansion.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The field report records a sealed course from the front cabinet and calls it stable. The plan does not add a use instruction or a patient. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The Day 21 report lists sealed broad-spectrum antibiotics among recovered items. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The next reader gets a provenance claim, not advice. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 043 — A sealed course

The field report records a sealed course from the front cabinet and calls it stable. The plan does not add a use instruction or a patient. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The Day 21 report lists sealed broad-spectrum antibiotics among recovered items. Neither voice exists to lecture the player.

Field reporter: “The package held.”
Radio listener: “That is what this note can say.”
Field reporter: “The next reader gets a provenance claim, not advice.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 044 — A sealed course

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The next reader gets a provenance claim, not advice. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The next reader gets a provenance claim, not advice.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Sealed item reported recovered; no treatment instruction in this expansion.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 045 — Half a bottle

A half-full bottle appears on the salvage list without being made into a promise about what it can do. The report lists one half-full usable iodine bottle. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “It is less than a full bottle.”
Radio listener: “And more than an invented one.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The report lists one half-full usable iodine bottle. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A future copy keeps the same limited description. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 046 — Half a bottle

Proposed diegetic text: “Iodine — half full, as reported. Quantity not expanded.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A half-full bottle appears on the salvage list without being made into a promise about what it can do. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The report lists one half-full usable iodine bottle. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A future copy keeps the same limited description. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 047 — Half a bottle

A half-full bottle appears on the salvage list without being made into a promise about what it can do. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The report lists one half-full usable iodine bottle. Neither voice exists to lecture the player.

Field reporter: “It is less than a full bottle.”
Radio listener: “And more than an invented one.”
Field reporter: “A future copy keeps the same limited description.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 048 — Half a bottle

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A future copy keeps the same limited description. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A future copy keeps the same limited description.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Iodine — half full, as reported. Quantity not expanded.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 049 — Gauze still in wrappers

The reporter counts rolls by the number already entered in the completed trip report. No cache is refilled by the scene. The source lists three sealed rolls of gauze. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The number came from the field note.”
Radio listener: “Then quote its date with it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source lists three sealed rolls of gauze. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The later text does not turn salvage into ongoing stock. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 050 — Gauze still in wrappers

Proposed diegetic text: “Three rolls, sealed, according to the Day 21 report.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The reporter counts rolls by the number already entered in the completed trip report. No cache is refilled by the scene. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source lists three sealed rolls of gauze. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later text does not turn salvage into ongoing stock. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 051 — Gauze still in wrappers

The reporter counts rolls by the number already entered in the completed trip report. No cache is refilled by the scene. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The source lists three sealed rolls of gauze. Neither voice exists to lecture the player.

Field reporter: “The number came from the field note.”
Radio listener: “Then quote its date with it.”
Field reporter: “The later text does not turn salvage into ongoing stock.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 052 — Gauze still in wrappers

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later text does not turn salvage into ongoing stock. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The later text does not turn salvage into ongoing stock.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Three rolls, sealed, according to the Day 21 report.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 053 — Scissors on a cloth

Clean scissors are described as an object in the report, not as an invitation to demonstrate a procedure. The field report lists one pair of surgical scissors as clean. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The tool is clean in the report.”
Radio listener: “For that observation, on that day.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The field report lists one pair of surgical scissors as clean. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The sentence remains attached to its source. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 054 — Scissors on a cloth

Proposed diegetic text: “One pair reported. No procedural use described.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: Clean scissors are described as an object in the report, not as an invitation to demonstrate a procedure. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The field report lists one pair of surgical scissors as clean. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The sentence remains attached to its source. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 055 — Scissors on a cloth

Clean scissors are described as an object in the report, not as an invitation to demonstrate a procedure. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The field report lists one pair of surgical scissors as clean. Neither voice exists to lecture the player.

Field reporter: “The tool is clean in the report.”
Radio listener: “For that observation, on that day.”
Field reporter: “The sentence remains attached to its source.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 056 — Scissors on a cloth

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The sentence remains attached to its source. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The sentence remains attached to its source.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “One pair reported. No procedural use described.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 057 — The vaccines stay warm

A volunteer reads the report’s vaccine line and folds the page shut. The story does not stage a debate over using them. The source explicitly warns that the vaccines are warm and not to be used. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “That part is not ambiguous.”
Radio listener: “Not in this report.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source explicitly warns that the vaccines are warm and not to be used. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The next reader receives the warning without an extra condition. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 058 — The vaccines stay warm

Proposed diegetic text: “Warm vaccines: do not use, as recorded in the report.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A volunteer reads the report’s vaccine line and folds the page shut. The story does not stage a debate over using them. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source explicitly warns that the vaccines are warm and not to be used. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The next reader receives the warning without an extra condition. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 059 — The vaccines stay warm

A volunteer reads the report’s vaccine line and folds the page shut. The story does not stage a debate over using them. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The source explicitly warns that the vaccines are warm and not to be used. Neither voice exists to lecture the player.

Field reporter: “That part is not ambiguous.”
Radio listener: “Not in this report.”
Field reporter: “The next reader receives the warning without an extra condition.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 060 — The vaccines stay warm

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The next reader receives the warning without an extra condition. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The next reader receives the warning without an extra condition.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Warm vaccines: do not use, as recorded in the report.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 061 — The back room is not entered

The scene ends with the back-room door closed. The author refuses a body inventory or an additional hazard list. The expedition report says the back room has bodies and advises not entering without a mask. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “There is a reason to leave it shut.”
Radio listener: “The report has already given enough.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The expedition report says the back room has bodies and advises not entering without a mask. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The return vignette does not turn avoidance into a secret reward. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 062 — The back room is not entered

Proposed diegetic text: “Back room not entered in the report. No further description added.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The scene ends with the back-room door closed. The author refuses a body inventory or an additional hazard list. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The expedition report says the back room has bodies and advises not entering without a mask. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The return vignette does not turn avoidance into a secret reward. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 063 — The back room is not entered

The scene ends with the back-room door closed. The author refuses a body inventory or an additional hazard list. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The expedition report says the back room has bodies and advises not entering without a mask. Neither voice exists to lecture the player.

Field reporter: “There is a reason to leave it shut.”
Radio listener: “The report has already given enough.”
Field reporter: “The return vignette does not turn avoidance into a secret reward.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 064 — The back room is not entered

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The return vignette does not turn avoidance into a secret reward. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The return vignette does not turn avoidance into a secret reward.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Back room not entered in the report. No further description added.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 065 — Casualties, not patients

A listener marks the first distress fragment’s word choice in the margin and does not claim that vocabulary alone proved the sender’s identity. The radio record says the terminology is wrong and the signal is a raider false flag. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “It sounded like a request.”
Radio listener: “The trace found a reason to doubt this one.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The radio record says the terminology is wrong and the signal is a raider false flag. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The warning stays specific to frequency 478.2. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 066 — Casualties, not patients

Proposed diegetic text: “Signal transcription: casualties and transport are source terms.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A listener marks the first distress fragment’s word choice in the margin and does not claim that vocabulary alone proved the sender’s identity. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The radio record says the terminology is wrong and the signal is a raider false flag. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The warning stays specific to frequency 478.2. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 067 — Casualties, not patients

A listener marks the first distress fragment’s word choice in the margin and does not claim that vocabulary alone proved the sender’s identity. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The radio record says the terminology is wrong and the signal is a raider false flag. Neither voice exists to lecture the player.

Field reporter: “It sounded like a request.”
Radio listener: “The trace found a reason to doubt this one.”
Field reporter: “The warning stays specific to frequency 478.2.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 068 — Casualties, not patients

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The warning stays specific to frequency 478.2. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The warning stays specific to frequency 478.2.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Signal transcription: casualties and transport are source terms.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 069 — No names in the first fragment

The first radio fragment gives three casualties without names or conditions. The note makes that absence visible, not suspicious by itself. The radio outcome hint calls it a manifest, not a plea. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “There is no one to picture yet.”
Radio listener: “There is still someone asking.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The radio outcome hint calls it a manifest, not a plea. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The next passage does not use absence as proof. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 070 — No names in the first fragment

Proposed diegetic text: “Names: none in source fragment. Conditions: none in source fragment.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The first radio fragment gives three casualties without names or conditions. The note makes that absence visible, not suspicious by itself. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The radio outcome hint calls it a manifest, not a plea. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The next passage does not use absence as proof. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 071 — No names in the first fragment

The first radio fragment gives three casualties without names or conditions. The note makes that absence visible, not suspicious by itself. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The radio outcome hint calls it a manifest, not a plea. Neither voice exists to lecture the player.

Field reporter: “There is no one to picture yet.”
Radio listener: “There is still someone asking.”
Field reporter: “The next passage does not use absence as proof.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 072 — No names in the first fragment

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The next passage does not use absence as proof. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The next passage does not use absence as proof.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Names: none in source fragment. Conditions: none in source fragment.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 073 — The clinic was already stripped

A reader checks the trace note before carrying the signal farther. The line is attributed to the existing broadcast, not a general map report. The radio record says the old clinic had been stripped months earlier. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “That place could not have sent this as described.”
Radio listener: “That is what the warning says.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The radio record says the old clinic had been stripped months earlier. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: No other clinic is declared empty by this line. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 074 — The clinic was already stripped

Proposed diegetic text: “Clinic condition copied from the false-flag record only.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A reader checks the trace note before carrying the signal farther. The line is attributed to the existing broadcast, not a general map report. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The radio record says the old clinic had been stripped months earlier. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No other clinic is declared empty by this line. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 075 — The clinic was already stripped

A reader checks the trace note before carrying the signal farther. The line is attributed to the existing broadcast, not a general map report. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The radio record says the old clinic had been stripped months earlier. Neither voice exists to lecture the player.

Field reporter: “That place could not have sent this as described.”
Radio listener: “That is what the warning says.”
Field reporter: “No other clinic is declared empty by this line.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 076 — The clinic was already stripped

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No other clinic is declared empty by this line. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “No other clinic is declared empty by this line.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Clinic condition copied from the false-flag record only.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 077 — A trap after the response

A follow-up signal names the ambushed responders and the stolen kits after the existing branch where the lure is taken. The follow-up is conditional on the authored ambush outcome. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The message came after the team was already gone.”
Radio listener: “Use its existing timing.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The follow-up is conditional on the authored ambush outcome. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A later reader sees this only on the branch that owns it. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 078 — A trap after the response

Proposed diegetic text: “Conditional radio callback only; no new ambush event.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A follow-up signal names the ambushed responders and the stolen kits after the existing branch where the lure is taken. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The follow-up is conditional on the authored ambush outcome. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader sees this only on the branch that owns it. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 079 — A trap after the response

A follow-up signal names the ambushed responders and the stolen kits after the existing branch where the lure is taken. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The follow-up is conditional on the authored ambush outcome. Neither voice exists to lecture the player.

Field reporter: “The message came after the team was already gone.”
Radio listener: “Use its existing timing.”
Field reporter: “A later reader sees this only on the branch that owns it.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 080 — A trap after the response

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader sees this only on the branch that owns it. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A later reader sees this only on the branch that owns it.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Conditional radio callback only; no new ambush event.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 081 — The false flag is one record

A reader writes the frequency number on the outside of a card so the warning cannot be mistaken for a rule about all medical calls. The record identifies one false flag from raiders. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “That call lied.”
Radio listener: “This note does not say all calls lie.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The record identifies one false flag from raiders. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The scope label remains on every copy. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 082 — The false flag is one record

Proposed diegetic text: “478.2 — one identified false flag. Scope not generalized.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A reader writes the frequency number on the outside of a card so the warning cannot be mistaken for a rule about all medical calls. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The record identifies one false flag from raiders. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The scope label remains on every copy. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 083 — The false flag is one record

A reader writes the frequency number on the outside of a card so the warning cannot be mistaken for a rule about all medical calls. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The record identifies one false flag from raiders. Neither voice exists to lecture the player.

Field reporter: “That call lied.”
Radio listener: “This note does not say all calls lie.”
Field reporter: “The scope label remains on every copy.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 084 — The false flag is one record

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The scope label remains on every copy. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The scope label remains on every copy.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “478.2 — one identified false flag. Scope not generalized.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 085 — The expedition leader’s name

Mira’s report is signed in the existing catalog and is not expanded into a biography. The report identifies Mira the Scavenger, team size one, duration one day. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “She carried back the report.”
Radio listener: “That is already enough to credit her.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The report identifies Mira the Scavenger, team size one, duration one day. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: No second expedition is silently appended. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 086 — The expedition leader’s name

Proposed diegetic text: “Reporter: Mira. Team: one. Duration: one day.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: Mira’s report is signed in the existing catalog and is not expanded into a biography. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The report identifies Mira the Scavenger, team size one, duration one day. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No second expedition is silently appended. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 087 — The expedition leader’s name

Mira’s report is signed in the existing catalog and is not expanded into a biography. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The report identifies Mira the Scavenger, team size one, duration one day. Neither voice exists to lecture the player.

Field reporter: “She carried back the report.”
Radio listener: “That is already enough to credit her.”
Field reporter: “No second expedition is silently appended.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 088 — The expedition leader’s name

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No second expedition is silently appended. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “No second expedition is silently appended.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Reporter: Mira. Team: one. Duration: one day.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 089 — The loading-bay approach

A field note references the reported loading-bay approach and then returns to the objects found in the pharmacy. It does not create a route for the player. The expedition report records approach via the hospital loading bay. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The report got inside.”
Radio listener: “The current map must still tell you whether you can.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The expedition report records approach via the hospital loading bay. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The old route note is not a present navigation command. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 090 — The loading-bay approach

Proposed diegetic text: “Approach quoted from completed report. Current access not inferred.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A field note references the reported loading-bay approach and then returns to the objects found in the pharmacy. It does not create a route for the player. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The expedition report records approach via the hospital loading bay. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The old route note is not a present navigation command. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 091 — The loading-bay approach

A field note references the reported loading-bay approach and then returns to the objects found in the pharmacy. It does not create a route for the player. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The expedition report records approach via the hospital loading bay. Neither voice exists to lecture the player.

Field reporter: “The report got inside.”
Radio listener: “The current map must still tell you whether you can.”
Field reporter: “The old route note is not a present navigation command.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 092 — The loading-bay approach

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The old route note is not a present navigation command. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The old route note is not a present navigation command.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Approach quoted from completed report. Current access not inferred.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 093 — A list that can be checked

A copyist places the four salvage observations in their own dated box, away from the tape’s broad instruction. The report lists antibiotics, iodine, gauze, and scissors, with conditions. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The list is shorter than the shelves.”
Radio listener: “Because it is the list the report can verify.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The report lists antibiotics, iodine, gauze, and scissors, with conditions. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The shelves do not acquire a count from the summary. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 094 — A list that can be checked

Proposed diegetic text: “Report summary retained with source and date.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A copyist places the four salvage observations in their own dated box, away from the tape’s broad instruction. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The report lists antibiotics, iodine, gauze, and scissors, with conditions. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The shelves do not acquire a count from the summary. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 095 — A list that can be checked

A copyist places the four salvage observations in their own dated box, away from the tape’s broad instruction. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The report lists antibiotics, iodine, gauze, and scissors, with conditions. Neither voice exists to lecture the player.

Field reporter: “The list is shorter than the shelves.”
Radio listener: “Because it is the list the report can verify.”
Field reporter: “The shelves do not acquire a count from the summary.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 096 — A list that can be checked

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The shelves do not acquire a count from the summary. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The shelves do not acquire a count from the summary.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Report summary retained with source and date.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 097 — The former worker field

A final-wish note names a pharmacy only through a placeholder and leaves the worker unnamed in this expansion. The existing step says to find the old pharmacy where {name} worked. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Could that have been their desk?”
Radio listener: “The step does not tell us which one.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The existing step says to find the old pharmacy where {name} worked. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The blank continues to belong to the player’s save. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 098 — The former worker field

Proposed diegetic text: “Former worker: {name}, as supplied by the owning quest. No name inferred here.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A final-wish note names a pharmacy only through a placeholder and leaves the worker unnamed in this expansion. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The existing step says to find the old pharmacy where {name} worked. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The blank continues to belong to the player’s save. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 099 — The former worker field

A final-wish note names a pharmacy only through a placeholder and leaves the worker unnamed in this expansion. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The existing step says to find the old pharmacy where {name} worked. Neither voice exists to lecture the player.

Field reporter: “Could that have been their desk?”
Radio listener: “The step does not tell us which one.”
Field reporter: “The blank continues to belong to the player’s save.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 100 — The former worker field

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The blank continues to belong to the player’s save. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The blank continues to belong to the player’s save.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Former worker: {name}, as supplied by the owning quest. No name inferred here.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 101 — A tape is not an inventory

The cassette completion can lead to its existing hidden cache, but this prose does not repeat or supplement the cache contents. The set’s hidden cache is already declared in the cassette catalog. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The tape gets someone to look.”
Radio listener: “The report says what the expedition found.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The set’s hidden cache is already declared in the cassette catalog. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The story keeps narrative discovery and item ownership separate. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 102 — A tape is not an inventory

Proposed diegetic text: “Cache behavior belongs to cassette playback; this plan adds none.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The cassette completion can lead to its existing hidden cache, but this prose does not repeat or supplement the cache contents. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The set’s hidden cache is already declared in the cassette catalog. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The story keeps narrative discovery and item ownership separate. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 103 — A tape is not an inventory

The cassette completion can lead to its existing hidden cache, but this prose does not repeat or supplement the cache contents. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The set’s hidden cache is already declared in the cassette catalog. Neither voice exists to lecture the player.

Field reporter: “The tape gets someone to look.”
Radio listener: “The report says what the expedition found.”
Field reporter: “The story keeps narrative discovery and item ownership separate.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 104 — A tape is not an inventory

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The story keeps narrative discovery and item ownership separate. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The story keeps narrative discovery and item ownership separate.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Cache behavior belongs to cassette playback; this plan adds none.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 105 — Different dates, different claims

The reader puts the tape date and field report day on opposite sides of a note, without declaring one a correction to the other. The sources have different provenance and timing. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “The words changed because the room changed.”
Radio listener: “Or because someone saw more.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The sources have different provenance and timing. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The later copy keeps both possible readings open. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 106 — Different dates, different claims

Proposed diegetic text: “Source comparison: recorded instruction / later expedition observation.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The reader puts the tape date and field report day on opposite sides of a note, without declaring one a correction to the other. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The sources have different provenance and timing. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later copy keeps both possible readings open. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 107 — Different dates, different claims

The reader puts the tape date and field report day on opposite sides of a note, without declaring one a correction to the other. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The sources have different provenance and timing. Neither voice exists to lecture the player.

Field reporter: “The words changed because the room changed.”
Radio listener: “Or because someone saw more.”
Field reporter: “The later copy keeps both possible readings open.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 108 — Different dates, different claims

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later copy keeps both possible readings open. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The later copy keeps both possible readings open.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Source comparison: recorded instruction / later expedition observation.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 109 — A warning carried without a cure

A team member copies the back-room caution onto a card and leaves out any medical recommendation not present in the report. The source says avoid entering without a mask; it does not describe treatment. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “A warning can be useful by itself.”
Radio listener: “It does not need to pretend to solve the room.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source says avoid entering without a mask; it does not describe treatment. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: No further clinical sentence is added on revisit. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 110 — A warning carried without a cure

Proposed diegetic text: “Hazard warning transcribed. Treatment field omitted.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A team member copies the back-room caution onto a card and leaves out any medical recommendation not present in the report. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source says avoid entering without a mask; it does not describe treatment. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No further clinical sentence is added on revisit. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 111 — A warning carried without a cure

A team member copies the back-room caution onto a card and leaves out any medical recommendation not present in the report. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The source says avoid entering without a mask; it does not describe treatment. Neither voice exists to lecture the player.

Field reporter: “A warning can be useful by itself.”
Radio listener: “It does not need to pretend to solve the room.”
Field reporter: “No further clinical sentence is added on revisit.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 112 — A warning carried without a cure

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No further clinical sentence is added on revisit. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “No further clinical sentence is added on revisit.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Hazard warning transcribed. Treatment field omitted.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 113 — The door after the report

A return reader finds the pharmacy door still described as intact in the location row, not as a door guaranteed open by the expedition. The location record and expedition report describe different content facts. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “It was open for her.”
Radio listener: “That does not tell us it is open now.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location record and expedition report describe different content facts. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A revisit uses the present owner, not this report. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 114 — The door after the report

Proposed diegetic text: “Present access: verify current location state. Door condition in source row retained.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A return reader finds the pharmacy door still described as intact in the location row, not as a door guaranteed open by the expedition. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location record and expedition report describe different content facts. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A revisit uses the present owner, not this report. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 115 — The door after the report

A return reader finds the pharmacy door still described as intact in the location row, not as a door guaranteed open by the expedition. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The location record and expedition report describe different content facts. Neither voice exists to lecture the player.

Field reporter: “It was open for her.”
Radio listener: “That does not tell us it is open now.”
Field reporter: “A revisit uses the present owner, not this report.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 116 — The door after the report

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A revisit uses the present owner, not this report. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A revisit uses the present owner, not this report.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Present access: verify current location state. Door condition in source row retained.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 117 — A voice for the next reader

A proposed return note addresses whoever finds the two records and tells them to read the source labels before carrying either sentence onward. The plan’s two records are not merged or canonized by being placed together. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Take the words seriously.”
Radio listener: “Take their dates seriously too.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The plan’s two records are not merged or canonized by being placed together. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The warning arrives without turning into universal distrust. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 118 — A voice for the next reader

Proposed diegetic text: “Read tape and field report as separate sources.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: A proposed return note addresses whoever finds the two records and tells them to read the source labels before carrying either sentence onward. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The plan’s two records are not merged or canonized by being placed together. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The warning arrives without turning into universal distrust. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 119 — A voice for the next reader

A proposed return note addresses whoever finds the two records and tells them to read the source labels before carrying either sentence onward. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The plan’s two records are not merged or canonized by being placed together. Neither voice exists to lecture the player.

Field reporter: “Take the words seriously.”
Radio listener: “Take their dates seriously too.”
Field reporter: “The warning arrives without turning into universal distrust.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 120 — A voice for the next reader

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The warning arrives without turning into universal distrust. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The warning arrives without turning into universal distrust.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Read tape and field report as separate sources.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 121 — One key, no new keeper

The key closes in a drawer in this draft. No new pharmacist appears to claim it or operate the location. The source says the key is behind the diploma; it does not name its current holder. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Someone left it for a finder.”
Radio listener: “The source does not say who found it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source says the key is behind the diploma; it does not name its current holder. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: The unknown holder remains unfilled. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 122 — One key, no new keeper

Proposed diegetic text: “Key-holder: unknown after recorded discovery.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The key closes in a drawer in this draft. No new pharmacist appears to claim it or operate the location. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source says the key is behind the diploma; it does not name its current holder. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The unknown holder remains unfilled. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 123 — One key, no new keeper

The key closes in a drawer in this draft. No new pharmacist appears to claim it or operate the location. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The source says the key is behind the diploma; it does not name its current holder. Neither voice exists to lecture the player.

Field reporter: “Someone left it for a finder.”
Radio listener: “The source does not say who found it.”
Field reporter: “The unknown holder remains unfilled.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 124 — One key, no new keeper

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The unknown holder remains unfilled. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “The unknown holder remains unfilled.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Key-holder: unknown after recorded discovery.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 125 — The instruction survives its limits

The closing scene lets the generous line remain legible while the report sits beside it, narrower and later. The tension is in the sources, not in a newly authored betrayal. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial.

Field reporter: “Take everything.”
Radio listener: “Read the report before you decide what that meant.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The tension is in the sources, not in a newly authored betrayal. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal.

Scene close: A later copy preserves the question instead of an easy answer. The report remains specific to the day it was written. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 126 — The instruction survives its limits

Proposed diegetic text: “Instruction and later report both retained with attribution.”

Proposed author and audience: Unidentified hospital worker (tape voice only); someone tracing the old evacuation call. The artifact exists in this proposal for a practical reason: The closing scene lets the generous line remain legible while the report sits beside it, narrower and later. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The tension is in the sources, not in a newly authored betrayal. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later copy preserves the question instead of an easy answer. A broad instruction is not a measurement of present condition. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 127 — The instruction survives its limits

The closing scene lets the generous line remain legible while the report sits beside it, narrower and later. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. The recorded doctor speaks in the clinical register already present in the tapes; no new recording is attributed to that voice. The field report is concise and sensory. The radio reader is alert to vocabulary but never portrayed as a universal detector of lies. Do not let characters list medical jargon to the player as a tutorial. Keep the conversation attached to the work already in the scene: The tension is in the sources, not in a newly authored betrayal. Neither voice exists to lecture the player.

Field reporter: “Take everything.”
Radio listener: “Read the report before you decide what that meant.”
Field reporter: “A later copy preserves the question instead of an easy answer.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 128 — The instruction survives its limits

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later copy preserves the question instead of an easy answer. One false call does not turn every call into a lie. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Field reporter: “A later copy preserves the question instead of an easy answer.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Instruction and later report both retained with attribution.”

Return boundary: Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. Candidate text can sit beside the existing pharmacy expedition, cassette completion, false-flag aftermath, or final-wish step only after the current text consumer is verified. These fragments do not unlock the cache, restock medicine, modify the radio trace, or alter an expedition outcome. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

## 15. Beat selection index

| Beat | Editorial focus | Candidate in-world artifact | Continuity limit |
|---:|---|---|---|
| 01 | Door behind the stair | Door intact. Stair access collapsed. No alternate entry recorded. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 02 | The shelf seen through the gap | Visible stock is not counted in this note. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 03 | The final tape begins | Tape set: part three. Key location described in source recording. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 04 | A diploma face down | Key location copied from the tape. Name of diploma holder not added. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 05 | The key and its wording | Tape wording retained as tape wording; present condition requires a later source. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 06 | Triage at six hours | Recorded: Day Zero plus six hours. No count copied. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 07 | The taped pause | Tape transcript note: pause retained; no motive added beyond the speaker’s words. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 08 | Warm air in the pharmacy | Cold-chain condition: failed in the observed report. Do not mark unspecified stock usable. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 09 | The hum is gone | Refrigeration hum absent at inspection. Cause not entered. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 10 | Sweetness from the back room | Back-room warning retained from source. No entry scene proposed. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 11 | A sealed course | Sealed item reported recovered; no treatment instruction in this expansion. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 12 | Half a bottle | Iodine — half full, as reported. Quantity not expanded. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 13 | Gauze still in wrappers | Three rolls, sealed, according to the Day 21 report. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 14 | Scissors on a cloth | One pair reported. No procedural use described. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 15 | The vaccines stay warm | Warm vaccines: do not use, as recorded in the report. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 16 | The back room is not entered | Back room not entered in the report. No further description added. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 17 | Casualties, not patients | Signal transcription: casualties and transport are source terms. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 18 | No names in the first fragment | Names: none in source fragment. Conditions: none in source fragment. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 19 | The clinic was already stripped | Clinic condition copied from the false-flag record only. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 20 | A trap after the response | Conditional radio callback only; no new ambush event. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 21 | The false flag is one record | 478.2 — one identified false flag. Scope not generalized. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 22 | The expedition leader’s name | Reporter: Mira. Team: one. Duration: one day. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 23 | The loading-bay approach | Approach quoted from completed report. Current access not inferred. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 24 | A list that can be checked | Report summary retained with source and date. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 25 | The former worker field | Former worker: {name}, as supplied by the owning quest. No name inferred here. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 26 | A tape is not an inventory | Cache behavior belongs to cassette playback; this plan adds none. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 27 | Different dates, different claims | Source comparison: recorded instruction / later expedition observation. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 28 | A warning carried without a cure | Hazard warning transcribed. Treatment field omitted. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 29 | The door after the report | Present access: verify current location state. Door condition in source row retained. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 30 | A voice for the next reader | Read tape and field report as separate sources. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 31 | One key, no new keeper | Key-holder: unknown after recorded discovery. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |
| 32 | The instruction survives its limits | Instruction and later report both retained with attribution. | Do not provide medical treatment or dosage instructions. Do not encourage entering the back room; the field report says not to. Do not present warm vaccines as usable. Preserve the distinction between sealed stable salvage and other stock. Do not write a new false flag, identify an unrecorded pharmacist, or generalize suspicion from one hostile signal. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions or state. The existence of a record in JSON does not prove its consumer. Verify the current owning system and its exposed state before choosing a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `hospital_pharmacy expedition` | current expedition and field report | Do not reroll, restock, or change the recorded salvage. The report remains a completed authored trip. |
| `freq_distress_478_2` | false-flag trace and ambush follow-up | Use only the existing radio outcome. Do not add a generic trust penalty to other broadcasts. |
| `hospital_saint_maren` | existing three-part cassette completion | Cache unlocking is already assigned to cassette playback; this plan does not add items or a second finale. |
| `reach_the_pharmacy` | placeholder character’s existing final-wish step | Preserve {name}; no identity is inferred for the former worker. |

## 17. Collision and unresolved authority

The tape says take everything; the later expedition report says leave the rest and identifies the broken cold chain. Keep both artifacts and their dates visible. The correct editorial handling is not to erase one: a player-facing plan should show why source, date, and condition matter. The audio completion cache remains the already-authored cache, not a reward proposed here. If a future implementation needs a single authoritative answer, pause content selection until the current owner resolves the source conflict. No text in this plan is that resolution.

## 18. Review checklist

Before selecting a passage, compare it again with the source rows named in section 3. Keep source facts and existing choices exact, mark proposed voices as editorial until approved, and independently verify any route, text consumer, or state condition. Do not infer reachability from a location description. Read every line aloud for role-appropriate vocabulary, remove any sentence that sounds like a feature spec, and keep each artifact’s author, audience, and purpose plausible.

## 19. Acceptance boundary

This plan is complete as a game-content proposal when selected passages can be traced to the cited location or linked records, existing choices and consequences remain unchanged, no unknown is promoted into canon, and an existing owner is identified for any implementation. If that owner or consumer is absent, retain the prose as a draft rather than inventing a route or system. Character counts are Unicode code-point counts of the saved Markdown files.
