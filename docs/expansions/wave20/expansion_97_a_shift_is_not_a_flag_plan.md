# Expansion 97 — A Shift Is Not a Flag

**Requested series:** Plan 1 of 5  
**Requested plan length:** At least 120,000 words  
**Delivery status:** Complete as a design document; production implementation requires current-source revalidation.  
**Measured length:** 120,825 words (100.69% of the minimum, including headings and closeout; recalculate if revised).
**Plan class:** Systemic quest and faction-support expansion proposal  
**Evidence snapshot:** `5be1a30a63cd86cf23e4034473b739ac514f0f2a` plus the current uncommitted worktree  
**Implementation status:** Plan only. No production source, gameplay data, integration ledger, or ownership record was changed.

The requested word count is a minimum for this series: at least 120,000 words per plan, delivered one plan at a time. Expansion beyond the minimum is acceptable when it adds useful, distinct design content. The repository already contains Wave 19 plans 92–96; those are existing user work and were left untouched. They are prose-first settlement expansions of roughly 20,000–22,000 words apiece and do not satisfy this plan's separate action-branching brief. This new plan starts at Expansion 97. Its word count will be measured from the saved file as installments are added. The first plan remains in progress until it reaches the minimum; a measured count above 120,000 is also acceptable.

## Delivery map for the 120,000-word minimum plan

The initial coverage map uses twelve distinct volumes of approximately 10,000 words each. It is a floor-oriented map, not a cap: additional volumes may be added when they bring distinct quest, branch, playstyle, faction-support, or implementation detail. Each volume must add implementable content or evidence; repeated paraphrase does not count as expansion.

1. **Volume I — Evidence and design charter:** current implementation, verified constraints, thesis, action model, first branch graph, and integration boundary. Begun in this installment.
2. **Volume II — People who keep the shift:** character histories, practical skills, relationships, voices, conflict, and private pressures for the shelter cast.
3. **Volume III — Military and Rebel routes:** full quest sequences, command and collective pressures, operational choices, supporting-current roles, delayed callbacks, failure states, and endings.
4. **Volume IV — Independent and no-commitment routes:** negotiated service, local capacity, limits of neutrality, and an honest path for players who refuse a major commitment.
5. **Volume V — Action history contract:** verified signal sources, event provenance, state ownership, save/restore requirements, branch predicates, and the evidence shown to players.
6. **Volume VI — Supporting-current questlines:** bounded roles for the Archivists, Long Walk, and Scavenger Guild, with optional work from other currents only after their live activation rules are verified.
7. **Volume VII — Quest and encounter atlas:** act-by-act gates, alternate approaches, scene transitions, encounter decks, journals, and content identifiers.
8. **Volume VIII — Playstyle routes and cross-system consequences:** maintainer, scheduler, courier, reciprocal trader, reserve keeper, improviser, delegation and boundary patterns; duty roster, needs, inventory, repairs, radio, expeditions, faction standing, survivor relations, and campaign calendar interactions.
9. **Volume IX — Endings and epilogues:** full ending matrices for each major route, composite action outcomes, supporting-current reactions, and delayed consequences.
10. **Volume X — Failure, refusal, and recovery:** missed meetings, conflicting records, exhausted work crews, broken agreements, abandonment, partial success, and non-catastrophic restart options.
11. **Volume XI — Prose, diegetic content, and continuity:** dialogue banks, notices, work sheets, radio fragments, environmental text, journals, state-aware variants, and content collision audit against existing plans and canon.
12. **Volume XII — Integration brief and acceptance map:** dependency order, named ownership decisions still required, non-goals, acceptance evidence, and final word-count closure.

## 1. Expansion Thesis

A shift is work that can be assigned, traded, shared, refused, or quietly left undone. A flag is a public claim about who owns the work. This expansion asks what happens when three major political routes all need the same ordinary service: a functioning routine that people can keep after the player leaves the room.

The player does not earn a hidden label such as “good organizer” or “bad commander.” They make concrete choices across several days: whether to repair an existing device or replace it with a scarce one; whether to post a schedule, keep a private copy, or make two copies with different audiences; whether to lend workers, trade materials, call in a specialist, or reserve capacity for the shelter; whether to keep an agreed time when the weather turns; whether to report the failure accurately or make the next shift look more reliable than it was. Those actions have costs in time, labor, materials, exposure, trust, and future flexibility.

The major faction remains the political spine. Military, Rebel, and Independent branch commitments continue to mean what their existing systems mean. The new content asks what the player can make that commitment do in practice. Supporting currents provide capabilities at a local scale: a survey, a route report, a copy of a record, a salvage apprenticeship, or a witnessed agreement. They do not take over the story, command the player, replace a major faction, or become a new territorial power.

The expansion is about memory expressed as operations. If the player keeps the same shift for a week, someone notices. If the player changes the rota every time a crisis appears, someone else learns to keep a private list. If the player sends a report that says “unknown,” a future helper may trust the report more than a confident guess. A later ending names the habit through its consequence, not through a moral score.

### Player promise

By the close of the arc, the player should be able to say:

- “I know why the service lasted or failed.”
- “The people who helped were useful in specific ways and were allowed to remain themselves.”
- “My choices changed the available work and its risks, not only a line of ending text.”
- “I could repeat the arc with a different operating practice while keeping the same major-faction commitment.”
- “The game remembered what I did, and it did not pretend to know why I did it.”

### Emotional register

A notice hangs lower after its string slips. A roster has the same three names written beside every difficult night. A second copy is made on the back of a ration sheet because the store has run out of clean forms. No character gives a speech about hope. The evidence is that somebody came back to the board the next morning and corrected the missed hour.

## 2. Canon Fit

This plan extends current ASHFALL structures rather than treating its proposal as present canon.

### Verified source fit

- `FactionBranchCoordinator` owns the three major branch families: Military, Rebel, and Independent. It enforces mutual exclusivity after commitment and gates admission by `MoralChoiceSystem.CurrentBand`; Independent can also require PRPF standing or hostility conditions. This is an existing route contract, not a bug to erase.
- `NarrativeQuestlineSystem` is an active, saved survivor-arc owner. Its documented shape is a linear objective-item ladder followed by one binary crisis fork and a resolution epilogue. Its current save state already preserves the selected branch and timing.
- `PersonalQuestSystem` is an active, saved player-facing route. Its current host reward callbacks are unbound in the working tree, and its panel passes Day 1 when resolving a choice. Expansion content must not depend on those effects until their host boundary is addressed.
- `CampaignOutcomeEvaluator` is the live Main endgame projection inspected for this audit. It considers campaign facts beyond moral standing, including treaty state, debts, evidence, demographics, deaths, and duration.
- `currents.json` contains 17 supporting-current definitions. Several are marked active and several are explicitly inactive. This proposal uses only current definitions whose present status and caller path can be verified before integration.

### Canonical boundary

Expansion 97 does not add a fourth major faction or a new Current. It does not change the ownership of inventory, morale, work shifts, radio, or expedition state. It does not make a Current a general-purpose service menu. An Archivist can help preserve a source record; that does not make the Archivists judges. The Long Walk can provide a sector report; that does not give it control of the route. The Scavenger Guild can share salvage knowledge; that does not transfer ownership of every recoverable object.

The story keeps ordinary uncertainty. A report may be incomplete. Two copies can disagree because one was corrected later. A repair may extend the life of a machine without making it safe forever. Characters may decline assistance. No branch claims that a perfectly virtuous leader can avoid scarcity.

### Relationship to the major factions

Each major faction sees the same work from a different operational position:

- **Military:** values predictable coverage, legible responsibility, and a schedule that can be acted on. Its danger is mistaking a posted plan for proof that the shift was staffed.
- **Rebel:** values local control, mutual aid, and the ability to change a plan without waiting for distant permission. Its danger is relying on informal knowledge that does not survive a departure or disagreement.
- **Independent:** values negotiated terms, resources that can be reconciled, and limits on any one office’s authority. Its danger is spending so long documenting a shared decision that the decision arrives after the useful hour.

These are strengths with costs, not good/evil categories. Each route can produce a stable service, a local failure, an over-centralized arrangement, an exposed network, or a useful compromise depending on the player’s actions.

## 3. Existing Systems Reused

| Existing authority | Proposed use | Evidence and limitation |
|---|---|---|
| `FactionBranchCoordinator` and branch save section | Keep Military/Rebel/Independent as the major campaign choice. Overlay authored route-specific scenes and outcomes after commitment. | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`; state is captured through the `weight_of_choices` section in `src/Main.FactionBranch.cs`. The current moral-band gate remains unchanged in this plan. |
| `NarrativeQuestlineSystem` | Candidate home for the long-form personal/community arc if its state model is approved for action-conditioned branches. | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` already persists arc state, selected branch, and day fields. Its current binary branch cannot directly express this plan’s full graph. |
| `DutyRosterSystem` | Let actual assigned work and repeated shift patterns create readable, earned options. | Reuse the owner and its current save route. Do not derive a work history from a UI label or create a second roster. |
| Inventory and trade owners | Make service choices consume or preserve actual goods. Item costs and receipts stay with the current inventory/economy owners. | No quest-local stockpile or shadow supply count. Exact event seams require a premise check during integration. |
| Power, water, greenhouse, and workshop owners | Supply repair-versus-replacement and maintenance scenes that respond to actual equipment or production state. | The expansion must read current owner state; it must not author a second power, water, crop, or repair simulation. |
| Radio / narrative communications | Provide an optional route for publishing a schedule, reporting uncertainty, or requesting a local answer. | No duplicate radio history and no assumption that broadcasting guarantees receipt. |
| `currents.json` | Reuse established supporting currents as task-specific contributors. | Current status matters. `faction_archivists`, `faction_long_walk`, and `faction_scavenger_guild` are marked active in the inspected data; other candidates must be checked before use. |
| `CampaignOutcomeEvaluator` | Let the local arc leave a factual record that a later campaign outcome can read only if the canonical save/event path is approved. | The active endgame evaluator already consumes multiple campaign facts. This plan does not add an ending authority or duplicate its snapshot. |

### Required audit before implementation

The current `CampaignConsequenceLedger` exposes save-state capture and restore in Core, but this audit did not find a Main save/restore call for that DTO. Therefore this plan does **not** assume that setting a campaign flag is sufficient persistence. Before implementation, a named owner must establish whether the action history belongs in an existing quest/faction save section or an already-owned campaign consequence section. If a new mutable owner is proposed, stop for the architecture decision required by the project rules.

## 4. New Design Space

### What is new here

The distinct design space is a campaign arc whose branches are selected by the player’s observed operating choices across several systems, with major factions framing those choices and smaller currents enabling particular approaches. It is not a new faction trust meter, a new moral axis, a job scheduler, a logistics simulation, or a generic “faction help” menu.

The content aims to make a player’s existing habits legible in narrative:

- **Maintenance before replacement:** does the player preserve a working-but-limited tool or spend a scarce replacement to buy certainty?
- **Regularity versus adaptation:** does the player keep a known rota through a disruption or change the assignment as conditions change?
- **Disclosure versus controlled custody:** who sees the schedule, what is marked uncertain, and who can correct the record?
- **Reciprocity versus reserve:** does the player lend a worker or material with a stated return, trade at a negotiated rate, or keep capacity local?
- **Delegation versus direct control:** does the player ask a supporting Current for a bounded service, assign the work to shelter staff, or do the work personally?

No single answer dominates. A fixed schedule can keep people alive and make the shelter predictable; it can also fail badly when the assigned person is absent. A flexible rota can respond to illness; it can also make a promise impossible to keep. A public record can support accountability; it can also tell an adversary when the shelter is least staffed. A reserved part can protect the shelter; it can also leave a helper without a repair they could have completed.

### What is not new

- Not a fourth major faction.
- Not a social alignment score, virtue score, or hidden playstyle class.
- Not an all-seeing AI that infers intent from unrelated choices.
- Not a new resource wallet or staffing ledger.
- Not a replacement ending evaluator.
- Not a guarantee that every minor Current is active in every save.
- Not a quest that collapses into “pick the best person” or “tell the truth.”

### Action evidence rule

A branch may cite only an action the game can prove. If the game cannot tell whether the player repaired a specific device, then dialogue may ask what happened, but it may not claim the player repaired it. If the player has not shared a count, the scene may offer a count-sharing choice; it may not call the player secretive. A player can adopt a different practice halfway through the arc. The content should remember the change and its date rather than flattening the whole run to a permanent label.

The design should prefer named events and explicit choices over inferred scores. The proposed record is a small set of factual witnesses such as “posted a rota,” “reassigned after an absence,” “sent an uncertainty-marked report,” or “kept the reserved part.” These are working descriptions, not approved data IDs. Their source event, day, and subject must be preserved by the eventual owner.

## 5. Core Mechanics

### 5.1 The support window

A support window is a short authored opportunity in the main questline. It asks the player to arrange one piece of work before the next scene, not to run a new simulation. Example windows are “who checks the ventilation before night,” “who carries the revised schedule,” or “which copy of the count is sent with the repair request.”

The window has four visible elements:

1. **Need:** the service that is currently missing.
2. **Constraint:** the actual person, resource, time, or system pressure that limits the response.
3. **Available contributors:** shelter staff, a committed major faction, and any supporting Current whose access condition is truly met.
4. **Receipt:** a later scene that reports what the chosen arrangement did and did not accomplish.

A window does not mint a new trust number. It records the action through its current state owner. A successful choice does not imply universal standing with the Current who helped.

### 5.2 Branch dimensions

The quest should track separate decisions, not compress them into a single total:

| Dimension | Example player action | Immediate gain | Deferred cost or risk |
|---|---|---|---|
| Work method | Repair, replace, or postpone | Different use of labor and stock | Wear, future scarcity, or an unanswered need |
| Cadence | Fixed rota, rotating cover, or on-call response | Predictability or adaptability | Fatigue, missed handoff, or a smaller reserve |
| Record custody | Public board, private copy, or paired copies | Accountability or controlled access | Exposure, disagreement, or delayed correction |
| Exchange | Give, trade, loan with a date, or reserve | Relationship and available material change | A real obligation, depleted stock, or lost access |
| Delegation | Assign to shelter, invite a Current, or act directly | Skills and relationships matter | Cost, dependency, reduced autonomy, or player time |
| Response to failure | Repeat, revise, report uncertainty, or suspend | A second opportunity or a limit | Trust loss, lower output, or no next service window |

The major branch changes who can authorize or challenge the work. These dimensions change what the work looks like. That combination prevents the three major routes from becoming the entire branching system.

### 5.3 Eligibility and visible explanations

An option is hidden only when the player could not reasonably know it exists or cannot act on it. Otherwise, unavailable options remain visible with a concrete reason: “No second copy has been made,” “The rota has not held through an absence,” “The Guild has not been asked to survey the shelf,” or “The reserve is already committed.” The UI should not expose implementation terms such as “action signature.”

The arc may begin with no useful action history. In that case, the player receives an initial choice and the game starts recording from that moment. There is no retroactive judgment.

### 5.4 Failure is specific

A support window can fail because the part does not fit, the worker becomes unavailable, the report arrives after the shift, a Current declines the request, or the player chooses not to spend scarce supplies. Failure changes later options and dialogue. It does not automatically kill a survivor, destroy the shelter, or condemn a whole faction.

## 6. Cross-System Interactions

At least three systems should matter in every major quest phase. The following map is a proposal; every source read must be checked against live APIs during integration.

| Quest phase | Systems that can participate | How their contribution changes play |
|---|---|---|
| Set the first rota | Duty roster, survivor needs, campaign calendar | A willing worker can still be too fatigued or assigned elsewhere. The player chooses whether to reassign, shorten the shift, or delay service. |
| Fix the work site | Power/water/workshop owner, inventory, Scavenger Guild support | Repair competes with replacement stock; Guild advice identifies salvage possibilities but never creates free material. |
| Decide who sees the plan | Radio, quest state, faction branch, Archivists | The report can be sent, held, or duplicated. The Archivists can preserve a source without deciding what the major faction should do. |
| Keep a commitment | Calendar, roster, inventory/economy, survivor relations | A loan or schedule has a due point. If the player spends the same capacity elsewhere, the missed commitment becomes a later scene. |
| Close the arc | Faction branch, support-current participation, campaign outcome | Major faction gives the arrangement a political meaning; the actual outcome still follows the player’s recorded actions and their consequences. |

The combination matters more than a series of stat bonuses. A public rota with enough rested staff and a working light can be safe and legible. The same public rota with one exhausted worker may tell someone exactly when the service is unattended. A private reserve can protect continuity until a second failure makes the refusal of help visible. A Current’s contribution can improve the chance of work without transferring responsibility for what the player asked them to do.

## 7. Main Narrative Spine

### Working questline identity

**Proposed title:** “A Shift Is Not a Flag”  
**Proposed quest ID:** `quest_a_shift_is_not_a_flag` (unverified and not registered)  
**Primary location:** the player’s current shelter operations spaces; no new settlement or map route is assumed  
**Campaign placement:** mid-campaign, after a major faction route becomes available or after a separate neutral opening condition is met  
**Duration:** multiple authored scenes over several campaign days; no independent clock or deadline subsystem  
**Core cast:** one shelter scheduler, one maintenance worker, one runner/communicator, and optional contacts from existing supporting currents. Names remain provisional until the full collision pass.

### Act I — The empty square

A shift board has one blank interval. The player first assumes the missing name is an error. The schedule clerk says it is the hour nobody has agreed to own. The machine does not fail in a spectacular fashion; it waits long enough to be noticed and then stops doing the one thing it was assigned.

The player may fill the space with a named worker, set a rotating assignment, keep the interval unstaffed while a repair is assessed, or ask whether an outside Current can help. Each option carries a visible cost. The player can also ask what the blank interval means before committing. That question reveals a modest earlier disagreement: one person treated it as rest, another as standby, and a third as work that was already being done without credit.

### Act II — The first receipt

The selected arrangement produces a practical result. If the player chose a named worker, the next scene reports on that worker’s availability and fatigue. If they chose a rotation, someone can cover an absence but must read a schedule they did not write. If they delayed for a repair, the machine remains unavailable until an existing owner says it is ready. If they asked a Current for help, the Current responds within its established offer and access conditions.

The scene gives the player a receipt: what happened, who did it, and what remains uncertain. The first branch changes quest progression, not merely the dialogue line.

### Act III — The copy

A major faction representative asks for the schedule. The player chooses whether to send the current copy, add a correction, make a second copy for the shelter, or refuse distribution. The representative does not become a cartoon villain. Each major route has a credible use for the information and a credible cost if the schedule is wrong.

The Archivists can preserve a copy only where their current access path and request conditions are satisfied. They do not authenticate the machine state. A dated sheet proves that someone wrote a plan on a date; it does not prove that the work happened as written.

### Act IV — The absence

One planned contributor cannot cover the next shift. The cause need not be death or injury. It can be a regular assignment, treatment, a family obligation, a necessary rest interval, or a previously established relationship conflict. The player can honor the absence, override it, find a replacement, or postpone the work.

This is the arc’s first major action-based branch. The game compares what the player said would happen with what they do when the plan becomes inconvenient. A player who changes the rota is not branded unreliable. The record simply says who covered the gap and whether the missing person was told.

### Act V — The spare part

The work now needs a material the shelter can use elsewhere. The player can repair with a lower-grade part, use the best stored part, ask the Scavenger Guild about salvage, barter with a current supplier where available, or stop the work and return the stock to the shelf.

The Guild’s support is narrow: it can identify an accessible salvage lead or teach a worker how to evaluate a part. It does not guarantee a successful trip, erase expedition risks, or create an item. The player may also decline the Guild offer without losing the main questline.

### Act VI — The correction

The first schedule copy and the actual work no longer match. A new note must be made. The player can correct the original, keep both versions with a clear date, send the correction outward, or leave the contradiction unresolved. The important choice is whether a later reader can tell which statement was planned, which was observed, and who made the correction.

### Act VII — The table

The major faction and available supporting contributors meet the player at the board. They do not vote the player out or decide the ending. They state what they can continue to offer and what conditions they will not accept. The player chooses the service arrangement that remains after the meeting: a fixed assignment, rotating work, a reciprocal service agreement, a shelter-held reserve, or a suspension with a clear return condition.

### Act VIII — The later hour

After an authored delay already supported by campaign time, the game checks what the arrangement can honestly claim. The result can be stable, useful but narrow, exposed, under-resourced, or discontinued. A later ending callback uses observed events from the arc and never reinterprets them as a moral verdict.

## 8. Major Questlines

This expansion has one campaign spine with three major-faction overlays and one no-commitment route. Each overlay has its own initiating voice, pressure, and closing interpretation. They share a bounded work problem, not identical quests with names swapped.

### 8.1 Military overlay — “Coverage Has a Name”

The Military route asks for a service schedule it can plan around. Its representative has been asked to promise a window before knowing which workers will cover it. The player can provide a fixed schedule, a schedule with explicit uncertainty, or a smaller guaranteed service.

**Distinct decisions:**

- Give the Military a complete copy, a redacted copy that still identifies the service window, or only a confirmation after the work is done.
- Assign a steady worker, rotate among trained staff, or borrow an outside specialist for a single task.
- Preserve the scheduled window through a disruption, or stop work until the shelter’s own needs are met.
- Record the gap as a missed commitment or rewrite the schedule without showing the earlier version.

**Supporting roles:**

- The Scavenger Guild may help inspect a repairable part or teach a shelter worker to assess it.
- The Long Walk may provide a situation report if the route and contact conditions exist.
- The Archivists may keep a dated copy if their corroboration and access requirements can be met.

No Current grants command authority to the Military. The representative may use the schedule; the player remains responsible for what they promised.

**Possible Military resolutions:**

- A dependable, narrow service that the Military plans around and the shelter can actually staff.
- A larger promise with a clear risk note, which preserves flexibility but makes the Military’s plans less certain.
- A closed service window after a missed commitment, with the reason recorded and a condition for reopening.

### 8.2 Rebel overlay — “Nobody Carries the Whole Board”

The Rebel route arrives with a workable rota already in circulation, but its copies disagree. One group says the plan can be changed locally. Another fears that every correction becomes a reason to withdraw support. The player is asked to choose how a change is shared, not whether local control is good.

**Distinct decisions:**

- Let each team keep a copy, send corrections through one runner, or publish one shared board.
- Use a fixed meeting hour, a rotating caller, or an on-call message.
- Let a worker swap a shift directly, require a second witness, or suspend the work when the schedule changes.
- Share a scarce part under an agreed return date, exchange it immediately, or keep it for the shelter.

**Supporting roles:**

- The Long Walk can provide a broad situation report, not a secure communications service.
- The Scavenger Guild can provide local site knowledge and salvage assessment, not armed escort by default.
- The Archivists can retain a copy of the sequence of corrections, not certify which group was right.

**Possible Rebel resolutions:**

- A distributed rota that survives local changes but requires more people to keep informed.
- A rotating caller arrangement that adapts quickly but loses detail when a message is missed.
- A written mutual-aid agreement whose participants can withdraw at a stated boundary.

### 8.3 Independent overlay — “Terms With an End Date”

The Independent route asks the player to negotiate a service that does not become a permanent claim on the shelter. Two parties agree about the work and disagree about what a loan means after the first missed return date. The player is not asked to make everyone happy. The player is asked to write terms that can be followed and declined.

**Distinct decisions:**

- Use a one-time exchange, a dated loan, a rotating contribution, or a pooled reserve.
- Let each contributor keep a separate receipt or produce one combined count.
- Offer a narrow renewal after a successful cycle or require a new negotiation each time.
- Respond to a failed return with an extension, a replacement, a public note, or the end of the arrangement.

**Supporting roles:**

- The Archivists can preserve distinct accounts side by side.
- The Long Walk can report whether a distant contact is reachable, when the live system supports that report.
- The Scavenger Guild can offer a repair-learning or salvage-assessment contribution rather than a standing supply stream.

**Possible Independent resolutions:**

- A renewable agreement with a visible end date and no automatic debt escalation.
- A successful one-time service that does not become an ongoing entitlement.
- A declined renewal that leaves the participants able to work together on a later, different task.

### 8.4 No-commitment route — “The Board Stays Local”

The player may refuse all three major faction commitments. The arc remains playable if its neutral opening conditions are met. This is not a secret fourth faction. It is a smaller service agreement between the shelter and whichever supporting contributors have independently become available.

The route has less reach. It cannot obtain a major faction’s manpower or authority. It can still produce a sturdy local maintenance practice, a documented refusal, a limited trade, or an honest pause. It receives its own endings and does not automatically count as a failure to choose.

## 9. Side Quests

All side quests are proposed content. Their IDs, triggers, locations, and effect routes require collision and consumer checks before use.

### 9.1 “The Name Beside the Hour”

A shelter worker has been covering an unnamed interval. The player can name the worker, rotate the responsibility, pay the time back through another shift, or leave the line anonymous. Later dialogue responds to whether a person can find their own work in the record. No option changes a global morality score.

### 9.2 “The Part That Almost Fits”

A salvaged component has the right dimensions but uncertain wear. The player can install it through the existing workshop/repair authority, keep it as a sample, ask for a second assessment, or refuse it. A later scene remembers the actual result. It must not grant technical certainty beyond the game’s existing item/repair rules.

### 9.3 “A Copy for the Drawer”

An Archivist asks whether the shelter wants a local copy of the work record. The player can provide a complete copy, a named excerpt, a corrected copy with the original attached, or nothing. The Archivist explains exactly what they will and will not infer from it.

### 9.4 “The Message That Arrived After”

A Long Walk report reaches the shelter after the planned shift. The player can use it for the next schedule, keep it as context, or explain that the information arrived too late to justify a past decision. The current does not take responsibility for a choice it could not influence.

### 9.5 “Swap the Hour”

Two workers want to exchange a shift. The player can approve a direct swap, require a shared acknowledgment, deny it because of the actual staffing state, or change the task so the swap is unnecessary. Later scenes respond to whether the participants understood the change.

### 9.6 “The Empty Shelf”

A reserve part was kept through two requests and now appears unused. The player can release it, continue reserving it, offer it under a return date, or inspect whether the original contingency remains real. The story avoids treating precaution as greed or sharing as virtue.

## 10. Survivors/NPCs

At initial authoring, the plan called for a complete cast and dialogue bank. Later installments developed character arcs and dialogue; names, survivor IDs, and provisional network identities still require a repo-wide collision pass before production use.

| Working role | Dramatic function | What the character knows | What the character cannot decide for the player |
|---|---|---|---|
| Shift clerk | Maintains the visible rota and remembers who corrected it | Which assignments were posted and which were changed at the board | Whether a late change was justified |
| Maintenance worker | Knows what has been repaired, borrowed, or set aside | The condition of the part they inspected | Whether the player should spend the reserve |
| Runner | Carries messages between the shelter and the available support contact | When a message was sent and when it arrived | Whether the recipient should act on it |
| Military liaison | Represents a request for predictable coverage | What the Military plans to do with a confirmed window | Whether the shelter owes the Military a permanent service |
| Rebel caller | Represents a locally organized request | Who has received the last correction | Whether all distributed copies are current |
| Independent broker | Negotiates a limited exchange | Which terms each party has accepted | Whether the arrangement should renew automatically |
| Current contact | Supplies one bounded Current service | Their own group’s actual offer and limit | The major faction’s policy or the shelter’s final decision |

Characters should disagree about procedures because those procedures affect their work, not because one was secretly evil. A person can be competent and still want a schedule that the player cannot afford to keep. A person can be generous and still decline an open-ended obligation.

### Sample voice fragments

> “I wrote the time we agreed on. I didn’t write that we agreed.”

> “You can keep the part. I can still show you how to check the one you have.”

> “If I send the correction, they’ll get the correction. They will also know there was something to correct.”

> “Don’t put me down as on call. Ask me when you need me.”

These lines are tonal examples, not final character canon. The later prose volume will establish voice, relationships, individual history, and state-aware variants before implementation.

## 11. Locations

The arc should use existing shelter and world sites wherever the live location authority supports them. No new map connection is assumed. The setting should be readable through the player’s already available work spaces:

- The existing shelter duty or operations board for the rota.
- The relevant power, water, greenhouse, or workshop space when the actual subsystem state justifies a visit.
- The radio station or existing communications space for optional reports.
- An existing meeting or trade location only when its catalog route is reachable.

The plan does not yet assign exact `loc_` IDs. That is deliberate: the current user workspace contains ongoing expansion and data work, and candidate locations must be checked for route reachability, hazards, and conflicting content before data is authored.

## 12. Encounters

Encounters in this arc are operational conversations, not combat gates. Any tactical event remains under the existing combat/encounter authority.

1. **The blank interval:** a missing assignment with three plausible interpretations.
2. **The changed notice:** a schedule is current in one location and stale in another.
3. **The absent worker:** the player must respond to a real absence without assuming betrayal.
4. **The wrong part:** a component can be made to fit but its remaining life is uncertain.
5. **The arriving report:** useful information arrives too late to influence the first action.
6. **The public copy:** a major faction asks for a record with a real operational use and a real exposure cost.
7. **The exchange date:** a loan return cannot happen on time; the parties choose what the date means.
8. **The quiet board:** after an arrangement ends, the player can leave the board intact, annotate it, or clear it.

Every encounter must have a refusal or delay response. The player is not forced to invent a promise simply because a choice list exists.

## 13. Faction Reactions

### Major factions

| Major route | What it can provide | What it asks | How action history changes its reaction |
|---|---|---|---|
| Military | Predictable access, organized personnel, a chain for confirming a work window | A schedule it can plan around and a clear account of missed service | Repeated revisions produce requests for confirmation; a well-labeled uncertainty is treated differently from a false guarantee |
| Rebel | Local contacts, distributed participation, rapid local corrections | A way to reach the people actually doing the work | Changes that stay local can be effective; missing copies make the next handoff harder |
| Independent | Negotiated terms, limited exchange, a route to renew or decline | Specific quantity, duration, return condition, and an end date | Kept or renegotiated terms support renewal; a clearly stated refusal preserves the chance of later work |

The faction reactions are written as institutional interpretations of observable acts. They must not translate these acts into “good person,” “bad person,” or alignment labels.

### Supporting currents

| Supporting Current | Data-backed contribution | Boundary in this story |
|---|---|---|
| The Archivists of the Before | A record-keeping culture with a corroboration rule; current data lists archival and morale-related offers | They preserve and contextualize a copy; they do not authenticate machine readings or decide who is right |
| The Long Walk | Sector-wide situation reports and access to distant goods are listed as offers in current data | It can report what it knows when its route/contact condition is met; it does not promise delivery time or serve as a courier command |
| The Scavenger Guild | Rich salvage routes and apprenticeship appear among current offers | It can share knowledge or teach assessment; the player still owns expedition choice, risk, and inventory |

All supporting-current appearances are optional. Current `is_active` values, access conditions, and live callers are rechecked before implementation. Inactive currents do not appear just because a plan names them.

## 14. Items/Resources

The arc reuses existing materials and records. It does not create a second inventory.

- Repair components already represented in the live catalog.
- Lamp, battery, or maintenance materials only when the selected subsystem actually consumes them.
- Existing trade goods for a negotiated exchange.
- Paper or archive items only if the item catalog and inventory owner support them as real items; otherwise the “copy” is a narrative record held by the quest state and must not pretend to occupy inventory.
- Existing radio capacity and expedition supplies where the route uses them.

Every cost needs one owner and one observable result. If an authored option says “give two batteries,” the inventory owner must spend two batteries. If the current data or consumer cannot do that, the plan must use non-inventory wording or wait for the relevant integration decision.

## 15. Radio/Environmental Storytelling

Radio is an optional way of changing who knows about an arrangement. The player may send a schedule, send a correction, broadcast a general availability window, or keep the communication local. A transmission being sent does not mean it was received; the existing radio system owns signal quality and delivery evidence.

Environmental details should change with actual quest state:

- One copy curled at the edge where condensation reached the board.
- A date crossed out once, with the correction written beside it rather than over it.
- A hook left empty after the borrowed lamp goes back to its owner.
- A tool returned to the wrong shelf, not because it vanished but because the shift changed.
- Two copies pinned separately because one group needs the schedule and another needs the correction history.

The environment does not display a universal “faction reputation” explanation. It shows the physical record of the decision.

## 16. Persistent Consequences

The central persistence requirement is to save facts, not a derived virtue label.

Candidate facts include:

- Which version of the work arrangement the player selected.
- Which specific support requests were accepted or declined.
- Who actually completed a work window, where the owning system exposes that fact.
- Whether a schedule was corrected, duplicated, or withheld.
- Whether an exchange was completed, extended, or closed.
- The day and source event for each consequential action.
- The terminal resolution and any promised return condition.

Before any field or flag is authored, the integrator must map it to one current save owner. `NarrativeQuestlineSystem` already owns saved arc state but currently supports one binary fork. Faction branch state already owns major commitment. The consequence ledger has a Core capture/restore DTO, but its Main runtime enrollment was not proven in this audit. No new parallel campaign ledger is approved by this document.

### Delayed callbacks

Callbacks should be scheduled through the existing campaign calendar or quest lifecycle. Examples:

- After a few days, a worker reports whether the rotation was legible to someone returning from medical care.
- After a later meeting, a faction representative brings the old copy and asks about one correction.
- At the next trade contact, a supporting Current remembers whether the player returned a borrowed item on time.
- At an ending evaluation, the selected arrangement appears as a concrete regional fact only if the event was saved and restored correctly.

No callback should be inferred from morality bands alone.

## 17. Failure/Alternate Outcomes

Failure is an authored branch with a cause and a possible next step.

- **A worker cannot cover the shift:** move the assignment, reduce service, wait, or proceed short-handed only if current systems permit it.
- **The repair consumes the reserve part:** the shelter can continue with the repaired unit and loses the spare; the story names that exact trade.
- **The report is not received:** preserve the fact that it was sent and use an existing delivery receipt only if available; otherwise the story says receipt is unknown.
- **Two copies conflict:** preserve both dates and sources; do not automatically call either writer dishonest.
- **A supporting Current declines:** continue using shelter capacity or postpone. The declined offer is not converted to hostility without a real cause.
- **A major faction withdraws:** the player can keep a narrower local arrangement, ask for a new meeting later, or end the arc without an invented coup.
- **The player refuses the work:** record the refusal and its material reason if they give one. Do not force a hidden “selfish” conclusion.

The arc must support partial completion. The player may end with a working local service and no regional relationship, a published schedule with reduced supply, a reliable exchange that does not survive a staff change, or a documented pause.

## 18. Replayability

Replayability comes from materially different work patterns and their consequences, not from a random personality label.

A second run can keep the same major-faction commitment and change only one operating practice. For example, the player can use the same Military route but choose a rotating rota, restricted information, and a reserved part; the result should differ from a fixed schedule, public corrections, and a Guild apprenticeship. A Rebel run can centralize the radio caller, while another distributes copies to local teams. An Independent run can renew a dated exchange or complete one cycle and stop.

The arc should also replay well under:

- Different survivor needs and availability.
- Different inventory pressure.
- Different repair readiness.
- Different radio reachability.
- Different access to supporting currents.
- Different prior major-faction standing.
- Different campaign outcomes in the existing evaluator.

No single playthrough should be asked to see every scene. Optional content remains legible if a Current is inactive or a player never uses a specific system.

## 19. Implementation Classification

**Current status:** Proposed, unapproved, documentation only.

This is not a data-only expansion yet. The main content goal requires the game to preserve and query multiple action facts over a quest arc. The current `NarrativeQuestlineSystem` is explicitly limited to one binary fork; the current personal-quest effect binding is incomplete in the working tree. Therefore the plan must not pretend the desired graph is already supported by authored JSON.

Potential implementation categories, pending a named owner and decision:

1. **Content-only candidates:** prose variants that depend on conditions the current host and data loaders already expose and persist.
2. **Core extension candidates:** a bounded, multi-stage branch model under the current narrative-quest owner, with explicit action facts and deterministic transition rules.
3. **Host wiring candidates:** adapter calls from existing owner events, inventory/morale owners, radio, and the campaign day source.
4. **Data candidates:** a versioned quest definition and dialogue/encounter records after the live schema and consumer are verified.
5. **Cross-system candidates:** any outcome requiring a fact from multiple owners, a save section migration, or an endgame consumer.

No line in this plan authorizes production changes. The future implementation must read the current integration and ownership ledgers, claim exact files, establish save and restore ownership, and use focused verification.

## 20. Collision Audit

### Existing content with adjacent subject matter

- Wave 19 plans 92–96 are current user drafts about five settlements and their local characters. This plan does not rewrite those settings or reuse their cast.
- Wave 18 plans 87–91 focus on named characters, a feeder, salvage camp, batching plant, map, and checkpoint record. This plan is a systemic faction-support arc, not another character-prose bank.
- Wave 11 plans 62–64 already cover a cache grid, electrical dispatch records, and sample transport handoffs. Expansion 97 does not claim route dispatch, cargo tracing, or provenance as new mechanics.
- Earlier plans cover the Tally, Standing Record, major faction charters, currents, the Muster, regional routes, and many evidence/provenance stories. Expansion 97 uses the existing systems as context and must not reopen their mechanics under a new label.

### Distinction

The proposed addition is the combination of (a) a player-facing sequence of practical operating decisions, (b) action history that remains factual rather than moralized, (c) distinct follow-through under each existing major faction, and (d) supporting Currents whose roles remain bounded. This combination still needs a deeper repository-wide collision pass in Volume XIV before implementation. The distinction is a proposal, not a novelty claim beyond that evidence.

### Prohibited collision outcomes

- Do not introduce a new “route ledger” when one already exists in current data or prior content.
- Do not make Archivists a tribunal, make the Long Walk a universal courier, or make the Scavenger Guild a free materials faucet.
- Do not create another settlement or faction to justify a support roster.
- Do not reuse a preexisting plan’s character voice, location hook, or flag namespace without explicit evidence and ownership.

## 21. Expansion Hooks

Expansion 97 can leave clean hooks for future content, but it should not require any follow-up expansion to resolve its own arc.

- A later personal arc can ask a survivor whether the schedule gave them rest or only made their work easier to count.
- A regional treaty or epilogue can mention whether the shelter’s service arrangement remained local, was renewed, or ended.
- A future supporting-current story can use the player’s actual record-keeping behavior without changing that Current’s political role.
- A radio callback can report the arrangement’s limits if the transmission was sent and received through the radio owner.
- A new playstyle-focused plan in the requested series may explore another domain, but must not reuse this arc’s central action model with renamed variables.

## 22. Strongest Recommended Content

### Scene: “The Hour Nobody Took”

The player opens the operations board. One interval has no name beside it. There is a line under the hour, then another line under the line, as if somebody had begun writing and stopped.

The shift clerk is sorting pins by color. They do not look up.

> “That’s not an empty shift.”

> “It is on the board.”

> “It’s empty because nobody said yes.”

The player can ask who worked it last, who was meant to cover it, whether it can wait, or what happens if the interval stays blank. The clerk answers with records they can identify. If the previous work was never recorded, the clerk says so. The player may assign a worker, propose a rotation, set a shorter service window, ask for help, or leave the square open for now.

No option is marked righteous. Each shows the immediate cost in staff availability, time, parts, or reliability. The player can leave without a commitment, and the board remains as it is.

### Scene: “Two Copies”

A second sheet arrives with the same hour written differently. One copy shows the planned assignment. The other has a correction and no signature. The maintenance worker recognizes the pencil, but not the handwriting.

> “It’s the time we used.”

> “Which time?”

> “The time after we changed it.”

The player can attach both copies with dates, ask the writer to amend the sheet, send one copy to the major faction, keep both local, or mark the source unknown. The Archivists can offer a folder for the copies if their actual access conditions are met. They do not tell the player what happened in the room.

### Scene: “Return Is a Date”

The Independent broker places a small receipt on the board. It names a part, a return date, and the place where it should be left. The maintenance worker says the part will likely be needed by then. The broker says “likely” is not a term.

The player can accept the date, ask for an extension before signing, offer another part instead, or decline the loan. If they accept and later cannot meet the date, the future branch offers an extension, replacement, a written shortfall, or closure. There is no automatic debt system added by this plan.

### Ending sketch: “The Board Is Still There”

The arrangement ends without a speech. The board stays on the wall because taking it down would not return the hours. One copy has the original plan. The other has a correction that arrived too late to help the first shift and early enough to matter to the next one.

The player’s chosen major faction calls the arrangement something different. The Military calls it limited coverage. The Rebels call it a rota kept in more than one place. The Independents call it a term that ended when it said it would. The shelter calls it the board.

The final lines vary according to what the player actually did: who covered the gap, what stock was spent, whether the correction was sent, and which contributors can still be asked. None says that the player was good, evil, honest, or cowardly. The game describes the work and lets the player understand the rest.

## Current plan boundary

At the time this installment was written, it defined the thesis, evidence limits, initial quest spine, major-faction overlays, supporting-current roles, action dimensions, consequences, and implementation gate. Its note that the requested length had not yet been reached is an interim snapshot superseded by the Plan 1 formal closeout. Expansion 97 remains a design proposal until the project’s normal ownership and integration authorities accept an implementation package.

# Installment 2 — People, branch grammar, and route-specific play

The first installment established the expansion’s purpose and limits. This installment deepens the playable people and the decisions they bring into the same shared problem. The content below remains proposed. Character names and IDs are working material; they are not game canon until checked against the full data authority and adopted through the project’s normal content process.

## 23. The shelter cast: full working dossiers

The central cast should be small enough that the player remembers who owns which piece of knowledge. Three recurring characters make the arrangement personal without turning them into representatives of every system. They know different facts, have different tolerances for uncertainty, and can disagree without one being the moral answer.

### 23.1 Rade Seln — shift clerk

**Working role:** keeps the board legible, records substitutions, and notices when a task quietly becomes a permanent expectation.  
**Working name:** provisional; a narrow search found no exact match in current authored data or expansion prose, but the required full collision pass is still pending.  
**Prior experience:** before the shelter routine formed, Rade copied shift changes between two rooms because the radio handsets were unreliable. He believes a schedule is a promise only when the person expected to perform the work has seen it. He has no interest in making the board beautiful.

**Competence:** Rade can tell the player which version was posted and when it was changed. He notices repeated substitutions and can describe whether a change was acknowledged. He cannot tell whether the person at the work site actually completed the task unless another system or witness records it.

**Blind spot:** Rade trusts legibility more than resilience. If the board has a complete row, he is tempted to treat it as solved even when the worker is tired, the tool is failing, or the assignment has become unfair. This is a procedural weakness, not an evil secret.

**Relationship pressure:** Rade resents being asked to remember oral changes that nobody wanted written down. He does not want his record to become a disciplinary instrument. If the Military route asks for exact coverage, he asks who will read the names. If the Rebel route asks for distributed copies, he asks who will remove a stale one. If the Independent route asks for a term, he wants a line that says when the term ends.

**Player interaction:**

- If the player has used a fixed schedule and later honored a change request, Rade asks whether the next rota should include an explicit swap line.
- If the player repeatedly assigns the same person to an emergency shift, he does not accuse them of exploitation. He points at the repeated name and asks whether it is still a choice.
- If the player leaves a square blank, he does not auto-fill it. He can preserve the blank as evidence that no agreement exists.
- If the player corrects a record, Rade offers to attach the old version instead of letting it disappear.

**Possible personal-arc resolution:** Rade can learn to treat a board as a record of decisions rather than proof of compliance. Alternatively, he can leave the clerk role after the player turns every correction into a demand for justification. He may also remain, with the explicit rule that no worker is assigned by his handwriting alone.

**Voice:** short, exact, often questions the verb rather than the motive.

> “You changed the hour. Did they change with it?”

> “I can write ‘covered.’ I can’t write ‘safe’ unless somebody checked.”

> “The blank isn’t an answer. It is what we know.”

### 23.2 Osha Marin — runner and communications hand

**Working role:** carries messages between people when the radio path is uncertain, keeps the receipt or admits there is none, and distinguishes sending from delivery.  
**Working name:** provisional; the initial collision search found no exact match.  
**Prior experience:** Osha took messages between shelter teams during a period when the radio station had power but several field sets did not. She remembers the physical trip and the time it took, not every sentence that was said at either end.

**Competence:** Osha knows when a note left the shelter, who carried it, and whether the receiver acknowledged it. She can tell the player that a transmission was attempted. She cannot confirm a recipient’s understanding from a successful carrier tone alone.

**Blind spot:** Osha avoids writing interpretations. This protects her from being blamed for the wrong reading, but it can leave a later reader with a sequence of times and no explanation of why the message changed. She prefers a blank “reason” field to a guess, even when the guess would help someone make a timely decision.

**Relationship pressure:** Osha once carried a correction that arrived after people had acted on the earlier instruction. She will not make that story a centerpiece. If asked, she says the correction was true and late. She is concerned when the player broadcasts a schedule without saying whether it is a plan, a request, or a confirmed assignment.

**Player interaction:**

- A player who uses radio carefully can ask Osha to carry a correction only when the recipient needs the detail.
- A player who sends broad broadcasts can ask her to make the call at a fixed hour; she warns that predictable contact is not the same as safe contact.
- A player who relies on couriers can give Osha a route plan or allow her to choose an alternate path. Route hazards remain owned by the expedition system.
- A player who refuses to send a message can ask Osha to record that refusal. She will not invent a delivery attempt.

**Possible personal-arc resolution:** Osha can learn to include concise context alongside a timestamp. She can also set a boundary that she will carry the words she was given but will not add a reassurance the sender did not provide. If the player repeatedly says “they know” when there is no receipt, Osha may stop carrying messages that depend on that assumption.

**Voice:** concrete, focused on what crossed a boundary and when.

> “I sent it. I don’t have a receipt.”

> “That was a correction. It was not there in time to stop the first shift.”

> “Say whether you want them to know the time or know the plan.”

### 23.3 Mina Ver — maintenance worker

**Working role:** inspects the working parts, knows which tool has been borrowed, and distinguishes a repair from a replacement that only looks like one.  
**Working name:** provisional; the initial collision search found no exact match.  
**Prior experience:** Mina learned maintenance from several workers who disagreed about what counted as a usable spare. She keeps failed parts until the repair decision is recorded, not because she believes every broken thing can be fixed but because a cracked part can explain why the replacement failed too.

**Competence:** Mina can describe visible wear and what has already been attempted. If a live subsystem exposes a condition, the quest may show that condition. She cannot guarantee remaining lifetime or safety beyond the owning system’s rules.

**Blind spot:** Mina prefers a repair that she can explain to a replacement she cannot. That can spend too much labor on a tool that should be retired. She will acknowledge when she is out of time but may take too long to say it.

**Relationship pressure:** Mina dislikes being asked to label all salvaged material as either useful or worthless. She can make a test plan with the player, but she will not install an item that the existing repair owner rejects. On the Scavenger Guild path, she is interested in apprenticeship but does not want her knowledge treated as a free service.

**Player interaction:**

- If the player repairs first, she asks whether the repair is temporary, monitored, or meant to be forgotten after it works once.
- If the player replaces the part, she asks whether the removed part can explain the failure or whether it should be discarded through an existing process.
- If the player spends the reserve, she records that the reserve changed purpose instead of calling it waste.
- If the player delays, she can help define what evidence would make a return to the task worthwhile.

**Possible personal-arc resolution:** Mina can teach a shelter worker one inspection routine through an existing skill or training owner, if that hook is available. She may also decide that the best contribution is to document a retirement threshold rather than keep repairing the same unit. The plan must not create an independent maintenance skill system.

**Voice:** material nouns, few generalizations, no miracle repairs.

> “It ran after the repair. That is the fact we have.”

> “If you want it kept as a spare, write down what it can still do.”

> “I can show you the wear. I can’t tell you which need matters more.”

### 23.4 The three-person conflict

Rade wants a visible commitment, Osha wants a reliable receipt, and Mina wants a truthful account of what the machine can do. None of them has the whole operation. The player’s role is not to reconcile their personalities; it is to make a decision that is specific enough for each person to act on.

A key scene should let the trio reach different working agreements depending on prior action:

- A player who favors regular shifts may ask Rade to post a fixed rota, Mina to identify a maintenance check, and Osha to report only missed windows.
- A player who favors adaptation may ask Rade to preserve changes, Mina to report when repair evidence changes, and Osha to carry corrections with a clear send/receipt distinction.
- A player who keeps the operation local may ask Rade to hold the record in the shelter, Mina to define a stop condition, and Osha to send no outside schedule unless the player authorizes it.
- A player who uses outside support may ask each Current for one bounded service, while retaining the shelter’s own plan and responsibility.

The trio should not become a single “party” with a combined loyalty meter. Their disagreements remain separate and can be revisited in later scenes.

## 24. Branch grammar and action predicates

This section converts the story concept into rules that a future implementation review can challenge. The predicates below are design examples, not existing API names or approved flag IDs.

### 24.1 Record a fact with its context

An action witness should contain, at minimum, the action kind, source owner, day, and relevant subject or operation. A simple boolean can tell a branch that something happened; it cannot explain whether it happened before the disruption, after the correction, or for a different survivor. If the canonical owner cannot capture this information, the content must not present it as persistent history.

**Illustrative witness, not production schema:**

| Field | Example value | Why it matters |
|---|---|---|
| Action | schedule changed after absence | Distinguishes adaptation from a fixed rota |
| Source | duty roster / quest choice / radio receipt | Makes the fact traceable to an owner |
| Day | campaign day when confirmed | Enables delayed callbacks and chronology |
| Subject | operation or assigned survivor, where applicable | Prevents one person’s action being attributed to another |
| Confidence | performed / attempted / acknowledged / unknown | Separates action from delivery or result |

The design does not need an intent field. It should not store “player trusts people,” “player is disciplined,” or “player is selfish.” Those are interpretations and should not be persisted as facts.

### 24.2 Branch choice is not a score threshold

A branch opens when one or more relevant witnesses exist, but a player can always choose an available action now. For example:

- A prior roster change opens a “keep the correction beside the schedule” dialogue; it does not force that path.
- A completed trade opens an “offer a dated exchange” approach; it does not make the player a broker archetype.
- A radio receipt opens a “reuse the confirmed contact hour” option; an attempted but unacknowledged transmission does not.
- A maintenance action opens a “use the repaired unit again” option only if the active equipment owner still considers the unit usable.

If multiple witnesses point in different directions, the game should offer multiple paths. It must not average them into a hidden alignment.

### 24.3 Choice, act, and outcome are distinct

The plan uses three different concepts:

1. **Choice:** what the player selects in the current scene.
2. **Action:** what the relevant authority confirms happened (for example, a resource was spent or a shift was assigned).
3. **Outcome:** what the later scene observes (the task succeeded, failed, arrived, or remains unknown).

A selected option does not always imply the action completed. A sent message may not be received. A repair may be refused by the subsystem. A loan may be agreed but later missed. The quest state must not set an outcome flag from a button press when the relevant owner has not reported the outcome.

### 24.4 Reconnection points

The quest can rejoin branches at four moments without erasing their differences:

- After the first support window, everyone learns what the selected work method cost.
- After the absence, all routes confront a staffing change but carry distinct records.
- After the correction, all routes decide who holds the next version.
- At the closing table, all routes select what remains available, but each major faction explains the result from a different institutional purpose.

Reconnection avoids exponential content growth. The plan can support many combinations by using fact-specific callbacks at shared scenes rather than writing a unique full campaign for every permutation.

## 25. Extended action matrix

The first installment listed six dimensions. This matrix makes their cost and feedback more precise. Each row must use a real owning system or an explicit authored choice with persistent state. Rows with no verified signal remain proposals.

| Action domain | Player-facing alternatives | Required observable fact | Good callback | False inference to avoid |
|---|---|---|---|---|
| Shift coverage | fixed assignment, rotation, on-call, reduce scope | actual roster change or explicit quest choice | who covered the missed hour | “fixed means responsible” |
| Repair posture | repair, replace, inspect, retire, defer | owner-confirmed repair state or explicit decision | whether the unit remained usable | “repair means frugal” |
| Information | publish, send privately, duplicate, correct, withhold | authored disclosure choice plus send/receipt state where relevant | who saw which version | “private means dishonest” |
| Material use | reserve, spend, loan, exchange, substitute | inventory/economy transaction or saved choice | which future option disappeared | “share means generous” |
| Current support | ask, decline, ask later, accept a bounded service | current access and actual interaction outcome | whether the promised service was delivered | “decline means hostile” |
| Failure response | repeat, revise, reduce, suspend, document uncertainty | confirmed outcome and follow-up selection | whether the same failure recurred | “repeat means stubborn” |
| Authority | assign centrally, delegate, invite local decision, refuse delegation | actual owner or explicit task assignment | who can revise the next schedule | “delegate means weak” |
| Time | act now, wait for forecast, schedule a safe window | existing calendar/weather condition and selection | cost of delay or avoided exposure | “wait means cowardice” |
| Relationship | ask, explain, accept refusal, override | social system event or explicit choice | whether the person returns to the task | “agreement means affection” |
| Record correction | replace old copy, append amendment, mark disputed | version and source that the quest owner persists | later reader can see the sequence | “first record is true” |

### 25.1 When dimensions interact

The content should create recognizable interactions rather than an unrelated choice list. Examples:

- A fixed rota with a publicly posted correction can help the Military plan around a narrow service window. If a worker is unavailable, the same public promise can make the missed interval costly.
- A rotating rota with distributed copies can fit the Rebel route. If the player changes it without sending the correction, distributed control turns into uneven information.
- A dated loan can work on the Independent route when the return date remains visible. If the player spends the item and hides the shortfall, the later conflict comes from the unreported action rather than from a moral meter.
- A reserved component can preserve a repair option after a storm. If a Current offers instruction rather than free material, the player can save the part while still gaining useful knowledge.
- A radio report marked “unconfirmed” can be more actionable than an unsupported exact hour. If no receipt arrives, the game keeps the outcome unknown.

### 25.2 Changing practice mid-arc

The player may change course after learning something. That can be one of the strongest forms of branching. A player who begins with a fixed assignment can later move to rotation after observing fatigue. A player who publishes a schedule can restrict later detail after seeing how broadly the first copy traveled. A player who holds reserve stock can lend it after the system reveals that the spare is not needed for the feared contingency.

The story should record both the first and later practice. It should not erase the first decision or label the player inconsistent. A later character can say, “We changed how we do it,” and point to the day the change began.

## 26. Military route: detailed sequence

The Military overlay needs its own quest rhythm and consequence shape. It should not use the same scene order and simply replace the faction name. Its central tension is the difference between a service that can be planned around and a guarantee that the shelter cannot safely make.

### 26.1 Opening — “A time they can use”

A liaison asks for the window when the shelter can maintain the shared service. The request is reasonable: the Military has assignments downstream and needs to know whether the system will be available. Rade points out that the schedule names a worker who has been absent from two recent shifts. The liaison does not know this because the previous report said only “covered.”

The player can:

- Confirm the old window and assign coverage now.
- Offer a shorter window that the shelter can staff.
- Send a provisional schedule with a clear uncertainty note.
- Wait until the worker’s availability is confirmed.
- Decline to provide a schedule and keep the arrangement local.

Immediate response changes who the liaison contacts next. None is a morality choice. The short window can save capacity but frustrate the Military’s downstream plan. The provisional schedule can help planning while risking a later correction. Waiting can improve confidence while consuming time.

### 26.2 First work phase — “The name beside the check”

Mina identifies a maintenance action that must happen before the service window. The task costs a worker and a part. The player can ask the Scavenger Guild for an assessment, assign the existing maintenance shift, or reduce the service window until the part is confirmed. The Guild’s assessment is optional and its result is not a guaranteed repair.

Rade notices that the posted schedule has a check mark in the completed column before Mina has finished. This is not framed as fraud: the mark may have meant “assigned” to the person who wrote it. The player decides whether to clarify the column headings, correct the mark, or leave the old copy with an attached note.

### 26.3 Second work phase — “A guarantee has a body”

The liaison returns with a plan that uses the confirmed hour. One shelter worker is needed for another urgent task. The player chooses a substitute, keeps the service narrow, or withdraws the guarantee. If the player previously used a fixed assignment, this scene tests whether they can revise it. If the player previously chose rotation, the scene checks whether the next worker has enough information to take over.

The liaison responds to the actual plan:

- A fixed, confirmed hour lets the Military schedule around it but creates a risk if the named worker is unavailable.
- A narrower window reduces the downstream promise and preserves shelter capacity.
- A rotation spreads the load, but only if the replacement knows the task and the service can tolerate a handoff.
- A refusal returns the service to local control and removes the Military’s planning benefit.

### 26.4 Record and correction — “The first report stands beside the second”

A report has already reached the liaison. A new report changes the available hours. The player may send an amendment and preserve the original; issue a replacement that clearly marks the earlier report superseded; provide only the new window; or ask the liaison to hold action until confirmation.

If the player previously sent uncertain information, the liaison can distinguish it from a false guarantee. If the player previously omitted uncertainty, the liaison may ask why. It does not automatically reduce standing; the consequence is specific to an operational plan that used incomplete information.

### 26.5 Closing agreement — “Coverage, not ownership”

The Military asks to keep the schedule for the next period. The player can agree to a time-limited service, renew only after checking staff, provide a reduced service, or end the arrangement. The closing text reflects actual deliveries, absences, repairs, and corrections.

**Military resolution families:**

1. **Narrow reliable coverage:** fewer hours, a worker who can be replaced, and a schedule that accurately names its limits.
2. **Commanded coverage:** the Military receives a regular window and clearer authority over when it begins, at a cost to the shelter’s discretion. This path can still be chosen by a player who values dependable coordination.
3. **Conditional support:** the player keeps the local service but shares a report only when a specified condition is met.
4. **Closed window:** the operation ends because the capacity is not there. The record names why and what would need to change before reopening.

## 27. Rebel route: detailed sequence

The Rebel overlay centers the problem of distributed knowledge. Its people can adapt quickly because they do not wait for one office to approve every change. That same independence makes it harder to know whether a message has reached every participant.

### 27.1 Opening — “Which copy is current?”

A caller brings a rota with several pencil amendments. The service was completed, but two teams believe they were promised the same hour. Nobody has hidden the first sheet. The player can see that each person has a different version.

The player can publish one shared schedule, leave each group its own copy and appoint a correction runner, choose a rotating caller, or shrink the service until the groups can agree. A player who has already broadcast corrections may open a route that uses the same channel; a player without that history can establish one now.

### 27.2 Local control — “Who can change a line?”

The next scene asks whether a team can swap a worker without the player. If the team can, it can adapt to a need that the shelter did not foresee. If it cannot, everyone waits for a single approval. The player may authorize direct swaps, require a named acknowledgement, or allow changes only for a listed set of reasons.

This is a mechanical boundary, not an alignment judgment. More autonomy can reduce response time and weaken central visibility. More control can improve consistency and delay an urgent substitution.

### 27.3 The missed correction — “A message without a witness”

A runner says the new hour was carried to one contact. Another contact did not receive it. The player can ask Osha to make a second trip, delay the operation until both groups know, proceed with the people who received the correction, or cancel the shared window. The scene distinguishes “message sent” from “all teams informed.”

If the player has distributed copies, the second group may have the correction already. If one caller carried every update, that caller becomes a point of dependence. If the player did not set any communication path, the game says what is unknown and lets the player create one.

### 27.4 Shared work — “A swap that leaves a mark”

A worker asks to exchange shifts. The player can approve the swap directly, ask both workers to acknowledge it, change the task so no exchange is needed, or deny it because the service cannot be covered safely. If the player previously promised local autonomy, overriding the swap has a specific political cost; if the player has a safety condition attached, refusal can be consistent with the agreement.

### 27.5 Closing agreement — “Copies that can be corrected”

The Rebel representatives want a system that can be changed without waiting on the shelter. The player can leave all copies local, appoint rotating readers, allow direct corrections with dated source marks, or keep one master copy and permit local annotations.

**Rebel resolution families:**

1. **Distributed rota:** changes can be made locally; the player accepts that the copies may temporarily diverge.
2. **Rotating caller:** one person coordinates for a short period, then hands the role to someone else. The arrangement is quick but dependent on a clean handoff.
3. **Local annotations:** each group can record what happened to its own copy, and the player accepts the extra work of reconciling them later.
4. **Reduced shared service:** the parties keep the autonomy but reduce the hours until they can reliably coordinate.

The Rebel path must not become a generic “chaos” route. Its successes should be tangible: faster adjustment, useful local knowledge, and people who can act without waiting for the player. Its costs should also be specific: uneven information, more reconciliation work, or a local correction that did not reach every group.

## 28. Independent route: detailed sequence

The Independent overlay centers bounded commitments. The player can negotiate what is exchanged and when it ends. The route’s key pressure is not whether to be fair; it is whether the agreement describes what the parties can actually deliver and how they can change it.

### 28.1 Opening — “Name the end date”

Two groups arrive with different expectations about a shared service. One thinks the borrowed part returns after the first successful shift. The other thinks it returns after the whole schedule cycle. Neither has written a date. The broker will not decide for them.

The player can propose a one-shift loan, a week-long exchange, a rolling renewal, or no exchange. Each proposal states the material, window, and expiry. If the current economy system supports a transaction, the real transaction uses that owner. If no transaction owner matches, the choice must remain a quest-state commitment and must not pretend to change inventory.

### 28.2 The term changes — “The return cannot be made”

The part or service cannot be returned on the first date. The player has a choice: arrange an extension before the term closes, offer a replacement, document the shortfall and propose a new date, or end the agreement and accept the consequence. The other party can accept, counter, or decline based on what was promised, not on a hidden trust score.

A player who previously stated uncertainty can open an earlier extension conversation. A player who made an unconditional promise can still renegotiate, but the recipient needs a reason to believe the next date. A player who kept a dated record can show exactly when the plan changed.

### 28.3 The count — “Same goods, different accounts”

Two receipts agree on the quantity but disagree about whether it was a loan or an exchange. The player can preserve both descriptions, ask both parties to rewrite the term, choose one account as the operative agreement while keeping the other as a dissent, or end the arrangement. The Archivists can preserve the two versions if their access path is available; they do not determine the legal or political outcome.

### 28.4 Renewal — “Ask again”

The player may renew the exchange, let it expire, renew with a smaller quantity, or switch to a different contributor. Renewal is not automatic. A Current’s support remains bounded by its own rules; a prior contribution does not create permanent entitlement.

**Independent resolution families:**

1. **Renewable term:** the service continues through explicit renewal, with a visible expiry and a clear return obligation.
2. **Completed exchange:** one cycle ends cleanly. The parties can collaborate again later without a standing relationship being imposed.
3. **Revised agreement:** the quantity, schedule, or contributor changes after a documented shortfall.
4. **Negotiated pause:** the service stops until a stated condition is met. The pause preserves the relationship without pretending that work is continuing.

## 29. No-commitment route: detailed sequence

The neutral route must be a real branch, not an absence of content. It exists for the player who refuses Military, Rebel, and Independent commitment or who is not yet eligible to commit under the existing system. It has its own limits and endings, but does not create a rival polity.

### 29.1 Opening — “We can keep it here”

A worker asks whether the shelter intends to make the service regional. The player can say yes, no, not yet, or “only if the next cycle holds.” This is a direct choice, not a inference from prior morality. The answer changes whether the supporting Current is asked to help with a local task or to contribute to an outside-facing agreement.

### 29.2 Local capacity

Without a major faction, the shelter lacks some external manpower and command reach. It still has the ability to repair, schedule, communicate, trade, and refuse. The player can maintain a smaller local window, ask a Current for an optional specific contribution, or end the service. The plan should not penalize the player by making all neutral endings collapse.

### 29.3 The limits of refusal

A local arrangement affects other people. If the player declines to share a schedule, an outside party cannot use it. If they reserve a part, another repair waits. If they keep the record local, a distant reader may not know that a service existed. These are not alignment judgments; they are consequences of scope.

### 29.4 Neutral resolution families

1. **Local continuity:** the service continues within the shelter boundary and is not promised outside it.
2. **Open invitation:** the player keeps the service local but allows one Current to contribute on agreed terms.
3. **Timed review:** the player makes no long commitment and chooses when the next review happens.
4. **Explicit refusal:** the service is discontinued and the reason, if stated, remains in the record.

## 30. Supporting-current service playbooks

Each Current gets a defined service card for this questline: what can be asked, what the Current can actually do, what the player must supply, what result can be observed, and when the Current declines. This keeps the supporting role narrow.

### 30.1 Archivists — “Two versions, one shelf”

**Offer in this arc:** preserve two dated versions of a schedule or receipt together. The Archivists can note that one version was amended after the other.  
**Player contribution:** provide the source and date; where the existing access rule requires corroborated testimony about a person, the player must meet that rule or the record remains a copy without a name.  
**Result:** later scenes can refer to the existence of both copies. The archive does not establish which schedule was performed.  
**Decline state:** if the player does not meet the Archivists’ access rule, the answer is a clear refusal or a narrower anonymous record, depending on existing canon. The plan does not weaken the rule for convenience.

The Archivists’ side quest is about a missing date, not a missing truth. The player can leave the date uncertain, add a witness, or keep the copies separate. Any statement about the deceased or survivor named in a record follows current canon and privacy rules.

### 30.2 Long Walk — “A report from beyond the hour”

**Offer in this arc:** a sector-wide situation report when the relevant contact is reachable.  
**Player contribution:** a question that can be answered by the report and enough time for it to arrive.  
**Result:** the player receives a dated report whose confidence and arrival time are visible. It may inform the next cycle, not rewrite the previous one.  
**Decline state:** the route is not reachable, the report cannot be assembled in time, or the Long Walk does not know. The quest continues with local information.

The Long Walk’s side content can introduce one unreliable early estimate and a later correction. The lesson is not “never trust reports.” It is that every report has a horizon and a date. The player can make room for that in the next schedule or ignore it and accept the consequence.

### 30.3 Scavenger Guild — “Show, then teach”

**Offer in this arc:** a salvage assessment or apprenticeship opportunity tied to an actual expedition or repairable item.  
**Player contribution:** material access, a willing learner, or a completed expedition that can inspect the candidate source.  
**Result:** the Guild shares an assessment or teaches a bounded skill through an existing training/knowledge route if one is available. The item remains subject to inventory, expedition risk, and repair authority.  
**Decline state:** no qualified Guild contact, no accessible salvage, no willing learner, or the player declines the cost.

A later callback recognizes whether the player used the instruction. It does not silently grant the materials that the lesson described. The Guild retains a distinct economic identity; it is not converted into a charity arm of the chosen major faction.

### 30.4 Optional-current candidates

The Grain Exchange, Lamplighters, The Tally, Quiet House, and other inactive or conditionally active currents may be considered only in later volumes after checking live activation, access, and consumer routes. They are not required for the main arc. The plan must not assume that data presence equals availability.

## 31. Playstyle routes: deeper definitions

These are descriptive names for content coverage, not permanent player classes. A player may combine them or change direction. Dialogue should describe the observed action, not call the player an archetype.

### Maintainer pattern

**Observable practice:** the player inspects, repairs, or retires equipment through the owning subsystem before buying or consuming a replacement.  
**Strength:** fewer blind replacements; better continuity when existing equipment remains serviceable.  
**Cost:** more labor, a longer period of uncertainty, and the risk of spending time on an asset that should be retired.  
**Distinct scenes:** Mina asks for a stop condition; the Scavenger Guild can teach assessment; the final copy distinguishes “operational today” from “reliable for the season.”

### Scheduler pattern

**Observable practice:** the player assigns or rotates work with a known cadence and follows through on a changed assignment.  
**Strength:** other people can plan around a time they have seen and accepted.  
**Cost:** fixed commitments can be brittle when the worker or equipment becomes unavailable.  
**Distinct scenes:** Rade asks whether to keep a substitution line; the faction representative requests an exact service window; the ending names who carried the absence.

### Courier pattern

**Observable practice:** the player sends corrections and checks whether they were received before relying on them.  
**Strength:** more parties can act from current information.  
**Cost:** repeated communication consumes time and can expose the schedule or sender.  
**Distinct scenes:** Osha separates sending and receipt; the Long Walk report arrives too late for one decision but before another; the closing copy includes a delivery boundary.

### Reciprocal trader pattern

**Observable practice:** the player offers a trade or loan with a quantity and return condition and later reconciles it.  
**Strength:** material can move without a permanent allegiance or vague obligation.  
**Cost:** an explicit term can become a visible shortfall when circumstances change.  
**Distinct scenes:** the Independent broker negotiates an extension; the player can replace rather than conceal; a completed exchange ends without creating an automatic alliance.

### Reserve keeper pattern

**Observable practice:** the player preserves scarce capacity or stock for a future need instead of spending it on the current service.  
**Strength:** the shelter retains a response option.  
**Cost:** a current need remains unmet, and another party may have to wait.  
**Distinct scenes:** a later failure tests what the reserve was for; the shelf is inspected; the ending can value a preserved option without claiming it was always the best choice.

### Improviser pattern

**Observable practice:** the player changes the schedule, method, or contributor after a confirmed disruption.  
**Strength:** the plan responds to live conditions instead of requiring people to obey a stale record.  
**Cost:** every change needs a new handoff; several changes can leave people uncertain about which instruction is current.  
**Distinct scenes:** Rade preserves revisions; Osha carries the correction; the Rebel route can make local changes easier while increasing reconciliation work.

### Direct operator pattern

**Observable practice:** the player personally performs or directly supervises the task when existing systems allow.  
**Strength:** fewer handoffs and more immediate control.  
**Cost:** the player’s time becomes the bottleneck, and knowledge may not transfer to the next worker.  
**Distinct scenes:** Mina asks who can repeat the repair; the closing agreement distinguishes a task that depends on the player from one the shelter can continue.

### Delegator pattern

**Observable practice:** the player assigns work to trained survivors or requests one specific Current contribution.  
**Strength:** broader capacity and specialist knowledge.  
**Cost:** the player must communicate limits and accept that the work may be carried out differently.  
**Distinct scenes:** the assigned person can decline or request clarification; the Current defines its service boundary; the ending shows whether authority was actually shared.

### Hybrid patterns

The most interesting runs may combine these practices: maintain equipment but rotate people; keep the schedule local but publish the correction; reserve one part while asking the Guild to teach a repair; directly supervise the first cycle and delegate the second. The plan must make hybrids first-class. No route should require a single dominant style across the whole campaign.

## 32. Ending matrix: operational outcomes and faction overlays

The arc’s ending family is selected by the final arrangement and its action history. The major faction overlays alter the language, scope, and follow-up actor. They do not replace the action outcome with one generic faction label.

| Ending family | Action conditions | Practical result | Delayed callback | Main risk |
|---|---|---|---|---|
| **A schedule that listens** | A schedule was kept at least once and revised after a real absence; the revised plan was communicated to those expected to act | A stable rota with a defined substitution path | A later worker uses the substitution line without asking the player | The schedule may become over-detailed or slow to update |
| **Two copies, both dated** | The player preserved an original and a correction with their sources or uncertainty intact | A reader can distinguish planning from later observation | A faction representative cites the corrected copy, not the superseded hour | More readers can see sensitive operational detail |
| **The spare remains on the shelf** | A real reserve was held rather than spent; a future inspection confirms it remains usable | The shelter retains an option for a later breakdown | A later need uses the reserve or reveals why it was kept | The current external task receives less support |
| **One cycle, then renewal** | A bounded exchange was completed or explicitly renegotiated before expiry | The parties can work together without permanent entitlement | A Current or faction asks again after the term expires | Every cycle requires another negotiation |
| **Work shared without a master copy** | Several groups contributed and the player accepted distributed corrections | Local teams can adapt with partial autonomy | A later disagreement brings multiple local records to the board | Reconciliation needs time and readers may not share one account |
| **The hour is closed** | The player ended or suspended the service with a stated condition or explicit refusal | No false promise remains active | A future scene can reopen the task if the condition becomes true | The service does not benefit the outside party during the pause |
| **The promise outlived the capacity** | The player kept a larger promise after a confirmed failure without a viable replacement or correction | The service becomes unreliable and at least one relationship is strained | A later request is smaller or requires evidence before acceptance | The player loses a future option; no catastrophic global penalty |

### 32.1 Military wording variants

- “The coverage window is narrow and confirmed.”
- “The old schedule remains in the file. The correction sits on top.”
- “No outside service was promised for the next cycle.”
- “The service is closed until the spare is inspected.”

### 32.2 Rebel wording variants

- “The latest hour is on three boards. One still carries the old mark.”
- “The local caller can change the rota; everyone else needs the correction.”
- “No one owns the hour after it ends.”
- “The groups agree on the work and disagree about the copy.”

### 32.3 Independent wording variants

- “The term expires on the date written.”
- “The exchange ended. The door is still open to another one.”
- “The shortfall is recorded, and the new quantity is smaller.”
- “Renewal is not assumed.”

### 32.4 No-commitment wording variants

- “The shelter keeps the service for its own use.”
- “The invitation remains open, but nothing is scheduled.”
- “The work was stopped. The reason was not supplied.”
- “The board has a review date, not a promise.”

## 33. Scene bank: additional state-aware encounters

These scenes are modular. A scene should only appear when its factual predicate is available. Each includes the observed detail, the player’s practical options, and the callback that proves the choice mattered.

### 33.1 “The pin moved twice”

A blue pin has been moved from one hour to another and back again. There is a shallow crescent in the board where its point went through the paper. Rade says he moved it once. Mina says the shift happened at the second hour. Neither claims the other is lying; they are describing two different parts of the process.

The player can ask both to mark their version; replace the sheet and attach the old one; keep the board as-is; or choose a confirmed hour and ask the participants to acknowledge it. The later result checks whether the people who needed the new hour saw it. The pin itself is not treated as proof of attendance.

### 33.2 “A lamp left in the wrong room”

A portable lamp is found beside the maintenance bench rather than the scheduled work site. If the player’s route has used light or field coverage, a supporting contact can explain who last borrowed it. The player can return it, assign it to the next shift, keep it as a reserve, or ask the owner before moving it. No option silently creates a new lamp or changes the route map.

### 33.3 “The worker who said no”

A worker refuses a shift. Their reason is brief. The player can ask whether they want to explain, accept the refusal, offer a different task, or insist if the current duty system and rules permit assignment. The scene must obey the actual system’s legal actions; it cannot authorize an action the simulation forbids.

The later branch distinguishes consent, capability, and assignment. A worker who declines because of fatigue is not marked disloyal. A worker who accepts a different task may still support the arrangement. If the player overrides a refusal through a valid system command, the relationship consequence should be specific to that command.

### 33.4 “The receipt with no cargo”

A form marks an exchange as received, but the goods are not in inventory. The player can check the inventory authority, ask the sender, preserve the discrepancy, or amend the receipt as “reported, not found.” The game must not infer theft from absence alone. If the inventory system proves the item never arrived, the action can create a factual discrepancy; if not, the story remains uncertain.

### 33.5 “The room that was ready”

The work area is clean and prepared, but the equipment is still offline. A survivor has prepared the space for a shift that no longer exists. The player can reassign the room, wait for the subsystem to confirm readiness, or leave it ready for the next opportunity. This scene ties physical environment, labor, and operational truth together without inventing a second building state.

### 33.6 “The second knock”

A Current contact returns to ask whether the player wants the same bounded service again. If the previous service was accepted and completed, the player can renew, change the task, or decline. If the previous request was declined, the Current does not behave as though it was accepted. This is a test of consequence chronology, not a loyalty check.

## 34. Dialogue variation by action, not morality

The same character can respond differently to the same major faction based on what the player actually did. These short lines are examples for the later full dialogue bank.

| Observed action | Rade | Osha | Mina |
|---|---|---|---|
| Kept a fixed time | “I’ll leave the hour where it is. Tell me if the worker changes.” | “I can carry this one time. I’ll mark it as confirmed.” | “The unit held through that window. It still needs another check.” |
| Changed the time and recorded why | “The old line stays attached. The new line is the one they should use.” | “I’ll carry the correction and say it replaces the earlier time.” | “The repair took longer. I’d rather the next schedule show that.” |
| Changed the time without notifying everyone | “The board is current here. I don’t know who else has this copy.” | “I can take the update. I can’t make the first trip arrive sooner.” | “The machine is ready. The people may not be.” |
| Reserved a part | “I’ll mark it held, not available.” | “I won’t offer it in the report.” | “If it stays on the shelf, it still needs checking.” |
| Lent a part under terms | “The due date is visible.” | “I’ll carry the receipt, not promise the return.” | “I can tell you what we lent. I can’t guarantee what comes back.” |
| Declined Current support | “Then the assignment stays with us.” | “I’ll leave the request unsent.” | “We can still inspect what we already have.” |

No line uses “you are the kind of person who…” The dialogue stays local to the action. If the player later changes course, the next line reflects the newer action and its relation to the old one.

## 35. Persistence and restore contract, expanded

The branch experience depends on history, so save/restore must preserve enough state to reproduce future availability. This plan deliberately does not specify a new save store. It asks the integration owner to compare current save owners and choose one authoritative location.

### 35.1 State that must survive

- Active arc identity and stage.
- Major route context used for the arc, without duplicating the FactionBranchCoordinator’s commitment.
- Explicit player selections that have not yet produced an owner-confirmed action.
- Confirmed actions with day and source where callbacks depend on sequence.
- Outcomes reported by the owning system: completed, refused, unavailable, failed, or unknown.
- Supporting-current participation and the actual service accepted.
- Callback due conditions already established by the current calendar/quest owner.
- The final resolution and whether the arc has already emitted its consequence events.

### 35.2 State that must not be guessed on restore

- Do not mark a radio message received because it was sent before saving.
- Do not assume a borrowed item was returned because the agreement was renewed.
- Do not turn an attempted repair into a successful repair when restoring.
- Do not reapply morale or item rewards just because the selected option is present in the save.
- Do not repeat a faction callback if the owning event already recorded it.
- Do not recreate a supporting Current offer whose access conditions are no longer satisfied.

### 35.3 Deterministic ordering

Action witnesses should be processed in a stable order, preferably by campaign day and an explicit owner sequence or source identity. Hash-set enumeration must not decide which branch becomes visible. UI presentation must not consume simulation RNG. If a quest option uses RNG through an existing owner, the same seeded input and same action record must produce the same result.

### 35.4 Restore presentation

After restore, the board should show the correct latest version and any attached superseded copy. The panel should refresh through existing lifecycle events. A restored action history should not fire fresh “action completed” events merely because it was reconstructed. These requirements must be checked against the selected host and save owners before implementation.

## 36. Integration boundary: evidence required before any code package

Expansion 97 remains a design plan. A future implementation package must not begin from this document alone. It should first answer these questions from current source:

1. Which current subsystem owns each observable fact: roster change, equipment readiness, inventory spend, message receipt, Current interaction, and dated agreement?
2. Which of those facts are saved today, and through which save section and restore order?
3. Can the selected quest owner express conditional branches, delayed callbacks, and multiple action witnesses without a parallel ledger?
4. Which public event can the host subscribe to without adding gameplay decisions to a panel?
5. Which existing data schema and catalog validator consume the proposed quest records?
6. How does a new action fact interact with existing faction exclusivity and the moral-band entry gate without quietly changing either contract?
7. How will the UI explain an unavailable option using player-facing wording rather than internal flag IDs?
8. How will a future focused test prove the production route, not only a directly constructed Core object?

If those answers require a new architecture decision, the work stops at the design boundary until the named authority supplies it. This is not a reason to weaken the content graph; it is a requirement to give the graph one real owner.

## 37. Acceptance sketch for the finished expansion plan

The finished 120,000-word document should be considered complete only when its final review can find all of the following without searching for implied rules:

- A distinct questline with a full act graph, visible gates, reconnection points, failure responses, and endings.
- A complete cast with histories, specific skills, limits, relationships, and state-aware voices.
- Military, Rebel, Independent, and no-commitment variations that alter scenes and practical outcomes.
- Supporting-current playbooks that name service, cost, access, refusal, and boundary without creating new major factions.
- Multiple playstyles defined by observable actions and trade-offs, with hybrids supported.
- Persistent consequences with current owner and restore expectations, or an explicit unresolved decision where the owner is not authorized.
- Complete dialogue and environmental content sufficient to make the branching readable in play.
- A collision audit against current data and expansion plans, including the user-owned Wave 19 plans.
- An implementation classification that distinguishes content-only, Core, host, data, and cross-system work.
- At least 120,000 words measured on the saved plan file; expansion above the minimum is acceptable when it adds useful content. No repeated filler, duplicated dialogue, or section-padding counts as substantive completion.

# Installment 3 — Branch-state atlas, complete side routes, and delivery sequence

## 38. Branch-state atlas: what the story remembers

The branching only works if later scenes distinguish relevant past actions without flattening them into a moral score. The state model below describes story facts, not a proposal for a new global reputation system. Each fact has a narrow narrative owner: the shift-board arc. It must be represented through whichever current quest or campaign persistence authority is confirmed during implementation discovery. These labels are provisional design language; they are not claims that fields already exist or permission to add a parallel save store.

### 38.1 Evidence categories

For every consequential player action, authors must answer four questions. What physically happened? Who witnessed it? What did the actor intend, if the game can know that from an explicit player action? What did the affected people learn? These are separate facts. The player might change a board but fail to tell people who relied on it. A courier might deliver a correction to one person but not the shelf copy. A worker might accept one shift and later explain that acceptance was not consent to repeat it. Recording only “completed task” loses the distinction that makes a later branch meaningful.

Use a compact event description during design:

- **Observed action:** a registered player verb, such as inspect, repair, post, remove, copy, deliver, ask, wait, delegate, or disclose.
- **Affected object:** the board, receipt, lamp, tool, message, shelf, or other concrete thing that changed.
- **Known audience:** characters who were present or received the resulting record. Do not infer a witness from proximity unless a scene explicitly establishes it.
- **Response:** accepted, refused, delayed, partially fulfilled, misunderstood, or unresolved.
- **Repair action:** a later act that changes the consequence without erasing the earlier event. Corrections coexist with the record they correct.
- **Current practice:** the method the shelter now uses for the recurring need. Practice is revisable, not a personality label.

A human-readable example is: “The player copied the board, Osha received the duplicate, Rade did not, one name was already stale, and the player returned the duplicate next shift.” Later implementation mapping must use the project’s existing identifiers, validation, serialization, and deterministic ordering conventions. This plan does not prescribe JSON keys before the catalog and schema owners are consulted.

### 38.2 Candidate story facts and their readers

| Story fact | Set by | Read by | Revision rule | Design reason |
|---|---|---|---|---|
| A schedule line changed | Direct edit, authorized edit, or assisted edit | Assigned person, clerk, route scenes | Current line may change; history remains | Separates authority from access |
| Notice reached a named person | Direct delivery, witnessed announcement, confirmed relay | Scenes dependent on what that person could know | New notice supersedes; it cannot make late notice timely | Makes communication routes matter |
| A duplicate exists | Copy action with source and recipient | Archivist, clerk, courier, correction scene | Duplicate can be corrected or destroyed with witness | Avoids a magically current copy |
| A tool was returned | Return interaction | Worker tracking the tool | Later return closes the open loan | Gives reciprocity material consequences |
| A shift was accepted | Explicit acceptance from assigned character | Coverage scenes and worker dialogue | Future shifts can be renegotiated; old consent is not rewritten | Makes consent time-bound |
| A substitute covered a shift | Explicit reassignment with notice result | Reliability and fairness branches | Another substitute can be found | Avoids inferring cover from a task flag |
| Player disclosed a limitation | Direct conversation about capacity | Delegation and boundary scenes | New constraints can be disclosed; earlier knowledge remains unchanged | Makes boundary setting playable |
| A missed task was acknowledged | Direct admission or witnessed correction | Follow-up and ending scenes | Task can be repaired; admission remains | Distinguishes repair from denial |
| Contribution was credited | Player or NPC posts or reads an attribution | Credit and dispute scenes | Credit can be corrected with explanation | Supports shared authorship |
| Local practice was adopted | Group decision with an implementable procedure | Final act and epilogue | Practice can be amended, suspended, or abandoned | Makes endings describe living routines |

Facts with no later reader should not be persisted merely because they appear in this table. Conversely, a fact used after loading needs an explicit restore source. Time-sensitive notice must use the campaign’s authoritative day/time value only after integration confirms the correct projection. The separate audited defect where a personal quest resolves with day 1 is not a ready-made clock API.

### 38.3 Avoiding score substitution

No branch should say “if the player has enough trust” when the actual requirement is that the affected person received notice. No ending should say “high independent reputation” when the visible act is that the player declined a standing agreement and still returned the borrowed tool. If an existing numerical value later supports pacing, its semantics must be documented and it must not replace an inspectable event the story needs to acknowledge.

For each scene, authors should answer:

1. What did the player do? Name the action that can accurately be quoted back.
2. What did each person have reason to believe at the time?
3. What is the current shared arrangement, including its expiry, owner, and review point?

Two runs may reach the same practical arrangement through different histories; dialogue can remember those histories. The same action can also have different consequences depending on its audience. Preserve causal history that matters, not every incidental click.

## 39. Main questline graph: route gates and rejoin contracts

Each act has a shared question and multiple answers through play. Major-faction alignment changes who can help, the pressure applied, and which public language is available; it does not decide the action branch. Routes rejoin when they encounter a comparable practical problem, carrying different causal context forward.

### 39.1 Graph terms

- **Entry:** necessary condition for a scene to appear. More than one action can satisfy it.
- **Branch:** an earlier action materially changes information, cost, relationship, or response.
- **Rejoin:** campaign returns to a shared act while preserving route-specific context.
- **Exit:** a valid path forward or a deliberate unresolved state, not a hidden dead end.
- **Readback:** later moment when the story shows what an action caused through dialogue, object state, service, or changed procedure.

### 39.2 Act I — The empty square

**Entry:** the public shift board has missing names and times. It cannot safely guide the day. Multiple people hold conflicting expectations. The player is asked to establish what is known before assigning work.

The player can inspect the board and compare a receipt, learning which lines conflict without telling anyone. They can ask people waiting what they were told, learning lived expectations but not necessarily where the error originated. They can stabilize an immediate need first, such as moving a lamp or tool that blocks access, reducing an obstruction while leaving the disputed schedule visible. They can announce a pause until records are compared, protecting against another mistaken assignment while consuming time. Or they can delegate comparison to Rade, Osha, or Mina. The selected person’s skill changes the report; delegation does not count as player observation unless that report returns.

The next scene names the evidence source correctly. An inspection route sees physical overwriting. A conversation route sees incompatible expectations. A stabilization route sees the newly accessible space. A pause route sees whether the queue dispersed. A delegated route receives a fallible account whose source is named. All paths identify at least one affected person and deliver a provisional description of the mismatch. None invents a definitive culprit. The player can proceed with uncertainty, which remains visible in the next act.

### 39.3 Act II — The first receipt

The group has one readable record, but cannot agree whether it is current. Rade requests a second source before posting a replacement. The player can ask permission to copy the receipt, preserving the source but spending time; make a marked working transcript, which is fast but can blur quotation and inference; deliver it to Osha to trace timing; ask Mina to verify whether the recorded maintenance work corresponds to a physical change; or hold it temporarily to prevent premature reposting, taking responsibility for its return.

Every route says who holds the original and whether copies circulate. A second account can be obtained, refused, or left outstanding. The player can continue on one source if the result is labeled provisional. The story does not turn the absence of a second witness into a moral test.

### 39.4 Act III — The copy

The dispute becomes “what does a copy permit its holder to do?” The player can provide a full copy with source and date; a narrow extract with sufficient context; a spoken readback without a portable record; refuse a copy and explain the risk; or let the recipient choose between spoken and marked formats. Each option states what the recipient actually received. A later scene checks usability. Privacy is not inherently honest, and broad circulation is not inherently fair; context governs the cost.

### 39.5 Act IV — The absence

An expected worker does not arrive. The record cannot establish refusal, failed notice, delay, or inability. The player can hold coverage open while someone checks the route, assign a substitute while recording the reason as unknown, ask a witness about delivery, send Osha if available and willing, publicly mark the assignment unconfirmed, or quietly erase it. Each choice has an operational consequence: delay, reassigned labor, partial witness knowledge, a new attempt rather than proof of old delivery, visible embarrassment, or a stale public expectation.

The act rejoins once the work has coverage or a named gap. The absent worker’s reason remains unknown until they provide it or reliable evidence arrives. Later dialogue acknowledges waiting, substituting, checking, marking, or erasing rather than a generic completion.

### 39.6 Act V — The spare part

The board’s physical condition makes revision difficult without losing history. Mina can repair it, but the group must choose how to preserve the prior version. Repair in place leaves old marks visible but crowds the board. Removing the sheet and attaching a dated copy improves legibility but consumes materials. Rebuilding with separate current and record areas requires parts and maintenance. Delegating the work while the player documents it tests whether the jobs can occur independently. Declining repair until a posting practice is chosen avoids embedding a rule into a fixture but prolongs inconvenience.

Every route restores a usable posting method and states its cost. No route repairs the board for free. The final act may preserve, replace, or abandon the method.

### 39.7 Act VI — The correction

A named person offers a correction, refuses one, disputes another account, or cannot be reached. The player can add a sourced correction beside the original; replace the current line while preserving a dated prior version; hear both accounts separately, then read back to both; hold a timed shared meeting; ask each person which record format they accept; or leave the account unresolved while posting only the coverage required today.

The rejoin produces a correction procedure or identifies its limits. A correction is a new event, not erasure. Retaining the prior version requires a reason and scope. Access must be considered; the archive is not automatically public.

### 39.8 Act VII — The table

The player can write a small rule and invite objections; let workers propose rules in turn and name maintainers; ask Rade to draft, Osha to test delivery, and Mina to test physical upkeep; make no standing rule; adopt a trial with an expiry; or defer a lasting choice until an absent stakeholder can participate while still making a temporary rule for the next shift.

All routes end with a usable next-shift procedure, explicit trial, or explicit absence of procedure. It should identify who posts, who can edit, how corrections are signaled, how a worker can decline, and when the arrangement is reviewed. If a question is unanswered, the ending says so.

### 39.9 Act VIII — The later hour

Time passes or a new shift starts. The player can inspect the record before speaking; ask the assigned worker whether it was workable; check a physical dependency before declaring success; compare copies and find a stale one; let the group assess the practice without leading; or skip review. The ending then names the working rule, its cost, unresolved disagreement, and who can revisit it. Faction language overlays this account but never replaces it. The ending should differ when the actual practice differs, even if faction affiliation is identical.

## 40. Side quest route sheets

These six quests are optional entry points into the central theme, not six mandatory chores. A run should normally see two or three, with others appearing when triggered naturally. Main-story summaries still explain the final choice if a side quest is skipped. Exact rewards and identifiers need later catalog validation; these descriptions are not ready-to-import JSON.

### 40.1 “The Name Beside the Hour”

**Need:** a person sees their name beside a shift they do not recall accepting. The player must learn what the mark means before it becomes a commitment.

**Entry variations:** the assigned person asks directly; Rade spots a mismatch; Osha reports no acknowledgment; or the player notices different handwriting. This changes who the character is comfortable speaking in front of.

The player can compare the line with the source receipt. A missing acceptance allows the player to mark the assignment unconfirmed, but even a mark does not prove understanding. They can ask privately whether the person wants to explain; the person can disclose a constraint, decline to discuss it, or say they accepted once but cannot repeat it. They can ask the schedulers to explain their process, whose accounts may differ without either lying. They can offer replacement coverage, a smaller task, a later time, or no assignment. The player then chooses a public correction, inspectable private note, or no correction yet.

Possible outcomes include a clean correction with new coverage; a bounded one-time acceptance with expiry; a declined assignment with an explicit gap; a private repair that leaves the public board stale until a follow-up; or an unresolved account where the player substitutes without demanding an explanation. At the next shift, the player can check whether record and arrangement agree. Completion does not imply social settlement. The shelter adopts a no-assumption rule only if the player proposes it and the group accepts it.

### 40.2 “The Part That Almost Fits”

**Need:** an available maintenance part does not fit cleanly. Mina has found a risky workaround. The player weighs legibility, safe repair, and retaining history.

The player may inspect the mounting; ask Mina to demonstrate; search for a compatible part; make a temporary brace; ask a scavenger for uncertain stock; defer physical repair and use a separate sheet; or attempt the fit without Mina’s guidance. An unsupervised attempt can produce a visible fault, never an invisible random failure.

A compatible part permits a stable replacement and clear before/after record. A temporary brace works for a stated period and requires inspection. A salvaged part works only after the scavenger demonstrates limits; hiding provenance leads to a later wear or mismatch reveal. Deferral keeps records intact but leaves the board awkward, and Osha can offer a spoken update route. A rushed fit restores use today but creates a visible maintenance obligation. Record whether the part is temporary, who inspected it, the recheck time, what happens if the check is missed, and whether old records remain readable. A repair without a maintenance owner is incomplete. This quest can unlock an implementation of the current/record split, but never chooses the posting policy.

### 40.3 “A Copy for the Drawer”

**Need:** a survivor wants a personal schedule copy because they cannot reliably visit the board. Another worries that extra copies become stale and expose private assignments.

The player can make a dated copy; give a spoken summary; ask what information is needed; create a second shared posting point; refuse while offering another method; or let the requester make a copy under an update rule. The game does not presume why the survivor cannot visit. They may decline to explain; no diagnosis or private disclosure is required to receive access. The other survivor’s concern is also grounded: an old copy can lead to missed handoffs or reveal an absence.

Outcomes include a dated copy useful through a stated time; a spoken summary that can be replaced if the recipient needs portability; a second posting point with a named checker; a narrow copy containing only the necessary shift and contact instructions; or a refusal with a usable alternative, or an explicit unresolved access need if none exists yet. A later act checks whether the copy was refreshed. Completion alone never certifies it as current.

### 40.4 “The Message That Arrived After”

**Need:** a correction reaches Osha after someone has already acted on the earlier schedule. The player must handle the gap.

The player can identify arrival time, contact the affected person, log the correction, find coverage, tell those who saw the first version, or wait for scheduled review. Order matters. Recording arrival time first improves sequence evidence but delays direct contact. Contacting first speeds coordination but a missing timestamp may complicate a later dispute. Reassigning without explanation repairs coverage while leaving the affected worker believing the original assignment stands. Public disclosure may correct more expectations but reveal too much; scope should be agreed. Preserving both messages keeps the chain reviewable but may expose details until access is limited.

Outcome families are same-day coverage repaired, partially repaired, an explicit gap, uncertain receipt, or a temporary pause while facts are confirmed. None is a trust score. Affected people can remain angry after a responsible action, and their help remains voluntary.

### 40.5 “Swap the Hour”

**Need:** two workers want to exchange hours but disagree about who should update the shared record.

The player may ask separately, confirm together, relay one person’s proposal, ask Rade to edit, edit while both are present, or wait until both inspect the change. A relay remains second-hand until confirmed. Either worker can withdraw consent before the swap takes effect. It can be reciprocal and balanced, reciprocal but burdensome, one-sided with explicit compensation, or temporary emergency cover. Compensation might be later relief, a returned tool, a meal, or none. Each person must separately accept; silence is not acceptance.

Follow-up checks whether posted and verbal arrangements match, each person knew the effective time, a maintenance task moved, both could decline without losing future access, and the swap was one-time or recurring. A successful swap with no duration seeds a later problem rather than closing the issue. It teaches a trial rule only if the group explicitly adopts it.

### 40.6 “The Empty Shelf”

**Need:** paper or marker supply is gone. There is enough to create one immediate record, not every desired copy.

The player may spend the sheet on the current schedule; preserve it for the archive; request controlled stock from Archivists; trade with a supporting current; write on a reusable surface; send oral notice with named follow-up; or wait. A current schedule prevents immediate confusion but leaves no archive copy. An archive preserves history but needs a separate way to announce today’s work. Archivist supply can require dating and return of superseded copies, with labor cost and expiry. Trade may use goods needed elsewhere. A reusable surface can erase history unless someone makes a dated record. Oral notice is fast but needs durable readback. End states include a usable current schedule with limited archive capacity; retained archive and provisional oral schedule; bounded supply; shortage with named recheck; or no copies until supply returns. All feed different Act VIII readbacks.

### 40.7 Trigger and crosslink map

| Quest | Natural trigger | Main acts informed | What it never decides |
|---|---|---|---|
| The Name Beside the Hour | Disputed assignment | I, IV, VI, VII | The absent worker’s later account |
| The Part That Almost Fits | Repair proposed | V, VIII | Posting policy |
| A Copy for the Drawer | Alternate access requested | III, VII, VIII | Whether every copy is current |
| The Message That Arrived After | Delayed correction | II, IV, VI | Whether original notice arrived |
| Swap the Hour | Workers propose exchange | IV, VII, VIII | Recurring consent |
| The Empty Shelf | Materials insufficient | II, III, V, VIII | Future supply |

## 41. Action matrix: making consequences genuinely distinct

A branch is substantial when it changes who knows what, who performs work, resource cost, what record survives, what the group can claim, or which practical ending is available. A different dialogue sentence alone is not an expanded route, though dialogue can reinforce the difference.

| Action pattern | Immediate benefit | Immediate cost | Persistent evidence | Later readback |
|---|---|---|---|---|
| Inspect before changing | Fewer unsupported assumptions | Time or tool access | Observation and source | Receipt comparison |
| Change now, document later | Covers urgent need | Others may use old copy | Change time, later note | Delayed correction |
| Document before change | Preserves earlier state | Slower, scarce supplies | Dated old/current relation | Archive review |
| Ask affected person | Direct knowledge and consent | Person may decline or need time | Asked, answered, or boundary honored | Correction scene |
| Ask an intermediary | Reaches an absent person | Relay can lose context | Source, time, confirmation | Delivery callback |
| Assign substitute | Covers work promptly | Transfers labor or displaces task | Reason and duration | Next-shift check |
| Hold slot open | Avoids premature blame | Leaves a real work gap | Window held and cost | Coverage discussion |
| Post provisional claim | Lets work continue | May be mistaken as final | Label and expiry | Review scene |
| Delegate inspection | Uses another person’s skill | Report may be partial or delayed | Scope and returned report | Maintenance callback |
| Delegate decision | Shares authority | Player may miss process details | Delegated authority and acceptance | Meeting outcome |
| Admit missed follow-up | Restores truthful timeline | Costs face; may not repair reliance | Admission and repair status | Act VIII |
| Repair privately | Reduces public pressure | Others may rely on stale info | Private audience | Later public correction |
| Repair publicly | Corrects shared belief | Can expose excess detail | Public scope | Ending review |

This matrix catches fake branching: multiple options that set the same completion bit, give the same reward, and lead to the same later scene. Where routes converge for authoring or implementation reasons, preserve history in a visible readback. If even readback cannot differ, present the selection as dialogue tone, not a branching decision.

## 42. Ending cross-product: practice and affiliation

Four major route overlays do not constitute the ending tree. Endings combine practical board state, action history, supporting-current relationship, and major faction if present. A shared central event can carry state-specific dialogue, props, services, and callbacks; a unique cinematic per combination is not required.

### 42.1 Operational outcome axis

1. **Current and reviewable:** schedule clear, prior version retained within agreed scope, correction duties named.
2. **Current but fragile:** works today but depends on one person or an unrenewed supply arrangement.
3. **Local and revisable:** workers update through direct agreement, with little central process and explicit review each time.
4. **Delegated with boundaries:** a named maintainer or supporting current has a visible scope and review date; shelter can correct the record.
5. **Temporary emergency practice:** urgent need covered; lasting decision deferred to a named follow-up.
6. **Unresolved but legible:** no group rule, but coverage, disputed facts, and open decision are accurately marked.
7. **Discontinued board practice:** another coordination method replaces the board because it is inaccessible, unreliable, or too costly. It still needs an owner and failure path.

These are not a quality ladder. A detailed archive may be wrong when privacy is dangerous. Local revision can work well when people communicate directly. An unresolved but legible state can be more truthful than forced consensus. The ending names what became possible and what remains unsolved.

### 42.2 Action-history axis

The ending can reflect verified facts before commitment; urgent cover followed by documentation; listening before changing the record; use of intermediaries with delivery follow-up; delegated work with checked scope; admitted error with attempted repair; privacy-limited record access; or a decision left to the group. These patterns combine. They are not moral categories. A general pattern is named only after multiple supporting actions; one scene cannot establish an identity.

### 42.3 Supporting-current axis

At most one supporting current gets the foreground service moment; other participants may appear as callbacks. The foreground current is selected by service actually requested and accepted, not hidden preference. Archivists may leave a dated restricted copy and retrieval date. Long Walk may leave a time-bound route report that cannot certify local agreement. Scavenger Guild may leave a repair or stock with quality and return terms. If none was engaged, the shelter’s maintenance burden stays visible.

### 42.4 Major-faction overlays

Military, Rebel, and Independent variants change pressure and language without rewriting the practical state to flatter the faction. A faction can describe a delegated schedule as orderly, collective, or contractual; the shelter still knows who updates it and when it expires. If a faction claims credit for survivor labor, the narrative can show that appropriation rather than silently endorsing it. No-commitment emphasizes local control but shows outside dependencies: a needed guild part remains a relationship with terms.

### 42.5 Four distinct ending examples

**Military, fragile current board:** Mina checks the mount and Rade posts the list. A military supply promise expires after the next delivery. The ending recalls that the player chose rapid cover during the absence, then posted a correction. It asks who maintains the board if supplies do not arrive.

**Rebel, local and revisable:** workers agree to direct edits, and Osha reads updates at a second location. The Rebel representative supports local control, while the epilogue notes that the practice depends on two people being present. Delegating the final decision allowed workers to choose their own review time.

**Independent, private record:** the player accepts a bounded paper supply and withholds personal details beyond the affected worker and clerk. The broker asks for a renewal price. The ending shows local control with a real renewal cost; it does not treat secrecy or trade as inherently virtuous.

**No major alignment, unresolved dispute:** the next shift is usable, but one worker disputes the historical correction and misses the meeting. The player kept old and new records beside each other and marked the dispute. This is an unresolved ending, not failure. A later callback can offer review without forcing agreement.

## 43. Future implementation delivery map

This is sequencing guidance, not a claim on owned paths. Before implementation, the foreman must compare the proposal with INTEGRATION_PLANS.md and WORKTREE_OWNERSHIP.md, assign packages, identify shared seams, and record required architecture decisions. Plan 97 does not override the current queue or stop-on-missing-authority rule.

### Package A — premise and collision refresh

Confirm that the premise remains current and proposed names, quest beats, and supporting roles do not collide with authored content. Inspect quest, faction, character, radio, and flag catalogs; Wave 19 plans 92–96; any current Wave 20 content; and current validators. Treat user WIP as read-only unless its owner authorizes a change. Exit only with accepted, revised, and rejected beats tied to exact source paths. A negative text search alone cannot prove absence of collision.

### Package B — state and ownership contract

Map proposed persistent facts to an existing owner, restore path, and host projection, or explain why a fact needs no save. Inspect quest APIs, campaign state, save registration, host events, time projection, deterministic ordering, and journal/chronicle state. Mark each candidate fact as existing, safely derivable, requiring an approved extension, or unnecessary. No fact that must survive load may remain panel-local or in an unsaved cache. If a new authority would be required, record the decision and stop until an owner approves it.

### Package C — content-only route prototype

Implement a small slice through existing quest and choice infrastructure, such as Act I plus “The Name Beside the Hour,” or Act IV plus “The Message That Arrived After.” The foreman selects based on active queue and ownership. Include a declined option, a delayed or failed action, a later repair, and a reconnection point. Exit requires catalog integrity, reachable states through current APIs, truthful host presentation, correct save behavior where required, valid dialogue references, and no invented UI dates or rewards. The package owner chooses focused verification under TEST_POLICY.md.

### Package D — reachability and regression review

Enumerate graph nodes and incoming edges; map every player action to state mutation; identify readers for persistent facts; review load boundaries and deterministic ordering; run focused tests and content validators; inspect runtime only if Godot route changes. Every advertised branch needs an observable result or must be relabeled dialogue-only. Each rejoin must receive enough context for the correct callback. Unsupported outcomes remain open findings, not generic completion.

### Package E — faction and service overlays

Locate current faction authority and route eligibility. Verify no moral band silently dominates action branching. Map each service to an existing merchant, data, encounter, or quest owner; verify refusal, expiry, and re-entry conventions. No new faction registry, service ledger, or reputation counter is introduced. If existing architecture cannot express bounded support without a new owner, request the architecture decision and stop.

### Package F — full arc and ending assembly

Scale the proven slice to eight acts, six optional quests, support-current services, and endings. For every graph node, identify prerequisite, state read, output, save boundary, dialogue context, and fallback. Confirm the central arc works without all side quests. Endings summarize actual practice, faction context, unresolved costs, and future review. No ending depends on an unannounced moral threshold, and every content line has a validated route.

### Package G — closeout and authority updates

Produce changed-path list, acceptance evidence, focused command results, limitations, deferred decisions, untouched user-owned paths, and save/determinism notes. Only the named integrator updates the proper ledger. The existence of this plan does not mean a work package has started, passed, or closed.

## 44. Package-specific risks and responses

### 44.1 Graph exceeds current narrative model

If current narrative data supports only a linear ladder and one fork, the proposed graph may exceed the authoring model. Do not bury branch logic in arbitrary host callbacks. First establish whether current data expresses necessary conditions and outputs. If not, report the smallest missing capability and await an authorized architecture decision. A reduced graph should preserve action dimensions rather than collapse into morality merely to fit.

### 44.2 Current faction gate masks action evidence

The audit found FactionBranchCoordinator currently gates by moral bands. Refresh that premise before altering the system. If a gate makes action paths unreachable, report the evidence and request a decision; do not add a second faction gate in quest JSON that conflicts with the current owner. The target distinction is overlay versus action outcome, subject to one coherent authority.

### 44.3 Personal quest issues remain independently scoped

The audit separately found active personal-quest reward callbacks apparently unbound in production and the choice panel passing day 1. These findings may affect which host seams are safe to reuse, but are not automatically in Plan 97 scope. Any dependency on a repair must be separately accepted under the active queue. Do not opportunistically absorb that repair or claim Plan 97 resolves it.

### 44.4 Character or resource collision

The provisional names passed only an initial exact-name search. A complete review may find similar roles, voices, histories, or locations without exact string collisions. Rename or reshape before authoring dependent text. Do not edit user-owned Wave 19 files to force a fit. Verify candidate item IDs before assigning paper, marker, fastener, lamp, or tool costs; if no owner exists, use an already-supported time/labor cost or request a data decision. Never deduct through a panel-local counter.

### 44.5 Inaccessible soft locks

A required fact available only from a character who can become unavailable creates a soft lock. Every critical fact needs an alternate source or a valid uncertain route forward. Uncertainty may affect the ending, but should not be disguised as “cannot continue.” A refusal or absence must produce an understandable state.

### 44.6 Long prose obscures the immediate choice

Expanded prose helps only if the player can still identify the practical problem. Each scene should state the need, show evidence, offer actions with visible stakes, and provide concise readback. Histories belong in optional dialogue, journals, props, and callbacks. The player should not need to read plan-length material to understand a single choice.

## 45. Full-run branch probes

These examples test whether a faction route supports multiple playstyles and whether different factions can converge without becoming identical. They are route probes, not a fixed set of implementation flags.

### 45.1 Military: rapid operator who repairs later

The player moves the blocked tool before comparing records. The queue advances, and Mina sees a practical response. Rade later gives a partial account; Osha provides delivery history. A military liaison requests coverage information before patrol. During the absence, the player assigns a substitute and marks the reason unknown. The substitute accepts one shift but asks for an end time. The player returns to disclose that the old board remained up, creating a real correction. At the table, the player offers military supplies with their expiry stated but lets the shelter choose the maintainer.

The ending has dependable short-term supplies and a locally revisable schedule. It remembers urgency and repair, not a generic careless/heroic identity. A callback lets the substitute set a boundary on future coverage.

### 45.2 Military: careful verifier with narrow disclosure

The player pauses posting to compare sources. This creates a coverage gap; the liaison calls the process inefficient. The player refuses to publish a private explanation but records the practical fact that the worker did not accept the shift. An accessible summary reaches a second location without private details. Mina inspects the mounting, and the player checks her finding. The player accepts time-limited material help and negotiates a local review date.

The ending has a slower emergency process but fewer unsupported assignments. Military remains a supplier and pressure, not owner of the household’s record policy.

### 45.3 Rebel: delegated organizer who misses follow-up

The player invites workers to propose edits and delegates the summary to Osha. A local rule emerges, but the player does not verify that the second board changed. At Act VIII, one copy remains stale. Osha names the missed follow-up without calling the player malicious. The player can retrieve it, mark it superseded, or accept that a worker used old information. The Rebel organizer argues that local control worked; another worker says shared control still needs upkeep.

The ending shows cooperation with an uneven relay. Repair records both failure and repair. Without repair, the next shift starts with a clear limitation. This is neither automatically successful nor reduced to goodness.

### 45.4 Independent: bounded reciprocal trader

The player trades surplus material for dated posting supplies and states that the deal ends after two deliveries. They ask what the broker wants and disclose that only one current board can be maintained. A narrow record copy is allowed; private details are withheld. When the broker requests early renewal, the player can negotiate, refuse, or accept reduced supply. Osha validates the delivery route.

The ending shows an efficient but transactional relationship with a real renewal cost. Callbacks depend on whether quantity and dates were honored, not an abstract Independent score.

### 45.5 No alignment: conflict avoider and open dispute

The player marks the assignment uncertain and avoids convening a meeting. A voluntary substitute covers the shift. At Act VI, the original worker returns but refuses public correction. The player makes a private note and leaves the old mark until a safe update is agreed. Another resident says the board cannot be trusted. The player can make a provisional public note without private reasons, or retain only practical coverage.

The ending preserves local control but leaves an incomplete public record. No one is assigned through the unsupported line. An unresolved outcome remains playable and legible.

### 45.6 Mixed supporting-current service

The player asks Archivists to preserve one dated copy, asks the Scavenger Guild to repair the mounting, but declines a continuing contract. The Long Walk offers an external timing report; the player accepts it as route evidence, not proof of local consent. No single support faction owns the process. The archive remains under its retention terms, the Guild has no ongoing claim, and local workers maintain the current board. Each relationship has its own cost and boundary. Declining the Long Walk report would not invalidate local agreement.

## 46. Dialogue and callback truth table

Callbacks prove that actions mattered. They must distinguish observed fact from interpretation and respect what each speaker could know.

| Recorded evidence | A character may say | Must not claim |
|---|---|---|
| Player changed line before contacting assigned worker | “I saw the schedule change before anyone asked me.” | “You never asked me,” unless no later request occurred |
| Osha delivered a message but received no acknowledgment | “I carried it to the east room; I did not see them read it.” | “They knew about the change.” |
| Player copied board and labeled source | “This copy says where it came from.” | “Every copy is current.” |
| Substitute assigned; reason remains unknown | “There was a gap, and you covered one shift.” | “They refused to help.” |
| Private question and refusal to explain respected | “You left the reason private.” | Any guessed motive or diagnosis |
| Player admitted missed follow-up | “You told us you didn’t return the copy.” | “The mistake is fixed,” before correction |
| Mina inspected temporary mount | “It held when I checked it.” | “It will last indefinitely.” |
| Player declined standing faction agreement | “You kept the deal to the delivery we made.” | “You will never work with us again.” |
| Player delegated meeting and skipped review | “The group chose a rule; no one checked the second board.” | “Everyone agreed and used it.” |
| Final dispute remains open | “We have not settled that account.” | “The truth is unknown forever.” |

### 46.1 Speaker knowledge belongs to the branch

The same line can be appropriate from one person and false from another. Rade can identify a version he copied. Osha can describe a delivery attempt. Mina can describe an inspection. A worker can describe their own intention and boundary. None automatically knows the whole chain unless it was shared. Faction representatives may receive reports, but those reports can be partial, late, or interested.

### 46.2 Emotional variation without false exposition

Emotional response can differ with facts held constant. Someone may be relieved that the shift is covered and angry it was assigned without asking. They may accept an apology but decline future work. They may value an archive but dislike who can read it. These reactions add depth without contradictory facts.

### 46.3 Quiet callbacks

Not every branch needs a speech. A corrected date in different handwriting, returned tool beside a superseded copy, relocated lamp, or named review time can show a retained lesson. Environmental callbacks require exact state and must not appear where their cause never occurred.

## 47. Release criteria for the completed plan

Before declaring the 120,000-word minimum complete, the document must meet these content-plan criteria. They do not claim implementation has passed.

- Measure the saved file with one documented word-count method; report the exact count rather than a rounded estimate.
- Give each major questline multiple action routes with distinct costs, information, relationships, or practical outcomes.
- Support at least four action dimensions combining without one morality axis deciding availability.
- Let major factions alter pressure, resource access, language, or risk while preserving action history.
- Give each supporting current a bounded service, cost, consent/refusal path, expiry, and no authority over the central campaign.
- Allow side quests to be skipped without making the main story incomprehensible.
- Make each ending identify current practice, unresolved cost, information limits, outside dependency, and who may review it.
- Show failure consequences and understandable next actions where play continues.
- Give persistent facts readers and restore or non-persistence rationale; do not promote panel or host cache to authority.
- Separate implementation claims from creative recommendations and ground claims in current source/data evidence.
- Leave existing user work, especially Wave 19 plans, unchanged unless its owner authorizes inclusion.
- Mark provisional names, data shapes, state labels, rewards, and route conditions unapproved until the relevant authority reviews them.

## 48. Open authoring questions

These remain open because the plan is being expanded in installments and some answers require full content review. The list keeps creative and technical decisions visible rather than silently treating drafts as canon.

1. Does the shelter location have a canon name, layout, or board owner that should replace the provisional setup?
2. Do Rade Seln, Osha Marin, and Mina Ver collide with existing names, roles, voices, or histories after full review?
3. Which current quest data types express multi-condition branches, reconnection context, and delayed callbacks?
4. Can existing quest/save owners persist a source-aware event chain, or should the first implementation reduce it to fewer proven facts?
5. Which campaign clock is authoritative for quest timestamps and deadlines?
6. Can visible route requirements explain enabling actions without exposing internal field names?
7. Which accessibility options are already supported by current UI and interaction patterns?
8. Which existing item IDs represent paper, markers, fasteners, lamps, or tools, if any?
9. Can each supporting current provide the bounded services described through existing owners?
10. Does the current major-faction arc have room for this local issue without displacing a more important beat?
11. What unresolved outcome should occur when the central arc ends but an optional dispute remains?
12. What branch volume can runtime and content review support comfortably?
13. Which production owner reviews reachability and which narrative owner approves final dialogue?
14. Which package should own unrelated reward callback, clock, or save defects found during implementation?
15. Which count command and text normalization should be used for the final measurement if formatting changes?

## Installment 3 closing note

This installment expands the branch logic into a route atlas, defines source-aware state, makes all six side quests playable through distinct actions, tests endings against faction and practice combinations, and sequences future implementation behind ownership checks. It remains creative planning only. It does not claim the proposal is canon or authorize a change to the active integration queue.
# Installment 4 — Scene drafts, route performances, and diegetic texture

## 49. Scene draft — “The board in the rain”

**Placement:** Act I, with optional callbacks in Acts V and VIII. The scene gives the player a tactile start and lets them choose which evidence to trust before a faction route is foregrounded.

Rain enters through one thin line in the patched roof. It runs down the board and pools under the lower nail. One corner of the schedule has lifted. The ink remains readable, but the paper has softened. A tin cup sits below the leak. Someone wrote “after the second knock” in the margin. No one knows whether that means a time, a reminder, or a message to one person.

Rade holds down the loose corner. “If I pull it now, the bottom goes with it.”

Mina tests a short strip of metal against the frame. “If you leave it, the next sheet goes the same way.”

Osha stands inside the doorway, damp cuffs dark against her sleeves. “Someone asked me which version to carry. I said I’d bring them an answer.”

The player can hold the paper flat while Rade copies it; move the cup and ask Mina to inspect the frame; compare the wet line with the receipt in Rade’s sleeve; or ask Osha who is waiting. These choices do not set a hidden “careful” value. They establish which evidence enters the story first.

**Preserve the sheet:** Rade copies the readable part, but the lower edge tears when the player lifts it. “That line is still there,” he says, “but I can’t promise the paper will keep it.” Later, an archive scene can show the torn original beside its copy.

**Move the cup and inspect the frame:** Mina finds a split behind the old nail. “It’ll hold if we stop feeding water down this seam. Not forever. Long enough to choose what we’re saving.” This opens a temporary brace route but consumes no repair part until the player explicitly takes one.

**Compare the receipt:** the top lines agree and one lower assignment conflicts. Rade says, “That proves what I copied. It doesn’t prove who read it.” The player gains a source comparison, not a complete answer.

**Ask Osha:** she identifies one person who needs the answer but does not disclose why. “They asked me to bring the time, not the whole board.” The player can authorize a narrow readback, ask Osha to return for the person’s preferred format, or decline to send an answer until the record is clearer.

The routes rejoin at the question of what to preserve before the paper fails. A faction representative can enter after the first action and change the pressure. The military liaison asks which hours can be covered. A Rebel organizer asks who wrote the note. An Independent broker asks whether the board needs material. None can resolve what “second knock” means.

## 50. Scene draft — “The hour that moved”

**Placement:** Act IV, after a worker misses the shift shown on the board. The scene distinguishes absence, refusal, miscommunication, and delay without forcing the story to know which explanation is true.

The workroom door is open. A lamp burns over the empty bench. The tool that should have been used is on the floor rather than its hook. The clock has moved far enough that the first task is no longer on time. Rade says the name was on the board. Osha says she did not carry a confirmation. Mina says the lamp has been on since before the shift began.

The player can keep the task open for one more interval, ask for a substitute, inspect the board copy the worker was meant to see, send a fresh message, or mark the absence unresolved and record the gap. The game states the cost: another task may slip, a substitute may surrender their own planned work, or the task may remain undone.

**Keep the interval open:** Rade starts to write “late.” The player can allow this, ask him to write “not confirmed,” or leave the line untouched. If untouched, residents may read the original assignment as active. If “late” is posted, the absent worker may object to judgment without contact.

**Ask for a substitute:** Mina may volunteer if her maintenance work is covered or explicitly deferred. If asked without discussing the lamp, she points at it: “You can have me there or here. You don’t get both.” The player can ask another person, split the task, or accept a maintenance delay.

**Send a fresh message:** Osha can deliver a new request if available and willing. A message that arrives now says nothing about whether the old notice arrived. The player chooses whether it asks for an explanation, a time estimate, or only says the shelter reassigned the work.

**Mark the record unresolved:** the line remains visible but is marked unconfirmed, not erased. This protects the worker from a false claim and leaves the schedule less tidy. A support faction can help with the practical gap but cannot reveal the worker’s intention.

When the worker returns, the opening changes with the player’s action. After waiting: “You left the place open.” After a substitute was asked: “Who did you move to cover me?” After a new message: “I got the second one.” After the board was marked: “At least you didn’t write why I wasn’t here.” These lines open conversation; they do not grant automatic forgiveness or rewards. The worker can explain, decline, or correct only the practical arrangement.

## 51. Scene draft — “Two copies, one correction”

**Placement:** Act VI. The player decides how to handle disagreement after duplicate records diverge.

The first copy says the east room takes the early watch. The second has that line crossed out and another name beside it. Its date is newer. The person who made it says they changed the line after speaking with the first worker. The first worker says they never agreed to the change. Both copies are legible. Neither alone settles what happened.

The player can place them side by side and ask both people what they remember; speak separately with each person; mark the newer copy current while noting the dispute; suspend the assignment and seek other coverage; or ask a supporting current to retain both under a narrow agreement. A faction representative may watch, but their presence does not make a record more authoritative.

In a shared conversation, one worker says, “I said I could do the first hour. That is not the same as taking the whole watch.” The other answers, “You said yes while we were moving the lamp.” The player can ask what each person thought the request meant. The scene does not decide that either person lied.

**Keep both versions:** Rade adds a source and notes that the account is disputed. This is reviewable and untidy. If the player previously limited access, the disputed detail can be placed in a restricted envelope while current coverage is posted separately.

**Speak separately:** the player may learn each person’s understanding, then ask permission to share a summary, share only the new coverage plan, or keep the accounts separate. If there is no readback, neither person later speaks as if they heard the other’s account.

**Mark a provisional current copy:** the player chooses an operational assignment based on direct availability, not on who is morally right. The disputed line remains marked. The next scene names the person who accepted the provisional duty. This can lead to a fully valid ending with unresolved disagreement.

**Suspend coverage:** the task may remain undone or be reassigned. The player sees the cost before confirming. A representative who demands a tidy roster can object; the narrative shows that pressure without declaring the player correct.

## 52. Supporting-current encounter scripts

Supporting currents widen available actions and add terms. They do not become new main factions, hidden bosses, or ending owners. Each encounter offers a specific service, a refusal path, a practical cost, and a boundary that can be remembered later.

### 52.1 Archivists — “A date is not a witness”

The Archivist unfolds a paper sleeve. Three fields are penciled on it: date received, source named, and review requested. There is no line for “truth.”

“We can keep a copy,” she says. “We can say who brought it and when. We can’t make the person who wrote it agree with the person named on it.”

The player can deposit the disputed copy, ask for a narrow excerpt, ask how to withdraw it later, or decline. The Archivist states the retention term before commitment. If asked to choose the true version, she refuses: “That would make our shelf sound like a witness.”

If accepted, a later callback returns the sleeve with its date and source, plus a note that the copy is disputed. It confers neither truth nor faction standing. If declined, the Archivist can explain how to mark a local copy; the main route continues.

### 52.2 Long Walk — “The road cannot say who agreed”

A Long Walk courier arrives with grit in one boot and sets down a route slip. It gives departure, blockage, and arrival windows. It does not say who was waiting at the destination.

“I can tell you when the message crossed the north cut,” he says. “I can’t tell you when somebody saw it. The room has its own clock.”

The player can trade for a timing report, ask the courier to carry a new notice, request return acknowledgment, or pass. Acknowledgment costs another route window and may fail if the recipient is absent. If accepted, transmission and delivery are separate recorded facts. If the player passes, Osha can attempt a local relay; Long Walk is never required.

Later, a faction may cite the route time as proof of notice. The player can point out the difference between arrival on the road and receipt by a person. This callback offers language for resisting overclaim; it does not decide the dispute.

### 52.3 Scavenger Guild — “It will hold until it doesn’t”

The Guild mechanic turns a bracket over, tests the bend with a thumb, and sets it beside Mina’s fastener. “This one fits if no one leans on the lower corner. If they do, it walks.”

The player can ask for a fitting, buy it loose, trade for a stronger part, request a demonstration, or decline. The stronger part costs a resource that may be needed elsewhere. A demonstration reveals its limit but leaves the shelter responsible for inspection. A fitted part is complete only when the repair owner and next check are recorded.

The Guild does not demand a monopoly. If the player wants to learn, the mechanic demonstrates and leaves. If hired, terms specify whether labor, part, or both are covered. Later wear is a foreseeable maintenance event, not a random betrayal.

## 53. Environmental and radio fragment bank

Fragments add voices and history without carrying mandatory branch facts that the player might miss. State-specific variants require validation before production.

### 53.1 Chalk note beside the board

**Base:** “If you move the lamp, leave the handle facing the wall. It catches sleeves.”

**After the player moved it and told Mina:** a second hand adds, “Handle’s turned. Good.”

**After moving it without notice:** “It’s not where I left it. Ask before you borrow it.”

If the player never moved the lamp, no callback appears. An empty space is not a substitute for a recorded fact.

### 53.2 Rade’s folded draft

The paper reads: “First write what changed. Then write what was there. Leave enough room to be wrong.” Underneath are three dates; one is crossed out and re-entered. If the player preserved an older copy, Rade keeps the draft near the board. Otherwise he folds it into his coat. This is character detail, not a required clue.

### 53.3 Osha’s route slip

“North door shut. West stair clear. One knock from inside. Waited. No answer. Returned at next light.”

The slip contains route observations without interpreting silence. If Osha carried a private message, a second slip lists the recipient’s requested reply window but not the content. If the player declined the route, the slip is absent.

### 53.4 Mina’s inspection mark

“Lower nail held on the 12th. Not checked since. Don’t hang a wet sheet there.”

After repair, Mina adds the date and initials. A temporary brace reads “temporary — check at next rotation.” This is a maintenance callback, not a claim of indefinite safety.

### 53.5 Shared room radio: late evening

“Shelter desk to anyone still copying the old board: the left column is current for the next shift only. The crossed line stays up because the correction is disputed. If that affects your work, speak to Rade before taking a tool.”

This broadcast appears only if the player requested a public update. With privacy-limited disclosure, it instead says: “The next shift list has changed. Check with the desk for your assignment. No private reason is posted.” The difference makes disclosure scope visible.

### 53.6 Radio after a failed delivery

“Osha made the west run. The recipient wasn’t there. The message is back at the desk, unopened. If you need it today, ask for a second route.”

The fragment distinguishes an attempt from received notice. A second route may require time or a substitute. If the player does not authorize it, the message stays at the desk rather than becoming silently delivered.

### 53.7 Notice on the tool shelf

“Returned: one brace, one chalk nub, two clean wraps. Missing: small wrench. No name recorded.”

If the player returns the wrench, the notice receives a date without a name unless the player chooses disclosure. If they request privacy, it says “one small wrench returned.” If it remains missing, the wording does not accuse anyone.

### 53.8 Environmental authoring rule

Ambient text has three tiers: a baseline object gives mood or ordinary use; a conditional variant acknowledges an action actually taken; a continuation reveals a cost or changed routine. Do not put the only explanation for an ending behind an optional radio, missable prop, or inaccessible room. Do not reuse one generic note to stand for multiple causes. Conditional placement and wording must follow confirmed state.

## 54. Faction scene overlay table

Each major route introduces a genuine resource, pressure, or competing interpretation, while the player still makes and owns the local action.

| Major route | Contribution | Pressure or risk | Player can refuse by | Action consequences remain governed by |
|---|---|---|---|---|
| Military | Coverage window, spare lamp, named outside contact | Timely roster and dependable staffing | Limit details, negotiate a narrow report, or decline resource | Notice, coverage, and current procedure |
| Rebel | Meeting space, messenger access, or dispute observer | Expectation to demonstrate collective control | Decline public meeting; use smaller witness group; record disagreement | Participation and accepted rule |
| Independent | Paper, marker, or repair trade with quantity and dates | Renewal terms or scarcity pricing | Decline, counteroffer, buy one delivery, or find another source | Goods exchanged, expiry, and labor cost |
| No commitment | Keep issue local and avoid external reporting | Less outside assistance and no default intermediary | Valid in itself; narrow service may still be requested | Local capacity and accepted service terms |

A faction representative may appear in one or two scenes per act. Their absence must not stop action routes. The central cast remains responsible for the shelter’s consequences. Major factions matter through supply, authority, social pressure, and political interpretation without converting a local schedule dispute into a recruitment quest.

## 55. Voice, staging, and restraint guide

This expansion uses ordinary objects under pressure: damp paper, a failing bracket, a missing tool, a half-lit room, a message returned unopened. The tone stays restrained. Characters do not explain the quest thesis. They speak from immediate problems and show values through disagreement, boundaries, and work.

Rade tends to qualify what he knows. Osha distinguishes what she carried from what someone received. Mina distinguishes what works now from what will last. These are tendencies, not rigid speech algorithms; fatigue, urgency, and relationships change cadence. Faction speakers use institutional language without becoming caricatures. Archivists discuss records as both care and burden. Long Walk members speak in route intervals and handoffs. Guild members discuss fit, wear, and repair.

Avoid having a character call another evil, pure, dishonest, or good as a shortcut to branch logic. If a character accuses someone of lying, the scene frames it as their claim and keeps evidence separate. The player can respond to its practical impact without declaring a person’s entire identity.

Player-facing options favor verbs and visible outcomes: “post the change,” “ask her directly,” “hold the slot open,” “send Osha back,” “mark this copy provisional,” “leave the reason private,” and “find a substitute.” They should not promise hidden results such as “gain trust.” Response text can name the risk plainly: “This leaves the bench uncovered until the next message arrives.”

## Installment 4 closing note

This installment supplies scene-level writing, bounded support services, state-aware radio and environmental fragments, and major-faction overlays that change pressure without claiming the player’s decisions. Names, props, text, and route hooks remain provisional pending current-content collision review and the project’s normal ownership and integration process.
# Installment 5 — Character arcs and playstyle route maps

## 56. Character arcs: the people affected by the board

The central quest is not only about a schedule. It is about what three people have learned to do when an arrangement fails. These arcs should emerge from their work, conversations, and boundaries. They are not loyalty meters. The player can affect whether a character feels heard in a particular event without making that character permanently approve of the player.

Each arc has a personal need, an action-sensitive sequence, and more than one ending. The character’s ending is about their own practice and relationship to the work, not an award the player wins. None is gated only by faction alignment or moral classification.

### 56.1 Rade Seln — a record can serve people and bind them

**Personal need:** Rade learned to keep written lists after a previous handoff failed. He is proud of a system that prevents forgotten work, but he sometimes treats a written line as more settled than the person who is named on it. His arc asks whether he can make records easier to correct without giving up the protection they provide.

**Opening beat — the copy that holds:** Rade presents the oldest legible receipt and asks the player to keep it intact. If the player copies it with a visible source mark, Rade appreciates the care. If the player changes the current schedule first, he is concerned that people may not know which copy to follow. If the player asks him to choose, he will choose a provisional answer and identify his uncertainty.

**Second beat — the name in the margin:** Rade finds a name assigned in handwriting he recognizes but did not make. He can identify who had the marker; he cannot establish that the named person accepted. The player can ask how he knows, seek the named worker directly, record the mark as disputed, or erase it. Each action changes who gets the first word in the later conversation.

**Third beat — his rule under pressure:** a faction representative requests an export of the schedule. Rade wants the shelter to appear reliable because assistance may follow. The player can ask what information is actually needed, give a narrow count of covered hours, disclose the full roster with consent, or refuse. Rade’s response depends on whether the player discusses the pressure with him or simply chooses. A refusal does not make him hostile; he may still worry that the shelter loses support.

**Fourth beat — the correction Rade made:** Rade privately admits he changed a line based on an informal conversation and did not return to confirm the new wording. The player can help him tell the affected worker, ask him to correct the board first, invite him to explain his assumption, or decline to take responsibility for his disclosure. This is his mistake to address. The player should not receive a dialogue option that hides the correction while still collecting an uncomplicated reward.

**Fifth beat — shared procedure:** Rade proposes a record format with three small spaces: current, changed, and review date. The player can endorse it, ask him to test it with the people who use it, suggest a narrower design, or decline a standing format. If adopted, he agrees to maintain it for a trial period rather than declaring it permanent.

**Possible arc endings:**

- **Record keeper with a correction habit:** Rade continues maintaining the board and invites affected people to check changes. The arrangement depends on his availability, so a later scene asks who covers the task.
- **Shared clerk work:** two residents rotate posting and each leaves a readback for the next person. Rade retains a smaller archive role and accepts that not every disagreement can be settled on the wall.
- **Private recorder:** Rade keeps a limited record for people who need a history, while the public board carries only current work. This protects detail but requires someone to update two forms.
- **Withdrawn from posting:** Rade stops being the default clerk after the group chooses another method. He may feel relief, loss, or both. The player’s action can make the transition respectful, but cannot make it painless.
- **No settled arc:** if the player never revisits his correction, Rade continues the current task while the underlying problem remains. An ending can name the unresolved assumption without treating the relationship as broken forever.

### 56.2 Osha Marin — delivery is not receipt

**Personal need:** Osha is often asked to carry messages because she knows the shelter routes and can move quickly. She wants the work to be recognized as skilled labor, not treated as an invisible extension of a schedule. She also needs the freedom to refuse a route or avoid carrying information that should not be hers to disclose.

**Opening beat — the request at the door:** a resident asks Osha to deliver an answer. The player can ask Osha whether the route is safe and manageable; the resident can give the message directly; or the player can choose another route. Osha does not need to disclose why one passage is difficult. If the player assigns the route without asking, the first callback concerns the assignment process, even if she completes it.

**Second beat — a returned message:** a sealed note comes back because the recipient was absent. The player can authorize a second attempt, leave it at a mutually agreed place, ask the sender to speak directly, or return it unopened. Osha states exactly what she did and did not observe. If the player falsely describes the message as delivered, she corrects the record in a later scene.

**Third beat — relay burden:** two people ask her to pass incompatible instructions. The player can ask each sender to clarify, hold both notes for a shared conversation, deliver each privately with their consent, or decline to make Osha responsible for resolving the conflict. Any route can continue; the limitation is about role and information, not a gate.

**Fourth beat — the route is hers:** Osha maps a safer route that takes longer and includes a designated return point. The player can fund the extra time through an existing cost, ask her to demonstrate the route, ask the Long Walk for a comparison, or decide the delivery is not worth the risk. Osha may propose a person to cover her other task. If the player ignores this displacement, the other task has a real consequence.

**Fifth beat — credit and records:** a faction representative cites Osha’s delivery work as evidence that the faction’s schedule succeeded. The player can correct the attribution, ask Osha how she wants the work described, let the claim stand, or limit the public report. Osha’s reaction is about credit and exposure, not a binary favor state.

**Possible arc endings:**

- **Named route worker:** Osha remains a courier for specific, accepted tasks, with route cost and confirmation status written down.
- **Shared relay:** the shelter rotates delivery and return acknowledgment, reducing dependence on one person but increasing coordination work.
- **Private messenger:** Osha carries only messages whose sender and recipient agree on the channel. Public schedule changes use another method.
- **Route adviser:** Osha stops carrying routine messages and maps routes or trains others. The work remains valuable though less visible.
- **No route agreement:** urgent messages still move, but the shelter has not established a stable method. The ending should describe the dependency and risk.

### 56.3 Mina Ver — maintenance includes the right to stop

**Personal need:** Mina is trusted to repair whatever is failing, and that trust has become a pressure to accept every job. Her arc asks whether the shelter can distinguish “Mina can fix it” from “Mina has agreed to fix it now.” She may decide to teach, specialize, rotate, or refuse repair work.

**Opening beat — the bracket test:** Mina shows that the board’s lower fastening is weak. The player can inspect with her, ask for a repair estimate, obtain a part, ask her to show someone else, or use a temporary workaround. She says what each method will and will not support.

**Second beat — competing repairs:** the lamp fails on the same day as the board. The player can prioritize one, seek help for the other, borrow a lamp, ask Mina what can safely wait, or postpone both. The choice affects actual visibility and scheduling. Mina’s response depends on whether the player names the work she will have to defer.

**Third beat — unsafe shortcut:** a supporting current offers a part that nearly fits. The player can decline it, ask for a demonstration, ask Mina to adapt it, or use it temporarily with an inspection deadline. Mina refuses any route that requires her to certify it as permanent when it is not. This refusal is an understandable constraint, not hostility.

**Fourth beat — a handoff:** Mina wants to teach one repair to another worker but worries that a rushed lesson will turn into an expectation that they replace her. The player can help define a one-time lesson, a rotating duty, a paired repair, or no transfer. The learner can also decline. A valid teaching branch records scope and whether the trainee accepts future work.

**Fifth beat — maintenance budget:** the group must choose which repair receives scarce materials. The player can prioritize safety, frequency of use, accessibility, or preserving records. Each is a defendable criterion with visible cost. Mina can advise but does not make the final choice automatically.

**Possible arc endings:**

- **Specialist with refusal rights:** Mina repairs selected work and can defer jobs without losing her role.
- **Teacher and checker:** she teaches a bounded repair and reviews it on agreed dates; the trainee is not silently made her replacement.
- **Rotating maintenance:** a small group takes turns, reducing single-person dependence while increasing coordination.
- **Protected repair window:** the shelter sets aside time for planned maintenance, delaying some urgent but noncritical work.
- **Independent practice:** Mina limits repair work and keeps her own tools. The shelter has less capacity but a clearer boundary.
- **Unresolved burden:** if requests continue to be assigned without negotiation, she may stop answering them. The narrative states the resulting capacity gap without punishing the player with an unrelated catastrophe.

### 56.4 Relationship edges are specific and revisable

A relationship callback should refer to an event and a current boundary. “You changed the board before asking me” can remain true even if the player later apologizes. “I’ll show you the brace again, but I won’t certify it as fixed” can be true after a respectful repair conversation. Avoid a single friendship number that overwrites these facts. If an existing relationship authority provides a relevant measurement, use it only according to its documented meaning and do not create a parallel relationship ledger for this quest.

## 57. Playstyle route maps across the eight acts

The following maps show how the same playstyle expresses itself through different verbs and trade-offs in each act. They help writers provide continuity without turning a style into a class. A player can switch at any act; the story records the actual choice. Hybrid play is expected.

### 57.1 Maintainer

- **Act I:** inspects the wet frame before moving the sheet; immediate task waits.
- **Act II:** checks whether the receipt is readable and asks for a protected copy.
- **Act III:** proposes separate current and archive areas, with extra construction cost.
- **Act IV:** checks the lamp and tool before assigning cover; the bench stays idle longer.
- **Act V:** selects a repair with an inspection date and names who can perform it.
- **Act VI:** asks whether the correction method physically fits the repaired board.
- **Act VII:** proposes a maintenance rotation or rejects a design that only Mina can upkeep.
- **Act VIII:** returns to inspect wear; a successful route can still reveal a new repair need.

This style is not “always fix things.” A maintainer can decide that a board is not worth repairing and choose a different coordination method.

### 57.2 Scheduler

- **Act I:** asks what work is due before investigating who wrote the mismatched line.
- **Act II:** creates a provisional coverage list while evidence is gathered.
- **Act III:** specifies who can update each copy and by when.
- **Act IV:** keeps the slot open for a bounded interval, then activates an explicit substitute.
- **Act V:** chooses a layout that makes current work visually distinct.
- **Act VI:** marks a disputed duty separately from the confirmed schedule.
- **Act VII:** proposes a trial rule with review at the next rotation.
- **Act VIII:** checks actual completion against the procedure and revises overloaded intervals.

The scheduler’s cost is that organizing the board can become more important than listening. Characters may ask whose labor is hidden by a tidy plan.

### 57.3 Courier

- **Act I:** asks who is waiting for a direct answer and which route reaches them.
- **Act II:** carries a receipt or asks Osha for route context.
- **Act III:** distinguishes verbal readback from a delivered physical copy.
- **Act IV:** sends a second message and keeps its attempt separate from confirmation.
- **Act V:** helps move replacement materials or relay a temporary notice.
- **Act VI:** gets permission before carrying a private account to another person.
- **Act VII:** proposes a return acknowledgment for schedule changes.
- **Act VIII:** checks who actually received the update and whether the route is repeatable.

The courier can be fast and still fail. A run with several deliveries but no recipient confirmation is a distinct state, not a successful communications score.

### 57.4 Reciprocal trader

- **Act I:** offers an exchange for temporary material or labor, naming what the shelter gives up.
- **Act II:** asks whether a copy can be exchanged for archive service.
- **Act III:** negotiates a narrow copy, date, or return condition.
- **Act IV:** offers compensation for substitute labor only after asking what the substitute wants.
- **Act V:** weighs a cheap part against a stronger one with a future maintenance cost.
- **Act VI:** negotiates disclosure scope with any outside observer.
- **Act VII:** proposes terms and an expiry, while allowing the shelter to decline.
- **Act VIII:** checks whether both sides met quantity, condition, and date.

Reciprocity is not the same as keeping an account of every kindness. The player may decide that a request is not an exchange and give help without a return term.

### 57.5 Reserve keeper

- **Act I:** stabilizes immediate access while preserving scarce supplies.
- **Act II:** saves paper for a dated copy rather than recopying every note.
- **Act III:** chooses which recipient needs a full copy and which can use a readback.
- **Act IV:** avoids spending a reserve until the gap is confirmed, accepting delay risk.
- **Act V:** uses a temporary brace and names its replacement date.
- **Act VI:** preserves one authoritative current copy and restricts duplicates.
- **Act VII:** proposes a minimum stock and trigger for replenishment.
- **Act VIII:** checks whether the reserve survived and who used it.

The reserve keeper’s drawback is concentration of risk: if the single stored copy is lost, the shelter may have no second source.

### 57.6 Improviser

- **Act I:** uses an available cup, cord, or spare surface to make the wet board readable.
- **Act II:** writes a clearly labeled transcript when copying supplies are absent.
- **Act III:** reads a schedule aloud and asks for a repeat-back.
- **Act IV:** splits the task into a smaller safe portion or changes order.
- **Act V:** uses a temporary repair with an explicit limit.
- **Act VI:** posts a provisional line and schedules the correction review.
- **Act VII:** offers a trial practice rather than a permanent rule.
- **Act VIII:** revisits the workaround and either formalizes it or removes it.

Improvisation can solve today’s problem while generating tomorrow’s hidden dependency. Every workaround should name what it requires to remain safe.

### 57.7 Direct operator

- **Act I:** changes the obviously dangerous or obstructive condition first.
- **Act II:** makes a practical copy and tells people where it came from.
- **Act III:** gives the needed information directly, then checks whether a durable record is still required.
- **Act IV:** assigns coverage quickly and records that the reason is unknown.
- **Act V:** installs or requests a repair before the next shift.
- **Act VI:** chooses a current assignment for operational reasons while preserving the dispute.
- **Act VII:** proposes a short procedure the group can begin using now.
- **Act VIII:** returns for corrections and accepts that some cost was created by speed.

Direct action is a genuine style, not a mistake to be corrected by a hidden honesty score. Its branches should reward useful intervention and show when speed transfers cost to someone else.

### 57.8 Delegator

- **Act I:** asks Rade, Osha, or Mina to investigate a specific question and confirms what they can report.
- **Act II:** lets a named character make the copy while preserving source context.
- **Act III:** assigns access decisions to the people receiving the copies.
- **Act IV:** asks for a volunteer substitute rather than assigning one without consent.
- **Act V:** divides physical repair and recordkeeping between people, checking dependencies.
- **Act VI:** asks a neutral witness to facilitate, while retaining a fallback if they decline.
- **Act VII:** lets the group write the rule and names a review date.
- **Act VIII:** checks whether delegated duties were accepted and fulfilled, without treating delegation as proof of completion.

Delegation can distribute agency or conceal work. The game distinguishes assigning a task, receiving an acceptance, and observing a result.

### 57.9 Boundary setter

- **Act I:** declines to name a culprit before evidence is available.
- **Act II:** limits copying to the information needed for the task.
- **Act III:** offers the recipient a choice of format instead of exposing the full schedule.
- **Act IV:** refuses to infer the absent worker’s motive and still arranges safe coverage.
- **Act V:** declines an unsafe repair while identifying the remaining hazard.
- **Act VI:** will not carry one person’s private account to another without permission.
- **Act VII:** supports a rule that lets workers decline shifts and limits who can see records.
- **Act VIII:** keeps an unresolved dispute open rather than demanding public disclosure.

Boundary-setting must not be coded as passivity. It contains active alternatives, explicit costs, and protection against unsupported claims.

## 58. Playstyle combination and transition rules

A player may start as a direct operator, become a reserve keeper after a shortage, and delegate the final meeting. The narrative should not force the player to maintain a consistent persona. Continuity comes from remembered actions, not from a style label that gates future verbs.

### 58.1 What a transition should do

When the player changes approach, a character can notice the practical shift: “You usually move the line first. Today you asked me before touching it.” This observation is an invitation to explain, not an accusation. If the player says resources ran short, the scene can acknowledge that reason only because the player chose it. A transition can be surprising and still valid.

### 58.2 Keep prior consequences without locking the future

If the player assigned a substitute without notice in Act IV, later choosing a participatory process does not erase that event. It may establish a better future rule. Likewise, one successful open meeting does not guarantee future consent. Later scenes read both the history and the current practice.

### 58.3 Hybrid routes that deserve explicit support

- **Maintainer + courier:** repairs a durable board and also checks who received changes; costs time on two kinds of follow-up.
- **Scheduler + boundary setter:** creates orderly assignments with a clear decline path; may leave fewer people available on short notice.
- **Trader + reserve keeper:** buys only what can be sustained, keeps emergency stock, and accepts that some service requests must be refused.
- **Improviser + delegator:** creates a temporary solution and teaches a named person to maintain it; must verify that teaching was accepted.
- **Direct operator + reciprocal trader:** acts quickly but later accounts for who contributed labor and what was exchanged.
- **Courier + privacy steward:** relays only the minimum information, increasing the need to confirm that recipients have enough context.
- **Observer + organizer:** gathers different accounts, then convenes a practical decision; risks delay and must establish an interim coverage rule.
- **Maintainer + independent route:** relies on local repair skills but accepts external parts under short, clear terms.
- **Reserve keeper + emergency operator:** spends a reserve only when a stated condition is reached and records what must be replenished.
- **No fixed style + changing needs:** chooses each act on its own terms. This remains a fully supported approach, not a weak or incomplete run.

### 58.4 Avoid personality rewards

Do not award a special ending merely because the player repeated one approach eight times. Repetition can be recognized when the history supports it, but the ending quality depends on actual work, consent, cost, upkeep, and review. Consistency may make the process predictable; flexibility may fit changing conditions. Neither should dominate by default.

## 59. Route balance review questions

For every act and side quest, reviewers should ask:

1. Can the player identify the immediate practical need without reading optional lore?
2. Does each option state its material or social risk in ordinary language?
3. Is a refusal valid, and if not, is the remaining consequence clear?
4. Can the player continue after an NPC is absent or unwilling?
5. Does the scene know only what its speakers could know?
6. Does each branch produce a distinct state, cost, relationship, or observable callback?
7. Does the story distinguish an attempted action from a completed one?
8. Does a later repair coexist with the record of the mistake?
9. Can the player change playstyle without losing access to core content?
10. Does a faction change the situation without dictating the outcome?
11. Is any support faction’s service bounded and rejectable?
12. Does the ending report actual practice and remaining dependencies?
13. Is the action evidence explainable to a player who did not inspect debug fields?
14. Are resource costs routed through an existing owner if implemented?
15. Would removing a moral score change nothing because no branch actually uses action evidence? If so, redesign the branches before implementation.

## Installment 5 closing note

This installment gives each core character a personal arc with multiple practical resolutions and maps nine play approaches across all eight acts. It treats style as the player’s observed method, supports transitions and hybrids, and preserves earlier consequences without locking future action. The routes remain proposals pending canon, ownership, and implementation review.
# Installment 6 — Four campaign routes across the full act spine

## 60. Military route — “Coverage Has a Name,” extended campaign treatment

The Military route should make a credible offer: it can bring equipment, predictability, and a contact who can respond when a shift fails. Its risk is that operational certainty can turn local people into entries on an outside roster. The route does not make military alignment the player’s personality. It gives the player a new relationship to negotiate while the shelter’s own actions remain decisive.

### 60.1 Military opening — request for coverage

A liaison asks for a reliable shift summary because an upcoming patrol intends to use the shelter’s workroom as a temporary stop. The player can share confirmed coverage totals without names; invite the liaison to wait while the shelter confirms the board; share named assignments after each affected worker consents; request equipment before providing any roster; or refuse the information request and accept the risk that support may be delayed.

The liaison has a legitimate operational need to know whether the room will be staffed and lit. They do not automatically need the reason for each worker’s absence or a full history of the dispute. A narrow report can meet the need. If the player shares names, each person’s consent and the disclosed scope are recorded. A refusal can still leave a useful working relationship if the player explains what information the shelter can provide.

### 60.2 Military act II — tested certainty

The liaison arrives with a roster format designed for patrol handoffs. Its columns are clear but leave no place for provisional status or correction history. The player can use the format for one shift; add a visible “unconfirmed” mark; translate the patrol’s needs into a local sheet; ask the liaison to accept a coverage count rather than names; or decline to copy a second roster at all.

If the player uses the format unchanged, it makes handoff easier but obscures the dispute. A later patrol may treat the recorded assignment as certain. If the player adds a provisional notation, the liaison can accept it while asking for a review time. If the player declines, the shelter retains one source of truth but the patrol must arrange its own light or staffing plan. None of these outcomes should award or remove an abstract honor value.

### 60.3 Military act III — an urgent vacancy

An equipment check has to happen before the patrol passes through. One resident is absent and another is already doing maintenance. The liaison offers to assign a patrol member to help. The player can accept the limited assistance with a clear end point; ask the patrol member to demonstrate the equipment check so a local worker can take over; send the patrol to another stop; or ask an available resident to cover and postpone maintenance.

The key branch concerns task ownership after the patrol leaves. A temporary helper can get the equipment working without transferring the ongoing duty. A demonstration can create local capacity if the learner accepts the lesson. Moving the patrol may cost the shelter timely access to supplies. Reassigning a resident can put the lamp or board at risk. The game names these costs before confirmation.

If the player accepts the patrol’s help, the helper leaves a checklist only if asked or if that is an agreed part of the service. The liaison does not quietly become the shelter’s permanent supervisor. If the player asks for instruction, the local learner may later decide whether to continue the task.

### 60.4 Military act IV — disputed report

A summary sent to the liaison lists a worker as “unavailable.” The worker says they never gave that explanation. The player can send a correction; ask the worker what wording they want shared; report only the practical coverage gap; show the worker the original report and let them decide whether to respond; or leave the report uncorrected.

This branch carries a public-information cost. Correcting a report can restore accuracy but may disclose that there was a dispute. A narrower correction can state “coverage reassigned; reason not confirmed.” The player can also tell the liaison who authored the wording, if that is both known and appropriate. The response acknowledges the original report as an event; it does not erase it.

If the liaison uses the inaccurate report to justify a later request, the player can cite the correction and its date. If no correction was sent, the next scene does not pretend the liaison knew the worker’s account.

### 60.5 Military act V — contract with an expiry

The liaison proposes a recurring supply exchange: reliable light and paper in return for a coverage summary each week. The player can accept a short trial; negotiate counts without names; set limits on how a summary may be reused; ask for a return copy of any report; refuse recurring terms and request one-time assistance; or decline all support.

A useful agreement names a duration, what counts as delivery, who can renegotiate, and how either side exits. If the player accepts a trial, the shelter tests the process and can discover that the reporting effort consumes more labor than expected. If the player asks for restrictions, the liaison may agree, counteroffer, or refuse. A declined term is not a betrayal. If the player walks away, the story keeps the local maintenance and supply gap visible.

### 60.6 Military act VI — closing assembly

At the final review, the liaison presents the patrol’s version of what the arrangement achieved. The shelter has its own evidence: which shifts were covered, which notices arrived, who maintained the light, and what the weekly report cost. The player can ask the liaison to present both accounts; speak first with the shelter; read out only agreed facts; decline a public review and handle renewal privately; or end the arrangement.

**Route resolutions:**

- A bounded partnership delivers supplies and reports with an expiry and local correction rights.
- A practical one-time service helps with one task but leaves no recurring roster obligation.
- A revised arrangement shares totals, not personal names, and adds a correction route.
- A terminated arrangement records what was received and what was returned, with remaining needs assigned locally.
- A fragile continuing arrangement remains available when the player accepts unclear renewal terms; later scenes can surface the cost without retroactively changing consent.

The Military ending scene should show operational capacity and the negotiation boundary at once. The faction remains important because its resources and authority matter. Its representative does not decide whether Rade, Osha, or Mina accepts the shelter’s local practice.

## 61. Rebel route — “Nobody Carries the Whole Board,” extended campaign treatment

The Rebel route should make a credible offer: shared control, rapid local coordination, and the ability to challenge outside claims. Its risk is that “everyone owns it” can conceal who is actually doing the work, and a public meeting can expose people who need privacy. The route should not assume that the most collective-looking action is automatically the most just.

### 61.1 Rebel opening — meeting or smaller conversation

A Rebel organizer offers a common room and observers for the schedule discussion. The player can accept a public meeting; ask for a small group of affected people first; request a written proposal with a response window; choose a private conversation with each worker; or decline the organizer’s help.

The meeting can create visibility and participation, but not universal consent. A person who misses it is not treated as having agreed. If the player chooses smaller conversations, a later readback is required before a shared rule is posted. The organizer can accept that limit while warning that public support may be harder to mobilize. The player’s choice changes who can speak and what becomes visible, not a hidden Rebel loyalty state.

### 61.2 Rebel act II — the hand-to-hand record

The organizer proposes that each worker carry a copy of the current schedule, so no central board can be altered without people noticing. This improves distributed access but creates multiple copies to maintain. The player can trial two copies; use a single master with a public edit log; invite each worker to choose whether they want a copy; establish a rotating pair responsible for synchronization; or reject duplicate records as too costly.

If two copies diverge, the game shows which one each person used. The organizer can help reconcile them, but the people affected must decide which line is operational. A player who chose a single master can still support shared decision-making through an edit meeting. The route avoids equating decentralization with duplication.

### 61.3 Rebel act III — message network under load

Two residents need updates at different times. The organizer offers a volunteer relay network. The player can approve the relay for all changes; restrict it to urgent changes; require the sender to check who received it; rotate the messenger; or use face-to-face readback for this one decision.

A relay network can make action fast and resilient when one person is absent. It can also multiply a misunderstanding. If the player requires acknowledgment, the update may arrive later but has a stronger receipt record. If they use face-to-face readback, not every absent person hears it. If the player rotates messengers, the burden spreads but each person may need training. The volunteer can decline a route; the game should not punish refusal with a moral label.

### 61.4 Rebel act IV — the public claim

A Rebel speaker announces that the shelter has established collective control. One worker says the meeting did not include them. The player can correct the speaker publicly; ask the worker privately how to represent their absence; describe the rule as a trial supported by those present; delay the announcement until the worker can respond; or let the claim stand.

The organizer may feel that a public correction weakens the movement. Another resident may feel relieved that the absent person was not spoken for. Both responses can coexist. A corrected claim should preserve what the group did decide and name its limits. If the player delays, temporary coverage still has to be established. A missed meeting is not a reason to keep work impossible.

### 61.5 Rebel act V — the work nobody sees

The schedule is being updated by a rotating pair, but the same two people keep volunteering because they know how. The player can ask the pair if they want to continue; ask for volunteers with a bounded duration; create a training session; reduce the number of copies; or accept the pattern and name it as a dependency.

The organizer can offer space for training but cannot make a trainee accept the job. If work rotates, the process becomes more resilient but may take longer. If the pair continues, the shelter keeps expertise but relies on their availability. If the player simplifies the record, some historical context may be lost. The branch is about labor distribution, not proving ideological purity.

### 61.6 Rebel act VI — closing assembly

The group checks whether the rule was used, whether copies stayed aligned, and who bore the coordination work. The player can invite a public review; publish a summary while keeping individual details private; allow a smaller group to decide renewal; set a recall date for the organizer’s role; or leave without a continuing agreement.

**Route resolutions:**

- A distributed copy system survives absences but has a named reconciliation method.
- A single shared board remains central but has transparent edits and a review role.
- A meeting-led practice gives affected workers authority but uses temporary coverage during absences.
- A volunteer relay exists for defined messages and can be refused or rotated.
- An incomplete collective arrangement is still reported honestly: the rule was agreed by some, untested by others, and requires another review.

The Rebel ending shows whose participation shaped the practice and whose participation remains open. It does not declare the arrangement collective merely because the organizer said so.

## 62. Independent route — “Terms With an End Date,” extended campaign treatment

The Independent route should make a credible offer: supplies, specialist access, and negotiated flexibility beyond the major factions’ structures. Its risk is that scarcity can make supposedly voluntary terms difficult to refuse and repeated renewals can become quiet dependency. A transaction is a tool the player can use, not a test of selfishness or virtue.

### 62.1 Independent opening — the offer sheet

A broker offers a bundle: paper, a marker, and one repair visit in exchange for goods or work. The player can take the full bundle; negotiate only the item urgently needed; offer a different good; ask for a written quantity and delivery date; request a trial without renewal; or decline.

The broker explains replacement and return terms. If the player lacks the requested exchange, the broker can counteroffer labor or wait, but the shelter still has its own material needs. No hidden penalty appears for declining. The exchange is recorded through the existing item or trade authority if later implemented; this plan does not invent a parallel market ledger.

### 62.2 Independent act II — what counts as a delivery

The first bundle arrives short one marker. The broker says the supply runner counted the package before it was opened. The player can accept the partial delivery and adjust the exchange; wait for the missing item; ask the runner to inspect the seal; record a disputed count; or cancel the rest of the deal.

Each action leads to different future trust in the transaction, expressed through evidence rather than a score. If the player accepts the short package, they may still negotiate a credit. If they wait, the schedule may be harder to update today. If they record a dispute, the broker can accept it, offer a witness, or refuse. If they cancel, any goods already exchanged require a return or a clear loss; the transaction does not simply vanish.

### 62.3 Independent act III — the convenience fee

The broker offers to keep the schedule copies synchronized for a fee. The player can accept for one rotation; ask what the service does not cover; hire only a copy delivery; train a local worker instead; or decline. The player can also ask the affected residents whether they want an outside copy in circulation.

The service is valuable when local labor is stretched. It is risky if the broker becomes the only person who knows which copy is current. A one-rotation contract needs an end date and an instruction for disputes. Training reduces future dependence but costs a work window. A refusal leaves synchronization local and names who will do it. Later scenes check actual delivery rather than assuming the contract worked.

### 62.4 Independent act IV — dependency becomes visible

A new delivery is delayed because the broker’s route is blocked. The schedule supply will not arrive before the next rotation. The player can use a reusable surface; ask the Guild for a substitute; request the broker’s route report; ration remaining paper; or change the schedule process to reduce copies. The broker may help solve the problem but cannot guarantee the route.

This branch is where a generous deal may reveal dependency. The game should not retroactively call the player foolish for accepting it. The player responds to the current state with options. The remaining contract may be honored later, renegotiated, suspended, or closed.

### 62.5 Independent act V — renewal and exit

At the end of the trial, the broker offers a renewal at a different cost. The player can renew as written; negotiate a lower volume; switch to occasional one-time purchases; ask for transfer of the synchronization notes; end the deal and return reusable materials; or let it lapse. The residents can have distinct opinions: one values reliable supply, another dislikes outside access, and another wants less time spent maintaining copies.

The player can set a future review with no advance promise. If renewal is refused, the broker does not become an enemy by default. The route records what was delivered and which dependency remains.

### 62.6 Independent act VI — closing account

The ending recounts quantities delivered, work exchanged, service used, and outstanding balance if supported by current economy systems. It also states which practice remains local and what service expires.

**Route resolutions:**

- A bounded supply agreement with renewal by explicit choice.
- A narrow one-time trade that solves a current need without ongoing service.
- A paid synchronization service with local correction authority and named exit procedure.
- A declined arrangement that leaves a visible material shortage but no outside access obligation.
- A lapsed or disputed agreement whose final terms are still reviewable.

The Independent route remains distinct through negotiation and service, while player actions—not an “independent” label—determine how much control and dependence the shelter experiences.

## 63. No-commitment route — “The Board Stays Local,” extended campaign treatment

The no-commitment route is a real route, not a blank state between faction quests. It allows the player to keep the issue local while acknowledging that neutrality does not create resources, erase outside pressure, or prevent factions from acting around the shelter.

### 63.1 Opening — local request

The player declines a major faction’s first offer. The representative asks whether that means no contact, no roster, or no help of any kind. The player can clarify the boundary: no names shared; no recurring commitment; no public claim; no faction representative in the decision; or no present agreement while leaving a future conversation possible.

A clear answer prevents the game from interpreting “no” as a universal ban. If the player gives no clarification, the representative withdraws and the shelter proceeds with fewer outside services. The route should not force the player to choose a permanent political identity.

### 63.2 Act II — capacity inventory

Without a faction-provided copy or patrol relay, the player takes stock of local materials and skills. They can ask each character what they can maintain; inspect current supplies; ask a supporting current for one bounded service; reduce the schedule’s information needs; or postpone a permanent system. The game reveals constraints through observations, not by declaring that independence is impossible.

A supporting current can still be hired or consulted. The player may use the Guild for one repair while refusing a standing deal, or ask Long Walk for one timing report without changing political alignment. The fact of receiving a service is recorded separately from major-faction commitment.

### 63.3 Act III — outside pressure without a sponsor

A major faction uses the shelter’s route or resource nearby. The player can ask for a temporary boundary; arrange a local contact; document the crossing without sharing the roster; move the workroom schedule away from the exposed location; or allow limited use under a written end time. If the player refuses all contact, the faction may proceed under its own authority, and the shelter deals with the consequence. This is a real cost, not a hidden alignment penalty.

### 63.4 Act IV — local failure recovery

A locally maintained record is lost or damaged. The player can reconstruct it from people’s accounts; accept uncertainty and make a new schedule; ask a support current to preserve a replacement; or suspend assignments until the affected workers are consulted. Without an outside institution, local memory matters more, but it may be distributed and conflicting. The route avoids pretending that local control makes records infallible.

### 63.5 Act V — deciding what not to build

The group cannot maintain every desired feature. The player can prioritize current clarity, historical record, accessibility, privacy, or low upkeep. They can also choose no permanent board and hold a short meeting each shift. Every choice has a burden: more meetings consume time, limited records make disputes harder to reconstruct, narrow access requires more targeted delivery, and an elaborate board needs maintenance.

### 63.6 Act VI — close without a faction seal

At the ending, no faction endorses the practice. A local resident describes who will maintain it and what the shelter cannot guarantee. The player can invite a supporting current to a narrow review; publish a local account; leave it private; or leave the process open.

**Route resolutions:**

- A local board with named upkeep and no outside reporting.
- A minimal roster with direct confirmation at each shift.
- A local process supported by an occasional, bounded service from a support current.
- A temporary arrangement while capacity is rebuilt.
- An unresolved local practice with a precise account of what remains unavailable.

The no-commitment ending is not automatically more free or more difficult than the faction endings. Its costs come from the concrete services and agreements the player did not accept.

## 64. Convergence without route erasure

The four routes may arrive at a shared final review, but the review must preserve their different institutional relationships.

A Military run may need to report whether the agreed coverage service remains active. A Rebel run may need to verify whether people outside the meeting saw the new rule. An Independent run may need to count which supplies or services expire. A no-commitment run may need to ask whether local labor can continue without a sponsor. These are different questions even when the final board looks similar.

The route can converge on one physical scene with distinct attendees, props, and concerns. If the player’s board is current and reviewable on every route, the same phrase “the list is clear” may be true, but the underlying ending must also show who maintains it, whose information is stored, and what happens when the person responsible is away.

### 64.1 Minimum route-specific ending payload

Every route’s ending payload should identify:

- The practical state of the schedule or replacement process.
- The current maintainer and whether they accepted recurring responsibility.
- The form and audience of any retained record.
- The access method for a person who cannot use the central board.
- The outside service, if any, and its expiry or renewal terms.
- The faction pressure that remains active after the ending.
- The specific unresolved event that could reopen the quest.
- The character who has a meaningful next action, even if the player is absent.

If any field is unknown, the ending should present it as unknown rather than fill it with a default “everyone is satisfied.” A compact ending can satisfy these points through a short exchange, a prop, and one journal entry rather than a lengthy exposition scene.

### 64.2 Route-neutral branch availability

The player should be able to verify facts, make a rapid repair, listen to an affected worker, delegate, or set a boundary on any of the four routes, subject to actual situational constraints. The faction changes cost and available help, not the player’s capacity to act. A route-specific offer may be unavailable after a refusal or expiry, but its absence should be explained in context.

### 64.3 How re-entry works after refusal

A player who declines an early offer may receive a later, different offer if circumstances change. Re-entry should not erase the refusal or pretend that the player accepted the original terms. The later scene names what changed: the faction reduced the requested information, replaced a recurring obligation with one-time aid, or now requires a different condition. The player can reconsider, negotiate, or refuse again.

## 65. Route stress tests for future implementation review

These stress tests are design questions for a later implementation package, not tests to run now.

1. If the player refuses every major-faction offer, can the main quest still reach an ending with clear practical consequences?
2. If the player accepts military help but gives no names, does the route remain coherent?
3. If a Rebel meeting excludes one affected person, does the narrative preserve that absence rather than falsely declaring unanimity?
4. If the Independent delivery is late, can the player continue with an existing alternative?
5. If the player changes route stance halfway through, do previous actions remain in history while new options appear?
6. If every support current is declined, does the central story still resolve?
7. If only one support current is accepted, can that current provide its service without becoming the sole cause of the ending?
8. If an NPC refuses to share a personal reason, can the player still respond to the practical effect?
9. If an outside faction misstates an event, can a correction be made without erasing the original report?
10. If a route has a resource shortage, are costs visible and connected to an existing resource owner?
11. If a player skips all side quests, can they understand each final option?
12. If an ending occurs with a stale copy, does the final scene show the risk and name who can correct it?
13. Does each faction route retain meaningful differences after the central cast’s actions are held constant?
14. Does each identical action across routes still produce the same causal fact, even if the faction response differs?
15. Can a reviewer identify which player action made each route option available without consulting a hidden moral score?

## Installment 6 closing note

This installment develops four campaign routes across the act spine. Military introduces operational support with reporting boundaries; Rebel introduces shared control and its coordination labor; Independent introduces negotiable services with scarcity and renewal; no-commitment gives local capacity a full story with visible limits. Shared endings retain route history and do not convert affiliation into the sole determinant of outcome. The material remains provisional pending canon and architecture review.
# Installment 7 — Survival-system intersections and consequence ownership

## 66. Cross-system branch design: pressure must come from an existing owner

This questline takes place inside a survival management game. Its schedule and record dispute should sometimes collide with survival demands, but the narrative must never invent a second source of food, labor, health, water, power, radiation, or campaign time. The content plan can describe a branch that depends on an existing system signal. Before implementation, each proposed signal must be checked against the current API, current owner, save behavior, and host event route. If the signal does not exist, either leave the branch as a player-authored dialogue choice or request a separately approved system change.

A narrative interaction is valid only if it respects this boundary:

- The existing system owns its state and mutation.
- The quest reads a stable fact or receives an event through the established seam.
- The player makes a narrative decision through the existing quest command route.
- A consequence is applied by the system that owns it, not by a panel or local quest counter.
- Persistent story context is captured by the current save owner if it must survive a reload.
- If no verified seam exists, the interaction stays a design proposal and must not be represented as an implemented feature.

The branch should not translate survival performance into virtue. A low food reserve does not mean the player is selfish. A sick survivor does not mean the player failed to care. A broken light does not mean Mina was negligent. These states shape what options are practical and what costs follow; characters still respond to the actual choices and constraints.

## 67. Cross-system interaction matrix

| Existing domain to verify | Narrative trigger | Branch contribution | Consequence that remains with owner | Prohibited shortcut |
|---|---|---|---|---|
| Inventory and trade | Needed paper, fastener, lamp, tool, or repair item is unavailable | Reuse a surface, request a bounded supply, trade, defer, or reduce copies | Actual item quantity and transaction | Quest-local supply counter |
| Needs and work capacity | A worker is unavailable or requests a lower burden | Defer, split, cover, or simplify the task | Existing need/workload state | Inferring motive or consent from an absent shift |
| Health or body integrity | A maintenance task may be unsafe for a survivor | Offer alternate work, wait, or seek help with consent | Existing health and treatment owner | Quest panel diagnoses or changes health |
| Power and lighting | A room or board becomes difficult to use in low light | Move the work, repair the lamp, post elsewhere, or use a timed readback | Existing power/repair authority | Treating an unlit location as merely flavor if the game models power |
| Water and sanitation | Shift timing conflicts with a shared water task | Reorder tasks, assign coverage, or accept a known delay | Current water and task owners | Adding a second water tally for narrative outcomes |
| Radiation exposure | A courier route or outdoor posting point becomes costly | Choose another route, use a relay, wait for a window, or expose a willing actor under existing rules | Existing exposure and radiation systems | Quest-defined exposure amount or unseeded random hazard |
| Relationship/relationship events | A character reacts to a specific act or boundary | Acknowledge, repair, disagree, or choose another collaborator | Existing relationship owner if applicable | Hidden loyalty score parallel to current authority |
| Campaign clock | Notice arrives late or a trial period expires | Make timing visible and let the player act before or after a deadline | Existing campaign day/time authority | Hardcoded day 1 or wall-clock timestamp |
| Expedition or travel | A required person or supply is outside the shelter | Send a route request, wait, choose a substitute, or abandon the dependency | Existing travel and expedition owner | Quest-only travel state |
| Radio and communications | The player chooses a public update or waits for a reply | Broadcast only approved scope; distinguish transmission from receipt | Existing radio/event route | Treating broadcast as proof every listener heard it |
| Journal or chronicle | A major correction or unresolved decision deserves a record | Record a dated summary with source and uncertainty | Current journal/chronicle owner | A second independent history database |
| Faction campaign state | Route representative offers service or applies pressure | Accept bounded assistance, negotiate, refuse, or change who can help | Current faction authority | A new allegiance or morality meter |

The table is a review tool rather than a promise that every listed domain has a usable event. The implementation package must mark each row supported, unsupported, or intentionally excluded. An unsupported row cannot be filled with a guessed field or direct reach into unrelated mutable state.

## 68. Scenario: a maintenance shift meets a health need

Mina is scheduled to stabilize the board mount. Before the shift, an existing health or needs system may make her unavailable or show a relevant limitation. The story must not disclose a private diagnosis unless Mina chooses to share it. It may instead say that she asked to postpone the task or is not available for this shift.

The player can accept the delay and use a temporary posting method; ask Mina whether she wants to suggest another worker; request a short demonstration if she is willing; seek an outside repair; or defer the board task and prioritize another survival need. The game uses the existing system to represent any health consequence. The narrative records only the consented work arrangement and whether coverage changed.

If the player pressures Mina after she declines, the branch should reflect that pressure in later dialogue and the work can remain incomplete. The content must not silently mark the repair complete because a quest option was clicked. If the player chooses a substitute, the substitute’s own capacity and acceptance are checked through the existing system or explicit conversation, not assumed.

A successful branch can still carry cost: the board remains awkward until the next work window, or the substitute postpones their own task. A failed repair does not require a catastrophic injury. The consequence is visible workload, delayed access, or a named maintenance gap.

## 69. Scenario: low supplies change the copy policy

A resource check finds enough paper for one durable copy. This fact must come from the current inventory owner. The player can spend it on the current schedule; preserve it for archival evidence; use a reusable surface and create a readback routine; trade for more materials; or narrow the record to the information needed for the next shift.

The choice creates two separate questions: what item was spent, and what practice was adopted. Spending a sheet does not automatically mean the group now favors public records. Saving a sheet does not mean the player has chosen secrecy. Dialogue names the immediate purpose, and a later scene checks whether the method worked.

If a trade is accepted, the ordinary trade owner handles the cost and transfer. A failed negotiation leaves inventory unchanged and the narrative says why the alternative is needed. The quest must not subtract supplies when a panel closes, when a scene reloads, or because the player selected text that only described an offer.

A later shortage can produce a callback without resetting history. The player may have adopted a good practice that the shelter can no longer afford. The ending then distinguishes “the rule was sound” from “the required material was available.”

## 70. Scenario: the route is unsafe or expensive today

Osha’s delivery path may cross a hazardous or costly area. Before implementation, verify whether the current game models that cost through travel time, radiation exposure, injury risk, or another existing domain. If no corresponding mechanic exists, author a clear fictional constraint and let the player choose without inventing a numeric penalty.

The player can delay the delivery until a safer window; ask a willing person to use a different path; relay only the practical fact through radio; meet the recipient halfway if the location supports it; or decide the message is not urgent enough to send. Osha may decline the route. Her refusal is not a betrayal, and another character does not automatically inherit it.

A route option should show the player what kind of cost is known and what remains uncertain. “This crosses the irradiated underpass; no one has checked it today” is more legible than “high risk.” If a modeled exposure cost is applied, the existing radiation authority handles it with deterministic behavior. The quest does not roll an unrelated random injury.

The player can choose to accept a cost for an urgent message. The later scene remembers that the message was sent through that route and whether it arrived. It does not call exposure a moral sacrifice or grant a hidden virtue reward.

## 71. Scenario: water duty and the board dispute collide

A resident scheduled to update the board also has a water collection task. The player must choose which task happens first, whether someone can cover, or whether the record can wait. If the current survival schedule already owns both tasks, this quest reads them. If it does not, the content may still present a human conversation about competing time but cannot claim a systems-level water assignment exists.

**Prioritize water:** the board stays stale for a known interval. The player can post an interim verbal notice or mark the old copy provisional. Water delivery follows the current owner’s rules.

**Prioritize the record:** the water task slips or receives a substitute. The player sees the supply or labor consequence before confirming.

**Split the shift:** two people share the work if both accept. The resulting arrangement identifies which part each person covered.

**Delay both:** the player can convene a quick decision, but a timer or system cost must use the current clock owner. If there is no supported timer, the choice remains a narrative timing agreement rather than a false countdown.

The branch makes competing needs visible without declaring that schedules always outrank survival tasks or vice versa. The characters can disagree about the priority, and the player may change the rule next time.

## 72. Scenario: the expedition report and local witness

A Long Walk report returns from outside the shelter. It says when a message left the route and when it reached a checkpoint. A local witness says the recipient did not see it until later. These accounts are compatible if they refer to different stages of transmission.

The player can keep both reports with their source and scope; ask the courier for a route explanation; ask the recipient what they saw; avoid choosing a disputed timestamp; or use only the information needed for today’s coverage. The expedition system owns the route outcome. The local quest owns only the decision about how the shelter represents the evidence, through the current quest authority.

If the player relies on the route report to declare delivery, the recipient can correct them. If the player treats the local account as proof the courier never arrived, the courier can point to the checkpoint. The story supports partial knowledge and correction without accusing either person of lying.

## 73. Scenario: public radio versus private correction

A roster correction may matter to several listeners while including a private reason that should not be broadcast. The player chooses among a broadcast with only the practical change; direct messages to named workers; a public notice asking listeners to check in; a delayed announcement after obtaining consent; or no broadcast.

The communications owner determines whether the message was transmitted. A quest branch may record the intended scope and the later confirmation, but transmission does not prove receipt. If the broadcast fails, the game should expose that through the current radio system’s event or return status if available. Where no receipt signal exists, the narrative should say “broadcast sent” rather than “everyone informed.”

A private correction can be paired with a public operational update. This allows the player to fix a shared schedule without making a person’s reason public. If the player publicly shares more than the affected person agreed to, a later scene recognizes the exposure and allows a practical response; it should not hide the breach in an invisible morality total.

## 74. Cross-system ending combinations

The final act can recognize one major survival pressure alongside the board practice. It should not create an exhaustive Cartesian product of every resource and faction. Instead, select the most causally important condition and use the ending to show the others through concise evidence.

- **Supply scarcity + public current schedule:** the board remains current, but only one durable copy exists. The ending names the replenishment need and the chosen interim communication method.
- **Worker capacity shortage + rotating maintenance:** the rotation is fairer but currently underfilled. The group must reduce maintenance frequency or recruit a willing additional worker.
- **Power instability + accessible second posting point:** the alternate location remains useful but requires light or daylight access. The story names the dependency instead of calling access solved.
- **Radiation route restriction + courier practice:** the shelter uses a safe-window delivery and accepts slower notice. Urgent messages need a separate fallback.
- **Radio failure + local confirmation:** the player no longer treats broadcast as receipt and uses direct confirmation for high-impact changes. This costs time and limits range.
- **Expedition delay + bounded Independent supply:** the supply agreement still has terms, but the next delivery date has shifted. The player can negotiate or use a local substitute.
- **Relationship conflict + no faction sponsor:** the dispute remains local, and the shelter lacks an outside mediator. A chosen witness or time-limited review can still provide structure.
- **Health limitation + delegated repair:** the repair owner changes for a period, but the original worker retains the right to return or not return to the task.

These are ending overlays only when source events support them. No ending should infer “scarcity” from a low value unless the current owner exposes that condition and the content is designed to read it.

## 75. Cross-system implementation boundary checklist

Before any package connects this expansion to a survival system, the owner should answer:

1. Which existing service is authoritative for the state being read or changed?
2. Is the quest consuming a domain event, querying a stable view, or issuing a command through the current host seam?
3. Does the state survive save/load, and which existing section owns capture and restore?
4. Is the operation deterministic and replayable?
5. What does the player see if the event never arrives or the target system is unavailable?
6. Can the operation be retried without applying a duplicate item, task, or relationship effect?
7. Does the UI report the actual result, including refusal, delay, partial fulfillment, and failure?
8. Is the proposed behavior already owned by another current plan or integration package?
9. Does the change require an architecture decision or overlap a path claim?
10. Can the same narrative result be authored as a non-systemic choice if no valid seam exists?

A “no” or unknown answer is an integration boundary, not an invitation to create a local substitute. Keep the plan’s narrative branch and mark the systems hook unapproved until evidence and ownership are available.

## 76. Survival pressure does not become moral pressure

The expansion may place several needs in conflict, but it must not tell the player that one need is always the righteous answer. Choosing water over paperwork can be responsible. Choosing to correct a public record before taking a supply trip can also be responsible. The result depends on urgency, who is affected, what alternatives exist, and what the player actually knows.

Branch descriptions should present the known consequence in concrete terms. Do not write “the kind choice” or “the selfish option.” Write “This uses the last dry sheet, so no dated copy remains,” or “This keeps the worker off the repair today, but the bracket will still need another check.” Let characters hold different values and let players understand the trade.

This also prevents the faction overlay from becoming a morality proxy. The Military may value quick coverage; the Rebel organizer may value open participation; the Independent broker may value a clear exchange; the local group may value privacy. The player can agree with any value in one scene and contest it later. None of those positions is a complete account of the player.

## Installment 7 closing note

This installment specifies how survival systems can shape the quest without duplicating them. It supplies conditional scenarios for health/work capacity, supplies, travel risk, water duty, expedition reports, and radio; maps ending overlays; and defines the evidence required before any production connection. Every system interaction remains provisional until its current owner and route are verified.
# Installment 8 — Encounter atlas, journal language, and recurring cast

## 77. Encounter atlas: ten ways the same dispute reaches the player

These encounters are short playable units that can sit between the eight main acts. They make the shelter feel inhabited and let the player meet the consequences of an earlier decision during ordinary work. They are not ten mandatory quests. At most a few should appear in one run, selected by the current narrative state and available characters. Their presence, ordering, and route must be validated against the existing encounter system before implementation.

### 77.1 The cup under the leak

**Trigger:** the player has not yet repaired or covered the wet board, or a later storm reopens the problem.

A cup beneath the leak is almost full. The schedule edge has begun to curl again. The player can empty the cup, move the board, cover the paper, ask Mina to inspect the seam, or leave the current task for another worker.

Emptying the cup prevents an immediate spill but does not fix the leak. Moving the board can protect the schedule and make the adjacent wall harder to use. Covering the sheet preserves it only if the material is actually available. Asking Mina can reveal that the earlier repair was temporary, not that she failed. Leaving the task is valid if another person accepted it; otherwise the cup may overflow later.

The encounter tests whether a previous repair had an owner and a revisit time. It should not recur every day as a nuisance if no state changed. When it reappears, the room shows what changed: more water, a new cover, a dry wall, or the same unresolved seam.

### 77.2 The worker who wants a spoken answer

**Trigger:** the player posted a correction but has no confirmed notice event for one affected person.

The worker asks, “Can you tell me what changed without pointing me at the wall?” The player can read the current shift only; explain the correction and its source; ask whether they want the old line included; provide a dated copy; or ask someone else to deliver the message.

A spoken explanation can satisfy immediate access but is not a durable record. A full explanation may disclose more than needed. The worker can interrupt to ask about one detail. If the player delegates, the delegate may return with a question rather than a confirmation. The encounter teaches that access format is a choice, not a consequence of whether someone is “good at reading.”

### 77.3 The unclaimed marker

**Trigger:** the player has used a marker or writing tool and no current owner for the tool is recorded.

A marker lies on a dry shelf. Two residents believe it was left for their work. The player can ask who used it last, post a shared tool note, give it to one person for the current task, return it to the common shelf, or leave it available without deciding.

If one person receives it, the other task may be delayed. A shared tool note can help later only if someone maintains it. Returning it to a common shelf preserves availability but does not identify who should replace it when dry. Leaving it does not count as a hidden refusal. A later callback depends on whether an actual borrower and return were recorded.

### 77.4 The unplanned witness

**Trigger:** the player updates or removes a public line while another resident is present, but that resident was not part of the decision.

The witness says, “I saw the words change. I don’t know what you agreed with them.” The player can explain the reason; show the old copy; ask the witness to compare it with the new one; ask them not to repeat private details; or say the change was urgent and leave the explanation open.

This encounter separates seeing an action from understanding it. If the player asks the witness to carry the explanation, they must agree. If not, their presence remains an observation, not an authorization. A faction representative who witnessed the change may later describe it from their own interest; the encounter does not automatically promote them to neutral witness.

### 77.5 The request for a name

**Trigger:** a report or broadcast says that coverage is missing but does not name the worker.

A liaison or organizer asks, “Who left the shift open?” The player can give the name with consent; explain that the reason is unconfirmed; offer a coverage total; ask why a name is needed; or refuse to provide it. The other character can accept the limited information, challenge the refusal, or explain a practical need.

If the player provides a name without consent, the affected character may later respond to the disclosure. If a name was already public on a board, the player can still distinguish “the name was visible” from “the reason is public.” The encounter makes scope actionable without requiring the player to deny known facts.

### 77.6 The tool with the wrong mark

**Trigger:** a repair tool or part is used by a character other than its usual owner, or an earlier repair remains temporary.

A wrench has a new scrape across its handle. Mina recognizes the tool, not the hand that used it. The player can ask who borrowed it, ask whether the mark affects use, clean and return it, document the repair, or leave the tool while asking Mina to test it.

The branch reveals that wear can be observed without knowing who caused it. If a previous task required a return, the encounter can close that loan. If no borrower was recorded, the story does not invent one. A scavenger may identify the mark as common wear but cannot confirm who used it.

### 77.7 The empty chair at the review

**Trigger:** the player scheduled a meeting or a review, and a stakeholder did not attend.

The empty chair may mean the person was not told, could not come, declined, or simply chose another task. The player can begin and record who is absent; delay the decision; send a second invitation; ask whether a proxy is authorized; or make only a temporary operational choice.

A proxy speaks for someone only if the person authorized it. Beginning without the person can still produce a usable temporary arrangement, but not their consent. Delaying may preserve inclusion and leave work unresolved. A second invitation costs time and can fail. The encounter prevents silence or absence from becoming consent.

### 77.8 The second board

**Trigger:** the player created or authorized another posting point.

A resident arrives carrying two versions. The second board has one extra line and an older review date. The player can compare both, ask who maintains each, mark one as superseded, keep them both with distinct purposes, or remove the second posting point.

If one board is intentionally a current schedule and another is an archive, the encounter confirms whether users understand that distinction. If not, the player can relabel them. Removing a board may reduce stale information but make access harder for someone who relied on it. The branch shows that duplication requires maintenance and purpose.

### 77.9 The offer to take the task forever

**Trigger:** a character has covered the same task repeatedly or a player has asked them to take it again.

A character says, “I can keep doing this if you want. I need you to say whether you mean today or every week.” The player can ask for one shift, ask the character to propose a rotation, accept recurring work with a review date, seek another volunteer, or admit the shelter has no durable cover yet.

An acceptance without duration is not automatically recurring consent. A rotation can distribute burden but requires training. A single shift solves today’s gap. If the player accepts recurring work, a later scene checks whether the arrangement is still wanted. This encounter can surface hidden labor without automatically condemning the player.

### 77.10 The version nobody remembers

**Trigger:** two records disagree and no person can reliably identify which one came first.

The player can keep both as disputed; create a new current schedule after asking affected people; select the version that matches observed work but mark the source; ask the Archivists to retain both; or stop using the record until a new process is agreed.

This encounter has no perfect answer. A new schedule can restore operation but cannot reconstruct the missing history. Keeping both supports review but can perpetuate confusion. Choosing the record that matches observed work uses evidence but does not prove consent. Asking for outside retention provides custody, not truth. The ending may remain unresolved while the next shift still proceeds.

## 78. Recurrence and pacing rules for encounters

Recurring content should reflect change rather than repeat a completed conversation. An encounter may return when a new event occurs: a second missed delivery, a new copy, a changed resource condition, a person returning to a task, or the expiry of a trial. A returned scene should have an explicit new question.

The narrative owner should classify encounter recurrence:

- **One-time scene:** appears once, then records its outcome.
- **State check:** reappears only when a relevant dependency changes.
- **Scheduled review:** returns at an agreed interval or story milestone.
- **Failure follow-up:** appears after an identifiable event, such as an expired copy or missed repair.
- **Ambient callback:** changes a line or prop but does not reopen the quest by itself.

Do not use a generic daily chance to make this quest feel alive. A randomized encounter that repeats without a deterministic trigger can contradict the player’s history and undermine replayability. If encounter selection uses seeded randomness under an existing owner, content must still respect eligibility and avoid reintroducing a resolved decision as new.

A good pacing pattern is to let a major scene create a practice, then let a quieter scene test that practice. For example, Act VII establishes a trial rule; one small encounter asks whether the copy reached a worker; Act VIII reviews the actual consequence. The quiet encounter should not reveal a new rule the player could not have known when choosing.

## 79. Journal and objective language across route states

Journal entries should explain what the player knows, not summarize an omniscient version of the truth. They should distinguish accusation, report, observation, decision, and unresolved question. A journal line should never say “the worker refused the shift” if the only fact is that they did not arrive.

### 79.1 Objective wording for the main acts

- **Act I:** “Find out what the damaged board can still tell you.” Optional sub-objectives may include comparing the receipt, asking a waiting worker, or stabilizing the wet sheet. They are alternative approaches, not a checklist requiring all.
- **Act II:** “Decide which account is safe to use for the next shift.” The journal names the source held and whether a second account exists.
- **Act III:** “Choose who needs a copy, readback, or current posting.” It states whether a duplicate was created and who received it.
- **Act IV:** “Cover the work or leave a clear gap while the absence remains unconfirmed.” This wording preserves both action and uncertainty.
- **Act V:** “Restore a usable way to post, with a known repair limit.” The objective reports the selected method and its inspection date.
- **Act VI:** “Correct the current record without erasing the disputed account.” If the player chooses not to correct publicly, the objective should state that the public line remains unresolved.
- **Act VII:** “Set a next-shift practice, a trial, or no standing rule.” The journal names maintainer, expiry, and unanswered questions where known.
- **Act VIII:** “Review what the practice did when people used it.” It must not claim success before the player checks.

### 79.2 Sample knowledge-limited entries

**After a message is sent but not acknowledged:** “Osha carried the second notice to the west room. No acknowledgment has returned.”

**After a person refuses to explain:** “The assignment is declined. The reason remains private. Coverage is still open.”

**After two records conflict:** “The later copy changes the east watch. The people involved remember the conversation differently.”

**After a substitute accepts one shift:** “Mina will cover this shift only. Her maintenance check is postponed until the next available window.”

**After public correction:** “The board now marks the earlier line as disputed. The reason was not included in the public notice.”

**After no rule is chosen:** “The next shift has a provisional plan. The shelter has not agreed on a recurring process.”

**After a timed trial begins:** “The shared-copy practice runs through the next review date. Either side may ask to revise it.”

### 79.3 Journal entries must be retractable only as current summaries

A later correction may change the current summary, but the event history should not be rewritten as if the earlier belief never existed. If the player learns the message was delivered, the journal can update the current knowledge to “delivery confirmed on the second attempt.” It should not delete the record that the first attempt was unconfirmed. This is a narrative continuity requirement and a persistence question for the appropriate journal authority.

## 80. Short dialogue bank: actions have different social texture

These lines are modular examples. They are not a substitute for full context. The speaker and listener must have the required knowledge.

### 80.1 After inspection

- Rade: “You checked the copy before you moved the line. That gives me something I can point to.”
- Osha: “You asked who had actually seen it. Good. I only knew who I’d met.”
- Mina: “You looked at the bracket before asking me to fix it. It’s not the same job as replacing the sheet.”
- A worker: “I thought the board meant I was expected. Now I know it was a question.”

### 80.2 After acting first and documenting later

- Rade: “The bench stayed covered. The old copy stayed up longer than it should have.”
- Osha: “I can carry the correction now. Tell me which parts you want repeated.”
- Mina: “It’s working. Put the check date beside it so nobody calls that permanent.”
- A worker: “I saw the change after I came back. I can work with that if you tell me what happened.”

### 80.3 After a refusal is respected

- Rade: “You left the reason off the record. The assignment still needs a new name.”
- Osha: “You didn’t ask me to carry the part that wasn’t mine to tell.”
- Mina: “No one wrote me down for the next repair. I can decide when I’m ready.”
- A worker: “You asked once. I said no. We can talk about the job without talking about why.”

### 80.4 After delegated work

- Rade: “You left the copy with me. I changed the date, not the assignment.”
- Osha: “I made the run. The return message didn’t come back.”
- Mina: “I showed her the brace. She hasn’t agreed to take the maintenance.”
- A worker: “You let us decide, then came back to see what we decided.”

### 80.5 After a repair

- Rade: “The old line is still there under the new one. That’s useful.”
- Osha: “The second copy made it to me this time. I didn’t assume it was current.”
- Mina: “The board is straight. Ask me again after the rain.”
- A worker: “I believed the first notice. I saw the correction later. Both things happened.”

### 80.6 After an outside faction overclaims

- Rade: “Their report says the roster was accepted. Our copy says it was a trial.”
- Osha: “The message left the checkpoint. I can’t say when the room heard it.”
- Mina: “They supplied the bracket. They didn’t maintain it.”
- A worker: “They can call the meeting collective. I wasn’t in it.”

These lines deliberately avoid universal praise or condemnation. Each acknowledges an action while leaving space for a separate opinion.

## 81. Minor cast roles for encounters

The three core survivors carry the main arc. A small number of recurring shelter roles can make the consequences visible beyond the core trio. Names below are provisional and require a collision pass; they are not canon and must not be added to data without review.

### 81.1 Iven Tal — kitchen rotation lead

Iven cares about meal timing and practical handoffs. He is willing to tell the player when a schedule change causes a missed meal or interrupts a water task, but he does not want the board to become another place where people’s private reasons are displayed. He can make a meal-time adjustment, offer a portion later, or refuse to absorb a recurring missed handoff. His role makes survival pressure tangible without becoming a moral judge.

**Encounter use:** if the player moves an update meeting into the meal period, Iven can say that the room is available but the kitchen cannot spare the person assigned to stir the pot. The player can reschedule, find cover, or accept a meal delay. If they chose a different hour earlier, he can add a quiet note that the change let the rotation finish together.

### 81.2 Sera Noll — stores assistant

Sera tracks consumable supplies through the existing inventory authority. Her personal concern is that people promise materials before confirming stock. She will distinguish an item on a shelf from an item reserved for another task. She can offer substitute materials, but names their quality limits.

**Encounter use:** when the player requests paper for an archive copy, Sera can explain that two sheets are reserved for medical labels. The player can keep that reservation, ask the group to reprioritize, use a reusable surface, or search for a trade. She must not own a second inventory count.

### 81.3 Pell Arven — night watch volunteer

Pell prefers concise notices and is often awake when other residents are not. He is a useful witness for whether a room was lit or whether a knock happened, but he does not know what a silent person intended. He can decline additional watch duties.

**Encounter use:** he can confirm that the second knock happened at a certain hour, if that event was actually observed. He cannot say who was inside unless he saw them. If the player asks him to remain an ongoing witness, he requests an expiry and relief.

### 81.4 Dema Venn — new arrival

Dema has not learned the shelter’s history and interprets the posted schedule literally. They ask direct questions that expose assumed knowledge: which copy is current, whom to ask, how to decline, and whether a crossed-out line still applies. Dema’s newness should not be used as a device for exposition dumps; their practical choices can reveal which parts of the process are legible.

**Encounter use:** a player who has maintained a clear current copy can point to it. A player whose rules are mostly spoken may need to introduce Dema to a resident. Both routes can work, but one creates a training task and the other creates a new posting obligation.

### 81.5 Minor-role boundary

Minor cast members should increase the number of perspectives, not inflate the faction roster or create a new resource authority. Their repeated appearances need a defined practical role, one personal boundary, and one way they can disagree with the player. They should not all repeat the core characters’ dialogue. Before use, verify that names, occupations, and scene functions do not collide with current survivor content or user-owned plans.

## 82. Encounter outcome vocabulary

To keep content, UI, and save behavior consistent, authors can use the following outcome words during planning. They remain descriptive until mapped to current project conventions.

- **Confirmed:** the named person acknowledged a specific message or arrangement.
- **Attempted:** an action was made, but its intended recipient or result is not established.
- **Provisional:** usable for a limited time, with a stated expiry or review.
- **Disputed:** more than one account remains, or a participant contests the record.
- **Deferred:** the action has not happened and has a stated reason or next opportunity.
- **Declined:** a person or the player refused a request; the reason may be private.
- **Covered:** a task was accepted by a named substitute for a defined scope.
- **Uncovered:** no one accepted responsibility for the task.
- **Corrected:** the current information changed and the old version remains identifiable.
- **Expired:** a trial, offer, copy, or agreement is no longer current.
- **Superseded:** a newer version is available, but the old one may be retained.
- **Unresolved:** the current play can continue while a question remains open.

These terms prevent common narrative errors. “Attempted” is not “confirmed.” “Covered” is not “completed.” “Corrected” is not “forgotten.” “Declined” is not “hostile.” “Unresolved” is not “blocked.” If the production system uses different state names, use its established names and preserve the conceptual distinctions.

## 83. Optional epilogue micro-scenes

The ending should have one main scene and a small number of optional micro-scenes that depend on what the player did. They give depth without turning every branch into a long final monologue.

### 83.1 The first update after the ending

A worker approaches the board with a change. If a correction rule exists, they use it without prompting. If no rule exists, they ask whom to tell. If the board was abandoned, they go to the agreed alternative. The player sees the practice operate once outside the quest’s immediate crisis.

### 83.2 The returned tool

If a tool was borrowed and returned, it rests on its hook with a date. If it remains missing, the shelf shows an empty place and the current maintenance limit. If the player chose not to track personal borrowers, the absence is recorded without an invented name. This scene makes resource accountability human-scale.

### 83.3 The rain test

If the player repaired or moved the board, the rain returns. A successful repair is not declared permanent: Mina checks it or someone reports the leak. If no repair occurred, the shelter uses its chosen alternate method. The scene rewards continuity, not a particular ending.

### 83.4 The message that gets through

If the player selected a return acknowledgment, the recipient answers with a short confirmation. If not, the courier reports only the attempt. This lets the player see the value and labor cost of confirmation without declaring that one communication style is always best.

### 83.5 The review date

If a trial rule had an expiry, a character points at the date and asks whether to renew, revise, or let it lapse. The ending remains open to future play. A deadline without a response does not automatically renew the arrangement unless the terms stated that behavior and the relevant owner supports it.

### 83.6 The quiet disagreement

If one person still disputes the history, they may leave their own note beside the official account, request privacy, or decline to participate. The final scene can show disagreement persisting alongside functioning work. No extra quest is mandatory to make the ending emotionally complete.

## 84. How to scale encounter content without multiplying flags

A large authored plan can easily recommend too many individual states. Before any implementation, group branches by player-visible consequences rather than by every sentence variant. For example, six different message actions may reduce to three meaningful outcomes: recipient confirmed; attempt unconfirmed; no message sent. The prose can still name which action caused that outcome if the current narrative owner retains the necessary context.

This reduction must not erase information that changes consent, cost, or future choice. A private message and a public broadcast should not collapse if later privacy dialogue depends on the audience. Two ways to get the same current schedule can converge if they preserve who was told, whether the source was checked, and who must maintain the process.

A branch audit should include:

1. The concrete player action.
2. The fact produced by that action.
3. The next scene that reads the fact.
4. The observable difference to the player.
5. The cost or risk that remains.
6. The state needed after save/load.
7. Whether another action can repair or supersede it.
8. Whether a run can still reach an ending if the branch is skipped.

If any branch has no distinct observable result and no later reader, either remove it, label it as flavor dialogue, or give it a justified consequence. Do not preserve it merely to make the graph look larger.

## Installment 8 closing note

This installment adds a ten-encounter atlas, pacing rules, knowledge-limited journal entries, a dialogue bank, provisional minor survivor roles, shared outcome vocabulary, and optional epilogue scenes. It strengthens recurrence and flavor while keeping the core branch facts small enough for a future owner to validate. All new names and encounter triggers remain provisional pending collision review.
# Installment 9 — Ending library, epilogue variants, and continuation hooks

## 85. Ending library: ten practical resolutions

The act graph needs endings that describe how the shelter will operate after the player leaves the immediate conflict. These are not a ranked good-to-bad ladder. Each ending has an entry condition based on actions taken and agreements actually made, a public-facing scene, a cost that remains, a faction lens if relevant, and a way for play to continue. The finale may combine one primary ending with a short route overlay and one character callback.

### 85.1 “A Line Anyone Can Correct” — public current record with bounded history

**Entry conditions:** the player and group made a procedure for current updates; the board or replacement method is usable; someone accepted maintenance; prior versions are retained only within an agreed scope.

At the final review, Rade writes the current shift in a steady hand, then leaves a clear space below it. Mina tests the board edge. Osha reads the date aloud to two people waiting by the doorway. The player is asked who should be allowed to correct an entry. The answer already exists in the group’s trial rule; the scene checks whether the rule is remembered.

The strength is reviewability: a person can see what changed and ask about it. The cost is labor and exposure. The public record can reveal more than some residents want, and somebody must keep it current. If the player used a privacy-limited approach, the history sits in a restricted copy and the public board shows only the operational change. If the player accepted a Rebel meeting process, the group decides the correction rule in the room; if not, the record steward and affected people use the agreed smaller process.

The military liaison can call the result dependable but cannot claim sole authorship. The Independent broker may ask whether the shelter needs another sheet, but renewal remains a separate choice. The no-commitment route may produce the same board without outside endorsement. The epilogue’s first correction tests whether the process works when no one is congratulating the player.

### 85.2 “Just Enough to Work” — low-disclosure current schedule

**Entry conditions:** the player limited personal details; the group has a way to tell each person what they need to know; immediate coverage is assigned.

The public board shows names or task assignments only where necessary, with private reasons omitted. A worker checks their own line with Rade. Another asks Osha for a spoken readback. The player can choose whether the process has a public correction marker or whether each correction is sent directly.

The strength is reduced exposure. The cost is more person-to-person communication and a narrower shared view. Someone who missed a message may not know what changed. The ending therefore names the contact method and what happens if the message cannot be delivered.

A faction overlay changes what an outside representative is allowed to receive. Military can receive a count of covered hours; Rebel can review the procedure without receiving private explanations; Independent may have no access to personal data even if it supplied the paper. If no faction was involved, the shelter’s own relay burden remains. This ending is valid when the player values privacy, but it is not a perfect shield against misunderstanding.

### 85.3 “Two Hands on the Shift” — rotating maintenance

**Entry conditions:** at least two people accepted a defined rotation, or the group agreed to train replacements; a review point is recorded.

The first person finishes the board update and passes the marker rather than the entire responsibility. The second person checks the date and leaves a small initial beside the current list. Mina asks whether the rotation includes repairs or only posting. The player can clarify that boundary in the final scene.

The strength is reduced dependence on one maintainer. The cost is training and coordination; a rotation may be slower and each person can make a different mistake. If one participant declined training, the ending must not imply they accepted. The military overlay can emphasize continuity between shifts; the Rebel overlay can emphasize shared access; the Independent overlay can offer material tools; none determines whether the people sustain the rotation.

A follow-up can show the rotation working, being paused, or becoming uneven. The ending commits only to the period people accepted, not an indefinite duty.

### 85.4 “Rade Keeps the Book” — single steward with review

**Entry conditions:** Rade accepted a bounded record role; affected residents can inspect or challenge entries; the role has a review date or replacement path.

Rade sits at the table with the oldest legible copy under a flat weight. He writes the new line and asks the player to read it back. He does not call the method permanent. The scene shows a familiar hand doing careful work and one named risk: if Rade is away, no one else may know the full record.

This is not a failure ending for choosing central stewardship. It may be the most workable short-term method. The cost is concentration of knowledge and possible overload. A follow-up can invite a second person to learn the process, but only with their consent. If the player used a military format, Rade records which fields the shelter accepted and which it left blank. If the player chose no outside role, the same steward model can operate with local copies only.

### 85.5 “The Shelf and the Wall” — restricted archive with public current copy

**Entry conditions:** the player accepted archival support or created a local restricted record; a current copy remains available; retention terms are understood.

The Archivist returns a sleeve containing the dated copy. She confirms its source and states the agreed retrieval or review date. Rade posts only the current schedule. One resident asks why the history is not on the wall; another says the wall has enough information already. The player can answer through the agreed privacy rule or ask the Archivist to explain her custody terms.

The strength is separation of immediate use from longer history. The cost is dependence on the archive’s terms and the possibility that retrieval takes time. The ending must say who can request the copy and how a correction can be attached. It must not suggest that archival custody makes the account true. If the player declined the Archivists, this ending is unavailable, but a local restricted copy may create a similar practice with different upkeep.

### 85.6 “A Route, Not a Promise” — communication through a bounded courier path

**Entry conditions:** the player used Osha, Long Walk, or another agreed messenger for at least one consequential notice and distinguished attempt from confirmation.

Osha marks two points on the route card: where the message left the shelter and where a recipient acknowledged it. If acknowledgment never came, the second point stays blank. The player can ask whether she wants to carry future messages, rotate the route, or train another willing courier.

The strength is visible delivery work and the ability to reach people away from the board. The cost is travel, delay, exposure, and dependency on available messengers. The ending names a fallback if the route is closed. A Long Walk report can supply route timing but not local consent. Faction representatives may request priority delivery; the player can accept only specified messages or refuse the request.

### 85.7 “The Board Can Wait” — direct confirmation before each shift

**Entry conditions:** the player and group did not adopt a durable public schedule but agreed to check assignments directly at a recurring point.

The shelter gathers around a table at the start of a shift. One person reads the work need, each affected person responds, and Rade writes only what was accepted. The meeting takes time and can exclude someone who cannot attend. The player can establish a proxy rule or a way to send a response later, but a proxy cannot decide for another person without authorization.

The strength is that assignments are confirmed close to the moment they occur. The cost is coordination time and reduced usefulness for people who need advance notice. The ending should show whether the group has a second channel for those people. This route suits a player who distrusted stale paper but is not universally better than a reliable posted schedule.

### 85.8 “Until the Next Light” — temporary emergency practice

**Entry conditions:** immediate work is covered, but the group did not settle the lasting rule; a specific next review is named.

The final scene has one usable temporary sheet, a returned tool, and a note that the arrangement expires. One character asks who will convene the next review. The player can name a willing person, volunteer themselves for a bounded follow-up, or leave the task open with a journal reminder if the current quest system supports one.

The strength is honesty about limited capacity. The cost is that the issue can reopen and the temporary method may fail before then. A faction may offer a lasting solution, but the player can decline. A side quest may remain available as a follow-up rather than being silently marked complete.

### 85.9 “The Disagreement Stays” — unresolved history, usable present

**Entry conditions:** at least one key account remains disputed; the player chose not to force an agreement; the next practical shift has a workable plan.

Two versions remain visible or are stored separately according to the chosen access rule. The people involved do not shake hands for the player’s benefit. One agrees to the immediate task, one does not, and a third person sets a date for another conversation. The ending is emotionally complete because it accurately names the relationship and the work; it does not need consensus.

The cost is uncertainty and the risk that the disagreement returns. The strength is that no one has been assigned a false confession or pressured into a public explanation. A later callback can reopen the discussion if a relevant event occurs. This ending can appear on any faction route.

### 85.10 “Take the Board Down” — replace the method

**Entry conditions:** the group concluded that the physical board is inaccessible, unsafe, too costly, or not trusted; a replacement practice has been tried or a valid temporary method exists.

The player and Mina unfasten the board. The old paper does not disappear; it is retained or discarded under the agreed record rule. The wall shows a lighter rectangle where it stood. The replacement might be a smaller posting point, a direct-confirm routine, a second location, a radio update with acknowledgment, or a combination.

The strength is refusing to maintain a tool that no longer serves people. The cost is the labor and uncertainty of change, plus any new dependency. If no replacement was tested, the ending cannot call the method solved; it becomes a temporary discontinuation with a named next step.

## 86. Ending assembly rules

An ending is assembled from a primary operational resolution, action readback, character arc beat, and at most one foreground service or faction overlay. It should not enumerate every recorded event. The selection logic can be designed as a content table later, but each combination must be based on observable state and current authority.

### 86.1 Primary resolution and overlays

Choose the primary ending from the method that is actually in use. Then add no more than three overlays:

1. **Action readback:** one early action and one later repair, if both are consequential.
2. **Character beat:** the most relevant accepted boundary, delegated task, or unresolved relationship.
3. **Outside relationship:** one current faction service or the fact that no recurring service was accepted.

Other outcomes can appear as prop changes or journal lines. This keeps the ending readable while preserving broad history.

### 86.2 Ending precedence

If a temporary arrangement is the only one actually used, the ending must not select a permanent-method resolution because the player verbally discussed one. If a board is still stale, choose a current-state ending that shows the stale copy, even if the player intended to update it. If no one accepted recurring responsibility, do not choose the rotating or single-steward ending. If a faction agreement expired, it is not an active service. If the central historical dispute remains unresolved, do not use dialogue that implies everyone agreed.

### 86.3 A missing fact is not a negative fact

No saved confirmation does not mean the person refused. No record of a public update does not prove nobody heard it. No item receipt does not prove the supply never arrived unless the inventory authority confirms that. Endings should differentiate not recorded, not observed, not completed, and explicitly declined.

### 86.4 Outcome visibility

Before the final scene, give the player a short status summary through an existing journal or quest presentation path if the current UI supports it. The summary can say: “Current list: posted. Correction method: not agreed. One notice: unconfirmed. Repair: temporary through next shift.” It should not expose internal variable names or score calculations. If no suitable UI exists, the final conversation should establish these facts naturally before presenting the last action.

## 87. End scene dialogue variants by practical outcome

These are example lines for the final group scene. They should be assigned only when their prerequisite is true.

**Current and reviewable**
- Rade: “The current line is here. The earlier one is still beside it, with the date.”
- Mina: “I can keep the mount. Someone else needs to learn the check.”
- Osha: “The copy got where it needed to go. The return note took another run.”

**Low disclosure**
- Affected worker: “I know when I’m expected. I don’t need the wall to say why.”
- Rade: “The short version is public. The rest is where we agreed to keep it.”
- Liaison: “I have a coverage count. I don’t have the names.”

**Rotating maintenance**
- First maintainer: “I’m done for this turn. Ask me again before the next one.”
- Second maintainer: “Show me the part you usually check. Don’t just hand me the key.”
- Mina: “That is a rotation if both of you keep saying yes.”

**Temporary practice**
- Osha: “This gets us through tomorrow. It doesn’t get us through next week.”
- Rade: “I wrote the review date in the same place as the shift.”
- Worker: “I can come back to the table. I can’t promise what I’ll say.”

**Unresolved dispute**
- First worker: “I still say I didn’t agree to the full watch.”
- Second worker: “I still remember you saying you’d cover it.”
- Rade: “The current plan doesn’t depend on us settling that tonight.”

**Discontinued board**
- Mina: “The wall can dry out now.”
- New arrival: “Where do I check?”
- Osha: “At the table before the shift. If you can’t get here, tell us where to send the answer.”

A line from one variant must not be reused as though it proves another outcome. For example, “The current line is here” does not fit an ending where the group removed the board and has not set a replacement.

## 88. Character-specific ending variations

The primary resolution remains the same, but the cast’s personal arcs can produce different final exchanges.

### 88.1 Rade variation

If the player helped Rade make corrections visible, he may hand the pen to another person without guarding the old copy. If the player repeatedly changed the record without readback, he may ask to inspect any future edits before posting them. If he withdrew from stewardship, the scene can show him sitting away from the board and feeling relief without implying he has stopped caring. If no personal conversation occurred, Rade reports the practical state and nothing more.

### 88.2 Osha variation

If her routes were confirmed but bounded, she leaves one route slip at the desk and keeps the next message for herself. If she was assigned without consent, she may decline another route and name the unreturned task that makes her unavailable. If the shelter adopted a shared relay, she trains someone only if both agree on the scope. If a faction claimed her work, she can request attribution or privacy.

### 88.3 Mina variation

If the player respected repair limits, Mina writes the next check date and gives the tool to a named willing learner. If a temporary brace was described honestly, she tests it without calling it permanent. If her work was repeatedly assumed, she may refuse the next repair until the group assigns it explicitly. If she chose to step away, the group shows what capacity is missing and what alternate method they adopted.

### 88.4 Minor-role variation

Iven may move the review away from meal preparation; Sera can identify whether a reserved item was used; Pell can confirm one observed event without interpreting it; Dema can use the final process as someone who did not learn the old routine. These callbacks help the shelter feel broader than the three principal voices. They remain optional and should not compete with the core resolution.

## 89. Ending and epilogue navigation

The player should be able to understand what action follows from the ending. If the shelter has a review date, the journal or current quest owner may retain a follow-up hook. If the story is complete, the ending can still leave a diegetic open future without manufacturing an endless quest marker.

Possible continuation hooks:

- A trial practice reaches its review date and asks whether to renew.
- A second copy is found with an older date.
- A worker trained for one repair accepts a second task only after a new conversation.
- A supporting current’s bounded service expires and the shelter must decide whether to renew.
- A newly arrived person asks how to receive schedule changes.
- The board fails during bad weather and the shelter uses its fallback.
- A faction representative challenges the public account of the arrangement.
- The worker whose history remained disputed returns with new evidence or declines further discussion.
- A route closes and forces a new delivery method.
- A local tool or material shortage tests whether the group can maintain the chosen process.

These hooks should enter through their owning quest, encounter, faction, or resource system. Do not keep Plan 97 perpetually active through a panel-local timer. A completed story can have a reusable practice and occasional callbacks without a permanent objective.

## 90. Ending diversity and replay evaluation

To evaluate whether endings are genuinely distinct, compare them on concrete dimensions rather than prose length:

| Dimension | Questions |
|---|---|
| Current coordination | What does a person do to learn their assignment? |
| Correction | Who can change an entry and how is that change communicated? |
| Record scope | What history remains, who can see it, and for how long? |
| Maintenance | Who checks the board or replacement method? |
| Capacity | What breaks if the named maintainer is absent? |
| Outside relationship | What service, pressure, or dependency remains? |
| Character outcome | Which boundary or responsibility changed for a named person? |
| Unresolved cost | What still requires labor, supplies, trust, or a future decision? |
| Player action | Which concrete choice from earlier play does the ending remember? |
| Continuation | What event could reopen the arrangement without invalidating the ending? |

If two ending drafts differ only in faction vocabulary and all ten dimensions are identical, they are likely the same ending with alternate labels. They may remain as route dialogue variants, but should not inflate the count of branching endings. Conversely, two endings can share the same central practice and still differ meaningfully if one preserves private records, uses a courier route, carries an outside service expiry, or acknowledges a specific unresolved dispute.

A playthrough should be capable of reaching at least three significantly different operational resolutions without changing moral alignment: for example, a current public record, a privacy-limited direct-confirmation process, and an unresolved but usable temporary plan. It should also be possible for two players with the same faction affiliation to reach different outcomes based on their actual actions.

## Installment 9 closing note

This installment expands the ending matrix into ten practical resolutions, defines precedence and missing-fact rules, provides outcome-specific dialogue, adds character closing beats, and lays out follow-up hooks that can continue the story without creating a permanent quest loop. No ending is a moral rank, and faction affiliation does not replace the player’s action history.
# Installment 10 — Supporting-current questlines with bounded agency

## 91. Archivists — optional sequence “The Copy With No Home”

This sequence begins when the player brings a schedule copy to the Archivists or asks what should happen to a disputed record. It can be skipped entirely. The Archivists provide custody, dating, comparison, and retrieval; they do not decide whether a person accepted a shift or which version is morally correct.

### 91.1 Entry — “Who is allowed to keep it?”

The Archivist asks where the copy came from, who may inspect it, and when it should be returned or reviewed. The player can deposit it under the narrowest agreed scope; bring an excerpt rather than the whole schedule; ask the affected people to decide together; ask for a blank sleeve and keep the record locally; or decline.

A deposit creates no claim that the content is true. If the player cannot name a source, the Archivist labels the source unknown rather than inventing one. If the player wants a person’s private explanation retained, the Archivist asks whether that person consented. A refusal to take the copy is an available outcome, not a failure state. The player can still continue the main quest.

### 91.2 First branch — “Two dates, one account”

The player returns with a second copy dated later. It changes one assignment. The Archivist can compare physical differences, note which copy arrived first, and preserve both. She cannot confirm the conversation behind the change.

The player may request a side-by-side description; ask the Archivist to keep only the latest version; ask her to attach the second source without copying personal details; or take both records back to the shelter. Keeping only the latest copy improves retrieval but may lose evidence of the dispute. Preserving both costs shelf space and retrieval work. Taking them back leaves custody local.

A later callback uses precise language: “This copy arrived one day after the other.” It does not say “this copy is correct.” A major faction may later ask to inspect the records. The player can share a summary, request consent, or refuse according to the original scope.

### 91.3 Second branch — “Correction without erasure”

A person disputes the archive label. They say “unconfirmed” makes it sound as if they failed to answer, when they had never been told. The player can ask them what wording is accurate; invite the original source to respond; add a dated correction while retaining the old label; restrict the record until both parties review it; or withdraw the copy.

The Archivist explains that an annotation can correct how the record is read without changing the fact that the earlier label existed. The affected person may choose not to provide a full account. If the player withdraws the copy, the Archivist confirms whether a receipt remains, subject to the agreed retention terms. A withdrawn document is not a secret reward and does not erase any other copies known to exist.

### 91.4 Third branch — “The retrieval request”

A resident asks for the archived copy to prepare for a meeting. The player can request the full copy under the original access terms; ask for a narrow excerpt; invite the resident to inspect it with the Archivist; refuse because the person is not within the agreed access group; or revisit the original agreement with the people named.

Each choice shows a different tension between access and privacy. If an access term was never established, the player cannot pretend the original depositor authorized broad release. The Archivist may pause retrieval until the scope is clarified. That delay is real, but it does not stop the main story. The player may rely on a new current summary instead.

### 91.5 Sequence resolutions and service boundaries

- **Restricted custody:** the Archivists retain dated copies for a defined period and allow access only to named people or by renewed agreement.
- **Open comparison:** affected residents inspect the copies together; the Archivists facilitate handling but do not decide the dispute.
- **Local return:** the documents return to shelter custody; the player assumes the upkeep and access burden.
- **Withdrawal:** the copy is removed from the archive according to agreed terms, while known derivative copies remain an open question.
- **No deposit:** the player receives advice on source labeling and keeps all records local.

The Archivists never grant a special ending, faction allegiance, or permanent authority over the schedule. Their sequence adds historical legibility and forces a conversation about who may read a record.

## 92. Long Walk — optional sequence “The Return Route”

This sequence begins when the player needs to send or verify a message across a route. The Long Walk can provide route knowledge and a return path. It cannot guarantee that a recipient is present or that a message was understood.

### 92.1 Entry — “A message needs a way back”

A courier asks whether the player wants a one-way delivery, a return acknowledgment, or a request for a reply. The player can specify only the operational change; include the source and date; ask the recipient to confirm a detail; make the message private; or decide that the trip is not worth its cost.

The courier names the route window and any current hazard known to them. A return acknowledgment requires another leg. If the player chooses a one-way delivery, later status must remain “attempted” unless another source confirms receipt. If they ask for a reply, the message may return with no answer because the recipient was absent, declined, or did not receive it. Those cases remain distinct when known.

### 92.2 First branch — “The route has changed”

A blockage closes the usual route. The courier offers a slower path, a handoff at an intermediate stop, or a later departure. The player can use a different messenger, broadcast a limited update, wait, or cancel the message.

The slower route has a known time cost. A handoff adds another person who can carry or misplace the message; its source must be named. Broadcasting reaches a different audience and may be inappropriate for private information. Waiting preserves the message but leaves current coordination unresolved. Cancelling closes the request without pretending that notice was delivered.

If the player chooses a risky route, the existing travel or exposure authority must determine any systemic cost. The content must not invent a roll. The Long Walk reports what they know and what they do not know.

### 92.3 Second branch — “The answer is not a receipt”

A returned note says, “I’ll talk after the morning work.” This confirms that the person received something but does not answer the schedule question. The player can send a clarification, wait for the conversation, set temporary coverage, ask Osha to arrange a meeting, or close the request and mark the assignment unresolved.

If the player sends another message, the route records that it is a second request. If they wait, the recipient’s proposed time becomes a future opportunity, not guaranteed consent. If they set temporary coverage, that arrangement can proceed without the discussion. The story does not turn delayed response into refusal.

### 92.4 Third branch — “Who pays the return trip?”

The shelter has used multiple deliveries. The courier explains that the return route is now taking labor from another journey. The player can purchase one more return; establish a limited recurring exchange; train a local messenger; ask the recipient to send a reply through their own route; or stop requiring acknowledgment for low-impact notices.

Each option has a different cost and reliability. A local messenger can reduce outside dependence but needs consent, time, and route knowledge. A recurring exchange must have an expiry and a fallback. Not every update needs a reply; high-impact assignments may justify confirmation, while simple notices may not.

### 92.5 Sequence resolutions and service boundaries

- **One-time confirmed route:** one message and one acknowledgment return; no ongoing obligation.
- **Known route, unconfirmed receipt:** the player knows the delivery attempt and any checkpoint but not recipient understanding.
- **Shared relay:** a local person handles some return trips after training and acceptance.
- **Priority-limited service:** the Long Walk carries only named urgent messages during a defined period.
- **Route declined:** the message stays local and the player chooses another method or proceeds with uncertainty.

The Long Walk does not become a messenger monopoly. Their route data cannot establish local consent, and no ending depends on using them.

## 93. Scavenger Guild — optional sequence “Measure Twice”

This sequence begins when the player needs a part, wants to repair the board, or asks how a salvaged item will wear. The Guild provides material knowledge, fitting, and a possible apprenticeship. It does not take over the shelter’s maintenance plan or claim ownership of the repaired object.

### 93.1 Entry — “What must the part do?”

The mechanic asks what load the bracket must hold, whether the board will be wet, and who can inspect it later. The player can show the damaged part; ask for a standard replacement; ask what salvage is available; request a demonstration; or postpone repair until Mina can inspect it.

The mechanic separates measurements from guarantees. A part can fit and still fail under repeated wet paper or a leaning hand. If the player does not know the load, the Guild will not certify it. They may offer a temporary fit with explicit limits. If the player postpones, the board remains inconvenient but nobody is charged.

### 93.2 First branch — “The parts bin”

Three candidates are available: a close-fit bracket with uncertain wear, a stronger but heavier piece, and a reusable clamp that requires daily placement. These are proposed narrative options; future implementation must map them to existing catalog entries or seek authority to add data.

The player can choose one based on cost and use, ask the mechanic to demonstrate all three, take no part, or seek a different supplier. The close fit uses fewer materials but needs inspection after rain. The heavier piece holds securely but costs more and may require another worker to install. The clamp avoids permanent drilling but has a daily labor cost and can be forgotten.

A purchase or trade goes through the existing inventory/economy owner. The quest must not spawn the selected item through a dialogue reward if the authority requires a merchant or resource command.

### 93.3 Second branch — “Learn the repair”

Mina or another willing worker can learn the fit. The player can ask for a one-time demonstration; request a paired installation; ask the Guild for a written measurement note; pay for a short lesson; or let the Guild complete the repair without training anyone.

The learner may refuse or accept only the demonstration, not ongoing maintenance. If the player delegates installation, the actual installer and inspection responsibility are recorded. The Guild can leave the shelter with a repair that works today, but the follow-up remains with a named person or a future service request.

### 93.4 Third branch — “The repair fails honestly”

After use, the temporary part shifts. No character claims it was guaranteed forever. Mina can point to the wear mark, the player can check the inspection date, and the Guild can explain whether the part failed within its stated limit or was used outside it.

The player can refit the part, replace it, return to the Guild with evidence, switch to a reusable clamp, or abandon the board and choose another posting method. If the part was used beyond its stated conditions, the story says so without accusing the player of malice. If it failed under the agreed use, the Guild can honor the agreed replacement terms or dispute the assessment; either response becomes a separate consequence.

### 93.5 Third branch — “Keep the method local”

The Guild offers to take future repair requests. The player can accept one follow-up only; ask for a parts list to buy elsewhere; establish a price ceiling and expiry; invite Mina to maintain the board with no outside contract; or decline. A continuing contract is never presumed from a single purchase.

### 93.6 Sequence resolutions and service boundaries

- **Fitted and locally maintained:** the Guild supplies a part; a willing shelter worker owns routine inspection.
- **Fitted with a follow-up visit:** the Guild returns once on a date or event trigger; there is no automatic recurring service.
- **Temporary clamp:** the board works without permanent repair but requires a daily placement owner.
- **Repair apprenticeship:** a named learner accepted a bounded lesson and may later decide whether to continue.
- **No repair:** the player preserves materials and chooses a different coordination method.

The Guild is a support current because it makes a practical capability available. It does not decide who owns the schedule, whether the record is public, or whether the factions accept the shelter’s choice.

## 94. Supporting-current convergence and distinct consequences

The three optional sequences can intersect without becoming a single combined quest.

**Archivists plus Long Walk:** the player can send a copy for remote review and ask the courier to return an acknowledgment. The archive can confirm custody when the copy arrives; the courier can report a route attempt. Neither fact proves the intended reader agreed with the contents.

**Archivists plus Guild:** the Guild repairs a storage box or shelf for copies, while the Archivists set custody terms. The repair improves handling but does not authorize broader access. If the shelf is damaged, the shelter can retrieve records or pause the deposit.

**Long Walk plus Guild:** the Guild supplies a protected container for a route document. The container reduces physical damage if supported by the item system, but cannot guarantee delivery. The player can also choose a lighter message and accept more ambiguity.

**All three declined:** the central arc remains available. The shelter may use local copies, local repair skills, and direct conversations. The ending clearly shows lower external support, but the route is not a hidden failure path.

**All three accepted:** each agreement remains bounded. The Archivists hold a record; Long Walk carries one message; the Guild repairs or supplies a part. None owns the others’ duties or becomes an automatic bundle. The player must individually agree to future renewals.

## 95. Supporting-current acceptance and refusal matrix

| Current | Service accepted | Explicit refusal | Partial service | Expiry or renewal | What this current cannot decide |
|---|---|---|---|---|---|
| Archivists | Dated storage, comparison, or retrieval | Keep the record local or decline deposit | Store excerpt, not full copy | Retention/review date | Truth, consent, schedule assignment |
| Long Walk | Delivery, route timing, or return request | Use another channel or proceed without notice | One-way attempt, no acknowledgment | One trip or agreed window | Recipient’s intention or local approval |
| Scavenger Guild | Part, fitting, demonstration, or repair | Preserve stock or use another method | Temporary brace or advice only | One visit, trial, or priced renewal | Maintenance ownership or record policy |

The acceptance state must name what the player received, not merely that they spoke to a representative. A declined offer remains declined even if a later different service is accepted. A partial service is not expanded by dialogue implication. The player can ask for terms before accepting, and the representative can decline a counteroffer.

## 96. Cross-current scene — “Three receipts”

A table has three scraps: an Archivist custody slip, a Long Walk route receipt, and a Guild part label. They each describe a different part of the same week. The player can lay them beside the current board, ask each representative to explain what their receipt confirms, keep them separate, or leave one with its owner.

The Archivist receipt says when a copy was deposited. The route receipt says when a message reached a checkpoint. The part label says when a bracket was fitted and under what use limit. None says the shift was accepted. A new arrival asks which paper is the schedule. The player must point to the current operational record or explain that the shelter has not settled one.

If the player keeps all receipts, the ending has a richer audit trail but more custody work. If they return them, the shelter holds less paper and depends on those currents to retrieve records. If they separate the documents, the story shows a clear division of authority. If they mistakenly call one receipt proof of agreement, a character corrects them and the final report can be amended.

The scene embodies the core support-faction boundary: external groups can contribute evidence and capability, but no stack of outside receipts replaces the people who must live with the arrangement.

## 97. Optional supporting-current replay paths

The player can encounter any one of these currents without committing to the others. Route paths include:

- **Archive-first:** preserve a source copy, then negotiate a local practice with the shelter.
- **Route-first:** solve a communication delay, then choose whether any record should remain.
- **Repair-first:** restore physical access, then decide who may update the board.
- **Local-first:** decline support initially, then accept a specific service after its terms change.
- **No-support:** use local skills and accept a slower or less durable result.
- **Multi-service but no patron:** accept several individual services while refusing any recurring ownership.
- **Support withdrawal:** end one agreement while continuing another.
- **Changed circumstances:** revisit a refused service when urgency, available supplies, or route safety changes.

A supporting current’s branch availability should be based on service context and recorded agreement, not hidden affection. A player who declined archival custody can still buy paper from a different source. A player who refused a courier trip can later ask Long Walk for a route report. A player who declined Guild maintenance can still request a one-time demonstration. The previous decision is acknowledged, but does not lock unrelated future choices.

## Installment 10 closing note

This installment develops three optional support-current questlines with multi-step branches, refusal and partial-service paths, bounded endings, and cross-current combinations. The Archivists provide custody without truth, Long Walk provides route information without consent, and the Scavenger Guild provides repair capability without ownership. Their roles enlarge the player’s options while leaving the central decisions with the player and shelter cast.
# Installment 11 — Secondary questlines and the shelter beyond the main cast

## 98. Secondary questline design rule

The main board arc has three principal characters, but a shelter is more than three people and one representative per faction. Secondary questlines let other residents affect the practical outcome without displacing the player or turning every bystander into a faction leader. Each arc should have a clear local need, multiple action approaches, a bounded resolution, and one or more main-quest crosslinks. The provisional names and functions introduced below require a collision and canon review before use.

A secondary character can provide evidence, capacity, a cost, or a different interpretation. They should not unlock the only correct ending. The player may finish the central arc without completing any secondary arc. If a secondary quest changes a shared procedure, the main ending must reflect the accepted procedure; if it only changes one relationship or service, its result should remain local.

## 99. Rade’s secondary quest — “The Page He Kept”

**Premise:** Rade has kept an old page from an earlier shift arrangement. It contains a partial record and an annotation written by someone who is no longer at the shelter. He has not shown it because he does not know whether it helps or harms the person named.

### 99.1 Entry and first disclosure

The quest can begin when the player asks why Rade wants to preserve the damaged board, finds the page during an archive conversation, or sees Rade remove a loose paper from a drawer. Each entry changes who first explains the page. Rade may show it directly, the Archivist may mention that he has not decided whether to deposit it, or the player may discover only that an older record exists.

The player can ask what the page means to him; ask who is named; ask whether the named person has consented; offer to help compare it with current records; or say it is his choice whether to share it. The page is not automatically transferred to the player’s inventory. Rade keeps custody until he chooses otherwise.

### 99.2 Branch — preserve, summarize, or destroy

Rade can choose to preserve the full page locally, ask for a restricted archive sleeve, make a narrow summary that omits a personal detail, return the page to the person named, or destroy it under an agreed process. The player can recommend an option and explain its cost, but cannot choose for Rade.

Preserving the full page may support a later correction and may also expose private information. A summary is easier to share but loses context. Returning the page respects the named person’s control but may leave the shelter without a copy. Destruction can be valid if the page has no continuing purpose and the named person agrees; it is not a shortcut that erases copies already made. If the person named cannot be reached, the player and Rade must decide what can safely be done without pretending consent exists.

### 99.3 Branch — the annotation is disputed

A later conversation reveals that the annotation can be read in two ways: it may record an accepted shift or only a request. The player can compare handwriting and date with other sources; ask the named person if available; keep both readings in a note; ask the Archivist to describe the material but not interpret it; or leave the annotation unresolved.

If a new source clarifies the line, the record receives a dated correction. The earlier uncertainty remains in the history. If no evidence emerges, the ending can still be complete: Rade has chosen a retention and access practice, even though the annotation is not resolved.

### 99.4 Resolution and main-arc crosslink

Possible resolutions are a restricted local record, a source-labeled summary, a returned page with no retained copy, a mutually agreed destruction, or an unresolved page kept sealed until a review date. The main arc may then reuse Rade’s chosen handling practice when it decides what to do with the wet board copy. The player receives no moral reward for preserving or destroying the page. Rade’s personal result is that he no longer holds it without a defined reason and boundary.

## 100. Osha’s secondary quest — “The Note That Came Back”

**Premise:** Osha returns from a delivery with a folded note. The recipient was absent, and the sender is not available. Osha has not opened it. The player has to decide whether to help reconnect the message without converting Osha into an unconsenting information channel.

### 100.1 Entry variations

The quest can begin with Osha putting the unopened note on the table; the recipient asking if anything came back; or another resident seeing the delivery mark and assuming the message was read. The player can ask Osha what she observed, ask the recipient whether they expect a message, locate the sender, keep the note sealed, or arrange a new attempt.

Osha states the known facts: the route she took, where she left the note, and why it returned. She does not know the content. Opening the note is not a default option. The sender’s consent and any agreed delivery terms matter.

### 100.2 Branch — return, wait, or redirect

The player can ask Osha to return the unopened note to its sender; hold it for the recipient; deliver it to a named safe location if the sender authorized that; ask the recipient whether they want a second attempt; or wait until both parties can be reached. If the shelter uses a locked or restricted area, it must be an existing world interaction or a future approved content object, not a fabricated inventory container.

A return trip consumes route capacity if the travel authority models it. Holding the note creates a custody responsibility with a date. Redirecting it may expose the message to another person. Waiting leaves the question unresolved but preserves the envelope. The player can also tell the sender that Osha completed the attempt without asking her to carry a second message.

### 100.3 Branch — courier boundary

Osha tells the player that she is willing to carry future messages only when the sender gives a return instruction. The player can help her propose a simple route form; ask whether the shelter can use a different messenger; arrange an opt-in rotation; or tell her the current route pressure makes the service unsustainable.

The route form records delivery attempt, recipient response if volunteered, and return instruction. It does not record message contents. A rotation must be accepted by each volunteer. Osha can decide to carry urgent messages only, stop carrying private messages, or continue under the new rule.

### 100.4 Resolution and main-arc crosslink

The note can reach its sender, remain sealed for the recipient, be delivered under a revised instruction, or remain undelivered after the parties cannot be contacted. The main quest can reuse Osha’s distinction between route receipt and person receipt. A player who never completes this side quest still sees that distinction in the main acts; the side arc makes it personal rather than mandatory.

## 101. Mina’s secondary quest — “A Lesson Has an End”

**Premise:** A shelter resident wants Mina to show them how to repair a bracket. Mina is open to a lesson but does not want one demonstration to become permanent assignment. The player can help set a fair scope.

### 101.1 Entry and role negotiation

The learner, Mina, or a failed temporary repair can trigger the quest. The player can ask the learner whether they want to learn; ask Mina what the lesson covers; propose a paired repair; set a one-time demonstration; or choose an outside repair. The learner can accept a lesson but refuse future maintenance. Mina can agree to teach one step but not to certify independent work.

The lesson has a practical scope: inspect the bracket, place a temporary brace, replace a fastener, or identify when a repair is beyond local skill. The scope should be clear before the lesson begins.

### 101.2 Branch — observe, perform, or stop

The learner can watch Mina perform the repair; perform one step under her guidance; explain the check back in their own words; or stop after discovering that the work is uncomfortable or unsafe. The player can arrange extra light, obtain a compatible part, or call the Guild for a demonstration. No route labels the learner incapable because they stop.

If the learner performs a step, the game records what they actually did. Completing the step does not make them the new repair owner. Mina may ask them to decide whether they want another lesson before scheduling one.

### 101.3 Branch — the follow-up request

Later, the board needs inspection. The player can ask Mina, ask the learner if they previously accepted follow-up, ask for another volunteer, or postpone the inspection with a visible temporary limit. The learner may accept a single check, accept a bounded rotation, or decline. Mina may return as a reviewer but not as the default permanent supervisor.

If the player assumes that training created consent, the learner corrects the assumption. This creates a relationship callback and may require the player to find other coverage. The quest does not silently punish the learner or erase their prior effort.

### 101.4 Resolution and main-arc crosslink

Possible resolutions are a one-time lesson; a paired repair with two named check roles; a rotating maintenance trial; outside service for specialized work; or no training, with the task still assigned to someone who accepted it. The main quest can then choose a realistic repair ending based on who is available, not on a generic “maintenance unlocked” flag.

## 102. Dema’s secondary quest — “Nobody Told Me That”

**Premise:** Dema is new to the shelter and follows the posted schedule literally. They arrive at a closed room because an update was only spoken to the regular group. This quest is about onboarding and legibility, not making the newcomer responsible for learning hidden customs.

### 102.1 First misunderstanding

Dema finds the old line and assumes it remains valid. The player can explain the correction; show the new record; ask who told Dema the old time; apologize for an inaccessible update; or help Dema choose a reliable way to receive future changes. The player should not say “everyone knew” unless the event history confirms it.

Dema may be frustrated, embarrassed, or practical. Their response depends on what the player actually does and whether the next work task is still available. A clear explanation can resolve the immediate confusion without deciding whether the whole posting system works.

### 102.2 Branch — orient or revise

The player can walk through the current schedule; ask Dema to repeat back where they will check; ask Dema what format works for them; invite them to propose a newcomer instruction; or let them decline future work until the process is clearer. Dema can choose a board copy, direct notice, or a specific contact, but no option should require them to disclose a private reason.

If Dema proposes a simple key such as “current for next shift” and “old copy,” the player can add it to a trial board label. The group must then accept and maintain the label. Dema’s suggestion does not automatically rewrite the full shelter practice.

### 102.3 Branch — how much history to explain

Dema asks why two residents disagree about a past shift. The player can give the practical current plan; share the dispute with consent; explain that the history remains unresolved; or tell Dema where to ask if they want more detail. The player should not force a newcomer to mediate an established relationship.

### 102.4 Resolution and main-arc crosslink

Dema can become a confident user of the current procedure; receive a tailored direct notice; help test a simpler label; or decide not to accept further shift work until the method improves. Their ending makes a strong accessibility readback: if a new resident cannot tell what is current, the process is not yet self-explanatory. The main ending may use Dema as a test reader only if they agreed.

## 103. Iven’s secondary quest — “The Hour Before the Meal”

**Premise:** A meeting or roster review has repeatedly overlapped with meal preparation. Iven can rearrange the kitchen rotation once, but recurring changes may move labor onto another person.

### 103.1 Entry and immediate choice

Iven finds three people waiting to discuss the next schedule while the pot needs attention. The player can delay the meeting; ask someone to cover the kitchen; make a brief decision about urgent coverage and defer the broader discussion; move the meeting; or ask Iven what can safely wait.

The meal is a shared need, not an emotional reward. If the current needs and food systems model meal timing, the quest reads those owners. If not, it remains a narrative conflict about labor and routine, without creating a new food counter.

### 103.2 Branch — single exception or new practice

The player can ask for one exception; move all future reviews away from meal time; rotate who misses part of the meal; use a short agenda and let some people leave; or keep the current time while acknowledging that not everyone can attend. Each option redistributes time. Iven can accept, counteroffer, or refuse recurring changes.

A short meeting can still exclude people who need more time. A new time can conflict with maintenance or travel. A rotating schedule can distribute burden but be harder to remember. If the player chooses the existing time, the journal must note who was absent; the resulting rule is not unanimous.

### 103.3 Resolution and main-arc crosslink

Possible resolutions include a single moved meeting, a rotating meal cover, a shorter urgent check followed by a later full review, or an explicit record that some residents could not attend. The main ending should use this fact if it claims that everyone participated. A recurring review time is not valid unless someone accepted the task of reminding people.

## 104. Sera’s secondary quest — “Reserved Is Not Available”

**Premise:** Sera has counted the shelter’s writing supplies and found that several sheets are already reserved for medical labels and route records. The player sees the difference between an item physically present and an item available for this quest.

### 104.1 Entry and inventory evidence

The player asks for paper and Sera says there is a stack, but not all of it is free. The current inventory owner must be the evidence source if implemented. Sera can explain reservations and ask what the player needs the sheet for. The player can spend available stock; ask the group to reprioritize; use a reusable surface; trade for more; or wait.

The choice is not a secret rationing moral test. The player can inspect the listed uses and choose which consequence to accept. If the stock count is unknown or unmodeled, Sera should describe a narrative shortage without pretending it matches a numeric inventory state.

### 104.2 Branch — substitute materials and quality

Sera offers a reused package liner, a slate-like board, or a short sheet with enough room for one shift. Each has a visible limitation: smudges, fixed location, or low detail. The player can test readability with Dema, ask Mina about mounting, use oral confirmation, or decline the substitute.

A substitute can work for one purpose and fail for another. A package liner can carry current assignments but should not be archived as durable history. A reusable surface needs an erasure and copy procedure. A short sheet forces the player to limit detail. Sera does not decide which information is private or operationally necessary.

### 104.3 Resolution and main-arc crosslink

The shelter can reserve stock for other needs and adopt a reduced schedule format; spend stock on a dated public copy; establish a short-term trade; or wait for resupply and use spoken updates. The main ending names the supply dependency and chosen fallback. If Sera’s inventory evidence changes later, the narrative updates through the current authority.

## 105. Pell’s secondary quest — “Knock, Wait, Write”

**Premise:** Pell is the night watch volunteer who can confirm what he personally observed but cannot interpret silence. A message was left at a door after a knock, and another resident reports that no one heard it.

### 105.1 Evidence and witness limits

The player can ask Pell when he arrived, how many knocks he made, whether he heard movement, and whether anyone answered. Pell’s account is bounded by sight and hearing. He can describe the door, light, and route conditions. He cannot say the recipient knowingly ignored the message.

The player may place Pell’s statement beside the returned delivery note, ask the recipient what they experienced, ask for another attempt, or mark both accounts without resolution. A faction representative may request Pell’s name as a witness. The player can ask Pell before sharing it.

### 105.2 Branch — recording the night

The player can use a time-only note; add Pell’s name with consent; record the attempt without a witness name; ask Pell to make a statement in his own words; or avoid a written record and rely on the current message system. Naming him can support review but also creates exposure and a future burden. Leaving his name off can protect him while making later confirmation harder.

Pell can decline recurring witness duties. If he accepts one statement, that does not make him a permanent night clerk. The next scene checks whether anyone treated one night’s observation as proof of a general pattern.

### 105.3 Resolution and main-arc crosslink

The night’s delivery remains confirmed, attempted, or unresolved according to actual evidence. The main quest can use Pell’s account to distinguish “knock happened” from “message received.” If no one asks him, the branch remains absent; his role does not make him an omniscient narrator.

## 106. Shared side-quest and main-arc connection map

| Secondary arc | Main-act connection | Specific fact it can add | Fact it cannot decide |
|---|---|---|---|
| The Page He Kept | II, VI, VII | Retention choice and source history | Meaning of disputed acceptance without evidence |
| The Note That Came Back | II, IV, VI | Attempt, custody, and return instruction | Recipient consent or understanding |
| A Lesson Has an End | V, VII, VIII | Accepted repair scope and future owner | Permanent maintenance consent |
| Nobody Told Me That | I, III, VII, VIII | Newcomer’s access and current-version understanding | Whether all residents agree |
| The Hour Before the Meal | IV, VII, VIII | Meeting access and labor overlap | Priority of every survival need |
| Reserved Is Not Available | II, III, V | Supply reservation and chosen substitute | Item counts outside inventory authority |
| Knock, Wait, Write | II, IV, VI | Witnessed attempt and knowledge limit | Why the recipient did not respond |

The player can finish the main arc without these side quests. Where one is completed, the ending may include a short callback that shows the contribution. A side quest must not secretly add a mandatory consent condition to the main route after the player skipped it.

## 107. Secondary cast cross-arc outcomes

Secondary arcs can produce combined outcomes, but the plan should keep them readable:

- Dema uses a current board label while Sera’s paper shortage forces a shorter format. The shelter tests both clarity and supply use together.
- Iven moves the meeting outside meal time while Pell asks for a night review. The player must decide whether one recurring time can serve both or whether two channels are needed.
- Sera reserves archive paper while Rade asks to preserve an older page. The player can retain the essential source summary rather than duplicating every sheet.
- Osha’s returned note intersects with Pell’s witnessed knock. The evidence confirms an attempt but does not reveal whether the recipient heard.
- Mina trains a learner while Dema asks how to interpret the repair date. The shelter must explain both the physical limit and the posting label.
- Iven asks the player to keep meal labor visible while a faction representative requests a coverage report. The player can share a work total without exposing private reasons or meal assignments.

These are optional overlap scenes, not a mandatory multi-quest chain. They should occur only when their prerequisite facts are already present and the characters are available. If two scenes compete for the same moment, select the one with the more urgent consequence and preserve the other for a later opportunity.

## 108. Side-quest ending discipline

Every secondary quest has an ending that can be expressed in three parts: what changed, what remains the character’s choice, and what the main quest can legitimately use. For example, after “A Lesson Has an End,” the change may be that a second person knows how to inspect the temporary brace; the learner retains the choice to accept future work; and the main quest can use only the one inspection they accepted. This structure stops optional content from generating broad state that the story cannot justify.

The side-quest journal should not declare “Rade’s page resolved” merely because the player saw it. It can say “Rade chose restricted local custody; the annotation remains disputed.” It should not say “Osha’s route is fixed” after one delivery. It can say “The unopened note remains at the desk pending the sender’s instruction.” It should not say “Mina has a replacement” after one lesson. It can say “One repair step was demonstrated; no recurring duty was accepted.”

## Installment 11 closing note

This installment adds seven secondary questlines for Rade, Osha, Mina, Dema, Iven, Sera, and Pell, plus crosslink and resolution rules. Their personal choices deepen the shelter’s routines and make absent perspectives matter, while remaining optional and bounded. The new supporting roles are provisional until they pass content collision review.
# Installment 12 — Camp-wide events and intersecting questlines

## 109. Event packages: several pressures, one player-owned response

These event packages bring multiple characters and systems into the same playable moment. They help the expansion feel like a community routine rather than a chain of isolated conversations. Each can intersect with the main arc and one secondary quest, but none should require every side story to be active. Where an event depends on weather, power, water, or travel simulation, the live authority must be checked before implementation. The narrative version can still work as a conversation when no systemic signal exists.

Each package has an immediate need, a limited time window, affected people, evidence the player can inspect, and a result that leaves a trace. It also needs an exit for refusal or inaction. The player must not be forced to settle every disagreement before leaving the scene; a temporary operating plan is often more truthful.

## 110. Camp event — “The Board Goes Dark”

**Situation:** the light over the board fails during an evening handoff. Several people need the next shift information, and a message update is due. The cause might be a local lamp, a shelter power interruption, a damaged connection, or simply the bulb reaching its limit. Which cause applies must come from the existing power or item owner if it is modeled.

### 110.1 First response

The player can move the board to a lit room; use a safe portable lamp if one is available; read the schedule aloud to the people present; postpone the handoff until light returns; or have the person responsible for lighting check the source. Each path trades access, privacy, time, and work disruption.

Moving the board makes information visible to a different audience. Reading aloud reaches present listeners but may expose personal details. A portable lamp consumes an existing resource, if applicable. Waiting is safe when no immediate task depends on the schedule, but it can delay a worker who is preparing to leave. Checking the source uses a character’s skill and may postpone another repair.

### 110.2 Information branches

If the player reads all names and assignments aloud, an outside representative may overhear. The player can pause, ask who is present, continue with only task changes, or move the conversation. If the player reads only the current task, someone may still need a direct private update. If they move the board, a resident who relies on the old location can miss the update unless a new notice is given.

A short-term spoken handoff can be logged as an event with named listeners only if they acknowledge it. Being in the room is not proof of hearing. The encounter can give an optional repeat-back: each person states what they understood. A mismatch discovered here can be fixed before leaving.

### 110.3 Repair and follow-up

Mina can inspect the lamp or connection if she is available and accepts the work. The player may ask for another worker to check it, request a Guild part, or use the temporary readback method. If Mina says the lamp will work only through the next handoff, the player should see that limit in the journal or visual state.

Possible outcomes include board relocated with a new access note; one-shift oral handoff; temporary lighting with an inspection date; postponed handoff and a named waiting cost; or unresolved light failure with a safe alternate channel. The next morning, a callback checks which method was actually used. The ending should not assume the lamp was repaired merely because the player began the inspection.

### 110.4 Character callbacks

- Rade cares whether the current copy stayed intact during the move.
- Osha cares whether the absent recipients were told and whether she agreed to carry that notice.
- Mina cares whether a temporary lamp was called a permanent fix.
- Dema cares whether the new location was explained.
- Iven cares whether moving the meeting interrupted another shared task.
- Sera cares whether a portable lamp or replacement part was actually consumed.
- Pell can describe what he saw at the evening handoff, but not who read the board after he left.

This event can produce a different branch even when the player has the same faction alignment, because the action sequence and information audience differ.

## 111. Camp event — “The Delivery With Three Owners”

**Situation:** a parcel arrives with supplies and a note. The parcel was requested by one person, reserved by another, and expected by a faction representative. The label is intact, but the contents have not yet been counted. This event tests custody, trade terms, and public claims without inventing a new inventory system.

### 111.1 Establish custody

The player can ask Sera to count the contents against the current inventory process; ask the person who requested it to open it; leave it sealed while checking the agreement; invite the representative who expects it; or decline custody and return it to the carrier. The parcel’s physical holder is not automatically its owner.

If the player opens it without the requester, Sera can still record quantity, but the requester may object to the process. If they leave it sealed, the shelter waits for a witness. If they return it, the promised supply remains unavailable until another delivery. The carrier may depart, so waiting has a clear cost.

### 111.2 Allocation branch

Once contents are confirmed, the player can allocate the promised material to current schedule copies; honor Sera’s reservation for another need; split the supply and reduce detail; return a portion according to the trade; or ask the group to choose. Each outcome changes available capacity through the actual inventory owner if implemented. The narrative records why and who agreed.

If the faction representative expected a report in exchange, the player checks the actual terms rather than assuming the parcel’s presence creates an obligation. If the terms were vague, the player can negotiate now, refuse a new condition, or accept a bounded exchange. The representative may be disappointed without becoming an antagonist.

### 111.3 Competing claims

The requester says the supplies were promised to them. Sera says a portion was reserved. The representative says the delivery was meant for the shelter as a whole. The player can compare the note, ask each person what they understood, accept the current reservation pending review, or assign only the immediately needed amount. No account is resolved by a hidden faction score.

A correction can be attached to the delivery record. If the written terms do not match a prior verbal agreement, the story records both. The player can accept the parcel while disputing the condition or refuse it and seek another source.

### 111.4 Resolution

The event closes with one of four practical results: counted and allocated supply; partial allocation with a named reservation; returned parcel and open need; or accepted delivery with a disputed exchange term. A later scene checks whether a promised report, return, or resupply actually happened. No outcome silently awards faction loyalty.

## 112. Camp event — “The Room Before the Meeting”

**Situation:** the player planned a group decision, but the shared room has another use and not everyone can attend. The cause may be a meal, treatment, rest, repair, or another current task. The scene is about access to decision-making, not meeting attendance as a moral test.

### 112.1 Who can enter

The player can hold the meeting now with whoever is present; move it to a more accessible space; collect private accounts before meeting; narrow the agenda to immediate coverage; or postpone. Moving spaces may make the gathering public. Private accounts cost time and do not replace a readback. Narrowing the agenda produces an interim rule only.

Dema can report that the room’s location was never explained. Iven can say the meal task cannot wait. A character with a health or rest need can request a different time without disclosing private details. The player can ask what accommodation would help, but the person is free not to answer.

### 112.2 Participation is not unanimity

At the meeting, the player can invite each person to speak; accept written or spoken responses; ask for objections before adopting a trial; delegate a draft to Rade; or let those present decide a temporary measure. Someone can pass. Someone can leave. Someone who was absent is not automatically represented by a friend.

If the player adopts a trial, they choose its duration, affected work, maintainer, and review method. If the player chooses a permanent rule, the scene asks how absent people can challenge it. If they defer, current work still needs a clear plan.

### 112.3 After the meeting

The player can read back the decision to present people; send a summary to absent people; post the operational change without the private discussion; or ask a participant to review the wording. A summary is not equivalent to consent. The journal lists who participated and who did not only when the player observed or recorded it.

The encounter can result in an agreed trial; a temporary coverage rule with a later meeting; separate private positions and no shared policy; or a failed meeting with a direct next action. The failed meeting does not disable the main quest.

## 113. Camp event — “The Same Hour, Two Requests”

**Situation:** a major faction asks for a temporary work crew during the same hour that the shelter needs to repair or repost its schedule. Both requests are real. The player must choose, negotiate, or split the available labor.

The faction can be Military, Rebel, or Independent based on route state, or the event can feature the shelter’s own urgent work on a no-commitment route. The request states its duration and known risk. The shelter task states the consequence of delay. The player can assign a willing person to one request; ask the faction to shift its time; split work if safe; postpone both and convene people; or decline outside labor.

The affected worker’s answer matters. A person asked to leave the shelter cannot be treated as a resource token. They may accept the outside task, accept only part of it, ask for compensation, or refuse. The player’s choice can preserve local maintenance or secure a useful external benefit; neither is universally correct.

**If the faction request is accepted:** the shelter task receives a named alternate or becomes a visible delay. The faction’s later report describes what its own workers observed, not the shelter’s entire outcome.

**If the shelter task is prioritized:** the faction may withdraw or offer a new window. The player can negotiate once or close the request.

**If labor is split:** each person has a defined task and check-in time. If either worker declines the split, the player revises rather than forcing the schedule.

**If both are postponed:** the player must state which deadline comes first and who will communicate it. The faction can impose a cost or refuse, but not rewrite the player’s past action.

This event is a strong test for playstyle flexibility. A direct operator may decide quickly and then document. A scheduler may negotiate windows. A boundary setter may ask each worker first. A trader may offer an exchange. The event should support them all without assigning a class.

## 114. Camp event — “The Missing Copy”

**Situation:** the current public copy cannot be found. No one knows whether it was removed for repair, taken by a reader, or discarded. The schedule is still partly remembered by several people.

### 114.1 Reconstruct or replace

The player can reconstruct the next shift from direct confirmations; ask Rade to compare his retained version; request the Archivist’s copy; ask Osha to carry confirmation requests; or pause assignments until enough people respond. Each source has a different scope. Rade’s copy may be old. The archive may take time to retrieve. A courier can collect answers but needs route capacity. Direct confirmation may exclude someone unavailable.

### 114.2 Investigate the missing object

The player can ask who last saw it; check the repair area; ask whether anyone took it for private reasons; or decline to investigate and focus on the current schedule. A search can find the copy, but the game must not imply theft without evidence. It may have been moved, damaged, or discarded accidentally.

If found in a private room, the player asks before entering or reading further. The copy’s location does not grant consent to inspect unrelated belongings. If found torn, the player can preserve a fragment or simply create a new schedule. If no copy is found, the quest moves on with the confirmed facts available.

### 114.3 Information risks

A replacement based on partial memory can be wrong. The player can mark uncertain lines; use only assignments each person has confirmed; leave a gap; or ask the group to validate the reconstructed copy. Each route has an operational cost. The journal distinguishes “reconstructed from available reports” from “copied from an authoritative source.”

This event can lead to a changed storage practice, but not automatically. The group may choose a hook, a sleeve, a second source, or no archive. The repair belongs to the people who use the board.

## 115. Event crosslink map

| Event | Main arc acts | Side quests it can touch | Primary branch facts |
|---|---|---|---|
| The Board Goes Dark | I, V, VIII | A Lesson Has an End; Nobody Told Me That | Alternate access, temporary lighting, confirmed readback |
| The Delivery With Three Owners | II, III, VII | Reserved Is Not Available; Independent offer | Custody, quantity, reservation, actual terms |
| The Room Before the Meeting | VI, VII | The Hour Before the Meal; Nobody Told Me That | Participation, absence, trial, readback |
| The Same Hour, Two Requests | IV, VII, VIII | Mina repair lesson; faction route | Consent, task split, delay, outside terms |
| The Missing Copy | II, III, VI | The Page He Kept; The Copy With No Home | Source, reconstruction, uncertainty, access |

Events may appear in different order if prerequisites are satisfied. The player should not encounter a “missing copy” event before any copy exists. If the Board Goes Dark event fires before a faction has entered the story, use the shelter’s ordinary needs, not a faction representative. Each event should have at least one valid route to the main ending even when its optional side quest is skipped.

## 116. Reactions when multiple characters disagree

Group scenes should not reduce every conflict to one spokesperson against one player. A useful scene can contain several true but partial concerns. For the delivery event, Sera knows what was reserved, the requester knows what they were promised, and the representative knows what their organization expected. None alone controls the full account.

The player can structure the conversation: ask each person to name only the fact they observed; ask what outcome they need now; separate today’s allocation from the disputed promise; or pause the discussion. These actions can lower confusion without resolving disagreement.

A later line should reflect a character’s actual participation. Someone who left may say, “I heard you divided it, but I don’t know how you got there.” Someone who accepted the allocation may still object to the process. Someone who received no supplies can accept the explanation without changing their opinion. The story treats emotional response, factual knowledge, and material outcome as separate.

## 117. Consequence chains with delayed callbacks

These chains illustrate how a short action can echo through later content without spawning a new score.

### 117.1 Move board to a public room

1. Player moves the board to improve light.
2. Dema can now find the schedule without help.
3. A private line is visible to a faction visitor.
4. Player can narrow the public copy, relocate the board again, or accept the exposure.
5. Ending names the access improvement and privacy cost.

### 117.2 Use spoken handoff

1. Player reads the schedule aloud during a blackout.
2. Present listeners repeat back their assignments.
3. One absent worker receives no notice.
4. Player can send a later message or leave the assignment unconfirmed.
5. Ending records that spoken handoff worked for those present, not that everyone knew.

### 117.3 Accept one repair lesson

1. Learner performs one step with Mina.
2. Mina records the scope and inspection limit.
3. A later repair request arrives when Mina is absent.
4. The learner can accept the bounded task, request another lesson, or decline.
5. Ending describes acquired capability without silently assigning a permanent job.

### 117.4 Reserve paper for another need

1. Player keeps the last sheet for the existing reserved task.
2. The schedule uses a reusable surface.
3. Sera or Dema notices that the current label is hard to read.
4. Player can reduce detail, ask for a second format, or trade for supply.
5. Ending names both the preserved reserve and the coordination burden.

### 117.5 Delay the faction crew

1. Player asks the faction to move its work window.
2. The local task is completed first.
3. The faction accepts, counters, or withdraws.
4. The player’s route relationship changes based on the actual exchange, not virtue.
5. Ending identifies whether the outside service remains available.

## 118. Camp-event authoring acceptance

A camp-wide event is ready for future implementation planning only when authors can specify:

- What starts the event and which existing owner provides the trigger.
- Which characters are present and what each personally knows.
- What practical decision the player faces.
- At least three materially distinct actions, including a refusal or defer path.
- What resource or labor cost each option creates.
- Which existing system owns any mutation.
- What later scene or ending reads each produced fact.
- How the scene resolves if a character is absent.
- How the player can continue with uncertainty.
- What optional side quest it may enrich without making it mandatory.
- How the event avoids implying consent, delivery, repair, or success that did not occur.
- What visual, audio, or journal change makes the outcome observable.

An event that cannot name its future reader does not need persistent state. An event that has persistent state but no verified owner remains a proposal. A crowd scene that only changes dialogue and nothing the player can act on should be marked as flavor rather than sold as a branching quest.

## Installment 12 closing note

This installment adds five camp-wide event packages, a crosslink map, multi-person conflict guidance, delayed consequence chains, and acceptance criteria. The new events test privacy, custody, competing labor, access, and uncertainty through concrete actions. They can enrich multiple routes while preserving the player’s ability to resolve the main quest without completing optional content.
# Installment 13 — Failure atlas and recovery without erased history

## 119. Failure is a change in the situation, not a verdict on the player

A failed action should create a concrete new state: a message did not arrive, a person withdrew from a task, a repair did not hold, a supply was unavailable, or a meeting did not produce agreement. It should not secretly tell the player that they chose the wrong personality. Recovery means the player gets another informed action, not that the previous event disappears.

Each failure entry below follows the same authoring pattern: cause, immediate consequence, recovery actions, remembered fact, and routes that remain open. If the player can recover, the game should state what recovery costs. If recovery is impossible, the game should state what remains possible instead.

## 120. Failure card — correction arrives after the shift

**Cause:** the updated schedule reaches the board after the assigned worker has already acted on the old copy.

**Immediate consequence:** the worker may have spent time or supplies on the wrong task. Other people may have started coverage based on the earlier plan.

**Recovery actions:** acknowledge the delay and ask what can still be corrected; reassign the next task with the worker’s agreement; record that the old notice remained active; offer a repair such as returning a borrowed tool or updating the alternate copy; or accept that the cost cannot be recovered.

**Remembered fact:** which version the worker saw and when the correction became available. Do not set a fact that says the worker “ignored” the update unless they received it and chose not to follow it.

**Open routes:** every major-faction route remains available. The final practice can still be public, private, delegated, temporary, or unresolved. This event may lower confidence in the delivery process, but it does not close the quest.

## 121. Failure card — substitute withdraws

**Cause:** a person initially accepts a one-shift cover, then discovers a conflicting obligation or changes their mind before taking the task.

**Immediate consequence:** coverage is open again, and the task may need to be delayed or divided. The substitute is not punished for withdrawing before the work begins.

**Recovery actions:** ask whether a smaller portion remains possible; find another willing substitute; reduce or defer the task; personally take the task if the player character can; or leave a clear gap and notify affected people.

**Remembered fact:** the original acceptance was provisional or withdrawn, not completed coverage. Any other work the substitute has already performed remains real.

**Open routes:** the original assigned worker can still return; the player can continue with a gap; a support current may assist; or the faction’s timing can be renegotiated. The branch does not imply that future volunteers are unreliable.

## 122. Failure card — repair part shifts

**Cause:** a temporary or salvaged fitting moves under use, within or beyond the limit explained when it was installed.

**Immediate consequence:** the board may be hard to read or unsafe to handle. A copy could fall or become damaged. The current shift needs a different way to communicate.

**Recovery actions:** remove the part and use a temporary readback; ask Mina to inspect if she accepts; contact the Guild under an active service term; install a different part; or discontinue the board and choose another method.

**Remembered fact:** what limit was disclosed, how the board was used, who inspected it, and whether the part shifted before the expected check. Do not infer negligence from failure alone.

**Open routes:** schedule work can proceed through direct confirmation or another posting point. The repair failure can become the final reason to retire the board, but only if the group chooses that outcome.

## 123. Failure card — archive retrieval delayed or denied

**Cause:** an archived copy is not immediately available because its access terms require review, the Archivist is absent, or the record was never deposited.

**Immediate consequence:** the player cannot use that copy to settle today’s question. A retrieval request is not a guaranteed immediate response.

**Recovery actions:** reconstruct the current operational plan from available sources; ask for an excerpt if terms allow; contact the original depositor; wait for the retrieval window; or leave the history unresolved and make a temporary assignment.

**Remembered fact:** whether the copy exists, who controls access, why retrieval was delayed, and whether an alternate source was used. Do not mark the Archivists as withholding a record if the player never deposited it or lacks access under an agreed term.

**Open routes:** the core story continues without the archive. The player may later revise the retention terms or withdraw the copy, if the agreement permits.

## 124. Failure card — meeting reaches no agreement

**Cause:** attendees disagree about the posting method, or one or more affected people cannot attend.

**Immediate consequence:** no durable rule is adopted. The next shift still needs an operational plan.

**Recovery actions:** adopt a narrow temporary measure; ask each person to test a different method for one shift; collect private feedback and return to the decision; delegate a draft but not the final authority; or leave the decision open and assign immediate coverage separately.

**Remembered fact:** which people participated, what alternatives were raised, which temporary measure was accepted, and whether absent people were contacted. Attendance is not universal consent.

**Open routes:** the player can finish with a temporary or unresolved ending. A later meeting can reopen the choice without retconning the failure. Faction representatives may disagree with delay, but do not get to declare consensus.

## 125. Failure card — faction offer expires

**Cause:** the player does not accept or renew a bounded service before its stated date, or the faction changes its available terms.

**Immediate consequence:** the resource, messenger, observer, or repair visit is no longer guaranteed. A prior delivery remains part of history.

**Recovery actions:** ask for a one-time service under current terms; negotiate a new duration; seek another supplier; use local capacity; or proceed without the service.

**Remembered fact:** exactly what the faction provided, what the player agreed to, and what expired. Do not treat discussion as acceptance. Do not erase the benefit of an earlier completed service.

**Open routes:** all action-based choices remain. A route overlay changes because the service ended, not because the player became “disloyal.” If the faction refuses renewal, its refusal is a current event that can be answered.

## 126. Failure card — courier returns without the message

**Cause:** a route is blocked, the recipient is absent, the delivery instruction is unclear, or the courier chooses not to continue under changed conditions.

**Immediate consequence:** the recipient has not received the message, or receipt is unknown. The original work remains affected.

**Recovery actions:** clarify the instruction; use another route; ask the recipient to initiate contact; leave an agreed notice; send a second attempt; or proceed with a provisional plan.

**Remembered fact:** departure, checkpoint, handoff, attempted delivery, and acknowledgment are distinct. The route does not set “received” just because Osha or Long Walk completed travel.

**Open routes:** direct discussion, delayed assignment, and local board methods remain available. The courier may decline another attempt without being cast as an antagonist.

## 127. Failure card — supply count differs from promise

**Cause:** a package is short, damaged, reserved, or counted differently from the agreement.

**Immediate consequence:** fewer supplies are available than expected. A current task may need to use a substitute or wait.

**Recovery actions:** compare the promise and receipt; accept a partial delivery with revised exchange; request the missing quantity; return the package; or reprioritize stock with affected residents.

**Remembered fact:** the physical quantity recorded by the inventory owner, the agreed quantity, and any unresolved discrepancy. A promise is not an item, and an item is not proof that an exchange was agreed.

**Open routes:** use available stock, seek another source, narrow the copy policy, or leave the task deferred. The player can continue without the exact supply.

## 128. Failure card — private detail becomes public

**Cause:** the player includes more personal information in a board, broadcast, or report than the affected person agreed to share.

**Immediate consequence:** the person may feel exposed or lose control over who knows the reason. The information cannot be made unknown.

**Recovery actions:** remove or restrict the detail where possible; correct the public record with the minimum necessary wording; ask the affected person what they want done next; tell recipients that the earlier disclosure exceeded scope; or accept that some recipients may retain what they heard.

**Remembered fact:** who disclosed, who could access it, what correction was made, and whether the affected person chose to respond. Do not make a later apology erase the disclosure.

**Open routes:** the person may still participate, refuse, or leave the decision to others. The main plot continues. The ending can acknowledge the repair attempt and remaining exposure.

## 129. Failure card — returned resident declines the conversation

**Cause:** the person whose absence caused the scheduling problem returns but does not want to discuss the past event.

**Immediate consequence:** the reason and original intent remain unknown. The operational assignment still needs a current answer.

**Recovery actions:** ask only whether they accept a future shift; offer a private channel; accept no explanation and find other coverage; leave the old event disputed; or ask a mutually agreed intermediary to convey a practical question.

**Remembered fact:** the person declined this conversation, not necessarily all work or all future contact. A refusal to explain does not prove the player’s preferred theory.

**Open routes:** the player can adopt a no-assumption rule, use a substitute, leave the matter open, or finish through another ending. No route requires extracting a confession.

## 130. Failure card — no one accepts the task

**Cause:** all available residents decline, are unavailable, or have conflicting work.

**Immediate consequence:** the task remains uncovered. If it is essential for safety or survival, the existing owning system determines the consequence.

**Recovery actions:** reduce the task scope; delay until a stated time; request bounded outside service; trade for assistance; perform it personally if possible; or discontinue the task and use another method.

**Remembered fact:** offers made, acceptances or refusals, task scope, and what was delayed. Do not mark the task completed because the scene has ended.

**Open routes:** the player can still complete the narrative with an uncovered task, a changed practice, or an explicit hazard. The story must not generate a random catastrophe to punish the group for having limited capacity.

## 131. Failure card — temporary rule expires without review

**Cause:** the trial period ends and no review occurred. The player may have missed the date, skipped the scene, or lacked a willing convener.

**Immediate consequence:** the old practice is no longer confirmed as current. It may still be what people are using, but the story should not assume renewal.

**Recovery actions:** ask whether participants want to extend it; use a one-shift provisional continuation; return to the previous method if safe; choose another practice; or let the agreement lapse.

**Remembered fact:** trial expired without review. If people continued using it, record actual use separately from explicit renewal.

**Open routes:** the ending may report an informal continuation, an expired agreement, or a changed method. No automatic penalty or reward is necessary.

## 132. Compound failure walkthrough — late notice plus withdrawn cover

A message arrives after the scheduled shift, and the substitute who initially offered help withdraws. The player cannot recover by simply replaying the same assignment as if neither event happened.

First, the player marks which version was available to the affected worker. Then they decide whether to find a second volunteer, reduce the task, delay it, or leave it uncovered. The player can contact the original worker, but cannot demand that they perform a task they did not accept. If the original returns and declines to explain, that boundary remains valid.

The final record shows the delayed notice and withdrawn cover. If the task is later completed by someone else, that is a new coverage event. If not, the ending names an open task and what the shelter can safely do without it. This path should remain possible without locking the player out of the main ending.

## 133. Compound failure walkthrough — temporary repair plus no available inspector

The board is held by a temporary brace, but Mina is unavailable when the review date arrives. The player can ask the learner from “A Lesson Has an End” to inspect only the step they accepted; request a one-time Guild check; move the board and stop using the brace; use spoken confirmation; or accept that the mount remains unverified and restrict access.

The character who learned one repair step does not become a full mechanic by implication. A Guild visit can confirm fit but may not create local upkeep. Moving the board can solve a short-term access problem while leaving the wall damaged. A spoken schedule can keep the current shift operating, but it is not a physical repair.

If no inspection occurs, the journal says the brace was not checked. The ending may discontinue the board or continue a temporary method with a known limit. It must not call the repair safe based only on elapsed time.

## 134. Compound failure walkthrough — faction report conflicts with local record

A faction report claims that every shift was covered. The shelter record shows one unfilled interval and one disputed assignment. The player can send a correction; provide a narrow count and request that the prior report remain attached; ask affected people before sharing details; or decline to participate in the report.

The faction representative may explain that their report describes planned coverage rather than completed work. This can be a difference in definition rather than deliberate deception. The player can clarify terms for future reporting. If the representative knowingly keeps the overclaim, the narrative can show that political conflict without making the entire faction universally dishonest.

The action record preserves both reports, their sources, and any correction. The ending can vary by whether the player accepted the service, negotiated the report scope, or refused renewal. The local schedule practice still resolves independently.

## 135. Recovery verbs and player feedback

Recovery options should be framed as actions the player can perform:

- **Correct:** change an active fact while preserving the earlier event.
- **Recontact:** attempt a new communication without claiming the first arrived.
- **Reassign:** find a willing person or reduce the task.
- **Defer:** state when or under what condition the decision returns.
- **Withdraw:** stop a service or task within its known terms.
- **Replace:** use another resource or process with its own cost.
- **Restrict:** reduce access or disclosure without pretending the information was never seen.
- **Acknowledge:** name what happened and the remaining effect.
- **Leave unresolved:** keep play moving while making uncertainty explicit.
- **Retire:** stop using an unsafe or unworkable practice and state its replacement or gap.

After each recovery action, show what changed and what did not. “The board is corrected. Osha has not confirmed that the east room received the update.” This kind of feedback is more useful than an invisible success message.

## 136. Recovery design checks

For each failure route, the authoring review asks:

1. Is the failure caused by a visible condition, a named decision, or a clearly represented unknown?
2. Does the player know which part failed?
3. Is there a new action available that respects prior consent and agreements?
4. Does recovery have a real cost or limitation?
5. Does the earlier event remain in history?
6. Can a character’s refusal remain valid after the failure?
7. Is there a route forward if no one can help?
8. Does the narrative avoid inventing motive from absence or silence?
9. Does the ending report partial success accurately?
10. Is any systemic consequence applied by its existing owner?
11. Can the player tell when the recovery itself has completed?
12. Is a repeated failure handled as a changed state rather than replaying the same scene unchanged?

A failure that passes these checks can add tension without making the player feel trapped by an invisible morality gate. It gives the player agency after a bad outcome while preserving the people and costs involved.

## Installment 13 closing note

This installment adds twelve concrete failure cases, three compound recovery walkthroughs, a shared vocabulary of recovery actions, and review checks. Failures now produce new information and new choices without erasing prior actions or inventing motive. The central quest remains completable through partial, delayed, or unresolved outcomes.
# Installment 14 — Compound action routes and mid-arc pivots

## 137. From isolated choices to recognizable action patterns

A meaningful run is made from combinations. A player who asks before assigning work in Act IV may still repair the board quickly in Act V. Someone who usually shares information may keep one person’s reason private in Act VI. This installment expands six compound routes to show how earlier methods can influence later scenes without becoming fixed character classes.

The route record should preserve only the distinctions the narrative reads:

- **Timing:** acted immediately, waited for evidence, or used a temporary plan.
- **Audience:** told the affected person, a group, a representative, or no one yet.
- **Source:** inspected a record, relied on a witness, used an intermediary, or proceeded with uncertainty.
- **Labor:** performed the task, delegated it, invited volunteers, or left it uncovered.
- **Scope:** made a one-time arrangement, accepted a trial, or created a recurring duty.
- **Repair:** corrected the record, repaired the process, acknowledged harm, or left the issue open.

These dimensions can combine. The design should not decide that “fast + public” equals careless or that “private + delayed” equals trustworthy. The following routes show how the same action dimension can lead to different outcomes.

## 138. Compound route — fast cover, narrow correction

**Act I:** The player moves an obstructing tool before comparing the schedule. Work can continue, but the old assignment remains visible. Mina notes that the moved tool must be returned.

**Act II:** The player checks one receipt and marks the rest of the board provisional. This protects against overclaim while keeping the current task usable. Rade knows which source was used; the player does not claim the list is fully verified.

**Act III:** A duplicate is requested by a resident who cannot visit the board. The player makes a narrow copy of the current shift and omits unrelated assignments. The recipient can read it, but it has an expiry and no archived history.

**Act IV:** An expected worker is absent. The player assigns a one-shift substitute, writes that the reason is unknown, and tells the substitute the coverage ends at a stated time. The original worker may later explain, or not.

**Act VI:** The original worker returns and disputes the earlier line. The player updates the current copy with a short correction and keeps the past version separately. They do not broadcast a private explanation.

**Ending:** the next shift is covered and the current record is clearer. The cost is that the source set remains incomplete and the narrow copy must be renewed. A callback can say, “You moved first and checked later. This time the line changed before the story did.” It describes the sequence, not a judgment.

## 139. Compound route — private boundary, complete operational coverage

**Act I:** The player asks each affected person what they need for the next task but does not ask for personal reasons. They inspect the current schedule after hearing those immediate needs.

**Act II:** The player asks Osha to deliver a private confirmation. Osha accepts one route with a return request. The sender gives the content and the audience; the player does not copy the entire board.

**Act III:** The recipient confirms the new task but declines to explain why they need the private channel. The player records the operational acceptance and leaves the explanation private.

**Act IV:** A different worker is absent. The player refuses to infer the reason from the earlier private arrangement. They find a willing substitute and set a limit; if no one accepts, they leave the task uncovered.

**Act VII:** The group chooses a current schedule with a private channel for exceptions. This means people can work from the public list while specific changes remain limited to those involved.

**Ending:** coverage is clear, but the system depends on direct messaging and a willing courier. It is not a universal replacement for public updates. The player can take this route with any major faction or none; faction pressure changes the cost of private reporting but not the worker’s right to a boundary.

## 140. Compound route — delegated work, verified scope

**Act I:** The player asks Rade to compare records, Osha to check delivery history, and Mina to inspect the board. Each receives a bounded question. The player does not treat all three reports as a single omniscient account.

**Act II:** Rade returns a copied line; Osha reports an unconfirmed message; Mina reports that the board can stay up for one more shift. The player asks what each observation does not prove.

**Act III:** The player lets a resident choose whether they want a copy or readback. The recipient selects a readback, and the player records that format rather than assuming the whole group has access.

**Act IV:** The player delegates the search for a substitute to a named resident. The resident returns with one volunteer who accepts a single shift. The player checks that the original task still has an owner.

**Act VII:** The group writes a trial rule together. The player delegates the draft but reads it back before it becomes current. The facilitator is not treated as the decision-maker.

**Ending:** more people’s skills shaped the outcome, but delegation created additional check-backs. If the player skipped a follow-up, the ending calls out that specific gap. This route can lead to a shared practice or a fragile one depending on whether scope was verified.

## 141. Compound route — negotiated support, then an intentional exit

**Act I:** The player accepts one Independent supply delivery under a written count and date. They do not commit to renewal.

**Act II:** The package arrives short one item. The player accepts the usable portion but adjusts the exchange. The discrepancy is recorded, and no report is promised beyond the deal.

**Act V:** The Scavenger Guild repairs the board under a one-time fitting agreement. The player asks Mina to inspect the result before the Guild leaves. The repair works, but its routine upkeep remains local.

**Act VI:** The broker asks to renew early in exchange for broader schedule access. The player declines the added access and asks for one more bounded delivery instead. The broker may accept or refuse under current terms.

**Act VII:** The group keeps the current record local and identifies a replacement supply source. If there is none, the shortage remains visible. The player does not pretend that leaving a contract created independence instantly.

**Ending:** the shelter used outside help without granting ongoing control. The cost is transition labor and possible supply gaps. This is neither a betrayal ending nor a triumph; the history records what each party provided and what ended.

## 142. Compound route — accept a faction service, challenge its report

**Act I:** A military liaison provides a lamp for a temporary period. The player accepts the practical service and records the end date.

**Act III:** The liaison asks for a roster. The player shares covered hours without personal reasons. The military route remains active, but its report contains an incomplete view.

**Act IV:** A shift remains open. The player assigns a substitute and records that the original person’s reason is unknown.

**Act VI:** The faction report describes the shift as “fully staffed.” The player compares it with the shelter’s record and corrects the claim to “coverage reassigned for one shift; original reason unconfirmed.” The player can ask the affected worker before sharing their name.

**Act VII:** The liaison may accept the correction, dispute the wording, or withdraw the lamp service. The player responds according to the actual agreement. If the service ends, the shelter finds another light method or accepts the constraint.

**Ending:** military support and disagreement coexist. The player’s route is still Military because a relationship and service exist, but the faction does not own the record. A later scene can show whether the corrected report was used.

## 143. Compound route — preserve uncertainty while protecting the task

**Act I:** The player finds conflicting records and cannot identify which was read. Rather than wait indefinitely, they ask who can do the task now.

**Act II:** The player creates a provisional schedule with an expiry. The old record remains available to inspect under a narrow access rule.

**Act IV:** The expected worker does not arrive. The player does not mark them as refusing. A substitute accepts part of the job; another part remains open.

**Act VI:** The original worker returns but declines the conversation. The player accepts that boundary and asks only about future availability. No explanation is written down.

**Act VII:** The shelter adopts a short-term procedure that marks uncertain assignments and requires direct confirmation for high-impact work. A review date is named.

**Ending:** the historical cause remains unresolved, but the immediate task is safer and the worker’s privacy is respected. The cost is a slower confirmation process and a partial coverage gap. This path should feel complete even though no one confesses or apologizes.

## 144. Mid-arc pivots: when the player changes their method

A pivot is a scene in which the player’s new action differs from their previous pattern. It is not a punishment and should not require the player to explain themselves. The character can notice a concrete change and open a response.

### 144.1 From quick action to consultation

After two rapid fixes, the player pauses before changing the board and asks the affected worker first. Rade can observe, “You waited for her answer this time.” The player may say the circumstances changed, give no reason, or continue the task. No trust number increases automatically.

### 144.2 From consultation to emergency action

After a meeting delays urgent coverage, the player acts first during a later emergency and then returns to explain. Osha can say, “You sent the notice after the change. I got the second half of it.” This recognizes both the urgency and the communication cost.

### 144.3 From outside service to local process

The player previously bought a copy service but now asks residents to maintain the board. The broker may ask whether the contract is ending. The player can provide notice, negotiate a final delivery, or terminate according to the terms. The local process still needs training and a review.

### 144.4 From public record to private notice

A past public update exposed too much detail. In a later scene, the player limits a correction to the practical assignment. An affected character may appreciate the changed scope while still being upset about the earlier disclosure. Both facts remain true.

### 144.5 From delegation to direct ownership

A delegated task was missed. The player chooses to do a later check personally. This does not mean delegation is always wrong; it means the player’s follow-through changes. A character can ask whether the player wants to retain the duty or train another person.

### 144.6 From strict tracking to trust

The player previously requested return acknowledgments for every message. In a low-impact scene, they decide that a spoken update is enough. The journal can show that no acknowledgment was requested. A later missed message is possible and does not prove the new approach was foolish; the risk was chosen.

## 145. Action pairing matrix

| Earlier action | Later action | Potential callback | What must remain distinct |
|---|---|---|---|
| Rapidly reassigned a shift | Asked the original worker before assigning next time | Player changed order of operations | Earlier reassignment and its cost |
| Shared a full copy | Restricted a later copy | Disclosure scope changed | Who already received the first copy |
| Accepted a trial service | Declined renewal | Use and exit are both real | Completed service remains in history |
| Delegated a record check | Verified the returned report | Delegation gains a check step | Delegation is not proof of observation |
| Kept a dispute open | Adopted temporary coverage | Uncertainty coexists with action | Temporary coverage does not settle history |
| Used a spoken handoff | Requested a written copy later | Format changed with context | Earlier spoken recipients and limits |
| Refused outside aid | Accepted a one-time service later | Circumstances or terms changed | Original refusal is not retroactively accepted |
| Asked for a public correction | Limited a private detail | Accuracy and privacy both matter | Correction itself is not erased |

The matrix is a dialogue and continuity aid. It is not a scoring system. A character can mention a pairing only if they know both events or the player tells them.

## 146. Action-route feedback: showing the branch without a label

The game can make branching visible in the scene rather than through a meter. A named person takes the substitute’s former task. A copy carries a date and source. The light has moved to the new posting point. The message remains folded because no delivery was authorized. A report has one line crossed out and another attached. The character now asks before borrowing the marker. Each signal tells the player what their action changed.

Feedback should be immediate enough that the player can connect cause and effect. If the effect arrives later, a journal note or callback can bridge the delay. Avoid congratulatory banners that say “Trust increased” or “Faction virtue gained.” Prefer a factual response: “The east room has the new time; the west room has not confirmed.”

A branch is still meaningful when its effect is a limit. A privacy choice can reduce who sees the record. A delay can preserve consent and leave a task uncovered. A refusal can prevent dependency but cost a repair. The feedback should identify both what the choice protected and what it made harder.

## 147. Branch combinations that should not collapse

Some outcomes sound similar in a summary but must remain distinct for future dialogue:

- **No message sent** and **message attempted without acknowledgment** are different.
- **Worker declined the job** and **worker was not asked** are different.
- **No one attended** and **attendees did not agree** are different.
- **Record is missing** and **record was intentionally withdrawn** are different.
- **Part was not available** and **part was available but reserved** are different.
- **Player did not renew service** and **provider refused renewal** are different.
- **Player privately corrected one person** and **publicly corrected the shared copy** are different.
- **Temporary task was completed** and **recurring duty was accepted** are different.
- **Witness saw the action** and **witness understood the agreement** are different.
- **A faction’s report differs** and **a faction knowingly misrepresented the event** are different.

If the current content model cannot preserve every distinction, the package must prioritize those with later consequences and revise the design explicitly. It must not merge states and then write dialogue that claims the missing fact.

## 148. Compound route review

For every advertised action combination, verify:

1. Each prior action can actually occur in the current route.
2. The later scene has a reader for the relevant evidence.
3. The callback uses only what its speaker can know.
4. A player can pivot without being locked into an archetype.
5. Any resource or faction service follows its real owner.
6. A repair changes current practice without erasing history.
7. The same ending is not chosen from discussion alone when the action never happened.
8. A stable unresolved state remains available.
9. The route difference changes an observable consequence, not only a hidden value.
10. The player can explain the difference between two outcomes in ordinary language.

## Installment 14 closing note

This installment develops six multi-act action combinations, six mid-arc pivots, an action-pairing matrix, and rules for visible feedback and branch preservation. It treats player style as changeable and grounded in actions, while keeping earlier consequences available for honest callbacks.
# Installment 15 — Location design and environmental branching

## 149. Space as a branching surface

The shelter’s locations should carry information about who uses them, what can be observed there, and which actions are safe. A room is not only a backdrop for dialogue. Moving a meeting, placing a copy, leaving a tool, or choosing a private doorway changes the audience and the work required. Each location needs at least one ordinary use, one consequence-bearing use, and a way for the player to understand why a scene happens there.

The plan does not assign final room names or map coordinates. Existing location authority and scene layout must be checked before implementation. The spaces below are functional placeholders. If a current location already serves the purpose, extend it rather than building a parallel room or duplicate hub.

## 150. The board alcove — public information with a narrow threshold

The alcove is the default place for current assignments. Its narrow entrance makes it easy to notice who approaches, but difficult for several people to gather without blocking the corridor. The player can inspect, post, remove, or read back a schedule here. A private conversation is possible only if the speakers deliberately step away.

**Branch use:** a public correction can be seen by people passing through. A narrow correction can say that a shift changed without giving a personal reason. A spoken explanation reaches only those present. If a person is standing in the corridor, they can witness a posting without being part of the decision.

**Environmental states:**

- Before repair: the paper edge curls where water runs from the ceiling.
- After temporary repair: a brace is visible and the check date is written beside it.
- After a lasting repair: the frame sits level; the mark of the old fastening remains.
- After a current/record split: one section reads “next shift”; another is dated and restricted.
- After board retirement: old nail holes remain, but no current assignment is posted.

**Player routes:** a character who cannot stand in the doorway can receive an alternate readback in another room. If a second posting point exists, it must show its own date and owner. The board alcove cannot become the only place where a necessary choice appears.

## 151. Rade’s shelf — custody and personal ownership

Rade’s shelf contains copies he has chosen to keep. The player does not automatically have the right to browse them. An interaction can begin with Rade offering a page, asking for help labeling it, or explaining that a copy has gone missing. If the player asks to inspect a particular record, Rade can consent, decline, or offer a summary.

**Branch use:** the player can distinguish local custody from outside archive. A record stored here is not necessarily public and not necessarily safe from loss. If Rade leaves the clerk role, the shelf needs a new owner, an agreed transfer, or an explicit decision to reduce the archive.

**Environmental states:**

- A page held flat by a clean tool indicates active use, not permanent retention.
- A folded corner may be Rade’s reminder; it is not automatically a secret code.
- A dated envelope shows that the group adopted a retention limit.
- An empty shelf can mean records were returned or the practice changed; a journal or character line establishes which.
- A page with an annotation attached shows that correction happened without overwriting the earlier entry.

**Optional interaction:** the player may help Rade sort records by current, superseded, disputed, or private status. Sorting changes access and retrieval, so the player sees the proposed categories before applying them. The process does not create truth labels.

## 152. East doorway — message handoff and uncertain receipt

The east doorway is a common handoff point because couriers can leave a message without entering private rooms. It is also exposed to traffic and weather. A message left here can be seen by the wrong person or missed entirely.

**Branch use:** the player can use the doorway for a sealed note, a public notice, a face-to-face meeting, or no message at all. Each mode has a different audience. A sealed note has custody risk. A public notice has exposure risk. A meeting requires both parties to arrive. No message leaves the player with uncertainty about whether the recipient knows.

**Environmental states:**

- A dry ledge is available after Mina or the Guild repairs it.
- A chalk mark can state the intended recipient without exposing the message content.
- A returned note remains folded if no one opened it.
- An empty hook does not prove a message was delivered.
- A route slip beside the door records an attempt only when someone actually left.

**Access alternatives:** Osha can deliver a message to the person’s preferred location if she accepts the route. Long Walk may provide an external route. A direct conversation can occur elsewhere. The doorway is never the only route to completing a required quest.

## 153. Repair bench — shared skill without assumed duty

The bench belongs to whoever is actively using it, not to Mina by default. Tools may be arranged for a task, but the layout can change when another worker takes a shift. The player can inspect a repair, request a demonstration, return a tool, or ask who accepted the next check.

**Branch use:** the player may bring the board bracket, lamp, or writing surface here. Mina can show the limit of a temporary fix. A learner can perform one step. A Guild mechanic can fit a part under an agreed service. These users need separate acceptance and scope.

**Environmental states:**

- A tool rests on a cloth if someone is midway through work.
- A returned tool goes to the shared rack only if that is the agreed location.
- A part waiting for inspection is marked with its actual status.
- A temporary brace is tagged with an inspection date.
- A finished repair shows the physical change but not who owns future upkeep unless a character or record says so.

The player can ask to clear the bench, but doing so may interrupt another task. If an NPC has left a tool on the bench, the player should not assume abandonment. A scene can ask whether they want it moved.

## 154. Kitchen pass-through — labor that a roster can hide

The kitchen pass-through connects meal preparation with shared-room traffic. It lets Iven hand over food without leaving the task. It is a poor place for a long meeting because people are moving and listening is uneven. A quick coverage decision may happen here; a full dispute review should offer another space.

**Branch use:** the player can change a meeting time, ask for one person to cover the kitchen, split a task, or delay the conversation. Moving a meeting away from meal preparation does not erase the labor cost of the alternative time. A person can accept one cover without becoming the permanent kitchen replacement.

**Environmental states:**

- Two bowls set aside may reflect an agreed late meal, not favoritism.
- A covered pot can indicate a short delay; it does not prove the meal is ready.
- A note with a time and initials records a handoff only if someone accepted the task.
- A cleared counter after the meeting can show that the group left space for kitchen work.
- If the meeting overlaps a meal, the absent worker should not appear in a later scene as though they attended.

This location gives the player ordinary opportunities to ask Iven about capacity without turning every conversation into a quest marker. He may answer in a short practical line and return to work.

## 155. Common table — open decisions and unequal access

The common table hosts group discussion, shared repair, and the final review. Its openness can support participation, but it also makes private information easier to overhear. Not every group decision belongs there.

**Branch use:** the player can convene a public meeting; ask for a smaller meeting; gather written positions; let the group work while discussing; or decline to make a collective decision. A public space does not guarantee equal participation. A private meeting does not guarantee fairness. The player sees who can attend and what the choice will reveal.

**Environmental states:**

- More chairs than attendees show that invitations were made, not that people agreed.
- A marked agenda can show that immediate coverage was separated from historical dispute.
- A cup moved aside can indicate a work area was cleared, not that the meeting ended.
- A blank portion of the table can make room for a person who may arrive later, if someone agreed to wait.
- Papers facing inward suggest restricted review, while a public copy can remain on the board.

A final scene can use this table for any faction route, but the attendees and documents change. A military route may include a liaison only if the player accepted their presence. A Rebel route may include an organizer only if the meeting terms allow it. An Independent broker may not be invited to a private resident discussion. A no-commitment route may still invite one bounded service representative without becoming a standing alignment.

## 156. Location-to-branch navigation map

| Location | Main actions | Evidence it can produce | Alternative route |
|---|---|---|---|
| Board alcove | Inspect, post, compare, read aloud | Current display and physical condition | Direct notice or second posting point |
| Rade’s shelf | Request access, label, return, restrict | Custody and source history | Local summary or no archive |
| East doorway | Deliver, wait, meet, leave sealed note | Attempt and handoff point | Courier route or face-to-face meeting |
| Repair bench | Inspect, demonstrate, fit, return tool | Physical condition and accepted work | Guild visit or alternate posting method |
| Kitchen pass-through | Reorder time, cover, defer | Labor conflict and accepted handoff | Move or shorten the meeting |
| Common table | Convene, draft, read back, defer | Participation and agreed procedure | Private consultations or temporary rule |

This map protects against spatial soft locks. If one room is unavailable, unsafe, occupied, or inaccessible, the player still has another route to the information or action. The alternate route may cost time, privacy, or labor, but it exists.

## 157. Location transitions should carry state

Moving a conversation can change what people hear, which records are available, and what tasks are interrupted. The story should acknowledge these changes explicitly. If the player moves from the board alcove to the common table, they may need to bring the copy. If they leave a message at the doorway, it may not reach the intended person. If they call a meeting in the kitchen pass-through, Iven may need to pause his work or ask the group to move.

A location transition should not reset the scene’s facts. The player cannot avoid a correction simply by walking away and reopening the dialogue. If the conversation was interrupted, it can resume with an accurate summary: what was agreed, what remains disputed, and who is no longer present.

The player can also choose to end a conversation. The NPC may leave, take a record, or continue a task. That is a real action with a consequence, not an automatic cancellation. The next interaction should begin from the resulting state.

## 158. Environmental storytelling variants by player history

### 158.1 After a public correction

The old line remains legible beneath a dated correction. Someone has underlined the new current assignment once. If the player asked for a restricted history, the private explanation is absent from the wall. A passerby can understand what changed without learning why.

### 158.2 After a private correction

The public board carries only the current task. In Rade’s shelf, a narrow dated note records that a correction was made. The player can retrieve it only under the agreed access terms. This arrangement can be efficient but requires the shelter to remember where the history is held.

### 158.3 After a failed delivery

The unopened note is back at the east doorway or desk. Its string is intact. Osha’s route slip says “returned” if she observed its return. No dialogue treats it as received. If the player authorized another attempt, the note moves only after that attempt happens.

### 158.4 After a successful shared repair

The board frame sits level and has an inspection mark. If the repair is temporary, the mark names the check date. If the task was delegated, the initials reflect who actually performed it, if they agreed to be named. The tool returns to its accepted storage place.

### 158.5 After a board is retired

The wall shows where the old board stood. A new location or process is visible through a sign, a route card, or a meeting time. If there is no replacement, the empty wall is accompanied by a journal entry or character line that states the current limitation. Silence alone should not imply a solved transition.

## 159. Ambient sound and small visual cues

Audio can reinforce who is doing work without making hidden state impossible to understand. A pencil pause, a paper tear, a returned tool click, a knock followed by silence, or the scrape of a chair can establish a moment’s texture. Sound cues are not proof of message receipt or NPC intent. The player still needs visual or dialogue confirmation for consequential facts.

A board can show a paper corner lifting or a new dated strip. A lamp can cast light over only part of the schedule. A table can have one chair left open for an invited participant. A route slip can be wet at the edge. These cues should match state and never imply a branch that did not occur.

The audio and visual implementation must use current asset, event, and presentation owners. This plan proposes no parallel sound manager, interaction registry, or manually duplicated state. If the audiovisual route cannot observe a relevant fact through existing event seams, use authored dialogue or a journal update instead.

## 160. Location and accessibility review

For every location-based branch, verify:

1. Can the player approach and understand the interaction without relying only on color?
2. Is essential information available in text or dialogue as well as sound?
3. Can the player choose an alternate conversation location if the default room is crowded or inaccessible?
4. Does moving the scene change audience and privacy in a legible way?
5. Can a person who does not use the central board receive necessary information?
6. Does the route provide a safe path if the room is unavailable?
7. Does a location-specific prop appear only when its state is true?
8. Does the player know which character accepted the work?
9. Can the player leave and return without the scene fabricating consent or completion?
10. Are environmental variants compatible with the current scene and asset authority?

Accessibility is part of branch design. A scene that can only be resolved by reading a distant wall or hearing a faint announcement excludes a playstyle and can falsely label the player as inattentive.

## Installment 15 closing note

This installment expands six functional shelter spaces, maps each to actions and evidence, and adds state-dependent environmental variants and audio cues. Location choice changes audience, access, and labor, while alternate routes prevent a single room or prop from becoming a soft lock. The final names and layout remain subject to current location authority and collision review.
# Installment 16 — Faction representatives, contested accounts, and shared work

## 161. The faction representative is a person with a task

Major factions matter through people who arrive with a duty, limited authority, and their own interpretation of events. The representative should not speak as if they are the entire faction. They can carry a request, offer a resource, summarize their organization’s position, or report what they personally observed. They cannot unilaterally control every member or guarantee every future service.

Representatives in this plan are role templates, not canon names. The existing story and faction authorities must determine whether a current representative already fills the role. If so, extend that character’s established voice and history rather than creating a duplicate.

Each representative needs:

- A task they are responsible for during the scene.
- A limit on what they can authorize.
- A fact they know firsthand.
- A belief or priority that can be challenged by new evidence.
- A service or concession they can offer within their actual remit.
- A refusal boundary for requests outside their authority.
- A way to remain in the story after disagreement without turning into a villain or ally by default.

The player’s relationship with a representative develops through concrete exchanges: whether a report was accurate, a delivery arrived, a boundary was respected, or an agreement was renegotiated. The quest does not add a second relationship meter for these interactions.

## 162. Military liaison role — operational clarity and reporting limits

The liaison coordinates a scheduled handoff or temporary use of shelter capacity. They are good at defining time windows and identifying what their unit needs. Their blind spot is that a neat roster can look more certain than the underlying agreements. They may ask for a name because their form has a name field, then accept a coverage count when the player explains that the field is not necessary.

### 162.1 First conversation

The liaison arrives with a clipboard and says, “I need to know whether the west room is covered between second light and shift change.”

The player can provide confirmed coverage; state that one interval is provisional; ask whether names are necessary; ask for the liaison’s own deadline; or decline to report until the shelter has checked the list.

If the player asks about need, the liaison can explain that equipment will be stored only if someone is present. This lets the player respond to the actual request rather than assume it is a demand for total control. If they decline, the liaison arranges another storage plan where possible and states the cost. If they mark coverage provisional, the liaison can accept it as a planning fact while reserving the right to ask again.

### 162.2 Conflict scene

The liaison’s report says “covered” because a substitute was assigned. The shelter’s record says “accepted for one shift; task not yet completed.” The player can ask the liaison to revise the wording; attach both states; send a count rather than a name; or leave the report as submitted and correct the shelter’s own copy.

The liaison may say, “My form asks whether someone owns the interval. It doesn’t ask whether the work is done.” If that is their meaning, the player can ask for the report’s scope to be labeled. If the liaison is overstating the result, the player can challenge the claim. Both branches can continue into later coordination.

### 162.3 Relationship evolution

If the player provides timely, scoped reports, the liaison begins asking which format is useful rather than handing over the same form. If the player repeatedly refuses without explanation, the liaison may stop offering that service but can still answer a one-time practical question. If the player corrects an overclaim, the liaison may accept the correction or defend the form; their response creates a story fact but does not change faction allegiance by itself.

## 163. Rebel convener role — participation and the absent voice

The convener creates space for collective decisions and can help people speak across a conflict. Their blind spot is that a well-attended meeting may look like the whole community even when several affected people could not come. They can accept smaller or asynchronous participation, though this takes more time.

### 163.1 First conversation

The convener asks, “Do you want us in the room while you sort this out, or would that make it harder?”

The player can accept a public meeting; invite the convener only as a listener; ask them to help gather written positions; request a smaller meeting; or decline. The convener can explain that their group may have fewer people available to help if no one sees the issue, but they cannot require the shelter to make its dispute public.

### 163.2 Conflict scene

After a meeting, the convener describes the new rule as a collective decision. A resident who was absent says they never saw it. The player can correct the public description; ask the convener to publish only the rule’s trial status; invite the absent resident to review it; or keep the arrangement temporary.

The convener may answer, “We had agreement from everyone who stayed.” The player can accept that narrow statement while noting who was absent. If the convener claims wider consent than the facts support, the player can ask them to amend the claim. The response can create tension without cancelling the shelter’s local procedure.

### 163.3 Relationship evolution

If the player separates participation from unanimity, the convener can help build a repeatable invitation and readback. If the player keeps decisions private, the convener may disagree but can still provide an observer for a later contested meeting. If the player accepts the meeting service and then declines a follow-up, the initial meeting remains real; the later service simply does not occur.

## 164. Independent broker role — terms and unequal leverage

The broker arranges supplies or paid services with explicit quantities. They are adept at finding alternatives and naming costs. Their blind spot is that a fair price on paper can still be difficult to refuse when the shelter has no substitute. The broker may negotiate a smaller service or delayed payment, but does not become a benevolent supplier by default.

### 164.1 First conversation

The broker sets out a small bundle and says, “One repair, two sheets, one trip. Choose which part you need.”

The player can accept the bundle; split it; counteroffer; ask for a day to compare local capacity; request written expiry; or refuse. The broker can tell the player which options remain available without hidden penalties. If the player cannot pay, the broker can propose another exchange or say there is no viable deal today.

### 164.2 Conflict scene

The delivered quantity is smaller than the agreed amount. The broker’s copy records the package as complete; Sera’s count does not. The player can compare receipts; ask the broker to replace the missing item; accept a credit; return the bundle; or ask for a witness to inspect it.

The broker can reveal that their runner counted a sealed package before a later transfer. This explains the discrepancy but does not solve it. The player can amend the trade terms for the next delivery, dispute the current one, or leave without renewal. If the broker offers credit, the player decides whether it is useful under the existing economy owner; the plan does not create local currency.

### 164.3 Relationship evolution

A player who asks for precise terms may receive clearer offers, not a secret trust boost. A player who declines may still be offered a different service later if circumstances and stock change. A player who accepts one trade is not locked into the broker’s future package. The broker can remember whether the player honored a return condition, but cannot claim ownership of the shelter’s schedule.

## 165. No-commitment contact role — a door kept open without a promise

A local contact helps the shelter understand outside pressure while allowing the player to avoid a standing faction agreement. This can be an existing character with an established role; the plan does not create a new faction. Their usefulness comes from explaining what is happening nearby, not from secretly representing every group.

The contact can answer one practical question, carry a request for a meeting, or identify a route consequence. They may be wrong or have partial information. The player can ask who told them, what they observed directly, and what they need in return. A one-time conversation does not establish future availability.

If the player later accepts a service from a major faction, the contact can help compare its terms with prior information. They do not grant permission or invalidate the choice. If the player refuses all services, they can still receive an occasional factual update if current story logic supports it.

## 166. One incident, four institutional accounts

**Shared event:** the board shows that a workroom was covered during a period when the assigned resident was absent. A substitute accepted the task, but no one yet knows whether the work was completed. Each representative describes a different piece of the event.

**Military account:** “A substitute accepted the interval. Our equipment stayed attended. We still need confirmation that the inspection happened.”

This account distinguishes coverage from completed work. It is operationally useful but may omit the original resident’s perspective.

**Rebel account:** “People changed the shift without waiting for a central order. The resident who was absent wasn’t part of the decision.”

This account notices local agency and exclusion. It may overemphasize the political meaning of a practical substitution.

**Independent account:** “The exchange covered one shift. If you want another, we need a new term.”

This account focuses on duration and value. It may fail to describe the social effect of the decision.

**Local no-commitment account:** “The workroom stayed open. The old line is still on the board.”

This account focuses on immediate operation and unresolved record state. It may understate outside dependencies.

The player can ask each speaker what they know firsthand; ask what they leave out; accept a narrow version; combine accounts in a report; or decline to publish a summary. No representative’s wording determines the canonical truth. The journal records sources and differences only if the player chooses to compare them or the current journal system records the event automatically.

## 167. Cross-faction cooperation scene — “One crate, two signatures”

A shipment needed by the shelter is temporarily held at a neutral checkpoint. A military contact can verify storage, a Rebel courier can transport it locally, and an Independent broker can provide packing materials. Their cooperation is possible because each role is bounded; none commands the others.

The player can coordinate all three; accept only the storage verification; ask the shelter to wait for a single provider; send a local worker; or decide the shipment is not worth the coordination cost. Each organization states its own terms. The military contact will not sign for an item they did not inspect. The courier will not carry an open personal record. The broker asks for the quantity and return conditions.

If the player coordinates all three, the cargo chain has multiple handoffs. Each handoff must be confirmed separately. A mismatch can be investigated without accusing the last person in the chain. If the player chooses one provider, the route is simpler but may have a known limitation. If they refuse, the shelter uses local stock or proceeds without the shipment.

The scene can reinforce major faction relationships without requiring a combined faction alliance. A player can accept military storage and still ask a Rebel courier to carry a public notice. An Independent broker can sell packaging to a player who has declined their longer contract. These are service decisions, not faction conversion.

## 168. Representatives can disagree with their own organization

A representative’s personal view may differ from a faction’s official position. The story can show this through a careful caveat: “My unit wants a full roster; I can approve a coverage count for this handoff.” Or: “The convener asked for an open meeting; I can help set up a private review if that is what the affected people agree to.”

This does not make the representative secretly good or the institution secretly harmless. It shows that a person has bounded authority and judgment. The player should not be able to infer a faction’s complete policy from one person’s concession. If the concession creates a binding agreement, current faction authority must record it through the existing path.

A representative can also act in a way the player considers unfair. The player may challenge, refuse, document, seek another contact, or accept a short-term cost. Avoid turning every conflict into combat or faction hostility if the game’s current design supports negotiation, withdrawal, or practical alternatives.

## 169. Relationship states without an approval meter

The plan can describe a representative relationship as a set of facts:

- A service was offered and declined.
- A report was corrected.
- A resource was received under a one-time term.
- A request for broader information was refused.
- A representative accepted a correction, disputed it, or withdrew the service.
- A follow-up conversation occurred or did not occur.
- The player honored or violated an explicit exchange term.

These facts can shape dialogue and availability through the current faction owner. They are preferable to adding a quest-local “faction trust” number. If existing faction standing already influences route access, the integration must verify its semantics and prevent it from overriding the concrete facts the narrative needs.

## 170. Faction-scene acceptance questions

Before implementing a representative scene, reviewers should ask:

1. Is the representative an existing character or a proposed role?
2. What authority do they actually have?
3. Which facts do they know firsthand?
4. What can they promise, and what must they request from another owner?
5. Can the player negotiate scope or duration?
6. Can the player decline without losing the entire quest?
7. Does the scene distinguish organization policy from personal interpretation?
8. Does an accepted service have a real cost and expiry?
9. Does the service use its current owner?
10. Can another faction or local route address the same need differently?
11. Does the scene preserve a non-aligned path?
12. Does the ending describe who holds practical responsibility after the representative leaves?

## Installment 16 closing note

This installment adds faction-representative role templates, conversation branches, one shared incident told through four partial accounts, cross-faction cooperation, and relationship continuity without a new approval meter. Major groups contribute resources and pressure; the player and shelter retain ownership of local decisions.
# Installment 17 — The table decision and the first test of the rule

## 171. Why the group meets

Act VII brings the shelter’s disagreement into one decision, but the meeting is not a vote over who was right in the past. The group needs a practical next-shift method. A good meeting can establish who updates the board, how an affected person is told, what remains private, and when the method will be reviewed. It cannot force everyone to share the same memory.

Before the meeting, the player sees a short preparation scene. Rade asks whether to bring the disputed copies. Osha asks whether she should invite the person who has not acknowledged the message. Mina asks who is responsible for the repair check. Iven asks whether the meeting can avoid the meal period. The player may do all of these, some, or none. Each preparation changes who is present and what can be decided without blocking the meeting entirely.

A missing person remains missing from the decision. A declined invitation remains declined. A person who sends a note contributes that note but has not attended. The player can proceed with temporary coverage and leave the lasting practice open.

## 172. Meeting scene — “The table has room”

### 172.1 Arrival and ground rules

The scene opens with one chair pulled back, one schedule copy on the table, and a second copy still held by Rade. The player chooses whether the meeting begins with the immediate shift problem, the record dispute, or the future process. This order changes time pressure.

Starting with the immediate shift lets the shelter assign today’s work before discussing history. Starting with the record dispute can give context but leaves coverage open longer. Starting with the future process focuses on prevention but risks making people feel that their account was skipped. The player can also state that the group will handle the urgent task now and return to the other questions at a named time.

The player may ask each attendee to state what they personally know, invite written input, request that people speak one at a time, or let the conversation begin without a formal structure. A structured meeting is not automatically fair if a participant cannot use the chosen format. The player can offer a spoken response, smaller conversation, or later readback.

### 172.2 The current task

Rade places the current line on the table. The group must decide whether it is usable for the next shift. Affected workers can confirm, correct, decline, or ask for time. The player can accept a confirmed assignment; record a provisional one; ask for a volunteer; reduce the task; or leave it uncovered.

If someone accepts only part of the task, the assignment is split. The record names the scope. If no one accepts, the group can seek bounded external help or delay. A faction representative, if invited, can offer support but cannot volunteer another resident. The meeting can proceed even if the immediate task remains uncovered.

### 172.3 The record dispute

The two copies are shown with their dates and sources. The player can ask each person what they remember; compare only the physical differences; invite a witness to describe what they saw; keep the details private and discuss only the needed schedule; or leave the history unresolved.

A witness may say, “I saw the line change. I didn’t hear anyone agree.” The person who changed it may say, “I thought the first hour meant the full watch.” The affected worker may say, “I agreed to the first hour.” These statements can all be true. The player can ask what each person understood without declaring a winner.

If one person leaves, the player can continue with a temporary plan but cannot claim their agreement. If two accounts conflict, Rade can record the dispute. If the player asks for a public correction, they decide what information it contains.

### 172.4 The process proposal

The group now considers how future changes should work. The player can propose a current copy with a dated correction; direct confirmation for high-impact shifts; a rotating posting role; a private exception channel; a shared copy system; a single steward with review; or no standing rule.

The player can ask one attendee to propose a method, invite each to name one requirement, or put forward a draft. A proposal becomes a trial only when someone accepts its duties and the affected group knows its limits. The player must identify who updates it, how notice is delivered, how someone declines, who can see records, and when review occurs. The meeting may leave some of these unanswered; the ending must say so.

### 172.5 Closing the meeting

Before the group disperses, the player can read back the current decision; ask an attendee to read it back; request objections or corrections; post the operational part and keep the dispute private; or end the meeting without a shared summary.

A readback is not a promise of unanimous agreement. It verifies that the participants heard the same current arrangement. If a person refuses to confirm, the player records that status. The final line can be “We have a plan for one shift” rather than “We agree.”

## 173. Agreement paths from the table

### 173.1 Time-limited trial

The group tests a method through a stated date or number of shifts. Someone accepts upkeep, and anyone affected can ask for review. The trial can expire without renewal. The benefit is learning from use; the cost is that temporary rules may be misunderstood as permanent.

### 173.2 Shared agreement among participants

Present participants accept a rule, and the player arranges notice for absent people. The record says who participated and which people have not confirmed. The benefit is direct ownership by the group; the cost is that the agreement’s scope is limited to those who accepted it.

### 173.3 Narrow consent for current work

People agree only to the next shift, not a recurring policy. The schedule can operate now while the historical dispute stays open. This avoids forced consensus but requires another decision later.

### 173.4 Rotating responsibility

A small group accepts a rotation. Each person has a clear duty and can decline future turns. The benefit is resilience; the cost is training, reminders, and possible inconsistency. If only one person accepts the rotation, it is not a rotation.

### 173.5 Delegated draft with local review

The group asks Rade, Osha, or another willing person to draft the process. Their draft returns to affected people for review. Delegated authorship does not mean delegated consent. The player can support the draft, suggest changes, or ask for another author.

### 173.6 No standing agreement

The group decides that future changes will be handled case by case. This can suit a shelter whose needs change quickly. The cost is repeated discussion and less predictability. The ending calls this a deliberate absence of a standing rule, not a completed policy.

### 173.7 Stalemate with temporary cover

No lasting method is accepted, but a limited task receives coverage. The group names a next opportunity to revisit the choice or accepts that there is no scheduled review. This path is valid when the meeting fails to produce agreement. It must not quietly assign work to an absent person.

## 174. Dialogue after disagreement

When the player asks Rade to summarize both copies, he can say, “I can tell you when each page arrived. I can’t tell you what the first yes meant.”

When Osha is asked whether the message was delivered, she says, “It left my hand at the door. No one answered. I brought it back.”

When Mina is asked to endorse a temporary brace, she says, “I’ll say it held when I checked. I won’t say it will hold all week.”

When a resident is asked to explain an absence, they may say, “I can tell you what I can do tomorrow. I’m not talking about yesterday.”

When the faction representative is asked to approve the local rule, they can answer, “I can approve the service we offered. This part belongs to the people using the room.”

When the player proposes a procedure, a person can ask, “Who is checking that the copy changed?” Another may answer, “I can do it today. Don’t put me down for every week.”

These lines are not mandatory exposition. Their availability depends on each speaker’s knowledge and the action the player took.

## 175. Meeting variations by route

### 175.1 Military liaison present

The liaison waits for a coverage answer before discussing the report. If the player starts with the record dispute, the liaison may ask whether the handoff can be delayed. If invited only as an observer, they do not speak unless asked. Their final report states what the liaison agreed to provide, not what the shelter decided.

### 175.2 Rebel convener present

The convener asks who is absent and whether the meeting should wait. If the player chooses a temporary decision, they can help send a readback. They cannot announce unanimity without evidence. If the player declines a public forum, the convener may prefer a later open review but can respect a smaller process.

### 175.3 Independent broker present

The broker waits until the group states what service it needs, then gives terms. They do not need to sit through private testimony. The player can send them out before the record discussion and invite them back for the supply proposal. Any later offer has a separate acceptance.

### 175.4 No representative present

The meeting uses local evidence and capacity. The group may still request a bounded service afterward. No outside endorsement is needed for the shelter to choose a local procedure.

## 176. Act VIII — the first test of the meeting’s decision

The next shift begins with a real use of the method chosen in Act VII. The player observes the result before declaring success.

**For a time-limited trial:** the current copy shows the expiry. The player can renew, revise, or let it lapse. A character may have misunderstood the expiry; the player can clarify it without rewriting what happened.

**For a shared agreement:** the scene checks whether absent people received the readback. If someone did not, they can ask for clarification before accepting a future assignment. The group can amend the rule.

**For a one-shift arrangement:** the task ends, and the next decision becomes open again. This is not a failure. It is the boundary the participants accepted.

**For a rotation:** the next person can accept their turn, ask for support, or decline. The rotation needs an alternate plan when the person cannot perform it.

**For a delegated draft:** the drafter can explain what changed after review. The player can confirm that the final version is the one posted.

**For no standing agreement:** the group repeats the discussion in a smaller form. The player can decide whether the repeated cost is acceptable or whether to propose a trial.

**For a stalemate:** temporary coverage expires. The player chooses another action or lets the task remain uncovered. The scene does not automatically force the same meeting again.

## 177. Meeting callbacks to side quests

- If “Nobody Told Me That” was completed, Dema can identify whether the meeting summary is understandable to a newcomer.
- If “The Hour Before the Meal” was completed, Iven can report whether the selected time avoided the kitchen conflict.
- If “A Lesson Has an End” was completed, the learner can state the exact repair scope they accepted.
- If “The Page He Kept” was completed, Rade can explain whether the disputed archive page is relevant to this decision.
- If “The Note That Came Back” was completed, Osha can distinguish the new message from the returned one.
- If the Archivists hold a copy, the group can request it under the existing access terms.
- If the Long Walk carried a message, the meeting can use the route record only for what it confirms.
- If the Guild repaired the board, Mina can focus the group on maintenance rather than re-litigating the part’s fit.

Each callback is optional. The meeting can still produce its central choices when no side quest has been completed.

## 178. Meeting outcomes and player-facing journal updates

After the meeting, the journal should summarize only confirmed outcomes:

- “The group set a one-shift coverage plan. No recurring method was agreed.”
- “Four residents accepted the trial posting process. Dema has not received the summary.”
- “Rade will maintain the current copy through the next review. He did not accept permanent responsibility.”
- “The meeting ended without agreement. The immediate repair has a temporary owner.”
- “The record dispute remains open. The current assignment is confirmed by the two people taking it.”
- “The Independent offer was discussed after the private account. No service was accepted.”
- “The Rebel convener observed the meeting. They did not decide the rule.”
- “The Military liaison accepted a coverage count without names.”

These descriptions make process visible. They are not evaluation labels. The player should be able to see the gap between what the group decided and what still needs action.

## 179. Meeting scene completion checks

The scene is complete when the player has taken one of these paths:

- A lasting practice was accepted with an owner and review.
- A time-limited trial was accepted.
- A one-shift arrangement was accepted.
- A temporary plan was created while the dispute remains open.
- The group declined to set a process and stated the consequence.
- The meeting failed, but the player can continue with a named gap.
- The player ended or left the meeting and the resulting state is accurately preserved.

The scene is not complete merely because all dialogue was exhausted. If a choice remains pending, the UI must show what the player is waiting for, who has not answered, or what action can close the meeting. If the player walks away, that is an intentional outcome with its own readback.

## Installment 17 closing note

This installment expands the Act VII decision into a full meeting sequence, seven agreement paths, faction-specific meeting presence, side-quest callbacks, and the Act VIII first-use test. It supports participation and disagreement without requiring unanimity, and it gives the player a clear practical outcome even when a lasting rule is not accepted.
# Installment 18 — Optional mystery quest: “After the Second Knock”

## 180. Mystery premise and authoring boundary

The phrase “after the second knock” appears in the margin of the damaged schedule. It can open an optional investigation about how people learned to communicate when the board was unreliable. The mystery is not a murder, conspiracy, or hidden faction plot. Its subject is a small shelter convention whose meaning has drifted because the people who used it did not all share the same explanation.

The player should be able to finish the main quest without solving the phrase. The investigation offers context, a possible access practice, and personal histories. It must not reveal that one faction secretly controlled the board or that a survivor deliberately caused the central dispute unless current canon independently establishes such a fact.

The authors should preserve several plausible meanings until the player gathers evidence. The conclusion may identify how the phrase was used by one person, while leaving its original meaning uncertain. This permits discovery without forcing a single explanation onto every run.

## 181. Investigation entry and first response

The phrase can be encountered during the rain scene, discovered on an older copy, or mentioned by a newcomer who asks what it means. Entry source changes who the player can ask first.

Rade says he remembers copying the words but not writing them. Osha may know that someone once used two knocks before leaving a message at the doorway. Pell can report whether he heard a repeated knock on a particular night, if that is part of the player’s available evidence. Mina remembers a time when people used a tap to signal that the repair bench was occupied, but she cannot say the note refers to that practice.

The player can ask one person directly; compare old records; inspect the east doorway; ask the Archivists whether they hold a dated copy; or leave the phrase unexplained. The first response should be ordinary and restrained. No character says, “This is a mystery you must solve.” The player sees a concrete question and decides whether it matters.

## 182. Lead A — the doorway custom

At the east doorway, the player finds a shallow mark on the frame, two small dents close together. They could be repeated knocks or old fastener damage. The player can inspect the frame, ask Mina to identify the marks, ask Pell whether he used the doorway at night, or leave the marks alone.

Mina identifies a likely old fastening but cannot rule out later use. Pell remembers a two-knock signal for a period when people did not want to call out names after dark. He does not know who established it or whether it was connected to the board. Osha remembers carrying a note after someone used the signal, but says the recipient may have heard only the first knock.

This lead supports a practical interpretation: two knocks meant “there is a message; wait for a response before leaving it.” It is plausible, not proven as the phrase’s original meaning.

## 183. Lead B — the repair bench signal

At the repair bench, Mina remembers that two light taps once meant “don’t move the part; I’m still fitting it.” A single tap meant the worker had finished and wanted the tool returned. The practice made sense when people could not see around the bench partition.

The player can ask Mina to demonstrate; ask a former learner whether they understood it the same way; search for an old task note; or decide the custom is unrelated. If demonstrated, a trainee can misunderstand and move a part after the first tap. The player can clarify the signal or replace it with a spoken instruction.

This interpretation connects the phrase to the workroom rather than message delivery. It can produce a useful repair practice: describe the object and desired response, rather than using an unexplained signal. It cannot explain the doorway marks unless a separate event supports that link.

## 184. Lead C — a schedule note with missing context

The player may find an older shift slip in Rade’s shelf. It has “after the second knock” written beside a time, but the copy begins midway through a list. The slip’s source and date are known only if recorded; otherwise, the paper is undated.

The player can ask Rade to compare handwriting; ask the Archivist to describe paper and custody; look for a matching entry; or leave the slip in its current location. Rade recognizes the style as someone who wrote quickly, but cannot name the writer. The Archivist can establish that the slip came to her shelf after another record, but not who wrote it unless the deposit receipt says so.

The phrase may have meant “come after the signal,” “wait until a second person confirms,” or merely “check the time after the knock.” The missing top of the list prevents certainty. The player can attach a note about the source limits or return the slip to custody.

## 185. Lead D — the radio habit

A radio fragment can be found in an old receiver log if the current radio or chronicle authority supports such a record. It contains a short instruction: “Two knocks, then wait. If there’s no answer, leave the lamp where it can be seen.”

The message does not identify who sent it or where it was used. It may refer to a delivery, a repair, or a night watch. The player can compare the phrase to the paper note, ask Osha whether the route language sounds familiar, or decide that the similar wording is coincidence.

If the player broadcasts the fragment, they can share the wording only; share the full log; ask whether any listener recognizes it; or keep it private. A new reply may identify a local use, but any reply is a character account and should be attributed. The radio cannot provide an omniscient solution.

## 186. Lead E — the newcomer’s literal reading

Dema reads the phrase as an instruction: wait for two knocks, then open the door. They followed it once and found no one there. Dema does not know whether the signal had a reply rule. They thought the note was current because it appeared beside the current schedule.

This lead reveals the cost of unexplained inherited practices. The player can explain the likely alternatives; ask Dema what they did; add a date or scope label; remove the phrase from the active board; or invite the group to decide whether it has any present use.

Dema’s misunderstanding does not prove the old custom was foolish. It proves that the instruction was not self-explanatory to a new person. The main quest can use this information to motivate clear posting even if the mystery remains unsolved.

## 187. Investigation branches

The player may pursue one lead deeply, compare several, ask different people, or stop after finding a practical use. The investigation should never silently require every clue. Its branch states can remain compact:

- **Doorway interpretation supported:** at least one firsthand account describes the knock-and-wait custom.
- **Bench interpretation supported:** Mina or a learner demonstrates the repair signal.
- **Record interpretation supported:** a dated or source-labeled note uses the phrase in scheduling context.
- **Radio interpretation supported:** an existing log preserves the phrase with uncertain sender or location.
- **Newcomer impact observed:** Dema or another new arrival acted on the phrase without understanding its scope.
- **Multiple interpretations retained:** evidence supports more than one local use.
- **No interpretation chosen:** player leaves the phrase unresolved.

These are content descriptions, not proposed identifiers. Before implementation, the current narrative system must be checked for how it stores clue discovery, and only states with future readers need persistence.

## 188. The shared retelling scene

If the player gathers two or more accounts, they can invite Rade, Osha, Mina, Pell, Dema, or other available witnesses to compare what they remember. Participation is optional. The scene is not a trial and the player is not a judge.

Rade can say, “I copied the words. That’s all I can prove.”

Mina can answer, “I used two taps at the bench. I don’t know who took it to the door.”

Osha can add, “I carried a note once. I can’t tell you what the note meant to the person waiting.”

Pell may say, “I heard the knocks. I didn’t see who answered.”

Dema can say, “I followed what was written. No one told me there was a second step.”

The player may ask everyone to label what they witnessed; write a practical new instruction; keep separate accounts; choose one interpretation for current use while marking it provisional; or end the conversation without choosing. A person can leave when they have answered. The group can make an operational rule without reaching agreement about the past.

## 189. Mystery outcomes

### 189.1 “Wait for an answer”

The player uses the phrase to design a new delivery practice: knock, pause, wait for a response, and bring the message back if there is none. The practice is adopted only if the relevant people accept it. The ending says the new rule is inspired by the old phrase but does not assert that this was the original meaning.

### 189.2 “Do not move the part”

The player keeps two taps as a repair-bench signal and adds a visible explanation. Mina agrees to use it during one repair window, or the learner offers a different signal. The phrase is removed from the public schedule because its scope was unclear.

### 189.3 “Keep the wording, add a date”

The group retains the phrase as a historical note beside the dated copy. New residents are told it is not a current instruction. This preserves a fragment without allowing it to direct present behavior.

### 189.4 “Several uses, no single origin”

The group accepts that different people used the same phrase for different tasks. The player records the individual contexts without collapsing them into a single official account. This is the most nuanced outcome and is not available unless evidence supports multiple uses.

### 189.5 “Leave it unresolved”

The player decides the phrase does not need to be solved. They remove it from the current schedule, retain or discard the scrap according to the chosen record practice, and tell Dema what the current procedure is. The mystery is unresolved, but the practical confusion is repaired.

## 190. Faction reactions to the phrase

The phrase can attract different interpretations from representatives, but no faction owns its origin.

- The Military liaison may read it as a confirmation protocol and suggest logging acknowledgment.
- The Rebel convener may read it as a local signal used when calling out names was unsafe or unwelcome.
- The Independent broker may recognize it as a handoff condition that prevents a message from being left unattended.
- A no-commitment route may treat it as a shelter custom that outsiders should not define.

Each interpretation can be useful and partial. The player can ask for the evidence behind it, use the representative’s practical suggestion, or keep the investigation local. A faction is not rewarded for guessing correctly because there may be no single correct origin.

## 191. Mystery questline consequences

The optional quest can change:

- Whether an unexplained phrase remains on the board.
- Whether a new message practice includes a reply step.
- Whether Dema or another newcomer receives an orientation.
- Whether an old record is retained, restricted, returned, or discarded.
- Whether a character is credited with a specific observed practice.
- Whether the player’s final archive includes multiple interpretations.
- Whether a later radio or doorway encounter has a clearer instruction.

It must not change who accepted the disputed shift unless the evidence directly establishes that fact. It must not reveal a hidden villain. It must not unlock a mandatory ending. Its reward is a more informed and more legible practice, not a moral point or a unique power.

## 192. Environmental and dialogue callbacks

If the player adopts the new knock-and-wait procedure, the doorway receives a small instruction with an expiry or review date. If they choose the repair signal, a two-tap sound occurs only during that repair context and is paired with visible status. If they archive the phrase, the old scrap is dated and placed with its source note. If they leave it unresolved, the wording is no longer on the current board, but the history can remain in a private copy.

A later visitor can ask what the new instruction means. A resident explains it in one sentence. This proves the process is now legible. If nobody accepted responsibility to maintain the note, it may fade or be removed; the narrative should not assume indefinite upkeep.

## 193. Optional mystery accessibility and clue fairness

The player should not need excellent memory, audio recognition, or a specific faction route to complete the practical quest. Any clue required to establish a current rule must also be available through a readable dialogue or journal source. Audio-only radio evidence can enrich the investigation but cannot contain its sole required fact.

Clues need not all point to one answer. The player should know whether a clue is firsthand, copied, inferred, or hearsay. If a character forgets or declines, the game should not punish the player with a hidden failure. A journal can state: “Mina remembers a two-tap signal at the repair bench. She does not know whether it matches the note at the door.”

## 194. Why the mystery remains optional

The note began as an unexplained detail. Turning it into the required solution to the central quest would make all other action branches subordinate to a single lore puzzle. Keeping it optional lets players pursue curiosity, character history, and communication design while allowing others to focus on immediate survival management.

A player who solves none of it can still adopt a good current practice. A player who gathers every clue may still conclude that the original meaning is unknowable. Both runs can produce a complete ending. The mystery deepens the shelter’s past without making the game’s present depend on an answer the evidence cannot support.

## Installment 18 closing note

This installment develops “After the Second Knock” into an optional investigation with five evidence leads, a shared retelling, multiple practical resolutions, faction interpretations, and accessibility rules. It creates discovery and character texture while preserving uncertainty and keeping the central quest completable without solving the phrase.
# Installment 19 — Main-act scene packets for Acts II, III, V, and VIII

## 195. Act II scene packet — “The First Receipt”

**Purpose:** move the story from a damaged, conflicting board to a first usable evidence chain. This scene should teach the player to separate source, copy, and recipient without lecturing them about narrative systems.

### 195.1 Staging

Rade places the receipt beside the board but keeps one hand on its edge. The receipt is dry, folded twice, and marked with a stain at the corner. One time is clear. A second has been written over. The player can inspect the paper, compare the handwriting, ask who supplied it, or ask Rade to explain how he used it.

Osha enters carrying an empty envelope. She says, “I was asked to take something to the west room. I didn’t know which copy they meant.” Mina is wiping water from the board frame with a cloth. No one has the full chain.

### 195.2 First action choice

The player can:

- **Copy the receipt with source and date.** Rade helps preserve the original. The new copy is easier to handle but cannot answer who read the old one.
- **Ask Rade to read the disputed line aloud.** This is fast, but listeners may hear his interpretation as fact. The player can ask him to describe what is legible and what is inferred.
- **Ask Osha to trace the message route.** She can identify a delivery attempt or a known handoff, not confirm receipt without an acknowledgment.
- **Ask Mina to compare the written maintenance task with the physical repair.** She can verify what work appears to have happened, not who authorized the assignment.
- **Keep the receipt folded and ask the group to decide who should inspect it.** This respects custody but delays the comparison.
- **Set the receipt aside and make only a provisional next-shift plan.** The player can proceed while preserving uncertainty.

### 195.3 Branch dialogue

If the player copies the receipt:

Rade: “I’ll put the date beside it. Not because the date settles it. So we know which copy came later.”

If the player asks Osha:

Osha: “I can tell you where the envelope went. I can’t tell you what was inside if I carried it sealed.”

If the player asks Mina:

Mina: “The bracket was tightened. That doesn’t tell me who asked for the repair.”

If the player asks for a group decision:

Rade: “Then we need to decide who can see it before we decide what to write from it.”

If the player proceeds provisionally:

Mina: “That gets the shift moving. Keep the word provisional where people can see it.”

### 195.4 Scene output

The scene exits with one of three evidence states: a sourced copy exists; the receipt remains in custody and unexamined; or a provisional next-shift plan was made without relying on the receipt. Any route can continue to Act III. The source holder, copy recipient, and current assignment are described separately. A later correction can update the assignment but cannot change who held the receipt.

### 195.5 Scene-specific failures

If the receipt is too damaged to read, the player can preserve it, ask other witnesses, or act on incomplete evidence. If Rade refuses to hand it over, the player can request a readback or leave it with him. If Osha is unavailable, no delivery history is invented. If Mina is occupied, a physical check can wait or be performed by another accepted worker. The player should never be forced to use an unavailable character as the only source.

## 196. Act III scene packet — “The Copy Has a Reader”

**Purpose:** make the information audience part of the player’s action. The central question is not simply whether a copy exists, but who can use it, what it contains, and what update method it carries.

### 196.1 Staging

A new copy is on the common table. It has blank space below the current list. Dema is looking at it from the doorway but has not touched it. Rade says the original is back on the board. Osha asks whether the copy is meant to travel. One resident has requested a personal version; another has asked that private explanations stay off the wall.

The player can take the full copy to the requester; create a narrow excerpt; read the assignment aloud; invite the requester to choose a format; ask whether a second posting point is feasible; or refuse to make another copy and offer a different communication route.

### 196.2 Full copy route

The player copies all current assignments and marks the source and date. This gives the recipient context and lets them compare other shifts. It also expands the audience for personal information. The player can ask affected people whether names and reasons may be included. If a person declines, the full copy can still include the operational assignment while leaving their reason blank.

Dema may ask, “Is this one the one I follow?” Rade can answer only if it is truly current. If a later line is still under review, the copy must say so.

### 196.3 Narrow copy route

The player copies only the recipient’s assignment, the effective time, and how to ask for a correction. This protects unrelated information but depends on a clear update path. If the assignment changes, someone must find the recipient or issue a new copy.

The recipient may say, “This is enough for today. If it changes, tell me where you’ll leave the next one.” The player can set that location, request an acknowledgment, or state that no update is promised.

### 196.4 Spoken readback route

The player reads the relevant information aloud. The recipient can repeat the time or ask a question. A readback confirms what that person heard at that moment; it is not a lasting record. The player may ask Osha or another willing person to carry a follow-up, but a relay remains an attempt until confirmed.

If the recipient has a different preferred format, the player can change approach. They are not required to justify the preference.

### 196.5 Refusal and alternative route

The player may refuse a copy because supplies are limited or because the copy would expose sensitive information. A valid refusal offers an alternative if one is available: a spoken notice, a narrow line, a later direct meeting, or a clear statement that access is not yet solved. The journal should say which option was offered and whether the person accepted it.

### 196.6 Scene output and Act VIII callback

The output records what information was copied, who received it, and whether an update path was accepted. At Act VIII, the game can ask whether the copy stayed current. If the player never checked, the ending reports it as unknown. No branch grants universal access from one successful delivery.

## 197. Act V scene packet — “The Spare Part Has a Limit”

**Purpose:** turn maintenance into a choice with material and labor consequences, without making a repair automatically solve the social problem.

### 197.1 Staging

Mina places two pieces beside the damaged mounting. One is a near fit with visible wear. One is stronger but requires a second person to hold it while she fastens it. A reusable clamp is available only if the shelter has one under current inventory authority. The wall behind the board is damp.

The player can ask Mina to compare the parts; ask a Guild mechanic for a demonstration; check available stock; ask whether the board needs to be moved; request a temporary fix; or abandon the repair and use another communication method.

### 197.2 Choice: fit the near match

Mina can use the near-fit part for a temporary repair and state what it can bear. The player can accept its limited use; ask for a check date; find a stronger piece; or reject the temporary fit. If accepted, the board becomes usable but visibly marked for inspection.

A later rain scene tests the choice. The part may hold; it may shift within the stated limit; or the board may have been used outside that limit. Each outcome requires its own dialogue and repair choice.

### 197.3 Choice: wait for the stronger part

The stronger part requires a second worker and more time. The player can ask for an available volunteer, split the job into preparation and fastening, trade for an expedited part, or wait. A worker’s acceptance is explicit. If no one is available, the job remains deferred.

The delay can leave the board inaccessible or the schedule provisional. The player can use an alternate posting point or direct confirmation during the wait. This prevents waiting for the “best” repair from becoming an unavoidable best answer.

### 197.4 Choice: move the board

Moving the board reduces exposure to water or improves access, but changes who can see it and who has to walk farther. The player can ask Dema or another affected person to check the proposed location, ask Osha about delivery paths, move it to the common room, or keep it on the current wall with temporary protection.

If the player moves it without telling people, a later encounter can show that someone relied on the old location. If the player posts a relocation note, it can still be missed; confirmation requires an actual response.

### 197.5 Choice: retire the board

The player can decide that repair is not worth the ongoing burden. The current assignments must be moved to an alternate method. The old board can be retained as a historical object, dismantled, or left in place without current information. Its retirement is not proof that the group solved every access problem.

### 197.6 Output

The scene ends with one of four visible states: temporary repair with review date; stable repair with named upkeep; moved board with an access update; or retired board with an alternate process. The narrative records who did the work and whether they accepted recurring maintenance. The physical result and social agreement remain separate.

## 198. Act VIII scene packet — “Use It Once”

**Purpose:** observe the chosen practice under ordinary pressure and let the player respond to its first real limitation.

### 198.1 Opening state

The next shift begins. Someone approaches the board or alternate method and tries to use it without the player prompting them. The current arrangement may work, partially work, or fail. The player first has a chance to observe. They can inspect the record, ask a user what they understood, check whether the message arrived, inspect the repair, or let the group use the process without intervention.

### 198.2 Outcome: the practice works

The new rule is followed. A worker updates the line and another person reads it back. The player can thank the maintainer, ask whether the effort is sustainable, or leave the process alone. The scene should not imply that one successful use proves the method will always work. The journal records that it worked this time.

### 198.3 Outcome: the practice works partially

The current schedule is correct but one copy is stale. The player can update it, retrieve it, mark it superseded, or notify the person who used it. A character may say, “I got the new time from the wall. The paper in my drawer still had the old one.” The player can preserve the copy chain or reduce future duplicates.

### 198.4 Outcome: the practice was not used

The person responsible was unavailable or forgot. The player can ask whether the method is too difficult; find a substitute; simplify the rule; or accept that this shift used an informal workaround. The original practice is not erased. Its review can produce a real amendment.

### 198.5 Outcome: someone declined

A worker declines a task or declines to participate in the review. The player can ask only what is needed for the current assignment, seek coverage, or leave a gap. The decline is a valid use of the process if the group agreed people could say no. It does not mean the process failed.

### 198.6 Outcome: the review reveals new cost

A task is completed, but the maintainer says it took longer than expected. The player can reduce record detail, rotate upkeep, seek a bounded service, or keep the method and accept its cost. The game should recognize workload, not only completion.

### 198.7 Scene exit

The player can renew the rule, revise it, let it lapse, keep it provisional, or leave the decision open. If a person who must consent is absent, the rule cannot be expanded beyond its accepted scope. This creates an endgame that feels like continuation rather than a binary success screen.

## 199. Act-specific state readback table

| Act | Player-facing question | Readback must distinguish | If unknown |
|---|---|---|---|
| II | Which source can we use now? | Original, copy, observation, inference | Proceed provisionally |
| III | Who has usable information? | Full copy, narrow copy, spoken notice, none | Offer another format |
| V | What method is physically usable? | Temporary, inspected, permanent, moved, retired | Limit use and name recheck |
| VIII | What happened when people used it? | Works, partial, unused, declined, unverified | Keep current state provisional |

These readbacks let the four scene packets connect without making their outcomes identical. They also give authors a check for whether the scene has an actual branch result or only alternate prose.

## 200. Dialogue bridges between packets

**From Act II to Act III, sourced copy:**

Rade: “This is the copy I made from the receipt.”
Dema: “Then why is the time different on the wall?”
Rade: “Because the wall changed after I copied it. I should have marked that copy old.”

This dialogue is available only if a copy exists and the wall changed afterward.

**From Act III to Act V, narrow information:**

Osha: “The note got to the west room. They asked for only their shift, so that’s what I gave them.”
Mina: “Then the board can stay here if the route stays clear.”
Osha: “If the route closes, I need another way to reach them.”

**From Act V to Act VIII, temporary repair:**

Mina: “It held through one wet night.”
Player choices can be: inspect now; thank her and schedule the agreed recheck; ask whether she wants another person to learn; or leave the board temporarily marked.

**From Act V to Act VIII, retired board:**

Dema: “The wall is empty. Where do I go?”
The player can point to the new board, explain the direct-confirm time, ask which format Dema needs, or reveal that no replacement was agreed yet.

## 201. Avoiding duplicate scene work

The Act II packet should not replay the “board in the rain” scene if the player already completed it. It can reference the resulting state and begin with the receipt. The Act III packet should not repeat the full conversation about the damaged line; it focuses on audience and format. Act V handles physical method, not the entire social dispute. Act VIII tests real use without retelling the meeting.

A later scene can summarize prior events in one accurate sentence and let the player act. If a scene is skipped, it should provide only the minimum context required, not pretend the player made choices they never made. This keeps the plan expansive in branch quality rather than requiring every route to replay the same exposition.

## Installment 19 closing note

This installment develops four main acts into scene-ready packets with action choices, dialogue, failure handling, and cross-act state readbacks. Acts II and III distinguish sources and audiences; Act V makes repair choices operational; Act VIII tests the adopted practice in use. Their outcomes feed the ending without collapsing distinct histories.
# Installment 20 — Optional radio questline: “A Voice at the Edge”

## 202. Radio quest premise and canon boundary

A neighboring shelter or listening point calls over a weak channel asking whether the shift-board method can be shared. This contact is an optional audience for the player’s practices, not a new major faction and not a source of rescue. The exact location and whether such a contact already exists must be checked against current world and radio canon. If there is an existing contact, use that character and channel. Do not create a parallel radio network.

The caller has a practical question: how does the player know that the person named on a schedule received the change? The player can answer, ask questions first, send a copy of the local method, request a route report, decline the conversation, or reply only with a general principle. This lets the player reflect on the quest’s own decisions without turning the radio into a recap quiz.

The quest should never offer a guaranteed supply cache or unique ending just because the player used the radio. Any help exchanged must use the current inventory, faction, travel, and communications owners. The neighboring contact can provide perspective or a bounded service, but the shelter’s current board practice remains the player’s central concern.

## 203. First contact — “Can you hear this?”

The radio crackles between two bursts of static. A voice says, “We have two lists and one runner. The runner says both are current.” The player can respond with a request for the caller’s name and location; ask what the two lists differ on; share that the shelter also has disputed copies; give a brief method for labeling current and old records; or wait for a clearer signal.

A clear reply establishes only that the other station heard a response. It does not prove its identity or the truth of its account. If the player asks about the mismatch, the caller can describe one operational problem without naming the people involved. The player may invite a later exchange, ask the caller to repeat the detail, or end transmission.

The dialogue can vary with the player’s own route:

- If the player has a confirmed readback practice: “We ask the person to repeat the time. That tells us they heard that message. It doesn’t tell us what they’ll choose.”
- If the player uses dated copies: “We mark which copy was current and when it changed. We still check who saw it.”
- If the player keeps the matter local: “We haven’t found one rule that fits every shift. We set a temporary plan and say when it needs another look.”
- If the player’s own practice failed: “We tried a method that did not reach everyone. We’re still deciding what replaces it.”

No answer is graded as correct. A player with no settled local method can still provide an honest account of uncertainty.

## 204. Verification without suspicion theater

The caller asks for a copy of the shelter’s procedure and says they will send their current list in exchange. The player must decide how much to trust, what to disclose, and what can be checked. The scene should support practical caution without treating every stranger as a spy.

The player can ask the caller to identify a shared route or known contact; request only the operational difference between their lists; send a sample procedure without local names; ask Long Walk to verify when the signal route was active if that service exists; or decline to exchange material.

If the player sends local names, the scene states which names and why. If they send a blank template, the other shelter receives a method but no personal information. A route report can confirm channel timing or relay location, not the caller’s authority. If the player declines, the contact can still ask one general question or end the exchange.

A later message may confirm that the received procedure was legible, ask for clarification, or report that it did not work. That response belongs to the caller’s experience and is not proof that the player’s own method is universally effective.

## 205. Second contact — “What do you need from us?”

The neighboring contact says their runner has been delayed and asks whether the player can relay an update to someone traveling through the area. The player can accept one message; ask for the recipient and expiry; decline because the shelter has no available courier; offer to receive only a route report; or suggest another channel.

Accepting requires a concrete delivery instruction. The player chooses whether the message is public, private, or limited to a practical task. A message without an expiry can remain relevant too long; the player may set one or ask the sender to do so. The shelter can use Osha or Long Walk only if the person accepts the route and the current travel system supports it.

If the player accepts, a later event distinguishes the message’s departure, handoff, attempted delivery, and acknowledgment. If no acknowledgment returns, the task remains incomplete or uncertain. The player can make another attempt, ask for a different method, or return the note to sender. The neighboring shelter is not automatically blamed if a route is blocked.

## 206. Third contact — trade, aid, or no exchange

The contact offers a small item, route detail, or repair hint in return for the procedure copy. Exact item and value must be validated against current catalogs and economy rules. The player can accept the exchange; give the procedure without asking for anything; request a nonmaterial exchange such as an updated route map; ask what data the contact will retain; or refuse.

A free exchange can be generous but still has a disclosure scope. A trade can be fair but dependent on reliable delivery. A route detail can be useful and time-bound. Refusal preserves local information but gives up outside knowledge. The scene does not equate payment with exploitation or free aid with virtue.

If the current economic system cannot support a proposed barter, use a narrative information exchange or leave the offer as an unimplemented option. Do not create an invisible barter counter.

## 207. Fourth contact — the other shelter’s correction

The neighboring contact later says the copied procedure caused confusion: one person thought “current until next review” meant the list could not be changed before that date. The player can clarify the phrase; ask what they understood before correcting; send a revised example; admit the local wording was ambiguous; or stop advising the other shelter.

This branch allows the player to learn that a method that works locally can fail elsewhere. The contact’s correction does not invalidate the player’s own practice; it supplies a new design constraint. The player can suggest that a trial always include both an expiry and a change procedure. The other shelter decides whether to adopt it.

If the player’s message was private or limited, they can ask the contact not to rebroadcast the example. If the contact already shared it, the player can request correction but cannot make recipients forget the old wording. That history remains.

## 208. Radio quest resolutions

### 208.1 “The Blank Copy”

The player sends an anonymized procedure and receives a revised blank form. The two shelters compare practices without exchanging names. The result is useful but limited: the player does not know how the other shelter handles individual refusals.

### 208.2 “One Message, One Return”

The player accepts a single delivery with a stated expiry and return acknowledgment. The process succeeds only if each handoff is confirmed. If no acknowledgment returns, the ending says so.

### 208.3 “Two Rules, Both Local”

The player and the caller exchange summaries but decide their circumstances differ. Both keep their own procedures and agree only to share future route information. This is not a failed alliance; it is a bounded relationship.

### 208.4 “The Advice We Corrected”

The player’s suggested language caused ambiguity. They send a correction and admit the wording problem. The receiving shelter may accept, revise, or ignore the new example. The player gains no trust points; the story records that advice was corrected.

### 208.5 “No Further Reply”

The signal fades or the player ends contact. The shelter keeps its own practice. No hostile consequence is inferred. The contact may return later through a verified channel, but the story does not assume continued access.

### 208.6 “A Route Worth Keeping”

The player agrees to periodic route updates, not a standing personal-data exchange. The arrangement has a clear interval and a way to stop. The route report can support future travel planning through existing systems, but the radio quest does not become a new campaign map authority.

## 209. Radio fragment bank

These fragments are state-dependent and must be emitted through the current radio/event owner if implemented.

**First contact, uncertain signal:** “Two lists. One runner. We don’t know which copy went out. Can you hear us?”

**After the player sends a blank method:** “We got the form. No names on it. We can try the three fields you marked.”

**After an unconfirmed delivery:** “The message left the north stop. We don’t have the receiver’s answer.”

**After correction:** “We used your first wording. It sounded final. We changed the notice to say trial. That’s ours to maintain now.”

**If the player declines aid:** “Understood. We’ll keep to the question we asked. The channel can stay open for route conditions.”

**If the player ends contact:** a brief carrier tone, then silence. No ominous musical cue should imply retaliation.

## 210. How the radio thread supports each route

- **Military:** the liaison may request a channel summary. The player can share coverage totals while refusing personal names.
- **Rebel:** the convener may invite the neighboring shelter to compare participation methods. The player can limit the exchange to procedures rather than political claims.
- **Independent:** the broker may offer to sell a clearer receiver or route service, subject to verified stock and terms. The player can decline and continue with the existing radio.
- **No commitment:** the player can respond directly without treating the exchange as an alignment. Any repeated service is still separately accepted.
- **All routes:** the player can ignore the incoming signal. The main shift-board quest remains completeable.

The radio contact should not decide which major route is valid. It offers a perspective and a chance for reciprocal aid, with the same boundaries around consent, confirmation, and source.

## 211. Follow-up scenes after radio contact

A follow-up can arrive when the player next uses the radio, when a route report returns, or at an existing story milestone. It should not use a new wall-clock timer. If no follow-up event arrives, the player’s journal can retain the last confirmed status without inventing a response.

The player can receive a corrected copy, a request to stop using a shared phrase, a note that the exchange helped, a statement that the other shelter chose another method, or no further reply. Each is valid. A positive response should not retroactively prove the procedure worked in every situation. A negative response should not erase the practical value of the exchange.

If the player changes their own method after hearing the other shelter’s experience, that is a mid-arc pivot. The dialogue can say, “We heard how the phrase read to someone new. We changed the wording here.” It does not need to claim the other shelter taught the player a perfect solution.

## 212. Integration and content boundary

Before implementation, verify that the current radio catalog, message bridge, event subscriptions, save ownership, and data validation can represent the proposed contact and its status. Confirm whether another shelter or listening post is established in current canon. Determine whether route reports and acknowledgments already have an owning system.

Do not add a parallel inbox, radio log, route map, or contact registry. If the current radio system cannot preserve a delivery attempt separately from a reply, the content remains an unapproved proposal or is simplified to a one-time dialogue exchange. Any item or barter uses existing data and trade authority. Any travel or radiation cost uses its existing owner.

## 213. Radio quest completion criteria

The optional quest is complete when the player chooses one of these outcomes: a bounded exchange was made; one message was attempted and its status is known; the contact received a clarification; the player declined further contact; the player chose no exchange; or the signal ended without a response.

Completion does not mean the neighboring shelter adopted the player’s system. The journal records the last confirmed communication and any open request. The main quest ending can mention the contact only if it materially changed a route, resource, or practice.

## Installment 20 closing note

This installment adds an optional radio-contact questline with verification, disclosure, aid, trade, correction, and refusal branches. The neighboring contact remains a supporting audience, not a new major faction or rescue solution. The thread extends the plan’s themes beyond the shelter while keeping every communication status precise.
# Installment 21 — Campaign-context variants for the eight-act arc

## 214. Context variation without hidden moral gating

The same act can feel different under changing survival conditions. This variation should come from verified world state—weather, supplies, available people, route access, power, or an active faction service—not from an inferred virtue score. Conditions can alter timing, costs, available approaches, and dialogue, while leaving the central question available.

Before implementation, authors must identify the existing owner for each condition and verify that the current campaign exposes it to the narrative route. A condition that the game does not authoritatively track should remain a fictional detail or be removed. Do not make a new quest-local weather, supply, health, or faction-state copy.

Every context variant needs a fallback. If the source condition is missing or unreadable, the scene uses its neutral version. Unknown is not the same as favorable or unfavorable.

## 215. Act I variants — the board meets the day

### 215.1 Wet weather

The paper is damp and one corner lifts. The player can preserve, move, cover, compare, or postpone. Mina can identify a leak if the board’s condition is visible. The weather may make a route or outdoor meeting harder, but only an existing weather/travel system can produce a mechanical cost.

### 215.2 Dry and clear conditions

The board is physically intact, so the conflict is entirely about records and communication. The player can inspect two conflicting copies without an urgent repair task. A dry setting must not remove the core branch; it simply gives the player more time to ask questions.

### 215.3 Limited light

The player can move the board, use an existing light resource, read the schedule aloud, or wait. Reading aloud changes audience. Moving changes access. Waiting delays decisions. If the game does not track lighting, this stays a visible scene condition rather than a new power counter.

### 215.4 Crowded passage

Several residents need the corridor. The player may move the conversation to the table, speak individually, or make a short update in place. The crowd makes a public readback more exposed and a long meeting less practical. A busy room does not imply that every passerby heard the discussion.

## 216. Act II variants — source comparison under resource pressure

### 216.1 Paper available

The player can preserve the original and make a source-labeled copy. The cost is time and writing labor. Extra copies remain optional; paper abundance does not imply permission to circulate names.

### 216.2 Paper reserved

Sera identifies the existing reservation. The player can use a reusable surface, ask to reprioritize, trade for stock, or proceed with a spoken comparison. The quest reads inventory through its current owner if implemented. The exact reserved purpose must be verified, not invented for convenience.

### 216.3 Archive accessible

The Archivists can compare custody dates or retrieve a copy under agreed terms. They do not establish the truth of the disputed line. The player may still proceed without waiting for them.

### 216.4 Archive unavailable

The player can use local copies, direct accounts, or a provisional plan. An unavailable Archivist does not invalidate other evidence routes. The journal records which source was unavailable and what the player used instead.

### 216.5 Witnesses occupied

Rade, Osha, or Mina may be busy with other accepted work. The player can wait, ask another available person, or proceed with limited information. An occupied character is not a failure gate. If another person reports what they observed, the report’s source is named.

## 217. Act III variants — access and delivery constraints

### 217.1 No courier available

The player can provide an alternate posting point, ask the recipient to come when able, broadcast only a public operational update, or defer the copy. No absent courier is silently substituted.

### 217.2 Radio channel clear

The player can send a concise update and request acknowledgment. A clear channel reduces transmission uncertainty but does not prove that every listener understood or agreed.

### 217.3 Radio channel intermittent

The player can repeat the message, send it by another route, use a local board, or leave the update unconfirmed. Repetition consumes time or channel access only when those costs are supported. The journal distinguishes attempts.

### 217.4 Recipient has limited access

The player asks which format works: a short copy, spoken readback, direct delivery, or an agreed meeting. The character does not need to disclose a diagnosis or justify the preference. If no option is currently accessible, the quest should state that access remains unresolved and allow a practical next step.

### 217.5 Public pressure nearby

A major faction representative may ask to hear the copy. The player can share a coverage count, ask them to step away, provide an anonymized version, or refuse. Their presence does not automatically convert a local copy into a political report.

## 218. Act IV variants — absence alongside survival work

### 218.1 Essential task window

If the task has a real deadline under an existing system, the player sees the deadline and consequence. They can find a substitute, split the task, request help, or leave a clearly identified gap. The original person’s reason remains unknown unless learned directly.

### 218.2 Non-urgent task

If the task can wait, the player can hold the assignment open, contact the person, or move work without penalty beyond delay. This is an important alternative to treating every absence as an emergency.

### 218.3 Low local capacity

If the current system shows few available workers, the player can reduce scope, delay, perform the task if possible, or accept a support service. A low-capacity state should not force a moral choice between two unavailable people.

### 218.4 High local capacity

More people are available, but the player must still ask before assigning. Availability does not mean consent. A volunteer can take one shift, a task segment, or a recurring role only if each scope is accepted.

### 218.5 Outside route blocked

The player cannot assume the missing worker can be reached. They can leave a local note, choose provisional coverage, or wait for the route to reopen. If the current campaign has no modeled route blockage, do not manufacture one as a state gate.

## 219. Act V variants — different repair capacity

### 219.1 Parts in stock

The player can select a compatible or stronger part using existing item data. Stronger may still require more labor or a different mounting. A stock item does not automatically include a service agreement.

### 219.2 Parts reserved elsewhere

The player can respect the reservation, negotiate reprioritization, use a temporary method, or retire the board. The reservation source and amount must be real. The player should see which other need would be delayed if the part is used.

### 219.3 Skilled worker available

Mina or another accepted worker can inspect, fit, or teach a bounded repair. The player chooses whether to perform it now, arrange a paired task, or wait for a better work window.

### 219.4 No skilled worker available

The player can use an explicitly limited temporary fix, request Guild help, change the posting method, or leave the repair unresolved. There is no hidden “correct” answer that makes an unavailable expert appear.

### 219.5 Board no longer worth repairing

Repeated damage or poor access can make retirement sensible. The player may preserve the board as an archive surface, remove it, or abandon the space. The story asks what replaces it. If nothing replaces it yet, the ending states the resulting communication gap.

## 220. Act VI variants — correction under faction pressure

### 220.1 No outside observer

The player can correct locally, ask the affected people, or leave the dispute open. The absence of a faction representative makes privacy easier but does not remove the need for accurate records.

### 220.2 Representative expects a report

The player can provide a narrow fact, ask for more time, share a correction with consent, or decline. The representative’s deadline may matter to a service, but it does not make the report accurate by itself.

### 220.3 Two major factions are present

The player can hear each request separately, limit the audience, use a neutral local summary, or ask one faction to leave. No faction is automatically the referee. If both offer assistance, each service is separately accepted and bounded.

### 220.4 Prior report already circulated

The player cannot undo who saw it. They can send a correction, request that recipients retain both versions, or state that the report was provisional. The story records a correction as an additional event rather than rewriting history.

### 220.5 No agreement possible today

A current operational line can still be updated while the past account remains disputed. The player can choose a temporary coverage plan, postpone the historical conversation, or end the meeting. The act does not require confession or consensus.

## 221. Act VII variants — meeting capacity and participation

### 221.1 Full attendance

The group can discuss a lasting practice, but presence still does not imply consent. Each affected person can accept, object, pass, or request a different format.

### 221.2 Partial attendance

The meeting can decide only what participants have authority to decide. The player can make an interim rule and send a readback, or wait for a missing person. If there is no urgent task, waiting may be practical. If there is, the temporary plan is explicit.

### 221.3 Low energy or competing needs

If the survival simulation exposes fatigue or need pressure, the meeting can be shortened or rescheduled. The narrative may also simply show that participants are tired without changing stats. Do not invent an effect on needs if no current owner supports it.

### 221.4 Faction observer present

The observer can listen, offer a bounded service, or ask a question. They cannot vote on the shelter’s local procedure unless the group explicitly gives them that role under a clear agreement.

### 221.5 No one wants a recurring duty

The group can adopt a one-shift method, rotate only when volunteers are available, reduce the schedule’s complexity, or accept a recurring gap. An unowned duty must not be assigned automatically to Rade or Mina.

## 222. Act VIII variants — test after the world changes

### 222.1 Stable conditions

The selected procedure is tested under ordinary use. A successful first use earns no claim of universal reliability. The player can renew, revise, or leave the procedure provisional.

### 222.2 New resource shortage

The method may become too expensive in paper, light, or labor. The player can adapt it, seek supply, or let the agreement lapse. The ending records a changing constraint rather than treating prior success as invalid.

### 222.3 Character unavailable

The person responsible for maintenance is absent. The player can use a fallback, ask a willing replacement, or accept that the process does not operate today. No standing responsibility is inferred from one prior acceptance.

### 222.4 Faction service expired

The service no longer applies unless renewed. The player can request a new one-time arrangement, transition locally, or go without. If the service had been integral to the process, the ending states that dependency plainly.

### 222.5 New arrival or changed access need

A newcomer asks how to use the process, or a current resident requests a different format. The player can explain, revise, or identify the gap. A prior procedure can be adequate for one group and still require change.

## 223. Context combination examples

**Low paper plus high courier availability:** use a narrow copy and confirm delivery. The method saves paper but depends on labor and route access.

**Full supplies plus privacy concern:** create a copy but restrict its audience. Abundance does not settle disclosure.

**Low staffing plus active military service:** ask the liaison for bounded temporary labor, but ensure the assigned person accepts. A service does not convert people into available inventory.

**No radio plus an accessible second board:** direct posting can work, but requires a maintainer and a way to tell remote residents.

**Severe rain plus no repair parts:** move the board, protect the current copy, use spoken notices, or retire the location. No option makes all costs vanish.

**Full meeting attendance plus faction pressure:** the shelter can decide locally, while an outside observer changes what people are comfortable saying. The player can move the private discussion elsewhere.

**No faction relationship plus archive support:** a local service can preserve records without major alignment. The player can keep political and practical decisions separate.

Each combination changes available approaches and costs. None decides whether the player is good, honest, loyal, or competent.

## 224. Context-variant content review

Before a variant is authored, confirm:

1. Which live owner provides the condition?
2. Is the condition observable to the player?
3. Does the condition change the available action, its cost, or only the scene text?
4. Is a neutral fallback defined?
5. Does this variant preserve the main quest’s core decision?
6. Can a player continue if the relevant system signal is absent?
7. Is the consequence applied through the current owner?
8. Does the scene distinguish temporary pressure from long-term practice?
9. Is the action still available on other faction routes?
10. Does the variant add a real branch or only duplicate prose?

## Installment 21 closing note

This installment adds campaign-context variants for all eight acts, with conditional approaches for weather, light, supply, communication, staffing, repair capacity, faction pressure, and changing access needs. The variants preserve the main decision while making survival conditions matter through verified sources and visible costs.
# Installment 22 — NPC initiative and off-screen consequences

## 225. NPC agency when the player is not present

The shelter should not stop acting whenever the player leaves a room. Characters can carry out tasks they accepted, ask for help when a problem exceeds their role, or decline a new request. This creates a world that feels inhabited and produces new branch states. It must not become a hidden simulation that silently decides major outcomes while the player is away.

An off-screen action is justified only when the character accepted the task or the action follows an established ordinary responsibility. A person who once repaired a bracket is not automatically responsible for every future repair. A person who carried one message is not a permanent courier. An expired trial does not continue by default.

NPC initiative should be reported when the player next has a fair opportunity to learn about it. If the action has a serious consequence, provide an early signal through an event, journal, radio, or visible world state. Do not let an unseen action invalidate hours of play without warning.

## 226. Rade updates the current copy

If Rade accepted a bounded posting role and a date arrives, he can update the current line while the player is elsewhere. The update is limited to the information he has and the scope he accepted.

**When he has a confirmed change:** he posts it and leaves the source/date. Later, the player can inspect the result, ask who was notified, or read back to an affected worker.

**When he has an unconfirmed report:** he marks it provisional or asks before posting, depending on the agreed rule. He does not guess the assignment.

**When he is unavailable:** the copy remains unchanged. The player sees that the role has no substitute and can ask for coverage, simplify the procedure, or accept the stale state for a limited interval.

**When he has withdrawn from stewardship:** the story does not show him continuing the task out of habit. The player must appoint another willing person or use a different method.

A journal callback might read: “Rade changed the east shift from the delivery note. He marked the notice provisional; no acknowledgment has returned.” This describes the act and its limit.

## 227. Osha declines or reroutes a message

If Osha accepted one defined route, she can carry that message. If a new request exceeds the agreed scope—unsafe route, private content without instructions, repeated trips, or an expired service—she can ask the player to confirm, propose a safer route, or decline.

If the player previously agreed to a return instruction, Osha follows it. If no return instruction exists, she uses only the default method she explicitly accepted or returns to ask. She does not invent consent from the recipient.

When she declines, the player can choose another courier, wait, broadcast a narrow operational fact, or proceed without contact. If Osha offers a safer route, it may take longer and should have a visible cost if the current travel owner supports it.

The off-screen outcome preserves the difference between “no message sent,” “route declined,” and “attempted but unconfirmed.” Her refusal is a boundary, not a random relationship penalty.

## 228. Mina stops a repair at its stated limit

If Mina accepted a temporary brace and inspection date, she may inspect it when that date arrives if she is available. She can confirm it held, report wear, or stop using it. If the condition is unsafe, she can prevent further use only through the game’s current command authority and clear player-facing communication.

If Mina did not accept a recurring inspection duty, the scene cannot quietly assign one. The player may have to ask again or let the temporary state expire. If a learner accepted one bounded check, that learner can perform only the agreed scope.

An off-screen inspection has a readback: Mina writes the date, tells the player, or updates an existing journal/event source. If the current system has no valid notification path, the inspection should happen in a visible scene rather than a silent mutation.

## 229. Dema tests whether the process explains itself

Dema can use a posted procedure without waiting for the player to explain it. If the labels are clear, Dema follows the current copy or asks one focused question. If the procedure is ambiguous, Dema may pause, use an old copy, or ask another resident. These actions follow what the character could see.

Dema does not act as a universal test suite or represent every person’s access needs. Their feedback is one person’s experience. The player can invite other residents to try the method, but each participant may have different needs and preferences.

A failed first use becomes a chance to revise the wording. It does not prove that the character was careless. The story can show the cost of ambiguity and then give the player a repair path.

## 230. Iven protects the kitchen rotation

If Iven accepted a meal-time adjustment for one meeting, that exception ends after the meeting. He can return to the prior rotation, ask whether the new recurring time is agreed, or decline another adjustment if it displaces work.

If the group adopts a recurring review outside meal time and Iven agrees to the change, he can update his task schedule through the current owner. He should not be shown preparing extra meals or absorbing another person’s duty without an accepted arrangement.

If the player asks for urgent help during a meal task, Iven can state what is already underway and ask whether the player wants to defer, find a cover, or accept the delay. This makes his work visible and gives the player a real choice.

## 231. Sera preserves reservations and reports changes

Sera can maintain an existing reservation only if that is part of the current inventory process and her role is confirmed. If a shipment changes the available amount, the inventory owner supplies the value. Sera can explain which planned use was affected, but cannot create or destroy stock through dialogue.

If a reserved item is no longer needed, Sera may ask whether it can be released. That question does not automatically free it. The owner or accepted decision process releases it. If another resident uses the item, the game records actual consumption through the current system.

This agency creates natural follow-ups: Sera asks whether the schedule copy can use a substitute; the player decides whether to reprioritize; the ending reflects what was spent. She does not become a parallel inventory manager.

## 232. Pell distinguishes observation from interpretation

Pell can independently record a night event if he accepted one bounded watch or witness task. His account states what he saw and heard. If he hears two knocks but sees no one answer, the record says exactly that.

If Pell is not on watch, the scene does not give him the information. If he leaves his role, later night observations come from another source or remain unknown. If a faction asks for his statement, the player asks whether he agreed to share it, unless the current evidence was already public.

His initiative is to preserve a careful account or refuse an ongoing witness duty, not to reveal a hidden culprit.

## 233. Player intervention windows

NPC initiative works only if the player has a chance to respond at useful times. Each autonomous action should expose one or more intervention windows:

- **Before acceptance:** the player can clarify scope or suggest another person.
- **At acceptance:** the character agrees to a specific task or duration.
- **During work:** a character can pause and request help if a new condition appears.
- **After completion:** the player can inspect or receive a readback.
- **At expiry:** the task returns to the player or group for renewal, revision, or closure.

A character may still complete a task before the player intervenes if the agreement was clear and the task is bounded. The design should not create an arbitrary confirmation click for every ordinary action. The key distinction is whether the action exceeds what the character accepted or changes another person’s consent.

## 234. Off-screen change matrix

| Character | Accepted initiative | Not authorized to do automatically | Player readback |
|---|---|---|---|
| Rade | Post a confirmed correction within a trial role | Resolve a disputed account or publish private reasons | Current copy and source |
| Osha | Carry one specified message by accepted route | Carry new private content or repeat a route indefinitely | Attempt, handoff, acknowledgment |
| Mina | Inspect a temporary repair on agreed date | Own all maintenance or certify beyond observed limits | Inspection and next check |
| Dema | Use or question a posted procedure | Represent all new arrivals or accept hidden duties | What was understandable |
| Iven | Adjust one agreed meal-time handoff | Absorb recurring labor without agreement | Meal/task handoff |
| Sera | Report reserved stock through inventory authority | Change inventory quantities locally | Actual available/used item state |
| Pell | Record an accepted watch observation | Infer intent or serve as permanent witness | What he saw and did not see |

## 235. NPC initiative after player neglect

If the player ignores a request, characters may still choose how to protect their time and work. The response should be proportionate and legible:

- Rade may stop posting unofficial updates after his trial expires.
- Osha may return an unopened message to its sender.
- Mina may tag a temporary repair as unchecked and decline further use.
- Dema may ask another resident for the current schedule.
- Iven may return to the agreed meal plan rather than keep moving it.
- Sera may preserve reserved stock for its original need.
- Pell may stop taking extra night observations.

These actions do not sabotage the player. They follow boundaries and resource terms. The player can respond by negotiating, finding alternatives, or accepting the changed capacity. The game should not convert neglect into a sudden total relationship collapse.

## 236. Off-screen continuity checks

Before any autonomous event is authored, confirm:

1. The character actually accepted the relevant duty.
2. Its scope and expiry are known.
3. The character has access to the necessary information and resource.
4. The action is possible through an existing owner.
5. The player receives a truthful readback.
6. Another character’s consent is not inferred.
7. Failure and unavailability have a fallback.
8. The event is deterministic or selected through an existing deterministic system.
9. A save/load boundary cannot cause the action to occur twice.
10. The player can still choose a different method next time.

## Installment 22 closing note

This installment gives the shelter cast bounded initiative, refusal rights, and visible follow-ups when the player is away. It makes autonomous behavior depend on accepted duties and current owners, not hidden approval scores or arbitrary off-screen simulation.
# Installment 23 — Playable vignettes for autonomous shelter activity

## 237. Dawn vignette — Rade posted the correction

**Availability:** Rade accepted a bounded posting role, a confirmed correction arrived, and the player was away.

At dawn, the player finds the date rewritten and a narrow correction attached to the current list. Rade is folding the source copy back into its sleeve. He says, “I changed the time that was confirmed. I didn’t fill in the open line.”

The player can inspect the source; ask who has received notice; thank Rade and leave the board in use; ask him to mark the update provisional; or ask whether he wants someone else to check the next copy. Rade can answer that the person at the west room acknowledged the change, while the east room has not. The player can send a second notice or leave that line unconfirmed.

If Rade exceeded the agreed scope, the player can ask why, request a correction, or discuss a new scope before future changes. The journal preserves both the posted edit and the scope issue. If the action was authorized, the callback is simple: the trial process was used once, not proven forever.

## 238. Midday vignette — Osha returned the note unopened

**Availability:** Osha accepted one delivery route with a return instruction, the recipient was absent, and the player was elsewhere.

Osha places the folded note on the table. The seal is intact. “No one answered. I brought it back.” She has not opened it.

The player can ask her to return it to the sender; hold it for a second attempt; ask the sender for a new instruction; give the recipient a public notice that a message is waiting; or close the delivery request. If the player chooses another route, Osha may accept it or state that she is not available. The player can ask someone else or leave the note in custody.

If the sender is unavailable, the player may preserve the note until a stated review point or return it to the agreed place. If the recipient later asks whether Osha delivered it, she says exactly what happened. The event does not become a completed quest merely because a route was attempted.

## 239. Afternoon vignette — Mina marked the brace unchecked

**Availability:** a temporary repair reached its agreed review date while Mina was available.

Mina has drawn a small line beside the brace and written “not checked” underneath. She has not removed it. “I didn’t have the right light to see the lower fastening. I’m not calling it safe.”

The player can move the board, supply a light if an existing resource is available, ask another willing person to help inspect, stop using the brace, or continue with a clearly limited provisional method. Mina can accept a second inspection window or decline if another task conflicts.

If the player accepts temporary use, the game states which uses remain safe or available according to the fictional design and current mechanics. If the project cannot model the limit reliably, the board is simply not used until inspected. The marker stays until a real check occurs.

## 240. Meal vignette — Iven returned to the agreed time

**Availability:** Iven agreed to move one review away from meal preparation, but no recurring change was accepted.

The player sees the original meal rotation on the slate. Iven is at the pass-through, stirring the pot. He says, “We moved it yesterday. I didn’t move today’s meal.”

The player can keep the meeting time and ask for cover; shift the meeting again; take the urgent issue in a short exchange; or defer the full review. Iven can describe what each option does to his work. If a cover is offered, the covering person explicitly accepts.

If the group did agree to a recurring new time, Iven’s line differs: “The new time is holding so far. Ask me after the next rotation.” The scene never treats one exception as a permanent schedule change.

## 241. Supply vignette — Sera preserved the reservation

**Availability:** a scarce item was reserved for an existing use, and the player has not authorized reprioritization.

Sera points to the remaining stack. “I kept the two sheets for the labels. There’s enough for one schedule copy if you want to change the plan.”

The player can preserve the reservation; ask the relevant owner to reprioritize; use a reusable surface; trade for more stock; or reduce the schedule’s copied detail. Sera reports the actual amount only through the current inventory owner. If a sheet is spent, the item count changes through that owner and the journal reports what was used.

The scene makes the reserved need visible without treating Sera as the final decision-maker. The player can change the priority, but the cost is clear.

## 242. Night vignette — Pell observed the knock

**Availability:** Pell accepted one watch observation and saw a delivery attempt.

Pell stands by the east doorway, holding his coat against the cold. “Two knocks. No answer that I heard. I didn’t see the person inside.”

The player can ask him to write a statement, ask whether he wants his name attached, record only the attempt, or leave the report out of the archive. Pell can decline a written statement and still provide a spoken account. If a faction asks for his identity, the player checks the agreed sharing scope.

If a recipient later says they heard nothing, the two accounts can coexist. Pell heard the knocks; the recipient did not hear them. The scene does not choose who is lying.

## 243. Vignette variation rules

Short autonomous vignettes should differ according to task acceptance and scope, not player virtue:

- If a task was accepted and completed, show what was done and what remains.
- If a task was attempted and failed, report the attempt and failure source.
- If a person declined, show the boundary and a next practical option.
- If no acceptance exists, do not show the task as completed off-screen.
- If the player authorized only one attempt, do not extend the duty.
- If the character acted within scope, a simple readback is enough; do not force a praise scene.
- If the character exceeded scope, give the player a repair or clarification option.
- If the player missed the update, the journal can report it without accusing them of neglect.

## 244. Vignette pacing

Only one autonomous vignette should claim the first interaction after a major player return, unless the current game already supports a readable event queue. If multiple events occurred, present a short summary and let the player choose which to inspect first. High-impact changes—safety limitation, failed delivery affecting an imminent task, or a service expiry—should be surfaced before low-impact ambient notes.

The order should be deterministic when several events share a moment. If the current narrative scheduler has an established priority model, use it. Do not add a new local queue that conflicts with campaign event ownership. Every skipped vignette remains available through a concise journal or later conversation if the action still matters.

## Installment 23 closing note

This installment adds six short scene vignettes and pacing rules for surfacing autonomous character actions. It makes accepted work visible, preserves declined requests, and gives the player follow-up choices without turning every NPC action into approval or punishment.
# Installment 24 — In-world records, marginalia, and conflicting summaries

## 245. Documents are actions in the world

A paper record can show who chose to make information durable, who was meant to read it, and which uncertainty remains. It is not a neutral encyclopedia page. A note has an author, purpose, audience, and limits. The player may choose to create, annotate, restrict, copy, return, or discard a document when the story supports that action.

Documents must not become a second quest database. Their gameplay consequences are routed through the current journal, quest, item, or archive authority. If a physical note is purely diegetic, it can be flavor; if it changes a branch, the underlying fact must be owned by the current narrative system. A note that disappears after a scene cannot serve as the only proof of a persistent outcome.

## 246. Player choices around a document

When the player finds or receives a document, they may:

- Read it immediately if it is addressed to them or publicly available.
- Ask the holder for permission when ownership or privacy is unclear.
- Ask for a readback or summary.
- Compare it with another source.
- Make a copy if the materials and consent allow.
- Add an annotation that preserves the original wording.
- Return it to its holder.
- Place it in an existing archive under agreed access terms.
- Decline to keep or circulate it.
- Leave it where found, accepting that its future custody is unknown.

These are not all required in every scene. The interaction should name the practical consequence: making a copy exposes it to another reader; leaving it may mean it is lost; restricting it can slow review. The player should not be punished for respecting an ownership boundary.

## 247. Document artifact — Rade’s current copy

**Purpose:** operational schedule for the next shift.

**Baseline text:** “Current for: next shift. Updated: [date from campaign authority]. Source: Rade, from the receipt and direct confirmations listed below.”

A current copy can show confirmed lines, provisional lines, and open coverage separately. It does not display private reasons by default. If the player chose a different record policy, the template changes: a private channel may carry individual updates, or the board may simply show the next task and an instruction to ask.

**Player interaction:** inspect source; ask Rade what changed; mark an unconfirmed line; request a narrow copy; or leave it alone.

**Variant:** if Rade’s role expired, the document carries “maintainer not confirmed” rather than pretending he still owns it.

## 248. Document artifact — Osha’s route slip

**Purpose:** record a message attempt without claiming that the person received it.

**Example:** “Departed: west door. Intended recipient: named by sender. Handoff: none. Returned: note unopened. Reply: none.”

The slip can be read by the sender or held for a courier review. It should not include message content unless the sender requested a content copy and the recipient scope allows it.

**Player interaction:** ask Osha to read the route details; request a second attempt; add an expiry; or return the slip to her.

**Variant:** after confirmed acknowledgment, the last line says “recipient acknowledged the stated time,” not “recipient agreed to the shift” unless that was what they explicitly confirmed.

## 249. Document artifact — Mina’s maintenance card

**Purpose:** state the physical condition and next check for a repair.

**Example:** “Lower fastening replaced. Holds current paper load. Check after rain or before the next full schedule. Does not certify the wall behind it.”

The card names the task performed, the limits observed, and the next inspection trigger. It does not automatically identify a future maintainer unless one accepted that role.

**Player interaction:** inspect the repair; ask for a demonstration; choose to keep using the board within the stated limit; or stop use until a check.

**Variant:** a temporary brace is marked with its exact inspection period. A missing check is labeled “unchecked,” never assumed passed.

## 250. Document artifact — Iven’s handoff note

**Purpose:** record a meal or shared task transfer when a meeting changes the usual time.

**Example:** “Pot covered until second break. Iven accepted the first handoff only. Next person has not confirmed.”

This note makes meal labor visible but must not become a second food inventory or needs tracker. It describes the handoff that people agreed to.

**Player interaction:** confirm who took the task; change the time; ask for another volunteer; or mark the handoff open.

**Variant:** if no substitute accepted, the note reads “task deferred; no cover named.”

## 251. Document artifact — Sera’s reservation slip

**Purpose:** distinguish stocked material from material already committed to another use.

**Example:** “Two sheets held for labels. One sheet available for current schedule. Count follows stores record; ask before reprioritizing.”

The inventory system owns quantities. The slip can point to a reservation but cannot change item state by itself.

**Player interaction:** inspect the planned use; request reprioritization through the proper owner; use a substitute; or leave the stock reserved.

**Variant:** when a reservation ends, the current owner updates availability and Sera replaces the slip. A player cannot spend an item because an outdated note says it is available.

## 252. Document artifact — Pell’s witness statement

**Purpose:** preserve one observation from a bounded watch.

**Example:** “Heard two knocks at the east door. No reply heard. Did not see who was inside. Time approximate.”

The statement can be named or anonymous according to Pell’s consent and the agreement in play. It must not infer why nobody replied.

**Player interaction:** ask Pell to read it aloud; request a correction; attach a second account; or keep it out of a public report.

**Variant:** if Pell was not on watch, the statement does not exist. His general role does not grant him omniscience.

## 253. Document artifact — faction coverage report

Every major faction can produce a report with its own purpose. The story may show these documents disagreeing without immediately judging intent.

**Military report:** “Coverage assigned: yes. Completion confirmed: pending.” This distinguishes staffing from finished work.

**Rebel meeting note:** “Present participants adopted a one-shift trial. Two invited residents have not replied.” This distinguishes participation from universal agreement.

**Independent invoice:** “One delivery, count accepted with one discrepancy noted. Renewal not included.” This distinguishes payment from continuing service.

**Local summary:** “Current task covered. Historical account remains disputed. No outside report requested.” This distinguishes operation from affiliation.

The player may inspect, annotate, share a narrow summary, request correction, or decline to circulate. If a representative edits their own report, the change is a new version with its own source. The original should not disappear merely because it is inconvenient.

## 254. Document artifact — newcomer’s guide card

**Purpose:** tell a new resident how to find the current instruction and how to ask for a correction.

**Example:** “Current list is dated. If your name or time looks wrong, ask before taking the task. You may decline an assignment. A crossed line may remain here because the correction is disputed.”

Dema can test whether the guide answers ordinary questions. The card may include a location or contact, but that information must match the current process. If the board is retired, the guide changes to the alternate channel.

The player can ask Dema to review it; ask another resident to read it; or leave the guide unwritten and provide direct orientation. A guide does not prove every resident has seen or understood it.

## 255. Document artifact — supporting-current receipt

An Archivist receipt records custody date, source, access terms, and review date. A Long Walk receipt records departure, route checkpoint, handoff, and return status. A Guild label records part condition, fitting date, and stated use limit. None certifies the shelter’s agreement.

The player can keep a copy, ask what the receipt means, challenge an inaccurate field, or return it to its owner. Cross-current scenes can put these receipts side by side and ask what each one can prove.

## 256. Optional document quest — “The Words Left Out”

A player notices that several records summarize an event differently. The military report says the slot was covered, the shelter copy says the substitute accepted one shift, and Osha’s route slip says the original notice was unconfirmed. The player can create a combined summary, preserve the documents separately, ask each author to review their wording, or decide that no combined report is needed.

### 256.1 Combined summary

The player writes only facts supported by the source documents: a substitute accepted one shift; the original notice remains unconfirmed; work completion is pending. This is concise and useful but cannot carry every person’s interpretation.

### 256.2 Separate documents

The player keeps each document with its author and scope. This preserves provenance but requires more effort for readers who want the whole sequence.

### 256.3 Author review

Each author can approve, correct, or decline the proposed summary. A faction representative does not have to agree with the shelter’s wording, and the shelter does not have to publish the representative’s report.

### 256.4 No combined report

The player can decide the documents serve different audiences. The ending records that choice and who can access each item. This is a valid resolution if the player can still explain the current work arrangement.

## 257. Document consistency rules

- Dates come from the authoritative campaign time source if one is available.
- A document identifies its source when the source is known.
- A copy keeps its own date and does not become current automatically.
- A quote remains distinguishable from a summary.
- Corrections attach to earlier versions rather than silently erasing them.
- A private reason is not copied into a public record without consent.
- A physical witness statement describes observation, not inferred motive.
- A service receipt records service, not faction endorsement.
- An empty field means unknown or not recorded; it does not mean “no.”
- The player can decline to make a durable record.

## 258. Diegetic text variation by action

The player’s earlier actions can change the wording of the same form.

**After a public readback:** “Read aloud to those present. West room acknowledgment: confirmed. East room: no response recorded.”

**After a private message:** “Direct notice requested. Public reason omitted. Recipient response: confirmed for next shift only.”

**After a failed delivery:** “Attempt returned unopened. Current assignment remains unconfirmed.”

**After a correction:** “Earlier line retained. Current time changed by direct confirmation. Review requested.”

**After a declined disclosure:** “Reason not included. Coverage plan changed.”

These variants are brief but meaningful. They let the player see how the record reflects their scope and method.

## 259. Document discovery and content pacing

Documents should be discovered where a person would reasonably keep them: a shelf, workbench, delivery point, or current posting location. The game should not hide mandatory instructions in optional files. Optional documents deepen character perspective, reveal how practices evolved, or provide a different account.

A player can skim or leave a document. A concise journal summary can state its source and main consequence. If a document opens a branch, its action options must be explicit. If it is flavor-only, do not pretend that collecting it increases faction or character standing.

## 260. Document branch audit

For each authored record, verify:

1. Who wrote it?
2. What did they know firsthand?
3. Who is allowed to read it?
4. Is it current, superseded, provisional, disputed, or unknown?
5. What decision can the player make after reading it?
6. Does the document alter state or merely present flavor?
7. Who owns its persistence?
8. Can a correction attach without erasing history?
9. Does the document include personal information beyond its purpose?
10. Is the player’s refusal to read or keep it still a viable path?

## Installment 24 closing note

This installment adds nine document types, a branched document-comparison quest, and rules for authorship, access, correction, and state-aware wording. Records become interactive evidence and flavor, while their ownership stays with the existing quest, inventory, journal, archive, radio, and faction authorities.
# Installment 25 — Additional play approaches and their trade-offs

## 261. Play approaches are loops of action, not character classes

The earlier playstyle routes cover common verbs. This installment adds approaches that may arise from how a player combines those verbs across scenes. These are optional design lenses for dialogue, quest routing, and replay review. They do not create a class selection, stat bonus, or hidden score. A player can move between them at any time.

A play approach is useful only when it changes the player’s practical method and creates a recognizable trade-off. It is not enough to rename “good” and “evil” as “careful” and “ruthless.” The approaches below are differentiated by what they inspect, what they ask, how they share authority, and what burden they accept.

## 262. Record analyst

**Core loop:** compare sources, identify provenance, preserve uncertainty, then make a limited operational decision.

- **Act I:** inspects the board before asking who changed it.
- **Act II:** compares the receipt’s date, copy, and source.
- **Act III:** chooses a document scope and labels the copy.
- **Act IV:** separates absence from refusal and records the basis for any substitution.
- **Act V:** preserves old and current versions through a repair.
- **Act VI:** attaches corrections without erasing the earlier account.
- **Act VII:** proposes an edit procedure and source label.
- **Act VIII:** checks whether the record matches actual use.

**Strength:** later disputes are easier to review and corrections have a clear source.

**Cost:** record work can delay immediate coordination, consume scarce materials, and make people feel observed. The player may learn what happened without making anyone feel heard. Affected people can decline to be documented.

**Distinct branch:** the record analyst can choose a deliberately small archive. They do not preserve everything; they preserve what a future decision actually needs.

## 263. Access designer

**Core loop:** ask how a person can receive or use the information, provide a workable format, then test it with the actual recipient.

- **Act I:** identifies whether the board location is reachable and readable.
- **Act II:** asks who needs the source record and who needs only a current instruction.
- **Act III:** offers written copy, spoken readback, alternate location, or direct contact.
- **Act IV:** checks whether a substitute can receive the task scope in time.
- **Act V:** chooses a location and layout with affected users.
- **Act VI:** separates a public correction from a private explanation.
- **Act VII:** makes sure absent residents have a channel to respond.
- **Act VIII:** asks a user what they understood instead of assuming success.

**Strength:** fewer people are excluded by the default board or meeting.

**Cost:** maintaining multiple formats or locations takes time and can create divergent copies. A spoken route has limited durability; a second board needs a checker; a private channel depends on a reachable contact.

**Distinct branch:** the access designer may choose one flexible format per person rather than standardizing the entire shelter. The story can show that personalized access is useful but adds coordination work.

## 264. Contingency planner

**Core loop:** name a likely failure, choose a fallback, assign an owner, and decide when the fallback activates.

- **Act I:** asks what people will do if the wet copy becomes unreadable.
- **Act II:** keeps a safe provisional source in reserve.
- **Act III:** records what happens if delivery fails.
- **Act IV:** chooses a substitute threshold or delay limit.
- **Act V:** sets a repair fallback for a temporary part.
- **Act VI:** plans how to operate if the dispute remains unresolved.
- **Act VII:** gives the trial rule an expiry and backup.
- **Act VIII:** tests one failure path rather than only the expected use.

**Strength:** the shelter can continue when one person, supply, or route is unavailable.

**Cost:** designing backups consumes materials and can make a simple process feel heavy. Too many fallback rules can be as confusing as no rule. People may resist being named as alternates.

**Distinct branch:** the planner can choose a low-resource fallback, such as a spoken handoff, rather than trying to duplicate the full primary process.

## 265. Consent facilitator

**Core loop:** invite input, check what each person accepts, keep nonparticipation visible, and avoid turning the player into an arbitrator of private motives.

- **Act I:** asks affected residents what task they expected.
- **Act II:** asks who is comfortable handling the receipt.
- **Act III:** lets recipients select an information format.
- **Act IV:** asks whether a substitute accepts the scope.
- **Act V:** checks whether the repair learner wants another step.
- **Act VI:** offers separate conversations or a shared meeting.
- **Act VII:** records participant scope and objections.
- **Act VIII:** renews a practice only with the people whose duties change.

**Strength:** agreements are more likely to reflect the people doing the work.

**Cost:** decisions can take longer and a person may choose not to participate. The player may have to leave a gap rather than force closure. A facilitator can also over-center conversation when an urgent action is needed.

**Distinct branch:** the player can establish consent for the process itself: who may convene, who may speak for another, and what can be decided without a missing person.

## 266. Minimal-process operator

**Core loop:** reduce coordination overhead to the smallest procedure that safely meets the current need.

- **Act I:** identifies the one task that must be clear today.
- **Act II:** copies only the relevant source line.
- **Act III:** sends the minimum usable information.
- **Act IV:** covers only essential work and states what is deferred.
- **Act V:** repairs only the part needed for the next shift.
- **Act VI:** corrects the operational line while leaving the historical dispute open.
- **Act VII:** refuses a standing rule if a one-shift agreement is sufficient.
- **Act VIII:** checks whether the minimum process held under use.

**Strength:** fewer forms, meetings, and copies; less labor on administration.

**Cost:** less historical context, weaker redundancy, and more dependence on direct knowledge. A future worker may not understand why the current line exists. If the minimal channel fails, there may be no archive or alternate.

**Distinct branch:** the minimalist can choose not to resolve history because doing so does not change today’s safe action. The story treats this as scoped decision-making, not avoidance, while leaving the dispute available later.

## 267. Capacity builder

**Core loop:** teach a bounded skill, spread responsibility, and verify that the learner wants to continue.

- **Act I:** asks who can inspect the board or route.
- **Act II:** invites another person to make a copy with source labels.
- **Act III:** teaches a second delivery format.
- **Act IV:** trains a substitute to accept a task clearly.
- **Act V:** pairs a repair lesson with a check date.
- **Act VI:** helps a resident correct the record themselves if they want.
- **Act VII:** creates a rotation with opt-out and replacement.
- **Act VIII:** lets the trained person use the process without the player stepping in.

**Strength:** reduces dependence on the player or a single expert.

**Cost:** training consumes time, learners may not want the role, and shared skill does not guarantee shared availability. A lesson cannot become a permanent job without acceptance.

**Distinct branch:** the capacity builder can teach a narrow task while leaving sensitive decisions with the original owner. For example, a learner can update the date without deciding who is named.

## 268. Negotiating observer

**Core loop:** hear competing institutional and local claims, ask what each can commit to, and prevent a discussion from becoming an unbounded promise.

- **Act I:** asks the representative to state their actual operational need.
- **Act II:** compares their form with local records.
- **Act III:** limits the audience and terms of any copy.
- **Act IV:** asks whether an outside substitute is one-time or recurring.
- **Act V:** negotiates a part or service under explicit limits.
- **Act VI:** corrects reports without treating every disagreement as hostile.
- **Act VII:** separates the faction service agreement from the shelter’s policy.
- **Act VIII:** checks delivery, renewal, and actual use.

**Strength:** outside resources become available without silently transferring authority.

**Cost:** negotiation takes attention and a service may be withdrawn. Clear terms can still be unequal when alternatives are scarce. The player may need to accept a short-term compromise while naming the dependency.

**Distinct branch:** the observer can accept a service while rejecting the representative’s interpretation of the event. This keeps political disagreement and practical exchange separate.

## 269. Quiet witness

**Core loop:** observe, record only what was seen, avoid premature intervention, and speak when an action or safety need requires it.

- **Act I:** listens to several accounts before posting.
- **Act II:** asks who handled the receipt.
- **Act III:** watches whether a recipient can use the copy.
- **Act IV:** leaves the reason for absence unknown and tracks only the coverage result.
- **Act V:** observes a repair demonstration before accepting it.
- **Act VI:** allows both accounts to be stated before proposing a procedure.
- **Act VII:** can let residents draft the rule while keeping a factual record of the decision.
- **Act VIII:** checks whether the rule was actually used.

**Strength:** preserves space for evidence and lets other people lead.

**Cost:** silence can be mistaken for agreement, and waiting can delay a necessary correction. The player may have to state explicitly that observing does not mean endorsing. A quiet witness who never acts can leave urgent work uncovered.

**Distinct branch:** the player can make a short intervention to protect a boundary, then return to observation. The approach is not passivity; it depends on knowing when observation is no longer enough.

## 270. Combined play approaches

- **Record analyst + access designer:** create a source-labeled narrow copy and test it with the recipient; cost is maintaining a separate history.
- **Contingency planner + minimalist:** choose one low-cost fallback and avoid duplicating every piece of information; cost is limited redundancy.
- **Consent facilitator + capacity builder:** ask who wants to learn, then create a bounded rotation; cost is time and the possibility that no trainee accepts.
- **Negotiating observer + privacy steward:** accept outside supplies while limiting personal data; cost is that the provider may refuse the terms.
- **Quiet witness + direct operator:** observe until a deadline, act to cover the urgent need, and leave the uncertain history intact; cost is a delayed conversation.
- **Minimal-process operator + record analyst:** preserve only the event necessary to support later correction; cost is that unrelated context is lost.
- **Access designer + contingency planner:** provide two ways to receive changes and define which one takes over when the first fails; cost is upkeep.
- **Capacity builder + negotiator:** use a service visit to teach a local worker, then end the contract; cost is that training may be incomplete or declined.

These pairings should produce recognizable outcomes without requiring a player to declare a preferred style. The game can reflect them through dialogue and process state only when the relevant actions occurred.

## 271. Playstyle and route availability

No play approach should be exclusive to a faction route. The Military may make contingency planning more valuable, the Rebel route may make consent facilitation more visible, the Independent route may make negotiation more frequent, and no-commitment may foreground local capacity. But players can use any approach in any route.

Faction route choices alter available help and pressure. They do not lock the player out of asking a worker, preserving a source, limiting disclosure, or making a direct repair. If a specific resource is unavailable, the player can use a different approach at its own cost.

## 272. Playstyle narrative feedback

A character may reflect on an action pattern only when enough specific actions support it. One example:

- After repeated source comparisons: “You keep asking where the copy came from. I know why now.”
- After one timely intervention: “You stepped in before the next assignment. That helped today.”
- After several accepted lessons: “People know the repair now. They still get to decide who does it.”
- After repeated privacy boundaries: “You’ve kept the reasons off the wall. The schedule still needs to be clear.”
- After a negotiated service ends: “We used their part. We don’t owe them the next decision.”

The dialogue does not label the player as a type or award a rank. It acknowledges a method and invites a response.

## 273. Play-approach design test

A proposed approach is worth keeping if:

1. It produces different actions across multiple acts.
2. It has a recognizable benefit and cost.
3. It can combine with at least two other approaches.
4. It remains available on each major faction route.
5. It can be abandoned or changed without a penalty score.
6. It does not duplicate an existing class system unless the game already owns that mechanic.
7. It receives feedback through world state or dialogue.
8. It does not require the player to be morally consistent.
9. It does not guarantee a superior ending.
10. Its consequences are supported by facts the current quest owner can represent.

## Installment 25 closing note

This installment adds eight additional play approaches—record analyst, access designer, contingency planner, consent facilitator, minimal-process operator, capacity builder, negotiating observer, and quiet witness—plus hybrid combinations and design checks. It expands the player’s methods without turning them into mutually exclusive classes.

## Installment 26 — Supporting currents with their own stakes

The next layer of expansion gives local support networks enough identity to matter without promoting them into replacement major factions. They are not four new campaign powers. They are overlapping groups of people who organize around a practical need: carrying messages, repairing equipment, checking routes, or making aid legible to the people receiving it. Membership is voluntary and porous. One resident may help the repair bench and carry a message on another day; nobody is required to choose a permanent badge.

These networks should be treated as provisional authored content. Before implementation, verify whether the current faction catalog or quest graph already has suitable identities and owners. If no such owner exists, this proposal remains a content plan until the responsible integrator chooses the smallest supported representation. Do not add a parallel loyalty meter or make a local working group a second campaign coordinator.

The player’s relationship with each network is expressed through observable exchanges: a route report accepted, a repair lesson shared, a copy restricted, a missed handoff acknowledged, or a commitment allowed to expire. Each network has internal disagreements. That is essential. If every member wants the same outcome, the group becomes a dialogue kiosk rather than a supporting cast.

## 274. The four working networks

The following working names make the quest drafts readable. They are descriptive labels, not assertions that these exact organizations already exist in project canon.

| Working network | Shared task | Internal disagreement | What it can offer | What it cannot decide |
|---|---|---|---|---|
| Signal Hands | Carry short messages between staffed points | Whether a relay should repeat every detail or only the actionable part | A second delivery channel, a time check, a correction | The shelter’s policy or the truth of a message it did not witness |
| Bench Crew | Repair and teach maintenance tasks | Whether scarce parts should go to the fastest repair or the most reusable tool | A demonstration, a part diagnosis, an apprentice | Inventory amounts, ownership, or who must work |
| Route Witnesses | Report passable paths and observed hazards | Whether a route can be called usable after one safe crossing | A recent observation, a marked alternative, a second witness | Guaranteed travel safety or command over a route |
| Common Table | Coordinate small mutual-aid exchanges | Whether a shared record protects recipients or exposes them | A distribution handoff, a private correction, a witness to terms | Who deserves aid, or an automatic claim on shelter stores |

Each network can appear on any major route. The Military may ask Signal Hands for confirmation; the Rebels may rely on their distributed relays; the Independents may negotiate delivery terms. Those differing uses change pressure and resource access, not the network’s basic identity. A network member can refuse a faction request and still cooperate with the player on a local task.

## 275. Signal Hands — “The Message That Arrived Twice”

At the start of the arc, two versions of a short message arrive by different paths. Both say that a delivery window changed. One includes the name of a resident who is not present; the other omits the name but gives no source. The delivery itself is not yet late. The immediate risk is that the shelter will assign work against a message nobody has confirmed.

The Signal Hands disagree about the cause. Lio wants to repeat the full message so nobody can accuse the relay of hiding a detail. Vara wants to repeat only the changed time because names do not help people meet the window. A third carrier, Jun, says the relay is already carrying stale instructions from a previous shift. None is simply right: full repetition can expose someone; a narrow message can omit context; silence can preserve a bad schedule.

**Branching actions:**

- Ask both carriers to trace the origin before updating the public board. The schedule remains unchanged for one scene, but the correction can later name the source and scope.
- Post the changed time with a clear “source unconfirmed” note. Workers can prepare around uncertainty without treating it as fact; the faction representative may ask for a firmer answer.
- Repeat the full message to maximize reach. This can help a late worker respond, but creates a later consent and privacy conversation if the named resident objects.
- Hold the message and send a runner to the delivery point. This spends time and may miss the window, but creates direct evidence.
- Ask the network to split the relay: one actionable public update and one private explanation to the affected people. This takes an extra handoff and only works if someone accepts that task.

The middle quest tests whether the network can correct its own copies. A relay reaches the north workbench, but the southern copy remains unchanged. The player can appoint one correction owner, ask for a two-person confirmation, mark both copies with an expiry time, or stop using paper for this one message and make direct contact. Each choice has a different maintenance burden. None grants the Signal Hands a permanent right to edit the shelter board.

In the closing quest, the Military liaison requests a named confirmation; the Rebel caller asks for a locally editable relay; the Independent broker offers a paid delivery run with a return receipt. On the no-commitment route, the player can negotiate each request separately or decline all three and keep the network’s limited local service. The Signal Hands’ ending state records what they can reliably deliver, what terms they rejected, and whether they trust the player to distinguish a confirmed message from a useful rumor.

## 276. Bench Crew — “One Bearing Left”

The Bench Crew arc begins with a worn bearing removed from a pump assembly. The repair is feasible, but the part is also the only compatible spare for a small handcart used to move supplies. The choice is not “save lives or save property”: the pump repair reduces a recurring water delay, while the cart may make several smaller deliveries possible. The immediate urgency is uncertain, and no one has authority to declare the cart expendable for everyone else.

Mara argues for a repair that lasts through the next rotation, even if it consumes the bearing. Eno argues for a temporary sleeve that keeps both the pump and cart usable at reduced capacity. Suli has a different concern: the proposed permanent repair can be performed by only one person, leaving the shelter dependent on that person. The group has a technical disagreement, a labor question, and a distribution question. The player must not collapse these into one morality choice.

**Branching actions:**

- Ask for a testable temporary repair. It buys time and preserves options, but requires a visible check interval and may fail if nobody accepts the follow-up.
- Commit the bearing to the pump after consulting the actual inventory owner and affected workers. The pump becomes more reliable; cart deliveries require a new route or substitute.
- Reserve the part while testing whether another bearing can be salvaged. This avoids a premature commitment but ties up stock that another project may need.
- Choose the sleeve and train a second worker. The repair becomes less dependent on Mara, at the cost of material and instruction time.
- Ask the Military for a replacement under an explicit delivery and ownership term. This can solve the shortage, but future repair access may depend on the agreement.
- Ask the Rebels to organize a salvage search. The group can find a substitute, but the search may consume labor needed for current operations.
- Ask the Independent broker to trade for a part. The part may arrive reliably, while the trade can create a debt or return obligation.

Every route retains at least one path that does not require joining a major faction. A player can use an existing tool with a shorter service life, repair one device while scheduling the other, or postpone the decision until new evidence arrives. The narrative should show who bears the inconvenience rather than silently turning the choice into a global pump statistic.

Later, the Bench Crew can respond to how the player treated the work. If the player accepted a lesson, a different worker may take the next inspection. If the player insisted on a quick repair without a check, the crew can point to the missed follow-up and ask whether they should trust the next promise. If the part was reserved and later released by its owner, the group can keep the reservation process without claiming the player was indecisive. This feedback is about a specific procedure, not a hidden virtue score.

## 277. Route Witnesses — “The Chalk Mark Has a Date”

The Route Witnesses maintain short, local observations: a passage was clear at a particular hour, a marker was missing, or a crossing became difficult after a storm. Their arc begins when two reports conflict. One witness crossed the service alley before dusk. Another returned later and found it blocked. A third has copied the earlier report onto a map without its time.

Nera favors a bold route map that people can read quickly. Tovin wants every mark dated, even if that makes the map crowded. Bex says the travelers who use the alley already know it is unreliable and objects to outsiders treating the map as a guarantee. Each position carries a practical cost: legibility, precision, and local trust compete for space.

**Branching actions:**

- Keep both observations with times and witnesses. The record is trustworthy but denser; a traveler may need help reading it.
- Replace the old mark with the newer obstruction. This improves immediate clarity but removes evidence of how conditions changed.
- Mark the route “unknown pending check.” This avoids false certainty while potentially diverting scarce travel capacity.
- Send two witnesses on a bounded inspection. It adds labor and risk; the result can separate a one-time blockage from a continuing hazard.
- Let the route users choose their own notation after a short trial. Their map may be clearer for them, but outsiders may need an explanation.
- Keep the observation private for one shift while warning those directly affected. This protects the witnesses from unwanted exposure, but can leave other travelers uninformed.

On the Military path, the route report can be attached to a coverage request, but the liaison must not treat it as a guarantee. On the Rebel path, distributed copies can make local correction faster, but the player must decide how stale copies are retired. On the Independent path, a broker may offer transport only if the route is described as passable; the player can reject the wording while still negotiating a limited crossing. With no commitment, the witnesses can keep a small chalk-and-verbal system whose range is modest and whose uncertainty is explicit.

The late arc places a witness under pressure to identify who reported a hazard. The player can share the name with permission, share only the time and location, obtain a second account, or refuse the request and accept the possibility that the faction will suspend its service. The choice affects that witness’s willingness to report future conditions, the route’s correction speed, and the faction’s confidence in the shelter’s reports. It does not alter whether the reported obstruction physically existed.

## 278. Common Table — “A Name Is Not a Portion”

The Common Table is a working circle that helps people coordinate small exchanges: a borrowed utensil, a shared delivery, an offered repair hour, or a requested substitute. It is not an aid authority and does not determine eligibility. Its quest begins when a resident’s requested portion is copied into a public handoff list, and that resident asks for the request to be removed. Another resident argues that removing the name will make the handoff impossible to verify.

The disagreement is not solved by choosing either full secrecy or total transparency. The player can separate the handoff from the recipient’s reason, let the recipient choose a private identifier, record the quantity with a trusted witness, or leave the request off the list and make a direct handoff. Each option changes how errors are detected and who must spend time checking them.

The faction-linked complication varies by route. A Military clerk may need a count to confirm that a promised delivery occurred. A Rebel organizer may want a public list so contributors can see that aid moved. An Independent trader may require a receipt to renew the exchange. On each route the player can offer proof of the transaction without exposing the recipient’s explanation, although some counterpart may decline that narrower proof. The supporting network negotiates the handoff; the major faction still owns its own agreement.

Three character outcomes are possible without reducing them to approval. The resident may accept a private identifier but later ask for it to be deleted. The witness may accept responsibility for a count but refuse to hold personal details. The organizer may keep the public total and ask for an independent check. The player can respect those limits even when the process becomes slower. If the player ignores a stated limit, later cooperation becomes harder because the breach is concrete and remembered.

The Common Table’s contribution is not that it makes aid “fair” by proclamation. It makes a small exchange easier to perform, question, correct, or decline. Its ending state may be a public schedule with private recipients, a witness-based count, a direct exchange with no standing record, or a paused service until the group agrees on a workable boundary.

## 279. Cross-network quest: “The Shift That Does Not Line Up”

In the fifth act, the player discovers that a repair visit, a delivery, and a route check have all been scheduled for the same narrow window. The Signal Hands carried the repair time, the Bench Crew offered a worker, the Route Witnesses marked the crossing available earlier that day, and the Common Table prepared a handoff. Every local action is reasonable on its own. Together, they create a bottleneck.

The quest presents four verifiable facts and one important unknown:

1. The route was clear when the most recent witness checked it.
2. The repair worker accepted the scheduled visit but not an extension.
3. The message confirming the delivery reached one board but not the second.
4. The Common Table has prepared the handoff but has not transferred the item.
5. It is unknown whether the crossing will remain clear at the next travel interval.

The player can resolve the collision by narrowing the repair task, splitting the delivery, moving the handoff to an alternate point, requesting a new route check, or asking one group to stand down. No choice preserves all four plans unchanged. The point is not to punish the player with a surprise; all constraints are visible before the commitment is made.

The supporting groups have distinct roles in the response. Signal Hands can carry a correction but cannot decide who loses a work slot. Bench Crew can say what repair can safely be deferred. Route Witnesses can provide a fresh observation but cannot guarantee the next crossing. Common Table can hold the exchange or redirect its handoff if the recipient agrees. The player decides which commitments to renegotiate and which costs to accept.

## 280. Action-derived outcomes across the networks

The game should not ask “Which group do you support?” as a single deciding prompt. Instead, let several small actions establish a working relationship. A player who asks Signal Hands to label uncertain messages, accepts the Bench Crew’s inspection interval, preserves route report dates, and asks recipients before naming them will earn a different kind of cooperation than a player who repeatedly requests speed, centralizes every update, and asks groups to absorb unassigned work.

Potential outcome facts, subject to the existing campaign-state and save owners, include:

- which network completed a bounded task;
- whether the player honored a stated scope or deadline;
- whether a report was confirmed, corrected, or left uncertain;
- who accepted recurring work and who accepted only one shift;
- whether a service is available, paused, or expired by agreement;
- whether a correction was delivered to all intended copies;
- whether a named person consented to disclosure;
- whether a major faction received a full, narrow, or no report.

These facts are more useful than a generic “support” value because they can drive dialogue and scenes directly. They also avoid falsely treating a group as a single mind: one member can retain trust while another declines a specific task. The implementation plan must map each proposed fact to an existing authority before authoring data. If there is no appropriate owner, leave it as a narrative condition pending integration rather than inventing a new persistent ledger.

## 281. How supporting currents touch the major faction endings

The support networks should alter the texture and reliability of the major ending, not replace the ending’s central commitment.

- **Military ending:** a narrow, confirmed service schedule can show disciplined coordination; a schedule with visible uncertainty can show that the shelter learned to report limits; a schedule built on unaccepted labor can produce a strained final exchange. Signal Hands may deliver the final correction, Bench Crew may cover one repair, Route Witnesses may qualify the approach, and Common Table may confirm a handoff. None commands the Military or changes the route’s strategic outcome.
- **Rebel ending:** distributed correction copies can make local decision-making tangible; a direct witness process can make trust less dependent on one caller; a failed correction can show the cost of decentralization. The groups remain local collaborators and do not become a unified Rebel council.
- **Independent ending:** expiring agreements, receipts, and accepted renewals can show that cooperation survives without permanent allegiance. A refused renewal can still be a successful ending if terms were honored and the refusal was clear.
- **No-commitment ending:** small services may continue with limited scope, or the player may let them expire to protect capacity. The ending should acknowledge what local work can sustain and what it cannot replace. Neutrality does not magically preserve every service without cost.

The epilogue can combine at most a few of these callbacks. Prefer one remembered action and one concrete consequence over a paragraph listing every group. If the player’s path was mixed, reflect the mixed record rather than forcing consistency. A careful disclosure choice in one act does not erase a rushed labor decision in another.

## 282. Branching scene: “Who carries the correction?”

**Availability:** at least two networks have participated in the cross-network scheduling quest; a shared update needs to reach the right people before the next work interval.

The first copy is already posted. The second is in a bag by the door. A carrier says the route is open according to the latest check. A repair worker says their time cannot move again. The person who prepared the handoff says the recipient has not agreed to have their name used. The player has one short interval to choose a correction plan.

**Available responses:**

- Ask Signal Hands to carry only the changed time and keep the recipient detail private. This is fast if a carrier accepts, but the repair worker may still need a direct confirmation.
- Ask Route Witnesses to verify the approach first. This improves confidence and delays the update.
- Move the meeting to the repair point and assign the delivery to a different route. This may work if the recipient consents and the substitute accepts.
- Suspend the handoff and preserve the item. This protects the recipient’s boundary but risks losing the delivery window.
- Ask each group what it can still do, then reduce the plan to the smallest confirmed task. This costs dialogue time but can prevent an unstaffed promise.

**Reactions:** if the player chooses a route check, a Signal Hand may say, “I can carry what you know. I won’t turn yesterday’s mark into today’s promise.” If the player narrows the update, the Common Table witness may say, “That’s enough to find the parcel. It doesn’t explain why it’s theirs.” If the handoff is paused, the recipient can thank the player for asking, while another worker plainly describes the missed opportunity. These lines should not frame the cost as punishment or the boundary as effortless.

**Persistent narrative consequence:** the journal records the plan actually completed, not the option chosen in dialogue. If the carrier leaves before the correction, the intended delivery remains unconfirmed. If a substitute accepts and completes the handoff, the service record reflects that. If the recipient withdraws consent, the public copy is corrected through its owner and the narrative records the scope of the correction.

## 283. Character voice by operational tension

Supporting members should sound different because their attention falls on different details, not because each has a slogan.

- The Signal Hand asks, “Which copy are you looking at?” and notices when a message has no time attached.
- The Bench Crew member asks, “Who checks it after we close?” and taps the repaired joint before calling it finished.
- The Route Witness asks, “When did you cross?” and dislikes a map that turns one safe passage into a promise.
- The Common Table witness asks, “Did they agree to leave that there?” and remembers who had to carry the extra container.

Voice lines vary with familiarity, urgency, and past experience. A member who trusts the player can be concise; distrust does not require hostility. A confident professional can still refuse a task. A nervous resident can still make a good observation. Avoid making every disagreement a dramatic confrontation: much of the tension comes from someone asking for one more minute, one name removed, or one clear answer before beginning work.

## 284. Practical implementation boundaries for these arcs

This installment is authored quest design, not a request to build four new subsystems. Before implementation, the integrator should check current quest identifiers, faction relationships, dialogue conditions, save ownership, catalog schemas, and route consumers against live source and data. The branch facts should be reduced to the smallest representation the current quest/faction authority supports.

Where the game already exposes a suitable fact, reuse it only after verifying its semantics. Do not overload a morality band, relationship score, or generic faction reputation field to mean “the player asked who saw the message.” A narrative choice may be recorded as a quest outcome if that is an existing supported pattern. If not, implementation requires a bounded design decision and ownership claim before adding state.

Content acceptance should be demonstrated by reachability: each proposed branch needs a valid entry condition, an action the player can take, a consequence that the responsible owner can represent, and a reachable follow-up or closure. A string present in JSON is not an integrated quest. A line conditioned on an impossible combination is dead content. The plan therefore treats every story beat as a graph node to validate, not as proof that a runtime route already exists.

## 285. Installment 26 closing note

This installment gives Signal Hands, Bench Crew, Route Witnesses, and Common Table provisional identities, internal tensions, multi-act quest arcs, and roles in cross-network coordination. It also defines how those supporting currents can shape Military, Rebel, Independent, and no-commitment endings while leaving the player’s actions—not a faction badge or a good/evil score—as the source of meaningful variation.

## Installment 27 — Resident arcs that change the working arrangement

The supporting networks give the settlement a wider social texture. This installment turns four recurring residents into people with their own multi-step aims: Dema wants newcomers to understand what is being asked of them; Iven wants meal work to stop becoming the shelter’s invisible emergency buffer; Sera wants reservations to remain truthful when plans shift; Pell wants his observations to be useful without having his silence or presence misrepresented.

These are not companion recruitment arcs. A resident’s story should not become a reward track in which the player earns access by choosing the right dialogue. Each arc concerns a problem the person already has, offers more than one useful way to participate, and may conclude with the resident declining further work. The player can help, complicate, or leave the issue unresolved. Each resolution changes a local arrangement or a relationship, not the resident’s ownership of their own life.

The arcs can occur in a different order and do not require the player to complete all four. Crossovers create richer callbacks when the relevant facts exist, but no arc should be a hidden key to another resident’s basic quest. The fact gates below are narrative proposals; an implementation pass must confirm which existing quest or campaign state can represent them.

## 286. Dema — “The first explanation”

**Personal question:** When a new person arrives, what does the shelter owe them before asking them to work?

Dema is asked to explain the day’s routine because she is good at noticing where the instructions fail. She agrees to help once, then learns that people have started treating her explanation as an official rule. She never accepted that role. Her arc is about making a useful orientation possible without assigning her permanent responsibility for every newcomer.

### Stage A — A request that sounds like an order

A new resident is told to “take the west shift after the bell.” They do not know which bell, where the west post is, or whether the request is optional. Dema sees the exchange and asks the player to clarify the instruction before it is repeated.

The player can ask the newcomer what they understood, give a direct explanation, point to the existing guide, or ask Dema to demonstrate. If Dema explains, she may choose her own words and stop when the newcomer understands. The game should not turn this into an automatic assignment to mentor all future arrivals.

The newcomer’s response differs by what was unclear. If the location was the issue, a marked route may help. If “after the bell” was ambiguous, a time or event reference is needed. If they did not want the shift, clarity alone does not create consent. This branch establishes the practical gap without deciding that Dema must fill it.

### Stage B — A guide that speaks for whom?

The shelter asks for a short orientation card. Dema points out that the draft includes a rule she has never heard anyone agree to. The player can remove it, ask the procedure owner to verify it, mark the card “trial guidance,” or retain it until the next meeting. A faction representative may ask to copy the card as a standard for other sites.

If the player verifies each instruction, the card takes longer but separates accepted procedure from suggestion. If they ask Dema to rewrite the whole guide, she can accept a bounded editing task or decline the larger role. If they copy the draft immediately, the later correction belongs to whoever posted it; Dema should not be blamed for text she did not approve.

### Stage C — Teach, rotate, or retire

After several arrivals use the guide, Dema says she wants a way to stop being the only person asked to explain it. Three outcomes are available: another resident volunteers to review it; the shelter keeps a simple card and lets arrivals ask anyone; or the group decides the card creates more confusion than it solves and returns to direct orientation.

The player can invite people to volunteer, but cannot appoint them by implication. A volunteer may take one shift, review only the map, or refuse recurring responsibility. If nobody accepts, Dema can still end her own role. The content must honor that limit rather than presenting an unfilled rota as Dema’s failure.

### Stage D — Resolution variants

- **Shared guide:** Dema contributed, another resident accepted upkeep, and the card has a clear review date. Dema remains available for occasional feedback but is not its permanent owner.
- **Direct welcome:** The card is retired; residents explain the routine face to face. This preserves flexibility and costs repeated time.
- **Bounded reference:** The guide records only locations and verified contacts. People explain local exceptions themselves.
- **Unresolved handoff:** The guide remains incomplete because no one accepted review. Dema can state what is unknown and stop carrying the task alone.

Each result can receive a different short epilogue. On the Military route, a copied guide may need a local version marker. On the Rebel route, the group may decide who can revise a distributed copy. On the Independent route, the guide can be shared under a limited-use agreement. With no commitment, it remains a local tool with no promise that it will travel beyond the shelter.

## 287. Iven — “Before the meal line”

**Personal question:** Can urgent work fit around meals without quietly making one person responsible for every delay?

Iven’s first scene is not an invitation to optimize him. A repair review, delivery, and meal preparation have collided, and someone suggests that Iven can “just move the pot.” He says he can delay one meal once; he cannot keep absorbing every scheduling conflict. The player’s task is to coordinate an actual handoff, not persuade Iven to become more accommodating.

### Stage A — Name the work in conflict

The player checks which task is time-sensitive, what is already underway, and whether anyone has accepted meal coverage. They can keep the meeting time and find a willing cover, reduce the meeting to its urgent question, move the full review, or defer the non-urgent work. Iven can state the cost of each option, but should not be the only person asked to solve the conflict.

If the player takes the meal task, the scene demonstrates a direct cover. If they find another volunteer, that person accepts a defined interval. If the review moves, the game records only the new time accepted by the participants. One moved meeting does not rewrite the recurring schedule.

### Stage B — The exception becomes expected

In a later act, another faction request arrives near meal preparation. The Military liaison wants predictable service hours; a Rebel organizer wants a meeting when more residents are present; an Independent broker offers a delivery window that overlaps the kitchen rotation. Iven asks whether the previous exception has become the new assumption.

The player can quote the accepted schedule, negotiate a different window with the requester, arrange a temporary cover, or decline that service window. If the player says “Iven can cover it,” the game asks whether he accepted this specific request. If not, the plan is not staffed. This is a meaningful failure risk, but it is also recoverable: the player can renegotiate, reduce the task, or accept a missed opportunity.

### Stage C — A rotation with an honest limit

Iven suggests a trial rotation only if the group wants one. The player can help set a check-in date, ask volunteers what they will cover, create a one-day backup, or keep the original arrangement and stop calling it temporary. A rotation can fail because a volunteer is absent; the recovery branch asks what will happen next time instead of rewriting the volunteer’s earlier consent.

If the player pressures Iven into recurring cover, he may withdraw from the extra task while continuing his ordinary meal work. The narrative should make the distinction visible: he has not abandoned the shelter; he has stopped accepting an undefined obligation.

### Stage D — Resolution variants

- **Accepted rotation:** At least two people accepted named intervals and a review point. The schedule is more robust but takes coordination.
- **Single bounded cover:** Iven or another resident accepted one defined handoff. Future conflicts still require a fresh agreement.
- **Protected meal window:** Meetings move around meal preparation. The faction service is narrower or delayed, but kitchen work is less disrupted.
- **No recurring change:** The player and residents keep the current arrangement and acknowledge that future conflicts remain open.

Each is a legitimate resolution. None is the “best worker” ending. The late callback is a scene in which Iven can say what he is willing to do next, in his own terms. The player may accept a limit without gaining a special reward; that is the point of letting the resident’s boundary matter.

## 288. Sera — “The number on the slip”

**Personal question:** How can a reservation remain useful when the plan changes faster than the stock count?

Sera notices that a supply slip still reserves sheets for labels, while a new schedule needs one sheet now. The current inventory owner confirms the actual stock; Sera can explain the reservation’s purpose but cannot change the quantity herself. One resident wants the schedule posted immediately. Another says the labels are needed to prevent confusion during the next delivery. The player chooses how to resolve a conflict between planned uses, not which person is more deserving.

### Stage A — Trace the reservation

The player can ask who requested the reservation, whether the need still exists, whether a reusable surface is available, or whether the schedule can be shortened. If the original requester is present, they can confirm, revise, or release the reservation. If they are absent, the player may preserve it, ask the owner to make a bounded reprioritization, or wait for direct confirmation.

This branch can produce a useful delay: a faction report leaves later, but no stock is spent against an uncertain release. Or the player may choose to spend the sheet under the applicable authority and accept that labels must use another method. The game should show the actual item change through its current inventory owner.

### Stage B — Delivery discrepancy

At a later handoff, the received stack differs from the expected count. Sera recalls that the count was uncertain before the handoff; the delivery carrier says the manifest was correct when they left. The player can record the discrepancy without accusation, compare the manifest with the receiving count, ask the carrier to witness a recount, or defer reconciliation until another person is present.

If the player publicly accuses the carrier without checking, the carrier can decline future witness work. If the player records a discrepancy without deciding its cause, both the shelter and the carrier can keep working while the count is reviewed. If the count is corrected, the original record remains as an audit trail according to the existing inventory/logging design; it should not be silently rewritten.

### Stage C — Reservation policy by example

Sera proposes that every reservation say what it is for and when it should be reviewed. Another resident worries that this creates more paper than the shelter can maintain. The player can test the rule for only scarce items, keep reservations oral with a named follow-up person, use a visible marker, or leave the process unchanged and accept the risk of stale slips.

The action determines Sera’s participation. She may maintain the limited trial for one shift, ask someone else to help, or decline continued upkeep. The player does not get to turn her observation into a permanent clerical assignment. A plan can be sound and still lack an accepted owner.

### Stage D — Resolution variants

- **Purpose and review date:** Scarce items receive bounded reservations; someone accepts periodic review.
- **Owner confirmation:** Reservation changes require the item owner’s explicit decision; the process is slower but responsibility is clear.
- **Short trial:** A temporary method is tested on one delivery, then evaluated before wider use.
- **No standing reservation:** The settlement uses available stock directly and accepts that work may need to compete again later.

Faction-specific callbacks concern evidence and terms. The Military may request a count before scheduling; the player can provide the verified amount and uncertainty. The Rebels may prefer a shared view; the player can share totals without personal reasons. The Independent broker may require proof of what was received; a witnessed discrepancy can be enough, or the broker can reject it. A refusal does not grant the broker ownership of shelter stock.

## 289. Pell — “The second knock”

**Personal question:** What does a witness owe after reporting what they saw—and what do other people owe the witness?

Pell’s arc returns to the optional night event. He heard two knocks but did not see who knocked or whether anyone inside was awake. A resident says there was no delivery. A Signal Hand says the message log shows someone tried the east door. None of those facts proves why the attempt failed. The scene can lead to a practical improvement without resolving the mystery.

### Stage A — Record without filling the gaps

Pell can give a spoken account, write a statement, attach his name, remain anonymous, or decline to be part of the record. The player can record the time and sound without identifying him, ask another witness to compare accounts, or leave the event unrecorded. If Pell declines, the quest continues through the delivery procedure rather than stopping at his refusal.

The player can ask whether he wants a follow-up, but cannot turn his participation into proof that he was present at every later event. If the player names him after he declined, the story should treat that as a breach with specific consequences: he may refuse future witness requests and insist on correcting the public copy.

### Stage B — Improve the next attempt

The group can add a second knock interval, use a different contact point, request a return message, or keep the current arrangement. Pell may help test one method, but he can limit his role to one night. The affected recipient can propose another approach or say they do not want additional contact. The player chooses between reliability, burden, and privacy; no universal alarm system is assumed.

### Stage C — Competing interpretations

A faction representative asks whether the missed contact was intentional. The player can provide only the confirmed attempt, share both accounts, ask the representative to state why the inference is needed, or refuse to speculate. The Military may need a reason to reassign coverage; the Rebel organizer may want to know whether the local contact path failed; the Independent broker may want proof before renewing a delivery. The player can address those operational needs without claiming knowledge of intent.

### Stage D — Resolution variants

- **Bounded witness practice:** Pell accepts occasional, explicitly limited observations. The group distinguishes what he saw from what he inferred.
- **Alternate contact method:** Pell steps away from the witness role; another agreed method is used.
- **Two-account record:** Separate reports coexist, with neither forced into a single explanation.
- **Unresolved event:** The shelter improves the next handoff while leaving the identity and intention of the knocker unknown.

That last outcome is complete. It gives the player a meaningful practical result while preserving an unanswered human event. The arc should not reveal a secret culprit merely because the player exhausted dialogue options.

## 290. Shared side sequence — “A task without an owner”

After two or more resident arcs have reached an interim outcome, the player can encounter a common problem: a posted schedule needs one correction, a reserved item needs review, a meal handoff is due, and a witness statement is waiting for its chosen scope. The player is not asked to complete every task personally. They are asked to find the owners and determine which tasks actually need to happen before the next shift.

This sequence branches according to earlier actions. If the player established a review date with Sera, that review can happen now. If an alternate witness method was accepted with Pell, it can be used. If Iven agreed only to one handoff, the schedule cannot quietly assign him another. If Dema’s guide has no accepted editor, the player can leave it marked incomplete rather than presenting an unapproved version as settled.

The player can triage the tasks, ask each resident to name their own priority, recruit a willing helper for a bounded task, or tell the faction representative that the shelter can confirm only one commitment. Any uncompleted work remains visible, with a reason where known. The scene’s strongest outcome is not a perfect board; it is a board that does not disguise uncertainty or unaccepted labor.

## 291. Major-route variations without character capture

The major route changes the pressure around these personal arcs, but should not absorb the resident’s independent aim.

- **Military:** Representatives offer standard forms, a dependable delivery window, or a way to escalate urgent requests. The player can accept the resource and still keep Dema’s local guide local, Iven’s schedule consensual, Sera’s counts under the inventory owner, and Pell’s statement within its agreed scope.
- **Rebel:** Organizers offer peer distribution and fast local correction. The player can accept those strengths while naming the work of maintaining copies, asking who accepted each task, and allowing a resident to opt out of public participation.
- **Independent:** Brokers make a narrow exchange possible with clear terms. The player can define an expiry, ask what evidence proves completion, and refuse a clause that claims ongoing labor after the exchange ends.
- **No commitment:** The player can negotiate with each party case by case, preserve local practices, or decline services. The story should make the reduced reach and added coordination visible; independence does not make supplies appear or conflicts disappear.

Residents do not become faction representatives by helping with a route-specific task. Dema may explain a shared procedure without endorsing the Military. Sera may verify a delivery without becoming an Independent clerk. Pell may provide an account to a Rebel organizer without joining the group. Their participation remains scoped to what they accepted.

## 292. Relational outcomes are specific, not points

At the end of each arc, the player should have a concrete way to describe what changed. Examples include: “Dema helped test a short guide, then handed its upkeep to a volunteer”; “Iven accepted one cover, but the weekly time stayed the same”; “Sera’s reservation was released by its owner, and the count was reconciled”; “Pell’s account stayed anonymous, and the next contact method changed.”

Dialogue may remember whether the player kept a promise, asked before sharing, or returned to correct an inaccurate copy. It should not report a hidden total such as “Dema trust +2.” When a character’s willingness changes, show the specific experience that informed it. Someone can decline a new task while still having a respectful relationship with the player.

The arcs should preserve contradiction. The player might protect Pell’s privacy and still pressure Iven into a badly timed task. They might mishandle Sera’s reservation, then make amends through a clear correction. These are coherent human histories; do not collapse them into an alignment axis or require a single consistent persona.

## 293. Failure, absence, and recovery

Each resident arc must remain playable if the resident is absent, unavailable, or declines participation. A substitute can state only what they know. The absence may delay a confirmation, narrow the available options, or leave a question unanswered; it must not create invented testimony.

Recovery follows the type of failure:

- A posted instruction can be corrected by its actual owner or marked uncertain while a replacement is sought.
- An unstaffed meal handoff can be returned to the prior schedule, covered by a new volunteer, or allowed to fail with a truthful consequence.
- A spent reserved item can be reconciled and its next use renegotiated; the old slip is not treated as current truth.
- A witness whose identity was exposed can request a correction and decline future participation; the game cannot purchase forgiveness with a generic gift.

For permanent losses, the arc can close with a changed arrangement rather than a forced recovery. If a service ends, the epilogue records what the settlement can no longer rely on. If trust is damaged, characters can cooperate on urgent work while limiting future disclosure. Recovery is a process, not a universal reset button.

## 294. Epilogue combinations

If the player has completed several resident arcs, choose one or two callbacks based on the most recent relevant action and the most consequential unresolved task. Do not display four achievement summaries. A short shift-change scene can carry multiple facts naturally: a guide sits in a new owner’s hands; the meal rotation remains posted; Sera checks a date; Pell passes the door without taking watch.

Possible combinations:

- **Shared knowledge, limited duty:** Dema’s guide is maintained by a volunteer; Iven’s meal window remains protected; the Military receives the updated service time but no staffing guarantee.
- **Local correction, open terms:** the Rebel route carries a corrected schedule; Sera has a review date for the next delivery; Pell’s report is shared only with the people who requested it.
- **Narrow exchange, clear expiry:** the Independent broker receives a verified handoff count; Dema’s guide stays local; the next service is explicitly unpromised.
- **Small circle, accepted limits:** no major faction takes ownership of the process; two residents keep one task each; other services expire because nobody accepted them.
- **Mixed and imperfect:** one promise was kept, another failed, and the residents decide what they still want to do together. The ending can be warm without pretending everything worked.

## 295. Sample shift-change scene

The board has been erased in three places and rewritten in two. One corner still has yesterday’s route mark beneath a line of chalk.

“That’s the old crossing,” Nera says. She rubs the mark once with her sleeve, then stops. “I want the time left there until we have the second report.”

Iven lifts the lid on the pot. “Meal’s on time. The repair review moved. Nobody moved it for me.”

Sera lays a small slip beside the new schedule. “One sheet used. The labels are still covered. Ask before you take the rest.”

At the doorway Pell pauses. “I’m not on tonight. I told Jun what I heard yesterday. That’s all.”

Dema looks at the newcomer waiting by the map. “I can show you where the west post is. Someone else can explain the shift.”

The player can respond to one person, check the board, ask what is still uncertain, or leave the room to meet the faction representative. The scene is not a reward parade. It is a glimpse of several arrangements holding—some through cooperation, some through a boundary, and some only for this shift.

## 296. Content and integration checks for the resident arcs

Before any implementation:

1. Verify each character identifier, dialogue owner, quest owner, and current availability condition in the source and data.
2. Confirm that each branch can be represented by existing campaign or quest state; document any required new state and its authoritative owner before coding.
3. Check every arc with refusal, absence, and delayed-action paths so those outcomes do not dead-end unrelated progression.
4. Confirm item counts, meal schedules, and route observations come from their existing owning systems rather than dialogue-local copies.
5. Check each faction variation against the actual major-route gate and ensure no supporting character is silently enrolled in that route.
6. Validate dialogue conditions for unreachable combinations, especially where an arc can be entered late or completed out of order.
7. Make the final outcome visible through a changed schedule, corrected document, accepted service boundary, or character line—not just an unseen flag.

These are narrative implementation requirements, not permission to create parallel systems. If the current owners cannot express the outcome, the plan should identify the gap and wait for the proper integration decision.

## 297. Installment 27 closing note

This installment expands Dema, Iven, Sera, and Pell into optional personal questlines with multiple stages, practical decisions, route-specific pressures, refusal paths, recoverable failures, and distinct epilogues. The player shapes the conditions of collaboration through concrete actions; residents retain the right to set limits, change their minds, and finish an arc without becoming permanent faction assets.

## Installment 28 — The finale: moving the board without losing the shift

The finale turns the plan’s recurring questions into a time-bounded choice. A structural repair requires clearing the wall where the shelter posts its schedules, delivery notes, and working guidance. The board cannot remain in place during the work. The repair is necessary, but its timing is not ideal: several arrangements depend on information that currently lives in one visible place.

This is not a “save the board or let it be destroyed” melodrama. The physical surface can be moved or replaced. The actual risk is deciding what information to copy, who accepts responsibility for each copy, how corrections travel, and what should not be made public again. The repair crew needs space to work. Residents still need to coordinate. Major factions want different forms of confirmation. The player has enough time to make a plan, but not enough labor or materials to preserve every detail in every place.

The finale has four stages: inventory the dependencies, choose a temporary information arrangement, respond to a conflicting update, and review what remains after the wall is repaired. Players can enter with incomplete support-network arcs and still finish the main route. Completed arcs add options and more informed dialogue; they do not provide mandatory puzzle pieces.

## 298. Finale stage one — What is actually on the board?

The player surveys the board with the people who rely on it. Some notes are current. Some are stale. Some name a person or include a reason that was never intended for a wider audience. A few tasks exist only on the board because nobody accepted upkeep elsewhere. The inventory scene separates four categories:

1. **Current operational facts:** a confirmed time, an accepted handoff, a repair check, or a service that is actually in progress.
2. **Useful but uncertain information:** a route observation that may have changed, an unconfirmed delivery window, or a message whose source is unclear.
3. **Personal details:** names, reasons, or witness identities whose sharing scope was limited.
4. **Expired commitments:** tasks whose date passed, services that ended, and offers that were never accepted.

The player can ask the current owner of each item to verify it, mark uncertainty and copy it anyway, omit it pending confirmation, or ask an affected resident what can be shared. A hurried player can begin moving the board before reviewing every note; the repair proceeds, but any omitted dependency becomes a visible later complication. No scavenger hunt is required to reveal a note that the player could not reasonably know existed.

People notice different risks. Dema asks which instruction a newcomer can safely use. Iven asks whether a copied schedule will imply he accepted another meal cover. Sera asks which stock number is current. Pell asks whether a witness name will travel with an observation. A Route Witness asks when the path was last checked. A Signal Hand asks which copy counts as the latest. Their comments identify needs, not a compulsory sequence of dialogue checks.

## 299. Finale stage two — Choose a temporary arrangement

Four broad methods are available. They are not mutually exclusive doctrines; the player can combine them if the necessary people, materials, and time are present.

### A. One verified master copy

The player asks one accepted owner to maintain a single current sheet while the repair is underway. Other residents are directed to that sheet. This is easy to explain and reduces conflicting copies, but it creates a single point of failure and can exclude people who cannot reach the temporary location. The master copy should show its owner, last review time, and what remains uncertain.

### B. Two limited copies

The player makes one operational copy for the people doing the work and one public copy for general coordination. The public version omits private reasons and witness identities. This improves access and scope control but requires a clear correction process; two copies can drift apart. Signal Hands can offer a correction route if they accepted the task, but the player may choose another method.

### C. Direct handoffs with a short index

The group keeps only essential times and contact points on a small index, then delivers sensitive or changing details directly to affected people. This preserves privacy and reduces copying material. It costs time, depends on reachable recipients, and makes absence harder to handle. Dema’s orientation work or Pell’s alternate contact procedure may provide relevant experience without being mandatory.

### D. Pause nonessential coordination

The player identifies the tasks that can safely wait until the repair is complete. Only urgent work receives a temporary record. This avoids overloading the replacement arrangement and keeps labor available for the wall repair. A missed faction window or delayed service may follow. The choice is reasonable if its costs are named and the affected groups are told.

The player may choose a fifth option: postpone the repair while seeking a different work window. The repair owner can refuse if the wall is unsafe to leave. If postponement is accepted, the quest moves into a new scheduling branch with a real cost rather than treating delay as a free escape.

## 300. Commitments before the board moves

Before the repair begins, each supporting network answers a concrete request and can set its own limits.

- **Signal Hands:** carry a correction to specific locations; deliver only urgent updates; or decline because the relay is already occupied.
- **Bench Crew:** move the board, build a temporary stand, or keep its members on the repair task. It cannot guarantee that all copied information is accurate.
- **Route Witnesses:** check whether the temporary board location is reachable; report only the observed route condition; or decline another trip during the repair.
- **Common Table:** hold a handoff until the replacement process is clear; keep a private count with a consenting witness; or complete one existing exchange and accept no new one.

No group is auto-assigned. The player can ask one member, hear the boundary, and decide whether the plan still works. A refusal may require reducing scope, finding another volunteer, or accepting that a service will lapse. That is a branch, not a failed quest state.

The major-faction representative also makes a request. Military asks for one confirmed service window. Rebel asks for correction access at two local points. Independent asks for a receipt and an explicit renewal date. If the player is not committed to a major route, the representative can still appear as a practical counterparty, but the request is optional and remains limited to that exchange. On every route, the player may negotiate the request’s form, refuse it, or provide a narrower answer.

## 301. Finale stage three — The correction arrives late

As the board is moved, a message arrives that the planned delivery time has changed. Its source is credible but not yet independently confirmed. One resident has already left for the former location. The game presents the player’s chosen temporary method under pressure.

If the player chose a master copy, the question is whether the owner can update it before the next shift departs. The player can send a runner, delay departure, or let the first worker travel with the previously confirmed time. If the owner is unavailable, the player may mark the update unverified rather than silently editing the master.

If the player chose two copies, the first copy receives the correction while the second does not. The player can use the agreed relay, ask a witness to carry the update, or temporarily mark both copies “check before departure.” The method’s strength is useful only if its correction path was staffed.

If the player chose direct handoffs, the recipient may not be at the expected contact point. A second attempt, a substitute contact accepted in advance, or a delayed delivery are possible. The player cannot reveal private details to an unapproved person solely to make the handoff easier.

If the player paused nonessential coordination, the changed delivery may be one of the tasks left out of scope. The player can reopen that task at the expense of repair time, ask the counterparty to wait, or accept that the window is missed. The ending remembers whether the player communicated the pause honestly.

The message can be accurate or mistaken, but its arrival alone never proves either. If the player confirms it, the scene establishes how. If the player cannot confirm it in time, the journal preserves the uncertainty and reports what action followed.

## 302. Finale stage four — Review, repair, and return

Once the wall is repaired, the player decides what returns to the permanent board. There is no obligation to recreate its previous clutter. Residents can propose a compact public schedule, a dated route section, a limited service record, or a contact index. Personal explanations and expired promises need not return merely because they were on the old surface.

The review has three possible tempos:

- **Immediate restoration:** return a minimal set of verified operational facts now. This helps the next shift but leaves unresolved records elsewhere.
- **Resident review:** hold a short meeting with the affected contributors and decide what belongs on the board. This costs time but makes scope and upkeep visible.
- **Staged restoration:** post only the next shift’s needs and schedule a later review. This balances urgency with care, but the later review needs an accepted owner or it may not happen.

Any option may produce a good ending if the commitments it makes are honest. The player can restore a sparse board and leave a service paused. They can restore several processes and explicitly name the labor they require. They can keep a copy private or destroy it according to the agreed process. “More information preserved” is not automatically better if the information is stale or exposed beyond its purpose.

## 303. Ending construction as a three-part result

The finale should calculate its authored outcome from three dimensions that remain legible in the story. They are descriptive result categories, not proposed player stats.

**Continuity:** Did the next shift receive the facts needed to act? Outcomes can be dependable, workable with gaps, or disrupted but recoverable. Continuity is about actual handoffs and staffing, not whether the player chose the longest record.

**Custody:** Who accepted responsibility for keeping the temporary or restored information current? Outcomes can be individually owned, shared among willing people, narrowly held, or left without an accepted owner. The story should show the risk attached to the actual arrangement.

**Scope:** What was shared with whom? Outcomes can be broad and public, limited to operational detail, privately relayed, or withheld pending consent. Scope must not be scored as inherently virtuous; an urgent public warning and a private witness statement have different needs.

The active major faction supplies a fourth, independent frame: what service or relationship the campaign route has committed to. The finale then combines that route with the three result dimensions. It does not collapse them into “good ending” and “bad ending.” A Military campaign can end with dependable coverage and narrow disclosure, or a public schedule with fragile upkeep. An Independent campaign can conclude with a successful exchange and no renewal. A no-commitment run can be locally workable yet unable to sustain a distant service.

## 304. Sample ending variants

### Military: “A window with edges”

The shelter confirms one service window and states which intervals remain unstaffed. The representative accepts the narrow schedule because it can be planned around. A temporary board copy stays at the work point, and the next review belongs to a resident who volunteered for one more shift. The ending is steady, but the Military still wants a fuller schedule later. The player can promise to revisit it or let the request stand unanswered.

### Military: “Two copies, one correction late”

The schedule is visible at two locations, but the last correction reached only one. The shelter reports the discrepancy and agrees on a correction path before confirming the next service. The Military’s final response can be impatient without turning the protagonist into a traitor. The route continues with a smaller window while the relay is repaired.

### Rebel: “A correction that travels”

The player leaves two local copies with clear review marks and a named volunteer at each location. The Rebel organizer can update one copy without waiting for a central meeting. The burden is real: both volunteers have accepted the task for a limited period, and the group must revisit whether they want to continue.

### Rebel: “One board, openly incomplete”

The settlement declines to maintain a distributed network it cannot staff. The main board returns with a short list and a visible note that one route report is unconfirmed. The Rebel organizer may prefer wider distribution, but accepts that an acknowledged limit is easier to correct than an unstaffed promise.

### Independent: “Receipt, expiry, and return”

The exchange is recorded, the receipt confirms only what was delivered, and the agreement expires on its stated date. The broker asks whether to renew. The player may decline without undoing the completed exchange. The ending presents the relationship as useful and bounded.

### Independent: “The service paused”

The player cannot provide the requested record without crossing a resident’s agreed limit. The broker declines the next delivery. The shelter keeps its boundary and must find another source or reduce the plan. The ending holds both consequences in view: the refusal was meaningful, and the resource loss is real.

### No commitment: “The short list”

The shelter restores only the next shift’s schedule, a contact point, and one reviewed route note. Local workers keep the rest through direct exchange. The player can see exactly what this arrangement cannot do: serve distant partners reliably or carry every correction. It is a deliberate scale of cooperation, not a secret route that avoids politics and scarcity.

### No commitment: “The board waits”

The repair finishes, but nobody accepts ownership of a new standing board. The player leaves a dated note for the next meeting and completes only the commitments already accepted. The future remains open; the ending acknowledges that a stable process cannot be authored by one person’s good intentions alone.

## 305. Late callbacks based on actions, not moral alignment

The last lines should select from action facts. If the player asked who had accepted a task, Iven may correct the next schedule before it is posted. If the player separated a route observation from a guarantee, Nera may give the next report with a time and witness. If the player checked before sharing a name, Pell may offer one more bounded statement—or simply say he is off watch. If the player corrected a stale copy, a Signal Hand may ask which copy should be checked next.

These callbacks can coexist with disagreement. A resident can appreciate one action and reject another. A faction representative can trust the schedule but dislike the disclosure limit. A task can be completed and still leave a relationship strained. That mixture is more useful than a single reputation tier because it gives later scenes specific material to respond to.

The player is never asked to select “who they are” in the finale. They decide what to do with the board, who to ask, what to leave uncertain, and which commitments to make. The game’s conclusion is the pattern of those acts and their consequences.

## 306. Optional post-finale scene — “The first ordinary shift”

The day after the repair, no faction representative is present. The player walks through the work area and sees what the new arrangement feels like under ordinary use. One resident reads the schedule directly. Another asks a neighbor because the temporary copy is out of date. A volunteer replaces a route mark with a new time. A meal continues without being moved for an unaccepted task. Depending on earlier choices, some or none of these details appear.

The player can correct one remaining note, ask who owns it, or leave it for the accepted owner. The scene can close with a routine success, a small correction, or a practical gap. It must not introduce a fresh crisis just to keep the player busy. Its purpose is to let the player see the system they helped make while ordinary people use it without them.

## 307. Finale reachability and content checks

For every finale variant, the content audit should verify:

1. The variant can be reached through an observable combination of prior actions or a clearly available finale choice.
2. Missing side-quest participation falls back to an ordinary option rather than blocking the campaign.
3. The text distinguishes proposed plans from accepted tasks and accepted tasks from completed tasks.
4. Major-faction route language remains consistent with the current branch authority.
5. Any copied record is within the player’s and contributor’s agreed sharing scope.
6. A missed delivery, absent volunteer, or unconfirmed message has a truthful recovery or closure.
7. Epilogue callbacks do not imply that an unvisited character witnessed events they could not know.
8. Different action patterns produce legible results even when they reach the same broad route ending.
9. No ending treats privacy, cooperation, centralization, or independence as a universal moral score.
10. The final scene follows from the implemented owner’s state and does not depend on a content-only flag with no current consumer.

These checks protect the creative intent when the narrative is later integrated. They also leave room for the actual implementation audit to reject or reshape any proposed branch whose required state owner does not exist.

## 308. Installment 28 closing note

This installment defines a four-stage climax around moving the shelter’s shared board for a necessary repair. It adds choices about information scope, custodianship, verification, labor, and faction service; creates route-specific ending variants; and lets the player reach mixed, imperfect outcomes that follow from actions instead of a single moral score.

## Installment 29 — Worked playthroughs: same route, different practices

Branching design is easy to claim and hard to demonstrate. These worked runs trace actual sequences of actions through the proposed eight-act structure. They are authored examples for narrative review, not implemented walkthroughs or guarantees about current runtime state. Their purpose is to prove that faction commitment and player method remain separate dimensions: two players can enter the same major route and leave with different people, procedures, obligations, and endings.

Each run records four things: the action the player takes, who accepts or refuses the work, what practical cost follows, and what later scene can truthfully remember. A conversation choice alone is insufficient. The downstream callback needs a changed arrangement or a completed action.

## 309. Run A — Military route, bounded coverage

This player chooses the Military route and values predictability, but avoids promising service the shelter cannot staff. They are not defined as “honest” or “good”; they sometimes accept outside help, sometimes withhold details, and once miss a delivery window.

| Act | Action and response | Cost or unresolved pressure | Later callback |
|---|---|---|---|
| I | Ask the liaison to specify the work window before agreeing. | The request takes longer to settle. | The schedule distinguishes requested hours from accepted hours. |
| II | Have the Signal Hands carry a time change marked “unconfirmed.” | One worker waits for a second source. | The liaison knows uncertainty was disclosed. |
| III | Ask the current owner to verify one supply count; keep a resident’s reason off the report. | The public report has less context. | Sera can say the count was verified, not the private reason. |
| IV | Decline a recurring meal cover; ask Iven whether he will accept one named handoff. | The liaison receives a narrower schedule. | Iven’s task remains bounded to the accepted interval. |
| V | Use the Bench Crew’s temporary repair and reserve a check time. | The repair remains provisional. | The final schedule can include a maintenance check without claiming completion. |
| VI | Record two witness accounts separately instead of choosing an explanation. | The report stays unresolved. | Pell’s statement is not treated as proof of intent. |
| VII | Publish the verified window and mark the next one as pending staffing. | The Military asks for a follow-up before extending service. | The faction sees a usable but limited commitment. |
| VIII | Choose one master copy with an accepted owner and one review date. | A single copy remains a point of failure. | Ending: “A window with edges.” |

This route has dependable coverage at a modest scale. Its tension comes from the Military wanting a wider service promise and the shelter declining until labor and stock are confirmed. No moral reputation is needed to explain why the ending is stable but limited.

## 310. Run B — Military route, fast service and costly correction

This second player also chooses the Military, but prioritizes speed and visible coordination. They still protect one private detail and make a correction when the first plan strains. Their route is not villainous; it is an understandable sequence of urgent decisions.

| Act | Action and response | Cost or unresolved pressure | Later callback |
|---|---|---|---|
| I | Accept the broad service window while the staffing list is incomplete. | The promise exceeds confirmed coverage. | The board says “scheduled,” not “staffed,” once corrected. |
| II | Repeat the full delivery message to reach a worker quickly. | A named resident objects to the wider copy. | The player must negotiate a correction and sharing scope. |
| III | Spend the reserved sheet on the schedule before Sera confirms its purpose. | Label work needs another method. | Sera asks whether the next reservation will be checked first. |
| IV | Move Iven’s meal review without obtaining recurring agreement. | Iven refuses the next unplanned cover. | The schedule collision becomes explicit. |
| V | Ask the Military for a replacement part under a one-delivery agreement. | Repair depends on an outside delivery. | The faction can ask to renew; renewal is not automatic. |
| VI | Post an initial account before the second witness arrives. | The report is incomplete and attracts a challenge. | A correction is possible, but the first copy must be marked stale. |
| VII | Reduce the service to one confirmed interval and tell the liaison why. | The broad promise is withdrawn; trust is strained but not erased. | A later request can reference the reduced scope. |
| VIII | Use two copies, then discover the correction reached only one. | The next departure is delayed while the copies are reconciled. | Ending: “Two copies, one correction late.” |

This run demonstrates recovery without a morality cleanse. The player’s late correction matters, but it does not undo the labor conflict, stock use, or lost time. A future conversation can distinguish “you corrected the record” from “the first copy caused a problem.”

## 311. Run C — Rebel route, distributed work with explicit limits

This player chooses the Rebel route and wants decisions to stay close to the people doing the work. They do not assume that local control means unlimited volunteer capacity. At several points they choose a small, traceable process over a fast but broader one.

| Act | Action and response | Cost or unresolved pressure | Later callback |
|---|---|---|---|
| I | Ask two residents what information each needs before accepting a shared schedule. | The first plan is delayed. | The plan records different needs rather than asserting consensus. |
| II | Let Signal Hands carry a correction to both active copies, with a review time. | A carrier spends a second trip. | The old copy is crossed out rather than silently overwritten. |
| III | Ask Dema to test a short guide; another resident volunteers to maintain it. | The guide has a named review date and limited scope. | Dema is not treated as the ongoing owner. |
| IV | Keep Iven’s meal time fixed and move a planning conversation. | The group has less time for the meeting. | Iven can accept a separate one-time handoff later. |
| V | Have the Route Witnesses record time and condition on a local map. | Outsiders need help interpreting the marks. | The Rebel organizer can correct the local copy. |
| VI | Share Common Table’s public total but not recipients’ reasons. | A contributor requests a separate receipt. | The proof is split by audience. |
| VII | Ask volunteers whether they want another rotation before distributing work. | One location declines to host a second copy. | The plan remains distributed where accepted and single-copy elsewhere. |
| VIII | Restore a short public schedule and keep route updates at two staffed points. | Copy upkeep is a continuing task, not a solved problem. | Ending: “A correction that travels.” |

The Rebel route emphasizes proximity and correction speed, but retains cost: people must staff the copies, update them, and accept the work. The player’s limits prevent the story from turning mutual aid into infinite labor. The ending is locally resilient in some places and deliberately thinner in others.

## 312. Run D — Independent route, complete exchange and decline renewal

This player likes negotiated terms but does not treat every relationship as a contract. They accept one service, correct a discrepancy, and later let the agreement expire because the settlement cannot support another cycle.

| Act | Action and response | Cost or unresolved pressure | Later callback |
|---|---|---|---|
| I | Ask the broker to define quantity, date, and return condition. | Negotiation consumes the first meeting. | The agreement has a bounded delivery scope. |
| II | Give the Signal Hands permission to carry the window, not the recipient’s name. | A separate private contact is required. | The broker can verify the window without receiving extra personal data. |
| III | Ask Sera and the inventory owner to confirm the received count together. | The handoff pauses during recount. | The receipt includes a noted discrepancy rather than a false exact match. |
| IV | Accept a one-time meal cover from a volunteer and decline an automatic weekly arrangement. | The next conflict remains open. | Iven’s schedule stays unchanged after the exception. |
| V | Trade for a part with an explicit expiry and return location. | The part must be returned or the next exchange renegotiated. | The Bench Crew can plan around its temporary availability. |
| VI | Preserve Pell’s account as an optional witness note, not a condition of payment. | The broker has less evidence than requested. | The transaction and witness account remain separate. |
| VII | Complete the delivery, then decline a new service until stock is reviewed. | A future window is lost. | The broker can recognize the completed exchange while ending the arrangement. |
| VIII | Restore only the verified handoff and a renewal date that is clearly marked “not agreed.” | The board carries fewer future plans. | Ending: “Receipt, expiry, and return.” |

The player earns no special reward for refusing renewal. The meaningful result is that both sides know the first exchange was completed and the next one was not accepted. A future run could renew by changing only the stock decision, demonstrating replay variation without changing the route.

## 313. Run E — No commitment, practical relationships without a central pact

This player declines all three major route commitments. They still negotiate with representatives when a particular service is useful, but keep each exchange separate. The approach is not pure neutrality: it involves choices about which local task can be sustained and which external request must be refused.

| Act | Action and response | Cost or unresolved pressure | Later callback |
|---|---|---|---|
| I | Ask the Military for one repair window without accepting ongoing coverage terms. | The liaison cannot promise later availability. | One service can occur without a branch commitment. |
| II | Use direct handoffs for one sensitive message. | The player spends time finding recipients. | The shelter avoids making a broad public copy. |
| III | Let the Common Table witness a small exchange; leave other requests private. | The witness has a limited, accepted role. | The record confirms the handoff only. |
| IV | Decline a Rebel meeting when no local volunteer is available. | A group decision is delayed. | The decline is attributed to capacity, not hostility. |
| V | Use a temporary repair and postpone the cart repair. | Travel remains less efficient. | The Bench Crew can revisit it if labor and parts allow. |
| VI | Ask for a second route observation before agreeing to send a runner. | A narrow timing window is missed. | The journey has less risk but the delivery comes later. |
| VII | Tell the Independent broker that renewal is undecided until the stock count is checked. | The broker offers no reserved future slot. | The player owes no unaccepted renewal. |
| VIII | Restore a short shift list, direct contacts, and one dated route mark. | The arrangement has modest reach and depends on nearby cooperation. | Ending: “The short list.” |

No-commitment play can still use any supporting network and can still produce reliable local outcomes. Its limits are distance, coordination effort, and the absence of a guaranteed faction service. The ending should make both autonomy and capacity visible.

## 314. Run F — Mixed conduct and repair after a disclosure mistake

This run is designed to test whether the plan can handle inconsistency. The player first makes a careless disclosure, later respects a work boundary, then makes a separate operational mistake. The game allows specific repair attempts without converting the player into a permanently redeemed or condemned persona.

Early in the story, the player repeats a full message with a resident’s name because it is the quickest way to reach a worker. When the resident objects, the player can acknowledge the scope error, correct the active copies, ask where else the message traveled, and let the resident decide whether their name may remain in a private record. The copy already read by a faction cannot be made unread. The resident may accept the correction and still refuse future named reports.

Later, the player keeps Iven’s meal interval intact and takes a shorter meeting. This does not restore the resident’s confidence about the earlier disclosure; it shows a separate decision in another relationship. In Act V, the player schedules a repair check but forgets to confirm the worker. The task remains incomplete. The character who notices can say so plainly and ask for a new time.

At the finale, the player chooses a narrow public board, a private contact for the affected resident, and a master copy whose owner has explicitly accepted review. The ending is mixed: operationally more reliable than the middle acts, with one relationship still cautious and one repair task still outstanding. The epilogue can show a correction taking effect without erasing the earlier breach.

This trace tests three essential narrative properties: an apology does not delete consequences; distinct actions affect distinct relationships; and a player can learn a process without being forced into a single moral identity. The resulting ending can be hopeful and incomplete at once.

## 315. What the six runs prove

| Dimension | Runs A and B | Run C | Run D | Run E | Run F |
|---|---|---|---|---|---|
| Major route | Same: Military | Rebel | Independent | No commitment | Chosen route can remain unchanged |
| Information practice | Verified narrow copy vs rushed broad copy and delayed correction | Distributed, locally corrected copies | Terms-specific receipts and disclosure scope | Direct handoffs and minimal public information | Mistake, correction, and remaining limit |
| Labor practice | Bounded schedule vs overpromising then reducing scope | Shared only where accepted | One-time cover, no automatic renewal | Small local capacity | A respected boundary alongside a missed check |
| Supporting roles | Signal Hands and Bench Crew | All four networks can contribute | Networks take scoped exchange roles | Optional local assistance | Character-specific repair paths |
| Ending texture | Stable but limited vs strained and recovering | Distributed but maintenance-heavy | Completed exchange with expiry | Local and narrow | Mixed, improved, and unfinished |

The same faction choice does not make these runs converge to the same story. Their endings vary through accepted labor, correction reliability, evidence scope, and renewal decisions. Conversely, a different faction choice does not require different player habits: any route can include careful verification, rushed disclosure, capacity building, or refusal of recurring work.

## 316. Branch pressure and branch repair by act

The following recurrence keeps the branching authored and testable across the campaign:

- **Acts I–II:** a player action creates an initial procedure. Later scenes establish who understood it and where it was copied.
- **Acts III–IV:** the procedure encounters a cost: access, labor, time, or privacy. The player can keep it, narrow it, or change it.
- **Acts V–VI:** a supporting network or resident accepts a bounded role. A failure to confirm acceptance creates a task gap, not a moral penalty.
- **Act VII:** a major faction asks to use or extend the process. The player can negotiate, refuse, provide a narrower form, or defer.
- **Act VIII:** a practical consequence reveals whether the process was staffed and current. The ending records what worked, what did not, and who accepted the next step.

The pattern is not a formula that every quest must repeat. It is a continuity check: if an early choice has no cost, no callback, and no effect on what someone accepts later, it is probably cosmetic. If every choice leads to a fresh crisis, consequences become exhausting. A few visible callbacks can carry a long branch more effectively than a constant stream of alerts.

## 317. Branches with delayed and partial consequences

Some decisions should take effect immediately: a named copy is posted, a task is accepted, a delivery leaves, or a person declines a request. Other consequences should arrive later because the game needs to show an arrangement under use. The plan can use these three timing classes:

- **Immediate:** who heard the update, which item was spent, whether a service window was accepted.
- **Next-shift:** whether the handoff was staffed, whether the copy was corrected, whether a substitute arrived.
- **End-of-act:** whether the faction renews, the resident keeps participating, or the route reaches a new scale.

Delayed consequence must still have a readable cause. If a service fails three scenes later, the journal or dialogue should point to the unconfirmed staffing, stale copy, or declined renewal that contributed. Multiple causes can coexist. Avoid a single hidden threshold that retroactively explains every outcome.

Partial consequence is equally important. An action can help one group and burden another: a public copy lets more workers coordinate while exposing a witness; a narrow agreement protects capacity while costing a future service slot; a two-copy method improves access but adds correction work. The game should preserve those paired effects instead of letting a later reward cancel one side invisibly.

## 318. Run-trace quality review

Before adopting a worked trace as a canonical walkthrough, reviewers should ask:

1. Could the player understand the immediate stakes without reading the plan?
2. Did the character or faction accept the work, or did the player assume acceptance?
3. Is the stated cost visible at the moment of choice or soon after?
4. Does the later callback arise from an event the character could know?
5. Can another playstyle reach a comparable outcome through different actions?
6. Does the trace preserve at least one useful option after a refusal or mistake?
7. Are the resource and schedule facts owned by their actual systems?
8. Does the ending reflect the major route and the local actions separately?
9. Are mixed results allowed to remain mixed?
10. Would a player understand what they can try differently on another run?

These traces should inform content QA after implementation, but they are not a substitute for checking the live graph. A beautifully written run that depends on an unreachable branch remains a design defect. Each route must be grounded in current data and verified conditions before it is treated as buildable content.

## 319. Installment 29 closing note

This installment walks through six distinct campaigns: two different Military runs, a distributed Rebel run, a bounded Independent exchange, a local no-commitment run, and a mixed run that repairs some mistakes while retaining others. Together they demonstrate replay variety driven by concrete actions, accepted work, disclosure choices, and correction behavior rather than a binary moral axis.

## Installment 30 — Field quests for the supporting currents

The four working networks need quests that feel like ordinary pressures in a settlement, not a set of faction initiation trials. These eight field quests can be distributed across the middle and late acts. Each begins with a small practical problem, grows through a decision about method or responsibility, and ends with a change the player can see. They can be completed in different orders; their dependencies should use existing quest progression only after the live data graph is audited.

The working names remain provisional. No quest below establishes a new authority over inventories, schedules, faction allegiance, route safety, or resident consent. Each network offers a service and a point of view; it cannot force the player or another resident to accept that service.

## 320. Signal Hands quest — “A quiet channel”

**Hook:** A resident asks for a schedule correction but does not want their reason repeated on the public relay. One Signal Hand says that leaving out context makes a message easier to misunderstand. Another argues that a relay should carry only the information needed to act.

The player can ask the resident what may be repeated; send only the changed time; include the reason after obtaining permission; route the full explanation privately to the people affected; or decline to send a message until the origin is clearer. The narrow message is not automatically safe: if two shifts have similar names, omitting the location can create a new ambiguity. The full message is not automatically more accurate: it can turn a private reason into a rumor.

**Middle turn:** A recipient replies that they received the time but do not know which work point it refers to. The player can send a location-only correction, ask the carrier to return for clarification, or make the recipient call back. Each option costs a trip, a delay, or additional contact labor. The Signal Hands can make a recommendation, but the player chooses which cost to accept.

**Closures:** the network adopts a two-part message format for this exchange; it uses the public time plus a direct location check; it pauses the relay until a source is confirmed; or it decides that the channel is unsuitable for this kind of update. The closure is evaluated by whether the intended people received usable information, not by whether the player chose privacy or openness as an abstract virtue.

**Major-route pressure:** Military requests a confirmed time; Rebel asks for the update to reach both local copies; Independent wants a message that can be attached to a bounded delivery. In each case the player can supply the minimum needed to satisfy the request, negotiate another proof, or decline the service.

## 321. Signal Hands quest — “One warning, two arrivals”

**Hook:** A warning about a blocked passage reaches two locations at different times. The first copy is based on a witness report; the second is a relay repeating the first. A traveler asks whether the warning is still current. There is no independent confirmation yet.

The player can keep both reports and their times visible; label the second as a repeat rather than a new confirmation; ask the Route Witnesses for a fresh observation; or temporarily direct travelers to a different passage. Redirecting traffic reduces exposure to uncertainty but may burden the alternative route. A player who wants the warning removed can do so only after learning what evidence supports the change.

**Branch detail:** If the player asks for a fresh check, the Route Witnesses can accept one inspection and report an observation window. If they are unavailable, the message remains uncertain. If the player waits for a second source before warning anyone, the quest records the delay and which travelers were still at the shelter. It must not invent an offscreen injury solely to punish caution.

**Resolution:** keep the warning with a time limit; replace it with a confirmed update; annotate that the second arrival was not corroboration; or retire the route note after its validity window. Signal Hands gain a better relay practice only if a participant accepts the maintenance task. The “fix” does not happen automatically because the player selected a dialogue line.

## 322. Bench Crew quest — “The wrench that comes back”

**Hook:** A shared wrench is overdue at the repair bench. A delivery worker remembers lending it to someone on the south shift. The Bench Crew needs it for a repair, but the south team says the tool is still in use. Nobody knows whether this is forgetfulness, a changed task, or a dispute about who can borrow it.

The player can ask the current holder to return it after one defined use; arrange a substitute; bring the repair task to the tool; ask both crews to agree on a return time; or let the Bench Crew postpone its repair. The player should not choose an accusation option that turns an untracked tool into evidence of theft. If the holder refuses to return it, the game presents the refusal and its operational result without assigning an unsupported motive.

**Middle turn:** The tool returns with a damaged handle. The player can document the condition, ask who used it last, let the Bench Crew inspect it before drawing a conclusion, or repair the handle from available materials. If the material is consumed, the current inventory owner records it. A witness can report what they observed but cannot establish who caused damage they did not see.

**Closures:** agree on a one-use return note; keep the tool assigned to the current repair until it is finished; replace the shared tool with a temporary substitute; or stop lending it until the handle is repaired. The crew may still cooperate with the south team even if it changes its lending practice. A faction representative may ask for the wrench to complete their own task; that request must compete with the existing repair and the holder’s agreement.

## 323. Bench Crew quest — “A repair taught twice”

**Hook:** Two workers have learned different ways to stabilize a loose shelf bracket. Both have worked once. One method is faster; the other is easier for a second person to inspect. The shelf carries stored supplies, so leaving it unsupported is not an acceptable long-term option.

The player can ask for a side-by-side demonstration, use the faster method and schedule a check, use the more inspectable method and accept a delay, add a temporary brace while the crew compares them, or ask a qualified repair owner to decide. The player is not asked to adjudicate technical truth without evidence. The Bench Crew can decline to use a method it considers unsafe.

**Middle turn:** The next shift has a different worker. If the repair was documented in plain language and another person accepted a check, it can be inspected. If not, the shelf remains closed until someone qualified is available. A player who trained a second worker gets an extra option, but training does not guarantee availability or competence beyond what was actually taught.

**Closures:** the crew adopts one method for this bracket only; leaves the temporary brace until an owner reviews it; trains one volunteer on a bounded inspection; or closes the shelf and relocates supplies. The consequence concerns access and repair confidence. It should not become a generic settlement-wide engineering rating.

## 324. Route Witnesses quest — “The mark under the frost”

**Hook:** A pale chalk mark is visible beneath a thin layer of frost. One resident says it means the lower crossing was clear. Another says the mark was used to show where water collected. Neither remembers when it was drawn. The player must decide what to do with an ambiguous sign before a planned trip.

The player can leave it covered and treat the crossing as unknown; uncover the mark and ask who can interpret it; check the crossing directly; or use a route with a more recent observation. Uncovering the mark may reveal the old note’s date, but not who wrote it. A direct check may cost time and require an accepted witness. The alternative route may be longer or already crowded.

**Middle turn:** The Route Witnesses find a second mark at the far end, but its direction is unclear. The player can preserve both marks in the record, replace them with a current observation, or remove them and use a simple “checked/unchecked” notice. Each method changes how much history is available and how quickly a traveler can understand the immediate condition.

**Closures:** leave the crossing marked unknown; post a dated current observation; keep the original mark with a “meaning unconfirmed” note; or retire the informal marks after an agreed trial. The quest resolves when travelers know what the group can actually claim, not when the player discovers the ancient author of the chalk.

## 325. Route Witnesses quest — “The route for someone who will not travel”

**Hook:** A faction requests a route check, but the person they propose sending does not want to make the crossing. A Route Witness offers to go, though their last trip was already difficult. The player can accept the offer, ask for another method, delay the report, or refuse the request.

The player can use an observation from a safe nearby vantage; ask a second witness whether their existing observation is still current; send a different volunteer who explicitly accepts; negotiate a remote description with a local contact; or mark the route unknown. No one is pressured into a dangerous trip because a faction needs a definite answer. If the player accepts a route witness’s offer, they still choose what range and time they will inspect.

**Major-route variations:** Military asks for a clear logistical answer; Rebel asks whether local travelers can update the report themselves; Independent offers transport if the route is described as open. The player can answer with a limited observation, negotiate a provisional route, or decline to certify safety. The supporting group does not inherit responsibility for the faction’s downstream decisions.

**Closures:** a dated partial observation, a remote contact report, a volunteer’s bounded check, or an explicit unknown. The ending can still proceed if the route remains unknown; the faction must plan around that limit or choose another path.

## 326. Common Table quest — “The unclaimed parcel”

**Hook:** A wrapped parcel is left at the exchange point with a note saying “for the next shift.” No recipient is named. One person recognizes the wrapping but is not certain. Another warns that opening it would violate the sender’s request. The parcel may contain a useful supply, a personal item, or something unsafe; the player has no evidence yet.

The player can keep the parcel sealed in a designated holding place; ask the Signal Hands to carry a message asking for confirmation; ask the Common Table to witness its storage; or return it to the last known sender if that person confirms ownership. Opening it without permission is possible only as an explicit, consequential action when there is an immediate safety reason. The game should not reveal a valuable reward for opening it as a routine incentive.

**Middle turn:** A second person claims to have sent it but cannot describe the wrapping. The player can ask for a distinguishing detail, wait for the first contact, or ask a witness to observe the exchange. If nobody confirms it before the holding interval ends, the Common Table can propose extending storage or returning it to the known location. It cannot decide that unclaimed goods become communal stock.

**Closures:** confirmed private handoff; extended hold with an accepted review time; return to the sender; or unresolved storage pending another contact. If the item is eventually opened under an agreed safety procedure, its contents are recorded through the appropriate owner and not as a surprise bonus. The arc’s reward is a clearer boundary for future exchanges.

## 327. Common Table quest — “A receipt in another hand”

**Hook:** A resident asks someone else to receive a small exchange because they will be away. The intermediary accepts the handoff but does not want to hold the recipient’s personal details. The counterparty asks for a signature proving the recipient received the item.

The player can record that the intermediary accepted custody; request a second witness; ask the recipient to confirm later; limit the receipt to item and time; or refuse the exchange until both parties agree on proof. A custody receipt is not the same as a final receipt. The dialogue should make that distinction clear before the player signs or posts anything.

**Middle turn:** The recipient returns and reports that the item was short by one piece. The intermediary says the parcel was sealed when handed over. The player can record both statements, recount remaining stock with consent, check any existing inventory authority, or ask the parties to settle the missing piece privately. The system must not invent a thief or silently charge the intermediary.

**Closures:** issue a receipt for custody only; obtain the recipient’s later confirmation; use a second witness for the next exchange; or cancel future intermediary handoffs. The Common Table can continue other work. The player may keep a relationship with the intermediary while declining to use them for this role again.

## 328. Cross-quest callbacks among the field packets

These quests can alter later scenes through specific evidence:

- If “A quiet channel” established a narrow message format, “One warning, two arrivals” can use it without requiring the player to repeat its tutorial.
- If “The wrench that comes back” created a one-use return agreement, “A repair taught twice” can ask whether that same tool is available, not assume it is.
- If “The mark under the frost” left the route unknown, the route-check quest can start with that uncertainty already visible.
- If “The unclaimed parcel” established a holding interval, the receipt quest can reuse the agreed review time only if it remains current and accepted.
- If a field quest ended unresolved, a character can acknowledge that status without treating it as a failed campaign.

Crossovers should be small and truthful. A previous method may inform the next decision, but it should not solve the new problem automatically. Reusing a process still requires checking whether the people, time, and scope fit this case.

## 329. Optional field-quest pairings

Players can combine these quests to create a self-selected local style:

- **Fast relay + delayed confirmation:** send an actionable warning immediately, then mark it provisional until checked. This favors timely response while accepting a correction burden.
- **Tool custody + one-use lesson:** train a second worker on the repair and agree on a return time for the shared tool. This builds local capacity, but only if both workers accept the duties.
- **Unknown route + bounded service:** decline to certify the crossing and negotiate a smaller delivery at a known point. This sacrifices reach for a clearer risk boundary.
- **Sealed parcel + custody receipt:** protect the contents and document who accepted responsibility for storage. This preserves provenance without proving who owns it.
- **Public route note + private exchange:** make a hazard visible while keeping recipient details restricted. The player can distinguish operational warning from personal information.
- **Retired tool loan + direct faction request:** reject an outside request for the shared wrench while offering a later window after the current repair. The faction can accept or decline that narrower proposal.

These pairings should emerge from actions, not a menu asking the player to choose a named style. The language can vary with how the player explains the choice, but downstream outcomes depend on the concrete agreement.

## 330. Field-quest implementation and authoring limits

Before adding any field quest to production, confirm that the current quest system can represent its entry and completion facts, that the relevant faction/character IDs exist, and that all item, schedule, and route details come from their true owners. If the implementation cannot represent a temporary receipt or an accepted witness scope, flag that as an integration requirement rather than storing it in dialogue-local state.

Each field quest needs a reachable closure for acceptance, refusal, absence, and unresolved evidence. A quest can end without a tidy answer. It cannot strand the main campaign because a resident declined a statement or a route remained unknown. Where a callback depends on a completed action, define which system or quest owner records completion and verify that the ending consumer reads it.

No content should use the working network names to bypass the active faction branch rules. The four groups provide tasks and perspectives. They do not become new major campaigns, create independent territorial control, or override the current moral/faction gate. Any change to those contracts requires evidence and an explicit integration decision.

## 331. Installment 30 closing note

This installment adds eight field quests—two each for Signal Hands, Bench Crew, Route Witnesses, and Common Table—with action branches, middle reversals, failure-aware closures, faction pressures, and cross-quest callbacks. Their outcomes deepen the campaign’s practical texture while keeping each support network local, voluntary, and subordinate to the player’s main route.

## Installment 31 — The major factions meet the supporting currents

The supporting networks become most interesting when their work crosses a major faction’s request. A Military form, a Rebel distribution method, or an Independent service agreement can make a local task easier while introducing a new pressure. The player’s route determines who has standing to make the central campaign request; it does not determine whether a local resident accepts the method.

This installment defines sixteen encounter frames: each of the four working networks meeting each major faction or operating without a major commitment. The frames are not sixteen mandatory quests. They are modular scenes that can be placed where the current act and available cast make sense. Each contains a request, a point of friction, and a negotiation or refusal that changes what happens next.

## 332. The relationship rule: cooperation is scoped

A support network can cooperate with a major faction for one task and refuse its next request. A resident can accept a delivery, refuse a roster, then help the player with an unrelated repair. A faction can value a network’s work without gaining control of its membership. The player can broker a narrower agreement, but cannot promise on behalf of people who have not agreed.

Dialogue should distinguish four states:

- **Offered:** someone has proposed a service, but nobody has accepted it.
- **Accepted:** a person or group has agreed to a defined task and scope.
- **Completed:** the task happened and its owning system or quest authority can confirm it.
- **Renewed:** the parties accepted another term after the first one ended.

This language prevents a common narrative collapse in which one successful scene silently becomes indefinite service. It also creates useful branch points. The player may want the task to continue; the group may offer only one more shift; the faction may decline the limited terms; or the arrangement may simply expire without hostility.

## 333. Signal Hands across the political routes

### Military encounter — “Confirm the window”

The liaison asks for a relay confirmation before moving personnel. One Signal Hand has heard the update but has not checked its source. The player can report “received, unconfirmed,” send a second carrier to verify, delay the personnel decision, or give the liaison the earlier confirmed window. The Military can plan around a qualified answer, but may not treat receipt as confirmation. If the player sends a second carrier, the carrier accepts one trip and no standing relay duty.

### Rebel encounter — “Who may correct the copy?”

The Rebel organizer wants the two local copies to be editable by anyone working the shift. Signal Hands support fast corrections but worry that no one will know which version is current. The player can allow open edits with a visible timestamp, name a rotating correction contact, keep one master and one read-only copy, or leave the current distributed setup unchanged. Open editing increases speed and conflict risk; a correction contact adds work; a master copy slows distant updates.

### Independent encounter — “Receipt of delivery”

The broker wants proof the message reached the requested post. A Signal Hand will confirm the delivery time but will not certify that the recipient acted on it. The player can accept a receipt limited to delivery, ask the recipient for a separate confirmation, negotiate a witness, or decline to provide a receipt. The Independent party may reject the narrow evidence, in which case the service does not renew. The relay still happened; the disagreement is about what it proves.

### No-commitment encounter — “One more relay?”

Without a major agreement, the Signal Hands receive a local request to repeat a route update. The player can ask for one message, recruit a recipient to collect it directly, or reserve the network’s time for an already accepted task. The group may accept the one relay and still decline the next. The local ending can feature a reliable short-distance channel without suggesting that it serves the whole region.

## 334. Bench Crew across the political routes

### Military encounter — “Standard part, local repair”

The Military offers a compatible part if the repair follows its standard fit. The Bench Crew has used a field repair that is serviceable but not identical. The player can accept the standard, keep the local method and ask for the part separately, test both before choosing, or refuse the offer. Accepting the standard may improve compatibility with Military equipment while requiring an inspection the crew has not yet accepted. Refusal preserves the local method but leaves the part shortage unresolved.

### Rebel encounter — “Teach the whole block?”

The organizer asks Bench Crew to teach the repair method to several local teams. The crew is willing to teach one bounded session but cannot promise every team will attend or maintain competence. The player can schedule the session, ask for volunteer learners first, teach only the inspection step, or keep the repair local. A wider lesson can distribute capacity while using time and parts; a narrow lesson may help fewer people but make the follow-up clearer.

### Independent encounter — “A part on loan”

The broker offers a bearing on the condition it is returned after one service cycle. Bench Crew says the repair could make the bearing difficult to remove. The player can ask for a returnable alternative, negotiate a purchase, inspect the contract with the repair owner, or decline the loan. If the bearing is used, the game records the actual condition and agreed return, not an automatic debt. The broker may withdraw the offer if the terms change.

### No-commitment encounter — “Repair what can be maintained”

Bench Crew asks whether to fix one item with available materials or wait for a compatible part. The player can accept a temporary repair with a check date, divert labor to a smaller task, or leave the item unavailable. A local solution can be genuinely useful while remaining less durable. The ending should show its maintenance burden rather than mark it as a failed version of the Military standard.

## 335. Route Witnesses across the political routes

### Military encounter — “Passable for whom?”

The Military asks whether a route can carry a supply cart. The latest witness crossed on foot. The player can report only the foot crossing, ask for a cart-specific inspection, suggest an alternate route, or decline to certify it. A foot observation does not prove vehicle access. If the Military moves a cart based on a narrower observation, its choice remains its responsibility; the shelter’s report must retain its stated scope.

### Rebel encounter — “Local marks, shared meaning”

The organizer wants to put neighborhood symbols on the route map. Some travelers understand them; incoming volunteers do not. The player can keep local marks and add a key, create a shared legend through a short trial, use plain-language warnings, or retain two map versions for different audiences. Each method has a cost in map space, interpretation time, or upkeep. Route Witnesses can recommend a format but cannot force local travelers to adopt it.

### Independent encounter — “Route condition clause”

The broker’s delivery agreement says the crossing must be “clear” at departure. Witnesses object that their observation is time-bound. The player can negotiate “last observed clear at [time],” agree to a fresh check, choose another delivery point, or refuse the clause. If the broker needs a guarantee the shelter cannot make, the delivery may not proceed. A precise observation remains valuable even if it is not a guarantee.

### No-commitment encounter — “The path stays unknown”

No faction is willing to send a check, and the Route Witnesses are occupied. The player can delay travel, choose the known longer route, ask for a volunteer without pressure, or proceed with the uncertainty stated. If nobody accepts a check, the map stays unconfirmed. The player does not unlock a hidden scout by selecting a persuasive line.

## 336. Common Table across the political routes

### Military encounter — “Count without a name”

The Military needs to reconcile a delivered quantity; the Common Table can provide a witnessed count but not the recipient’s reason for receiving it. The player can submit the count, request a receipt that confirms the handoff only, obtain consent for further detail, or decline to identify the recipient. If the liaison needs identity for a separate safety or custody reason, that need must be stated plainly, and the player can negotiate an alternative.

### Rebel encounter — “Public contributions, private recipients”

The organizer proposes posting both contributions and distributions so participants can see how aid moved. The Common Table worries that naming recipients will expose private need. The player can publish aggregate totals, let recipients choose a public acknowledgment, keep individual records with a consenting witness, or reject the list. Public visibility can encourage contributions; it can also stigmatize people. The group—not the player alone—decides whether it will maintain the chosen record.

### Independent encounter — “Proof and renewal”

The broker asks for a signed receipt before offering another exchange. A recipient agrees to confirm receipt but does not want their name shared beyond the transaction. The player can provide a scoped confirmation, negotiate a sealed receipt, request a second witness, or accept that the offer may lapse. Renewal is a separate decision. A receipt from one exchange does not entitle the broker to future recipient information.

### No-commitment encounter — “The table closes at dusk”

The Common Table has enough capacity for one more handoff before its members stop for the day. The player can prioritize an already accepted exchange, ask which resident is willing to take another, delay the new request, or close the table. The group’s limit may disappoint someone. The scene makes the limit visible without portraying the volunteers as selfish or the requester as entitled.

## 337. When the player’s route and the network’s preference diverge

A network member may prefer a method associated with a faction the player did not choose. For example, Signal Hands may like the Military’s clear receipt format while opposing its broad roster request. Bench Crew may want an Independent tool loan while rejecting its return clause. Route Witnesses may prefer the Rebels’ locally editable map but disagree about publicizing witness names. Common Table may accept the Military’s count procedure while withholding individual recipient details.

This divergence should create nuanced scenes rather than betrayal penalties. The player can support the member’s limited choice, explain why their route has another constraint, negotiate a parallel method, or state that they cannot authorize the arrangement. A relationship can become more candid after disagreement. The player’s faction commitment remains intact, and the network is not forced to mirror it.

If a supporting group chooses to pause work with a faction, the scope should be exact: perhaps it will no longer carry that faction’s named roster, but still deliver a general schedule; perhaps it will stop lending tools while continuing to teach; perhaps it will decline receipts but still witness local exchanges. A total break should occur only after authored events support it, not as a generic result of one refused request.

## 338. Distinct dialogue reactions to negotiation

Each network should express the cost it understands best:

- **Signal Hands:** “I can carry that wording. I can’t make it a second source.”
- **Bench Crew:** “We can leave it serviceable. We can’t call it checked until somebody checks it.”
- **Route Witnesses:** “I crossed there at first light. I can tell you what it looked like then.”
- **Common Table:** “I can confirm the parcel moved. Ask them before you write why.”

Faction representatives should also have distinct responses. A Military liaison may accept the narrower service but ask for a follow-up time. A Rebel organizer may accept local variation but request a way to correct both copies. An Independent broker may decline an exchange when the evidence does not meet the agreed term, yet leave the door open for a future contract. These are reactions to terms and capacity, not emotional labels such as “angry,” “disloyal,” or “impressed.”

## 339. Sample confrontation — “The copy nobody asked for”

The liaison lays a clean form beside the old board. It has a box for every shift and a blank beside each name.

“This would tell us who can cover the window,” they say. “We can move the delivery if we know.”

Lio from the Signal Hands turns the form around. “It tells you who was written down. It doesn’t tell you who agreed.”

The player can ask the liaison to accept confirmed intervals only; ask residents whether they want their names on the form; provide a total number of available workers without identities; or decline the form and negotiate service after the work is staffed.

If the player asks for consent, one resident agrees to be listed for a single interval and another declines. The form can proceed with the accepted name and a blank for the rest. If the player offers an aggregate count, the liaison may accept a narrower schedule or say the delivery cannot be moved on that information. If the player declines outright, the form is removed but the request for coverage remains unresolved.

The scene can end with the liaison saying, “I can plan around a blank. I can’t plan around a name that won’t come.” Lio answers, “Then ask the person.” Neither character wins the scene. The next action determines whether the delivery changes and whether a real worker accepts the shift.

## 340. Ending texture from inter-faction cooperation

At the ending, a supporting current may have one of several relationships with the active major faction:

- **Useful and renewed:** one exchange succeeded, the terms were reviewed, and both sides accepted another bounded task.
- **Useful and expired:** the task was completed, but the relationship ended on its stated date.
- **Narrow but continuing:** the network continues a limited service while refusing a broader request.
- **Paused after a breach:** work stopped after information or labor limits were ignored; a concrete correction may reopen one task, but not restore blanket access.
- **Not tested:** the player never involved the network in that route’s service, and the ending makes no claim about their relationship.

These variations can appear beneath each major route without inventing another campaign ending. A single line, prop, or optional epilogue exchange can show the distinction. The supporting networks remain supporting: their status changes the quality of cooperation, not the strategic result of the major faction storyline.

## 341. Content review questions for faction intersections

For every network/faction encounter, review:

1. What exactly is being requested, and who is authorized to request it?
2. What can the network truthfully provide from its direct knowledge?
3. Who accepted the labor, information scope, or service term?
4. What does each party receive, spend, expose, or leave uncertain?
5. Can the player negotiate a narrower form without losing the main quest?
6. Can the player refuse without the scene mislabeling the refusal as betrayal?
7. Does a limited agreement expire, and does renewal require a new acceptance?
8. What later scene can observe the agreement’s real outcome?
9. Does the faction route still own its central campaign decision?
10. Are the supporting cast’s motives and limits visible in dialogue and action?

If a scene cannot answer these questions, it risks using a support network as decoration or as a concealed authority. Revise the request until the relationship is mutual, bounded, and legible.

## 342. Installment 31 closing note

This installment supplies sixteen route-intersection frames, distinguishes offered, accepted, completed, and renewed work, and shows how supporting networks can cooperate with or resist a major faction on a task-by-task basis. The player can negotiate scope and consequences; no network automatically adopts the player’s allegiance or becomes a rival campaign power.

## Installment 32 — Texture that remembers how the shelter works

Flavor is most effective when it grows from use. A room should not become more hopeful because the player chose a benevolent dialogue option; it can feel more settled because someone accepted responsibility for a repair, the route note has a fresh time on it, or residents know where to leave a correction. A shelter can also remain tense after a materially successful decision. The prose should notice those concrete changes without narrating a moral verdict.

This installment establishes recurring spaces, objects, and voice patterns for the campaign’s support networks. These details are optional authored presentation. They do not imply a new simulation, inventory owner, time system, or faction reputation variable. The actual environment and resource owners remain authoritative if a scene displays changing conditions.

## 343. Four recurring spaces

### The board wall

The board wall is a work surface first and a symbol second. In early acts, pins overlap, the lower edge curls away from the plaster, and old chalk smears remain beneath fresh marks. When someone corrects a copy, the crossed-out line stays visible until a resident removes it. The scene can show whether the shelter has a current owner for the board: a hand replaces the time; no hand arrives; or two people check different corners before anyone leaves.

The wall’s state should reflect the plan’s operational choices. A master copy is less crowded but may have one pencil tied to it. Two copies show different handwriting and a correction mark. A short list leaves open wall space that can feel like relief or like missing information, depending on what is still needed. A private note may be folded and held at the pass-through instead of shown publicly. No text should announce that the board has become “good” or “bad.”

### The kitchen pass-through

The pass-through is a narrow opening between the preparation area and the common room. A schedule moved away from meal preparation can be seen in the cook’s steadier pace and the reduced number of people waiting in the doorway. If Iven accepted one temporary handoff, a second bowl can be waiting with a name and return time; it is not evidence of a permanent change. When no cover was accepted, steam fogs the window and the meeting waits outside until the task is safe to pause.

Small sensory details belong to the task: a spoon set down before someone answers; a lid held with a cloth; a bowl counted twice because the first count was interrupted. Keep the kitchen human and ordinary. It should not become a spectacle of deprivation every time a scheduling branch fires.

### The repair bench

The bench carries shallow grooves from repeated work. A tool left in the wrong place is not automatically theft; it may be in use at another station. If an item is on loan, a small tag can state where it went and when it is expected back, if that process was accepted. A temporary repair has a strip of cloth around the joint until its check. A completed repair loses the marker only after the responsible person confirms it.

The bench’s environmental text can reflect technique without judging it. “The clamp remains in place, paint chipped on one side” says what is there. “The bench looks trustworthy again” substitutes an authorial verdict. Characters can express confidence or worry in their own voices; the environment should report visible facts.

### The route table

The route table is a map laid beneath a cracked pane or weighted at its corners. Marks are dated in the margin. One edge is smudged by repeated fingers; a route observation may be old even when its line is dark. If two witness reports differ, both can remain legible with separate times. A player who chooses one current mark can still preserve the earlier line in a folded note if someone accepts that recordkeeping.

This space allows uncertainty to be visible without a hazard icon that declares the route safe. The map can show “last checked,” “reported by,” and a blank for what remains unknown, using language the characters can read. If the implementation does not support a particular label, the content plan should not claim it is interactive.

## 344. Four recurring objects

### The pin with thread

A short thread tied to a pin links a message to its copy location. When a correction is accepted, the thread can move to the current copy. When no one accepts upkeep, it hangs loose. The object provides an unobtrusive visual callback to correction responsibility. It has no magical tracking function; a resident still needs to carry an update.

### The grease pencil

The grease pencil writes on the temporary board and can be wiped away. It is shared because paper is scarce, not because the object is a quest token. If it goes missing, a character can mention that it was last used at the repair bench, but the game should not launch a fetch quest unless the item’s owner and gameplay role support one.

### The folded slip

A folded slip carries information intended for a smaller audience. Its crease can hide the text from people passing the table. Opening it is not an automatic consent violation if the player is an intended recipient, but copying it publicly can exceed its scope. A scene can make the distinction clear through who handed it to the player and what was agreed.

### The repair tag

A tag attached to a tool or repaired item can say “in use,” “check after,” or “return by.” It should only show a date if one was agreed and the current schedule can support it. If a tag has gone stale, a character can remove it and ask the owner rather than treating it as a binding instruction.

## 345. Voice texture: Signal Hands

Signal Hands talk in units of receipt, source, distance, and correction. They tend to use short clauses because messages may be repeated aloud. Their language is practical rather than coded; outsiders can understand it.

- At a relay point: “I have the time. I don’t have the source yet.”
- When a carrier is tired: “I can make one more run. Pick which copy matters.”
- When a correction arrives: “That changes the hour. It doesn’t change yesterday’s note.”
- When privacy is disputed: “Say what they need to do. Don’t borrow their reason.”
- When a relay fails: “The south point got it. The east point didn’t.”
- When a faction asks for certainty: “I can tell you it arrived. Ask them whether they acted.”

The same character may speak less tersely when off duty. A relay role does not flatten the person into a radio operator. One may collect bottle caps, hum while waiting, or tell a long story that is not an encoded message. Such texture helps the network feel made of individuals.

## 346. Voice texture: Bench Crew

Bench Crew members talk in terms of load, fit, access, and the next check. They disagree about workmanship without automatically insulting one another. Their pride comes from making something usable and teachable, not from declaring every repair permanent.

- Before a repair: “Hold it there. I want to see where it shifts.”
- After a temporary fix: “It’ll carry this load. Don’t call that a lifetime.”
- When asked to teach: “I can show the clamp. Somebody else has to stay for the check.”
- When a part is borrowed: “Write where it went. I don’t need a name if the tool comes back.”
- When a repair slips: “That joint moved. We stop using the shelf until we know why.”
- When a faction wants speed: “You can have it sooner if you tell me what weight it needs to hold.”

Their dialogue can be dry, patient, or sharp depending on the situation. Avoid making every craftsperson a gruff archetype. The person inspecting a repair may be quiet and exact; the apprentice may joke because the quiet has become uncomfortable.

## 347. Voice texture: Route Witnesses

Route Witnesses anchor statements to time, vantage, and what was actually observed. They do not need a specialized jargon dialect. Their lines often qualify a report and correct assumptions gently.

- “I crossed once, before the wind changed.”
- “From the upper step I could see the waterline. I couldn’t see the lower turn.”
- “That mark is old. I don’t know how old.”
- “The cart didn’t cross. The footpath did.”
- “I’ll check the near side. I’m not going farther today.”
- “You can use what I saw. You can’t make it a promise from me.”

The route scene can show small physical habits: one witness checks boot laces before leaving; another records the direction of light; a third refuses to write while their hands are cold. These details should emerge in scenes where they are present and relevant, not repeat as a character’s single trait.

## 348. Voice texture: Common Table

Common Table members talk about who carries, waits, receives, and confirms. Their vocabulary makes invisible labor visible without claiming authority over recipients.

- “They asked for a parcel. They didn’t ask to be written on the wall.”
- “I can keep it here until tomorrow. After that, ask me again.”
- “That’s the count I saw. I didn’t see who opened the wrap.”
- “If you want a witness, ask before the handoff.”
- “A favor can end when the person says it ends.”
- “We moved the item. We didn’t settle the reason.”

Members can be generous and still protect their time. A person may offer an exchange because they have surplus today, then decline tomorrow. The narrative should not imply that previous generosity creates a continuing obligation.

## 349. Ambient exchanges that react to action

Short optional exchanges can reveal state without forcing exposition. They should fire only when the underlying event occurred and the speaker was present or was later told.

- After a corrected relay: “Which one is current?” “The one with the later time. I crossed out the first.”
- After a resident declines a name on the board: “Did the count go through?” “Yes.” “And the name?” “No.”
- After an accepted one-time meal cover: “You’re free after this bowl.” “Good. I have my own work waiting.”
- After a tool returns damaged: “Is it still usable?” “Not until we check the handle.”
- After a route report stays unknown: “Can we go?” “We can. We can’t call it checked.”
- After a service expires: “Will they come back?” “They offered another date. We haven’t accepted it.”
- After an unresolved delivery: “Did it arrive?” “Not while I was there.”
- After a correction mistake: “You crossed out the name.” “I crossed out the copy. They still heard it.”

These exchanges do not award hidden approval. They reinforce what happened and what remains incomplete. If no one has discussed an event with the speaker, the line must not appear as omniscient ambient dialogue.

## 350. Quiet environmental variants

Use three levels of environmental change so every decision does not require a new scene:

1. **Prop-level:** a tag, folded slip, marker, or changed note appears.
2. **Line-level:** an existing character changes one line to reference the accepted task or unresolved gap.
3. **Scene-level:** a new short exchange occurs when the decision changes labor, access, or trust enough to warrant attention.

The lowest level is usually enough. A single updated time can be a better callback than another formal meeting. Scene-level additions should be reserved for substantial consequences such as a disclosure breach, a service expiring, a repaired object being returned to use, or a network pausing work.

Environmental flavor also needs a neutral state. If the player never joins the Bench Crew’s repair scene, the bench should not mysteriously gain their tag. If Pell never accepted watch, there is no Pell witness note. If a player declines the Common Table handoff, the parcel remains where the actual story placed it or is removed by its established owner; it does not move to a convenient display location.

## 351. Sensory writing boundaries

Weather, light, sound, and material detail should support the action rather than bury it. A scene about whether a witness consents to a written report does not need a long paragraph of atmospheric rain. One detail—the paper softening at its edge, a thumb holding it away from the stove—can create place and immediate handling risk.

Vary repeated sensory cues. Do not make every meeting cold, every repair loud, every radio line static, or every meal thin. The setting has texture beyond misery. A repaired latch can close cleanly. Someone can save a sweet peel for later. A resident can laugh at the wrong moment and then continue the serious conversation. Such relief does not trivialize scarcity; it makes the people more than its symptoms.

Use physical details to clarify space and access: who can reach the board, whether the route table can be read from a seated position, whether a person can hear the call from the kitchen, and how a temporary location changes travel inside the shelter. If a character cannot access a surface, offer a spoken or carried alternative when the story’s resources allow it.

## 352. Flavor review: evidence and restraint

Before finalizing a flavor line or prop variant, ask:

1. Is the referenced event known to this speaker?
2. Does the physical object exist in this scene and belong to the right owner?
3. Does the text describe a condition or infer a motive?
4. Does the sensory detail reveal work, access, or relationship without repeating stock imagery?
5. Is there a neutral line for players who did not complete the related quest?
6. Does a character retain interests and habits beyond their faction function?
7. Does the line distinguish a one-time task from recurring responsibility?
8. Does the player receive enough context to understand a choice without a lore dump?
9. Does the scene allow quiet, imperfect, or unresolved outcomes to feel complete?
10. Does the flavor remain consistent with the game’s grounded, restrained tone?

## 353. Installment 32 closing note

This installment adds lived-in spaces, recurring objects, voice textures for all four supporting networks, conditional ambient exchanges, and a restrained sensory palette. Flavor now reflects accepted work, stale information, local limits, and ordinary shelter life without assigning moral labels or inventing new gameplay authority.

## Installment 33 — Authored decision scenes with action-shaped branches

The following scenes are intended as reusable quest modules. Each has a concrete situation, several defensible actions, an immediate result, and a later consequence. Their purpose is to make a player’s approach visible through what they do under pressure. Dialogue can acknowledge a pattern, but the branch is driven by completed actions and accepted terms rather than an alignment label.

These modules may be adapted to different acts and routes. Their exact entry conditions depend on the current quest graph and must be verified before implementation. A route variation can change who makes the request or what service is available, but should not erase the choice’s basic human stakes.

## 354. Decision scene — “Before you post”

**Situation:** A revised schedule must be visible before the next shift. The draft includes a worker’s name, an unconfirmed delivery time, and a reason for the change. The writer says the details are all useful. The worker has not reviewed the copy.

**Player actions:**

- Post the full draft now. The schedule is immediately legible; the worker may object to the name or reason being shared, and the delivery time may need correction later.
- Ask the worker what can be public. This costs time and may produce a narrower schedule, but the scope is agreed.
- Post the time and task, mark the source unconfirmed, and keep the reason off the board. People can prepare while uncertainty remains visible.
- Ask the Signal Hands to carry the private context directly. This adds a second handoff and only works if a carrier accepts it.
- Hold the whole schedule until every detail is checked. This avoids a premature copy but can leave a staffed task uncoordinated.

**Immediate response:** the player sees who accepts the schedule and who still needs to be contacted. A character may ask whether “posted” means “accepted.” The scene should distinguish notice from consent.

**Later consequence:** if the player posted the full draft and a worker objects, the next board scene can show a correction mark and a specific conversation about scope. If the time was marked unconfirmed and later verified, the Signal Hands can carry a simple update. If the player waited, the missed coordination must be acknowledged rather than hidden by an offscreen solution.

**Sample line:** “I can work from a time that might move. I can’t work from a name that volunteered me.”

## 355. Decision scene — “The volunteer who says maybe”

**Situation:** The player needs someone to check a repair after the next shift. Nara says, “Maybe, if I’m back before dark.” The Bench Crew would like to mark the check as covered.

**Player actions:**

- Record the check as pending and ask when to confirm. The repair plan remains conditional; someone must follow up.
- Ask whether another person can accept a backup. If that person accepts, the group has a real fallback; if nobody does, the plan stays open.
- Schedule the check for a later time that Nara can accept with confidence. The repair stays provisional longer.
- Use another inspection method with the repair owner’s approval. It may cost materials or access but avoids treating Nara’s uncertain availability as a promise.
- Mark Nara as the checker anyway. The game allows the action but later shows an unstaffed check and the damage to trust when she returns.

**Immediate response:** the UI and dialogue should use “pending,” “accepted,” or “uncovered” accurately. No hidden roll should convert “maybe” into acceptance.

**Later consequence:** the check can be completed by a backup, rescheduled, or missed. If Nara returns in time, she may still choose to do it; the previous “maybe” was not a commitment. The player’s plan should respond to the actual action.

**Sample line:** “If you need a yes, ask me after the route. Right now I’ve got a maybe.”

## 356. Decision scene — “What can the route report prove?”

**Situation:** A faction representative asks whether the lower path is safe. The Route Witness has crossed it once on foot, in daylight, and found one section passable. The representative wants an answer before assigning a delivery.

**Player actions:**

- Report exactly what the witness observed, including time, mode of travel, and limits. The faction may still choose to send a cart, but the shelter’s statement is precise.
- Ask for a second check tailored to the planned load. This takes time and may be declined if the witness does not want another trip.
- Recommend an alternate route known to be longer. The delivery can proceed with a different cost.
- Decline to answer until the route is checked again. This protects against overstatement and may lose the current service window.
- Say “safe” to satisfy the request. The player gains immediate momentum, but the later scene can show the gap between what was observed and what was promised.

**Route-specific reaction:** the Military asks for a capacity estimate; the Rebels ask who can update the local mark; the Independent broker asks whether the observation satisfies the delivery term. A no-commitment player can simply choose whether to relay the observation or travel by another known path.

**Later consequence:** the system records what actually happened on the route through its owner. The quest records only the report and decision. A route can be passable even if the report was cautious, or blocked even if a prior crossing succeeded. The narrative must not falsely treat caution as a failed prediction.

**Sample line:** “I can tell you where I walked. I can’t turn my footprints into a cart test.”

## 357. Decision scene — “The offer includes a second request”

**Situation:** An Independent broker offers a part for the pump. The receipt is straightforward, but a second line asks the shelter to provide a named coverage list at the next delivery. The pump needs the part; the list has not been discussed with the workers.

**Player actions:**

- Accept both terms and ask the affected workers to review the list before it is sent. This may close the trade if they decline.
- Accept the part while negotiating the second request as optional or separately priced. The broker may agree, counteroffer, or withdraw.
- Ask the current inventory owner whether another repair path exists before committing. This can take time and may confirm there is no substitute.
- Refuse the entire exchange because the package terms are unacceptable. The pump remains unrepaired through this source, and another option must be found.
- Accept the part and send the list without consent. The exchange completes, but the later disclosure breach is explicit and cannot be erased by the repair’s success.

**Immediate response:** the broker states which terms are accepted. The player sees whether the part is transferred and when the second request is due. A broad “accepted” response must not silently agree to an undefined future service.

**Later consequence:** if the list is negotiated separately, the exchange receipt can be complete while the follow-up remains undecided. If the player shares names without review, affected workers can correct the record and limit future cooperation. If the broker withdraws, the player may reopen negotiations later with a different scope.

**Sample line:** “The bearing is one line. The roster is another. I’d like you to answer them separately.”

## 358. Decision scene — “The count and the reason”

**Situation:** Common Table witnessed a parcel handoff. The counterparty asks who received it and why, saying a reason will help decide whether to repeat the exchange. The recipient agreed to confirm receipt but did not agree to share the reason.

**Player actions:**

- Provide the witnessed count and receipt confirmation only. The counterparty has proof the item moved but not the context they requested.
- Ask the recipient whether they now consent to sharing the reason. A new consent decision is possible; previous consent to receive the item is not enough.
- Offer an aggregate outcome such as “delivered to the agreed person,” if that meets the counterparty’s actual need.
- Decline to provide any record. The recipient’s scope is respected, but the counterparty may not renew.
- Share the reason without consent to preserve the relationship with the counterparty. Later scenes reflect the specific breach and the recipient’s response.

**Branch outcome:** the counterparty may accept the narrow confirmation, request a different proof for future exchanges, or end the service. Common Table can keep witnessing other transactions. The player does not determine who deserves aid, and no hidden generosity score is awarded.

**Sample line:** “You have the count. The reason was never part of the handoff.”

## 359. Decision scene — “The correction already traveled”

**Situation:** The player discovers a schedule copy has an outdated time. One Signal Hand already delivered a correction to one work point. A second copy remains unchanged, and a resident says they left based on the earlier time.

**Player actions:**

- Send another carrier to the second point and ask the resident to return if reachable. This spends time and relay effort.
- Mark the stale copy visibly and leave a note at the departure point. It may not reach the person already traveling.
- Ask a Route Witness to check whether the resident can be met at a nearby turn. The witness must accept the trip; their observation is not guaranteed.
- Keep the corrected copy but stop trying to recall the resident. The current shift proceeds, and the player records that the correction did not reach everyone.
- Hide the outdated copy without replacing it. The immediate contradiction disappears, but affected people may have no current schedule.

**Later consequence:** the next meeting can ask what the correction process should be: one relay owner, two confirmed points, an expiry on posted times, or no standing update beyond direct contact. The resident may be inconvenienced without suffering a contrived disaster. If the process changes, someone must accept its upkeep.

**Sample line:** “The new time is right here. I’m still on the old road.”

## 360. Decision scene — “One rule or two local versions?”

**Situation:** Two work points have different needs. One is staffed continuously; the other has only one volunteer at a time. A faction representative asks for one standard shift rule so reports can be compared.

**Player actions:**

- Use one rule at both points. Reporting is easier; the less-staffed point may be unable to meet it.
- Keep local versions with a shared minimum: the same fields are reported, but staffing method differs. Comparison takes more explanation.
- Pilot the standard at one point and review it before wider use. The other point keeps its current method for now.
- Ask each group to propose its own coverage and report only the total. This protects local control but gives the faction less detail.
- Reject a common process and negotiate each service individually. This may preserve fit while increasing administrative effort.

**Later consequence:** each point can succeed or fail independently. A missed interval at one place does not erase coverage at the other. The faction can renew one service and decline another. A supporting network may help translate between procedures without taking ownership of either.

**Sample line:** “Compare the hours if you need them. Don’t pretend we staffed them the same way.”

## 361. Choice language that reveals action

Choice text should state the action rather than label the intended personality. Compare:

- **Action:** “Post the time and mark the source unconfirmed.”
- **Action:** “Ask the worker what can be copied publicly.”
- **Action:** “Delay the delivery until a second route check.”
- **Action:** “Accept the part and negotiate the roster separately.”
- **Action:** “Keep the handoff receipt; leave the reason private.”

Avoid choices such as “Be honest,” “Show compassion,” “Trust the faction,” or “Act independently.” They flatten distinct operational outcomes into a moral shorthand and encourage players to select an identity rather than evaluate a situation. If a choice includes an explicit role-play line, keep the action clear in the description or immediate response.

## 362. Decision aftermath by observable fact

Each module should leave at least one trace the player can observe: the copy’s status, the actual person who accepted work, the route report’s scope, whether the part changed hands, whether an exchange renewed, or which correction arrived. Character reactions should refer to those facts. If the game cannot represent a proposed fact through an existing owner, the implementation plan must say so and defer that branch until the proper authority is assigned.

The trace need not be a numerical meter. A changed prop, revised schedule, journal summary, returned tool, paused service, or line of dialogue can communicate the result. The important distinction is between authored intention and world outcome: selecting “ask for a second check” does not mean a second check occurred until someone accepts and completes it.

## 363. Installment 33 closing note

This installment adds seven decision modules covering public schedules, uncertain labor, route evidence, compound offers, exchange privacy, corrections already in transit, and local-versus-standard procedures. Each choice is an action with visible immediate costs and later callbacks, supporting varied play without moral labels.

## Installment 34 — Beyond the board: a field survey with several kinds of value

The central campaign should sometimes let the player leave the shelter and encounter a problem that cannot be solved by arranging a better copy. This optional field arc gives exploration its own texture: route reading, careful observation, material inspection, conversation with people encountered outside, and the choice to turn back. A player can complete it for a usable route report, for a salvage lead, for contact with a distant group, or for a clearer decision not to travel again.

The arc is titled **“The Marker Past the Turn.”** A pale marker that used to point toward an old service road has been repainted. The new arrow points north, but the current route note lists the northern gate as closed. A strip of blue cloth is tied around the post. Nobody knows who placed it or when. The marker might have been corrected, reused for another purpose, or shifted by weather and repeated handling. The player is not promised a hidden villain or a rare reward for solving it.

The investigation can be initiated by the player, a Long Walk report, a Route Witness observation, a Scavenger Guild interest in the marker hardware, or an Archivist’s dated map. These are alternative invitations. No single support group is required, and the player can leave the question unanswered while continuing the main campaign.

## 364. Entry branches — Why go out there?

Four motivations can start the field arc. Each frames a different useful question:

- **Route need:** a planned delivery needs a second path if the usual crossing remains blocked. The player wants to learn whether the service road is passable.
- **Material interest:** the Scavenger Guild notices that the marker uses a mount that might be reused. The player wants to inspect it without promising to dismantle it.
- **Historical comparison:** an Archivist has a dated map with a similar mark. The player wants to compare the location and record what remains uncertain.
- **Personal curiosity:** the player sees the changed arrow and asks who touched it. Curiosity is a valid entry, but the trip can end with no answer about who changed it.

The player can also decline each invitation. If no one goes, the marker stays unverified. The main story continues with the known route or an alternate service. Refusal is recorded only if a later scene needs to know that the report was never made.

## 365. Preparation — Choose a field method

Before leaving, the player chooses what the trip is for and what evidence would justify returning. The method sets available approaches and costs without assigning a character class.

- **Survey by stages:** record each visible landmark and compare the route on the return. This is slower and gives the clearest comparison if the traveler accepts the task.
- **Reach the marker directly:** follow the most legible line and inspect the changed arrow. This can reach the site sooner but may miss side paths and context.
- **Ask the Long Walk for a time-bound route report:** receive a known departure window and an estimate of the return. The report does not prove the current marker is accurate.
- **Take a Guild inspection kit or request a demonstration:** focus on the mount, fasteners, and whether it was recently moved. The Guild does not guarantee the route is open.
- **Use an old Archivist map:** compare landmarks and historical names, preserving the map’s date and source. An old map is context, not a current route guarantee.
- **Travel light and set a turn-back point:** sacrifice a second inspection opportunity in exchange for a clear limit on the trip. This makes stopping an explicit plan rather than a failure.

If a companion is proposed, the player asks that person directly. A companion can accept only the outward trip, the return, a limited inspection, or no part of the route. The trip can still proceed without the companion if another method remains. The player cannot assign an absent resident through a dialogue shortcut.

## 366. Field node one — The Sluice Steps

The first point has a clear view over three route lines. The old service road descends beside a drainage channel; a higher path follows the ridge; a narrow foot track runs behind a wall. From the steps, the player can see fresh paint on the marker but cannot see what lies past the next bend.

The player can sketch the visible branches, compare them with the old map, inspect the drainage channel from a safe distance, or continue to the turn. If the player sketches only the route chosen, the report is narrow. If they record all three, they spend more time and must still verify whether the lines remain passable. A route can be visible and unusable.

At the steps, the blue cloth is visible on a branch but not tied to the marker. A Route Witness may say it could be a field repair tag, a personal marker, or cloth caught by the wind. The player can check it, leave it, or record its position without assigning a purpose. The clue is not a test of whether the player guesses the correct owner.

## 367. Field node two — The Service Shed

The shed stands beyond the first bend. Its latch is open; a chalk arrow inside points toward the higher path. There are old tool marks on the door and newer scratches near the hinge. The shed contains no required key or unique item. Its value depends on the player’s chosen purpose.

**Survey approach:** compare the chalk arrow with the route from the Sluice Steps. The arrow may have been made for workers approaching from the opposite direction. The player can map both approaches, ask a witness to read the mark, or treat the arrow as ambiguous.

**Salvage approach:** ask the Guild member to inspect the hinge and wall bracket. They can identify a reusable mount only if its condition supports that claim. The player can leave it in place, remove a component with permission from the relevant owner if known, or take a rubbing/measurement instead. Salvage must use the current item authority if anything is actually collected.

**Historical approach:** compare the shed’s location with an archival landmark. A name may match, but that does not prove the old map was made by the same workers who left the chalk. The player can preserve both names, annotate a possible match, or omit the historical link from an operational report.

**Contact approach:** a person from a nearby work camp arrives while the player is inspecting the shed. They ask whether the shelter is planning to use the road. The player can answer directly, ask what the person knows about the marker, offer a limited exchange, or end the conversation and continue the inspection. The person may decline to give a full route account; the quest continues.

## 368. The changed marker — Four readings, no forced culprit

At the marker, the player finds that the arrow is attached with a removable bracket. One screw is newer than the others. The post itself has weathered marks showing it was turned more than once. The facts support several interpretations:

1. Someone redirected the marker for the current season.
2. A repair moved the sign while the post was being stabilized.
3. The arrow was reused from another location.
4. The sign was turned during a past route closure and never restored.

The player can inspect the fasteners; ask the nearby contact; compare the mark with the Archivist’s map; ask a Route Witness to check the path beyond it; or write down that the reason remains unknown. Two explanations can both be true at different times. The scene should not narrow to a definitive culprit unless a direct, current source provides evidence.

The player may keep the marker as found, turn it back toward the old road, remove it, add a dated temporary note, or leave a clear “unverified beyond this point” mark. Any physical change must be based on an actual available action and the player’s authority to make it. If the marker serves local travelers, the player should consult them or accept the consequence of changing it without agreement.

## 369. Branch outcome — What counts as a successful expedition?

The field arc has several valid completions:

- **Verified alternate route:** the player reaches a useful section, records the observation time and limits, and returns. The road is not declared universally safe.
- **Useful material lead:** the Guild identifies a reusable mount or repair method, but the route itself remains partly unknown. The player can bring back the finding without dismantling the marker.
- **Contact established:** the nearby worker accepts a bounded future exchange or shares a limited local observation. The player does not gain ownership of the camp’s route knowledge.
- **Marker corrected by agreement:** local users confirm the new direction or accept a temporary label. The reason for the earlier change may remain unknown.
- **Return at the planned limit:** the player turns back before reaching the marker because conditions, time, or companion capacity do not fit the plan. The expedition yields a partial observation and a clear reason for returning.
- **No useful finding:** the player reaches the post but cannot verify a route, identify a source, or safely alter it. A truthful unresolved report is still a complete conclusion for this attempt.

The quest journal should describe the value actually gained. It may say, “Observed the service road from the Sluice Steps; the path past the bend remains unchecked.” It should not say, “Discovered the northern route,” if the player never traveled it.

## 370. Supporting-current contributions on the field trip

### Long Walk

Long Walk can provide route windows, past travel notes, and a return expectation when its current conditions allow. A report is dated and scoped. A courier may know the road but not the marker’s recent history. If the route report arrives late, the player can use it on a later attempt without rewriting the first trip’s decision.

### Scavenger Guild

The Guild can assess material quality, demonstrate a safe removal method if the item owner permits it, and offer a salvage alternative. A part’s value depends on condition and fit. The Guild does not automatically award an item because the player completed an inspection scene. A refusal to dismantle the sign can be the most practical outcome if its use is still uncertain.

### Archivists

The Archivists can compare dated names, preserve a field sketch, and explain what the map does not prove. They may offer to retain a copy under explicit access terms. They cannot identify the arrow’s author from a similar mark alone and do not adjudicate who owns the road.

### Route Witnesses

Route Witnesses can split the survey into a near-side observation and a farther check, if someone accepts both. Their statements identify vantage and time. They may tell the player to stop at the turn-back point if conditions change. The player can continue alone only if the current travel rules and their own plan allow it.

If a current is unavailable or declines, the field arc retains a basic player-only route. It may yield less information or cost more time, but is not blocked.

## 371. The faction’s request after the return

After the trip, one major faction asks for the result. The request differs by route, and each asks the player to interpret the report in a different institutional frame.

- **Military:** asks whether the service road can carry a planned group or delivery. The player can provide the specific foot or cart observation, request a dedicated check, recommend a different path, or decline to certify it. The report’s limits remain visible.
- **Rebel:** asks to share the route map among local teams. The player can provide the verified portion, include an uncertainty mark, ask who will update distributed copies, or keep the survey local. The organizer can accept a partial map or decline it as insufficient.
- **Independent:** offers a limited trade or delivery if the player provides a route description. The player can negotiate what the report promises, share only the observed segment, use an alternate exchange point, or reject the terms.
- **No commitment:** the player can use the report for local planning, share it with a nearby contact, trade the information for a specific good if such an exchange is supported, or keep it private. The reach is smaller, and the shelter must maintain its own copies.

The faction request arrives after the expedition, so the player cannot pretend the faction’s need was the sole reason for going out unless they accepted that objective beforehand. The player may share a report without transferring custody of the original field notes.

## 372. Exploration play approaches

The field arc supports several methods without class selection:

- **Cartographer:** records junctions and return points, gains a more legible route picture, and spends time on scope.
- **Scout:** prioritizes the uncertain bend, learns less about side routes, and accepts a larger risk of an incomplete return.
- **Salvage inspector:** studies the marker and shed hardware, may identify a repair or reuse opportunity, and can leave route safety unanswered.
- **Contact seeker:** spends time speaking with the nearby worker, gains local context if offered, and may leave with no map that the shelter can rely on.
- **Archivist’s reader:** compares dated material and keeps source distinctions clear, but may have less current field evidence.
- **Conservative returner:** uses the turn-back point, preserves the team’s capacity, and leaves the farther route unknown.
- **Opportunistic reviser:** changes the plan after seeing new evidence. The journal distinguishes the initial aim from the revised one; changing course is a consequence, not inconsistency.

These approaches may combine. A player can sketch the near route, inspect the sign, speak briefly with the worker, and turn back. The game should reflect the actual sequence rather than assign one dominant style.

## 373. Closing scenes for the field arc

**A dated path:** The player’s route note has a new time in the margin. It lists two checked junctions and one unchecked bend. A faction can plan around the checked stretch; nobody calls the whole road open.

**A repaired marker:** The post points toward the service road again, but a temporary label says “verify beyond bend.” The nearby worker says the new label will help them, then asks who will replace it if the weather takes it. The player can accept a review or leave upkeep to the local group.

**A tool instead of a route:** The Guild’s inspection shows that the mount can be used at the shelter. The marker remains where it is, and the player brings back a fitting note instead of the sign itself. The route is still unknown; the workshop has a practical lead.

**A person’s account:** The nearby worker shares why they use one particular path but declines to give a wider survey. The player can respect the boundary and still use the account for the one crossing it describes. If they press for more, the person ends the conversation.

**An empty return:** Nothing is recovered, the marker remains ambiguous, and the player comes back with fewer answers than expected. A character can ask what the trip was for. The player’s reply may be that now they know what not to promise. The scene respects the cost without inventing a reward.

## 374. Field arc integration boundaries

The field trip is a quest proposal, not authority to add route simulation. Before implementation, confirm how current travel and expedition owners represent destination, travel time, risk, returning companions, and save/restore. A field node may be narrative-only if no spatial route API exists; do not create a quest-local travel graph to imitate one.

Any recovered item must pass through the existing inventory authority. Any exposure or injury consequence must come from the current relevant owner. Any route observation must not become a global safe/unsafe value unless the current route system supports that meaning. Contacts encountered outside need an authored source and availability condition; they must not appear everywhere because the plot needs an answer.

The quest should have entry, return, interruption, and closure paths. A companion can refuse, a report can arrive too late, a path can stay unverified, and the player can turn back. None of those conditions should block the major faction storyline or force an unsupported recovery. The content audit must prove each planned branch is reachable and that every reported outcome can be observed in the proper system or through an explicit narrative event.

## 375. Installment 34 closing note

This installment adds an optional multi-stage field survey with route-planning, observation, salvage, history, contact, and turn-back approaches. It gives the Long Walk, Scavenger Guild, Archivists, and Route Witnesses distinct supporting roles; offers route-specific follow-up requests; and makes partial knowledge a meaningful expedition result rather than a failed quest.

## Installment 35 — The report comes home

Returning from the field is not a single “quest complete” button. The traveler arrives with some combination of direct observations, other people’s accounts, recovered material, unverified interpretations, and unanswered questions. The shelter must decide how to use those pieces. This installment deepens the expedition’s homecoming and gives the trip consequences beyond receiving a journal summary.

The first rule is to keep the field report in layers. What the player saw belongs in one layer; what a companion saw belongs to that companion; what a contact said belongs to the contact; what an old map suggests belongs to its source; and what the player infers remains an inference. The player can bring these layers together for a practical decision without pretending they have all become one confirmed fact.

## 376. Return condition — The traveler arrives before the room is ready

When the traveler returns, the shelter may be mid-shift. The people who asked for the report may be absent, the board may already have changed, and the repair task may still be active. The player can leave a short arrival note, wait for the intended recipient, ask a current witness to hold the report, or give only the immediate safety detail now and save the rest for later.

Each choice changes who can act first:

- **Leave a short arrival note:** the next shift knows that the traveler is back and where the report can be discussed. No route details are assumed to have been read.
- **Wait for the intended recipient:** the report stays directly with its intended audience, but any time-sensitive plan may remain unresolved.
- **Ask a witness to hold the report:** a witness can confirm custody if they accept, but holding does not grant them authority to summarize it.
- **Share only an immediate warning:** the urgent part reaches people quickly, while the wider survey waits. This may require a second conversation later.

If no one accepts custody, the player keeps the report. The narrative does not silently put it on a public board. If the player leaves it unattended, characters may react to the actual handling, but the game should not fabricate who read it.

## 377. Debrief structure — Five questions, no interrogation tree

The debrief can be short or detailed. It should let the player identify the report’s useful scope without forcing a complete transcript. Five prompts are available, and any can be skipped:

1. **What did you personally see?** Choose a landmark, marker condition, or checked segment actually observed.
2. **What came from another person?** Name a companion or contact only if they agreed to be identified in the report.
3. **What did you bring back?** Record a material only if the existing inventory authority confirms it; otherwise report a measurement, sketch, or offer.
4. **What remains unknown?** Select one or more unchecked sections or uncertain interpretations.
5. **What decision should this inform?** Identify a route option, repair choice, contact, or faction request.

The player may focus on one question and leave the rest unresolved. A cartographer can bring a clear segment map and no history. A salvage inspector can bring a fitting note and no route proof. A contact seeker can bring a person’s limited account without having checked the wider road. The debrief should not convert these approaches into a ranked result.

## 378. The first interpretation dispute

At least one resident compares the report with an earlier local belief. The disagreement can be about the marker’s purpose, the road’s condition, or who should maintain the temporary note. The resident is not automatically wrong because the player has an external observation. A route can have changed since their last trip.

The player can place both reports side by side; ask when each was made; identify where they refer to different segments; use the newer observation for the next shift while retaining the older one as history; or choose the safer route until the conflict is resolved. If the player declares one account false without evidence, the person whose account was dismissed can challenge the inference later.

This exchange can rejoin once the next task has a usable instruction. Rejoining does not erase the disputed account. The local map can say “checked to the bend today” and “beyond the bend: older report.” That concise operational description is enough for the next action, even if nobody agrees about the marker’s origin.

## 379. The map review is not a faction vote

If the player invites several people to review the report, each discusses one practical concern. The Route Witness asks about time and vantage. The Long Walk asks whether the proposed trip fits a known return window. The Guild asks whether the mount can be moved without damaging the marker. The Archivist asks whether the old map should be kept with its date and source. A local contact may ask that the shelter not publish a description of their work site.

The player can ask for a round of short observations, hold separate conversations, share only the checked map segment, or skip a collective review. A meeting does not create consensus automatically. If two participants disagree, the player can note both positions and make a bounded decision for the next trip.

The review should not create a new Current council, new faction authority, or new persistent panel. It is a scene in which existing actors offer their scope of knowledge. Any lasting agreement must have a supported owner and a clear, accepted task.

## 380. Who may receive the field report?

The traveler’s report can be divided into four delivery forms:

- **Operational extract:** checked segment, observation time, and uncertainty. Useful for route planning; excludes personal descriptions and historical speculation.
- **Full field account:** observations, measurements, sketches, and attributed statements. More useful for long-term comparison; requires agreement from any named source.
- **Material note:** fitting dimensions or repair condition supplied to the Guild or maintenance owner. It need not include the contact’s identity.
- **Private contact follow-up:** a direct message to the person met outside. It can ask permission to return, share one practical update, or close the exchange.

The player can provide different extracts to different recipients. The Military may receive a checked segment and capacity limit. The Rebels may receive the same segment with a local correction method. The Independent broker may receive a report that satisfies one agreement but does not include future route access. A no-commitment player can keep the report local or negotiate each request separately.

If a faction asks for the full account, the player can ask why it needs that level of detail, give a narrower extract, obtain consent for named statements, or refuse. A refusal may cost access to that faction’s service. The support networks cannot force a disclosure, and their own willingness to retain a copy remains separately scoped.

## 381. The nearby contact’s return choice

If the field trip included a nearby worker, the contact can choose among three follow-ups: meet again at the same place; send a limited report through a carrier; or leave the matter there. The player can ask for another route fact, offer a specific exchange, return an item or measurement, or accept the ending without another visit.

The contact can also change their mind. They may agree to share a path description but not the marker history; offer a one-time check but no recurring survey; accept the player’s map while refusing to appear in the public account; or end contact after a request expands beyond its original scope. The player can accept, renegotiate, or stop. No relationship meter obliges the contact to keep cooperating.

The contact’s decision can lead to one of several future possibilities: a short return exchange, a new independent observation, an offer of salvage help, a disagreement over what the shelter told a faction, or no further meeting. The main quest does not require the contact to remain available.

## 382. Reusing the report on a later shift

The report can inform a later task, but its validity is bounded by what can change. A stable landmark may remain useful for orientation; a route condition can age quickly; a repair measurement remains relevant only to the object inspected; a contact offer may expire. The game should not apply a generic freshness timer to every kind of information.

At a later shift, the player can:

- use the checked segment as a starting point and ask for an updated crossing observation;
- rely on the repair measurement while the object remains unchanged;
- revisit the old map comparison while labeling the new report separately;
- use the contact’s one-time offer only if its accepted terms remain current;
- disregard the old route condition and choose a known alternative.

If the route later changes, the old report is still true about its observation time. The world state can change without rewriting history. This is a key source of depth: useful evidence has context, and characters learn to ask what was checked when.

## 383. Faction request changes after new evidence

The field report may change what a major faction asks next, but it should not force a route switch. The Military can move a planned delivery to a checked junction, offer a more demanding survey request, or decide that the report is too limited. The Rebel organizer can ask whether local teams may update the map and which copies need correction. The Independent broker can adjust a service term or ask for a paid second check. Each counterparty can decline the player’s offer or propose another exchange.

The player can use the new report to renegotiate previously accepted work. If the original plan depended on the road being open and the new report says otherwise, the player can pause it, choose the verified segment only, or accept a longer alternative. The faction may be dissatisfied, but it cannot demand that the shelter treat an outdated assumption as current truth.

The report can also strengthen a decision to refuse. The player may now know that the route is not suitable for the offered service, that the repair will take longer than expected, or that a contact does not wish to be included. A well-supported refusal can preserve a working relationship even when it closes the immediate transaction.

## 384. Late-arriving evidence and re-entry

Sometimes a second report arrives after the field arc has concluded. It may confirm one segment, dispute a marker interpretation, or say only that conditions changed. The late report can reopen a practical decision without resurrecting the entire quest.

The player can append the new source and time; ask whether the old report still supports the current task; notify those who received the earlier extract; or decide the difference does not affect the present plan. If the late source is a person who asked for anonymity, their identity remains protected unless they change that request. If the report contains no new actionable fact, the player can archive it or leave it outside the public schedule.

The game should avoid late reports that exist only to shame a previous choice. A cautious route decision remains reasonable based on the evidence available at that time. A confident decision can also be understandable even when later conditions change. The character callback can discuss what the new information changes, not claim that the player should have predicted it.

## 385. Sample debrief — “What came back is not the road”

The player sets the folded map on the bench. A line of pale dust follows the crease.

Rade waits until the map is flat. “Which part did you cross?”

The player can point to the Sluice Steps and the bend; show the marker hardware; describe the contact’s account without naming them; or say that the trip ended before the road itself was checked.

Mina looks at the sketch. “You’ve got enough here to set the mount. Not enough to call the hinge safe.”

Osha turns the paper toward the light. “That mark is in the old copy. The new arrow is yours from today.”

The player can ask Mina to assess a repair, ask Osha about a second route window, let the Archivist preserve the dated copy, or keep the report for local use. If the player chooses more than one option, each person accepts a separate task only if they agree.

Rade taps the unmarked bend. “Then that’s still blank.”

The blank is left on the map. The player can plan around it, arrange a later check, or stop pursuing the route. The scene closes on the next practical decision, not on a demand to solve the unexplained marker.

## 386. Return-phase acceptance criteria

The content review for this return phase should confirm:

1. The report distinguishes direct observation, attributed account, recovered item, and inference.
2. The traveler’s arrival does not imply that intended recipients read the report.
3. Each network’s review is optional and limited to its actual knowledge.
4. Field notes can be split by audience without changing the source record silently.
5. A nearby contact can end or narrow cooperation without blocking the campaign.
6. Later route conditions do not retroactively falsify a time-bound earlier observation.
7. A faction’s changed request follows the report and its own service authority.
8. Delayed evidence creates a new decision without shaming the earlier choice.
9. The player can close with partial knowledge and still receive a complete quest outcome.
10. All implemented effects resolve through current route, inventory, faction, quest, and save owners.

## 387. Installment 35 closing note

This installment expands the homecoming phase of “The Marker Past the Turn.” It adds report layering, flexible debriefs, review and sharing choices, a returning contact’s limits, later report reuse, faction renegotiation, and late evidence. The expedition now has consequences both outside and inside the shelter while preserving uncertainty and the player’s freedom to stop.

## Installment 36 — Epilogues: the first ordinary day after the choice

The final campaign image should show the new arrangement under use. It should not be a victory tableau, a faction ceremony, or a fresh emergency. The shelter has made a choice about its work, information, and outside relationships; the epilogue lets that choice become visible during an ordinary day.

The four route scenes below share the repaired wall and the first shift after the finale, but they differ in who approaches the board, what service is expected, and what remains unsettled. Each can receive one short callback from a support network and one from a resident. The scene should not stack every possible callback into a roll call. Select what the player’s actions made most relevant.

## 388. Military route epilogue — “Window at first light”

Before sunrise, the Military liaison arrives with a folded service request. The schedule is posted at the work point, not at the public entrance. It lists one confirmed window, a check time for the temporary repair, and one unstaffed interval marked plainly. The liaison reads it without touching the page.

“That’s narrower than the first offer,” the liaison says.

The player can explain the staffing limit, offer to review a second window after the next shift, ask what the Military can provide in exchange for a wider commitment, or say that the current agreement is all the shelter can maintain. Each answer keeps the existing campaign route and alters the relationship’s next practical step.

If the player’s earlier reports were reliable, the liaison can accept the narrow schedule as something they can plan around. They may still dislike its limits. If the player previously overpromised and corrected the record, the liaison can request one completed shift before considering an extension. If the player disclosed a worker’s name without consent, the schedule now uses confirmed coverage without the personal explanation; the liaison receives the operational information but cannot demand that the disclosure breach be treated as resolved.

The Bench Crew member checks the wall mount and leaves the temporary marker in place if a later inspection is still due. The player can ask for the check now, leave it to the accepted worker, or keep the board at the current location until the agreed time. The scene ends with a short signal from the liaison—not an order—confirming the delivery window or asking for a follow-up.

The route’s result is dependable at a chosen scale. A larger promise remains possible, but only after the shelter has evidence that it can staff and maintain it. The Military has a role in the ending without acquiring the shelter’s schedule as permanent command property.

## 389. Rebel route epilogue — “The second copy is current”

The room is busier than at first light. Two people arrive from different work points and compare the copies they carried. One is current; the other still has yesterday’s time crossed through. The newer copy has a correction mark and a review note. Nobody says the distributed process is effortless.

The Rebel organizer asks whether the shelter wants help sending the next update. The player can let local volunteers correct both copies; keep the current copy as master for one shift; hold a short meeting to renew the copy owners; or pause distribution until someone accepts maintenance. If one volunteer declined the role, that location is not quietly staffed by an unnamed extra person.

If the player’s actions previously built a correction path, the second copy is updated before the next departure. The organizer can say, “It got here because someone carried it, not because the paper knew where to go.” If the player shared responsibility without confirming acceptance, the stale copy can remain visible. The Rebel route can still end with local autonomy, but the scene acknowledges that an unowned process has gaps.

Dema may see a newcomer reading the guide and ask whether they need a direct explanation. If a volunteer accepted upkeep, Dema leaves the guide with them; if not, she can offer one bounded orientation and then return to her own work. The player can help, ask another person to volunteer, or let the newcomer choose whom to ask.

The Rebel organizer leaves with the copy they are responsible for. The shelter keeps the other. They are connected by an accepted correction routine, not by a claim that every resident agrees on every detail. The ending emphasizes local participation and its maintenance cost together.

## 390. Independent route epilogue — “The receipt is enough”

On the table is the last receipt from the exchange. It records what arrived, the discrepancy that was noted, and the date the term ended. The broker reads it once, then asks whether the shelter wants to renew.

The player can renew the same limited service, offer a different quantity, ask to wait until stock is reviewed, or decline. If the parties renew, a new term is written and accepted separately. If the player declines, the broker can take the receipt and leave without a retaliatory line. The completed exchange remains completed.

If the player provided only the count, the broker may say that the record was sufficient for the agreed delivery but not for a larger guarantee. If the player gave more information than agreed, the recipient may choose not to renew their participation even if the broker is willing. If a temporary part is still on loan, the scene names its actual return condition; it does not disappear into the epilogue because the conversation ended.

Sera may check the supply count before the player answers. If she accepts the task, the count is confirmed through the inventory owner and the broker receives only the agreed proof. If she declines, the player can postpone the renewal or ask another accepted person. No one is assumed to perform the count simply because the exchange is important.

This ending rests on clear terms, not on a promise of permanent friendship. The broker can be useful again later. The shelter can also decide that the next trade is not worth its cost. Either outcome follows from a fresh choice.

## 391. No-commitment epilogue — “The short list”

The repaired wall has one plain sheet on it. The next shift, one confirmed contact point, and a dated route observation fit in the top half. The rest of the wall is empty. A folded account stays with the Archivists; a tool tag hangs at the Bench Crew’s station; the Long Walk report remains folded beside the local map.

The player can keep the short list, ask whether residents want another item posted, or leave the space open for the next shift’s actual needs. The absence of a faction seal does not mean the shelter has rejected every exchange. A contact may still bring a part, a route report may still help, and a local group may still accept one bounded request. What the ending lacks is a guaranteed regional service or a standing commitment from a major faction.

If the player accepted local collaboration, two residents can exchange a correction directly. If the player chose minimal coordination, the board may remain sparse and one person may ask where to get the route detail. The player can explain the contact path or accept that the information is not publicly available. The scene does not reward secrecy or punish it; it shows the actual reach of the chosen arrangement.

Pell may pass the doorway without taking watch. If he agreed to a bounded future observation, he can say what night he is available. If he did not, no witness note appears. The quiet space around the board is part of the ending: some work is deliberately not continuing.

The player can end on a practical question—what needs doing before dusk—or on a personal exchange with a resident. Neither choice converts no-commitment into an unspoken fifth faction. The shelter’s scale and the player’s authority remain limited.

## 392. Ending overlays from the supporting currents

At most one support-current callback should accompany each route epilogue, selected by the service that had the strongest actual consequence:

- **Archivists:** a dated copy is returned, retained, or left under its agreed access terms. The Archivist can say what was preserved and what is not proved by the record.
- **Long Walk:** a route report is returned late, on time, or not at all. The courier distinguishes the attempt from the delivery and asks whether another trip is wanted.
- **Scavenger Guild:** a repaired or borrowed part is checked, returned, or still awaiting inspection. The Guild discusses condition and fit, not moral ownership of the shelter’s workshop.
- **Signal Hands:** a correction reached one or more points. The carrier names which copies they updated and whether they accepted future relay work.
- **Bench Crew:** a repair is in use, provisional, or paused. The crew names who accepted the next check.
- **Route Witnesses:** the map carries a current, partial, or unknown report. A witness states where and when they observed it.
- **Common Table:** an exchange is complete, held, or expired. A member confirms the handoff only within the agreed scope.

Do not combine three callbacks simply because three groups appeared in the quest. The epilogue should protect the route’s central mood and leave optional detail discoverable elsewhere.

## 393. Character overlay: what the residents choose next

Resident presence can vary according to their own accepted tasks, but the ending must not freeze them into the player’s process:

- Rade may return to the board to correct a date, leave it alone because an owner is assigned, or ask who will notice the next outdated copy.
- Osha may take a route, decline a late trip, or ask the player to mark the return window before she accepts.
- Mina may inspect the mount, teach a bounded check, or postpone her work if another repair has priority.
- Dema may give one orientation, pass the guide to its accepted maintainer, or decline to explain a policy that is not settled.
- Iven may keep the meal time, accept a single handoff, or remind the player that the rotation was only a trial.
- Sera may confirm one count, ask for the reservation owner, or leave the stock decision open.
- Pell may provide one observation, decline the next watch, or ask for his name to be removed from a copy.

The player can ask after them, offer support, disagree, or let them proceed. Their future is not a loyalty reward. A resident who takes an independent action should do so only when the current game’s character and simulation systems support that behavior; otherwise, the authored scene should present a choice the player can respond to rather than claim an autonomous state change.

## 394. A final line chosen by the playthrough

The closing line may vary according to the dominant concrete action pattern, with no style label displayed:

- If the player repeatedly verified before sharing: “The copy is shorter today. Nobody had to guess what it meant.”
- If the player built accepted rotations: “Two people know the check now. Ask them before you put it on the schedule.”
- If the player often chose direct contact: “I found them at the pass-through. Took longer. They heard it from me.”
- If the player negotiated narrow services: “One window’s agreed. Tomorrow is still tomorrow.”
- If the player made and corrected a disclosure mistake: “The old page is gone from the board. I remember it was read.”
- If the player turned back from the expedition: “That road is still unknown. I’m glad we didn’t pretend otherwise.”
- If the player stopped pursuing optional details: “We’ve got enough for this shift.”

Only lines supported by recorded and accessible action history should appear. If the game cannot persist the specific pattern, choose the line from the current quest’s observable state or omit it. Never recreate an unsaved history through guessed dialogue.

## 395. Epilogue pacing and return to play

Keep the epilogue short enough that it feels like an ending to this arc, not the start of another act. The player should be able to inspect the wall, hear one support-current response, talk with one resident, and leave. Optional conversations may expand the scene, but the core outcome should be available in the first pass.

After the final line, allow normal shelter play to continue if the campaign does so. The schedule’s actual owners still determine assignments. Faction services remain available only according to their route and agreement rules. Temporary repairs remain temporary. The ending does not reset fatigue, replenish resources, close an unaccepted obligation, or make an expired offer live again.

If the campaign instead moves directly to a final credits or outcome screen, present the same information through a concise authored summary based on supported facts. Do not create a new epilogue simulation just to show every variation.

## 396. Epilogue acceptance review

An epilogue is ready for production review only when:

1. Its route remains the major campaign path already chosen.
2. The scene shows at least one operational consequence in use.
3. A support-current callback is based on accepted and completed work.
4. Resident lines reflect what each person knows and accepted.
5. Renewal is separate from completion and requires new agreement.
6. A sparse or incomplete result can still feel like a deliberate ending.
7. No resource, fatigue, schedule, or travel state is reset by narrative convenience.
8. The player’s final action is not scored on a good/evil axis.
9. The route’s unresolved cost remains visible without turning into a cliffhanger.
10. The scene can be skipped or left without blocking the campaign’s current closeout path.

## 397. Installment 36 closing note

This installment supplies full route-specific epilogue scenes, supporting-current overlays, resident futures, action-derived closing lines, and pacing requirements. It closes the arc on ordinary work in progress: each ending shows what the player’s choices made possible, what remains costly, and who has accepted the next step.

## Installment 37 — Production sequence and content acceptance path

The plan is intentionally much larger than a single implementation package. Its production value depends on choosing a coherent slice, verifying current owners, and preserving the branch behavior that makes the expansion distinct. The sequence below converts the authored scope into reviewable work without treating this plan as authorization to modify production code.

## 398. Gate zero — Reconfirm the premise and avoid content collision

Before assigning implementation, compare this document against the live branch, quest, character, encounter, location, and supporting-current catalogs. The review should answer:

- Which portions of Expansion 97 already exist in production or another plan?
- Do the provisional working networks collide with existing organizations, terms, or IDs?
- Which cast members and locations are already canonical, and which are only illustrative?
- Are the cited faction branches still the current major branches with the same route gates?
- Which action outcomes have a current quest or campaign owner?
- Does any content depend on a feature explicitly held or deferred by the integration authority?

The result is a collision note and a reduced, evidence-backed content list. If the premise is stale, return the affected sections for revision instead of forcing the proposed text into data. This gate avoids writing scenes around unavailable characters or making duplicate faction identities.

## 399. Gate one — Ship a single action branch end to end

Select one short slice from the core schedule conflict: one source, one work assignment, one accepted response, and one later correction. It should include a meaningful action other than selecting a moral dialogue label. The slice is considered authored only when the player can enter, make the choice, observe the actual result, and reach a truthful closure.

**Acceptance evidence:** the scene is reachable in the current route; the relevant owner records the accepted or refused task; a later consumer displays the result; absence and refusal still permit progression; and the action can be corrected or closed without a duplicate local state store.

The first slice should prove the source/recipient/scope distinction. It should not simultaneously implement every Current, all eight acts, or the field expedition. Keeping the first change narrow makes a failed assumption cheap to correct.

## 400. Gate two — Complete one character arc with a refusal path

Choose one resident arc (Dema, Iven, Sera, or Pell) only after verifying the character and quest owners. Author its entry, middle turn, one practical resolution, one refusal or absence path, and one later callback. Keep the character’s accepted task bounded and ensure the arc’s outcome does not create a second morality score.

**Acceptance evidence:** the arc works if the character agrees, declines, is absent, or changes their mind; all item and schedule effects use current owners; the closure is visible; and the line of dialogue reflects what the character could know. If implementation requires a new persisted action fact, the package must name its owner and save/restore path before adding it.

This gate should use one resident, not turn the full cast into a batch of parallel quests. The rest of the arcs can be authored and integrated once the pattern has demonstrated a current supported route.

## 401. Gate three — Add one supporting-current intersection

Pick the Current whose offer is relevant to the implemented slice. Archivists, Long Walk, and Scavenger Guild already have established bounded roles in the plan’s evidence base; the additional task networks remain provisional and require a collision check. Add one offer, one accepted scope, one refusal or renegotiation, and one completion report.

**Acceptance evidence:** the Current offers only its supported service; the player can use an alternative or decline; the Current does not take over inventory, route, faction, or character ownership; and a successful offer does not silently renew. For a faction intersection, show the major faction’s request and preserve its central route decision separately.

If no current Current can perform the planned role without a new authority, do not create an ad hoc generic service provider. Return to the foreman/user for a bounded architecture or content decision.

## 402. Gate four — Add one route overlay and one alternative route

The same core scene should be exercised under two campaign states: one major faction commitment and either a different major route or the no-commitment state. The differences should arise from request, service, language, and downstream consequence—not from rewriting the same morality check with new faction names.

**Acceptance evidence:** both routes remain reachable; route-specific content respects the live mutually-exclusive branch gate; the alternative path does not require hidden “perfect” behavior; and the player’s action history can produce mixed results. The plan should include at least one case where both routes share a useful local procedure but disagree about its scale or evidence.

Only after this proof should the content team expand the overlay to the remaining route states. The work should remain data-authoritative and avoid adding a new branch coordinator for authored narrative variations.

## 403. Gate five — Complete endings and secondary content

Once the core action, resident arc, and route overlay are live and reviewable, add supporting-current field quests, the expedition, and the route-specific epilogues in dependency order. Optional content must not become a hidden prerequisite for the main ending. Add each follow-up only when its source action can be established and its ending consumer can read it.

**Acceptance evidence:** optional quest absence has a default line and does not block progression; each epilogue reflects a major route plus at least one observable practical outcome; expired services require renewal; and no unresolved optional mystery blocks the closeout.

The art, audio, and UI work should follow the locations and interactions that are confirmed in game. Do not produce assets for provisional network names or unverified scene surfaces and then force the content to use them.

## 404. Branch traceability matrix for review

Every authored branch should have a review row with these fields:

| Field | Review question |
|---|---|
| Entry | What current route, act, character, and world facts make the scene available? |
| Action | What can the player physically or socially do here? |
| Acceptance | Who accepted the work, information scope, or service? |
| Owner | Which existing system, quest, or data authority records the result? |
| Cost | What time, item, access, disclosure, or labor is spent or delayed? |
| Failure | What happens if the actor refuses, is absent, or the evidence is late? |
| Callback | Which later consumer uses the result, and what can it truthfully say? |
| Closure | How does the player finish without completing optional branches? |

Rows should be concrete enough that a narrative reviewer can trace the scene and a technical integrator can identify the state owner. “Trust improves” is not a sufficient owner or callback. “The resident accepts one more delivery after the previous receipt was returned on time” is a reviewable condition if the current data and quest system can represent it.

## 405. Scope control during production

The full plan intentionally contains more possible scenes than any one playthrough should see. Production should select branches that carry different play experiences, then leave the rest optional or deferred. The player should not be made to read every support-network scene to understand the ending.

When scope is reduced, preserve branch diversity in this order:

1. Keep at least one action-based branch distinct from faction alignment.
2. Keep a meaningful refusal, delay, or turn-back route.
3. Preserve one consequence that becomes visible later.
4. Retain one supporting role with a bounded, accepted service.
5. Keep one mixed ending in which the player has a success and a cost.
6. Defer duplicate voice variants or low-impact callbacks before cutting the only alternative route.

This order is a creative prioritization guide, not a license to change project architecture. Any proposed state or content authority still follows the active governance and claim process.

## 406. Expansion 97 content sign-off checklist

Before treating any portion as implementation-ready, reviewers should be able to answer yes to the applicable items:

- The source audit still supports the described major-route and quest owners.
- New actions do not encode good/evil or honesty as the only branch axis.
- Supporting currents and task networks remain subordinate to major faction routes and player choices.
- Characters can refuse or limit labor without being punished by a hidden affection value.
- Branches have visible, attributable results and truthful failure paths.
- Survival, inventory, travel, radio, and save effects use their current owners.
- Optional discoveries are accessible through more than one fair method where necessary.
- Endings reflect distinct actions and permit mixed outcomes.
- Provisional names, locations, and scene props have passed a canon collision review.
- All implementation work has exact claims, acceptance criteria, and focused verification.

## 407. Installment 37 closing note

This installment makes the large creative proposal implementable in stages. It defines premise, branch, character, supporting-current, route, and ending gates; gives reviewers a traceability matrix; and protects the expansion’s action-driven variety when production scope is reduced.

## Installment 38 — Companion goals on the field survey

Companions should bring their own reasons for traveling. Their presence is not a passive bonus attached to the player’s chosen class, and taking someone along does not mean they consent to every detour or task. This packet extends “The Marker Past the Turn” with optional personal goals for Rade, Osha, and Mina, while allowing the expedition to proceed without any of them.

Before departure, each invited character can state one bounded interest. The player can include it in the trip plan, ask the character to keep the trip focused, propose another time, or leave the invitation open. The character chooses whether to accept the player’s scope. A companion’s optional goal can add a clue, route note, repair observation, or new question; it never provides an exclusive key to the main plot.

## 408. Rade’s goal — compare the present route with an old receipt

Rade asks to see whether the current route marker matches a receipt from an earlier handoff. He is not trying to prove someone lied; he wants to know whether the record described the same place. The player can let him compare the receipt at the Sluice Steps, ask him to keep it at the shelter and provide a description, or decline the comparison to protect the trip’s time limit.

If Rade joins, the scene gives him space to distinguish the receipt’s terms from the field observation. He may find that the receipt names a delivery point, not a route. This makes the old document more useful by narrowing what it proves. If the player asks him to identify a person from the handwriting, he can refuse to speculate. A refusal does not end the expedition.

If Rade stays behind, the player can bring back a sketch for a later comparison. That path costs an extra conversation and keeps Rade’s receipt in his custody. If the player skips the comparison, the expedition can still produce a valid route report; the relation simply receives no new evidence from this trip.

**Callback:** “It matches the delivery point. It never said which side of the post you took.” This line appears only if the comparison actually occurred.

## 409. Osha’s goal — make the return leg legible

Osha agrees to travel if the plan includes a return point she can identify. Her interest is not merely finding the outward route; she wants a way to know when the group should stop waiting and turn back. The player can help set a visible return marker, ask her to choose it, accept a shorter route with fewer observations, or invite a second traveler who can track the return.

If Osha chooses the marker, it belongs to her route plan and should not be presented as a universal standard. She can place a temporary, removable sign if the item and route owners permit it, or simply choose a recognizable landmark. If the player overrules her and continues past the agreed point, she can ask to turn back, withdraw from the route, or continue only if she actively accepts the revised scope.

If Osha declines the trip, the player may travel with another accepted person or use a different method. No later scene says she betrayed the plan. If she travels and turns back at the agreed point, the report can still be complete for the segment reached, with the rest explicitly unchecked.

**Callback:** “I knew where the return started. We didn’t have to argue about the road while we were already tired.” It appears only when the return marker was agreed and used.

## 410. Mina’s goal — understand the marker mount

Mina wants to know whether the marker’s bracket can be stabilized without replacing it. The question comes from her practical interest in repair, not a requirement that she mend everything. The player can take a close measurement, ask her to inspect the fasteners, leave the bracket untouched and draw its shape, or defer the repair question until a qualified work window.

If Mina inspects it, she may identify a loose fastening or a mount designed to rotate. She cannot infer who turned it from the wear alone. The player can ask her to correct the arrow, but she will first ask who uses the route and what the current direction is meant to indicate. The player can bring the question to the nearby contact or decide not to alter the marker.

If Mina is unavailable or declines the field trip, a Guild worker can assess the material if that service is accepted; otherwise the marker remains uninspected. The expedition can still return with route observations. The quest must not teleport Mina’s knowledge into the player’s journal because the character would have been a useful witness.

**Callback:** “The bracket turns. It was made to turn. That doesn’t tell us who changed it this week.” The line separates a repair fact from a historical inference.

## 411. The player’s own trip objective

The player can select one primary objective before departure: verify a route segment, inspect the marker, meet the nearby worker, or return before the next shift. This does not lock out incidental findings, but the objective helps characters ask for the right kind of report. The player can also mark “undecided” and choose after reaching the Sluice Steps.

If a companion’s goal conflicts with the primary objective, the player can renegotiate before leaving, split the trip if the current travel owner supports it, take turns at one location, or decline the optional request. A companion can disagree with the player’s plan and still travel if a bounded alternative is accepted. No one is treated as disloyal because they want the trip to mean something different.

At each stop, the player can continue toward the primary objective, make room for the companion’s accepted goal, ask whether they still wish to proceed, or turn back. This adds action-order variation without multiplying every branch into an exclusive ending.

## 412. Paired goals and field dialogue

Two companions can have compatible goals. Rade can compare the receipt while Mina inspects the mount. Their scene may find that the receipt names the post but provides no clue about the bracket’s orientation. The player can preserve that distinction, ask the nearby contact, or stop after answering the practical route question.

Osha’s return marker can support either companion’s work. If the group agrees to use it, the character doing the inspection can still set a limit on how long they remain. If the player asks everyone to wait while only one person works, each companion’s availability is checked separately. A shared trip does not automatically mean shared labor.

If two goals conflict—Osha wants to turn back while Mina is mid-inspection—the player can stop the inspection, ask Mina whether a brief finish is safe and accepted, change the return plan with everyone’s consent, or split up only if the travel system and current capabilities allow it. No split-party action should be written as available unless the live game supports it.

## 413. Partial companion participation

A companion may join only one leg, remain at the first site, hold an item, or leave before the final observation if those actions are supported by the travel owner. If the current system supports only all-or-nothing expedition members, author the dialogue as pre-trip acceptance and refusal rather than creating unsupported mid-route party controls.

Partial participation should still matter. A person who waits at the Sluice Steps can report only what they observed from that vantage. A person who inspects the marker can offer a repair fact without becoming a route witness. A person who turns back can tell the shelter where and when they left, not what happened after.

When a companion declines, the player can select another method, ask a different willing person, or reduce the expedition’s scope. The main quest remains completable. Where only one character can provide a particular optional perspective, omitting it leaves the interpretation open rather than blocking a core branch.

## 414. Relationship outcome without approval scoring

Companion reactions follow how the player handled the agreement. If the player respected a turn-back point, Osha may accept a future route while still requiring a clear return plan. If the player let Mina inspect the mount but did not follow her repair recommendation, she can disagree about the decision without treating the consultation as meaningless. If the player used Rade’s receipt outside its scope, he can ask for a correction even if the expedition otherwise succeeded.

These outcomes should not be merged into a follower approval value. One character may value a direct answer and another may value time to inspect. The player’s action record can support both responses. A relationship can remain warm while a character declines a task; it can also remain strained after an apology if the same boundary is ignored again.

## 415. Companion goal closure variants

- **Goal completed:** the companion got the agreed observation or comparison, and its limits are recorded.
- **Goal narrowed:** the player and companion chose a smaller version after new evidence or a time limit.
- **Goal deferred:** the trip established what would be needed for another attempt; no one promises to make it.
- **Goal declined:** the companion or player decided the optional task did not fit this trip.
- **Goal interrupted:** the team turned back or lost access before completing it; the result remains partial.

All five are complete narrative states. The last three are not failures that require a compensation reward. A later scene may reopen the question when people, time, and the route make it useful.

## 416. Installment 38 closing note

This installment adds companion objectives, pre-trip negotiation, mid-route reconsideration, partial participation, compatible and conflicting goals, and five clear closure states. It broadens exploration play through relationships and choices while preserving voluntary participation and existing travel-system boundaries.

## Plan 1 formal closeout — Expansion 97: “A Shift Is Not a Flag”

### Closeout status

Expansion 97 is complete as a **design plan** and exceeds the user’s minimum length of 120,000 words. This closeout is part of the measured document; the final count is recorded in the header after this section is saved. The 120,000-word figure is a floor. Additional content beyond that floor is acceptable when it contributes distinct, usable design material.

This closeout marks completion of Plan 1 in the five-plan series. It does not mark any production implementation, data integration, test acceptance, or release package complete. The work delivered in this file is creative and architectural planning grounded in the repository evidence snapshot identified near the top of the document.

### What the plan now contains

The plan develops the central shift-board conflict into a campaign with an eight-act spine, action-driven branches, practical failure and recovery paths, multiple player approaches, and endings that preserve unresolved costs. The player’s method can vary through actions such as verifying a source, making a copy, negotiating scope, accepting a task, turning back, building capacity, delaying a service, or correcting a previous mistake. Branch availability is not reduced to a good/evil or honesty scale.

Military, Rebel, Independent, and no-commitment routes retain separate political pressures and service relationships. Supporting roles stay bounded: existing currents such as Archivists, Long Walk, and Scavenger Guild provide specific services; provisional local work networks add task-level perspectives without replacing major factions or creating a parallel campaign authority.

The file now contains detailed resident arcs for Dema, Iven, Sera, and Pell; field quests for supporting groups; the optional expedition “The Marker Past the Turn”; branch-aware debrief and report-sharing choices; action traces across several playthroughs; route-specific finale and epilogue scenes; and staged production and acceptance gates. Refusal, absence, partial evidence, late correction, and an unresolved question can each lead to a complete narrative outcome.

### Design acceptance summary

| User requirement | Plan 1 coverage |
|---|---|
| Expansive questlines | Eight-act central arc, resident arcs, supporting-current field quests, optional expedition, return-phase follow-up |
| Branching paths | Choices change access, labor, evidence, information scope, route use, service terms, and who accepts future work |
| Branching endings | Major-route epilogues with distinct continuity, custody, service, and information outcomes, including mixed results |
| Playstyle expansion | Surveying, repair, relay, mediation, negotiation, record analysis, access design, capacity building, direct operation, and hybrid methods |
| More flavor and depth | Cast histories and limits, network voices, recurring spaces and objects, ambient exchanges, dialogue scenes, and ordinary-shift epilogues |
| Supporting factions/groups | Bounded offers and refusals that contribute to the player’s story without becoming competing major powers |
| Beyond morality axes | Repeated, traceable actions and accepted agreements determine branches; values can conflict within one playthrough |

### Implementation boundary and remaining review

Before any production package starts, the implementer must revalidate current code, data, branch owners, quest identifiers, narrative IDs, save ownership, and consumers. Working network names and any provisional characters or locations require a collision check. Each selected branch must map to a current owner or wait for an explicit architecture decision. No prose in this plan grants authority to create a duplicate inventory, schedule, relationship, faction, route, or save system.

The implementation package should take one bounded slice at a time, claim exact paths, name acceptance criteria, and run focused verification under the active test policy. It must distinguish an authored choice from a completed action and a completed action from a persistent fact. If a current consumer cannot represent the result, mark the integration gap rather than hiding it in panel or dialogue-local state.

### Work performed and verification

- **Files changed for Plan 1:** `docs/expansions/wave20/expansion_97_a_shift_is_not_a_flag_plan.md`.
- **Production source or gameplay data changed:** none.
- **Governance ledgers changed:** none.
- **Tests run:** none; this was a documentation-only expansion and the user did not request test execution.
- **Word-count method:** `wc -w` on the saved Markdown file, including front matter, headings, tables, and this closeout.
- **Limit:** the file remains a design proposal pending current-source revalidation and normal ownership, integration, and acceptance steps.

### Series handoff

Plan 1 is closed at the requested minimum or above. The next document in the sequence is Plan 2, **Expansion 98 — “A Lesson Kept Between Shifts.”** Its existing scope defines exactly three education subfeatures. The series continuation will deepen those three through learner/teacher paths, replay-safe lessons, and knowledge that leaves the lesson; it will not add a fourth feature pillar. Plans 3–5 remain untouched until their turn.

**Plan 1 closeout decision:** complete as a design document; ready for premise revalidation and a separately owned implementation package. This status does not claim the game has implemented or verified the proposed content.
