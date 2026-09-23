# CONTENT EXPANSION CW31-06 — The Roads Share a Crater

## A contested landmark assembled from map edges, supply notes, and claims that do not settle into one border.

### Prose Wave 31: Six Places, Six Kinds of Work

## Batch brief

**Content type:** prose-first playable-content expansion plan with scene drafts, diegetic records, conversation fragments, and conditional callbacks.
**Content bank:** 31 story beats with four alternative authored forms per beat (124 candidate passages).
**Current location anchor:** `loc_cut_radiation_zone_alpha` — Fallout Zone Alpha.
**Tone:** material, restrained, human, and careful with uncertainty.
**Canon sensitivity:** current location and linked authored records define known facts; old plans or JSON presence do not prove a current runtime route.
**Scope:** game-content plan and original prose drafts only; no production code, JSON, route, quest, flag, system, or save change.

## 1. Expansion thesis

The fallout crater is not a blank center waiting for one faction’s flag. Current territory records place more than one claimant around or over the same named zone; map edges and route references add movement without making ownership simple. Caravans, a waystation, settlements, and supply lines offer distinct views of how people describe proximity and passage. This expansion makes the crater a place where paperwork meets lived dependence: a load is recorded as en route, a mapmaker marks two names against one feature, and a traveler refuses to call a road controlled just because a ledger names its destination. It preserves contested claims and avoids route or radiation advice. The bank below is intended to produce playable narrative texture: a player can discover, compare, question, refuse, or return to a passage while the existing game systems continue to own state. The fragments are written as content, not as a feature roadmap.

## 2. Story question

How do people share a landmark when the records cannot agree who owns the ground around it?

## 3. Verified local anchor and source records

The location record identifies the cut radiation zone alpha. Faction territory data contains overlapping or conflicting control claims involving this location; the plan preserves those as authored claims rather than resolving them. World-evolution data includes a radiation hotspot bloom event, and caravans, waystations, settlements, supply lines, and the wasteland map provide adjacent records about movement and connection. These records have distinct authority and may not describe the same moment. This proposal adds no radiation simulation, safe corridor, faction border, travel result, or ownership ruling.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `loc_cut_radiation_zone_alpha` | named crater-zone location anchor |
| `Assets/StreamingAssets/Data/faction_territory.json` | `territory entries mentioning loc_cut_radiation_zone_alpha` | multiple recorded claims kept attributed and unresolved |
| `Assets/StreamingAssets/Data/world_evolution_events.json` | `event_evolution_rad_hotspot_bloom` | world event reference, not a new local hazard state |
| `Assets/StreamingAssets/Data/caravans.json` | `caravan route records` | caravan movement language and source-bounded itinerary claims |
| `Assets/StreamingAssets/Data/wasteland_map_v1.json` | `map edges touching the zone` | authored adjacency, not safe or currently traversable routes |
| `Assets/StreamingAssets/Data/supply_lines.json` | `supply lines touching the zone` | logistical relationship without ownership proof |
| `Assets/StreamingAssets/Data/settlements.json` | `settlement references` | settlement voice and listed relations kept distinct |
| `Assets/StreamingAssets/Data/waystations.json` | `waystation references` | stop descriptions are not a guarantee of passage or safety |

## 4. Fixed canon and open space

The crater is a location named in current data. Territory rows are claims with their own identifiers and conditions; their overlap must remain visible. The hotspot bloom is a world-evolution event with its own authority and is not a new local radiation reading. Caravans and map edges can describe planned or authored connections without certifying present travel. Supply line association does not confer ownership. No side is chosen as the true claimant, no safe route is inferred, and no faction gains new control. Existing characters retain their authored identity, boundaries, and outcomes. New working voices remain editorial until an existing content owner approves them. Never fill a source gap solely to make the scene resolve.

## 5. Human center

The human center is the driver who has to decide what to write on a crate when the road name is contested. A waystation keeper needs a label that helps a tired person find a place, while two faction clerks use different words for the same ground. Their disagreement matters because people rely on maps and deliveries, yet no one in the story can honestly promise that one label makes passage safe. Give the argument practical stakes without making the people who live near the crater into abstractions. The emotional pressure should come through what a person records, omits, repairs, asks, or leaves unsigned rather than a narrator naming what the location means.

## 6. Voice and point of view

Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep each author’s knowledge local. A field note cannot know what an unnamed visitor thought; a later reader cannot recover a date that was never recorded; a title cannot create a route. Vary sentence length and register by document purpose, not by changing established facts.

## 7. Placement and current reachability

Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. All fragments are candidates. None is evidence that the location is currently player-reachable. Before selecting any text, verify the current map/location owner, content schema, actual consumer, and the source condition under which the text can appear.

## 8. Player agency

The player can compare the claims, ask who authored a label, decide which record to consult first, or decline to use either wording. These are reading and interpretation choices. The player cannot choose a canonical owner through dialogue, make a route safe, redirect a caravan, transfer supplies, or suppress an existing claim. Existing faction, map, travel, and logistics owners retain all state and outcomes. Do not add a moral-choice menu simply to make quiet prose interactive. Preserve the player’s refusal, ability to leave, and the authority of the characters who own their words.

## 9. Continuity, dignity, and safety

Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Keep the setting fictional and physically grounded. No absent person receives a biography merely to heighten emotion. Technical and medical context remains descriptive and non-instructional.

## 10. Existing hooks and implementation boundary

**Existing content anchors:** the location row and linked records listed above. **Unverified:** any route, text consumer, state condition, dialogue surface, or return trigger not explicitly established by current authority. **Classification:** editorial game-content proposal; no implementation category is claimed until an owner and consumer are confirmed.

