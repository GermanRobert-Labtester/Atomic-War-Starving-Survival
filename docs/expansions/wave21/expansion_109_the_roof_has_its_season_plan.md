# EXPANSION 109 — The Roof Has Its Season

## Dalia Marun, tested seed lots, a roof that is not fully trusted, and the private tin she keeps outside the settlement audit.

### Wave 21: Records Kept in Human Hands

## 1. Expansion thesis

Make the seed annex a place where time is handled carefully, not a storehouse that grants a promised harvest. Dalia tests lots, keeps a schedule that admits defeat, and will not issue untested seed. The player’s current choice is whether to bring timber and re-sack the lots before the season turns or wait for the better season everyone wants. This is a prose-first game-content plan. Its proposed deliverable is scenes, conversations, marginal notes, and conditional return passages that deepen the existing character quest. It adds no gameplay feature and does not claim that any proposed text is already in the game.

## 2. Story in one sentence

A seed keeper shows the visitor a year that has not happened yet and asks whether they will help protect its chance without asking to own it.

## 3. Verified local anchor and current story

`characters.json` defines `npc_dalia_marun` as a Seed Keeper at `loc_seed_library_annex`; she wants grain seed, sack cloth, and a dry season, offers viable stock and planting schedules that admit defeat, and will not issue untested lots. `npc_arcs.json` records initial, evolved, late-kept, late-lost, and late-unmet states. `narrative_encounters_npc_arcs.json` registers `enc_arc_dalia_01_annex`; `quests_npc_arcs.json` registers `quest_arc_dalia_01_annex`. The existing choice IDs are `dalia_haul_roof` and `dalia_wait`. The haul path repairs the roof and re-sacks lots, then Dalia cuts from her private tin for the player’s planters at an unchanging rate; the later kept state says the freehold failed but the annex did not. Waiting leads to wet inventory after the roof fails on the season’s timetable. There is a source tension: `locations.json` describes the librarian as gone while the character and quest data place Dalia at the annex. This plan does not identify Dalia as that librarian or rewrite either record; place character scenes only through the authored encounter when its current presentation allows. Names, locations, quest IDs, choice IDs, and state summaries above come from the current data. The encounter catalog proves the authored scene and choices; it does not prove that every optional callback below is already reachable. Keep proposed text behind the exact existing state condition.

## 4. Fixed canon and proposed prose

Dalia names seed varieties like relatives, shows germination rates before the door, and refuses to leave while the roof holds. Her private tin has never been weighed by the settlement audit and is not for discussion. Tested lots, the current roof, the two existing choices, and their late states are fixed. Do not invent crop yields, seed counts, the tin’s contents, or a guarantee that a season succeeds. Existing choices remain the decision authority. The fragments below are candidate content, not an amendment to the character record, a new outcome, or a new account of an unnamed person.

## 5. Human center

Dalia’s care is not sentiment detached from work. Each label connects a variety, a year, and a harvest; a roof failure can erase the usefulness of a careful inventory. She protects her own limits as rigorously as the lots. The player may help repair what the quest names without earning access to what she keeps private. The character is not a puzzle whose private fact the player earns by being persistent. Let the player notice the labor around the choice and leave with uncertainty when the person whose life is involved chooses not to explain.

## 6. Voice and point of view

Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Keep the prose near what a person can see, hear, count, carry, decline, or write down. Avoid a narrator who explains the character’s symbolism. Ordinary work should continue even when the player leaves.

## 7. Placement in the current story

Use beats 1–5 with `enc_arc_dalia_01_annex`. Beats 6–10 are conditional on `dalia_haul_roof`, from `evolved` through `late_kept`. Beats 11–14 apply only to `dalia_wait` and `late_lost`. Remaining beats may support the initial or unmet state if they do not resolve the source tension about the location’s librarian. Do not identify Dalia as the absent librarian without an authoritative content decision. Each proposed beat has four editorial forms: scene, diegetic record, conversation, and later vignette. They are alternatives for one story moment, not four mandatory encounters. Use a current encounter or character-location owner as the insertion point; do not create a parallel quest chain or a second mutable owner.

## 8. Player agency and consequence

The existing choice is to haul timber and re-sack the lots before the season turns, or wait for a better season. Do not imply that the player chooses a planting method, controls the private tin, or can force Dalia to leave. The future consequence is already specified by the current quest and arc; prose may make the work visible but may not add seed yield. Preserve the current choice text and outcomes. The player may agree, refuse, witness, ask a practical question, or leave where the scene allows. Prose may clarify stakes before the choice or reflect a branch after it, but it may not secretly change that branch.

## 9. Continuity, dignity, and safety

Seed handling, germination tests, and radiation exposure remain non-instructional. The plan adds no planting calendar or crop outcome beyond what the current state names. Protect private identities and preserve the source’s unknowns. Do not turn the location, medical, food, security, financial, or archival context into a tutorial or a new operational procedure.

