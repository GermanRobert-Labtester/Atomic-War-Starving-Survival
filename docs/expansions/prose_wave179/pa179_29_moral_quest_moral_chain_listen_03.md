# Subject Plan 179.29 — What the Water Knows: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_listen_03` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 81 words / 451 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Make room for what the speaker is trying to communicate and what remains unsaid. Preserve the difference between testimony, memory, signal, and verified fact. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits.

This is the Listen branch record **What the Water Knows**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_listen_03` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A child at the water station is drawing in the dirt with a stick. The drawing looks like a map — or a warning.

**Place:** `loc_water_station` · **category:** `listen` · **day window:** 5–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (81 words / 451 characters):**

> The child is maybe eight, maybe ten — hard to tell in the wasteland. They're drawing concentric circles in the dust, with lines radiating outward like a sun that fell to earth. When you crouch beside them, they don't stop. 'This is where the water goes,' they say, pointing to the center. 'And this is where it comes from. But nobody listens to the where.' They look up at you with the unflinching seriousness that only children and the dying possess.

**Existing choices, frozen for context:** 1. Ask them to explain the map. Take it seriously.; 2. Give them a real pencil and paper to draw it properly.; 3. Tell them to be careful — the dust might be contaminated.; 4. Children don't understand water tables. Walk on.

Adjacent chain records: predecessor `quest_moral_chain_listen_02`; successor `quest_moral_chain_listen_04`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

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
      "epitaph": "A child drew me a map of water. Adults had walked past it for weeks.",
      "label": "Ask them to explain the map. Take it seriously.",
      "moral_delta": 14,
      "outcome_text": "The child traces the lines with a dirty finger. The water comes from underground — they've been watching where the seepage appears after rain, marking it with stones no one else notices. It's not a child's game. It's observation, patient and methodical, from someone who's learned that adults stop looking at the ground. You kneel in the dirt and listen to a ten-year-old explain hydrology through drawings. The map is crude. The knowledge is real."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Gave a child a pencil. Got a map that might save lives. The world rewards the wrong people.",
      "label": "Give them a real pencil and paper to draw it properly.",
      "moral_delta": 9,
      "outcome_text": "Their eyes widen — not at the pencil, but at the paper. They draw carefully, labeling things in a child's uneven hand. The map that emerges is more detailed than the dirt version: three source points, a contamination zone marked with an X, and a path to clean water that no one at the station has found. They hand it to you like it's nothing. Like they didn't just solve a problem the adults gave up on."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Protected a child. Lost a map. Sometimes the safe choice costs more than the risky one.",
      "label": "Tell them to be careful — the dust might be contaminated.",
      "moral_delta": 2,
      "outcome_text": "The child looks at their hands, then back at the drawing. 'I know,' they say. 'But someone has to write it down.' They smooth over the drawing with their foot and walk away. The map is gone. Whatever they saw in the water's patterns is gone too. You protected their health and lost something you can't name."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Ignored a child. Ignored a map. The water kept flowing. I kept not knowing.",
      "label": "Children don't understand water tables. Walk on.",
      "moral_delta": -5,
      "outcome_text": "You walk past. The child doesn't watch you go — they're already drawing again, a new map, a new set of lines that another adult will ignore. The water station creaks and pumps. Somewhere beneath it, the water follows paths that a child understands and you don't."
    }
  ],
  "display_name": "What the Water Knows",
  "id": "quest_moral_chain_listen_03",
  "location_id": "loc_water_station",
  "max_day": 0,
  "min_day": 5,
  "trigger": "A child at the water station is drawing in the dirt with a stick. The drawing looks like a map — or a warning."
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
- Compare before/after JSON, proving only `quest_moral_chain_listen_03.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
