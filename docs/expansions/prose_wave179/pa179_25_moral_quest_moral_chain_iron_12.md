# Subject Plan 179.25 — The Lieutenant's Doubt: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_12` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 137 words / 805 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Lieutenant's Doubt**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_12` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** Your second-in-command questions a decision that hurt someone. They're having second thoughts.

**Place:** `loc_shelter_perimeter` · **category:** `comfort` · **day window:** 25–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (137 words / 805 characters):**

> Reeves has been with you since the beginning. He helped you secure the shelter, organize the first rationing system, and make the hard calls that kept people alive. He's the person you trust most. Which is why it matters when he sits across from you, hands wrapped around a cup of water he's not drinking, and says: 'The Lowfield arrangement — Tess's people, they're not thriving. They're surviving for us. There's a difference.' He's not challenging your authority. He's questioning your methods. He says he's been thinking about the Harlan situation too, and the way you handled Petra. He's not accusing you of cruelty. He's asking if you've noticed the cost. He looks tired. Not physically — tired in the way that people get when they've been carrying something heavy for too long and finally admit it.

**Existing choices, frozen for context:** 1. Listen. Really listen. Acknowledge that he might be right.; 2. Explain your reasoning. Help him see the larger picture.; 3. Dismiss his concerns. He's getting soft. The group needs strength, not doubt.

Adjacent chain records: predecessor `quest_moral_chain_iron_11`; successor `quest_moral_chain_iron_13`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "comfort",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Reeves talked. I listened. He was right about most of it. I admitted it. The group got better decisions. I got a harder mirror.",
      "label": "Listen. Really listen. Acknowledge that he might be right.",
      "moral_delta": 6,
      "outcome_text": "You don't defend. You don't explain. You listen. Reeves talks for twenty minutes about specific people, specific moments, specific costs that you didn't see because you were looking at the bigger picture. He's right about most of it. You admit that. He looks relieved — not because you agreed, but because you heard him. You agree to review the Lowfield arrangement and the medical allocation. Reeves stays. He's still your second-in-command. But something has shifted — he speaks more freely now, and you listen more carefully. The group's decisions improve incrementally. The cost is that you have to admit you were wrong sometimes. That turns out to be survivable."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I explained the logic. He understood it. He still doesn't love the outcomes. Neither do I. But now he carries it with me instead of alone.",
      "label": "Explain your reasoning. Help him see the larger picture.",
      "moral_delta": 0,
      "outcome_text": "You walk him through the logic: the Lowfield arrangement secured food for sixty people. Harlan's theft, if unchecked, would have set a precedent. Petra's opposition, if empowered, would have fractured the group at a vulnerable moment. Every decision had costs. Every decision also had benefits that outweighed them. Reeves listens. He nods at some points. He pushes back on others. By the end, he says he understands the reasoning even if he doesn't love the outcomes. You agree to be more transparent about the trade-offs. Reeves stays. He's still your second-in-command. But he's also still carrying the weight. You've lightened it slightly by sharing the burden of explanation. It's not the same as sharing the burden of doubt."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I told him to get hard or get out. He stayed. He also stopped telling me when I was wrong. I didn't notice the difference at first. Everyone does eventually.",
      "label": "Dismiss his concerns. He's getting soft. The group needs strength, not doubt.",
      "moral_delta": -11,
      "outcome_text": "You tell Reeves that survival isn't a popularity contest. You say the words: 'If you can't make the hard calls, say so now and I'll find someone who can.' Reeves goes quiet. Something in his face closes — not anger, not sadness, but the expression of a man who's decided not to bother anymore. He nods. He stays. He does his job. But he stops bringing you information he thinks you don't want to hear. He stops questioning decisions. He stops being the person who tells you when you're wrong. You've kept your second-in-command. You've lost your conscience. The group doesn't notice immediately. You will."
    }
  ],
  "display_name": "The Lieutenant's Doubt",
  "id": "quest_moral_chain_iron_12",
  "location_id": "loc_shelter_perimeter",
  "max_day": 0,
  "min_day": 25,
  "trigger": "Your second-in-command questions a decision that hurt someone. They're having second thoughts."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_12.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
