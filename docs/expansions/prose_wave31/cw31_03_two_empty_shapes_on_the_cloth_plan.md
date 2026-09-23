# CONTENT EXPANSION CW31-03 — Two Empty Shapes on the Cloth

## A paired heirloom story about work remembered through separate objects.

### Prose Wave 31: Six Places, Six Kinds of Work

## Batch brief

**Content type:** prose-first playable-content expansion plan with scene drafts, diegetic records, conversation fragments, and conditional callbacks.
**Content bank:** 33 story beats with four alternative authored forms per beat (132 candidate passages).
**Current location anchor:** `ruined_garage` — Ruined Garage.
**Tone:** material, restrained, human, and careful with uncertainty.
**Canon sensitivity:** current location and linked authored records define known facts; old plans or JSON presence do not prove a current runtime route.
**Scope:** game-content plan and original prose drafts only; no production code, JSON, route, quest, flag, system, or save change.

## 1. Expansion thesis

The garage’s most intimate record is a cloth with tools outlined in marker, two empty shapes, and a half-rebuilt water pump still held in a vise. A field report from Mira describes an open door, a day’s work, and a tool roll; two separate final-wish records send other survivors looking for a father’s wrench and a grandfather’s soldering iron. These records do not say the wrench in the wish is one of the metric tools in Mira’s report, nor do they identify the person who left the outlines. The expansion holds the objects beside one another without merging their owners. The bank below is intended to produce playable narrative texture: a player can discover, compare, question, refuse, or return to a passage while the existing game systems continue to own state. The fragments are written as content, not as a feature roadmap.

## 2. Story question

When an object returns to a shelter, what part of its history should arrive with it?

## 3. Verified local anchor and source records

