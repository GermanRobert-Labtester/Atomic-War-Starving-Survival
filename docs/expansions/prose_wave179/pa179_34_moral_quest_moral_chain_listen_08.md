# Subject Plan 179.34 — What the Soldier Carried Home: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_08` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 86 words / 487 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits.

This is the Listen branch record **What the Soldier Carried Home**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_08` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A former soldier at the military outpost has started talking in their sleep. The other refugees are frightened. You're asked to intervene.

**Place:** `loc_garrison_checkpoint_gamma` · **category:** `listen` · **day window:** 16–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (86 words / 487 characters):**

> The soldier sleeps on a cot in the corner of the outpost's barracks. Even from across the room, you can hear the fragments — place names, numbers, a repeated phrase in a language you don't recognize. The other refugees have backed away. The soldier's hands move in their sleep: checking a magazine, cradling something that isn't there, digging. When you sit beside them, their eyes open — not awake, not asleep — and they say, clearly, 'The bridge is gone. Tell them the bridge is gone.'

**Existing choices, frozen for context:** 1. Stay with them. Wake them gently. Ask what they need to tell.; 2. Write down what they say in their sleep. Give it to them in the morning.; 3. Move them to a separate room so others can sleep.; 4. "They need to get over it. We all do."

Adjacent chain records: predecessor `quest_moral_chain_listen_07`; successor `quest_moral_chain_listen_09`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "listen",
  "choices": [
    {
      "empathy_delta": 4,
      "epitaph": "A soldier's war followed them home. The bridge was gone. The dead couldn't hear. I could.",
      "label": "Stay with them. Wake them gently. Ask what they need to tell.",
      "moral_delta": 14,
      "outcome_text": "They wake shaking. The story comes in pieces over the next hour. They were part of a retreat — the last column out of the eastern corridor. They carried a message: the bridge at sector seven was compromised, and anyone using it would fall into irradiated water. The message was delivered. But the column that received it didn't make it. Forty soldiers walked onto a bridge that the soldier's unit knew was gone. 'I carry it home every night,' they say. 'The bridge is gone. I keep telling people. But the people I need to tell are already dead.'"
    },
    {
      "empathy_delta": 3,
      "epitaph": "Wrote down a soldier's nightmares. Gave them back in the morning. Lighter, they said. Both of us.",
      "label": "Write down what they say in their sleep. Give it to them in the morning.",
      "moral_delta": 11,
      "outcome_text": "You sit with paper and pencil through the night. The soldier talks for three hours. You write everything — names, coordinates, the repeated phrase (it's a unit motto: 'No one walks alone'). In the morning, you hand them the pages. They read them in silence. Then they fold the papers carefully and put them in their pocket. 'Now it's not just in my head,' they say. 'Thank you for taking it out.'"
    },
    {
      "empathy_delta": 0,
      "epitaph": "Solved the noise problem. Lost the story. Some practical solutions have impractical costs.",
      "label": "Move them to a separate room so others can sleep.",
      "moral_delta": 0,
      "outcome_text": "You carry the soldier's things to a storage room. They sleep more peacefully there — alone. The other refugees relax. But the soldier's story stays locked in their sleeping mind, unspoken, unrecorded. You've solved a practical problem. You've also ensured that whatever the bridge was, whatever message it carried, dies with the soldier. Eventually."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Said a soldier should get over it. Easier said. The bridge was still gone in their dreams.",
      "label": "\"They need to get over it. We all do.\"",
      "moral_delta": -5,
      "outcome_text": "The refugee who asked you to intervene nods. The soldier sleeps on, carrying their bridge, their dead, their unspoken message. You've sided with the living against the haunted. It's understandable. It's also the kind of thing that makes survivors into strangers."
    }
  ],
  "display_name": "What the Soldier Carried Home",
  "id": "quest_moral_chain_listen_08",
  "location_id": "loc_garrison_checkpoint_gamma",
  "max_day": 0,
  "min_day": 16,
  "trigger": "A former soldier at the military outpost has started talking in their sleep. The other refugees are frightened. You're asked to intervene."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_08.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