## 10. Integration boundary

This plan changes no production code, JSON, quest or encounter identifier, location, state rule, resource amount, schedule, save field, or system. If an editor selects a fragment, verify the current schema and state consumer and place it under the existing owner. The plan itself is review material.

## 11. Content bank: proposed prose

The sections are a drafting bank. A writer may select one form per beat, shorten it, or leave it unused. The plan is complete as editorial material even when an implementation later chooses a smaller, coherent subset.

### Scene draft 001 — Labels borrow the old card catalogue

At the annex, envelopes have a variety, year, and harvest written in pencil. The card catalogue has been repurposed and kept current. Dalia lets the visitor read a label before explaining why the old category still matters. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “Everything in here is a year that has not happened yet. Do not hurry it into a promise.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not invent a variety, quantity, or successful future crop. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the three label parts distinct if this detail returns. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 002 — Labels borrow the old card catalogue

Proposed diegetic text: “Shelf label: Variety, year, harvest; pencil retained for the current record.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the annex, envelopes have a variety, year, and harvest written in pencil. The card catalogue has been repurposed and kept current. Dalia lets the visitor read a label before explaining why the old category still matters. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not invent a variety, quantity, or successful future crop. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the three label parts distinct if this detail returns. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 003 — Labels borrow the old card catalogue

At the annex, envelopes have a variety, year, and harvest written in pencil. The card catalogue has been repurposed and kept current. Dalia lets the visitor read a label before explaining why the old category still matters. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “Everything in here is a year that has not happened yet. Do not hurry it into a promise.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Keep the three label parts distinct if this detail returns.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not invent a variety, quantity, or successful future crop. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 004 — Labels borrow the old card catalogue

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the annex, envelopes have a variety, year, and harvest written in pencil. The card catalogue has been repurposed and kept current. Dalia lets the visitor read a label before explaining why the old category still matters. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Keep the three label parts distinct if this detail returns. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not invent a variety, quantity, or successful future crop.

Dalia Marun may say: “Everything in here is a year that has not happened yet. Do not hurry it into a promise.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Shelf label: Variety, year, harvest; pencil retained for the current record. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 005 — The test comes before the door

Dalia shows germination rates before she shows the door. The visitor can read that she has tests without receiving a step-by-step method or a claim that every lot will grow. The work is presented as her standard, not a system for the player to operate. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “I will show you what was tested. I will not tell you a seed is a harvest.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No rate, procedure, or crop promise is added. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: A later visit may display a current test only if source content supplies one. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 006 — The test comes before the door

Proposed diegetic text: “Test note: Germination rate shown before access; no rate copied into this proposal.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Dalia shows germination rates before she shows the door. The visitor can read that she has tests without receiving a step-by-step method or a claim that every lot will grow. The work is presented as her standard, not a system for the player to operate. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No rate, procedure, or crop promise is added. Do not let the form claim authority that its keeper has not been given.

Later reading: A later visit may display a current test only if source content supplies one. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 007 — The test comes before the door

Dalia shows germination rates before she shows the door. The visitor can read that she has tests without receiving a step-by-step method or a claim that every lot will grow. The work is presented as her standard, not a system for the player to operate. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “I will show you what was tested. I will not tell you a seed is a harvest.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “A later visit may display a current test only if source content supplies one.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No rate, procedure, or crop promise is added. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 008 — The test comes before the door

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Dalia shows germination rates before she shows the door. The visitor can read that she has tests without receiving a step-by-step method or a claim that every lot will grow. The work is presented as her standard, not a system for the player to operate. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: A later visit may display a current test only if source content supplies one. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No rate, procedure, or crop promise is added.

Dalia Marun may say: “I will show you what was tested. I will not tell you a seed is a harvest.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Test note: Germination rate shown before access; no rate copied into this proposal. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 009 — The roof beam argues with the wall

At the point where the beam has begun to argue with the wall, Dalia stops naming varieties. The pause is practical: the building is part of the work that keeps each lot legible and dry. The scene does not stage a collapse or specify a repair technique. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “The roof is not asking for a speech. It is asking what you brought.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add structural engineering directions or a new damage threshold. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep this image at the existing encounter’s stated uncertainty. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 010 — The roof beam argues with the wall

Proposed diegetic text: “Roof margin: Beam and wall movement observed; repair choice remains open.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: At the point where the beam has begun to argue with the wall, Dalia stops naming varieties. The pause is practical: the building is part of the work that keeps each lot legible and dry. The scene does not stage a collapse or specify a repair technique. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add structural engineering directions or a new damage threshold. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep this image at the existing encounter’s stated uncertainty. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 011 — The roof beam argues with the wall

