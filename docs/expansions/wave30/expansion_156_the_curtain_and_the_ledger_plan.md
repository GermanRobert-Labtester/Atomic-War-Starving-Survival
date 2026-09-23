# EXPANSION 156 — The Curtain and the Ledger

## A care-room story about what a thin partition can and cannot keep private.

### Wave 30: What a Record Cannot Settle

## Batch brief

**Content type:** prose-first playable-content expansion plan with scene drafts, diegetic records, conversation fragments, and conditional callbacks.
**Content bank:** 31 story beats with four alternative authored forms per beat (124 candidate passages).
**Current location anchor:** `loc_shelter_infirmary` — Shelter Infirmary.
**Tone:** material, restrained, human, and careful with uncertainty.
**Canon sensitivity:** current location and linked authored records define known facts; old plans or JSON presence do not prove a current runtime route.
**Scope:** game-content plan and original prose drafts only; no production code, JSON, route, quest, flag, system, or save change.

## 1. Expansion thesis

The infirmary has two couches, a cabinet of scavenged supplies, a dosage ledger that grows by the week, and a curtain whose division between treatment and triage is more psychological than practical. Existing recordings preserve a changing case definition, a separate entrance, a no-visitors notice, and the difficulty of keeping a ward staffed. Existing quest branches already give the room lasting consequences around a medic’s secrecy, a scarce chelation dose, and succession. This expansion adds no fourth medical dilemma. It makes the existing choices legible through the paperwork, pauses, and work left for the next shift. The bank below is intended to produce playable narrative texture: a player can discover, compare, question, refuse, or return to a passage while the existing game systems continue to own state. The fragments are written as content, not as a feature roadmap.

## 2. Story question

When the curtain cannot make care private, what does the room owe to the person on the other side of it—and to everyone who can hear?

## 3. Verified local anchor and source records

The location row describes fluorescent tubes that hum even when main power is offline, two examination couches, a cabinet of scavenged supplies, an expanding dosage ledger, antiseptic and burnt coffee, and a curtain that is more psychological than practical. The four-part quarantine cassette set records an evolving case definition, wristbands, arrows, staffing pressure, soap borrowed from the pharmacy, and a no-visitors order. The linked moral-choice catalog holds three already-authored decisions with persistent outcomes. The records are not one continuous patient file: keep them separate unless an existing content owner explicitly connects them.

