# Subject Plan 179.28 — Before the Ash: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_02` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 76 words / 424 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits.

This is the Listen branch record **Before the Ash**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_02` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** An old scavenger at a fire near the eastern road keeps glancing at you. When you approach, they gesture to the seat across the flames.

**Place:** `loc_eastern_road` · **category:** `listen` · **day window:** 3–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (76 words / 424 characters):**

> The fire is small — careful, almost. The scavenger sits cross-legged on a piece of cracked asphalt, a tin cup of something warm between their knees. They're old in the way the wasteland makes old: not wrinkles but erosion. When you sit, they don't offer the cup. Instead, they point at the sky. 'You know what color it was? Before?' They're not asking about the weather. They're asking if you remember a world that had blue.

**Existing choices, frozen for context:** 1. "Tell me about it."; 2. Share what you remember of the before, too.; 3. "I don't have time for nostalgia."

Adjacent chain records: predecessor `quest_moral_chain_listen_01`; successor `quest_moral_chain_listen_03`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "empathy_delta": 3,
      "epitaph": "An old teacher kept setting up chairs in a collapsed school. I understand that now. I think.",
      "label": "\"Tell me about it.\"",
      "moral_delta": 10,
      "outcome_text": "They talk for an hour. Not about the before — about the in-between. The weeks after the first exchange when everyone still thought it would end. They were a teacher. They kept showing up to the school building even after the roof collapsed, setting up chairs in the rubble, waiting for students who never came. 'The habit of it,' they say. 'That's what almost killed me. The habit of expecting things to work.' You listen to the fire. The cup goes cold. The story doesn't end — it just runs out of air, like everything else."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Traded memories by a fire. Left lighter and heavier at the same time.",
      "label": "Share what you remember of the before, too.",
      "moral_delta": 8,
      "outcome_text": "You trade memories like ration cards. They had a garden. You had... something. The exchange is uneven — theirs are vivid, yours are fading at the edges — but the act of sharing builds a bridge neither of you expected. They nod slowly when you finish. 'Good,' they say. 'Someone else carries it now. One more person.' The fire dies. They stand, stiffly, and walk east. You realize they were giving you something. Not a story. A responsibility."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Called remembering a waste of time. The old woman's pity was worse than any raider's threat.",
      "label": "\"I don't have time for nostalgia.\"",
      "moral_delta": -3,
      "outcome_text": "The scavenger's face doesn't change. They just pick up their cup, drink, and set it down. 'Nostalgia,' they repeat, as if tasting a word from a dead language. 'That's what the young ones call it.' They turn back to the fire. You've been dismissed — not with anger, but with something worse. Pity."
    }
  ],
  "display_name": "Before the Ash",
  "id": "quest_moral_chain_listen_02",
  "location_id": "loc_eastern_road",
  "max_day": 0,
  "min_day": 3,
  "trigger": "An old scavenger at a fire near the eastern road keeps glancing at you. When you approach, they gesture to the seat across the flames."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_02.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