At the point where the beam has begun to argue with the wall, Dalia stops naming varieties. The pause is practical: the building is part of the work that keeps each lot legible and dry. The scene does not stage a collapse or specify a repair technique. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “The roof is not asking for a speech. It is asking what you brought.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Keep this image at the existing encounter’s stated uncertainty.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add structural engineering directions or a new damage threshold. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 012 — The roof beam argues with the wall

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. At the point where the beam has begun to argue with the wall, Dalia stops naming varieties. The pause is practical: the building is part of the work that keeps each lot legible and dry. The scene does not stage a collapse or specify a repair technique. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Keep this image at the existing encounter’s stated uncertainty. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add structural engineering directions or a new damage threshold.

Dalia Marun may say: “The roof is not asking for a speech. It is asking what you brought.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Roof margin: Beam and wall movement observed; repair choice remains open. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 013 — A private tin has no public label

The settlement audit has never weighed the private tin in Dalia’s collection. A visitor asks if it belongs on the shelf list. Dalia leaves the tin outside the conversation; the story does not reveal its contents to reward a better question. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “No. The tin is not for discussion. You have the answer already.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not name its contents, quantity, or destination. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Every branch preserves this boundary unless source data explicitly changes it. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 014 — A private tin has no public label

Proposed diegetic text: “Collection note: Private tin excluded from the settlement audit.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The settlement audit has never weighed the private tin in Dalia’s collection. A visitor asks if it belongs on the shelf list. Dalia leaves the tin outside the conversation; the story does not reveal its contents to reward a better question. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not name its contents, quantity, or destination. Do not let the form claim authority that its keeper has not been given.

Later reading: Every branch preserves this boundary unless source data explicitly changes it. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 015 — A private tin has no public label

The settlement audit has never weighed the private tin in Dalia’s collection. A visitor asks if it belongs on the shelf list. Dalia leaves the tin outside the conversation; the story does not reveal its contents to reward a better question. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “No. The tin is not for discussion. You have the answer already.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Every branch preserves this boundary unless source data explicitly changes it.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not name its contents, quantity, or destination. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 016 — A private tin has no public label

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The settlement audit has never weighed the private tin in Dalia’s collection. A visitor asks if it belongs on the shelf list. Dalia leaves the tin outside the conversation; the story does not reveal its contents to reward a better question. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Every branch preserves this boundary unless source data explicitly changes it. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not name its contents, quantity, or destination.

Dalia Marun may say: “No. The tin is not for discussion. You have the answer already.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Collection note: Private tin excluded from the settlement audit. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 017 — The lots are tested, not promised

A planter asks whether a tested lot guarantees food. Dalia describes the limit of the word tested: it belongs to the lot and its recorded check, not to weather or future yield. The answer can be disappointing without making her sound evasive. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “I can tell you what the test says. I cannot sell you a harvest in advance.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No yield, planting instruction, or guarantee. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the schedule’s admission of defeat in any later offer. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 018 — The lots are tested, not promised

Proposed diegetic text: “Seed note: Tested lot; yield and season remain unknown.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: A planter asks whether a tested lot guarantees food. Dalia describes the limit of the word tested: it belongs to the lot and its recorded check, not to weather or future yield. The answer can be disappointing without making her sound evasive. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No yield, planting instruction, or guarantee. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the schedule’s admission of defeat in any later offer. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 019 — The lots are tested, not promised

A planter asks whether a tested lot guarantees food. Dalia describes the limit of the word tested: it belongs to the lot and its recorded check, not to weather or future yield. The answer can be disappointing without making her sound evasive. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “I can tell you what the test says. I cannot sell you a harvest in advance.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Keep the schedule’s admission of defeat in any later offer.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No yield, planting instruction, or guarantee. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 020 — The lots are tested, not promised

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. A planter asks whether a tested lot guarantees food. Dalia describes the limit of the word tested: it belongs to the lot and its recorded check, not to weather or future yield. The answer can be disappointing without making her sound evasive. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Keep the schedule’s admission of defeat in any later offer. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No yield, planting instruction, or guarantee.

Dalia Marun may say: “I can tell you what the test says. I cannot sell you a harvest in advance.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Seed note: Tested lot; yield and season remain unknown. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 021 — Timber crosses the threshold

After `dalia_haul_roof`, timber is brought over the annex threshold. The prose stays with the weight of carrying and Dalia’s sequence of work; it does not turn the action into a construction guide. The player helps because the registered choice says so. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “Set it there. We will use it when the next step is ready.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new materials, amount, recipe, or repair procedure. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Display only after `dalia_haul_roof`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 022 — Timber crosses the threshold

Proposed diegetic text: “Work tally: Timber brought; annex roof work begun under current choice.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After `dalia_haul_roof`, timber is brought over the annex threshold. The prose stays with the weight of carrying and Dalia’s sequence of work; it does not turn the action into a construction guide. The player helps because the registered choice says so. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new materials, amount, recipe, or repair procedure. Do not let the form claim authority that its keeper has not been given.

