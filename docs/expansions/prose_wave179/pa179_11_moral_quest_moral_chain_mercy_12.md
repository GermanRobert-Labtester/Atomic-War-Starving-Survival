# Subject Plan 179.11 — The Plague Bearer: branching-quest discovery prose

Lane: A · Subject family: moral-choice branching content · Status: PROPOSAL (single-record, data-first prose subject; implementation route evidence included, not integration approval)

## Bounded outcome

Review one field—`discovery`—for selector `quest_moral_chain_mercy_12` in `Assets/StreamingAssets/Data/moral_choice_quests_branching.json`. Current depth: 49 words / 259 characters. The aim is to clarify the authored encounter that players see when this moral-choice quest is offered. The plan does not ask for a longer passage; retain the current text if a revision would add no useful information.

**Editorial question:** Keep the material need and the limits on what help can accomplish visible together; let the scene carry its moral pressure without announcing the morally correct answer. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch.

This is the Mercy branch record **The Plague Bearer**. The branch identity and actual situation are source facts; do not use this pass to rewrite the larger quest arc.

## Verified premise and source evidence

- **VERIFIED (2026-09-24):** `quest_moral_chain_mercy_12` is present exactly once in the current `schema_version: 1` `quests` array, which contains 100 entries.
- **VERIFIED:** the loader record shape is `id`, `display_name`, `category`, `trigger`, `discovery`, `location_id`, `min_day`, `max_day`, `choices`; the Core loader maps these to `MoralChoiceQuestDefinition` without changing the discovery text.
- **VERIFIED:** `Main.SetupMoralChoice` loads this branching catalog and appends its definitions to the same moral-choice catalog. `MoralChoiceModal.RefreshContent` renders `Trigger`, then `Discovery`, then the choice labels. This provides the existing data-only authoring route for the prose field.
- **VERIFIED:** the exact ID does not appear in the prior expansion indexes searched for this batch. Related-chain records are identified below for a semantic duplicate and continuity review.
- **MEASURED:** complete-catalog discovery text ranges from 42 to 143 whitespace-delimited words. Volume 56 of the local expansion authority measured the field shape and observed roughly 55 words in a head record; no plan-specific length target follows from that measurement.

**Trigger:** Your medic contracts the sickness from the quarantine patients. She asks you not to tell anyone.

**Place:** `loc_shelter_infirmary` · **category:** `comfort` · **day window:** 120–0 (0 max means unbounded in the current Core contract).

**Existing discovery prose (49 words / 259 characters):**

> She shows you her hands — the spots, unmistakable. 'I was careful,' she says. 'It wasn't enough.' She wants to keep working. Says she has weeks before it gets bad. Says the shelter needs her more than it needs the truth. Her eyes say: don't take this from me.

**Existing choices, frozen for context:** 1. Keep her secret and let her work; 2. Tell the shelter but advocate for her to keep working in isolation; 3. Quarantine her immediately — no exceptions; 4. Expel her — the risk is too high

Adjacent chain records: predecessor `quest_moral_chain_mercy_11`; successor `quest_moral_chain_mercy_13`. Read them in the live catalog before a revision; do not borrow a neighboring record’s reveal, character knowledge, or outcome.

## Editorial treatment

Keep the material need and the limits on what help can accomplish visible together; let the scene carry its moral pressure without announcing the morally correct answer. Where a choice concerns disclosure, do not reveal a fact earlier than the current discovery text permits. Where consequences involve punishment, keep the prose humane and specific; do not make violence decorative or pre-commit the branch. The discovery text should set the decision in motion, not answer it. Keep its relation to the separate `trigger` field clear: avoid restating the trigger verbatim when the current text already develops it, and avoid treating a UI heading as new diegetic speech. Use physical and social details supported by this record’s setting. Prefer one precise observation, action, or pressure point over a run of interchangeable bleak images.