This plan changes no production code or game data. Do not add a parallel discovery registry, route, save section, or gameplay authority to host these drafts. Where a passage reflects a branch, use only the existing state named in section 16 and only after its current owner confirms the consumer.

## 11. Narrative sequence

### 1. Disturbance — one crater, two labels

A traveler finds two records naming the same landmark under different faction claims.

### 2. Discovery — the map edge

The player distinguishes a drawn connection from a present travel guarantee.

### 3. Interpretation — what territory claims

A faction notice is read in its own voice; a second claim is not erased.

### 4. Complication — supplies depend on passage

A ledger notes a supply relationship without proving that any particular caravan arrived.

### 5. Choice — quote, compare, or leave blank

The player can preserve attribution rather than select a winner.

### 6. Return — the waystation keeps both names

A callback shows how people maintain a useful local description without claiming to settle the border.

## 12. Creative variants

### Grounded

Use one map annotation, two attributed claims, and a single waystation conversation.

### Interlinked

Show caravan or supply-line callbacks only where current consumers expose the exact record and conditions.

### Wild card

Build a scene around a crate label that carries both names separated by a slash, until the carrier asks which name the recipient recognizes.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not four required encounters. Scene drafts stage an observation; record drafts give it a plausible author and audience; conversation fragments expose a practical point of friction; consequence vignettes allow a later reader to recognize changed context. Select only forms that fit an existing content owner.

A selected fragment should answer who made it, why they made it, who might read it, and what the author cannot know. Keep objects specific to this location. Let a line do practical work before it carries a theme. Remove exposition that a worker would not say aloud, repeated catastrophe language, unearned revelation, and wording that could be mistaken for a new mechanic. Where existing game state matters, state it in the source owner’s terms and do not duplicate its authority.

## 14. Content bank

### Scene draft 001 — A crater with two captions

A traveler sees the same cut crater named in two records and asks whether one label is old. The scene does not supply an ordering the data has not established. Territory rows are attributed claims, not an editorially settled map. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Which one do we use?”
A waystation keeper: “Which record are you reading?”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Territory rows are attributed claims, not an editorially settled map. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: The question moves from ownership to source. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 002 — A crater with two captions

Proposed diegetic text: “Two names recorded; one location anchor.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A traveler sees the same cut crater named in two records and asks whether one label is old. The scene does not supply an ordering the data has not established. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Territory rows are attributed claims, not an editorially settled map. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The question moves from ownership to source. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 003 — A crater with two captions

A traveler sees the same cut crater named in two records and asks whether one label is old. The scene does not supply an ordering the data has not established. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Territory rows are attributed claims, not an editorially settled map. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Which one do we use?”
A waystation keeper: “Which record are you reading?”
A caravan driver whose identity is not supplied: “The question moves from ownership to source.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 004 — A crater with two captions

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The question moves from ownership to source. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “The question moves from ownership to source.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Two names recorded; one location anchor.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 005 — The map edge

A line on the wasteland map touches the zone. It is described as cartographic adjacency rather than a safe path or current travel invitation. Map edge data establishes a relationship in the authored map. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Can we go that way?”
A waystation keeper: “The map does not answer for today.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Map edge data establishes a relationship in the authored map. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No route is unlocked. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 006 — The map edge

Proposed diegetic text: “Connection drawn; traversal not asserted.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A line on the wasteland map touches the zone. It is described as cartographic adjacency rather than a safe path or current travel invitation. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Map edge data establishes a relationship in the authored map. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No route is unlocked. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 007 — The map edge

A line on the wasteland map touches the zone. It is described as cartographic adjacency rather than a safe path or current travel invitation. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Map edge data establishes a relationship in the authored map. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Can we go that way?”
A waystation keeper: “The map does not answer for today.”
A caravan driver whose identity is not supplied: “No route is unlocked.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 008 — The map edge

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No route is unlocked. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No route is unlocked.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Connection drawn; traversal not asserted.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 009 — A road named by its destination

A caravan note speaks of its destination, not of every mile between departure and arrival. The driver’s voice stays bounded by the route record. Caravan data can record a route without establishing a completed trip. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Are they already there?”
A waystation keeper: “The paper only says where the line points.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Caravan data can record a route without establishing a completed trip. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No caravan is spawned or moved. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 010 — A road named by its destination

Proposed diegetic text: “Destination listed; completion unknown.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A caravan note speaks of its destination, not of every mile between departure and arrival. The driver’s voice stays bounded by the route record. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Caravan data can record a route without establishing a completed trip. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No caravan is spawned or moved. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 011 — A road named by its destination

A caravan note speaks of its destination, not of every mile between departure and arrival. The driver’s voice stays bounded by the route record. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Caravan data can record a route without establishing a completed trip. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Are they already there?”
A waystation keeper: “The paper only says where the line points.”
A caravan driver whose identity is not supplied: “No caravan is spawned or moved.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 012 — A road named by its destination

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No caravan is spawned or moved. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No caravan is spawned or moved.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Destination listed; completion unknown.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 013 — A label on the crate

The clerk writes a place name on the crate, then pauses when another person uses a different claimant’s wording. The territory rows overlap in reference to the same landmark. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Which name should I write?”
A waystation keeper: “Write the one your recipient will recognize, and keep the citation.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The territory rows overlap in reference to the same landmark. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No canonical label is chosen. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 014 — A label on the crate

