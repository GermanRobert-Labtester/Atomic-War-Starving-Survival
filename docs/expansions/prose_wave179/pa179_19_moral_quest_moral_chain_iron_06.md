# Subject Plan 179.19 — The Merchant's Proposition: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_06` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 99 words / 625 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Merchant's Proposition**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_06` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A traveling merchant offers you an exclusive trade deal — if you cut off a competing settlement.

**Place:** `loc_water_station` · **category:** `share` · **day window:** 12–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (99 words / 625 characters):**

> The merchant's name is Voss. He trades out of a reinforced cart pulled by two mules that look better fed than most people in the valley. He has tools, medicine, ammunition, and a smile that suggests he's already calculated your margin. The deal is simple: exclusive trading rights through your territory, favorable prices, priority access to new stock. The condition is equally simple — you stop the Aldric settlement from accessing the eastern trade road. 'They're undercutting me,' Voss says. 'I'd rather do business with someone who understands monopoly.' He doesn't say the word monopoly. He says 'exclusive partnership.'

**Existing choices, frozen for context:** 1. Accept. Block the Aldric settlement's access. Take the exclusive deal.; 2. Counter-propose: you'll take the deal, but Aldric keeps access. You mediate.; 3. Refuse. You don't strangle neighbors for a discount.

Adjacent chain records: predecessor `quest_moral_chain_iron_05`; successor `quest_moral_chain_iron_07`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "share",
  "choices": [
    {
      "empathy_delta": 0,
      "epitaph": "Fifteen percent cheaper tools. Two extra days for Aldric. The math was simple. The morality wasn't. I chose the math.",
      "label": "Accept. Block the Aldric settlement's access. Take the exclusive deal.",
      "moral_delta": -10,
      "outcome_text": "You station people at the two access points along the eastern road. Aldric's traders arrive, find the road 'closed for safety repairs,' and are directed to a longer alternative that adds two days to their journey. Voss's carts pass through without delay. His prices to you drop fifteen percent. Aldric's people start paying more for everything within a month. Their leader sends a messenger asking what's happened. You send back a polite note about 'temporary security measures.' The messenger doesn't mention your name in his report. He doesn't have to."
    },
    {
      "empathy_delta": 1,
      "epitaph": "I didn't build a wall. I built a bridge and charged tolls on both sides. Everyone called it fair. Maybe it was.",
      "label": "Counter-propose: you'll take the deal, but Aldric keeps access. You mediate.",
      "moral_delta": 2,
      "outcome_text": "Voss doesn't like it. You can see the calculation behind his eyes — he wanted a weapon, not a diplomat. But he agrees to a trial period. You arrange a meeting between Voss and Aldric. The negotiation takes six hours. Aldric agrees to buy certain categories exclusively through Voss in exchange for reduced prices on staples. Voss gets market control. Aldric gets cheaper goods. You get a cut of every transaction as 'facilitation fee.' Everyone benefits. Voss respects you more. Aldric trusts you more. Neither of them realizes you've positioned yourself as the only person both sides need."
    },
    {
      "empathy_delta": 1,
      "epitaph": "I said no to a monopoly. Voss said no to me. Aldric got the medicine. I got the lesson.",
      "label": "Refuse. You don't strangle neighbors for a discount.",
      "moral_delta": 6,
      "outcome_text": "Voss's smile doesn't change, but something behind it does. He says he understands. He says there are other partners in the valley. He moves his cart through your territory without buying or selling anything. Three days later, you hear he's set up a trading post near Aldric's settlement. Your people start making the longer journey to reach him because his prices are better than anything you can offer. You refused to hurt Aldric. Aldric is now better supplied than you. The moral choice has a moral cost. You pay it."
    }
  ],
  "display_name": "The Merchant's Proposition",
  "id": "quest_moral_chain_iron_06",
  "location_id": "loc_water_station",
  "max_day": 0,
  "min_day": 12,
  "trigger": "A traveling merchant offers you an exclusive trade deal — if you cut off a competing settlement."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_06.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
