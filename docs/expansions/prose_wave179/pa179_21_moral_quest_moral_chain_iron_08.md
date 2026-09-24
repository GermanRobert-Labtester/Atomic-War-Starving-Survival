# Subject Plan 179.21 — The Protection Racket: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_08` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 106 words / 646 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Protection Racket**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_08` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A small settlement asks you to protect them from raiders — for a price.

**Place:** `collapsed_building` · **category:** `share` · **day window:** 16–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (106 words / 646 characters):**

> The settlement is called Lowfield. Fourteen people, mostly farmers who've turned a collapsed gas station into something resembling a home. They have crops — actual growing crops — a rarity in the contaminated soil of the valley. They also have a problem: a raider group has been hitting them every ten days or so, taking what they want, leaving bruises and threats. Lowfield's leader, a woman named Tess, sits across from you with dirt under her fingernails and exhaustion in her voice. She's offering forty percent of their next harvest in exchange for protection. Permanent protection. She says the word 'permanent' like it costs her something.

**Existing choices, frozen for context:** 1. Accept. Forty percent is steep, but the crops are valuable. Protect them completely.; 2. Accept — but negotiate down to twenty-five percent. They need to stay viable.; 3. Decline. Their problem isn't your problem. You're not running a charity.; 4. Offer to help eliminate the raiders — then absorb Lowfield into your group entirely.

Adjacent chain records: predecessor `quest_moral_chain_iron_07`; successor `quest_moral_chain_iron_09`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "epitaph": "Forty percent. Every harvest. They grow the food. I grow the guards. Nobody calls it a racket. We all know what it is.",
      "label": "Accept. Forty percent is steep, but the crops are valuable. Protect them completely.",
      "moral_delta": -4,
      "outcome_text": "You station three people at Lowfield permanently. The raiders come on schedule and find armed guards who know the terrain. The confrontation is brief — two raiders down, the rest running. Lowfield's crops are safe. Your people eat better for the next three months. Tess pays the forty percent without complaint the first time. The second time, she asks if there's been any discussion about reducing the rate. You explain that the threat hasn't decreased. She doesn't ask again. Lowfield grows. Your share grows with it. The farmers work harder. Some of them stop making eye contact with your guards."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Twenty-five percent and a jar of tomatoes. The tomatoes weren't part of the deal. That's what made them worth more.",
      "label": "Accept — but negotiate down to twenty-five percent. They need to stay viable.",
      "moral_delta": 3,
      "outcome_text": "Tess blinks when you say twenty-five. She was clearly prepared to agree to forty. You explain that a starving farm doesn't produce much of anything. She agrees quickly — too quickly, as if she's afraid you'll change your mind. Your guards protect Lowfield. The raiders don't come back after the second attempt. The harvest comes in at twenty-five percent, which is still a lot of grain. Tess's people eat. Your people eat. The arrangement holds. After six months, Tess brings you a jar of preserved tomatoes as a 'bonus.' They're the first tomatoes you've seen in a year. They taste like something from before."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Fourteen people. One gas station. A field of crops I'll never eat. I said it wasn't my problem. It became three more mouths. The math changed.",
      "label": "Decline. Their problem isn't your problem. You're not running a charity.",
      "moral_delta": -6,
      "outcome_text": "Tess leaves without arguing. She expected this, maybe. Three weeks later, the raiders hit Lowfield hard. They take the standing crop and burn what they can't carry. Your scouts report smoke for two days. You don't send anyone. Lowfield's survivors scatter — some head east, some south, a few show up at your perimeter looking hollow. You take in three of them. They work. They don't talk about Lowfield. Neither do you."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I didn't conquer Lowfield. I invited it in. Tess said yes. She always had a choice. It just wasn't a good one.",
      "label": "Offer to help eliminate the raiders — then absorb Lowfield into your group entirely.",
      "moral_delta": -11,
      "outcome_text": "You present it as partnership. Your people handle the raider threat. Lowfield joins your settlement — shared resources, shared labor, shared protection. Tess listens. She's smart enough to see what's happening. She asks what happens if she says no. You say you'll understand completely. She says yes. The raiders are eliminated within a week — your people are motivated. Lowfield's crops become your crops. Lowfield's people become your people. They have full rations, real security, and no autonomy. Tess farms. She doesn't lead anymore. Some nights, she sits on the gas station roof and looks south. You don't know what she's looking at. You don't ask."
    }
  ],
  "display_name": "The Protection Racket",
  "id": "quest_moral_chain_iron_08",
  "location_id": "collapsed_building",
  "max_day": 0,
  "min_day": 16,
  "trigger": "A small settlement asks you to protect them from raiders — for a price."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_08.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