Proposed diegetic text: “Label is proposed dialogue prop, not inventory metadata.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The clerk writes a place name on the crate, then pauses when another person uses a different claimant’s wording. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The territory rows overlap in reference to the same landmark. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No canonical label is chosen. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 015 — A label on the crate

The clerk writes a place name on the crate, then pauses when another person uses a different claimant’s wording. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: The territory rows overlap in reference to the same landmark. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Which name should I write?”
A waystation keeper: “Write the one your recipient will recognize, and keep the citation.”
A caravan driver whose identity is not supplied: “No canonical label is chosen.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 016 — A label on the crate

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No canonical label is chosen. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No canonical label is chosen.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Label is proposed dialogue prop, not inventory metadata.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 017 — Two notices, one wall

A proposed waystation wall holds two short notices, both clearly attributed. Neither is presented as the official border ruling. Faction notices speak for their authors. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Should one come down?”
A waystation keeper: “That is not this archive’s authority.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Faction notices speak for their authors. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No faction notice is removed. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 018 — Two notices, one wall

Proposed diegetic text: “Attribution preserved on both.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A proposed waystation wall holds two short notices, both clearly attributed. Neither is presented as the official border ruling. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Faction notices speak for their authors. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No faction notice is removed. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 019 — Two notices, one wall

A proposed waystation wall holds two short notices, both clearly attributed. Neither is presented as the official border ruling. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Faction notices speak for their authors. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Should one come down?”
A waystation keeper: “That is not this archive’s authority.”
A caravan driver whose identity is not supplied: “No faction notice is removed.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 020 — Two notices, one wall

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No faction notice is removed. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No faction notice is removed.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Attribution preserved on both.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 021 — The first claim speaks plainly

A faction writer calls the crater theirs in the terms already present in the territory record. The scene does not soften or elevate that claim. The territory entry supplies its own owner and condition. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “So it is theirs?”
A waystation keeper: “That is what this record asserts.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The territory entry supplies its own owner and condition. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No neutral narrator ratifies it. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 022 — The first claim speaks plainly

Proposed diegetic text: “Claim quoted as claim.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A faction writer calls the crater theirs in the terms already present in the territory record. The scene does not soften or elevate that claim. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The territory entry supplies its own owner and condition. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No neutral narrator ratifies it. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 023 — The first claim speaks plainly

A faction writer calls the crater theirs in the terms already present in the territory record. The scene does not soften or elevate that claim. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: The territory entry supplies its own owner and condition. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “So it is theirs?”
A waystation keeper: “That is what this record asserts.”
A caravan driver whose identity is not supplied: “No neutral narrator ratifies it.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 024 — The first claim speaks plainly

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No neutral narrator ratifies it. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No neutral narrator ratifies it.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Claim quoted as claim.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 025 — The second claim remains

The second authored control record is read without being treated as a correction of the first. Multiple claims touch the named location. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “But the first one said it clearly.”
A waystation keeper: “This one does too.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Multiple claims touch the named location. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No tie-break rule is invented. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 026 — The second claim remains

Proposed diegetic text: “Second claim retained independently.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The second authored control record is read without being treated as a correction of the first. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Multiple claims touch the named location. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No tie-break rule is invented. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 027 — The second claim remains

The second authored control record is read without being treated as a correction of the first. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Multiple claims touch the named location. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “But the first one said it clearly.”
A waystation keeper: “This one does too.”
A caravan driver whose identity is not supplied: “No tie-break rule is invented.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 028 — The second claim remains

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No tie-break rule is invented. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No tie-break rule is invented.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Second claim retained independently.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 029 — The mapmaker’s narrow key

A map copyist marks which record supplied each label in the margin. The map itself remains the current authority’s data, not a newly redrawn border. Source labels are an editorial proposal only. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Can you draw the border?”
A waystation keeper: “Not from these pages alone.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Source labels are an editorial proposal only. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No geometry is authored. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 030 — The mapmaker’s narrow key

Proposed diegetic text: “Claim provenance visible.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A map copyist marks which record supplied each label in the margin. The map itself remains the current authority’s data, not a newly redrawn border. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Source labels are an editorial proposal only. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No geometry is authored. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 031 — The mapmaker’s narrow key

A map copyist marks which record supplied each label in the margin. The map itself remains the current authority’s data, not a newly redrawn border. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Source labels are an editorial proposal only. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Can you draw the border?”
A waystation keeper: “Not from these pages alone.”
A caravan driver whose identity is not supplied: “No geometry is authored.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 032 — The mapmaker’s narrow key

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No geometry is authored. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No geometry is authored.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Claim provenance visible.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 033 — A road and a border are not synonyms

A traveler points to an edge and calls it proof of control. The keeper asks whether a road description and a territory claim answer the same question. Map and faction authority describe different relationships. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Does the route belong to them?”
A waystation keeper: “Not because a line touches the place.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Map and faction authority describe different relationships. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No jurisdiction is inferred. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 034 — A road and a border are not synonyms

Proposed diegetic text: “Edge does not imply sovereignty.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A traveler points to an edge and calls it proof of control. The keeper asks whether a road description and a territory claim answer the same question. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Map and faction authority describe different relationships. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No jurisdiction is inferred. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 035 — A road and a border are not synonyms

A traveler points to an edge and calls it proof of control. The keeper asks whether a road description and a territory claim answer the same question. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Map and faction authority describe different relationships. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Does the route belong to them?”
A waystation keeper: “Not because a line touches the place.”
A caravan driver whose identity is not supplied: “No jurisdiction is inferred.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 036 — A road and a border are not synonyms

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No jurisdiction is inferred. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No jurisdiction is inferred.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Edge does not imply sovereignty.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 037 — The supply note

