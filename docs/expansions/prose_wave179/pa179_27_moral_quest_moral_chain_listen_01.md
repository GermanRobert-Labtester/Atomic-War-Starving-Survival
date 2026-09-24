# Subject Plan 179.27 — The Weight of a Name: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_01` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 53 words / 306 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact.

This is the Listen branch record **The Weight of a Name**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_01` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A stranger sits near the shelter gate, turning a dog tag over in their fingers. They haven't spoken to anyone in three days.

**Place:** `loc_shelter_gate` · **category:** `listen` · **day window:** 1–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (53 words / 306 characters):**

> The stranger doesn't look up when you approach. Their thumb traces the engraved letters on a dog tag — the same letters, over and over, wearing the metal smooth. A canteen sits beside them, half-full. They push it toward you without a word, then stop mid-motion, as if remembering they're allowed to speak.

**Existing choices, frozen for context:** 1. Sit down. Say nothing. Let the silence do the work.; 2. Ask who the tag belonged to.; 3. Offer them food in exchange for the story.; 4. You don't have time for this. Walk on.

Adjacent chain records: predecessor `none (chain opening)`; successor `quest_moral_chain_listen_02`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "listen",
  "choices": [
    {
      "empathy_delta": 3,
      "epitaph": "I sat with a stranger until the silence broke open. Their sister's name tasted like copper and rain.",
      "label": "Sit down. Say nothing. Let the silence do the work.",
      "moral_delta": 12,
      "outcome_text": "You lower yourself onto the concrete beside them. Minutes pass. The wind carries grit across the gate. Finally, they speak — not to you, exactly, but to the air between you. A name. Their sister's. The tag belonged to someone who didn't come back from the eastern road. They tell you about the last letter, the one that arrived two weeks after the burial. You listen. That's all. But when they stand, they leave the tag on the ground between you. Not a gift. A witness mark."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Asked a question. Got a whole life in return. Didn't know that was possible anymore.",
      "label": "Ask who the tag belonged to.",
      "moral_delta": 7,
      "outcome_text": "They flinch at the question, then loosen. A name. A sister. The eastern road. The story comes out in fragments — a supply run gone wrong, a bridge that wasn't there, a radio that stopped answering. You piece it together from the gaps. When they finish, they look at you for the first time. 'You're the first one who asked,' they say. Not grateful. Just stating a fact."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Tried to buy a story. Got nothing. Some doors only open from the inside.",
      "label": "Offer them food in exchange for the story.",
      "moral_delta": -2,
      "outcome_text": "They look at the food, then at you. Something closes behind their eyes. They take the food — of course they take the food — but the story stays locked. 'Some things aren't for trade,' they say, and turn away. You've learned nothing except that hunger and grief don't share the same currency."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Walked past. The sound of that tag rubbing metal followed me for three days.",
      "label": "You don't have time for this. Walk on.",
      "moral_delta": -4,
      "outcome_text": "You pass them without a word. Their thumb keeps tracing the tag. You hear nothing — no plea, no curse, just the soft metal-on-skin sound fading behind you. The gate closes. Somewhere inside the shelter, someone else is carrying a story you'll never hear."
    }
  ],
  "display_name": "The Weight of a Name",
  "id": "quest_moral_chain_listen_01",
  "location_id": "loc_shelter_gate",
  "max_day": 0,
  "min_day": 1,
  "trigger": "A stranger sits near the shelter gate, turning a dog tag over in their fingers. They haven't spoken to anyone in three days."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_01.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