Later reading: Display only after `dalia_haul_roof`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 023 — Timber crosses the threshold

After `dalia_haul_roof`, timber is brought over the annex threshold. The prose stays with the weight of carrying and Dalia’s sequence of work; it does not turn the action into a construction guide. The player helps because the registered choice says so. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “Set it there. We will use it when the next step is ready.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Display only after `dalia_haul_roof`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new materials, amount, recipe, or repair procedure. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 024 — Timber crosses the threshold

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After `dalia_haul_roof`, timber is brought over the annex threshold. The prose stays with the weight of carrying and Dalia’s sequence of work; it does not turn the action into a construction guide. The player helps because the registered choice says so. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Display only after `dalia_haul_roof`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new materials, amount, recipe, or repair procedure.

Dalia Marun may say: “Set it there. We will use it when the next step is ready.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Work tally: Timber brought; annex roof work begun under current choice. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 025 — The sacks are re-made around the lots

In the evolved Roof Season state, lots have been re-sacked. The scene may show a pile of old ties beside new sacks without changing how much viable stock exists. Dalia continues to count the labels before she touches the next bundle. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “A better sack keeps a lot dry. It does not change what the lot is.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No seed count, quality upgrade, or bonus yield. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use with the evolved state only. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 026 — The sacks are re-made around the lots

Proposed diegetic text: “Inventory note: Lots re-sacked; tested status remains as authored.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In the evolved Roof Season state, lots have been re-sacked. The scene may show a pile of old ties beside new sacks without changing how much viable stock exists. Dalia continues to count the labels before she touches the next bundle. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No seed count, quality upgrade, or bonus yield. Do not let the form claim authority that its keeper has not been given.

Later reading: Use with the evolved state only. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 027 — The sacks are re-made around the lots

In the evolved Roof Season state, lots have been re-sacked. The scene may show a pile of old ties beside new sacks without changing how much viable stock exists. Dalia continues to count the labels before she touches the next bundle. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “A better sack keeps a lot dry. It does not change what the lot is.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Use with the evolved state only.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No seed count, quality upgrade, or bonus yield. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 028 — The sacks are re-made around the lots

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In the evolved Roof Season state, lots have been re-sacked. The scene may show a pile of old ties beside new sacks without changing how much viable stock exists. Dalia continues to count the labels before she touches the next bundle. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Use with the evolved state only. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No seed count, quality upgrade, or bonus yield.

Dalia Marun may say: “A better sack keeps a lot dry. It does not change what the lot is.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Inventory note: Lots re-sacked; tested status remains as authored. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 029 — The private tin begins to give

After the haul path, Dalia has started cutting from the private tin for the player’s column planters at the same unchanging rate. The text can show a measured pause before the cut without stating the amount. The choice earns neither ownership nor access to the tin. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “I said I would cut from it. I did not say it becomes yours.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not disclose quantity or suggest the player can inspect the tin. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use only for the haul branch; preserve the stated rate without inventing its value. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 030 — The private tin begins to give

Proposed diegetic text: “Planter note: Cut from private tin begins at the unchanging rate; amount omitted.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: After the haul path, Dalia has started cutting from the private tin for the player’s column planters at the same unchanging rate. The text can show a measured pause before the cut without stating the amount. The choice earns neither ownership nor access to the tin. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not disclose quantity or suggest the player can inspect the tin. Do not let the form claim authority that its keeper has not been given.

Later reading: Use only for the haul branch; preserve the stated rate without inventing its value. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 031 — The private tin begins to give

After the haul path, Dalia has started cutting from the private tin for the player’s column planters at the same unchanging rate. The text can show a measured pause before the cut without stating the amount. The choice earns neither ownership nor access to the tin. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “I said I would cut from it. I did not say it becomes yours.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Use only for the haul branch; preserve the stated rate without inventing its value.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not disclose quantity or suggest the player can inspect the tin. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 032 — The private tin begins to give

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. After the haul path, Dalia has started cutting from the private tin for the player’s column planters at the same unchanging rate. The text can show a measured pause before the cut without stating the amount. The choice earns neither ownership nor access to the tin. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Use only for the haul branch; preserve the stated rate without inventing its value. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not disclose quantity or suggest the player can inspect the tin.

Dalia Marun may say: “I said I would cut from it. I did not say it becomes yours.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Planter note: Cut from private tin begins at the unchanging rate; amount omitted. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 033 — The schedule does not flatter the season

Dalia’s schedule admits defeat. A page can show that a season may fail without assigning blame to a person who followed the written work. It is a planning record, not a promise that the next year will be generous. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “A schedule that cannot admit defeat is only a wish with boxes around it.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No planting calendar, crop rotation instruction, or new forecast. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Let the unamended schedule remain itself in `late_kept`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 034 — The schedule does not flatter the season