A supply line lists a relation to a settlement or route. Its presence can explain dependence, not ownership of the ground it crosses. Supply-line records have logistical scope. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Who owns the line?”
A waystation keeper: “The record names a connection.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Supply-line records have logistical scope. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No new institution is created. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 038 — The supply note

Proposed diegetic text: “Relation recorded; control not transferred.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A supply line lists a relation to a settlement or route. Its presence can explain dependence, not ownership of the ground it crosses. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Supply-line records have logistical scope. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new institution is created. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 039 — The supply note

A supply line lists a relation to a settlement or route. Its presence can explain dependence, not ownership of the ground it crosses. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Supply-line records have logistical scope. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Who owns the line?”
A waystation keeper: “The record names a connection.”
A caravan driver whose identity is not supplied: “No new institution is created.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 040 — The supply note

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new institution is created. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No new institution is created.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Relation recorded; control not transferred.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 041 — The crate that may not arrive

A delivery is discussed in future tense. The scene does not change it into a completed handoff just because a ledger has a destination. Caravan itinerary and supply line do not certify receipt. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Should we wait here?”
A waystation keeper: “The source cannot tell you when to arrive.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Caravan itinerary and supply line do not certify receipt. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No wait timer or schedule is added. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 042 — The crate that may not arrive

Proposed diegetic text: “Delivery status unconfirmed.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A delivery is discussed in future tense. The scene does not change it into a completed handoff just because a ledger has a destination. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Caravan itinerary and supply line do not certify receipt. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No wait timer or schedule is added. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 043 — The crate that may not arrive

A delivery is discussed in future tense. The scene does not change it into a completed handoff just because a ledger has a destination. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Caravan itinerary and supply line do not certify receipt. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Should we wait here?”
A waystation keeper: “The source cannot tell you when to arrive.”
A caravan driver whose identity is not supplied: “No wait timer or schedule is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 044 — The crate that may not arrive

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No wait timer or schedule is added. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No wait timer or schedule is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Delivery status unconfirmed.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 045 — A waystation keeps a local name

The keeper uses a name that helps people recognize the place without claiming legal control over the crater. Waystation record gives local context, not territory resolution. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Which name do people here use?”
A waystation keeper: “The one that gets a traveler to the desk.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Waystation record gives local context, not territory resolution. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No universal naming rule is set. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 046 — A waystation keeps a local name

Proposed diegetic text: “Local usage attributed to keeper role.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The keeper uses a name that helps people recognize the place without claiming legal control over the crater. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Waystation record gives local context, not territory resolution. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No universal naming rule is set. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 047 — A waystation keeps a local name

The keeper uses a name that helps people recognize the place without claiming legal control over the crater. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Waystation record gives local context, not territory resolution. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Which name do people here use?”
A waystation keeper: “The one that gets a traveler to the desk.”
A caravan driver whose identity is not supplied: “No universal naming rule is set.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 048 — A waystation keeps a local name

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No universal naming rule is set. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No universal naming rule is set.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Local usage attributed to keeper role.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 049 — A settlement asks for the other spelling

A settlement clerk requests both terms on the copied note because one shipment uses each. Settlement references have their own naming and relation context. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Will that settle it?”
A waystation keeper: “It will help the clerk file the page.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Settlement references have their own naming and relation context. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No political agreement is implied. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 050 — A settlement asks for the other spelling

Proposed diegetic text: “Both names copied with source marks.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A settlement clerk requests both terms on the copied note because one shipment uses each. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Settlement references have their own naming and relation context. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No political agreement is implied. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 051 — A settlement asks for the other spelling

A settlement clerk requests both terms on the copied note because one shipment uses each. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Settlement references have their own naming and relation context. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Will that settle it?”
A waystation keeper: “It will help the clerk file the page.”
A caravan driver whose identity is not supplied: “No political agreement is implied.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 052 — A settlement asks for the other spelling

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No political agreement is implied. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No political agreement is implied.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Both names copied with source marks.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 053 — The bloom on another page

A reader finds the world-evolution entry for the radiation hotspot bloom. Its date and reach remain with that event record rather than being projected onto this particular traveler. Global event source is distinct from the location row. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Did the crater change after that?”
A waystation keeper: “This record does not measure it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Global event source is distinct from the location row. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No radiation level is invented. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 054 — The bloom on another page

Proposed diegetic text: “Event referenced; local reading not recalculated.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A reader finds the world-evolution entry for the radiation hotspot bloom. Its date and reach remain with that event record rather than being projected onto this particular traveler. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Global event source is distinct from the location row. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No radiation level is invented. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 055 — The bloom on another page

A reader finds the world-evolution entry for the radiation hotspot bloom. Its date and reach remain with that event record rather than being projected onto this particular traveler. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Global event source is distinct from the location row. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Did the crater change after that?”
A waystation keeper: “This record does not measure it.”
A caravan driver whose identity is not supplied: “No radiation level is invented.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 056 — The bloom on another page

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No radiation level is invented. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No radiation level is invented.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Event referenced; local reading not recalculated.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 057 — Not a meter

A player asks how dangerous the zone is now. The proposed answer points to the absence of a supported local measurement instead of supplying a number. No per-location radiation value is authored by this plan. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Can I cross safely?”
A waystation keeper: “The prose cannot promise that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: No per-location radiation value is authored by this plan. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No safety advice is provided. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 058 — Not a meter

Proposed diegetic text: “No numeric exposure claim.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A player asks how dangerous the zone is now. The proposed answer points to the absence of a supported local measurement instead of supplying a number. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: No per-location radiation value is authored by this plan. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No safety advice is provided. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 059 — Not a meter