The location row gives the garage a half-gone roof, seized lifts, bare benches, unopened tool lockers, and an unopened pit. An atmosphere text describes the outlined tools, two empty shapes, and pump in a vise. Mira’s field report describes four metric wrenches, the half-rebuilt pump, oil, and a cloth roll. The final wishes separately name a nine-sixteenths wrench with inner-tube wrap and a brass soldering iron used to teach three people. Keep those source records distinct.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `ruined_garage` | roof, lifts, benches, lockers, pit |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` | `atm_env_storytelling_workbench` | tool outlines, two missing shapes, pump in vise |
| `Assets/StreamingAssets/Data/narrative/field_reports_expansion.json` | `exp_report_garage_tools` | Mira’s one-person report and recorded salvage |
| `Assets/StreamingAssets/Data/final_wishes.json` | `wish_mechanic_lost_wrench` | specific family wrench and existing retrieve-wish outcome |
| `Assets/StreamingAssets/Data/final_wishes.json` | `wish_electrician_soldering_iron` | grandfather’s iron and existing teaching outcome |
| `Assets/StreamingAssets/Data/expeditions.json` | `ruined_garage` | current expedition record; do not infer a new route or risk |

## 4. Fixed canon and open space

The garage description, atmosphere observation, expedition report, and final wishes are not a single continuous inventory. Mira’s four metric wrenches do not automatically satisfy the separate nine-sixteenths heirloom wish. The pump, its missing gasket and valve, the oil, and the two missing tool shapes retain only their source-specific descriptions. Existing wish completion text and morale outcomes remain owned by final_wishes.json. Existing characters retain their authored identity, boundaries, and outcomes. New working voices remain editorial until an existing content owner approves them. Never fill a source gap solely to make the scene resolve.

## 5. Human center

The center is two different people asking for two different tools, and a third person who recorded an organized bench without knowing whether the absent owner would return. No object stands for every worker. One wish leads back to a family tool; another leads toward a lesson given to a fourth person. The prose stays with use, memory, and the limits of a field report. The emotional pressure should come through what a person records, omits, repairs, asks, or leaves unsigned rather than a narrator naming what the location means.

## 6. Voice and point of view

Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep each author’s knowledge local. A field note cannot know what an unnamed visitor thought; a later reader cannot recover a date that was never recorded; a title cannot create a route. Vary sentence length and register by document purpose, not by changing established facts.

## 7. Placement and current reachability

Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. All fragments are candidates. None is evidence that the location is currently player-reachable. Before selecting any text, verify the current map/location owner, content schema, actual consumer, and the source condition under which the text can appear.

## 8. Player agency

The player may read Mira’s report, accept an existing wish, or leave the garage. The two wishes remain separate and retain their current steps. The player is not asked to decide which survivor deserves an heirloom. The prose does not turn a repairable object into a new crafting quest. Do not add a moral-choice menu simply to make quiet prose interactive. Preserve the player’s refusal, ability to leave, and the authority of the characters who own their words.

## 9. Continuity, dignity, and safety

Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Keep the setting fictional and physically grounded. No absent person receives a biography merely to heighten emotion. Technical and medical context remains descriptive and non-instructional.

## 10. Existing hooks and implementation boundary

**Existing content anchors:** the location row and linked records listed above. **Unverified:** any route, text consumer, state condition, dialogue surface, or return trigger not explicitly established by current authority. **Classification:** editorial game-content proposal; no implementation category is claimed until an owner and consumer are confirmed.

This plan changes no production code or game data. Do not add a parallel discovery registry, route, save section, or gameplay authority to host these drafts. Where a passage reflects a branch, use only the existing state named in section 16 and only after its current owner confirms the consumer.

## 11. Narrative sequence

### 1. Disturbance — the empty outlines

Begin at the bench and name the two absent shapes without filling them from the wish records.

### 2. Discovery — a report with its own list

Mira’s findings are copied exactly as her one-day expedition report, not as the garage’s complete inventory.

### 3. Interpretation — two separate wishes

The mechanic’s wrench and electrician’s iron carry different family histories and outcomes.

### 4. Complication — resemblance invites a false match

The metric set and the nine-sixteenths wrench sound close enough to confuse. The story makes the gap visible.

### 5. Choice — carry the right record

The player follows only the current wish step they accepted; the expansion adds no third object or condition.

### 6. Callback — the fourth learner

The existing iron wish already describes teaching a fourth person. Let that authored outcome close the second thread without moving the first.

## 12. Creative variants

### Grounded

Use the tool cloth, Mira’s workbench report, and two short wish excerpts with their original owners kept separate.

### Interlinked

A later paragraph may juxtapose the report and a wish only if the content owner supplies both authorized sources on that surface.

### Wild card

Let the narrative move among the empty marker outlines, the report list, and the wish request, making each author’s knowledge boundary part of the scene.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not four required encounters. Scene drafts stage an observation; record drafts give it a plausible author and audience; conversation fragments expose a practical point of friction; consequence vignettes allow a later reader to recognize changed context. Select only forms that fit an existing content owner.

A selected fragment should answer who made it, why they made it, who might read it, and what the author cannot know. Keep objects specific to this location. Let a line do practical work before it carries a theme. Remove exposition that a worker would not say aloud, repeated catastrophe language, unearned revelation, and wording that could be mistaken for a new mechanic. Where existing game state matters, state it in the source owner’s terms and do not duplicate its authority.

## 14. Content bank

### Scene draft 001 — The cloth before the list

A reader kneels beside the workbench and sees tools outlined in marker. Two shapes are empty; the scene does not compare them to any wish. The atmosphere record gives the tool roll and two blanks. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Which ones are gone?”
An unnamed wish-giver: “The note does not say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere record gives the tool roll and two blanks. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No wish item is assigned to the outlines. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 002 — The cloth before the list

Proposed diegetic text: “Tool shapes: two empty; identities not entered.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A reader kneels beside the workbench and sees tools outlined in marker. Two shapes are empty; the scene does not compare them to any wish. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere record gives the tool roll and two blanks. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No wish item is assigned to the outlines. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 003 — The cloth before the list

A reader kneels beside the workbench and sees tools outlined in marker. Two shapes are empty; the scene does not compare them to any wish. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The atmosphere record gives the tool roll and two blanks. Neither voice exists to lecture the player.

Mira the Scavenger: “Which ones are gone?”
An unnamed wish-giver: “The note does not say.”
Mira the Scavenger: “No wish item is assigned to the outlines.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 004 — The cloth before the list

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No wish item is assigned to the outlines. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No wish item is assigned to the outlines.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Tool shapes: two empty; identities not entered.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 005 — The vise still holds

The pump remains in the vise in the atmosphere text, and the draft looks at the clamp before it looks for a person. The atmosphere entry says the pump is half rebuilt and the vise is still tight. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can it be finished?”
An unnamed wish-giver: “The record stops before that answer.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere entry says the pump is half rebuilt and the vise is still tight. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No repair result is promised. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 006 — The vise still holds

Proposed diegetic text: “Pump in vise; status as observed.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The pump remains in the vise in the atmosphere text, and the draft looks at the clamp before it looks for a person. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere entry says the pump is half rebuilt and the vise is still tight. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No repair result is promised. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 007 — The vise still holds

The pump remains in the vise in the atmosphere text, and the draft looks at the clamp before it looks for a person. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The atmosphere entry says the pump is half rebuilt and the vise is still tight. Neither voice exists to lecture the player.

Mira the Scavenger: “Can it be finished?”
An unnamed wish-giver: “The record stops before that answer.”
Mira the Scavenger: “No repair result is promised.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 008 — The vise still holds

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No repair result is promised. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No repair result is promised.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Pump in vise; status as observed.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 009 — Mira’s open door

Mira’s report begins at the alley approach and records the garage door open. The scene keeps her one-day observation apart from later wishes. The expedition leader and report ID are authored in the field-report catalog. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Was it open before?”
An unnamed wish-giver: “Mira records what she found.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The expedition leader and report ID are authored in the field-report catalog. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The report does not become a global access rule. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 010 — Mira’s open door

Proposed diegetic text: “Door open in report context; no permanent state.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: Mira’s report begins at the alley approach and records the garage door open. The scene keeps her one-day observation apart from later wishes. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The expedition leader and report ID are authored in the field-report catalog. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The report does not become a global access rule. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 011 — Mira’s open door

Mira’s report begins at the alley approach and records the garage door open. The scene keeps her one-day observation apart from later wishes. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The expedition leader and report ID are authored in the field-report catalog. Neither voice exists to lecture the player.

Mira the Scavenger: “Was it open before?”
An unnamed wish-giver: “Mira records what she found.”
Mira the Scavenger: “The report does not become a global access rule.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 012 — Mira’s open door

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The report does not become a global access rule. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The report does not become a global access rule.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Door open in report context; no permanent state.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 013 — Four metric wrenches

A field-list copy names four metric pieces in good condition. A separate wish for a family wrench remains on another page. The report’s inventory differs from the nine-sixteenths heirloom description. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Is one of these the old wrench?”
An unnamed wish-giver: “The records do not match them.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The report’s inventory differs from the nine-sixteenths heirloom description. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No item substitution is authored. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 014 — Four metric wrenches

Proposed diegetic text: “Four metric wrenches, per report.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A field-list copy names four metric pieces in good condition. A separate wish for a family wrench remains on another page. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The report’s inventory differs from the nine-sixteenths heirloom description. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No item substitution is authored. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 015 — Four metric wrenches

A field-list copy names four metric pieces in good condition. A separate wish for a family wrench remains on another page. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The report’s inventory differs from the nine-sixteenths heirloom description. Neither voice exists to lecture the player.

Mira the Scavenger: “Is one of these the old wrench?”
An unnamed wish-giver: “The records do not match them.”
Mira the Scavenger: “No item substitution is authored.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 016 — Four metric wrenches

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No item substitution is authored. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No item substitution is authored.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Four metric wrenches, per report.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 017 — Nine-sixteenths on the wish card

The mechanic’s wish describes one adjustable wrench and an inner-tube wrap. The scene does not place it on the field-report cloth. The wish record gives a size, jaw, wrap, shelf, and search clue. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “You know exactly what it looks like?”
An unnamed wish-giver: “That is what the wish remembers.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The wish record gives a size, jaw, wrap, shelf, and search clue. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No new object location is selected. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 018 — Nine-sixteenths on the wish card

Proposed diegetic text: “Wish description only; physical recovery remains existing content.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The mechanic’s wish describes one adjustable wrench and an inner-tube wrap. The scene does not place it on the field-report cloth. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The wish record gives a size, jaw, wrap, shelf, and search clue. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new object location is selected. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 019 — Nine-sixteenths on the wish card

The mechanic’s wish describes one adjustable wrench and an inner-tube wrap. The scene does not place it on the field-report cloth. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The wish record gives a size, jaw, wrap, shelf, and search clue. Neither voice exists to lecture the player.

Mira the Scavenger: “You know exactly what it looks like?”
An unnamed wish-giver: “That is what the wish remembers.”
Mira the Scavenger: “No new object location is selected.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 020 — Nine-sixteenths on the wish card

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new object location is selected. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No new object location is selected.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Wish description only; physical recovery remains existing content.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 021 — The wrap in the sentence

The old inner-tube strips become a detail in a spoken request, not a crafting specification. The wish’s text says the wrap has rotted away in places at completion. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Does it still feel the same?”
An unnamed wish-giver: “The record says what changed.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The wish’s text says the wrap has rotted away in places at completion. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No new condition state is set. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 022 — The wrap in the sentence

Proposed diegetic text: “Existing final-wish detail only.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The old inner-tube strips become a detail in a spoken request, not a crafting specification. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The wish’s text says the wrap has rotted away in places at completion. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new condition state is set. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 023 — The wrap in the sentence

The old inner-tube strips become a detail in a spoken request, not a crafting specification. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The wish’s text says the wrap has rotted away in places at completion. Neither voice exists to lecture the player.

Mira the Scavenger: “Does it still feel the same?”
An unnamed wish-giver: “The record says what changed.”
Mira the Scavenger: “No new condition state is set.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 024 — The wrap in the sentence

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new condition state is set. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No new condition state is set.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Existing final-wish detail only.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 025 — A shelf behind paint tins

The player reaches the detail in the existing wish without the expansion drawing a map through the ruined house. The wish gives a second shelf and left side behind paint tins. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “What if the roof fell?”
An unnamed wish-giver: “The wish says to look anyway.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The wish gives a second shelf and left side behind paint tins. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No new route or search mechanic. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 026 — A shelf behind paint tins

Proposed diegetic text: “Clue attributed to the wish, not a navigation guide.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The player reaches the detail in the existing wish without the expansion drawing a map through the ruined house. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The wish gives a second shelf and left side behind paint tins. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new route or search mechanic. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 027 — A shelf behind paint tins

The player reaches the detail in the existing wish without the expansion drawing a map through the ruined house. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The wish gives a second shelf and left side behind paint tins. Neither voice exists to lecture the player.

Mira the Scavenger: “What if the roof fell?”
An unnamed wish-giver: “The wish says to look anyway.”
Mira the Scavenger: “No new route or search mechanic.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 028 — A shelf behind paint tins

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new route or search mechanic. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No new route or search mechanic.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Clue attributed to the wish, not a navigation guide.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 029 — The soldering iron kept warm

A different page describes a heavy brass iron, black at the tip. The draft refuses to place it beside the wrench simply because both are tools. The electrician’s wish has its own family history and object. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Is this the same bench?”
An unnamed wish-giver: “The source does not say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The electrician’s wish has its own family history and object. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No shared ownership is inferred. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 030 — The soldering iron kept warm

Proposed diegetic text: “Iron belongs to that wish only.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A different page describes a heavy brass iron, black at the tip. The draft refuses to place it beside the wrench simply because both are tools. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The electrician’s wish has its own family history and object. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No shared ownership is inferred. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 031 — The soldering iron kept warm

A different page describes a heavy brass iron, black at the tip. The draft refuses to place it beside the wrench simply because both are tools. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The electrician’s wish has its own family history and object. Neither voice exists to lecture the player.

Mira the Scavenger: “Is this the same bench?”
An unnamed wish-giver: “The source does not say.”
Mira the Scavenger: “No shared ownership is inferred.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 032 — The soldering iron kept warm

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No shared ownership is inferred. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No shared ownership is inferred.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Iron belongs to that wish only.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 033 — A fourth learner

The existing wish’s ending turns from the tool toward a younger survivor being taught. The expansion names no new student and no extra lesson. The final-wish completion text says the iron has taught four people. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “How many did it teach?”
An unnamed wish-giver: “The existing line says four.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The final-wish completion text says the iron has taught four people. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No skill gain is restated as a system. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 034 — A fourth learner

Proposed diegetic text: “Outcome already authored; no progression added.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The existing wish’s ending turns from the tool toward a younger survivor being taught. The expansion names no new student and no extra lesson. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The final-wish completion text says the iron has taught four people. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No skill gain is restated as a system. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 035 — A fourth learner

The existing wish’s ending turns from the tool toward a younger survivor being taught. The expansion names no new student and no extra lesson. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The final-wish completion text says the iron has taught four people. Neither voice exists to lecture the player.

Mira the Scavenger: “How many did it teach?”
An unnamed wish-giver: “The existing line says four.”
Mira the Scavenger: “No skill gain is restated as a system.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 036 — A fourth learner

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No skill gain is restated as a system. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No skill gain is restated as a system.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Outcome already authored; no progression added.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 037 — The report’s half-full oil

Mira’s note lists half a tin of machine oil. The player can read the quantity without being asked to infer what work remains. This is one salvage item from her report. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can we use it on the pump?”
An unnamed wish-giver: “The field report does not answer that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: This is one salvage item from her report. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No recipe is created. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 038 — The report’s half-full oil

Proposed diegetic text: “Reported quantity, no current inventory claim.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: Mira’s note lists half a tin of machine oil. The player can read the quantity without being asked to infer what work remains. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: This is one salvage item from her report. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No recipe is created. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 039 — The report’s half-full oil

Mira’s note lists half a tin of machine oil. The player can read the quantity without being asked to infer what work remains. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: This is one salvage item from her report. Neither voice exists to lecture the player.

Mira the Scavenger: “Can we use it on the pump?”
An unnamed wish-giver: “The field report does not answer that.”
Mira the Scavenger: “No recipe is created.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 040 — The report’s half-full oil

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No recipe is created. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No recipe is created.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Reported quantity, no current inventory claim.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 041 — The pump needs two things

The report names a gasket and a valve as missing from the half-rebuilt pump. The draft does not describe how to fit either. The specific repair note is in Mira’s report. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Which one should we find?”
An unnamed wish-giver: “That is not this report’s question.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The specific repair note is in Mira’s report. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No ingredient or quest chain is authored. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 042 — The pump needs two things

Proposed diegetic text: “Missing parts as report text; no crafting steps.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The report names a gasket and a valve as missing from the half-rebuilt pump. The draft does not describe how to fit either. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The specific repair note is in Mira’s report. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No ingredient or quest chain is authored. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 043 — The pump needs two things

The report names a gasket and a valve as missing from the half-rebuilt pump. The draft does not describe how to fit either. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The specific repair note is in Mira’s report. Neither voice exists to lecture the player.

Mira the Scavenger: “Which one should we find?”
An unnamed wish-giver: “That is not this report’s question.”
Mira the Scavenger: “No ingredient or quest chain is authored.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 044 — The pump needs two things

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No ingredient or quest chain is authored. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No ingredient or quest chain is authored.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Missing parts as report text; no crafting steps.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 045 — The vise and the absent owner

The atmosphere sentence says he meant to come back, then says he did not. The scene does not replace the absence with a name or cause. The source’s grammar marks an unnamed man but gives no identity. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Did he leave the tools?”
An unnamed wish-giver: “The record only tells you they stayed.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source’s grammar marks an unnamed man but gives no identity. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No biography is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 046 — The vise and the absent owner

Proposed diegetic text: “Identity and fate remain unknown.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The atmosphere sentence says he meant to come back, then says he did not. The scene does not replace the absence with a name or cause. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source’s grammar marks an unnamed man but gives no identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No biography is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 047 — The vise and the absent owner

The atmosphere sentence says he meant to come back, then says he did not. The scene does not replace the absence with a name or cause. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The source’s grammar marks an unnamed man but gives no identity. Neither voice exists to lecture the player.

Mira the Scavenger: “Did he leave the tools?”
An unnamed wish-giver: “The record only tells you they stayed.”
Mira the Scavenger: “No biography is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 048 — The vise and the absent owner

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No biography is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No biography is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Identity and fate remain unknown.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 049 — A cloth that survived an inventory

Mira calls the shapes an inventory of what was here. A later reader can disagree about whether an empty shape is proof of removal. The report names the tool roll as a map of the missing. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “A missing shape means somebody took it.”
An unnamed wish-giver: “It means the shape is empty now.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The report names the tool roll as a map of the missing. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No culprit or theft is implied. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 050 — A cloth that survived an inventory

Proposed diegetic text: “The report’s interpretation stays attributed to Mira.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: Mira calls the shapes an inventory of what was here. A later reader can disagree about whether an empty shape is proof of removal. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The report names the tool roll as a map of the missing. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No culprit or theft is implied. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 051 — A cloth that survived an inventory

Mira calls the shapes an inventory of what was here. A later reader can disagree about whether an empty shape is proof of removal. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The report names the tool roll as a map of the missing. Neither voice exists to lecture the player.

Mira the Scavenger: “A missing shape means somebody took it.”
An unnamed wish-giver: “It means the shape is empty now.”
Mira the Scavenger: “No culprit or theft is implied.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 052 — A cloth that survived an inventory

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No culprit or theft is implied. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No culprit or theft is implied.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “The report’s interpretation stays attributed to Mira.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 053 — Four pieces and one request

The field report count and the family wish are set side by side only as separate records. Their similarity does not establish that the wrench survived. One source lists four metric wrenches; another asks for a specific imperial measure. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Could one be the one?”
An unnamed wish-giver: “The record does not let us say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: One source lists four metric wrenches; another asks for a specific imperial measure. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The uncertainty survives the callback. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 054 — Four pieces and one request

Proposed diegetic text: “Comparison is not identity.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The field report count and the family wish are set side by side only as separate records. Their similarity does not establish that the wrench survived. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: One source lists four metric wrenches; another asks for a specific imperial measure. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The uncertainty survives the callback. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 055 — Four pieces and one request

The field report count and the family wish are set side by side only as separate records. Their similarity does not establish that the wrench survived. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: One source lists four metric wrenches; another asks for a specific imperial measure. Neither voice exists to lecture the player.

Mira the Scavenger: “Could one be the one?”
An unnamed wish-giver: “The record does not let us say.”
Mira the Scavenger: “The uncertainty survives the callback.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 056 — Four pieces and one request

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The uncertainty survives the callback. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The uncertainty survives the callback.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Comparison is not identity.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 057 — Mira leaves the bench

The one-person report names Mira as expedition leader; the garage scene does not invent a conversation between her and either wish-giver. The field report’s author/leader is explicit. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Did she know them?”
An unnamed wish-giver: “This record does not tell us.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The field report’s author/leader is explicit. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No relationship is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 058 — Mira leaves the bench

Proposed diegetic text: “Mira is not a wish owner by this evidence.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The one-person report names Mira as expedition leader; the garage scene does not invent a conversation between her and either wish-giver. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The field report’s author/leader is explicit. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No relationship is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 059 — Mira leaves the bench

The one-person report names Mira as expedition leader; the garage scene does not invent a conversation between her and either wish-giver. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The field report’s author/leader is explicit. Neither voice exists to lecture the player.

Mira the Scavenger: “Did she know them?”
An unnamed wish-giver: “This record does not tell us.”
Mira the Scavenger: “No relationship is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 060 — Mira leaves the bench

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No relationship is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No relationship is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Mira is not a wish owner by this evidence.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 061 — The lift that will not lift

The location says the lifts are seized. The scene notices their silent shape but offers no method for moving them. The physical fact belongs to locations.json. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can we free it?”
An unnamed wish-giver: “The location does not give a procedure.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The physical fact belongs to locations.json. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No repair advice is written. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 062 — The lift that will not lift

Proposed diegetic text: “Lift condition recorded in source terms.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The location says the lifts are seized. The scene notices their silent shape but offers no method for moving them. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The physical fact belongs to locations.json. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No repair advice is written. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 063 — The lift that will not lift

The location says the lifts are seized. The scene notices their silent shape but offers no method for moving them. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The physical fact belongs to locations.json. Neither voice exists to lecture the player.

Mira the Scavenger: “Can we free it?”
An unnamed wish-giver: “The location does not give a procedure.”
Mira the Scavenger: “No repair advice is written.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 064 — The lift that will not lift

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No repair advice is written. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No repair advice is written.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Lift condition recorded in source terms.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 065 — The locker remains unopened

The location says the tool lockers were never opened. The expansion does not turn that into a particular cache contents list. The row establishes the locker status and nothing more. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “What is in there?”
An unnamed wish-giver: “The source has not looked inside.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The row establishes the locker status and nothing more. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No loot table is invented. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 066 — The locker remains unopened

Proposed diegetic text: “Contents unknown.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The location says the tool lockers were never opened. The expansion does not turn that into a particular cache contents list. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The row establishes the locker status and nothing more. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No loot table is invented. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 067 — The locker remains unopened

The location says the tool lockers were never opened. The expansion does not turn that into a particular cache contents list. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The row establishes the locker status and nothing more. Neither voice exists to lecture the player.

Mira the Scavenger: “What is in there?”
An unnamed wish-giver: “The source has not looked inside.”
Mira the Scavenger: “No loot table is invented.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 068 — The locker remains unopened

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No loot table is invented. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No loot table is invented.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Contents unknown.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 069 — The pit beneath the work floor

A line mentions the pit as an unopened place. The scene does not encourage entry or describe a descent. The location names the pit but no safe route. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can we go below?”
An unnamed wish-giver: “This page does not answer that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location names the pit but no safe route. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No route or hazard override is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 070 — The pit beneath the work floor

Proposed diegetic text: “Pit access not proposed.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A line mentions the pit as an unopened place. The scene does not encourage entry or describe a descent. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location names the pit but no safe route. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No route or hazard override is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 071 — The pit beneath the work floor

A line mentions the pit as an unopened place. The scene does not encourage entry or describe a descent. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The location names the pit but no safe route. Neither voice exists to lecture the player.

Mira the Scavenger: “Can we go below?”
An unnamed wish-giver: “This page does not answer that.”
Mira the Scavenger: “No route or hazard override is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 072 — The pit beneath the work floor

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No route or hazard override is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No route or hazard override is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Pit access not proposed.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 073 — Roof half gone

A strip of grey light enters where the roof is missing. This is a proposed moment, not proof of current weather or structural stability. The location describes a roof half gone; the report separately recorded the garage sound on one expedition. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Does it keep the rain out?”
An unnamed wish-giver: “The records do not promise that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location describes a roof half gone; the report separately recorded the garage sound on one expedition. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No shelter status is created. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 074 — Roof half gone

Proposed diegetic text: “Light is scene staging; safety not inferred.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A strip of grey light enters where the roof is missing. This is a proposed moment, not proof of current weather or structural stability. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location describes a roof half gone; the report separately recorded the garage sound on one expedition. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No shelter status is created. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 075 — Roof half gone

A strip of grey light enters where the roof is missing. This is a proposed moment, not proof of current weather or structural stability. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The location describes a roof half gone; the report separately recorded the garage sound on one expedition. Neither voice exists to lecture the player.

Mira the Scavenger: “Does it keep the rain out?”
An unnamed wish-giver: “The records do not promise that.”
Mira the Scavenger: “No shelter status is created.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 076 — Roof half gone

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No shelter status is created. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No shelter status is created.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Light is scene staging; safety not inferred.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 077 — Bare benches

A bench has enough room for a paper list and not much else. The scene keeps the bare benches in view rather than decorating them with new tools. Location description says benches are bare. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Someone could work here.”
An unnamed wish-giver: “Someone did, according to the records.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Location description says benches are bare. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No workstation action is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 078 — Bare benches

Proposed diegetic text: “No new bench inventory.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A bench has enough room for a paper list and not much else. The scene keeps the bare benches in view rather than decorating them with new tools. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Location description says benches are bare. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No workstation action is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 079 — Bare benches

A bench has enough room for a paper list and not much else. The scene keeps the bare benches in view rather than decorating them with new tools. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Location description says benches are bare. Neither voice exists to lecture the player.

Mira the Scavenger: “Someone could work here.”
An unnamed wish-giver: “Someone did, according to the records.”
Mira the Scavenger: “No workstation action is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 080 — Bare benches

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No workstation action is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No workstation action is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No new bench inventory.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 081 — The wish returns with a name token

The request keeps its {name} placeholder until the existing wish surface supplies the owner’s name. The final-wishes source formats the speaker through a token. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can the page be signed?”
An unnamed wish-giver: “Only the existing character surface can answer.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The final-wishes source formats the speaker through a token. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No named NPC is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 082 — The wish returns with a name token

Proposed diegetic text: “Token remains owner-supplied.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The request keeps its {name} placeholder until the existing wish surface supplies the owner’s name. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The final-wishes source formats the speaker through a token. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No named NPC is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 083 — The wish returns with a name token

The request keeps its {name} placeholder until the existing wish surface supplies the owner’s name. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The final-wishes source formats the speaker through a token. Neither voice exists to lecture the player.

Mira the Scavenger: “Can the page be signed?”
An unnamed wish-giver: “Only the existing character surface can answer.”
Mira the Scavenger: “No named NPC is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 084 — The wish returns with a name token

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No named NPC is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No named NPC is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Token remains owner-supplied.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 085 — The family line stops at the wrench

The wish says father and grandfather before the war. The expansion does not add another generation or explain where the wrench came from earlier. Family history is exactly as stated by the wish. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Was it always in the garage?”
An unnamed wish-giver: “That is what the request points to.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Family history is exactly as stated by the wish. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No pre-war scene is invented. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 086 — The family line stops at the wrench

Proposed diegetic text: “No ancestor names added.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The wish says father and grandfather before the war. The expansion does not add another generation or explain where the wrench came from earlier. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Family history is exactly as stated by the wish. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No pre-war scene is invented. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 087 — The family line stops at the wrench

The wish says father and grandfather before the war. The expansion does not add another generation or explain where the wrench came from earlier. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Family history is exactly as stated by the wish. Neither voice exists to lecture the player.

Mira the Scavenger: “Was it always in the garage?”
An unnamed wish-giver: “That is what the request points to.”
Mira the Scavenger: “No pre-war scene is invented.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 088 — The family line stops at the wrench

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No pre-war scene is invented. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No pre-war scene is invented.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No ancestor names added.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 089 — A jaw stiff in the existing ending

The wish’s completion already says the jaw is stiff and the inner-tube wrap has rotted in places. The proposal keeps that outcome in the wish’s owner. Completion text remains canonical to the wish. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Did it work?”
An unnamed wish-giver: “The wish record already answers.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Completion text remains canonical to the wish. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No alternate durability is authored. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 090 — A jaw stiff in the existing ending

Proposed diegetic text: “Callback only after existing completion.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The wish’s completion already says the jaw is stiff and the inner-tube wrap has rotted in places. The proposal keeps that outcome in the wish’s owner. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Completion text remains canonical to the wish. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No alternate durability is authored. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 091 — A jaw stiff in the existing ending

The wish’s completion already says the jaw is stiff and the inner-tube wrap has rotted in places. The proposal keeps that outcome in the wish’s owner. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Completion text remains canonical to the wish. Neither voice exists to lecture the player.

Mira the Scavenger: “Did it work?”
An unnamed wish-giver: “The wish record already answers.”
Mira the Scavenger: “No alternate durability is authored.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 092 — A jaw stiff in the existing ending

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No alternate durability is authored. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No alternate durability is authored.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Callback only after existing completion.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 093 — The line of the iron’s teaching

The electrician’s story ends with a fourth learner in the source. The expansion does not extend that count to a new class or craft system. The current wish completion gives the number and response. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Will there be a fifth?”
An unnamed wish-giver: “The record does not say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The current wish completion gives the number and response. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No future student is promised. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 094 — The line of the iron’s teaching

Proposed diegetic text: “Existing outcome only.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The electrician’s story ends with a fourth learner in the source. The expansion does not extend that count to a new class or craft system. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The current wish completion gives the number and response. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No future student is promised. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 095 — The line of the iron’s teaching

The electrician’s story ends with a fourth learner in the source. The expansion does not extend that count to a new class or craft system. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The current wish completion gives the number and response. Neither voice exists to lecture the player.

Mira the Scavenger: “Will there be a fifth?”
An unnamed wish-giver: “The record does not say.”
Mira the Scavenger: “No future student is promised.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 096 — The line of the iron’s teaching

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No future student is promised. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No future student is promised.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Existing outcome only.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 097 — A metric list in a different hand

The expedition report is read as a report, not as the garage’s full inventory. Its four tools have a date and author context. Mira completed this field report on Day 7. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Could the list be older?”
An unnamed wish-giver: “We only have this report.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Mira completed this field report on Day 7. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No catalog update is implied. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 098 — A metric list in a different hand

Proposed diegetic text: “Report scope remains one expedition.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The expedition report is read as a report, not as the garage’s full inventory. Its four tools have a date and author context. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Mira completed this field report on Day 7. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No catalog update is implied. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 099 — A metric list in a different hand

The expedition report is read as a report, not as the garage’s full inventory. Its four tools have a date and author context. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Mira completed this field report on Day 7. Neither voice exists to lecture the player.

Mira the Scavenger: “Could the list be older?”
An unnamed wish-giver: “We only have this report.”
Mira the Scavenger: “No catalog update is implied.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 100 — A metric list in a different hand

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No catalog update is implied. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No catalog update is implied.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Report scope remains one expedition.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 101 — The missing shapes at noon

A reader returns to the cloth after comparing the two wishes and still cannot label either blank. The atmosphere record does not identify the two absent tools. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Now do we know?”
An unnamed wish-giver: “No.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere record does not identify the two absent tools. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The comparison does not resolve them. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 102 — The missing shapes at noon

Proposed diegetic text: “Blank shapes remain blank.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A reader returns to the cloth after comparing the two wishes and still cannot label either blank. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere record does not identify the two absent tools. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The comparison does not resolve them. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 103 — The missing shapes at noon

A reader returns to the cloth after comparing the two wishes and still cannot label either blank. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The atmosphere record does not identify the two absent tools. Neither voice exists to lecture the player.

Mira the Scavenger: “Now do we know?”
An unnamed wish-giver: “No.”
Mira the Scavenger: “The comparison does not resolve them.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 104 — The missing shapes at noon

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The comparison does not resolve them. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The comparison does not resolve them.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Blank shapes remain blank.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 105 — The empty space for a receipt

A proposed copyist leaves room to note which source described an item. No universal garage receipt is asserted. The local files do not define a shared artifact schema for this content. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Where do I write the source?”
An unnamed wish-giver: “Beside the sentence you copied.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The local files do not define a shared artifact schema for this content. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No record type is registered. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 106 — The empty space for a receipt

Proposed diegetic text: “Editorial copy only.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A proposed copyist leaves room to note which source described an item. No universal garage receipt is asserted. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The local files do not define a shared artifact schema for this content. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No record type is registered. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 107 — The empty space for a receipt

A proposed copyist leaves room to note which source described an item. No universal garage receipt is asserted. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The local files do not define a shared artifact schema for this content. Neither voice exists to lecture the player.

Mira the Scavenger: “Where do I write the source?”
An unnamed wish-giver: “Beside the sentence you copied.”
Mira the Scavenger: “No record type is registered.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 108 — The empty space for a receipt

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No record type is registered. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No record type is registered.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Editorial copy only.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 109 — The pump stays in its vise

The closing image returns to the clamp. No new owner arrives to finish the work, and the plan does not claim the pump is irrecoverable. The atmosphere text records the vise still tight. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Should we take it out?”
An unnamed wish-giver: “The source does not decide for you.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The atmosphere text records the vise still tight. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The scene leaves the object in its recorded posture. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 110 — The pump stays in its vise

Proposed diegetic text: “No completion inferred.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The closing image returns to the clamp. No new owner arrives to finish the work, and the plan does not claim the pump is irrecoverable. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The atmosphere text records the vise still tight. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The scene leaves the object in its recorded posture. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 111 — The pump stays in its vise

The closing image returns to the clamp. No new owner arrives to finish the work, and the plan does not claim the pump is irrecoverable. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The atmosphere text records the vise still tight. Neither voice exists to lecture the player.

Mira the Scavenger: “Should we take it out?”
An unnamed wish-giver: “The source does not decide for you.”
Mira the Scavenger: “The scene leaves the object in its recorded posture.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 112 — The pump stays in its vise

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The scene leaves the object in its recorded posture. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The scene leaves the object in its recorded posture.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No completion inferred.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 113 — Two pages, no merge

A reader puts the mechanic’s wish and electrician’s wish beside Mira’s report, then keeps the three headings separate. These are distinct current records. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “They are all about tools.”
An unnamed wish-giver: “That does not make them one story.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: These are distinct current records. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The file names remain separate. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 114 — Two pages, no merge

Proposed diegetic text: “Source separation visible.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: A reader puts the mechanic’s wish and electrician’s wish beside Mira’s report, then keeps the three headings separate. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: These are distinct current records. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The file names remain separate. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 115 — Two pages, no merge

A reader puts the mechanic’s wish and electrician’s wish beside Mira’s report, then keeps the three headings separate. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: These are distinct current records. Neither voice exists to lecture the player.

Mira the Scavenger: “They are all about tools.”
An unnamed wish-giver: “That does not make them one story.”
Mira the Scavenger: “The file names remain separate.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 116 — Two pages, no merge

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The file names remain separate. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The file names remain separate.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Source separation visible.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 117 — A tool roll as a map

Mira’s report calls the roll a map of what was here. This passage treats that as her metaphor, not a geometric route. The description of the tool roll is in the report. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Can I follow it?”
An unnamed wish-giver: “Only as a list of shapes.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The description of the tool roll is in the report. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No waypoint is drawn. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 118 — A tool roll as a map

Proposed diegetic text: “Map metaphor attributed; not navigation.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: Mira’s report calls the roll a map of what was here. This passage treats that as her metaphor, not a geometric route. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The description of the tool roll is in the report. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No waypoint is drawn. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 119 — A tool roll as a map

Mira’s report calls the roll a map of what was here. This passage treats that as her metaphor, not a geometric route. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The description of the tool roll is in the report. Neither voice exists to lecture the player.

Mira the Scavenger: “Can I follow it?”
An unnamed wish-giver: “Only as a list of shapes.”
Mira the Scavenger: “No waypoint is drawn.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 120 — A tool roll as a map

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No waypoint is drawn. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No waypoint is drawn.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Map metaphor attributed; not navigation.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 121 — One person’s day

The report has a one-day duration and a team of one. The scene does not add a team member because the room seems too quiet. The expedition report states team size and duration. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Was Mira alone?”
An unnamed wish-giver: “That is what the report records.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The expedition report states team size and duration. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No extra witness is created. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 122 — One person’s day

Proposed diegetic text: “One report, one day.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The report has a one-day duration and a team of one. The scene does not add a team member because the room seems too quiet. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The expedition report states team size and duration. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No extra witness is created. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 123 — One person’s day

The report has a one-day duration and a team of one. The scene does not add a team member because the room seems too quiet. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: The expedition report states team size and duration. Neither voice exists to lecture the player.

Mira the Scavenger: “Was Mira alone?”
An unnamed wish-giver: “That is what the report records.”
Mira the Scavenger: “No extra witness is created.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 124 — One person’s day

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No extra witness is created. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No extra witness is created.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “One report, one day.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 125 — The second empty shape

The second blank does not become the soldering iron simply because the next page asks for one. Atmosphere says two shapes are empty but does not name them. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “It could be that one.”
An unnamed wish-giver: “It could be something else.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Atmosphere says two shapes are empty but does not name them. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: The later reading preserves the open possibility. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 126 — The second empty shape

Proposed diegetic text: “No mapping between blank and wish.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: The second blank does not become the soldering iron simply because the next page asks for one. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Atmosphere says two shapes are empty but does not name them. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later reading preserves the open possibility. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 127 — The second empty shape

The second blank does not become the soldering iron simply because the next page asks for one. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Atmosphere says two shapes are empty but does not name them. Neither voice exists to lecture the player.

Mira the Scavenger: “It could be that one.”
An unnamed wish-giver: “It could be something else.”
Mira the Scavenger: “The later reading preserves the open possibility.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 128 — The second empty shape

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later reading preserves the open possibility. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “The later reading preserves the open possibility.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No mapping between blank and wish.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 129 — The garage waits without speaking

Mira’s report says the garage is waiting for someone who will not come back. The expansion leaves the sentence as report voice, not prophecy. Report authorship and atmospheric narration remain distinct. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child.

Mira the Scavenger: “Who will not come back?”
An unnamed wish-giver: “The file does not name them.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Report authorship and atmospheric narration remain distinct. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved.

Scene close: No death or disappearance is added. The two empty shapes are not an inventory of the two wishes. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 130 — The garage waits without speaking

Proposed diegetic text: “Wait attributed to report; fate not proven.”

Proposed author and audience: A mechanic speaking through the existing wish; the mechanic who asked for the wrench. The artifact exists in this proposal for a practical reason: Mira’s report says the garage is waiting for someone who will not come back. The expansion leaves the sentence as report voice, not prophecy. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Report authorship and atmospheric narration remain distinct. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No death or disappearance is added. A field report can say what it saw without naming the missing person. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 131 — The garage waits without speaking

Mira’s report says the garage is waiting for someone who will not come back. The expansion leaves the sentence as report voice, not prophecy. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Mira’s report is practical, exact, and blunt about the garage waiting for someone who will not return. Wish voices belong to their existing archetype records and remain tokenized where the source uses {name}. An unnamed reader can describe the bench but may not claim to know its former owner. Do not write a new named mechanic, electrician, or child. Keep the conversation attached to the work already in the scene: Report authorship and atmospheric narration remain distinct. Neither voice exists to lecture the player.

Mira the Scavenger: “Who will not come back?”
An unnamed wish-giver: “The file does not name them.”
Mira the Scavenger: “No death or disappearance is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 132 — The garage waits without speaking

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No death or disappearance is added. A tool’s history cannot be inferred from the vise around it. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Mira the Scavenger: “No death or disappearance is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Wait attributed to report; fate not proven.”

Return boundary: Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. Text may accompany the current garage field report or the existing wish surfaces after each owner confirms its consumer. A callback must use the exact existing wish completion state. No new salvage reward, repair recipe, tool inventory merge, garage route, or training mechanic is proposed. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

## 15. Beat selection index

| Beat | Editorial focus | Candidate in-world artifact | Continuity limit |
|---:|---|---|---|
| 01 | The cloth before the list | Tool shapes: two empty; identities not entered. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 02 | The vise still holds | Pump in vise; status as observed. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 03 | Mira’s open door | Door open in report context; no permanent state. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 04 | Four metric wrenches | Four metric wrenches, per report. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 05 | Nine-sixteenths on the wish card | Wish description only; physical recovery remains existing content. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 06 | The wrap in the sentence | Existing final-wish detail only. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 07 | A shelf behind paint tins | Clue attributed to the wish, not a navigation guide. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 08 | The soldering iron kept warm | Iron belongs to that wish only. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 09 | A fourth learner | Outcome already authored; no progression added. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 10 | The report’s half-full oil | Reported quantity, no current inventory claim. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 11 | The pump needs two things | Missing parts as report text; no crafting steps. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 12 | The vise and the absent owner | Identity and fate remain unknown. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 13 | A cloth that survived an inventory | The report’s interpretation stays attributed to Mira. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 14 | Four pieces and one request | Comparison is not identity. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 15 | Mira leaves the bench | Mira is not a wish owner by this evidence. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 16 | The lift that will not lift | Lift condition recorded in source terms. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 17 | The locker remains unopened | Contents unknown. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 18 | The pit beneath the work floor | Pit access not proposed. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 19 | Roof half gone | Light is scene staging; safety not inferred. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 20 | Bare benches | No new bench inventory. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 21 | The wish returns with a name token | Token remains owner-supplied. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 22 | The family line stops at the wrench | No ancestor names added. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 23 | A jaw stiff in the existing ending | Callback only after existing completion. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 24 | The line of the iron’s teaching | Existing outcome only. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 25 | A metric list in a different hand | Report scope remains one expedition. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 26 | The missing shapes at noon | Blank shapes remain blank. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 27 | The empty space for a receipt | Editorial copy only. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 28 | The pump stays in its vise | No completion inferred. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 29 | Two pages, no merge | Source separation visible. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 30 | A tool roll as a map | Map metaphor attributed; not navigation. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 31 | One person’s day | One report, one day. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 32 | The second empty shape | No mapping between blank and wish. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |
| 33 | The garage waits without speaking | Wait attributed to report; fate not proven. | Do not conflate the metric wrench set in the field report with the nine-sixteenths family wrench. Do not claim the soldering iron is the same as any item in Mira’s report. Do not add wiring or pump-repair instructions, identify the garage’s absent owner, or make a field report prove a person’s fate. Keep the two empty outlines unresolved. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions or state. The existence of a record in JSON does not prove its consumer. Verify the current owning system and its exposed state before choosing a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `final_wishes.json / wish_mechanic_lost_wrench` | locate_garage and retrieve_wrench steps | Use only the existing wish progression and completion text. |
| `final_wishes.json / wish_electrician_soldering_iron` | reach_the_workshop and retrieve_the_iron steps | Keep the iron’s lesson and fourth learner in this arc alone. |
| `narrative/field_reports_expansion.json / exp_report_garage_tools` | one-person Day 7 field report | Preserve Mira’s exact findings; do not merge with either wish. |

## 17. Collision and unresolved authority

The environment text has an absent “he” but no name; the wishes use character archetypes and distinct objects; the report names Mira and lists salvaged tools. The expansion must not fuse these sources into one owner or one inventory. The roof and hazard statements are also source-specific: the field report says the garage was sound and without radiation or hostiles on that expedition, but this is not a permanent safety guarantee. If a future implementation needs a single authoritative answer, pause content selection until the current owner resolves the source conflict. No text in this plan is that resolution.

## 18. Review checklist

Before selecting a passage, compare it again with the source rows named in section 3. Keep source facts and existing choices exact, mark proposed voices as editorial until approved, and independently verify any route, text consumer, or state condition. Do not infer reachability from a location description. Read every line aloud for role-appropriate vocabulary, remove any sentence that sounds like a feature spec, and keep each artifact’s author, audience, and purpose plausible.

## 19. Acceptance boundary

This plan is complete as a game-content proposal when selected passages can be traced to the cited location or linked records, existing choices and consequences remain unchanged, no unknown is promoted into canon, and an existing owner is identified for any implementation. If that owner or consumer is absent, retain the prose as a draft rather than inventing a route or system. Character counts are Unicode code-point counts of the saved Markdown files.
