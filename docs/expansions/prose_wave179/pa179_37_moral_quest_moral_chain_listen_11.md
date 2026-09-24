# Subject Plan 179.37 — The Defector's Confession: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_11` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 103 words / 586 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits.

This is the Listen branch record **The Defector's Confession**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_11` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A former raider at the raider camp wants to surrender intelligence about their old group. But they need someone to hear the full story first — to understand what they're leaving behind.

**Place:** `loc_recovery_yard` · **category:** `trust` · **day window:** 22–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (103 words / 586 characters):**

> The defector sits with their back to the wall, a packed bag at their feet. They've been with the raiders for two years. They want out — not because of guilt, but because they've seen what comes next. 'They're not just raiding anymore,' they say. 'They're building. A fort. A hierarchy. They're becoming the thing we survived.' They want to trade information: patrol routes, supply caches, the location of the new fort. But first, they need you to understand why they joined in the first place. 'Nobody starts as a raider,' they say. 'They start as someone who ran out of other options.'

**Existing choices, frozen for context:** 1. Listen to their full story before taking the intelligence.; 2. Take the intelligence and promise to advocate for their safety.; 3. "Just give me the information. We don't need the backstory."; 4. "You're a raider. Why should I believe anything you say?"

Adjacent chain records: predecessor `quest_moral_chain_listen_10`; successor `quest_moral_chain_listen_12`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "trust",
  "choices": [
    {
      "empathy_delta": 4,
      "epitaph": "A raider told me how they became one. The scariest part was how it made sense.",
      "label": "Listen to their full story before taking the intelligence.",
      "moral_delta": 14,
      "outcome_text": "The story takes two hours. A family destroyed. A settlement burned. A choice between starving alone or eating with people who take. The defector describes the slow erosion — how the first theft felt like survival, the tenth felt like routine, the hundredth felt like nothing. 'That's the part I'm running from,' they say. 'Not the raiding. The nothing.' You listen. You take the intelligence. And you understand, without agreeing, how a person becomes a raider. The defector leaves through the back. You hope they find something to replace the nothing."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Took a raider's map. Promised their safety. Promises are expensive currency now.",
      "label": "Take the intelligence and promise to advocate for their safety.",
      "moral_delta": 8,
      "outcome_text": "The defector hands over a hand-drawn map — patrol routes, cache locations, the fort's weak points. In exchange, you promise to make sure the peacekeepers know they've left the group. 'I don't need forgiveness,' they say. 'I just need to not be hunted.' You nod. The map is detailed, accurate, valuable. The defector disappears into the night. You've gained intelligence and made a promise. Whether it's kept depends on people you can't control."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Got the map. Missed the meaning. Intelligence without understanding is just a list of places.",
      "label": "\"Just give me the information. We don't need the backstory.\"",
      "moral_delta": -2,
      "outcome_text": "The defector's face hardens. They hand over the map — terse, functional, stripped of context. 'Patrol routes. Cache locations. That's what you want.' The intelligence is good. But you've missed the part that would have helped you understand not just where the raiders are, but why they do what they do. Information without context is just coordinates."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Refused to listen. A defector walked back into the raiders. Maybe they deserved it. Maybe not.",
      "label": "\"You're a raider. Why should I believe anything you say?\"",
      "moral_delta": -5,
      "outcome_text": "The defector stands. The bag stays at their feet. 'You shouldn't,' they say. 'That's why I needed someone to listen first. So they'd know whether the change was real.' They pick up the bag and walk out the other side. The intelligence is gone. The defector is gone. You've protected yourself from a possible lie and lost a possible truth. The raiders' fort is still being built. Someone else will find it. Someone else will listen."
    }
  ],
  "display_name": "The Defector's Confession",
  "id": "quest_moral_chain_listen_11",
  "location_id": "loc_recovery_yard",
  "max_day": 0,
  "min_day": 22,
  "trigger": "A former raider at the raider camp wants to surrender intelligence about their old group. But they need someone to hear the full story first — to understand what they're leaving behind."
}
```

Do not add or remove records or properties. Do not alter `id`, `display_name`, `category`, `trigger`, `location_id`, `min_day`, `max_day`, choice count/order/labels, outcome text, epitaph, flag, moral or empathy delta, or any nested field. Do not introduce another state variant or a parallel narrative authority. The active integration ledger and current ownership ledger remain controlling for any later implementation package.

## Route, ownership, and save boundary

The data-first route is a one-field edit to the existing JSON entry, after rechecking its current schema and path ownership. The proven route is: `MoralChoiceBranchQuestCatalogLoader.Load` → `Main.SetupMoralChoice` definition registration → existing availability/chain-access checks in `GetAvailableMoralChoices` → `MoralChoiceModal` display of trigger and discovery text. Exact display is still conditional on the existing day, chain, and resolved-state gates; this proposal does not change eligibility or guarantee that the record is offered in every campaign.

A top-level prose-only change has no save, RNG, or state impact. If it appears to require new host behavior, code, UI, flags, save data, mechanics, or a changed choice contract, stop and create a separately scoped, current-evidence integration plan. The subject plan authorizes no such work.

## Canon, continuity, and duplicate checks

1. Compare the record and both adjacent chain entries named above; verify the trigger-to-discovery handoff, reveal order, time, and unresolved questions.
2. Review the relevant current lore and moral-choice canon. Use Volumes 56–57 of `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` as discovery context, while treating shipped JSON and current code as implementation truth.
3. Search the existing moral-choice, event, echo, and encounter catalogs for duplicate scene beats and recycled phrasing. ID novelty alone is not content novelty.
4. Confirm each person, faction, place, resource quantity, and fact in the prose against existing authority. Do not add named entities or canon events by inference.
5. Keep the scene fictional, restrained, and playable. Avoid real-world references, borrowed phrasing, melodrama, moral instruction, and decorative suffering.

## Focused verification for a future edit

- Strict-parse the complete source catalog and confirm the schema version and 100-record count remain valid.
- Compare before/after JSON, proving only `quest_moral_chain_listen_11.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