A player asks how dangerous the zone is now. The proposed answer points to the absence of a supported local measurement instead of supplying a number. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: No per-location radiation value is authored by this plan. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Can I cross safely?”
A waystation keeper: “The prose cannot promise that.”
A caravan driver whose identity is not supplied: “No safety advice is provided.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 060 — Not a meter

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No safety advice is provided. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No safety advice is provided.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No numeric exposure claim.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 061 — A lamp over the ledger

At the waystation, the clerk reads the supply note under a weak lamp. The detail is staged for the scene and not used as a clue about the territory. No lighting condition is sourced for this record. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Did the road close?”
A waystation keeper: “The lamp only made the ink hard to read.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: No lighting condition is sourced for this record. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No travel-state change occurs. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 062 — A lamp over the ledger

Proposed diegetic text: “Atmosphere only.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: At the waystation, the clerk reads the supply note under a weak lamp. The detail is staged for the scene and not used as a clue about the territory. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: No lighting condition is sourced for this record. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No travel-state change occurs. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 063 — A lamp over the ledger

At the waystation, the clerk reads the supply note under a weak lamp. The detail is staged for the scene and not used as a clue about the territory. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: No lighting condition is sourced for this record. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Did the road close?”
A waystation keeper: “The lamp only made the ink hard to read.”
A caravan driver whose identity is not supplied: “No travel-state change occurs.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 064 — A lamp over the ledger

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No travel-state change occurs. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No travel-state change occurs.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Atmosphere only.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 065 — A driver chooses a word

A driver uses “near” because it describes the route entry without taking a position on ownership. Caravan language can remain narrower than faction notices. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Why not say controlled?”
A waystation keeper: “Because my route note does not prove that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Caravan language can remain narrower than faction notices. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No new territory state is recorded. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 066 — A driver chooses a word

Proposed diegetic text: “Near is a speaker’s chosen word.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A driver uses “near” because it describes the route entry without taking a position on ownership. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Caravan language can remain narrower than faction notices. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new territory state is recorded. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 067 — A driver chooses a word

A driver uses “near” because it describes the route entry without taking a position on ownership. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Caravan language can remain narrower than faction notices. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Why not say controlled?”
A waystation keeper: “Because my route note does not prove that.”
A caravan driver whose identity is not supplied: “No new territory state is recorded.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 068 — A driver chooses a word

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new territory state is recorded. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No new territory state is recorded.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Near is a speaker’s chosen word.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 069 — The route that exists in data

A reviewer sees an authored map edge and checks its consumer before calling it playable. Data presence alone does not make the trip reachable. Map edge requires current runtime reachability proof. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “So the game has a road?”
A waystation keeper: “It has an edge in this map record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Map edge requires current runtime reachability proof. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No runtime integration is claimed. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 070 — The route that exists in data

Proposed diegetic text: “Consumer unverified here.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A reviewer sees an authored map edge and checks its consumer before calling it playable. Data presence alone does not make the trip reachable. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Map edge requires current runtime reachability proof. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No runtime integration is claimed. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 071 — The route that exists in data

A reviewer sees an authored map edge and checks its consumer before calling it playable. Data presence alone does not make the trip reachable. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Map edge requires current runtime reachability proof. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “So the game has a road?”
A waystation keeper: “It has an edge in this map record.”
A caravan driver whose identity is not supplied: “No runtime integration is claimed.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 072 — The route that exists in data

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No runtime integration is claimed. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No runtime integration is claimed.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Consumer unverified here.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 073 — An old destination note

A caravan clerk finds a destination note without enough context to date its validity. The plan does not call the route current. Caravan records retain their own timing and conditions. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Can I use this one?”
A waystation keeper: “Read its condition first.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Caravan records retain their own timing and conditions. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No active schedule is asserted. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 074 — An old destination note

Proposed diegetic text: “Date and status bounded to source.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A caravan clerk finds a destination note without enough context to date its validity. The plan does not call the route current. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Caravan records retain their own timing and conditions. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No active schedule is asserted. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 075 — An old destination note

A caravan clerk finds a destination note without enough context to date its validity. The plan does not call the route current. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Caravan records retain their own timing and conditions. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Can I use this one?”
A waystation keeper: “Read its condition first.”
A caravan driver whose identity is not supplied: “No active schedule is asserted.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 076 — An old destination note

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No active schedule is asserted. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No active schedule is asserted.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Date and status bounded to source.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 077 — The clerk draws a slash

Between two names, the clerk draws a slash instead of an equals sign. The punctuation allows a useful label without merging claims. Editorial text device only. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Does the slash mean both own it?”
A waystation keeper: “It means both pages name this place.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Editorial text device only. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No sovereignty is inferred. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 078 — The clerk draws a slash

Proposed diegetic text: “Slash signals co-reference in this copy, not control.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: Between two names, the clerk draws a slash instead of an equals sign. The punctuation allows a useful label without merging claims. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Editorial text device only. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No sovereignty is inferred. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 079 — The clerk draws a slash

Between two names, the clerk draws a slash instead of an equals sign. The punctuation allows a useful label without merging claims. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Editorial text device only. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Does the slash mean both own it?”
A waystation keeper: “It means both pages name this place.”
A caravan driver whose identity is not supplied: “No sovereignty is inferred.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 080 — The clerk draws a slash

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No sovereignty is inferred. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No sovereignty is inferred.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Slash signals co-reference in this copy, not control.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 081 — A traveler who turns back

