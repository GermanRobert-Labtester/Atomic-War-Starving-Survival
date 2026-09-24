# Subject Plan 179.16 — The Price of Empty Hands: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_03` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 79 words / 467 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch.

This is the Iron branch record **The Price of Empty Hands**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_03` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A family of four arrives at the shelter gate. They have nothing. They're asking for work.

**Place:** `loc_shelter_gate` · **category:** `share` · **day window:** 5–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (79 words / 467 characters):**

> The father speaks. The mother doesn't look up — she's counting the children with her eyes, the way parents do when they're making sure the number hasn't changed. They've walked three days from a collapsed settlement to the south. They have no tools, no trade goods, no skills beyond farming and the father's half-trained knowledge of basic carpentry. The shelter has work. The shelter also has forty mouths already and a grain bin that shows the bottom some mornings.

**Existing choices, frozen for context:** 1. Take them in. Full rations. They'll earn their keep.; 2. Offer work — but half rations until they've proven themselves.; 3. Tell them you can't help. Point them toward the eastern road.; 4. Take only the father. He has useful skills. The rest can wait outside.

Adjacent chain records: predecessor `quest_moral_chain_iron_02`; successor `quest_moral_chain_iron_04`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "share",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Four more mouths. Six more hands. The math worked this time. It won't always.",
      "label": "Take them in. Full rations. They'll earn their keep.",
      "moral_delta": 7,
      "outcome_text": "You assign the father to repairs, the mother to the kitchen. The children — eight and eleven — carry water. Within a week, the father has reinforced the east wall with scrap lumber. The mother stretches every ration by a tenth. The grain bin still shows the bottom, but now there are hands to scrape it cleaner. You eat the same amount. Everyone else eats slightly more. Nobody mentions the arithmetic."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Two weeks of half rations. The children didn't complain. Parents notice that kind of thing.",
      "label": "Offer work — but half rations until they've proven themselves.",
      "moral_delta": -4,
      "outcome_text": "The father's jaw tightens but he nods. The mother finally looks up. She asks if the children get full rations. You say yes. She nods too. They work. The father builds. The mother cooks. After two weeks, you raise them to full rations. The children have gotten thinner in the interim. The mother notices everything. She says nothing about it. That's worse than if she'd shouted."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I pointed east. I don't know what's east. I chose not to know.",
      "label": "Tell them you can't help. Point them toward the eastern road.",
      "moral_delta": -8,
      "outcome_text": "You close the gate. Through the gap, you watch the father pick up the youngest and the mother take the oldest's hand. They walk east. The mother doesn't look back. The father does, once, at the bend. Three days later, you hear that the eastern road leads to a settlement that's been raiding caravans. You don't know if that's the family you turned away. You don't go looking to find out."
    },
    {
      "empathy_delta": 0,
      "epitaph": "A carpenter without a family builds faster. He also builds nothing worth coming home to.",
      "label": "Take only the father. He has useful skills. The rest can wait outside.",
      "moral_delta": -11,
      "outcome_text": "The father looks at you for a long time. Then he looks at his wife. She gives the smallest nod — the kind that means go, not the kind that means it's alright. He comes inside. You hear the children asking questions as the gate closes. The father builds you a proper workshop in three days. He doesn't speak unless spoken to. On the fifth morning, he asks if his family can come in. You say you'll consider it. He doesn't ask again."
    }
  ],
  "display_name": "The Price of Empty Hands",
  "id": "quest_moral_chain_iron_03",
  "location_id": "loc_shelter_gate",
  "max_day": 0,
  "min_day": 5,
  "trigger": "A family of four arrives at the shelter gate. They have nothing. They're asking for work."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_03.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