Preserve point of view and information access. A character may know what they witnessed, remember, or were told; the narration must not know another person’s motive unless the record supports that knowledge. Keep allegations, uncertain identities, and disputed accounts explicitly uncertain. Maintain names, counts, timing, and causal relationships exactly as the live record states them. If a line needs a new fact to work, omit the line or route that separate idea to an independently evidenced subject.

Do not rewrite choices, outcomes, epitaphs, morality deltas, empathy deltas, flags, availability, or the consequence grammar. The prose may frame the decision but cannot imply a choice has already been made or that its outcome occurred. Avoid explaining the moral score: the Core contract says the score remains invisible, and the world and recorded consequences carry the meaning.

## Frozen data contract

Only this record’s top-level `discovery` string is in scope. Every other value is immutable. This exact JSON snapshot is the complete record with `discovery` removed; compare it against the live object immediately before any future edit:

```json
{
  "category": "comfort",
  "choices": [
    {
      "empathy_delta": 3,
      "epitaph": "Kept the medic's secret. She worked eighteen more days. The cost came due. She carries it.",
      "label": "Keep her secret and let her work",
      "moral_delta": 4,
      "outcome_text": "You say nothing. She works another eighteen days before the cough gives her away. By then, three people she treated have been exposed. Two survive. One doesn't. She survives. The cost lives in her face."
    },
    {
      "empathy_delta": 2,
      "epitaph": "Told the shelter the truth. Advocated for isolated work. She saved two more lives from behind a sheet wall.",
      "label": "Tell the shelter but advocate for her to keep working in isolation",
      "moral_delta": 10,
      "outcome_text": "You tell them. The fear flashes through camp like voltage. But you stand for her — isolated work, masked, limited contact. She saves two more lives from behind a sheet wall. The shelter learns that mercy has logistics."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Quarantined the medic by the book. Two people died of things she would have caught. She recovered. Doesn't look at me the same.",
      "label": "Quarantine her immediately — no exceptions",
      "moral_delta": 6,
      "outcome_text": "You quarantine her. By the book. She doesn't argue. The infirmary runs worse without her, and two people die of things she would have caught. She recovers. Returns. Doesn't quite look at you the same."
    },
    {
      "empathy_delta": 0,
      "epitaph": "Expelled the medic who caught the plague treating patients I brought in. The shelter is safer. And smaller.",
      "label": "Expel her — the risk is too high",
      "moral_delta": -10,
      "outcome_text": "You tell her to leave. She packs her kit in silence. The infirmary door closes behind her. She walks south. The shelter is safe. The shelter is also poorer, meaner, smaller."
    }
  ],
  "display_name": "The Plague Bearer",
  "id": "quest_moral_chain_mercy_12",
  "location_id": "loc_shelter_infirmary",
  "max_day": 0,
  "min_day": 120,
  "trigger": "Your medic contracts the sickness from the quarantine patients. She asks you not to tell anyone."
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
- Compare before/after JSON, proving only `quest_moral_chain_mercy_12.discovery` changed; assert all IDs, array order, gates, trigger/category/place, choice payloads, flags, outcomes, and score deltas are unchanged.
- Run the existing data-integrity and moral-choice catalog checks that own this file; report the focused command selected under the then-current `TEST_POLICY.md`.
- In a current route-level check, verify the record remains correctly mapped and that the modal presents trigger, revised discovery, and every unchanged choice label in that order. Do not alter or invent a branch result to demonstrate the prose.
- Review voice, clarity, duplicate imagery, canon facts, and word count as a descriptive measurement only. A retained-text disposition passes if no edit improves the scene.

## Disposition

Recommended scope: one selector, one prose field, one bounded review. Accept a revision only when it makes this encounter clearer or more particular without changing its decision space. The live discovery string can remain untouched. This proposal has an evidenced existing route and can inform a future data-only package; it is not a path claim, approval to edit, or a substitute for current foreman selection and ownership checks.