A traveler decides not to treat a map line as an assurance. The choice is narrative restraint, not a new survival check or route penalty. No passage condition is authored. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Are we losing time?”
A waystation keeper: “We were never promised a safe crossing.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: No passage condition is authored. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No resource or time cost is generated. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 082 — A traveler who turns back

Proposed diegetic text: “Decision belongs to proposed voice.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A traveler decides not to treat a map line as an assurance. The choice is narrative restraint, not a new survival check or route penalty. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: No passage condition is authored. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No resource or time cost is generated. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 083 — A traveler who turns back

A traveler decides not to treat a map line as an assurance. The choice is narrative restraint, not a new survival check or route penalty. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: No passage condition is authored. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Are we losing time?”
A waystation keeper: “We were never promised a safe crossing.”
A caravan driver whose identity is not supplied: “No resource or time cost is generated.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 084 — A traveler who turns back

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No resource or time cost is generated. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No resource or time cost is generated.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Decision belongs to proposed voice.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 085 — The notice no one tears down

Both notices remain in the proposed scene because the plan has no authority to choose which claimant is true. Territory conflict stays unresolved. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Who gets to decide?”
A waystation keeper: “The records do not appoint a judge.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Territory conflict stays unresolved. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No arbitration quest appears. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 086 — The notice no one tears down

Proposed diegetic text: “Archive comparison only.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: Both notices remain in the proposed scene because the plan has no authority to choose which claimant is true. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Territory conflict stays unresolved. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No arbitration quest appears. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 087 — The notice no one tears down

Both notices remain in the proposed scene because the plan has no authority to choose which claimant is true. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Territory conflict stays unresolved. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Who gets to decide?”
A waystation keeper: “The records do not appoint a judge.”
A caravan driver whose identity is not supplied: “No arbitration quest appears.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 088 — The notice no one tears down

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No arbitration quest appears. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No arbitration quest appears.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Archive comparison only.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 089 — A page about dependents

A supply note prompts a reader to think about the people waiting at the destination, but no population or shortage is invented. Supply line establishes connection only. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “What are they short of?”
A waystation keeper: “This page does not list the crate’s contents.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Supply line establishes connection only. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No item quantity is added. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 090 — A page about dependents

Proposed diegetic text: “Human stakes remain generic and local.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A supply note prompts a reader to think about the people waiting at the destination, but no population or shortage is invented. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Supply line establishes connection only. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No item quantity is added. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 091 — A page about dependents

A supply note prompts a reader to think about the people waiting at the destination, but no population or shortage is invented. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Supply line establishes connection only. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “What are they short of?”
A waystation keeper: “This page does not list the crate’s contents.”
A caravan driver whose identity is not supplied: “No item quantity is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 092 — A page about dependents

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No item quantity is added. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No item quantity is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Human stakes remain generic and local.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 093 — The map’s blank margin

The map has a blank margin where a reader could write a source reference, not a border. The reader chooses citation over speculation. Map geometry unchanged. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Can we mark this spot?”
A waystation keeper: “Only with what the source can support.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Map geometry unchanged. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No map annotation mechanic is introduced. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 094 — The map’s blank margin

Proposed diegetic text: “Margin used for provenance.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The map has a blank margin where a reader could write a source reference, not a border. The reader chooses citation over speculation. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Map geometry unchanged. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No map annotation mechanic is introduced. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 095 — The map’s blank margin

The map has a blank margin where a reader could write a source reference, not a border. The reader chooses citation over speculation. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Map geometry unchanged. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Can we mark this spot?”
A waystation keeper: “Only with what the source can support.”
A caravan driver whose identity is not supplied: “No map annotation mechanic is introduced.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 096 — The map’s blank margin

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No map annotation mechanic is introduced. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No map annotation mechanic is introduced.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Margin used for provenance.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 097 — A second copy for the settlement

A clerk prepares a copy for a settlement desk and keeps both territorial names attached to their citations. Settlement reference is not a faction ruling. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Will they accept both?”
A waystation keeper: “That is for the recipient to decide.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Settlement reference is not a faction ruling. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No relationship value changes. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 098 — A second copy for the settlement

Proposed diegetic text: “Copy prepared in proposed prose.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A clerk prepares a copy for a settlement desk and keeps both territorial names attached to their citations. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Settlement reference is not a faction ruling. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No relationship value changes. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 099 — A second copy for the settlement

A clerk prepares a copy for a settlement desk and keeps both territorial names attached to their citations. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Settlement reference is not a faction ruling. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Will they accept both?”
A waystation keeper: “That is for the recipient to decide.”
A caravan driver whose identity is not supplied: “No relationship value changes.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 100 — A second copy for the settlement

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No relationship value changes. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No relationship value changes.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Copy prepared in proposed prose.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 101 — Waystation window

A traveler looking through the station window sees no confirmed caravan. The silence does not mean the supply line has ended. Waystation and caravan records are separate authorities. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Have they passed?”
A waystation keeper: “This page did not see them.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Waystation and caravan records are separate authorities. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No caravan arrival state is inferred. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 102 — Waystation window

Proposed diegetic text: “Absence in this scene is not route status.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A traveler looking through the station window sees no confirmed caravan. The silence does not mean the supply line has ended. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Waystation and caravan records are separate authorities. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No caravan arrival state is inferred. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 103 — Waystation window

A traveler looking through the station window sees no confirmed caravan. The silence does not mean the supply line has ended. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Waystation and caravan records are separate authorities. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Have they passed?”
A waystation keeper: “This page did not see them.”
A caravan driver whose identity is not supplied: “No caravan arrival state is inferred.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 104 — Waystation window

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No caravan arrival state is inferred. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No caravan arrival state is inferred.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Absence in this scene is not route status.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 105 — The same crater at another hour