Proposed diegetic text: “Schedule margin: Known limits retained; no yield promised.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Dalia’s schedule admits defeat. A page can show that a season may fail without assigning blame to a person who followed the written work. It is a planning record, not a promise that the next year will be generous. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No planting calendar, crop rotation instruction, or new forecast. Do not let the form claim authority that its keeper has not been given.

Later reading: Let the unamended schedule remain itself in `late_kept`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 035 — The schedule does not flatter the season

Dalia’s schedule admits defeat. A page can show that a season may fail without assigning blame to a person who followed the written work. It is a planning record, not a promise that the next year will be generous. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “A schedule that cannot admit defeat is only a wish with boxes around it.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Let the unamended schedule remain itself in `late_kept`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No planting calendar, crop rotation instruction, or new forecast. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 036 — The schedule does not flatter the season

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Dalia’s schedule admits defeat. A page can show that a season may fail without assigning blame to a person who followed the written work. It is a planning record, not a promise that the next year will be generous. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Let the unamended schedule remain itself in `late_kept`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No planting calendar, crop rotation instruction, or new forecast.

Dalia Marun may say: “A schedule that cannot admit defeat is only a wish with boxes around it.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Schedule margin: Known limits retained; no yield promised. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 037 — The freehold failed around the annex

In `late_kept`, the freehold failed while the annex did not. The regional planting traces to lots Dalia held through the bad season. The scene acknowledges that scale without making Dalia the sole author of everyone’s survival. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “The annex stayed. That does not mean everyone did.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not claim every settlement is restored or identify unnamed growers. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Only show after `late_kept`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 038 — The freehold failed around the annex

Proposed diegetic text: “Late note: Annex remains; freehold failed; viable planting traces to the held lots.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_kept`, the freehold failed while the annex did not. The regional planting traces to lots Dalia held through the bad season. The scene acknowledges that scale without making Dalia the sole author of everyone’s survival. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not claim every settlement is restored or identify unnamed growers. Do not let the form claim authority that its keeper has not been given.

Later reading: Only show after `late_kept`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 039 — The freehold failed around the annex

In `late_kept`, the freehold failed while the annex did not. The regional planting traces to lots Dalia held through the bad season. The scene acknowledges that scale without making Dalia the sole author of everyone’s survival. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “The annex stayed. That does not mean everyone did.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Only show after `late_kept`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not claim every settlement is restored or identify unnamed growers. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 040 — The freehold failed around the annex

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_kept`, the freehold failed while the annex did not. The regional planting traces to lots Dalia held through the bad season. The scene acknowledges that scale without making Dalia the sole author of everyone’s survival. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Only show after `late_kept`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not claim every settlement is restored or identify unnamed growers.

Dalia Marun may say: “The annex stayed. That does not mean everyone did.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late note: Annex remains; freehold failed; viable planting traces to the held lots. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 041 — The schedule is left unamended

The kept state says the shelter’s fields are sown from tested stock under Dalia’s unamended schedule. A margin remains blank where a visitor wanted to improve the wording. Dalia has not changed it merely to sound more certain. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “You can ask me to explain a line. You cannot make it say something it does not.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not add crop directions or revised rules. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Bind to the kept late state and leave the schedule itself authoritative. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 042 — The schedule is left unamended

Proposed diegetic text: “Schedule copy: Existing schedule retained without amendment.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The kept state says the shelter’s fields are sown from tested stock under Dalia’s unamended schedule. A margin remains blank where a visitor wanted to improve the wording. Dalia has not changed it merely to sound more certain. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not add crop directions or revised rules. Do not let the form claim authority that its keeper has not been given.

Later reading: Bind to the kept late state and leave the schedule itself authoritative. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 043 — The schedule is left unamended

The kept state says the shelter’s fields are sown from tested stock under Dalia’s unamended schedule. A margin remains blank where a visitor wanted to improve the wording. Dalia has not changed it merely to sound more certain. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “You can ask me to explain a line. You cannot make it say something it does not.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Bind to the kept late state and leave the schedule itself authoritative.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not add crop directions or revised rules. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 044 — The schedule is left unamended

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The kept state says the shelter’s fields are sown from tested stock under Dalia’s unamended schedule. A margin remains blank where a visitor wanted to improve the wording. Dalia has not changed it merely to sound more certain. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Bind to the kept late state and leave the schedule itself authoritative. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not add crop directions or revised rules.

Dalia Marun may say: “You can ask me to explain a line. You cannot make it say something it does not.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Schedule copy: Existing schedule retained without amendment. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 045 — Waiting leaves a date in the margin

If `dalia_wait` is chosen, the better season remains the reason given. A proposed paper records that the decision was made without assigning the visitor a villain’s motive. The later roof outcome belongs to the established state, not this moment alone. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “Waiting is what we chose. We do not know yet what the roof will say.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not foreshadow the late outcome as if already decided. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: This scene is before `late_lost`; keep the later callback conditional. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 046 — Waiting leaves a date in the margin