| Local source | Record or ID | Fact used by this plan |
|---|---|---|
| `Assets/StreamingAssets/Data/locations.json` | `loc_shelter_infirmary` | two couches, supply cabinet, dosage ledger, humming lights, thin curtain |
| `Assets/StreamingAssets/Data/cassette_sets.json` | `quarantine_tapes` | four recordings: case definition, entrance arrows, staff shortage, visitors |
| `Assets/StreamingAssets/Data/narrative/plan17_discoverable_documents.json` | `doc_death_register_infirmary` | restricted register with gaps and cut-off entries |
| `Assets/StreamingAssets/Data/narrative/plan17_discoverable_documents.json` | `doc_medical_note_rad_exposure` | single patient note; do not generalize its measurements |
| `Assets/StreamingAssets/Data/moral_choice_quests_branching.json` | `quest_moral_chain_mercy_12 / quest_moral_chain_mercy_14 / quest_moral_chain_mercy_24` | existing secret, scarce-dose, and succession branches |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` | `disc_fringe_epitaph_dosimeter_zero_tomb` | unresolved memorial identity; display-only provenance |
| `Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs.json` | `epitaph_scav_dosimeter_zero_tomb` | authored memorial testimony kept distinct from the register |

## 4. Fixed canon and open space

The room’s curtain, two couches, ledger, antiseptic and burnt-coffee smell, and unusual fluorescent hum are current location facts. The quarantine tapes have their own four-part chronology. The death register and patient note retain their stated provenance and incompleteness. The game’s already-authored moral branches remain authoritative; this proposal must not rewrite their choices, deltas, patient outcomes, or follow-up records. No patient from one catalog is assumed to be a patient from another. Existing characters retain their authored identity, boundaries, and outcomes. New working voices remain editorial until an existing content owner approves them. Never fill a source gap solely to make the scene resolve.

## 5. Human center

The human center is the person who cannot hear the whole conversation but hears enough to know a decision is being made. Work continues on both sides of the curtain: a clerk turns a page, someone counts supplies, a volunteer waits for a question to finish. The prose should not make illness a reveal or staff a moral lesson. A patient can remain outside the reader’s view. The emotional pressure should come through what a person records, omits, repairs, asks, or leaves unsigned rather than a narrator naming what the location means.

## 6. Voice and point of view

Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep each author’s knowledge local. A field note cannot know what an unnamed visitor thought; a later reader cannot recover a date that was never recorded; a title cannot create a route. Vary sentence length and register by document purpose, not by changing established facts.

## 7. Placement and current reachability

Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. All fragments are candidates. None is evidence that the location is currently player-reachable. Before selecting any text, verify the current map/location owner, content schema, actual consumer, and the source condition under which the text can appear.

## 8. Player agency

The player can notice the room, read a proposed notice, ask one narrow question, or leave the staff to their work. The three linked moral quests already contain their own player decisions; preserve those decisions exactly. Do not add a second vote on the medic, a new allocation rule, or a hidden test of empathy. Do not add a moral-choice menu simply to make quiet prose interactive. Preserve the player’s refusal, ability to leave, and the authority of the characters who own their words.

## 9. Continuity, dignity, and safety

Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Keep the setting fictional and physically grounded. No absent person receives a biography merely to heighten emotion. Technical and medical context remains descriptive and non-instructional.

## 10. Existing hooks and implementation boundary

**Existing content anchors:** the location row and linked records listed above. **Unverified:** any route, text consumer, state condition, dialogue surface, or return trigger not explicitly established by current authority. **Classification:** editorial game-content proposal; no implementation category is claimed until an owner and consumer are confirmed.

This plan changes no production code or game data. Do not add a parallel discovery registry, route, save section, or gameplay authority to host these drafts. Where a passage reflects a branch, use only the existing state named in section 16 and only after its current owner confirms the consumer.

## 11. Narrative sequence

### 1. Disturbance — the curtain

Start with the sound that crosses the curtain and the ledger that continues to grow. No patient is introduced as a reveal.

### 2. Discovery — a working notice

The player finds that a case definition was written in pencil and revised as staff learned. The words stay attributed to the tape set.

### 3. Interpretation — two imperfect lists

A register with damaged entries and a separate clinical note show why a list can be useful without becoming complete.

### 4. Complication — work has consequences

The authored secrecy and isolation branches remain distinct. Show the cost through existing aftermath, not a new statistic.

### 5. Choice — preserve the current branch

Only the decision already made in the owning quest can select a conditional passage. If the owner cannot expose that state, show no branch-specific prose.

### 6. Callback — the next shift

Return to a corrected notice or an unfinished ledger, and let readers see what has changed without claiming a cure or closure.

## 12. Creative variants

### Grounded

A few high-specificity notices and two short exchanges; no additional discovery.

### Interlinked

Optional text reads differently after one of the existing moral branches, using only verified state already owned by that quest.

### Wild card

A scene is told through the curtain’s acoustic leak: the reader hears pencil, kettle, and page turns but never receives a patient’s private words.

## 13. Alternative forms and editorial rubric

The four forms under each beat are alternatives, not four required encounters. Scene drafts stage an observation; record drafts give it a plausible author and audience; conversation fragments expose a practical point of friction; consequence vignettes allow a later reader to recognize changed context. Select only forms that fit an existing content owner.

A selected fragment should answer who made it, why they made it, who might read it, and what the author cannot know. Keep objects specific to this location. Let a line do practical work before it carries a theme. Remove exposition that a worker would not say aloud, repeated catastrophe language, unearned revelation, and wording that could be mistaken for a new mechanic. Where existing game state matters, state it in the source owner’s terms and do not duplicate its authority.

## 14. Content bank

### Scene draft 001 — The seam in the curtain

A clerk pulls the curtain until its hem stops against a chair leg. The gap is too small to see through and too wide to keep a conversation private. The location calls the curtain a psychological division between treatment and triage. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The cloth closes where it can.”
Waiting listener: “The room carries the rest.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location calls the curtain a psychological division between treatment and triage. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: On a later visit, the hem still misses the chair by a handspan. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 002 — The seam in the curtain

Proposed diegetic text: “Treatment / Triage — please lower your voice, not your concern.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A clerk pulls the curtain until its hem stops against a chair leg. The gap is too small to see through and too wide to keep a conversation private. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location calls the curtain a psychological division between treatment and triage. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: On a later visit, the hem still misses the chair by a handspan. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 003 — The seam in the curtain

A clerk pulls the curtain until its hem stops against a chair leg. The gap is too small to see through and too wide to keep a conversation private. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The location calls the curtain a psychological division between treatment and triage. Neither voice exists to lecture the player.

Infirmary clerk: “The cloth closes where it can.”
Waiting listener: “The room carries the rest.”
Infirmary clerk: “On a later visit, the hem still misses the chair by a handspan.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 004 — The seam in the curtain

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. On a later visit, the hem still misses the chair by a handspan. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “On a later visit, the hem still misses the chair by a handspan.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Treatment / Triage — please lower your voice, not your concern.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 005 — The hum without the main power

The fluorescent tubes continue their thin, even hum while the main power is offline. Nobody stops to explain the exception; a patient’s paper is lifted away from the draft. The sound is a stated location detail, not an invitation to add an electrical system. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It has hummed this way all morning.”
Waiting listener: “Then write that, and leave the why open.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The sound is a stated location detail, not an invitation to add an electrical system. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A second reader sees the same observation without a new explanation. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 006 — The hum without the main power

Proposed diegetic text: “Light noted. Hum noted. Cause not entered.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The fluorescent tubes continue their thin, even hum while the main power is offline. Nobody stops to explain the exception; a patient’s paper is lifted away from the draft. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The sound is a stated location detail, not an invitation to add an electrical system. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A second reader sees the same observation without a new explanation. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 007 — The hum without the main power

The fluorescent tubes continue their thin, even hum while the main power is offline. Nobody stops to explain the exception; a patient’s paper is lifted away from the draft. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The sound is a stated location detail, not an invitation to add an electrical system. Neither voice exists to lecture the player.

Infirmary clerk: “It has hummed this way all morning.”
Waiting listener: “Then write that, and leave the why open.”
Infirmary clerk: “A second reader sees the same observation without a new explanation.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 008 — The hum without the main power

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A second reader sees the same observation without a new explanation. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A second reader sees the same observation without a new explanation.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Light noted. Hum noted. Cause not entered.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 009 — Two couches, one room

A folded blanket lies across one examination couch; the other has a cup mark on its metal rail. The scene does not assign either object to a patient. The location establishes two examination couches, but no schedule or occupant list. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “Do we need to name who used it?”
Waiting listener: “Only if the record already knows.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location establishes two examination couches, but no schedule or occupant list. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The empty field remains readable in the next copy. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 010 — Two couches, one room

Proposed diegetic text: “Couch A: surface cleared. Couch B: rail wiped. Occupant fields left blank.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A folded blanket lies across one examination couch; the other has a cup mark on its metal rail. The scene does not assign either object to a patient. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location establishes two examination couches, but no schedule or occupant list. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The empty field remains readable in the next copy. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 011 — Two couches, one room

A folded blanket lies across one examination couch; the other has a cup mark on its metal rail. The scene does not assign either object to a patient. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The location establishes two examination couches, but no schedule or occupant list. Neither voice exists to lecture the player.

Infirmary clerk: “Do we need to name who used it?”
Waiting listener: “Only if the record already knows.”
Infirmary clerk: “The empty field remains readable in the next copy.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 012 — Two couches, one room

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The empty field remains readable in the next copy. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The empty field remains readable in the next copy.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Couch A: surface cleared. Couch B: rail wiped. Occupant fields left blank.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 013 — Burnt coffee by antiseptic

A cup sits outside the supply cabinet, its rim darkened where someone set it down without looking. Antiseptic keeps its sharper smell underneath. Both smells are in the location description; no ingredient or recipe is added. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It has gone cold again.”
Waiting listener: “Then it can wait until the page is done.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Both smells are in the location description; no ingredient or recipe is added. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The ring fades; the note about it does not become a medical record. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 014 — Burnt coffee by antiseptic

Proposed diegetic text: “Cup removed from work surface. Coffee ring left on the ledger cover.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A cup sits outside the supply cabinet, its rim darkened where someone set it down without looking. Antiseptic keeps its sharper smell underneath. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Both smells are in the location description; no ingredient or recipe is added. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The ring fades; the note about it does not become a medical record. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 015 — Burnt coffee by antiseptic

A cup sits outside the supply cabinet, its rim darkened where someone set it down without looking. Antiseptic keeps its sharper smell underneath. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: Both smells are in the location description; no ingredient or recipe is added. Neither voice exists to lecture the player.

Infirmary clerk: “It has gone cold again.”
Waiting listener: “Then it can wait until the page is done.”
Infirmary clerk: “The ring fades; the note about it does not become a medical record.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 016 — Burnt coffee by antiseptic

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The ring fades; the note about it does not become a medical record. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The ring fades; the note about it does not become a medical record.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Cup removed from work surface. Coffee ring left on the ledger cover.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 017 — The dosage ledger grows

The ledger has another line, but the proposed scene never supplies a medicine name or quantity. A clerk checks that the page is aligned before closing the cover. The location says the ledger gets more entries every week; its content is not provided. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It keeps needing another page.”
Waiting listener: “That is not the same as knowing what the page means.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The location says the ledger gets more entries every week; its content is not provided. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A future reader sees the cover closed rather than a new treatment claim. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 018 — The dosage ledger grows

Proposed diegetic text: “Weekly continuation confirmed. No totals copied into this margin.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The ledger has another line, but the proposed scene never supplies a medicine name or quantity. A clerk checks that the page is aligned before closing the cover. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The location says the ledger gets more entries every week; its content is not provided. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A future reader sees the cover closed rather than a new treatment claim. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 019 — The dosage ledger grows

The ledger has another line, but the proposed scene never supplies a medicine name or quantity. A clerk checks that the page is aligned before closing the cover. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The location says the ledger gets more entries every week; its content is not provided. Neither voice exists to lecture the player.

Infirmary clerk: “It keeps needing another page.”
Waiting listener: “That is not the same as knowing what the page means.”
Infirmary clerk: “A future reader sees the cover closed rather than a new treatment claim.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 020 — The dosage ledger grows

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A future reader sees the cover closed rather than a new treatment claim. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A future reader sees the cover closed rather than a new treatment claim.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Weekly continuation confirmed. No totals copied into this margin.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 021 — Pencil on the door

An old line has been rubbed down until the wood looks pale where the writing used to be. The replacement is shorter and dated only if the actual source record supports a date. The quarantine tape says the case definition was written in pencil because staff were still learning. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “We were wrong about one part.”
Waiting listener: “Then we were careful enough to change it.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The quarantine tape says the case definition was written in pencil because staff were still learning. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The changed wording is visible; the old line is not treated as a diagnosis. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 022 — Pencil on the door

Proposed diegetic text: “Case definition: see current pencil notice. Superseded wording not recopied.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: An old line has been rubbed down until the wood looks pale where the writing used to be. The replacement is shorter and dated only if the actual source record supports a date. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The quarantine tape says the case definition was written in pencil because staff were still learning. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The changed wording is visible; the old line is not treated as a diagnosis. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 023 — Pencil on the door

An old line has been rubbed down until the wood looks pale where the writing used to be. The replacement is shorter and dated only if the actual source record supports a date. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The quarantine tape says the case definition was written in pencil because staff were still learning. Neither voice exists to lecture the player.

Infirmary clerk: “We were wrong about one part.”
Waiting listener: “Then we were careful enough to change it.”
Infirmary clerk: “The changed wording is visible; the old line is not treated as a diagnosis.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 024 — Pencil on the door

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The changed wording is visible; the old line is not treated as a diagnosis. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The changed wording is visible; the old line is not treated as a diagnosis.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Case definition: see current pencil notice. Superseded wording not recopied.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 025 — Red means seen today

A wristband is shown only as an example in the scene. The prose repeats the tape’s limited meaning and does not create a new live triage mechanic. The cassette’s red wristband means seen today; blank means waiting. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It is a mark for the day.”
Waiting listener: “Not a measure of the person.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The cassette’s red wristband means seen today; blank means waiting. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later reader can distinguish the mark from an outcome. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 026 — Red means seen today

Proposed diegetic text: “Red: seen today. Blank: waiting. No color is assigned to a person in this draft.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A wristband is shown only as an example in the scene. The prose repeats the tape’s limited meaning and does not create a new live triage mechanic. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The cassette’s red wristband means seen today; blank means waiting. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader can distinguish the mark from an outcome. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 027 — Red means seen today

A wristband is shown only as an example in the scene. The prose repeats the tape’s limited meaning and does not create a new live triage mechanic. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The cassette’s red wristband means seen today; blank means waiting. Neither voice exists to lecture the player.

Infirmary clerk: “It is a mark for the day.”
Waiting listener: “Not a measure of the person.”
Infirmary clerk: “A later reader can distinguish the mark from an outcome.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 028 — Red means seen today

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader can distinguish the mark from an outcome. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later reader can distinguish the mark from an outcome.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Red: seen today. Blank: waiting. No color is assigned to a person in this draft.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 029 — A blank band

The blank band is still on its paper backing. Nobody fills in a name for demonstration. The source defines blank as waiting and nothing more. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “People keep wanting the blank to mean something else.”
Waiting listener: “The tape did not say that.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source defines blank as waiting and nothing more. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The field stays blank after the scene. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 030 — A blank band

Proposed diegetic text: “Unissued wristband. Waiting field remains unfilled.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The blank band is still on its paper backing. Nobody fills in a name for demonstration. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source defines blank as waiting and nothing more. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The field stays blank after the scene. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 031 — A blank band

The blank band is still on its paper backing. Nobody fills in a name for demonstration. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The source defines blank as waiting and nothing more. Neither voice exists to lecture the player.

Infirmary clerk: “People keep wanting the blank to mean something else.”
Waiting listener: “The tape did not say that.”
Infirmary clerk: “The field stays blank after the scene.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 032 — A blank band

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The field stays blank after the scene. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The field stays blank after the scene.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Unissued wristband. Waiting field remains unfilled.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 033 — Arrows before signs

A strip of tape turns on the floor at the point where a visitor might otherwise hesitate. A hand follows the arrow, then stops at the next one. The tapes describe floor arrows because people under fear read arrows better than signs. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The arrows are easier to see.”
Waiting listener: “That is why they are on the floor.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The tapes describe floor arrows because people under fear read arrows better than signs. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The next visit finds a repaired corner, not a new route. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 034 — Arrows before signs

Proposed diegetic text: “IN / OUT — follow the floor marks. Ask before crossing the second arrow.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A strip of tape turns on the floor at the point where a visitor might otherwise hesitate. A hand follows the arrow, then stops at the next one. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The tapes describe floor arrows because people under fear read arrows better than signs. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The next visit finds a repaired corner, not a new route. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 035 — Arrows before signs

A strip of tape turns on the floor at the point where a visitor might otherwise hesitate. A hand follows the arrow, then stops at the next one. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The tapes describe floor arrows because people under fear read arrows better than signs. Neither voice exists to lecture the player.

Infirmary clerk: “The arrows are easier to see.”
Waiting listener: “That is why they are on the floor.”
Infirmary clerk: “The next visit finds a repaired corner, not a new route.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 036 — Arrows before signs

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The next visit finds a repaired corner, not a new route. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The next visit finds a repaired corner, not a new route.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “IN / OUT — follow the floor marks. Ask before crossing the second arrow.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 037 — The separate entrance

A worker stands where the entrance arrows begin and waits for a companion to finish reading. The scene records the pause rather than treating it as refusal. A tape says the clinic has one door in and one door out; it does not specify a new map. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “There is only one clear doorway.”
Waiting listener: “The notice is for the decision, not the wall.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: A tape says the clinic has one door in and one door out; it does not specify a new map. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: Later text can reuse the notice if the same source surface is chosen. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 038 — The separate entrance

Proposed diegetic text: “Notice: use the marked entrance and exit. Staff will answer questions at the threshold.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A worker stands where the entrance arrows begin and waits for a companion to finish reading. The scene records the pause rather than treating it as refusal. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: A tape says the clinic has one door in and one door out; it does not specify a new map. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: Later text can reuse the notice if the same source surface is chosen. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 039 — The separate entrance

A worker stands where the entrance arrows begin and waits for a companion to finish reading. The scene records the pause rather than treating it as refusal. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: A tape says the clinic has one door in and one door out; it does not specify a new map. Neither voice exists to lecture the player.

Infirmary clerk: “There is only one clear doorway.”
Waiting listener: “The notice is for the decision, not the wall.”
Infirmary clerk: “Later text can reuse the notice if the same source surface is chosen.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 040 — The separate entrance

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. Later text can reuse the notice if the same source surface is chosen. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “Later text can reuse the notice if the same source surface is chosen.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Notice: use the marked entrance and exit. Staff will answer questions at the threshold.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 041 — The arithmetic of staff

Two check marks sit beside the staff note; a third line is not filled. The writer stops before turning absence into a roster. The tapes say two volunteers had fevers and describe staffing shortage, but do not name them. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It is not a full count.”
Waiting listener: “It is the count this page can stand behind.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The tapes say two volunteers had fevers and describe staffing shortage, but do not name them. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The note is still bounded when someone reads it aloud. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 042 — The arithmetic of staff

Proposed diegetic text: “Staffing note: two volunteers unwell. Names omitted from this public copy.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: Two check marks sit beside the staff note; a third line is not filled. The writer stops before turning absence into a roster. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The tapes say two volunteers had fevers and describe staffing shortage, but do not name them. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The note is still bounded when someone reads it aloud. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 043 — The arithmetic of staff

Two check marks sit beside the staff note; a third line is not filled. The writer stops before turning absence into a roster. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The tapes say two volunteers had fevers and describe staffing shortage, but do not name them. Neither voice exists to lecture the player.

Infirmary clerk: “It is not a full count.”
Waiting listener: “It is the count this page can stand behind.”
Infirmary clerk: “The note is still bounded when someone reads it aloud.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 044 — The arithmetic of staff

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The note is still bounded when someone reads it aloud. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The note is still bounded when someone reads it aloud.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Staffing note: two volunteers unwell. Names omitted from this public copy.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 045 — Soap borrowed from the pharmacy

A label is copied onto a small card and the original is left where it belongs. The clerk records that the pharmacy gave soap, not how long its stock lasted. The second quarantine tape says the pharmacy provided pale soap to the clinic. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “They gave us what we needed then.”
Waiting listener: “Keep the word then.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The second quarantine tape says the pharmacy provided pale soap to the clinic. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later reader can see the exchange without inferring a permanent supply line. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 046 — Soap borrowed from the pharmacy

Proposed diegetic text: “Pale soap received from pharmacy. Return amount: not recorded here.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A label is copied onto a small card and the original is left where it belongs. The clerk records that the pharmacy gave soap, not how long its stock lasted. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The second quarantine tape says the pharmacy provided pale soap to the clinic. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader can see the exchange without inferring a permanent supply line. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 047 — Soap borrowed from the pharmacy

A label is copied onto a small card and the original is left where it belongs. The clerk records that the pharmacy gave soap, not how long its stock lasted. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The second quarantine tape says the pharmacy provided pale soap to the clinic. Neither voice exists to lecture the player.

Infirmary clerk: “They gave us what we needed then.”
Waiting listener: “Keep the word then.”
Infirmary clerk: “A later reader can see the exchange without inferring a permanent supply line.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 048 — Soap borrowed from the pharmacy

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader can see the exchange without inferring a permanent supply line. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later reader can see the exchange without inferring a permanent supply line.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Pale soap received from pharmacy. Return amount: not recorded here.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 049 — No visitors today

The notice has been pinned level with the latch. Its edges are handled smooth where people have read it and gone back down the corridor. The third tape records a no-visitors order and a woman arguing about her father behind the ward wall. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “A notice can stop a visit.”
Waiting listener: “It cannot stop the reason for one.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The third tape records a no-visitors order and a woman arguing about her father behind the ward wall. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The sentence returns unchanged even if the paper is replaced. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 050 — No visitors today

Proposed diegetic text: “No visitors from today. Questions may be left with the duty clerk.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The notice has been pinned level with the latch. Its edges are handled smooth where people have read it and gone back down the corridor. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The third tape records a no-visitors order and a woman arguing about her father behind the ward wall. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The sentence returns unchanged even if the paper is replaced. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 051 — No visitors today

The notice has been pinned level with the latch. Its edges are handled smooth where people have read it and gone back down the corridor. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The third tape records a no-visitors order and a woman arguing about her father behind the ward wall. Neither voice exists to lecture the player.

Infirmary clerk: “A notice can stop a visit.”
Waiting listener: “It cannot stop the reason for one.”
Infirmary clerk: “The sentence returns unchanged even if the paper is replaced.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 052 — No visitors today

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The sentence returns unchanged even if the paper is replaced. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The sentence returns unchanged even if the paper is replaced.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “No visitors from today. Questions may be left with the duty clerk.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 053 — A question left at the door

A folded note is placed beneath the notice rather than pushed through the treatment curtain. Its author and recipient remain blank in the proposed copy. The tape says a woman argued about her father; it does not identify her or resolve the argument. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “I heard her ask.”
Waiting listener: “That is not the same as hearing the answer.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The tape says a woman argued about her father; it does not identify her or resolve the argument. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The unanswered field can remain a real absence in the archive. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 054 — A question left at the door

Proposed diegetic text: “Question received at the threshold. Reply field left open.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A folded note is placed beneath the notice rather than pushed through the treatment curtain. Its author and recipient remain blank in the proposed copy. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The tape says a woman argued about her father; it does not identify her or resolve the argument. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The unanswered field can remain a real absence in the archive. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 055 — A question left at the door

A folded note is placed beneath the notice rather than pushed through the treatment curtain. Its author and recipient remain blank in the proposed copy. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The tape says a woman argued about her father; it does not identify her or resolve the argument. Neither voice exists to lecture the player.

Infirmary clerk: “I heard her ask.”
Waiting listener: “That is not the same as hearing the answer.”
Infirmary clerk: “The unanswered field can remain a real absence in the archive.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 056 — A question left at the door

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The unanswered field can remain a real absence in the archive. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The unanswered field can remain a real absence in the archive.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Question received at the threshold. Reply field left open.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 057 — The word in the definition

A listener reads the current pencil line twice and asks which word changed. The clerk points only to the source copy, not to a cause the tapes do not establish. The quarantine set says staff kept learning and revised the case definition. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “That line is shorter now.”
Waiting listener: “Shorter can be more honest.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The quarantine set says staff kept learning and revised the case definition. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later copy includes the correction but not a guessed diagnosis. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 058 — The word in the definition

Proposed diegetic text: “Changed wording marked in pencil; reason not extended beyond the tape.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A listener reads the current pencil line twice and asks which word changed. The clerk points only to the source copy, not to a cause the tapes do not establish. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The quarantine set says staff kept learning and revised the case definition. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later copy includes the correction but not a guessed diagnosis. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 059 — The word in the definition

A listener reads the current pencil line twice and asks which word changed. The clerk points only to the source copy, not to a cause the tapes do not establish. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The quarantine set says staff kept learning and revised the case definition. Neither voice exists to lecture the player.

Infirmary clerk: “That line is shorter now.”
Waiting listener: “Shorter can be more honest.”
Infirmary clerk: “A later copy includes the correction but not a guessed diagnosis.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 060 — The word in the definition

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later copy includes the correction but not a guessed diagnosis. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later copy includes the correction but not a guessed diagnosis.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Changed wording marked in pencil; reason not extended beyond the tape.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 061 — Patient number 117

The scene keeps one clinical note in its own folder. A reader notices the patient asked when work could resume, then returns the page to its existing sleeve. The note is for one patient, has a guarded prognosis, and says he would not rest. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The question was about work.”
Waiting listener: “The record answers with rest, not a promise.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The note is for one patient, has a guarded prognosis, and says he would not rest. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: No other patient is made to share this line. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 062 — Patient number 117

Proposed diegetic text: “Patient 117 — note consulted; details remain in the clinical record.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The scene keeps one clinical note in its own folder. A reader notices the patient asked when work could resume, then returns the page to its existing sleeve. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The note is for one patient, has a guarded prognosis, and says he would not rest. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No other patient is made to share this line. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 063 — Patient number 117

The scene keeps one clinical note in its own folder. A reader notices the patient asked when work could resume, then returns the page to its existing sleeve. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The note is for one patient, has a guarded prognosis, and says he would not rest. Neither voice exists to lecture the player.

Infirmary clerk: “The question was about work.”
Waiting listener: “The record answers with rest, not a promise.”
Infirmary clerk: “No other patient is made to share this line.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 064 — Patient number 117

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No other patient is made to share this line. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “No other patient is made to share this line.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Patient 117 — note consulted; details remain in the clinical record.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 065 — Rest is written twice

A clerk sees that the instruction to rest is still on the note’s final line. The proposed dialogue does not claim that the patient complied. The source note says the patient would not rest; it does not establish the later result. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “Would he listen?”
Waiting listener: “This page does not say.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source note says the patient would not rest; it does not establish the later result. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The uncertainty stays after the note is refiled. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 066 — Rest is written twice

Proposed diegetic text: “Rest advised. Compliance not entered in this copy.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A clerk sees that the instruction to rest is still on the note’s final line. The proposed dialogue does not claim that the patient complied. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source note says the patient would not rest; it does not establish the later result. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The uncertainty stays after the note is refiled. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 067 — Rest is written twice

A clerk sees that the instruction to rest is still on the note’s final line. The proposed dialogue does not claim that the patient complied. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The source note says the patient would not rest; it does not establish the later result. Neither voice exists to lecture the player.

Infirmary clerk: “Would he listen?”
Waiting listener: “This page does not say.”
Infirmary clerk: “The uncertainty stays after the note is refiled.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 068 — Rest is written twice

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The uncertainty stays after the note is refiled. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The uncertainty stays after the note is refiled.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Rest advised. Compliance not entered in this copy.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 069 — The register begins at forty-one

The clerk opens the restricted register only long enough to verify its title and page break. The scene does not recite its named entries to a public reader. The discoverable document covers entries 41–58 and is restricted. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “We can say it exists.”
Waiting listener: “We do not have to read it aloud.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The discoverable document covers entries 41–58 and is restricted. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later reader knows the boundary was observed. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 070 — The register begins at forty-one

Proposed diegetic text: “Register access: restricted. Public excerpt not copied.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The clerk opens the restricted register only long enough to verify its title and page break. The scene does not recite its named entries to a public reader. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The discoverable document covers entries 41–58 and is restricted. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader knows the boundary was observed. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 071 — The register begins at forty-one

The clerk opens the restricted register only long enough to verify its title and page break. The scene does not recite its named entries to a public reader. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The discoverable document covers entries 41–58 and is restricted. Neither voice exists to lecture the player.

Infirmary clerk: “We can say it exists.”
Waiting listener: “We do not have to read it aloud.”
Infirmary clerk: “A later reader knows the boundary was observed.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 072 — The register begins at forty-one

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader knows the boundary was observed. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later reader knows the boundary was observed.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Register access: restricted. Public excerpt not copied.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 073 — The smeared middle

A band of ink has blurred across several lines; no one tries to reconstruct the letters from the shapes left behind. The document marks entries 47–52 as smeared beyond recovery. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “There is room to guess.”
Waiting listener: “There is not evidence.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The document marks entries 47–52 as smeared beyond recovery. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The copied note keeps the word unreadable. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 074 — The smeared middle

Proposed diegetic text: “Entries 47–52: unreadable in source. No names restored.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A band of ink has blurred across several lines; no one tries to reconstruct the letters from the shapes left behind. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The document marks entries 47–52 as smeared beyond recovery. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The copied note keeps the word unreadable. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 075 — The smeared middle

A band of ink has blurred across several lines; no one tries to reconstruct the letters from the shapes left behind. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The document marks entries 47–52 as smeared beyond recovery. Neither voice exists to lecture the player.

Infirmary clerk: “There is room to guess.”
Waiting listener: “There is not evidence.”
Infirmary clerk: “The copied note keeps the word unreadable.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 076 — The smeared middle

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The copied note keeps the word unreadable. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The copied note keeps the word unreadable.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Entries 47–52: unreadable in source. No names restored.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 077 — The cut-off line

The final page ends before the clerk reaches a sentence-ending mark. The copy closes there too. The register says entries 53–58 are cut off and the document ends mid-line. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “Could there be another page?”
Waiting listener: “Not in this record.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The register says entries 53–58 are cut off and the document ends mid-line. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: No missing page is invented as a quest clue. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 078 — The cut-off line

Proposed diegetic text: “Register ends mid-line. Continuation unknown.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The final page ends before the clerk reaches a sentence-ending mark. The copy closes there too. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The register says entries 53–58 are cut off and the document ends mid-line. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No missing page is invented as a quest clue. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 079 — The cut-off line

The final page ends before the clerk reaches a sentence-ending mark. The copy closes there too. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The register says entries 53–58 are cut off and the document ends mid-line. Neither voice exists to lecture the player.

Infirmary clerk: “Could there be another page?”
Waiting listener: “Not in this record.”
Infirmary clerk: “No missing page is invented as a quest clue.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 080 — The cut-off line

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No missing page is invented as a quest clue. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “No missing page is invented as a quest clue.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Register ends mid-line. Continuation unknown.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 081 — An epitaph with no owner

A single recovered memorial statement is displayed under its authored provenance. The reader is not asked to match it to a patient number. The discovery manifest labels the identity unresolved and display-only. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It sounds as if it belongs to someone here.”
Waiting listener: “Sound is not a name.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The discovery manifest labels the identity unresolved and display-only. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The later catalog still carries the unresolved label. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 082 — An epitaph with no owner

Proposed diegetic text: “Memorial testimony — identity unresolved. Do not attach to a register entry.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A single recovered memorial statement is displayed under its authored provenance. The reader is not asked to match it to a patient number. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The discovery manifest labels the identity unresolved and display-only. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The later catalog still carries the unresolved label. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 083 — An epitaph with no owner

A single recovered memorial statement is displayed under its authored provenance. The reader is not asked to match it to a patient number. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The discovery manifest labels the identity unresolved and display-only. Neither voice exists to lecture the player.

Infirmary clerk: “It sounds as if it belongs to someone here.”
Waiting listener: “Sound is not a name.”
Infirmary clerk: “The later catalog still carries the unresolved label.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 084 — An epitaph with no owner

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The later catalog still carries the unresolved label. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The later catalog still carries the unresolved label.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Memorial testimony — identity unresolved. Do not attach to a register entry.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 085 — The space after a name

The clerk copies one field label and leaves the space beside it clear. There is no extra annotation explaining the absence. The death register contains both named and unnamed entries; this draft adds none. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “A blank line can feel like work left over.”
Waiting listener: “Sometimes it is the most accurate line.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The death register contains both named and unnamed entries; this draft adds none. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later reader inherits no invented identity. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 086 — The space after a name

Proposed diegetic text: “Name field: as recorded. Unrecorded fields remain unrecorded.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The clerk copies one field label and leaves the space beside it clear. There is no extra annotation explaining the absence. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The death register contains both named and unnamed entries; this draft adds none. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later reader inherits no invented identity. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 087 — The space after a name

The clerk copies one field label and leaves the space beside it clear. There is no extra annotation explaining the absence. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The death register contains both named and unnamed entries; this draft adds none. Neither voice exists to lecture the player.

Infirmary clerk: “A blank line can feel like work left over.”
Waiting listener: “Sometimes it is the most accurate line.”
Infirmary clerk: “A later reader inherits no invented identity.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 088 — The space after a name

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later reader inherits no invented identity. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later reader inherits no invented identity.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Name field: as recorded. Unrecorded fields remain unrecorded.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 089 — Secrecy branch: the page hidden

If the existing quest branch leaves the medic’s illness undisclosed, a later note may reflect the authored delay and its exposure consequences without restaging the choice. The branch outcome belongs to quest_moral_chain_mercy_12. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The work went on for eighteen more days.”
Waiting listener: “This is the branch that says so.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The branch outcome belongs to quest_moral_chain_mercy_12. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The callback cites the existing outcome, not a new clock. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 090 — Secrecy branch: the page hidden

Proposed diegetic text: “Branch reference only: keep the existing authored consequence text.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: If the existing quest branch leaves the medic’s illness undisclosed, a later note may reflect the authored delay and its exposure consequences without restaging the choice. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The branch outcome belongs to quest_moral_chain_mercy_12. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The callback cites the existing outcome, not a new clock. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 091 — Secrecy branch: the page hidden

If the existing quest branch leaves the medic’s illness undisclosed, a later note may reflect the authored delay and its exposure consequences without restaging the choice. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The branch outcome belongs to quest_moral_chain_mercy_12. Neither voice exists to lecture the player.

Infirmary clerk: “The work went on for eighteen more days.”
Waiting listener: “This is the branch that says so.”
Infirmary clerk: “The callback cites the existing outcome, not a new clock.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 092 — Secrecy branch: the page hidden

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The callback cites the existing outcome, not a new clock. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The callback cites the existing outcome, not a new clock.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Branch reference only: keep the existing authored consequence text.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 093 — Disclosure branch: work behind a sheet

If the existing branch discloses the illness and allows isolated work, the note may show a screen being set where the existing quest already says it was. The branch outcome belongs to the same moral quest; no extra patient is added. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The room changed its arrangement.”
Waiting listener: “The people in it still had to do the work.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The branch outcome belongs to the same moral quest; no extra patient is added. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A later visit shows only the branch already stored by its owner. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 094 — Disclosure branch: work behind a sheet

Proposed diegetic text: “Branch reference only: isolated work as authored; no added contact count.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: If the existing branch discloses the illness and allows isolated work, the note may show a screen being set where the existing quest already says it was. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The branch outcome belongs to the same moral quest; no extra patient is added. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A later visit shows only the branch already stored by its owner. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 095 — Disclosure branch: work behind a sheet

If the existing branch discloses the illness and allows isolated work, the note may show a screen being set where the existing quest already says it was. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The branch outcome belongs to the same moral quest; no extra patient is added. Neither voice exists to lecture the player.

Infirmary clerk: “The room changed its arrangement.”
Waiting listener: “The people in it still had to do the work.”
Infirmary clerk: “A later visit shows only the branch already stored by its owner.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 096 — Disclosure branch: work behind a sheet

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A later visit shows only the branch already stored by its owner. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A later visit shows only the branch already stored by its owner.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Branch reference only: isolated work as authored; no added contact count.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 097 — The quarantine choice remains a choice

A draft reader sees that the quest has a separate quarantine option and stops before writing a competing outcome. The moral quest offers three choices; this plan does not simplify them to two. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “A new notice could make it simpler.”
Waiting listener: “It would also make it false.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The moral quest offers three choices; this plan does not simplify them to two. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The follow-up remains bound to the original authored branch. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 098 — The quarantine choice remains a choice

Proposed diegetic text: “Choice text remains in the existing quest. No substitute is posted.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A draft reader sees that the quest has a separate quarantine option and stops before writing a competing outcome. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The moral quest offers three choices; this plan does not simplify them to two. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The follow-up remains bound to the original authored branch. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 099 — The quarantine choice remains a choice

A draft reader sees that the quest has a separate quarantine option and stops before writing a competing outcome. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The moral quest offers three choices; this plan does not simplify them to two. Neither voice exists to lecture the player.

Infirmary clerk: “A new notice could make it simpler.”
Waiting listener: “It would also make it false.”
Infirmary clerk: “The follow-up remains bound to the original authored branch.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 100 — The quarantine choice remains a choice

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The follow-up remains bound to the original authored branch. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The follow-up remains bound to the original authored branch.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Choice text remains in the existing quest. No substitute is posted.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 101 — One dose, two names not supplied here

A small card repeats that the Healer’s Dilemma is about one available dose and two people. The plan’s card does not name either person. The existing quest specifies a scout and a stranger’s child; this plan adds no identity. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The page does not choose for them.”
Waiting listener: “The player already had that choice.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The existing quest specifies a scout and a stranger’s child; this plan adds no identity. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The current quest result, if exposed, controls only its own callback. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 102 — One dose, two names not supplied here

Proposed diegetic text: “Allocation choice: refer to the owning quest; no clinical quantities copied.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A small card repeats that the Healer’s Dilemma is about one available dose and two people. The plan’s card does not name either person. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The existing quest specifies a scout and a stranger’s child; this plan adds no identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The current quest result, if exposed, controls only its own callback. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 103 — One dose, two names not supplied here

A small card repeats that the Healer’s Dilemma is about one available dose and two people. The plan’s card does not name either person. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The existing quest specifies a scout and a stranger’s child; this plan adds no identity. Neither voice exists to lecture the player.

Infirmary clerk: “The page does not choose for them.”
Waiting listener: “The player already had that choice.”
Infirmary clerk: “The current quest result, if exposed, controls only its own callback.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 104 — One dose, two names not supplied here

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The current quest result, if exposed, controls only its own callback. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The current quest result, if exposed, controls only its own callback.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Allocation choice: refer to the owning quest; no clinical quantities copied.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 105 — The scout’s next morning

A conditional draft uses the existing branch’s aftermath as a remembered sentence, then ends before treating survival as a broader rule. The outcome belongs to quest_moral_chain_mercy_14. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “A person can live and still carry the result.”
Waiting listener: “That is what this line remembers.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The outcome belongs to quest_moral_chain_mercy_14. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: No new prognosis is attached to the existing outcome. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 106 — The scout’s next morning

Proposed diegetic text: “Branch callback: preserve the scout’s authored aftermath as written.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A conditional draft uses the existing branch’s aftermath as a remembered sentence, then ends before treating survival as a broader rule. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The outcome belongs to quest_moral_chain_mercy_14. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: No new prognosis is attached to the existing outcome. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 107 — The scout’s next morning

A conditional draft uses the existing branch’s aftermath as a remembered sentence, then ends before treating survival as a broader rule. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The outcome belongs to quest_moral_chain_mercy_14. Neither voice exists to lecture the player.

Infirmary clerk: “A person can live and still carry the result.”
Waiting listener: “That is what this line remembers.”
Infirmary clerk: “No new prognosis is attached to the existing outcome.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 108 — The scout’s next morning

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. No new prognosis is attached to the existing outcome. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “No new prognosis is attached to the existing outcome.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Branch callback: preserve the scout’s authored aftermath as written.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 109 — A corridor waits for succession

The scene starts after the existing succession encounter has resolved. People keep walking through the infirmary corridor while a name is carried elsewhere. The successor quest has its own candidates and branch outcomes. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “The corridor is moving again.”
Waiting listener: “It never stopped needing someone.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The successor quest has its own candidates and branch outcomes. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The prose recognizes the current branch without holding another vote. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 110 — A corridor waits for succession

Proposed diegetic text: “Successor reference: use only the selected existing branch.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The scene starts after the existing succession encounter has resolved. People keep walking through the infirmary corridor while a name is carried elsewhere. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The successor quest has its own candidates and branch outcomes. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The prose recognizes the current branch without holding another vote. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 111 — A corridor waits for succession

The scene starts after the existing succession encounter has resolved. People keep walking through the infirmary corridor while a name is carried elsewhere. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The successor quest has its own candidates and branch outcomes. Neither voice exists to lecture the player.

Infirmary clerk: “The corridor is moving again.”
Waiting listener: “It never stopped needing someone.”
Infirmary clerk: “The prose recognizes the current branch without holding another vote.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 112 — A corridor waits for succession

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The prose recognizes the current branch without holding another vote. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The prose recognizes the current branch without holding another vote.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Successor reference: use only the selected existing branch.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 113 — A cup moved off the ledger

The next shift shifts a cup away from the dosage book before opening it. The gesture is ordinary and is not scored as care. Coffee and the growing ledger are location facts; no daily routine is specified. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “There is room to write now.”
Waiting listener: “For this line, yes.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: Coffee and the growing ledger are location facts; no daily routine is specified. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The next page begins without a new number in this plan. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 114 — A cup moved off the ledger

Proposed diegetic text: “Work surface cleared before the next entry.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The next shift shifts a cup away from the dosage book before opening it. The gesture is ordinary and is not scored as care. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: Coffee and the growing ledger are location facts; no daily routine is specified. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The next page begins without a new number in this plan. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 115 — A cup moved off the ledger

The next shift shifts a cup away from the dosage book before opening it. The gesture is ordinary and is not scored as care. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: Coffee and the growing ledger are location facts; no daily routine is specified. Neither voice exists to lecture the player.

Infirmary clerk: “There is room to write now.”
Waiting listener: “For this line, yes.”
Infirmary clerk: “The next page begins without a new number in this plan.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 116 — A cup moved off the ledger

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The next page begins without a new number in this plan. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The next page begins without a new number in this plan.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Work surface cleared before the next entry.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 117 — A curtain folded back

The curtain is folded to one side after a conversation, not because privacy has been solved but because the staff need the room. The source says the curtain divides treatment from triage only psychologically. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “It was never a wall.”
Waiting listener: “It still gave people a moment.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: The source says the curtain divides treatment from triage only psychologically. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: The callback keeps that small difference without overstating it. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 118 — A curtain folded back

Proposed diegetic text: “Curtain open for work. Questions may wait until the room is ready.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: The curtain is folded to one side after a conversation, not because privacy has been solved but because the staff need the room. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: The source says the curtain divides treatment from triage only psychologically. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: The callback keeps that small difference without overstating it. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 119 — A curtain folded back

The curtain is folded to one side after a conversation, not because privacy has been solved but because the staff need the room. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: The source says the curtain divides treatment from triage only psychologically. Neither voice exists to lecture the player.

Infirmary clerk: “It was never a wall.”
Waiting listener: “It still gave people a moment.”
Infirmary clerk: “The callback keeps that small difference without overstating it.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 120 — A curtain folded back

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. The callback keeps that small difference without overstating it. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “The callback keeps that small difference without overstating it.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “Curtain open for work. Questions may wait until the room is ready.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

### Scene draft 121 — The page kept for morning

A closed ledger lies beneath the edge of the curtain. The last line is visible only to the person who must continue it. No new author, date, dosage, or medical outcome is supplied. The scene stays at the described scale. It uses an existing character only where a cited source already names one; any other voice remains an editorial role. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech.

Infirmary clerk: “We cannot finish the week tonight.”
Waiting listener: “Then do not call it finished.”

The player’s place in this beat is a readable pause: notice the object, ask a narrow question, or leave the work with the person already doing it. Existing decisions and state remain with their source owner. Let the physical detail carry the emotion: No new author, date, dosage, or medical outcome is supplied. Stop before a character supplies an answer the record does not contain.

Drafting boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer.

Scene close: A future reader receives an unfinished record, not a false resolution. The blank line remains a boundary, not an invitation to guess. The passage is a candidate game-text scene, not evidence of a new route, visit state, resource result, or system effect.

### Record and return 122 — The page kept for morning

Proposed diegetic text: “To morning shift: continue from the source page. No totals carried forward here.”

Proposed author and audience: Unidentified reader at the curtain; someone waiting beyond the curtain. The artifact exists in this proposal for a practical reason: A closed ledger lies beneath the edge of the curtain. The last line is visible only to the person who must continue it. Its final form and placement must wait for the current content owner and schema to be verified. This plan does not declare that the artifact already exists.

Reading context: No new author, date, dosage, or medical outcome is supplied. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. A later reader must be able to tell the source fact from the proposed observation. Leave authorship, date, count, and condition blank when the cited record does not provide them.

Later reading: A future reader receives an unfinished record, not a false resolution. A later shift can read the correction without inheriting the decision. The callback changes how a reader understands the wording; it does not assert a new gameplay result.

Editorial limit: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Preserve attribution and keep unknown fields visibly unknown.

### Conversation fragment 123 — The page kept for morning

A closed ledger lies beneath the edge of the curtain. The last line is visible only to the person who must continue it. The exchange may use an existing named character only where the cited record supports that character at this location. Any other speaker is an editorial role, not a new canonical identity. Keep the duty clerk exact about fields and uncertain about causes; keep a tired listener practical, never omniscient. The cassette voice is already authored and must not be ventriloquized as a new speaker. Any new line is an editorial candidate, not a recovered tape transcript. Let blank spaces and corrected pencil explain more than a speech. Keep the conversation attached to the work already in the scene: No new author, date, dosage, or medical outcome is supplied. Neither voice exists to lecture the player.

Infirmary clerk: “We cannot finish the week tonight.”
Waiting listener: “Then do not call it finished.”
Infirmary clerk: “A future reader receives an unfinished record, not a false resolution.”

A player response can be a short question, a correction about what they read, or silence. The other speaker answers only what they can know from the cited record and this proposed moment. Let the page, weather, or work take attention away before the conversation becomes an explanation of the whole world.

Continuity check: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Use this as an optional text passage only if an authorized content owner establishes its placement; it is not a new dialogue route.

### Consequence vignette 124 — The page kept for morning

If an existing, verified content surface lets the player revisit this material, the passage may return to the earlier wording. The current records do not prove a new route or revisit trigger. A future reader receives an unfinished record, not a false resolution. A record can preserve work without making a private life public. The change in understanding comes from a known source, an existing branch, or a later reading—not a new mechanical result.

Possible later line — Infirmary clerk: “A future reader receives an unfinished record, not a false resolution.”

The second reader can leave the note, add a narrow correction supported by current facts, or decline to circulate it. Those are editorial presentation choices, not new gameplay choices. No passage silently changes a location, route, resource, faction, character, quest, or save state.

Record left in view: “To morning shift: continue from the source page. No totals carried forward here.”

Return boundary: Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. Candidate placement is adjacent to existing infirmary quest and discovery content only after its live owner and consumer are verified. The plan does not add a new patient, diagnosis, medical action, route, status, flag, or cassette set. It can supply optional reading or branch-aware text if the existing system exposes the already-authored result. Do not present this as a universal epilogue; the current route and content condition must first be verified by their owner.

## 15. Beat selection index

| Beat | Editorial focus | Candidate in-world artifact | Continuity limit |
|---:|---|---|---|
| 01 | The seam in the curtain | Treatment / Triage — please lower your voice, not your concern. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 02 | The hum without the main power | Light noted. Hum noted. Cause not entered. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 03 | Two couches, one room | Couch A: surface cleared. Couch B: rail wiped. Occupant fields left blank. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 04 | Burnt coffee by antiseptic | Cup removed from work surface. Coffee ring left on the ledger cover. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 05 | The dosage ledger grows | Weekly continuation confirmed. No totals copied into this margin. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 06 | Pencil on the door | Case definition: see current pencil notice. Superseded wording not recopied. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 07 | Red means seen today | Red: seen today. Blank: waiting. No color is assigned to a person in this draft. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 08 | A blank band | Unissued wristband. Waiting field remains unfilled. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 09 | Arrows before signs | IN / OUT — follow the floor marks. Ask before crossing the second arrow. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 10 | The separate entrance | Notice: use the marked entrance and exit. Staff will answer questions at the threshold. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 11 | The arithmetic of staff | Staffing note: two volunteers unwell. Names omitted from this public copy. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 12 | Soap borrowed from the pharmacy | Pale soap received from pharmacy. Return amount: not recorded here. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 13 | No visitors today | No visitors from today. Questions may be left with the duty clerk. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 14 | A question left at the door | Question received at the threshold. Reply field left open. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 15 | The word in the definition | Changed wording marked in pencil; reason not extended beyond the tape. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 16 | Patient number 117 | Patient 117 — note consulted; details remain in the clinical record. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 17 | Rest is written twice | Rest advised. Compliance not entered in this copy. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 18 | The register begins at forty-one | Register access: restricted. Public excerpt not copied. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 19 | The smeared middle | Entries 47–52: unreadable in source. No names restored. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 20 | The cut-off line | Register ends mid-line. Continuation unknown. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 21 | An epitaph with no owner | Memorial testimony — identity unresolved. Do not attach to a register entry. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 22 | The space after a name | Name field: as recorded. Unrecorded fields remain unrecorded. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 23 | Secrecy branch: the page hidden | Branch reference only: keep the existing authored consequence text. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 24 | Disclosure branch: work behind a sheet | Branch reference only: isolated work as authored; no added contact count. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 25 | The quarantine choice remains a choice | Choice text remains in the existing quest. No substitute is posted. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 26 | One dose, two names not supplied here | Allocation choice: refer to the owning quest; no clinical quantities copied. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 27 | The scout’s next morning | Branch callback: preserve the scout’s authored aftermath as written. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 28 | A corridor waits for succession | Successor reference: use only the selected existing branch. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 29 | A cup moved off the ledger | Work surface cleared before the next entry. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 30 | A curtain folded back | Curtain open for work. Questions may wait until the room is ready. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |
| 31 | The page kept for morning | To morning shift: continue from the source page. No totals carried forward here. | Do not add clinical advice, dosing, a pathogen name, prognosis, or treatment guidance. Do not repeat the death register’s most intimate entries for shock. Never infer a patient identity from a blank or damaged line. Do not turn a memorial record into a live casualty state. Do not replace the existing moral-choice outcomes with a simpler right answer. |

## 16. Existing branch hooks and consequence boundaries

These are content-selection notes, not new conditions or state. The existence of a record in JSON does not prove its consumer. Verify the current owning system and its exposed state before choosing a branch-specific passage.

| Existing source | Current authored distinction | Draft boundary |
|---|---|---|
| `quest_moral_chain_mercy_12` | secret kept / truth disclosed with isolated work / quarantine choice | Three existing outcomes remain fixed; candidate callback only reflects the current authored branch. |
| `quest_moral_chain_mercy_14` | one chelation dose allocated between two people | Do not add a third recipient or make either existing result reversible. |
| `quest_moral_chain_mercy_24` | successor choice | A later corridor conversation may reflect the existing choice; this plan adds no vote or succession state. |

## 17. Collision and unresolved authority

This site has several records whose dates, subjects, and provenance differ. In particular, the quarantine tapes’ case-definition voice, the restricted death register, the canonical patient note, and the unresolved epitaph must not be fused into a single biography or timeline. Existing choice outcomes may be referenced only as outcomes already authored in their owning quest. The plan deliberately leaves clinical and identity gaps open. If a future implementation needs a single authoritative answer, pause content selection until the current owner resolves the source conflict. No text in this plan is that resolution.

## 18. Review checklist

Before selecting a passage, compare it again with the source rows named in section 3. Keep source facts and existing choices exact, mark proposed voices as editorial until approved, and independently verify any route, text consumer, or state condition. Do not infer reachability from a location description. Read every line aloud for role-appropriate vocabulary, remove any sentence that sounds like a feature spec, and keep each artifact’s author, audience, and purpose plausible.

## 19. Acceptance boundary

This plan is complete as a game-content proposal when selected passages can be traced to the cited location or linked records, existing choices and consequences remain unchanged, no unknown is promoted into canon, and an existing owner is identified for any implementation. If that owner or consumer is absent, retain the prose as a draft rather than inventing a route or system. Character counts are Unicode code-point counts of the saved Markdown files.
