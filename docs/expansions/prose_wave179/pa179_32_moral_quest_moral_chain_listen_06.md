# Subject Plan 179.32 — Signal in the Static: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_06` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 99 words / 567 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact.

This is the Listen branch record **Signal in the Static**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_06` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A coded message comes through the radio at the military outpost. The operator says it's been repeating for six days. Nobody can crack it.

**Place:** `loc_garrison_checkpoint_gamma` · **category:** `trust` · **day window:** 12–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (99 words / 567 characters):**

> The military outpost smells of rust and old batteries. The radio operator — a former signals technician, by their bearing — slides a sheet of paper across the table. Groups of five characters. Repeating every forty-seven seconds. 'It's not military cipher,' they say. 'I'd know those. This is something else. Personal.' They've been staring at it for six days. The pattern is there — you can feel it — but the key is missing. The operator pushes a second sheet toward you. 'I found this in the dead drop where the signal originates. It's a grocery list. From before.'

**Existing choices, frozen for context:** 1. Compare the grocery list to the cipher. Look for a personal key.; 2. Help the operator decode it systematically.; 3. "It's probably nothing. Just noise."

Adjacent chain records: predecessor `quest_moral_chain_listen_05`; successor `quest_moral_chain_listen_07`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "empathy_delta": 3,
      "epitaph": "Cracked a six-day cipher. It was a father's apology. Some messages aren't meant for armies.",
      "label": "Compare the grocery list to the cipher. Look for a personal key.",
      "moral_delta": 12,
      "outcome_text": "It takes an hour. The key is the grocery list — not the items, but the quantities. Each number maps to a letter shift. The decoded message is not tactical. It's a father telling his daughter where he buried her mother's jewelry box. Coordinates. A description of the oak tree. 'I'm sorry I couldn't save more,' the message ends. The operator reads it and puts their head in their hands. 'A father,' they whisper. 'All this time. A father.'"
    },
    {
      "empathy_delta": 2,
      "epitaph": "Spent three hours on a cipher. Found a love story. The war didn't erase everything.",
      "label": "Help the operator decode it systematically.",
      "moral_delta": 9,
      "outcome_text": "You work through it together — frequency analysis, pattern matching, dead ends. Three hours in, the operator finds the key: a personal memory encoded in the grocery list. The decoded message is coordinates and a name. A burial site. The operator stares at the decoded text. 'Someone loved someone enough to broadcast their location to the world,' they say. 'Even after the world ended.'"
    },
    {
      "empathy_delta": 0,
      "epitaph": "Called a message noise. The operator didn't forgive me. Neither would the father behind the cipher.",
      "label": "\"It's probably nothing. Just noise.\"",
      "moral_delta": -4,
      "outcome_text": "The operator's jaw tightens. 'Noise doesn't repeat every forty-seven seconds for six days.' They turn back to the radio. You've dismissed something that clearly matters to someone, and the operator will remember that. Not the dismissal — the fact that you didn't even try."
    }
  ],
  "display_name": "Signal in the Static",
  "id": "quest_moral_chain_listen_06",
  "location_id": "loc_garrison_checkpoint_gamma",
  "max_day": 0,
  "min_day": 12,
  "trigger": "A coded message comes through the radio at the military outpost. The operator says it's been repeating for six days. Nobody can crack it."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_06.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