Proposed diegetic text: “Margin: Better season awaited; result not yet recorded on this page.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: If `dalia_wait` is chosen, the better season remains the reason given. A proposed paper records that the decision was made without assigning the visitor a villain’s motive. The later roof outcome belongs to the established state, not this moment alone. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not foreshadow the late outcome as if already decided. Do not let the form claim authority that its keeper has not been given.

Later reading: This scene is before `late_lost`; keep the later callback conditional. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 047 — Waiting leaves a date in the margin

If `dalia_wait` is chosen, the better season remains the reason given. A proposed paper records that the decision was made without assigning the visitor a villain’s motive. The later roof outcome belongs to the established state, not this moment alone. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “Waiting is what we chose. We do not know yet what the roof will say.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “This scene is before `late_lost`; keep the later callback conditional.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not foreshadow the late outcome as if already decided. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 048 — Waiting leaves a date in the margin

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. If `dalia_wait` is chosen, the better season remains the reason given. A proposed paper records that the decision was made without assigning the visitor a villain’s motive. The later roof outcome belongs to the established state, not this moment alone. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: This scene is before `late_lost`; keep the later callback conditional. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not foreshadow the late outcome as if already decided.

Dalia Marun may say: “Waiting is what we chose. We do not know yet what the roof will say.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Margin: Better season awaited; result not yet recorded on this page. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 049 — Wet inventory is still inventoried

In `late_lost`, the seed library is wet inventory. Dalia still shows germination tests as a lesson, but the prose does not promise that a damaged lot recovers or give a salvaging method. Her work continues in the narrower form the source describes. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “A test can teach you what happened. It cannot un-wet the page.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No recovery procedure, viability estimate, or new seed stock. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Show only after `late_lost`. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 050 — Wet inventory is still inventoried

Proposed diegetic text: “Late inventory: Wet lots recorded as wet; test offered as a lesson.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: In `late_lost`, the seed library is wet inventory. Dalia still shows germination tests as a lesson, but the prose does not promise that a damaged lot recovers or give a salvaging method. Her work continues in the narrower form the source describes. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No recovery procedure, viability estimate, or new seed stock. Do not let the form claim authority that its keeper has not been given.

Later reading: Show only after `late_lost`. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 051 — Wet inventory is still inventoried

In `late_lost`, the seed library is wet inventory. Dalia still shows germination tests as a lesson, but the prose does not promise that a damaged lot recovers or give a salvaging method. Her work continues in the narrower form the source describes. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “A test can teach you what happened. It cannot un-wet the page.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Show only after `late_lost`.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No recovery procedure, viability estimate, or new seed stock. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 052 — Wet inventory is still inventoried

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. In `late_lost`, the seed library is wet inventory. Dalia still shows germination tests as a lesson, but the prose does not promise that a damaged lot recovers or give a salvaging method. Her work continues in the narrower form the source describes. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Show only after `late_lost`. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No recovery procedure, viability estimate, or new seed stock.

Dalia Marun may say: “A test can teach you what happened. It cannot un-wet the page.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Late inventory: Wet lots recorded as wet; test offered as a lesson. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 053 — The lesson is not a punishment

Visitors on the failed-annex path hear the lesson during a tour. Dalia does not use it to punish a person who waited with her. The scene can contain regret without rewriting the shared choice as a private accusation. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “You can learn from the roof without deciding one person caused the season.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not assign a sole culprit or add a new guilt effect. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the source’s lesson-and-inventory tone. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 054 — The lesson is not a punishment

Proposed diegetic text: “Tour note: Germination tests shown; no visitor blamed in the record.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Visitors on the failed-annex path hear the lesson during a tour. Dalia does not use it to punish a person who waited with her. The scene can contain regret without rewriting the shared choice as a private accusation. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not assign a sole culprit or add a new guilt effect. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the source’s lesson-and-inventory tone. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 055 — The lesson is not a punishment

Visitors on the failed-annex path hear the lesson during a tour. Dalia does not use it to punish a person who waited with her. The scene can contain regret without rewriting the shared choice as a private accusation. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “You can learn from the roof without deciding one person caused the season.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Keep the source’s lesson-and-inventory tone.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not assign a sole culprit or add a new guilt effect. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 056 — The lesson is not a punishment

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Visitors on the failed-annex path hear the lesson during a tour. Dalia does not use it to punish a person who waited with her. The scene can contain regret without rewriting the shared choice as a private accusation. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Keep the source’s lesson-and-inventory tone. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not assign a sole culprit or add a new guilt effect.

Dalia Marun may say: “You can learn from the roof without deciding one person caused the season.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Tour note: Germination tests shown; no visitor blamed in the record. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 057 — An empty place in the description