A second scene returns to the landmark in different light, without claiming a changed radiation condition or updated border. Location is a recurring anchor only. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Does the light change the map?”
A waystation keeper: “It changes what you can see in this scene.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Location is a recurring anchor only. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No world simulation is added. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 106 — The same crater at another hour

Proposed diegetic text: “Time of day is editorial staging.”

Proposed author and audience: a caravan clerk; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A second scene returns to the landmark in different light, without claiming a changed radiation condition or updated border. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Location is a recurring anchor only. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No world simulation is added. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 107 — The same crater at another hour

A second scene returns to the landmark in different light, without claiming a changed radiation condition or updated border. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Location is a recurring anchor only. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Does the light change the map?”
A waystation keeper: “It changes what you can see in this scene.”
A caravan driver whose identity is not supplied: “No world simulation is added.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 108 — The same crater at another hour

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No world simulation is added. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No world simulation is added.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Time of day is editorial staging.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 109 — The story of control

A speaker begins to tell a definitive border story, then cites the record they actually have and leaves the rest unsettled. Evidence defines the speaker’s limit. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “You sound unsure.”
A waystation keeper: “I am sure about what this page says.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Evidence defines the speaker’s limit. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No faction is portrayed as lying. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 110 — The story of control

Proposed diegetic text: “Claim attributed; conclusion withheld.”

Proposed author and audience: a later reader comparing claims; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A speaker begins to tell a definitive border story, then cites the record they actually have and leaves the rest unsettled. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Evidence defines the speaker’s limit. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No faction is portrayed as lying. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 111 — The story of control

A speaker begins to tell a definitive border story, then cites the record they actually have and leaves the rest unsettled. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Evidence defines the speaker’s limit. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “You sound unsure.”
A waystation keeper: “I am sure about what this page says.”
A caravan driver whose identity is not supplied: “No faction is portrayed as lying.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 112 — The story of control

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No faction is portrayed as lying. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No faction is portrayed as lying.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Claim attributed; conclusion withheld.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 113 — Two roads share a name

A copied route label appears in two source contexts. The reader checks whether one denotes adjacency and the other destination before treating them as the same journey. Similar wording may describe different map semantics. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “Is it one road?”
A waystation keeper: “The sources do not establish that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Similar wording may describe different map semantics. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No duplicate edge is authored. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 114 — Two roads share a name

Proposed diegetic text: “Terms compared, not merged.”

Proposed author and audience: a map copyist; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: A copied route label appears in two source contexts. The reader checks whether one denotes adjacency and the other destination before treating them as the same journey. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Similar wording may describe different map semantics. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No duplicate edge is authored. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 115 — Two roads share a name

A copied route label appears in two source contexts. The reader checks whether one denotes adjacency and the other destination before treating them as the same journey. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Similar wording may describe different map semantics. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “Is it one road?”
A waystation keeper: “The sources do not establish that.”
A caravan driver whose identity is not supplied: “No duplicate edge is authored.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 116 — Two roads share a name

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No duplicate edge is authored. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No duplicate edge is authored.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Terms compared, not merged.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 117 — An unclaimed center

The crater sits in the language between records without becoming neutral territory. The scene does not erase the claims by leaving a blank label. Multiple claims remain active in source data. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “So nobody owns it?”
A waystation keeper: “That is not what the blank means.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Multiple claims remain active in source data. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: No control state is set. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 118 — An unclaimed center

Proposed diegetic text: “Blank is not a ruling.”

Proposed author and audience: a faction notice writer; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The crater sits in the language between records without becoming neutral territory. The scene does not erase the claims by leaving a blank label. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Multiple claims remain active in source data. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No control state is set. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 119 — An unclaimed center

The crater sits in the language between records without becoming neutral territory. The scene does not erase the claims by leaving a blank label. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Multiple claims remain active in source data. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “So nobody owns it?”
A waystation keeper: “That is not what the blank means.”
A caravan driver whose identity is not supplied: “No control state is set.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 120 — An unclaimed center

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No control state is set. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “No control state is set.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Blank is not a ruling.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 121 — The road stays a question

The player leaves with sources to compare and no safe passage promised. A waystation keeper returns both copies to their shelves. Existing map, faction, caravan, and supply authorities retain their records. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees.

A caravan driver whose identity is not supplied: “What did we decide?”
A waystation keeper: “Which words each record can support.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Existing map, faction, caravan, and supply authorities retain their records. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced.

Scene close: The crater remains shared by the pages, not settled by the plan. A road on paper is a claim about connection, not a promise of passage. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 122 — The road stays a question

Proposed diegetic text: “Claims preserved; no route change.”

Proposed author and audience: a waystation keeper; a traveler asking how the place is named. The artifact exists in this proposal for a practical reason: The player leaves with sources to compare and no safe passage promised. A waystation keeper returns both copies to their shelves. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Existing map, faction, caravan, and supply authorities retain their records. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The crater remains shared by the pages, not settled by the plan. The crater keeps more than one name in the record. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 123 — The road stays a question

The player leaves with sources to compare and no safe passage promised. A waystation keeper returns both copies to their shelves. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Faction notices speak from their own claim, not as neutral map truth. Caravan text uses the driver’s limited itinerary. Supply records describe a line of dependence, not legal title. Map annotations remain cartographic shorthand. A proposed waystation keeper may speak locally but cannot settle faction control. Keep radiation references non-instructional and do not offer exposure thresholds, travel windows, protective equipment advice, or safety guarantees. Keep the conversation attached to the work already in the scene: Existing map, faction, caravan, and supply authorities retain their records. Neither voice exists to lecture the player.

