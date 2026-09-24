# Subject Plan 179.23 — The Dissenter: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_10` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 112 words / 605 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Dissenter**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_10` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A respected member of your group is questioning your leadership — openly, loudly, and with supporters.

**Place:** `loc_shelter_meeting` · **category:** `listen` · **day window:** 20–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (112 words / 605 characters):**

> Her name is Petra. She was a teacher before the exchange. She still has the voice — the one that carries to the back of a room without shouting. She's been saying things at meetings: that your decisions are getting harder to justify, that the group's morale is dropping, that you've become more interested in control than survival. She's not wrong about all of it. She has six or seven people who nod when she speaks. Not a faction yet. But the architecture of one. She's talking to you now, after a meeting, in front of the others. She's giving you a chance to respond. Her tone is calm. Her eyes aren't.

**Existing choices, frozen for context:** 1. Challenge her to a formal leadership vote. Let the group decide.; 2. Acknowledge her concerns publicly. Adjust course. Bring her into the decision-making.; 3. Undermine her quietly. Question her loyalty. Let her supporters drift away.

Adjacent chain records: predecessor `quest_moral_chain_iron_09`; successor `quest_moral_chain_iron_11`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "listen",
  "choices": [
    {
      "empathy_delta": 0,
      "epitaph": "Fourteen to six. I won. I also learned exactly who wasn't with me. The vote was the point. The result was just the excuse.",
      "label": "Challenge her to a formal leadership vote. Let the group decide.",
      "moral_delta": -3,
      "outcome_text": "You call the vote that evening. Petra is surprised — she expected resistance, not a referendum. The results are clear: fourteen for you, six for Petra, two abstentions. You win. Petra accepts it gracefully. Her supporters accept it less gracefully. Over the following weeks, Petra stops speaking at meetings. Two of her supporters ask to be reassigned to exterior work — anything that keeps them out of the main shelter. You've won the vote. You've also mapped exactly who wasn't with you. The map is more valuable than the victory."
    },
    {
      "empathy_delta": 2,
      "epitaph": "I gave up absolute authority and got better decisions. Petra gave up opposition and got a seat at the table. We both lost something. We both gained more.",
      "label": "Acknowledge her concerns publicly. Adjust course. Bring her into the decision-making.",
      "moral_delta": 7,
      "outcome_text": "You tell the group Petra has a point. You name specific decisions that could have been handled better — the Lowfield arrangement, the information security protocols, the ration adjustments. You propose a council: three elected members who review major decisions before implementation. Petra's expression shifts from adversarial to cautious to something like respect. She agrees to serve on the council. The first decision she reviews is one she would have voted against — and she votes for it, with amendments. It's better than your original plan. The group notices. Your authority is slightly diminished. The group's decisions are slightly improved. You learn to live with the trade."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I didn't fight her. I asked questions. Seven supporters became four, then two, then none. Petra is still here. She's just quiet now. I made her quiet.",
      "label": "Undermine her quietly. Question her loyalty. Let her supporters drift away.",
      "moral_delta": -12,
      "outcome_text": "You don't confront her. You don't need to. Over the next two weeks, you have private conversations with each of her supporters. You don't lie — you just ask questions. 'Have you noticed Petra hasn't volunteered for exterior duty?' 'Did you know Petra was considering leaving last month?' 'Do you think Petra has the group's interests first, or her own reputation?' By the time Petra speaks at the next meeting, three of her seven supporters are looking at the floor. She makes her points. Fewer people nod. She feels the shift but can't identify its source. Within a month, she stops speaking at meetings entirely. She's still in the group. She's just... quiet. You've neutralized a threat without a single public confrontation. The method was clean. The morality isn't."
    }
  ],
  "display_name": "The Dissenter",
  "id": "quest_moral_chain_iron_10",
  "location_id": "loc_shelter_meeting",
  "max_day": 0,
  "min_day": 20,
  "trigger": "A respected member of your group is questioning your leadership — openly, loudly, and with supporters."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_10.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