The location description says the librarian is gone, while Dalia’s character record and encounter place a Seed Keeper at this annex. A continuity note can acknowledge the mismatch for editors; player-facing prose should not resolve it by equating the two roles or declaring a chronology that is not authored. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “I am here for the lots. I did not tell you who left the library.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. Do not include this as a diegetic confession or resolve the content conflict in new canon. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Use the encounter’s own presence conditions and leave the generic location text unchanged. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 058 — An empty place in the description

Proposed diegetic text: “Editorial margin only: Location says librarian gone; character encounter names Dalia as Seed Keeper.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The location description says the librarian is gone, while Dalia’s character record and encounter place a Seed Keeper at this annex. A continuity note can acknowledge the mismatch for editors; player-facing prose should not resolve it by equating the two roles or declaring a chronology that is not authored. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. Do not include this as a diegetic confession or resolve the content conflict in new canon. Do not let the form claim authority that its keeper has not been given.

Later reading: Use the encounter’s own presence conditions and leave the generic location text unchanged. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 059 — An empty place in the description

The location description says the librarian is gone, while Dalia’s character record and encounter place a Seed Keeper at this annex. A continuity note can acknowledge the mismatch for editors; player-facing prose should not resolve it by equating the two roles or declaring a chronology that is not authored. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “I am here for the lots. I did not tell you who left the library.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Use the encounter’s own presence conditions and leave the generic location text unchanged.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. Do not include this as a diegetic confession or resolve the content conflict in new canon. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 060 — An empty place in the description

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The location description says the librarian is gone, while Dalia’s character record and encounter place a Seed Keeper at this annex. A continuity note can acknowledge the mismatch for editors; player-facing prose should not resolve it by equating the two roles or declaring a chronology that is not authored. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Use the encounter’s own presence conditions and leave the generic location text unchanged. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. Do not include this as a diegetic confession or resolve the content conflict in new canon.

Dalia Marun may say: “I am here for the lots. I did not tell you who left the library.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Editorial margin only: Location says librarian gone; character encounter names Dalia as Seed Keeper. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 061 — The door stays hers to open

Dalia says she will not leave the annex while the roof holds. The scene ends with her deciding whether to show the visitor more tests, not with the visitor extracting access to the private tin. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “I showed you the tests. You can leave with that much.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No forced entry, new permission, or inventory interaction. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: Keep the boundary in initial, evolved, and late states. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 062 — The door stays hers to open

Proposed diegetic text: “Visit note: Germination rates shown before access; private tin remains closed to discussion.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: Dalia says she will not leave the annex while the roof holds. The scene ends with her deciding whether to show the visitor more tests, not with the visitor extracting access to the private tin. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No forced entry, new permission, or inventory interaction. Do not let the form claim authority that its keeper has not been given.

Later reading: Keep the boundary in initial, evolved, and late states. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 063 — The door stays hers to open

Dalia says she will not leave the annex while the roof holds. The scene ends with her deciding whether to show the visitor more tests, not with the visitor extracting access to the private tin. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “I showed you the tests. You can leave with that much.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “Keep the boundary in initial, evolved, and late states.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No forced entry, new permission, or inventory interaction. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 064 — The door stays hers to open

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. Dalia says she will not leave the annex while the roof holds. The scene ends with her deciding whether to show the visitor more tests, not with the visitor extracting access to the private tin. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: Keep the boundary in initial, evolved, and late states. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No forced entry, new permission, or inventory interaction.

Dalia Marun may say: “I showed you the tests. You can leave with that much.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Visit note: Germination rates shown before access; private tin remains closed to discussion. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

### Scene draft 065 — A year remains in the envelope

The final image is one labeled envelope returned to its place among the others. The year it carries is not a harvest already promised; it is a record someone has kept intact long enough to be read. Begin with the work already under way, before the visitor is asked to decide what it means. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. Let an object move only when the person responsible for it chooses to move it. Give the room time to sound like a room people use: paper settling, a pan cooling, a queue taking one step, a chair waiting for its next student.

Dalia Marun: “You do not get a future by naming it loudly.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””

The player can answer, ask for the term again, wait, or leave. No answer should conceal the actual choice already recorded in the quest. No new item, crop, or outcome. The pause after the line may hold more than a speech could: a hand stays beside the page, the pot is not put away, or the guard waits for the next person to cross.

Scene close: End with the state-specific annex truth, not a generic hopeful harvest. Keep the final action with the character who has to continue living beside the record. Do not add an unseen witness, a new appointment, or a promised result to make the moment feel complete.

### Record and return 066 — A year remains in the envelope

Proposed diegetic text: “Shelf return: Envelope restored under its existing variety, year, and harvest labels.” Treat this as a compact in-world object: a ledger margin, page copy, notice, receipt, or line spoken into a keeper’s book. Keep the visible wording shorter than the editorial description around it. Leave author, date, signature, and audience blank whenever the current source does not supply them.