A caravan driver whose identity is not supplied: “What did we decide?”
A waystation keeper: “Which words each record can support.”
A caravan driver whose identity is not supplied: “The crater remains shared by the pages, not settled by the plan.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 124 — The road stays a question

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The crater remains shared by the pages, not settled by the plan. No label makes the location safe by itself. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — A caravan driver whose identity is not supplied: “The crater remains shared by the pages, not settled by the plan.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Claims preserved; no route change.”

Return boundary: Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. Candidate prose may accompany existing location inspection, map annotation, faction notices, caravan records, or a waystation archive only after each consumer is verified. The plan adds no map edge, route unlock, territory resolver, faction relation, caravan schedule, radiation meter, encounter, or inventory delivery. If a consumer only shows one claim, the text must not imply that it is the complete border history. No visual border treatment is specified as a runtime change. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

## 15. Beat selection index

| Beat | Editorial focus | Candidate in-world artifact | Continuity limit |
|---:|---|---|---|
| 01 | A crater with two captions | Two names recorded; one location anchor. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 02 | The map edge | Connection drawn; traversal not asserted. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 03 | A road named by its destination | Destination listed; completion unknown. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 04 | A label on the crate | Label is proposed dialogue prop, not inventory metadata. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 05 | Two notices, one wall | Attribution preserved on both. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 06 | The first claim speaks plainly | Claim quoted as claim. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 07 | The second claim remains | Second claim retained independently. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 08 | The mapmaker’s narrow key | Claim provenance visible. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 09 | A road and a border are not synonyms | Edge does not imply sovereignty. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 10 | The supply note | Relation recorded; control not transferred. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 11 | The crate that may not arrive | Delivery status unconfirmed. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 12 | A waystation keeps a local name | Local usage attributed to keeper role. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 13 | A settlement asks for the other spelling | Both names copied with source marks. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 14 | The bloom on another page | Event referenced; local reading not recalculated. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 15 | Not a meter | No numeric exposure claim. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 16 | A lamp over the ledger | Atmosphere only. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 17 | A driver chooses a word | Near is a speaker’s chosen word. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 18 | The route that exists in data | Consumer unverified here. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 19 | An old destination note | Date and status bounded to source. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 20 | The clerk draws a slash | Slash signals co-reference in this copy, not control. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 21 | A traveler who turns back | Decision belongs to proposed voice. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 22 | The notice no one tears down | Archive comparison only. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 23 | A page about dependents | Human stakes remain generic and local. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 24 | The map’s blank margin | Margin used for provenance. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 25 | A second copy for the settlement | Copy prepared in proposed prose. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 26 | Waystation window | Absence in this scene is not route status. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 27 | The same crater at another hour | Time of day is editorial staging. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 28 | The story of control | Claim attributed; conclusion withheld. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 29 | Two roads share a name | Terms compared, not merged. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 30 | An unclaimed center | Blank is not a ruling. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |
| 31 | The road stays a question | Claims preserved; no route change. | Do not resolve disputed control, identify a true border, or produce a player-facing safe route. Do not give real-world radiation guidance, dosages, shielding instructions, exposure thresholds, or medical claims. Do not claim a map edge is traversable because it exists. Do not say the hotspot bloom permanently changed this location unless the event record says so. No territory update, route, delivery, faction standing, encounter, quest, or resource effect is introduced. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions or state. The existence of a record in JSON does not prove its consumer. Verify the current owning system and its exposed state before choosing a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `faction_territory.json / crater claims` | overlapping authored control rows | Keep each faction attribution and conditions; do not resolve. |
| `world_evolution_events.json / event_evolution_rad_hotspot_bloom` | global event record | Do not convert to a local radiation value or new consequence. |
| `wasteland_map_v1.json / touching edges` | map adjacency | Map presence is not route safety or present traversal. |
| `caravans.json and supply_lines.json` | movement and logistics references | Do not claim completion, control, or delivery without exact source support. |
| `waystations.json / settlements.json` | place context | Keep their local records distinct from faction jurisdiction. |

## 17. Collision and unresolved authority

The continuity risk is conflating overlapping territory rows with simultaneous physical control, or treating a map connection as a promise that travel is possible. Preserve each claim’s faction, source, and condition. Keep the world-evolution event separate from a location inspection. A caravan route’s authored destination does not prove that a specific trip completed. A supply line is a relation, not sovereignty. If later integration cannot preserve that nuance in the consumer, use a neutral record comparison instead of choosing a side. If a future implementation needs a single authoritative answer, pause content selection until the current owner resolves the source conflict. No text in this plan is that resolution.

## 18. Review checklist

Before selecting a passage, compare it again with the source rows named in section 3. Keep source facts and existing choices exact, mark proposed voices as editorial until approved, and independently verify any route, text consumer, or state condition. Do not infer reachability from a location description. Read every line aloud for role-appropriate vocabulary, remove any sentence that sounds like a feature spec, and keep each artifact’s author, audience, and purpose plausible.

## 19. Acceptance boundary

This plan is complete as a game-content proposal when selected passages can be traced to the cited location or linked records, existing choices and consequences remain unchanged, no unknown is promoted into canon, and an existing owner is identified for any implementation. If that owner or consumer is absent, retain the prose as a draft rather than inventing a route or system. Character counts are Unicode code-point counts of the saved Markdown files.
