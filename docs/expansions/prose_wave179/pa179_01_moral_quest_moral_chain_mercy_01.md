# Subject Plan 179.01 — The Open Hand: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_01` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 42 words / 228 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the material need and the limits on what help can accomplish visible together; let the scene carry its moral pressure without announcing the morally correct answer.

This is the Mercy branch record **The Open Hand**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_mercy_01` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** A group of refugees reaches your shelter gate with nothing but the clothes they fled in.

**Place:** `loc_shelter_gate` · **category:** `share` · **day window:** 15–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (42 words / 228 characters):**

> Three families — eleven people, four of them children — stand at the gate in the rain. They have nothing. No food, no tools, no names you recognize. The shelter is at capacity. The rations are counted. The rain is getting worse.

**Existing choices, frozen for context:** 1. Open the gate and share rations; 2. Offer temporary shelter for one night; 3. Give food but not entry; 4. Turn them away

Adjacent chain records: predecessor `none (chain opening)`; successor `quest_moral_chain_mercy_02`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the material need and the limits on what help can accomplish visible together; let the scene carry its moral pressure without announcing the morally correct answer. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "share",
  "choices": [
    {
      "empathy_delta": 2,
      "epitaph": "Opened the gate to eleven strangers in the rain. The rations stretched thinner. Nobody complained.",
      "label": "Open the gate and share rations",
      "moral_delta": 12,
      "outcome_text": "You open the gate. Eleven people file in, soaked, silent, grateful in a way that has no words. The rations stretch thinner. Nobody complains — not tonight. Tomorrow is tomorrow's arithmetic.",
      "set_flag": "flag_shared_rations"
    },
    {
      "empathy_delta": 1,
      "epitaph": "Gave them one night. They left before dawn and cleaned the floor behind them.",
      "label": "Offer temporary shelter for one night",
      "moral_delta": 6,
      "outcome_text": "One night. You make it clear. They nod, understanding the economy of mercy. By dawn they're gone, and the floor is cleaner than before they came."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Passed food through the gate. Kept the gate closed. The rain didn't stop.",
      "label": "Give food but not entry",
      "moral_delta": 3,
      "outcome_text": "You pass rations through the gate slot. Hands take them. Eyes meet yours through the bars. The rain keeps falling on their side."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Told them we were full. We weren't. The smallest child looked back once.",
      "label": "Turn them away",
      "moral_delta": -8,
      "outcome_text": "You tell them the shelter is full. It isn't — not quite. But the lie is easier than the math. They turn away, the children last, and the smallest one looks back once."
    }
  ],
  "display_name": "The Open Hand",
  "id": "quest_moral_chain_mercy_01",
  "location_id": "loc_shelter_gate",
  "max_day": 0,
  "min_day": 15,
  "trigger": "A group of refugees reaches your shelter gate with nothing but the clothes they fled in."
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
- Compare before/after JSON, proving only `quest_moral_chain_mercy_01.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