Context for placement: The final image is one labeled envelope returned to its place among the others. The year it carries is not a harvest already promised; it is a record someone has kept intact long enough to be read. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. A record can preserve a choice without settling every fact around it. Let a reader distinguish observation, testimony, inference, and omission. No new item, crop, or outcome. Do not let the form claim authority that its keeper has not been given.

Later reading: End with the state-specific annex truth, not a generic hopeful harvest. A changed copy should name what changed and who changed it only where the existing story supports that attribution. A clean page is not proof that the earlier page was false; an incomplete page is not permission to invent what is missing. The proposed text remains optional and subordinate to the existing narrative owner.

### Conversation fragment 067 — A year remains in the envelope

The final image is one labeled envelope returned to its place among the others. The year it carries is not a harvest already promised; it is a record someone has kept intact long enough to be read. Let Dalia Marun speak to someone who has a practical reason to be present, not a visitor summoned to hear the story’s moral. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward. The other person can disagree about what a document proves while accepting what it physically says. The exchange has no hidden trust score and does not create a new choice.

Dalia Marun: “You do not get a future by naming it loudly.”
Other voice: “A person holding the sack says, “I can help carry it. I cannot promise the weather that comes after.””
Dalia Marun: “End with the state-specific annex truth, not a generic hopeful harvest.”

If the player speaks, answer the question they actually asked. Do not reward curiosity with a private name, a secret beneficiary, a missing amount, or a fact deliberately withheld by the character. No new item, crop, or outcome. Let the two people return to the task in different moods if that is more truthful than agreement.

### Consequence vignette 068 — A year remains in the envelope

On a later visit permitted by the current NPC arc state, the player may notice what the earlier choice left in view. The final image is one labeled envelope returned to its place among the others. The year it carries is not a harvest already promised; it is a record someone has kept intact long enough to be read. Do not display this passage as a universal epilogue: the corresponding existing choice and state must already be true. Dalia is exact and patient until patience is mistaken for permission to hurry her. She compares years, labels, and test results; she rarely generalizes about hope. Her signature line is not a prophecy. It is a warning that the person asking her to hurry may not be the one who has to live with the seed afterward.

Observable return: End with the state-specific annex truth, not a generic hopeful harvest. The follow-up can change what is displayed, who is willing to read it aloud, or which existing branch supplies the wording. It cannot silently alter money, food, passage, access, standing, the quest result, or another character state. No new item, crop, or outcome.

Dalia Marun may say: “You do not get a future by naming it loudly.” The other person may answer, correct one detail, or let the work continue without comment. Do not require thanks, absolution, or a confession.

Record left in view: Shelf return: Envelope restored under its existing variety, year, and harvest labels. Keep every returning detail inside the branch condition named in this plan. If that condition is unknown in the implementation, omit the callback rather than guess.

## 12. Short line bank

- Everything in here is a year that has not happened yet. Do not hurry it into a promise.
- I will show you what was tested. I will not tell you a seed is a harvest.
- The roof is not asking for a speech. It is asking what you brought.
- No. The tin is not for discussion. You have the answer already.
- I can tell you what the test says. I cannot sell you a harvest in advance.
- Set it there. We will use it when the next step is ready.
- A better sack keeps a lot dry. It does not change what the lot is.
- I said I would cut from it. I did not say it becomes yours.
- A schedule that cannot admit defeat is only a wish with boxes around it.
- The annex stayed. That does not mean everyone did.
- You can ask me to explain a line. You cannot make it say something it does not.
- Waiting is what we chose. We do not know yet what the roof will say.
- A test can teach you what happened. It cannot un-wet the page.
- You can learn from the roof without deciding one person caused the season.
- I am here for the lots. I did not tell you who left the library.
- I showed you the tests. You can leave with that much.
- You do not get a future by naming it loudly.

## 13. Continuity and editorial review

Verify `npc_dalia_marun`, `enc_arc_dalia_01_annex`, `quest_arc_dalia_01_annex`, and the states tied to `dalia_haul_roof` and `dalia_wait`. Keep the private tin private, the tested-lot rule unchanged, and the source tension between `locations.json` and the character encounter unresolved in the prose. The excerpt must not assert that Dalia is the librarian the generic location description says is gone. Confirm current content before placement. Keep the first-visit encounter recognizable, distinguish every existing choice, and omit any passage whose source condition is not true. Additional people, objects, dates, handwriting, and reactions are proposed only where explicitly marked as such.

## 14. Acceptance boundary

This plan is ready for editorial review when its selected passages can be staged through the existing character and encounter content, each callback matches the authored choice, and the prose leaves the source’s unknowns intact. It does not authorize production implementation. Counts below are Unicode character counts of the saved Markdown file.
