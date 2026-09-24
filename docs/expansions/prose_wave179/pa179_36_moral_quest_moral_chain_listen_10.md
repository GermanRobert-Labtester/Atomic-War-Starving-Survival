# Subject Plan 179.36 — The Mediator's Calculus: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_10` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 86 words / 509 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact.

This is the Listen branch record **The Mediator's Calculus**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_10` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A mediator at the peacekeeper outpost is trying to divide a single water filter between two settlements. Both claim it first. Both are dying without it.

**Place:** `loc_neutral_ground` · **category:** `trust` · **day window:** 20–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (86 words / 509 characters):**

> The mediator sits between two envoys who won't look at each other. The water filter sits on the table — a battered but functional unit, the only one within fifty kilometers. The mediator has been talking for six hours. They look at you with exhausted eyes. 'I need one more perspective,' they say. 'Not to decide. To understand what I'm deciding between.' The two settlements: one has children. The other has the engineers who can maintain the filter long-term. Both claims are legitimate. Both are desperate.

**Existing choices, frozen for context:** 1. Listen to both sides fully before speaking.; 2. Propose a rotation schedule — the filter moves between settlements.; 3. "Give it to the settlement with children. That's simple."; 4. "This isn't my problem. You're the mediator."

Adjacent chain records: predecessor `quest_moral_chain_listen_09`; successor `quest_moral_chain_listen_11`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

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
      "epitaph": "Listened to two dying settlements. Found a third option no one saw because everyone was defending.",
      "label": "Listen to both sides fully before speaking.",
      "moral_delta": 14,
      "outcome_text": "You hear both cases. Settlement A: fourteen children under twelve, three adults with radiation sickness, no technical capacity. Settlement B: no children, but six engineers and a workshop. Losing the filter kills A slowly. Losing the filter kills B's ability to fix the next one. You listen. You ask questions. And then you say something neither side expected: 'What if the engineers go to Settlement A? The filter stays with the children, and the people who can maintain it go where it's needed.' Silence. Then, slowly, the impossible becomes discussable."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Proposed a compromise both sides hated. It lasted eleven days. That was eleven days of water.",
      "label": "Propose a rotation schedule — the filter moves between settlements.",
      "moral_delta": 9,
      "outcome_text": "The rotation plan is practical: two weeks at Settlement A, two weeks at B, with the engineers traveling to maintain it. Both envoys hate it. Both accept it. The mediator signs the agreement and looks at you. 'It won't last,' they say. 'But it lasts long enough for someone to find a second filter.' Sometimes the best solution is the one that buys time."
    },
    {
      "empathy_delta": 1,
      "epitaph": "Chose the children. The math was cruel. The engineers let the second filter die. Morality has a half-life.",
      "label": "\"Give it to the settlement with children. That's simple.\"",
      "moral_delta": 4,
      "outcome_text": "The mediator nods. Settlement B's envoy stands and walks out. The filter goes to the children. The mediator pulls you aside. 'You chose the moral answer. Now Settlement B's engineers will let their filter break because they have no incentive to maintain someone else's equipment. In three months, both settlements will be without water. But the children will have lasted longer.' You helped. You also created a problem you can't see from here."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Walked away from a mediation. The mediator didn't. Some distances are measured in conscience.",
      "label": "\"This isn't my problem. You're the mediator.\"",
      "moral_delta": -3,
      "outcome_text": "The mediator stares at you. Then they turn back to the envoys and resume the negotiation alone. The filter stays on the table. The conversation continues for another three hours. You leave. Behind you, two settlements are still dying, and the mediator is still the only one trying to stop it. You've preserved your neutrality. You've also demonstrated that neutrality has a cost, and other people pay it."
    }
  ],
  "display_name": "The Mediator's Calculus",
  "id": "quest_moral_chain_listen_10",
  "location_id": "loc_neutral_ground",
  "max_day": 0,
  "min_day": 20,
  "trigger": "A mediator at the peacekeeper outpost is trying to divide a single water filter between two settlements. Both claim it first. Both are dying without it."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_10.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
