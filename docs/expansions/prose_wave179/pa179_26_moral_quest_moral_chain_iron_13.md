# Subject Plan 179.26 — The Old Friend: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_iron_13` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 136 words / 739 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the evidence, resource calculation, and authority pressure concrete. Do not make severity synonymous with competence or turn a hard policy into a promised result.

This is the Iron branch record **The Old Friend**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_iron_13` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** Someone from before the exchange finds you. They need help. Helping them would cost your group.

**Place:** `loc_shelter_gate` · **category:** `comfort` · **day window:** 27–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (136 words / 739 characters):**

> His name is Mako. You knew him before — not close, but well enough that you shared meals and talked about the kind of world you'd build after the crisis passed. He looks like the crisis passed him first. He's lost weight, his hands are scarred, and he has a cough that sounds like it's been there for months. He found out you were leading a settlement. He walked two days to get here. He's asking for shelter, medical attention, and a place to stay. He doesn't have anything to trade. He says he doesn't have anyone else. Your medical supplies are limited. Your shelter is full. Your group has a policy about resource allocation that you helped write. Mako is looking at you with the expression of someone who remembers who you used to be.

**Existing choices, frozen for context:** 1. Take him in. Full support. The policy can bend for someone you knew before.; 2. Offer limited help — medical treatment and three days of food. Then he's on his own.; 3. Turn him away. The group comes first. Always.

Adjacent chain records: predecessor `quest_moral_chain_iron_12`; successor `quest_moral_chain_iron_14`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "epitaph": "Mako walked two days. I said yes. The policy bent. Someone asked if it applies to everyone. I didn't answer. Mako earned his place. The question didn't.",
      "label": "Take him in. Full support. The policy can bend for someone you knew before.",
      "moral_delta": 5,
      "outcome_text": "You assign Mako a bunk, get him to the medical station, and make sure he eats a full meal that night. The cough is bad but treatable. Over the next two weeks, he recovers enough to work — light duty at first, then regular shifts. He's a good worker. He doesn't talk about the past much. He doesn't thank you, either, which you appreciate more than if he had. Your group notices the exception. Some people understand. Some don't. One person asks if the policy applies to everyone or just people you know. You don't have a good answer. Mako stays. He earns his place. The precedent is messy but human."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Three days. A treated cough. Food for the road. He didn't argue. That's what I remember — not the yes or the no, but that he didn't argue.",
      "label": "Offer limited help — medical treatment and three days of food. Then he's on his own.",
      "moral_delta": -4,
      "outcome_text": "You treat the cough. You give him three days of rations and a place to sleep in the common area. Mako understands the limits. He doesn't argue. On the fourth morning, he packs the food you gave him and walks out. You watch him go through the gate. He doesn't look back. You don't know where he goes. Your group doesn't say anything about it, but the mood is heavier for a day. You helped someone. You also turned someone away. Both things are true. You live with both of them."
    },
    {
      "empathy_delta": 0,
      "epitaph": "I said no to someone I knew before. He walked away. The cough got quieter. The food tasted like nothing. I still don't talk about it.",
      "label": "Turn him away. The group comes first. Always.",
      "moral_delta": -9,
      "outcome_text": "You tell him the shelter is full. You tell him the policy doesn't allow exceptions. You tell him you're sorry. Mako nods. He expected this, maybe. He turns and walks back the way he came. You watch him until he's out of sight. The cough gets quieter as he gets farther away. Your group doesn't know what happened — you handled it at the gate, privately. You eat dinner that night. The food tastes like nothing. You sleep. You dream about someone walking down a road with a cough and three days of food. You don't tell anyone about the dream."
    }
  ],
  "display_name": "The Old Friend",
  "id": "quest_moral_chain_iron_13",
  "location_id": "loc_shelter_gate",
  "max_day": 0,
  "min_day": 27,
  "trigger": "Someone from before the exchange finds you. They need help. Helping them would cost your group."
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
- Compare before/after JSON, proving only `quest_moral_chain_iron_13.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
